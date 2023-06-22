using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Web.WebView2.Core;
using SpineViewer.lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SpineViewer
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        CoreWebView2SharedBuffer sharedBuffer;
        public MainWindow()
        {
            this.InitializeComponent();

            if (MicaController.IsSupported())
                SystemBackdrop = new MicaBackdrop { Kind = MicaKind.BaseAlt };
            else
                SystemBackdrop = new DesktopAcrylicBackdrop();

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);

            InitializeWebView();
        }

        private void postWebMessage(CustomWebMessage message)
        {
            webView.CoreWebView2.PostWebMessageAsJson(message.ToJSON());
        }

        private async void openFilePicker(object sender)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

            picker.FileTypeFilter.Add(".skel");

            var file = await picker.PickSingleFileAsync();

            var dialog = DialogCreator.CreateTextDialog(this, "", "File info");

            if (file != null)
            {
                Console.WriteLine(file.Name);
                dialog.Content = $"File name: {file.Name}";
            } 
            else
            {
                Console.WriteLine("Operation cancelled.");
                dialog.Content = "Operation cancelled.";
            }

            await dialog.ShowAsync();
        }

        private void CommandBarButtonClick(object sender, RoutedEventArgs e)
        {
            AppBarButton btn = (AppBarButton)sender;
            switch(btn.Name)
            {
                case "openButton":
                    openFilePicker(sender);
                    postWebMessage(new CustomWebMessage(MessageType.OpenFile, null));
                    break;
            }
        }

        private async void InitializeWebView()
        {
            CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync();
            await webView.EnsureCoreWebView2Async();
            // Set up local web app for the app
            webView.CoreWebView2.SetVirtualHostNameToFolderMapping("spineviewer.assets", "Assets/web", Microsoft.Web.WebView2.Core.CoreWebView2HostResourceAccessKind.Allow);
            webView.Source = new Uri("https://spineviewer.assets/index.html");
            // webView.CoreWebView2.OpenDevToolsWindow();
            sharedBuffer = environment.CreateSharedBuffer(20);
        }
    }
}
