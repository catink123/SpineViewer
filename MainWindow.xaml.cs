using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Web.WebView2.Core;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SpineViewer.lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Image = SixLabors.ImageSharp.Image;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SpineViewer
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private WebViewController WebViewController;

        public MainWindow()
        {
            this.InitializeComponent();

            if (MicaController.IsSupported())
                SystemBackdrop = new MicaBackdrop { Kind = MicaKind.BaseAlt };
            else
                SystemBackdrop = new DesktopAcrylicBackdrop();

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            WebViewController = new(ref webView);
        }

        private void CommandBarButtonClick(object sender, RoutedEventArgs e)
        {
            AppBarButton btn = (AppBarButton)sender;
            switch(btn.Name)
            {
                case "openButton":
                    WebViewController.PostWebMessage(new CustomWebMessage { Type = MessageType.OpenFile, Data = null });
                    break;
            }
        }
    }
}
