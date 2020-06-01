using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using ProductionUI.ViewModels;
using ProductionUI.Models;

namespace ProductionUI.CustomControls
{
    public class CarouselPanel : Panel
    {
        private bool _eventHooked = false;

        private double _rWidth = 5 * 45;
        private double _rHeight = 3 * 45;

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

            foreach (UIElement child in Children)
            {
                ///_rWidth = child.DesiredSize.Width;
                ///_rHeight = child.DesiredSize.Height;

                child.Measure(new Size(_rWidth, _rHeight));
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
            double top = (finalSize.Height / 2) + (_rHeight / 2);
            int zIndex = 10000;


            double center = finalSize.Width / 2;
            center -= ((_rWidth * 2) /3) ;

            for (int leftCounter = 0; leftCounter < selectedIndex; leftCounter++)
            {
                var left = center - ((selectedIndex - leftCounter) * (_rWidth * .25)) - (_rWidth/2);
                var finalRect = new Rect(left, top, _rWidth, _rHeight);
                Children[leftCounter].Arrange(finalRect);
                CarouselPanel.SetAngle(Children[leftCounter], -15);
                Panel.SetZIndex(Children[leftCounter], zIndex);
                zIndex++;
            }

            center += (_rWidth / 3);

            for (int rightCounter = Children.Count - 1; rightCounter > selectedIndex; rightCounter--)
            {
                var left = center + ((rightCounter - selectedIndex) * (_rWidth * .25)) + (_rWidth / 2);
                var finalRect = new Rect(left, top, _rWidth, _rHeight);
                Children[rightCounter].Arrange(finalRect);
                CarouselPanel.SetAngle(Children[rightCounter], 15);
                Panel.SetZIndex(Children[rightCounter], zIndex);
                zIndex++;
            }

            Children[selectedIndex].Arrange(new Rect(center - (_rWidth / 3), top - (_rHeight * 1.5), _rWidth*1.5, _rHeight*1.5));
            CarouselPanel.SetAngle(Children[selectedIndex], 0);
            Panel.SetZIndex(Children[selectedIndex], 69000);


            ProductionCardDeckViewModel.SelectedIndex.IsActiveStep = false;
            ProductionCardDeckViewModel.SelectedIndex = (ProductionCardModel)listBox.SelectedItem;
            ProductionCardDeckViewModel.SelectedIndex.IsActiveStep = true;
            

            return base.ArrangeOverride(finalSize);
        }

        private ListBox GetListBox()
        {
            return VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(this))) as ListBox;
        }

        public int Angle
        {
            get 
            {
                return (int)GetValue(AngleProperty); 
            }
            set 
            { 
                SetValue(AngleProperty, value);
            }
        }

        public static readonly DependencyProperty AngleProperty = DependencyProperty.RegisterAttached("Angle", typeof(int), typeof(CarouselPanel), new UIPropertyMetadata(0));
        public static void SetAngle(DependencyObject target, int value) 
        { 
            target.SetValue(CarouselPanel.AngleProperty, value);
        }

        public static int GetAngle(DependencyObject target) 
        { 
            return (int)target.GetValue(CarouselPanel.AngleProperty); 
        }
    }
}

  