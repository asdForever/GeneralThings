using System;
using System.Windows;
using System.Windows.Media;

namespace myth.Classes
{
    public static class GlobalMethods
    {
        public static void GetChildList(UIElement parent, Type targetType, ref System.Collections.Generic.List<UIElement> children)
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    UIElement child = (UIElement)VisualTreeHelper.GetChild(parent, i);
                    if (child.GetType() == targetType)
                    {
                        children.Add(child);
                    }
                    GetChildList(child, targetType, ref children);
                }
            }
        }
        public static T FindChildOfType<T>(DependencyObject root) where T : class
        {
            var queue = new System.Collections.Generic.Queue<DependencyObject>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                DependencyObject current = queue.Dequeue();
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(current); i++)
                {
                    var child = VisualTreeHelper.GetChild(current, i);
                    var typedChild = child as T;
                    if (typedChild != null)
                    {
                        return typedChild;
                    }
                    queue.Enqueue(child);
                }
            }
            return null;
        }
        public static T FindParentOfType<T>(DependencyObject element) where T : DependencyObject
        {
            T result = null;
            DependencyObject currentElement = element;
            while (currentElement != null)
            {
                result = currentElement as T;
                if (result != null)
                {
                    break;
                }
                currentElement = VisualTreeHelper.GetParent(currentElement);
            }
            return result;
        }
        public static SolidColorBrush GetColorFromHexa(string hexaColor)
        {
            return new SolidColorBrush(
                Color.FromArgb(
                    0xFF,
                    Convert.ToByte(hexaColor.Substring(1, 2), 16),
                    Convert.ToByte(hexaColor.Substring(3, 2), 16),
                    Convert.ToByte(hexaColor.Substring(5, 2), 16)
                )
            );
        }
        public static void SetImageSource(System.Windows.Controls.Image img, string imgSource)
        {
            var bitmapImg = new System.Windows.Media.Imaging.BitmapImage();
            bitmapImg.UriSource = new Uri(imgSource, UriKind.Relative);
            System.Windows.Media.Imaging.BitmapImage bi = new System.Windows.Media.Imaging.BitmapImage();
            bi.CreateOptions = System.Windows.Media.Imaging.BitmapCreateOptions.BackgroundCreation;
            bi.UriSource = bitmapImg.UriSource;
            img.Source = bi;
        }
    }
}
