using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ListBoxSandBox
{
    public class DragContentAdorner : ControlHostAdornerBase
    {
        private readonly ContentPresenter _contentPresenter;

        public DragContentAdorner(UIElement adornedElement, Object draggedData, DataTemplate dataTemplate)
            : base(adornedElement)
        {
            _contentPresenter = new ContentPresenter
                                    {
                                        Content = draggedData,
                                        ContentTemplate = dataTemplate,
                                        Opacity = 0.7
                                    };

            Host.Children.Add(_contentPresenter);

           _contentPresenter.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Left);
           _contentPresenter.SetValue(VerticalAlignmentProperty, VerticalAlignment.Top);
        }
    }
}
