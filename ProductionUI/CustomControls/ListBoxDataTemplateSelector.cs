using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ProductionUI.CustomControls
{
    public class ListBoxDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ListItemFocusedWithImage { get; set; }
        public DataTemplate ListItemFocusedNoImage { get; set; }
        public DataTemplate ListItemRightOfFocus { get; set; }
        public DataTemplate ListItemLeftOfFocus { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if(item == null)
            {
                return null;
            }    
            var fard = container;
            var poop = item;

            return ListItemFocusedWithImage;

        }

    }
}
