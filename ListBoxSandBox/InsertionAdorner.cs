using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace ListBoxSandBox
{
    public class InsertionAdorner : Adorner
    {
        private UIElement _adornedElement;

        private readonly Grid _host;
        private readonly InsertionCursor _insertionCursor;

        public InsertionAdorner(UIElement adornedElement)
            : base(adornedElement)
        {


            _adornedElement = adornedElement;

            _host = new Grid();
            _insertionCursor = new InsertionCursor();

            _host.Children.Add(_insertionCursor);
        }

        /// <summary>
        /// Override of VisualChildrenCount.
        /// Always return 0
        /// </summary>
        protected override int VisualChildrenCount { get { return 1; } }

        /// <summary>
        /// Override of ArrangeOverride.
        /// Set host grid's size.
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            _host.Arrange(new Rect(finalSize));
            return base.ArrangeOverride(finalSize);
        }

        protected override Visual GetVisualChild(int index)
        {
            if (VisualChildrenCount <= index)
            {
                throw new IndexOutOfRangeException();
            }
            return _host;
        }
    }
}
