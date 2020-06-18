using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System;
using System.Diagnostics;
using System.Windows.Media.Animation;
using System.Threading;

namespace ProductionUI.CustomControls
{
    public class CarouselPanel : Panel
    { 
        private double _rWidth;
        private double _rHeight;
        private DoubleAnimation _animateX = new DoubleAnimation();

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
            
            double _bigWidth = _rWidth * 2;

            double center = finalSize.Width / 2;

            center -= _bigWidth;

            _animateX.Duration = new Duration(TimeSpan.FromSeconds(0.1));

            for (int leftCounter = selectedIndex  - 1; leftCounter > -1; leftCounter--)
            {
                int xval = selectedIndex - leftCounter;
                var left = center - (_rWidth * Math.Sqrt(xval));
                var finalRect = new Rect(left, top, _rWidth, _rHeight);
                Children[leftCounter].Arrange(finalRect);
                Panel.SetZIndex(Children[leftCounter], zIndex--);
                _animateX.From = Children[leftCounter].RenderSize.Width;
                _animateX.To = _rWidth;
                Children[leftCounter].BeginAnimation(WidthProperty, _animateX, HandoffBehavior.SnapshotAndReplace);
            }

            center += _bigWidth + _rWidth;

            for (int rightCounter = selectedIndex + 1; rightCounter < Children.Count; rightCounter++)
            {
                int xval = rightCounter - selectedIndex;
                var left = center + (_rWidth * Math.Sqrt(xval));

                var finalRect = new Rect(left, top, _rWidth, _rHeight);
                Children[rightCounter].Arrange(finalRect);
                Panel.SetZIndex(Children[rightCounter], zIndex--);
                _animateX.From = Children[rightCounter].RenderSize.Width;
                _animateX.To = _rWidth;
                Children[rightCounter].BeginAnimation(WidthProperty, _animateX, HandoffBehavior.SnapshotAndReplace);
            }

            center = finalSize.Width / 2;
            Children[selectedIndex].Arrange(new Rect(center - _bigWidth, top, _rWidth, _rHeight));
            Panel.SetZIndex(Children[selectedIndex], 1);
            _animateX.Duration = new Duration(TimeSpan.FromSeconds(0.7));
            _animateX.From = Children[selectedIndex].RenderSize.Width;
            _animateX.To = _bigWidth * 2;
            Children[selectedIndex].BeginAnimation(WidthProperty, _animateX, HandoffBehavior.SnapshotAndReplace);

            Children[selectedIndex].Arrange(new Rect(center - _bigWidth, top, _bigWidth * 2, _rHeight));

            return finalSize;
            //return base.ArrangeOverride(finalSize);
        }

        private ListBox GetListBox()
        {
            return VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(this))) as ListBox;
        }
    }
}

  