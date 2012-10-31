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
        private int? _draggedItemIndex;
        private Point? _initialPosition;

        public MainWindow()
        {
            InitializeComponent();
        }

        private static object GetDraggedData(ItemsControl itemsControl, DependencyObject visual)
        {
            if (itemsControl == null || visual == null)
            {
                return null;
            }
            var data = itemsControl.ContainerFromElement(visual) as FrameworkElement;
            return data == null ? null : data.DataContext;
        }

        private static int? GetItemIndex(ItemsControl host, DependencyObject item)
        {
            if (host.Items.Contains(item))
            {
                return host.Items.IndexOf(item);
            }
            return null;
        }

        private void CleanUpData()
        {
            _initialPosition = null;
            _draggedData = null;
            _draggedItemIndex = null;
        }

        private void ListBox_PreviewDrop(object sender, DragEventArgs e)
        {
            var itemsControl = sender as ItemsControl;
            var originalSource = e.OriginalSource as DependencyObject;
            var droppedItemIndex = GetItemIndex(itemsControl, originalSource);

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
            _draggedData = GetDraggedData(sender as ItemsControl, e.OriginalSource as DependencyObject);
            _draggedItemIndex = GetItemIndex(sender as ItemsControl, e.OriginalSource as DependencyObject);
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
                Debug.WriteLine("Drag end");
                CleanUpData();
            }
        }

        private void ListBox_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            CleanUpData();
        }
    }
}