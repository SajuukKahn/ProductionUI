using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System;
using System.Diagnostics;
using System.Windows.Media.Animation;
using System.Threading;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media.Effects;

namespace ProductionUI.CustomControls
{
    public class VerticalCarouselPanel : Panel
    {
        private double _rWidth;
        private double _rHeight;

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

            _rWidth = Math.Ceiling(availableSize.Width - 10);

            if (_rWidth < 125)
            {
                _rWidth = 125;
            }

            _rHeight = Math.Ceiling(availableSize.Height / 3);
            if (_rHeight < 25)
            {
                _rHeight = 25;
            }

            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var listBox = GetListBox();
            if (listBox == null) return base.ArrangeOverride(finalSize);
            if (listBox.SelectedIndex < 0) listBox.SelectedIndex = 0;
            if (listBox.SelectedIndex < 0) return base.ArrangeOverride(finalSize);

            int selectedIndex = listBox.SelectedIndex;

            int zIndex = 0;

            double center = Math.Ceiling(finalSize.Height / 2);

            center -= (_rHeight / 2);

            for (int topCounter = selectedIndex - 1; topCounter > -1; topCounter--)
            {
                int xval = selectedIndex - topCounter;
                var top = Math.Ceiling(center - (_rHeight * Math.Sqrt(xval)));
                var finalRect = new Rect(0, top, _rWidth, _rHeight);
                Panel.SetZIndex(Children[topCounter], zIndex--);

                Children[topCounter].Arrange(finalRect);
                Children[topCounter].Effect = new BlurEffect()
                {
                    KernelType = KernelType.Gaussian,
                    Radius = xval * 5
                };
            }

            //center += (_rHeight  / 2);

            for (int bottomCounter = selectedIndex + 1; bottomCounter < Children.Count; bottomCounter++)
            {
                int xval = bottomCounter - selectedIndex;
                var top = Math.Ceiling(center + (_rHeight * Math.Sqrt(xval)));
                var finalRect = new Rect(0, top, _rWidth, _rHeight);
                Panel.SetZIndex(Children[bottomCounter], zIndex--);

                Children[bottomCounter].Arrange(finalRect);
                Children[bottomCounter].Effect = new BlurEffect()
                {
                    KernelType = KernelType.Gaussian,
                    Radius = xval * 5
        };
            }

            center = Math.Ceiling(finalSize.Height / 2);
            Panel.SetZIndex(Children[selectedIndex], 1);
            Children[selectedIndex].Arrange(new Rect(0, center - (_rHeight / 2), _rWidth, _rHeight));
            Children[selectedIndex].Effect = new BlurEffect()
            {
                KernelType = KernelType.Gaussian,
                Radius = 0
            };
            return finalSize;
            //return base.ArrangeOverride(finalSize);
        }

        private ListBox GetListBox()
        {

            return VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(this))) as ListBox; ;
        }
    }
}

