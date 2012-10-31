using System.Windows;
using System.Windows.Controls;

namespace ListBoxSandBox
{
    public static class ExtentionMethods
    {
        public static object GetDraggedData(this ItemsControl itemsControl, DependencyObject visual)
        {
            if (itemsControl == null || visual == null)
            {
                return null;
            }
            var data = itemsControl.ContainerFromElement(visual) as FrameworkElement;
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
    }
}