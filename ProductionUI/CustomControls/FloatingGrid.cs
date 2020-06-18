using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ProductionUI.CustomControls
{
    class FloatingGrid : Grid
    {
        private readonly TranslateTransform _translation = new TranslateTransform();
        private readonly Storyboard _animation = new Storyboard();
        private readonly DoubleAnimation _animateX;
        private double _lastGoingToX = 0;

        public FloatingGrid()
        {
            LayoutUpdated += This_LayoutUpdated;
            RenderTransform = _translation;

            _animateX = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(.2)));
            Storyboard.SetTargetProperty(_animateX, new PropertyPath("(0).(1)", new[] { RenderTransformProperty, TranslateTransform.XProperty }));
            SineEase sineEase = new SineEase();
            sineEase.EasingMode = EasingMode.EaseOut;
            _animateX.EasingFunction = sineEase;
            _animation.Children.Add(_animateX);

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
            //_animation.Stop(this);

            var panel = FindPanel(this);
            if (panel == null) return; 

            var comingFromX = _lastGoingToX + _translation.X;

            var transform = (Transform)TransformToAncestor(panel);
            var goingToX = transform.Value.OffsetX - _translation.X;
            _animateX.From = comingFromX - goingToX;
            _lastGoingToX = goingToX;
            _animation.Begin(this, HandoffBehavior.SnapshotAndReplace, true);


        }
    }
}
