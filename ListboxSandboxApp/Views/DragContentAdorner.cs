using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ListBoxSandBox;

namespace ListboxSandboxApp.Views
{
    public class DragContentAdorner : ControlHostAdornerBase
    {
        private readonly ContentPresenter _contentPresenter;
        private TranslateTransform _translate;

        public DragContentAdorner(UIElement adornedElement, Object draggedData, DataTemplate dataTemplate)
            : base(adornedElement)
        {
            _contentPresenter = new ContentPresenter
                                    {
                                        Content = draggedData,
                                        ContentTemplate = dataTemplate,
                                        Opacity = 0.7
                                    };

            _translate = new TranslateTransform {X = 0, Y = 0};
            _contentPresenter.RenderTransform = _translate;

            Host.Children.Add(_contentPresenter);

           _contentPresenter.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Left);
           _contentPresenter.SetValue(VerticalAlignmentProperty, VerticalAlignment.Top);
        }

        public void SetScreenPosition(Point screenPosition, Point offset)
        {
            var positionInControl = base.AdornedElement.PointFromScreen(screenPosition);
            _translate.X = positionInControl.X - offset.X;
            _translate.Y = positionInControl.Y - offset.Y;
            base.AdornerLayer.Update();

        }
    }
}
