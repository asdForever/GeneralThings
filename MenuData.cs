using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace myth.Classes
{
    /// <summary>
    /// This class is used to create a menu (application is a collection of panoramas with 2 elements: the menu and the main window).
    /// </summary>
    public static class MenuData
    {
        #region left menu item
        public static void addMenuItems(ListBox listBoxMenu)
        {
            listBoxMenu.Items.Clear();

            Thickness marginForImage = new Thickness(10, 10, 10, 10);
            Thickness marginForTextBlock = new Thickness(10, 0, 0, 0);

            listBoxMenu.Items.Add(getStackPanel(marginForImage, marginForTextBlock, "/myth;component/img/menu/1.png", "news"));
            listBoxMenu.Items.Add(getStackPanel(marginForImage, marginForTextBlock, "/myth;component/img/menu/2.png", "contracts"));
            listBoxMenu.Items.Add(getStackPanel(marginForImage, marginForTextBlock, "/myth;component/img/menu/3.png", "payments"));
            listBoxMenu.Items.Add(getStackPanel(marginForImage, marginForTextBlock, "/myth;component/img/menu/4.png", "companies directory"));
            listBoxMenu.Items.Add(getStackPanel(marginForImage, marginForTextBlock, "/myth;component/img/menu/5.png", "option 5"));

            listBoxMenu.SelectionChanged += new SelectionChangedEventHandler(listBoxMenu_SelectionChanged);

            marginForImage = new Thickness();
            marginForTextBlock = new Thickness();
        }
        private static void setMenuItemsManipulationStartedAndCompletedHandlers(ListBox listBoxMenu)
        {
            List<UIElement> items = new List<UIElement>();
            var bw = new System.ComponentModel.BackgroundWorker();
            bw.DoWork += (s, args) => // This runs on a background thread.
            {
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    GlobalMethods.GetChildList(listBoxMenu, typeof(StackPanel), ref items);
                });
            };
            bw.RunWorkerCompleted += (s, args) =>
            {
                foreach (StackPanel item in items)
                {
                    item.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(tappedMenuItem_ManipulationStarted);
                    item.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(tappedMenuItem_ManipulationCompleted);
                }
            };
            bw.RunWorkerAsync();
        }
        private static void tappedMenuItem_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            StackPanel sp = (StackPanel)sender;
            if (sp != null)
            {
                sp.Background = GlobalMethods.GetColorFromHexa("#E7E7E7");
            }
        }
        private static void tappedMenuItem_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            StackPanel sp = (StackPanel)sender;
            if (sp != null)
            {
                sp.Background = GlobalMethods.GetColorFromHexa("#B4B4B4");
            }
        }
        private static void listBoxMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBoxMenu = (ListBox)sender;
            if (listBoxMenu != null)
            {
                // If selected index is -1 (no selection) do nothing
                if (listBoxMenu.SelectedIndex == -1)
                    return;
                string uri = "";
                switch (listBoxMenu.SelectedIndex)
                {
                    case (0):
                        uri = "/Views/NewsPanoramaPage.xaml";
                        break;
                    case (1):
                        uri = "/Views/ContractsPanoramaPage.xaml";
                        break;
                    case (2):
                        uri = "/Views/PaymentsPanoramaPage.xaml";
                        break;
                    case (3):
                        uri = "/Views/CompaniesDirectoryPage.xaml";
                        break;
                    case (4):
                        uri = "/Views/NewsPanoramaPage.xaml";
                        break;
                }
                if (uri != "")
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        (Application.Current.RootVisual as Microsoft.Phone.Controls.PhoneApplicationFrame).Navigate(new Uri(uri, UriKind.Relative));
                    });
                // Reset selected index to -1 (no selection)
                listBoxMenu.SelectedIndex = -1;
            }
        }
        #endregion
        #region header
        public static void addHeader(Grid LayoutRoot)
        {
            Grid grid = new Grid();
            grid.Background = GlobalMethods.GetColorFromHexa("#000000");
            grid.Height = 75;
            grid.VerticalAlignment = VerticalAlignment.Top;

            Image imageLogo = getImage(50, 50, new Thickness(25, 5, 0, 0), "/img/topMenu/img_logo_.png");
            Image imageBox = getImage(50, 50, new Thickness(140, 5, 0, 0), "/img/topMenu/img_box_act_.png");
            Image imageLet = getImage(50, 50, new Thickness(215, 5, 0, 0), "/img/topMenu/icon_let_act_-.png");
            Image imageTalk = getImage(50, 50, new Thickness(290, 5, 0, 0), "/img/topMenu/img_talk_act_.png");
            Image imageTelega = getImage(50, 50, new Thickness(405, 5, 0, 0), "/img/topMenu/telega_act_.png");

            grid.Children.Add(imageLogo);
            grid.Children.Add(imageBox);
            grid.Children.Add(imageLet);
            grid.Children.Add(imageTalk);
            grid.Children.Add(imageTelega);
            LayoutRoot.Children.Add(grid);
        }
        private static void setImageSource(Image sender)
        {
            string imgSource = sender.Source.ToString();
            if (imgSource.Contains("_act_"))
            {
                imgSource = imgSource.Replace("_act_", "_dis_");
            }
            else if (imgSource.Contains("_dis_"))
            {
                imgSource = imgSource.Replace("_dis_", "_act_");
            }
            GlobalMethods.SetImageSource(sender, imgSource);
        }
        private static void Image_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            Image img = (Image)sender;
            if (img != null)
            {
                setImageSource(img);
            }
        }
        private static void Image_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            Image img = (Image)sender;
            if (img != null)
            {
                setImageSource(img);
            }
        }
        #endregion

        private static Image getImage(int height, int width, Thickness margin, string imgSource)
        {
            Image img = new Image();
            img.Height = height;
            img.Width = width;
            img.Margin = margin;
            img.HorizontalAlignment = HorizontalAlignment.Left;
            img.VerticalAlignment = VerticalAlignment.Center;
            ImageBrush ib = new ImageBrush();
            BitmapImage bi = new BitmapImage(new Uri(imgSource, UriKind.Relative));
            bi.CreateOptions = BitmapCreateOptions.DelayCreation;
            ib.ImageSource = bi;
            img.Source = ib.ImageSource;
            img.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(Image_ManipulationStarted);
            img.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(Image_ManipulationCompleted);

            return img;
        }
        private static TextBlock getTextBlock(Thickness margin, string text)
        {
            TextBlock tb = new TextBlock();
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.FontSize = 40;
            tb.Margin = margin;
            tb.Foreground = GlobalMethods.GetColorFromHexa("#000000");
            tb.Text = text;

            return tb;
        }
        private static StackPanel getStackPanel(Thickness marginImage, Thickness marginTextBlock, string imgSource, string optionName)
        {
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.Width = 420;

            sp.Children.Add(getImage(60, 60, marginImage, imgSource));
            sp.Children.Add(getTextBlock(marginTextBlock, optionName));

            sp.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(tappedMenuItem_ManipulationStarted);
            sp.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(tappedMenuItem_ManipulationCompleted);

            return sp;
        }

        public static void getTopMenuTemplate(Grid LayoutRoot, Grid ContentGrid)
        {
            ImageBrush ib = new ImageBrush();
            BitmapImage bi = new BitmapImage(new Uri("/myth;component/img/panorama/simpleBack.png", UriKind.Relative));
            bi.CreateOptions = BitmapCreateOptions.DelayCreation;
            ib.ImageSource = bi;
            LayoutRoot.Background = ib;

            ContentGrid.Margin = new Thickness(0, 70, 0, 0);

            ImageBrush ib1 = new ImageBrush();
            BitmapImage bi1 = new BitmapImage(new Uri("/img/logo/logo5.png", UriKind.Relative));
            bi1.CreateOptions = BitmapCreateOptions.DelayCreation;
            ib1.ImageSource = bi1;
            Image imgLogo = new Image();
            imgLogo.Height = 65;
            imgLogo.VerticalAlignment = VerticalAlignment.Top;
            imgLogo.HorizontalAlignment = HorizontalAlignment.Left;
            imgLogo.Margin = new Thickness(10, 5, 0, 0);
            imgLogo.Source = ib1.ImageSource;

            Grid grid = new Grid();
            grid.VerticalAlignment = VerticalAlignment.Top;
            grid.HorizontalAlignment = HorizontalAlignment.Right;
            grid.Margin = new Thickness(0, 5, 10, 0);
            grid.Height = 64;
            grid.HorizontalAlignment = HorizontalAlignment.Right;

            //TextBlock textBlockTitle = new TextBlock();
            //textBlockTitle.Name = "textBlockTitle";
            //textBlockTitle.FontSize = 17.5;
            //textBlockTitle.Width = 250;
            //textBlockTitle.Margin = new Thickness(0, 0, 200, 0);
            //textBlockTitle.VerticalAlignment = VerticalAlignment.Center;
            //textBlockTitle.Text = GlobalVariables._categoriesList.GetItem(Convert.ToInt32(GlobalVariables.contractType)).categoryName.ToUpper();

            Button button1 = new Button();
            button1.Height = 64;
            button1.Width = 64;
            button1.BorderThickness = new Thickness(0);
            button1.Margin = new Thickness(150, 0, 0, 0);
            ImageBrush image1 = new ImageBrush();
            image1.ImageSource = new BitmapImage(new Uri("/myth;component/img/menu/1.png", UriKind.Relative));
            button1.Background = image1;

            Button button2 = new Button();
            button2.Height = 64;
            button2.Width = 64;
            button2.BorderThickness = new Thickness(0);
            button2.Margin = new Thickness(275, 0, 0, 0);
            ImageBrush image2 = new ImageBrush();
            image2.ImageSource = new BitmapImage(new Uri("/myth;component/img/menu/2.png", UriKind.Relative));
            button2.Background = image2;

            Button button3 = new Button();
            button3.Height = 64;
            button3.Width = 64;
            button3.BorderThickness = new Thickness(0);
            button3.Margin = new Thickness(400, 0, 0, 0);
            ImageBrush image3 = new ImageBrush();
            image3.ImageSource = new BitmapImage(new Uri("/myth;component/img/menu/3.png", UriKind.Relative));
            button3.Background = image3;

            //grid.Children.Add(textBlockTitle);
            grid.Children.Add(button1);
            grid.Children.Add(button2);
            grid.Children.Add(button3);
            LayoutRoot.Children.Add(imgLogo);
            LayoutRoot.Children.Add(grid);
        }
    }
}
