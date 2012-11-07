using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ListBoxSandBox
{
    /// <summary>
    ///   MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private object _draggedData;
        private Point? _initialPosition;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CleanUpData()
        {
            _initialPosition = null;
            _draggedData = null;
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
            _draggedData = (sender as ItemsControl).GetDraggedData(e.OriginalSource as DependencyObject);
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
            var container = (sender as ItemsControl).GetDraggedContainer(e.OriginalSource as DependencyObject);
            Debug.WriteLine("Enter: " + container);
        }

        private void ListBox_PreviewDragLeave(object sender, DragEventArgs e)
        {
            var container = (sender as ItemsControl).GetDraggedContainer(e.OriginalSource as DependencyObject);
            Debug.WriteLine("Leave: " + container);
        }

        private void ListBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            
        }
    }
}