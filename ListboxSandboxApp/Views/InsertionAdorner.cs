using System.Windows;
using ListBoxSandBox;

namespace ListboxSandboxApp.Views
{
    public class InsertionAdorner : ControlHostAdornerBase
    {
        private readonly InsertionCursor _insertionCursor;

        public InsertionAdorner(UIElement adornedElement, bool showInRightSide = false)
            : base(adornedElement)
        {
            _insertionCursor = new InsertionCursor();

            Host.Children.Add(_insertionCursor);

            _insertionCursor.SetValue(HorizontalAlignmentProperty,
                showInRightSide? HorizontalAlignment.Right : HorizontalAlignment.Left);
            _insertionCursor.SetValue(VerticalAlignmentProperty,VerticalAlignment.Stretch);
        }
    }
}
