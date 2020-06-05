using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using ProductionUI.ViewModels;
using ProductionUI.Models;
using System;

namespace ProductionUI.CustomControls
{
    public class CarouselPanel : Panel
    {
        private bool _eventHooked = false;

        private double _rWidth;
        private double _rHeight;

        protected override Size MeasureOverride(Size availableSize)
        {
            if (!_eventHooked)
            {
                var listBox = GetListBox();
                if (listBox != null)
                {
                    listBox.SelectionChanged += (sender, e) => InvalidateArrange();
                    _eventHooked = true;
                }
            }

            double _minHeight = 1;
            double _minWidth = 1;

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
                
                if (_rWidth < _minWidth)
                {
                    _rWidth = _minWidth;
                }
                if (_rHeight < _minHeight)
                {
                    _rHeight = _minHeight;
                }

            }
            base.MeasureOverride(availableSize);
            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var listBox = GetListBox();
            if (listBox == null) return base.ArrangeOverride(finalSize);
            if (listBox.SelectedIndex < 0) listBox.SelectedIndex = 0;

            int selectedIndex = listBox.SelectedIndex;
            double top = (finalSize.Height / 2) - _rHeight;
            int zIndex = 10000;


            double center = finalSize.Width / 2;
            center -= ((_rWidth * 2) /3) ;

            Children[selectedIndex].Arrange(new Rect(center - (_rWidth / 3), top, _rWidth * 2, _rHeight * 2));
            Panel.SetZIndex(Children[selectedIndex], 69000);

            ProductionCardDeckViewModel.SelectedIndex.IsActiveStep = false;
            ProductionCardDeckViewModel.SelectedIndex = (ProductionCardModel)listBox.SelectedItem;
            ProductionCardDeckViewModel.SelectedIndex.IsActiveStep = true;

            for (int leftCounter = 0; leftCounter < selectedIndex; leftCounter++)
            {
                var left = (center - ((selectedIndex - leftCounter) / 10 * (_rWidth * _rWidth)) - (_rWidth));
                //var left2 = center - (_rWidth + (_rWidth / (2 - (Math.Exp(1-leftCounter))));
                var left2 = center - (center/2 * (1 - (Math.Exp(0- (selectedIndex - leftCounter)))));
                var magic = (2 / Math.Sqrt(selectedIndex - leftCounter));
                var finalRect = new Rect(left2, top, _rWidth * magic , _rHeight* magic);
                Children[leftCounter].Arrange(finalRect);
                Panel.SetZIndex(Children[leftCounter], zIndex);
                zIndex++;
            }

            center -= (_rWidth / 3);

            for (int rightCounter = Children.Count - 1; rightCounter > selectedIndex; rightCounter--)
            {
                var left = (center + ((rightCounter - selectedIndex) / 10 * (_rWidth * _rWidth)) + (_rWidth));
                //var left2 = center + (_rWidth + (_rWidth / (2 - Math.Exp(1-rightCounter))));

                var left2 = center + (center/2 * (1 - (Math.Exp(0 - (rightCounter - selectedIndex)))));
                var magic = (2 / Math.Sqrt(rightCounter - selectedIndex));
                var finalRect = new Rect(left2, top, _rWidth * magic, _rHeight * magic);
                Children[rightCounter].Arrange(finalRect);
                Panel.SetZIndex(Children[rightCounter], zIndex);
                zIndex++;
            }

            return base.ArrangeOverride(finalSize);
        }

        private ListBox GetListBox()
        {
            return VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(this))) as ListBox;
        }
    }
}

  