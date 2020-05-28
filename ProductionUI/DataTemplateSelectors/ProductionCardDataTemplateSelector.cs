using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ProductionUI.DataTemplateSelectors
{
    public class ProductionCardDataTemplateSelector : DataTemplateSelector
    {
        public PropertyChangedEventHandler PropertyChanged { get; private set; }

        public DataTemplate ActiveCard { get; set; }
        public DataTemplate QueuedCard { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            PropertyChangedEventHandler lambda = null;
            lambda = (o, args) =>
            {
                if (args.PropertyName == "some property")
                {
                    this.PropertyChanged -= lambda;
                    var cp = (ContentPresenter)container;
                    cp.ContentTemplateSelector = null;
                    cp.ContentTemplateSelector = this;
                }
            };
            this.PropertyChanged += lambda;

            return QueuedCard;
        }
    }
}
