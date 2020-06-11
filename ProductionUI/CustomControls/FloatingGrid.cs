using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ProductionUI.CustomControls
{
    class FloatingGrid : Grid
    {
        private readonly TranslateTransform _translation = new TranslateTransform();
        private readonly ScaleTransform _scale = new ScaleTransform();
        private readonly Storyboard _animation = new Storyboard();
        private readonly DoubleAnimation _animateX;
        private readonly DoubleAnimation _animateY;
        private readonly DoubleAnimation _scaleX;
        private readonly DoubleAnimation _scaleY;
        private double _lastGoingToX = 0;
        private double _lastGoingToY = 0;

        private static bool _inAnimation = false;

        public FloatingGrid()
        {
            LayoutUpdated += This_LayoutUpdated;
            RenderTransform = _translation;

            VisualTransform = _scale;

            _animateX = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(.1)));
            Storyboard.SetTargetProperty(_animateX, new PropertyPath("(0).(1)", new[] { RenderTransformProperty, TranslateTransform.XProperty }));
            _animation.Children.Add(_animateX);

            _animateY = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(.1)));
            Storyboard.SetTargetProperty(_animateY, new PropertyPath("(0).(1)", new[] { RenderTransformProperty, TranslateTransform.YProperty }));
            _animation.Children.Add(_animateY);

            _scaleX = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(.5)));
            Storyboard.SetTargetProperty(_scaleX, new PropertyPath("(0).(1)", new[] { RenderTransformProperty, ScaleTransform.ScaleXProperty }));
            _animation.Children.Add(_scaleX);

            _scaleY = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(.5)));
            Storyboard.SetTargetProperty(_scaleY, new PropertyPath("(0).(1)", new[] { RenderTransformProperty, ScaleTransform.ScaleYProperty }));
            _animation.Children.Add(_scaleY);
        }

        private Visual FindPanel(Visual visual)
        {
            var item = visual as FloatingGrid;
            if (visual is Panel panel && item == null) return panel;
            if (visual != null)
            {
                var parent = (Visual)VisualTreeHelper.GetParent(visual);
                return FindPanel(parent);
            }
            return null;
        }

        private void This_LayoutUpdated(object sender, EventArgs e)
        {
            if (_inAnimation == false)
            {
                _inAnimation = true;
                AnimateLayout();
                _inAnimation = false;
            }
        }

        private void AnimateLayout()
        {

            _animation.Stop(this);

            var panel = FindPanel(this);
            if (panel == null) return;

            var comingFromX = _lastGoingToX + _translation.X;
            var comingFromY = _lastGoingToY + _translation.Y;

            var scalingX = panel;

            var transform = (Transform)TransformToAncestor(panel);
            var goingToX = transform.Value.OffsetX - _translation.X;
            _animateX.From = comingFromX - goingToX;
            _lastGoingToX = goingToX;
            var goingToY = transform.Value.OffsetY - _translation.Y;
            _animateY.From = comingFromY - goingToY;
            _lastGoingToY = goingToY;

            _animation.Begin(this, HandoffBehavior.SnapshotAndReplace, true);
        }
    }
}
