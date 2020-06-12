using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using ProductionUI.ViewModels;
using ProductionUI.Models;
using System;
using System.Windows.Documents;
using System.Collections.ObjectModel;
using Microsoft.VisualBasic;
using System.Linq;
using System.Collections;
using System.Diagnostics;

namespace ProductionUI.CustomControls
{
    public class CarouselPanel : Panel
    {
        //private bool _eventHooked = false;

        private double _rWidth;
        private double _rHeight;

        protected override Size MeasureOverride(Size availableSize)
        {
            //if (!_eventHooked)
            //{
            //    Debug.WriteLine("event hooked up");
            //    var listBox = GetListBox();
            //    if (listBox != null)
            //    {
            //        listBox.SelectionChanged += (sender, e) => InvalidateArrange();
            //        _eventHooked = true;
            //    }
            //}

            // double _minHeight = 1;
            // double _minWidth = 1;
            // double _absoluteMinWidth = Double.MaxValue;
            // double _absoluteMinHeight = Double.MaxValue;
            // 
            // foreach (UIElement child in Children)
            // {
            // 
            //     //child.Measure(availableSize);
            // 
            //     if (!(_minWidth.CompareTo(child.DesiredSize.Width) > 0))
            //     {
            //         _minWidth = child.DesiredSize.Width;
            //     }
            //     if (!(_minHeight.CompareTo(child.DesiredSize.Height) > 0))
            //     {
            //         _minHeight = child.DesiredSize.Height;
            //     }
            //     if(_minHeight < _absoluteMinHeight)
            //     {
            //         _absoluteMinHeight = _minHeight;
            //     }
            //     if(_minWidth < _absoluteMinWidth)
            //     {
            //         _absoluteMinWidth = _minWidth;
            //     }
            // }

            _rWidth = Math.Ceiling(availableSize.Width / 9) - 10;
            
            if(_rWidth < 25)
            {
                _rWidth = 25;
            }

            _rHeight = Math.Ceiling(availableSize.Height) - 10;
            if(_rHeight < 150)
            {
                _rHeight = 150;
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

            Children[selectedIndex].Arrange(new Rect(center - _bigWidth, top, _bigWidth * 2, _rHeight));
            Panel.SetZIndex(Children[selectedIndex], 69000);

            center -= _bigWidth;

            for (int leftCounter = selectedIndex  - 1; leftCounter > -1; leftCounter--)
            {
                int xval = selectedIndex - leftCounter;
                var left = center - (_rWidth * Math.Sqrt(xval));
                var finalRect = new Rect(left, top, _rWidth, _rHeight);
                Children[leftCounter].Arrange(finalRect);
                Panel.SetZIndex(Children[leftCounter], zIndex--);
            }

            center += _bigWidth + _rWidth;

            for (int rightCounter = selectedIndex + 1; rightCounter < Children.Count; rightCounter++)
            {
                //var left = center + (((rightCounter - selectedIndex) / 10) * (_rWidth * _rWidth)) + _rWidth;
                //var left2 = center + (_rWidth + (_rWidth / (2 - Math.Exp(1-rightCounter))));
                int xval = rightCounter - selectedIndex;
                var left = center + (_rWidth * Math.Sqrt(xval));

                //var left2 = center + ((center / 2) * (1 - (Math.Exp(0 - (rightCounter - selectedIndex)))));

                //var magic = 2 / Math.Sqrt(xval);
                var finalRect = new Rect(left, top, _rWidth, _rHeight);
                Children[rightCounter].Arrange(finalRect);
                Panel.SetZIndex(Children[rightCounter], zIndex--);
            }

            return finalSize;
            //return base.ArrangeOverride(finalSize);
        }

        private ListBox GetListBox()
        {
            return VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(this))) as ListBox;
        }
    }
}

  