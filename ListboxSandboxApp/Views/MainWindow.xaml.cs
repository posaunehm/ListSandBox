using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ListBoxSandBox;

namespace ListboxSandboxApp.Views
{
    public partial class MainWindow : Window
    {
        private object _draggedData;
        private int? _draggedItemIndex;
        private Point? _initialPosition;
        private InsertionAdorner _insertionAdorner;
        private DragContentAdorner _dragContentAdorner;
        private Point _mouseGripOffset;

        public MainWindow()
        {
            InitializeComponent();
        }

        private static bool MovedEnoughForDrag(Vector delta)
        {
            return Math.Abs(delta.X) > SystemParameters.MinimumHorizontalDragDistance
                   && Math.Abs(delta.Y) > SystemParameters.MinimumVerticalDragDistance;
        }

        private void CleanUpData()
        {
            _initialPosition = null;
            _draggedData = null;

            if (_insertionAdorner != null) {_insertionAdorner.Detach();}

            if (_dragContentAdorner != null) {_dragContentAdorner.Detach();}

            _insertionAdorner = null;
            _draggedItemIndex = null;
        }

        private void CreateInsertionAdorner(DependencyObject draggedItem, ItemsControl itemsControl)
        {
            var draggedOveredContainer = itemsControl.GetItemContainer(draggedItem);
            bool showInRight = false;
            if (draggedOveredContainer == null)
            {
                draggedOveredContainer = itemsControl.GetLastContainer();
                showInRight = true;
            }

            _insertionAdorner = new InsertionAdorner(draggedOveredContainer, showInRight);
        }

        private void DropItemAt(int? droppedItemIndex, ItemsControl itemsControl)
        {
            itemsControl.RemoveItem(_draggedData);

            if (droppedItemIndex != null)
            {
                droppedItemIndex -= droppedItemIndex > _draggedItemIndex ? 1 : 0;
                itemsControl.InsertItemAt((int)droppedItemIndex, _draggedData);
            }
            else
            {
                itemsControl.AddItem(_draggedData);
            }
        }

        private void ListBox_PreviewDragEnter(object sender, DragEventArgs e)
        {
            var itemsControl = sender as ItemsControl;
            if (itemsControl == null) { return; }

            CreateInsertionAdorner(e.OriginalSource as DependencyObject, itemsControl);
        }

        private void ListBox_PreviewDragLeave(object sender, DragEventArgs e)
        {
            if (_insertionAdorner != null)
            {
                _insertionAdorner.Detach();
                _insertionAdorner = null;
            }
        }

        private void ListBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            var currentPos = this.PointToScreen(e.GetPosition(this));
            _dragContentAdorner.SetScreenPosition(currentPos, _mouseGripOffset);
        }

        private void ListBox_PreviewDrop(object sender, DragEventArgs e)
        {
            var itemsControl = sender as ItemsControl;
            if (itemsControl == null) { return; }

            var dropTargetData = itemsControl.GetItemData(e.OriginalSource as DependencyObject);
            DropItemAt(itemsControl.GetItemIndex(dropTargetData), itemsControl);
        }

        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var itemsControl = sender as ItemsControl;
            if (itemsControl == null){ return;}
            _initialPosition = this.PointToScreen(e.GetPosition(this));
            _mouseGripOffset = CalculateMouseGripOffset(e.OriginalSource as DependencyObject, itemsControl);
            _draggedData = itemsControl.GetItemData(e.OriginalSource as DependencyObject);
            _draggedItemIndex = itemsControl.GetItemIndex(_draggedData);
        }

        private Point CalculateMouseGripOffset(DependencyObject dependencyObject, ItemsControl itemsControl)
        {
            var draggedOveredContainer = itemsControl.GetItemContainer(dependencyObject);
            if (draggedOveredContainer == null)
            { return new Point(); }
            var pointFromScreen = draggedOveredContainer.PointFromScreen(_initialPosition.Value);
            return pointFromScreen;
        }

        private void ListBox_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_initialPosition == null) { return; }

            var currentPos = this.PointToScreen( e.GetPosition(this));
            if (!MovedEnoughForDrag((_initialPosition - currentPos).Value)) { return; }

            var itemsControl = sender as ItemsControl;
            if (itemsControl == null) { return; }

            _dragContentAdorner = new DragContentAdorner(
                itemsControl, _draggedData, itemsControl.ItemTemplate);
            _dragContentAdorner.SetScreenPosition(currentPos, _mouseGripOffset);

            if (_draggedData != null)
            {
                DragDrop.DoDragDrop(itemsControl, _draggedData, DragDropEffects.Move);
            }
            CleanUpData();
        }

        private void ListBox_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            CleanUpData();
        }
    }
}