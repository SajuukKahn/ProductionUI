using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System;
using System.Diagnostics;
using System.Windows.Media.Animation;
using System.Threading;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;

namespace ProductionUI.CustomControls
{
    public class CarouselPanel : Panel
    { 
        private double _rWidth;
        private double _rHeight;
        private DoubleAnimation _animateWidth = new DoubleAnimation();
        private DoubleAnimation _animateTransformX = new DoubleAnimation();

        private bool _eventHooked;

        protected override Size MeasureOverride(Size availableSize)
        {
            if (!_eventHooked)
            {
                Debug.WriteLine("event hooked up");
                var listBox = GetListBox();
                if (listBox != null)
                {
                    listBox.SelectionChanged += (sender, e) => InvalidateArrange();
                    _eventHooked = true;
                }
            }

            double _minHeight = 1;
            double _minWidth = 1;
            double _absoluteMinWidth = Double.MaxValue;
            double _absoluteMinHeight = Double.MaxValue;

            foreach (UIElement child in Children)
            {

                child.Measure(availableSize);

                if (!(_minWidth.CompareTo(child.DesiredSize.Width) > 0))
                {
                    _minWidth = child.DesiredSize.Width;
                }
                if (!(_minHeight.CompareTo(child.DesiredSize.Height) > 0))
                {
                    _minHeight = child.DesiredSize.Height;
                }
                if (_minHeight < _absoluteMinHeight)
                {
                    _absoluteMinHeight = _minHeight;
                }
                if (_minWidth < _absoluteMinWidth)
                {
                    _absoluteMinWidth = _minWidth;
                }
            }
                
            _rWidth = Math.Ceiling(availableSize.Width / 9);
            
            if(_rWidth < 25)
            {
                _rWidth = 25;
            }

            _rHeight = Math.Ceiling(availableSize.Height - 10);
            if(_rHeight < 40)
            {
                _rHeight = 40;
            }

            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var listBox = GetListBox();
            if (listBox == null) return base.ArrangeOverride(finalSize);
            if (listBox.SelectedIndex < 0) listBox.SelectedIndex = 0;

            int selectedIndex = listBox.SelectedIndex;

            double top = (finalSize.Height / 2) - (_rHeight / 2);
            int zIndex = 10000;
            
            double _bigWidth = Math.Ceiling(_rWidth * 2);

            double center = Math.Ceiling(finalSize.Width / 2);

            center -= _bigWidth;

            _animateTransformX.Duration = new Duration(TimeSpan.FromMilliseconds(50));
            _animateWidth.Duration = _animateTransformX.Duration;
            _animateWidth.FillBehavior = FillBehavior.HoldEnd;
            _animateWidth.EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseOut };

            for (int leftCounter = selectedIndex  - 1; leftCounter > -1; leftCounter--)
            {
                int xval = selectedIndex - leftCounter;
                var left = Math.Ceiling(center - (_rWidth * Math.Sqrt(xval)));
                var finalRect = new Rect(left, top, _rWidth, _rHeight);
                Panel.SetZIndex(Children[leftCounter], zIndex--);

                Children[leftCounter].Arrange(finalRect);
                _animateWidth.From = Children[leftCounter].RenderSize.Width;
                _animateWidth.To = _rWidth;
                Children[leftCounter].BeginAnimation(WidthProperty, _animateWidth, HandoffBehavior.SnapshotAndReplace);
                
            }

            center += _bigWidth + _rWidth;

            for (int rightCounter = selectedIndex + 1; rightCounter < Children.Count; rightCounter++)
            {
                int xval = rightCounter - selectedIndex;
                var left = Math.Ceiling(center + (_rWidth * Math.Sqrt(xval)));
                var finalRect = new Rect(left, top, _rWidth, _rHeight);
                Panel.SetZIndex(Children[rightCounter], zIndex--);

                Children[rightCounter].Arrange(finalRect);
                _animateWidth.From = Children[rightCounter].RenderSize.Width;
                _animateWidth.To = _rWidth;
                Children[rightCounter].BeginAnimation(WidthProperty, _animateWidth, HandoffBehavior.SnapshotAndReplace);

            }

            center = Math.Ceiling(finalSize.Width / 2);
            Children[selectedIndex].Arrange(new Rect(center - _bigWidth, top, _rWidth, _rHeight));
            Panel.SetZIndex(Children[selectedIndex], 1);
            _animateWidth.From = Children[selectedIndex].RenderSize.Width;
            _animateWidth.To = _bigWidth * 2;

            Children[selectedIndex].Arrange(new Rect(center - _bigWidth, top, _bigWidth * 2, _rHeight));
            Children[selectedIndex].BeginAnimation(WidthProperty, _animateWidth, HandoffBehavior.SnapshotAndReplace);


            return finalSize;
            //return base.ArrangeOverride(finalSize);
        }

        private ListBox GetListBox()
        {
            return VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(this))) as ListBox;
        }
    }
}

  