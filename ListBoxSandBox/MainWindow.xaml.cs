using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace ListBoxSandBox
{
    public partial class MainWindow : Window
    {
        private FrameworkElement _draggedContainer;
        private object _draggedData;
        private int? _draggedItemIndex;
        private FrameworkElement _draggedOveredContainer;
        private Point? _initialPosition;
        private InsertionAdorner _insertionAdorner;
        private DragContentAdorner _dragContentAdorner;
        private AdornerLayer _dragImageAdornerLayer;
        private FrameworkElement _draggedImageSource;

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
            if (_draggedOveredContainer != null && _insertionAdorner != null)
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(_draggedOveredContainer);

                if (adornerLayer != null) { adornerLayer.Remove(_insertionAdorner); }
            }

            if (_dragImageAdornerLayer != null && _dragContentAdorner != null)
            {_dragImageAdornerLayer.Remove(_dragContentAdorner);}

            _insertionAdorner = null;
            _draggedItemIndex = null;
            _draggedContainer = null;
            _draggedOveredContainer = null;
        }

        private void CreateInsertionAdorner(DependencyObject draggedItem, ItemsControl itemsControl)
        {
            _draggedOveredContainer = itemsControl.GetItemContainer(draggedItem);
            bool showInRight = false;
            if (_draggedOveredContainer == null)
            {
                _draggedOveredContainer = itemsControl.GetLastContainer();
                showInRight = true;
            }

            _insertionAdorner = new InsertionAdorner(_draggedOveredContainer, showInRight);
        }

        private void DropItemAt(int? droppedItemIndex, ItemsControl itemsControl)
        {
            itemsControl.Items.Remove(_draggedData);
            if (droppedItemIndex != null)
            {
                if (droppedItemIndex > _draggedItemIndex)
                {
                    droppedItemIndex--;
                }
                itemsControl.Items.Insert((int)droppedItemIndex, _draggedData);
            }
            else
            {
                itemsControl.Items.Add(_draggedData);
            }
        }

        private void ListBox_PreviewDragEnter(object sender, DragEventArgs e)
        {
            var itemsControl = sender as ItemsControl;
            if (itemsControl == null) { return; }

            CreateInsertionAdorner(e.OriginalSource as DependencyObject, itemsControl);
            AdornerLayer.GetAdornerLayer(_draggedOveredContainer).Add(_insertionAdorner);
        }

        private void ListBox_PreviewDragLeave(object sender, DragEventArgs e)
        {
            if (_draggedOveredContainer != null)
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(_draggedOveredContainer);
                if (_insertionAdorner != null)
                {
                    adornerLayer.Remove(_insertionAdorner);
                    _insertionAdorner = null;
                }
            }
        }

        private void ListBox_PreviewDragOver(object sender, DragEventArgs e)
        {
        }

        private void ListBox_PreviewDrop(object sender, DragEventArgs e)
        {
            var itemsControl = sender as ItemsControl;
            if (itemsControl == null) { return; }

            DropItemAt(itemsControl.GetItemIndex(e.OriginalSource as DependencyObject), itemsControl);
        }

        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _initialPosition = e.GetPosition(this);
            _draggedData = (sender as ItemsControl).GetItemData(e.OriginalSource as DependencyObject);
            _draggedImageSource = e.OriginalSource as FrameworkElement;
            _draggedContainer = (sender as ItemsControl).GetItemContainer(e.OriginalSource as DependencyObject);
            _draggedItemIndex = (sender as ItemsControl).GetItemIndex(e.OriginalSource as DependencyObject);
        }

        private void ListBox_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_initialPosition == null) { return; }

            if (!MovedEnoughForDrag((_initialPosition - e.GetPosition(this)).Value)) { return; }

            var senderObj = sender as UIElement;
            if (senderObj == null) { return; }

            _dragContentAdorner = new DragContentAdorner(senderObj, _draggedData, null);
            _dragImageAdornerLayer = AdornerLayer.GetAdornerLayer(senderObj);
            if(_dragImageAdornerLayer != null)
            {
                _dragImageAdornerLayer.Add(_dragContentAdorner);
            }

            DragDrop.DoDragDrop(senderObj, _draggedData, DragDropEffects.Move);
            CleanUpData();
        }

        private void ListBox_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            CleanUpData();
        }
    }
}