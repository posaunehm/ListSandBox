using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace ListBoxSandBox
{
    public partial class MainWindow : Window
    {
        private object _draggedData;
        private Point? _initialPosition;
        private InsertionAdorner _insertionAdorner;
        private FrameworkElement _draggedContainer;
        private int? _draggedItemIndex;
        private FrameworkElement _draggedOveredContainer;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CleanUpData()
        {
            _initialPosition = null;
            _draggedData = null;
            if (_draggedOveredContainer != null && _insertionAdorner != null)
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(_draggedOveredContainer);

                if (adornerLayer != null)
                {
                    adornerLayer.Remove(_insertionAdorner);
                }
            }

            _insertionAdorner = null;
            _draggedItemIndex = null;
            _draggedContainer = null;
            _draggedOveredContainer = null;

        }

        private void ListBox_PreviewDrop(object sender, DragEventArgs e)
        {
            var itemsControl = sender as ItemsControl;
            var originalSource = e.OriginalSource as DependencyObject;
            var droppedItemIndex = itemsControl.GetItemIndex(originalSource);

            if (itemsControl == null)
            {
                return;
            }

            itemsControl.Items.Remove(_draggedData);
            if (droppedItemIndex != null)
            {
                if(droppedItemIndex > _draggedItemIndex)
                {
                    droppedItemIndex--;
                }
                itemsControl.Items.Insert((int) droppedItemIndex, _draggedData);
            }
            else
            {
                itemsControl.Items.Add(_draggedData);
            }
        }

        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _initialPosition = e.GetPosition(this);
            _draggedData = (sender as ItemsControl).GetItemData(e.OriginalSource as DependencyObject);
            _draggedContainer = (sender as ItemsControl).GetItemContainer(e.OriginalSource as DependencyObject);
            _draggedItemIndex = (sender as ItemsControl).GetItemIndex(e.OriginalSource as DependencyObject);
        }

        private void ListBox_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_initialPosition == null)
            {
                return;
            }
            var delta = (_initialPosition - e.GetPosition(this)).Value;

            if (Math.Abs(delta.X) > SystemParameters.MinimumHorizontalDragDistance
                && Math.Abs(delta.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                var senderObj = sender as DependencyObject;
                if (senderObj == null)
                {
                    return;
                }
                DragDrop.DoDragDrop(senderObj, _draggedData, DragDropEffects.Move);
                CleanUpData();
            }
        }

        private void ListBox_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            CleanUpData();
        }

        private void ListBox_PreviewDragEnter(object sender, DragEventArgs e)
        {
            var itemsControl = sender as ItemsControl;
            bool showInRight = false;
            if (itemsControl != null)
            {
                var temp = itemsControl.GetItemContainer(e.OriginalSource as DependencyObject);
                if(temp == null)
                {
                    _draggedOveredContainer = itemsControl.ItemContainerGenerator.ContainerFromIndex
                                              (itemsControl.Items.Count - 1) as FrameworkElement;
                    showInRight = true;
                }
                else
                {
                    _draggedOveredContainer = temp;
                }
            }
            var adornerLayer = AdornerLayer.GetAdornerLayer(_draggedOveredContainer);
            if (_draggedContainer != null)
            {
                _insertionAdorner = new InsertionAdorner(_draggedOveredContainer,showInRight);
                adornerLayer.Add(_insertionAdorner);
            }
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
    }
}