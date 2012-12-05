using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace ListBoxSandBox
{
    public class ControlHostAdornerBase : Adorner
    {
        private AdornerLayer _adornerLayer;
        protected Grid Host { get; set; }

        protected ControlHostAdornerBase(UIElement adornedElement) : base(adornedElement)
        {
            _adornerLayer = AdornerLayer.GetAdornerLayer(adornedElement);
            Host = new Grid();

            if(_adornerLayer != null)
            {
                _adornerLayer.Add(this);
            }
        }

        public void Detach()
        {
            _adornerLayer.Remove(this);
        }

        /// <summary>
        /// Override of VisualChildrenCount.
        /// Always return 1
        /// </summary>
        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Host.Measure(constraint);
            return base.MeasureOverride(constraint);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Host.Arrange(new Rect(finalSize));
            return base.ArrangeOverride(finalSize);
        }

        protected override Visual GetVisualChild(int index)
        {
            if (VisualChildrenCount <= index)
            {
                throw new IndexOutOfRangeException();
            }
            return Host;
        }
    }
}