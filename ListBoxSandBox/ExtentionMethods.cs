using System.Windows;
using System.Windows.Controls;

namespace ListBoxSandBox
{
    public static class ExtentionMethods
    {
        public static object GetItemData(this ItemsControl itemsControl, DependencyObject item)
        {
            var data = itemsControl.GetItemContainer(item);
            return data == null ? null : data.DataContext;
        }

        public static int? GetItemIndex(this ItemsControl host, DependencyObject item)
        {
            if (host.Items.Contains(item))
            {
                return host.Items.IndexOf(item);
            }
            return null;
        }

        public static FrameworkElement GetItemContainer(this ItemsControl itemsControl, DependencyObject item)
        {
            if (itemsControl == null || item == null)
            {
                return null;
            }
            return itemsControl.ContainerFromElement(item) as FrameworkElement;
        }


        public static FrameworkElement GetLastContainer(this ItemsControl itemsControl)
        {
            return itemsControl.ItemContainerGenerator.ContainerFromIndex
                       (itemsControl.Items.Count - 1) as FrameworkElement;
        }
    }
}