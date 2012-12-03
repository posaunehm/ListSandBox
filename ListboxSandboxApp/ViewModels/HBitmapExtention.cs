using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace ListboxSandboxApp.ViewModels
{
    public static class HBitmapExtention
    {
        /// <summary>
        /// ref: http://nine-works.blog.ocn.ne.jp/blog/2011/12/post_b0e8.html
        /// </summary>
        /// <param name="hBitmapSource">hBitmap</param>
        /// <returns>BitmapSource</returns>
        public static BitmapSource ToBitmapSource(this IntPtr hBitmapSource)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(
                hBitmapSource,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
                );
        }
    }
}