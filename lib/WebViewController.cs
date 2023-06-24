using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpineViewer.lib
{
    internal class WebViewController
    {
        public WebView2 WebView;
        private CoreWebView2SharedBuffer SharedBuffer;
        private FrameManager FrameManager;
        public WebViewController(ref WebView2 webView)
        {
            WebView = webView;
            FrameManager = new FrameManager();
            InitializeWebView();
        }

        private async void InitializeWebView()
        {
            await WebView.EnsureCoreWebView2Async();
            WebView.WebMessageReceived += WebMessageReceived;
            // Set up local web app for the app
            WebView.CoreWebView2.SetVirtualHostNameToFolderMapping("spineviewer.assets", "Assets/web", Microsoft.Web.WebView2.Core.CoreWebView2HostResourceAccessKind.Allow);
            WebView.Source = new Uri("https://spineviewer.assets/index.html");
            // WebView.CoreWebView2.OpenDevToolsWindow();
        }
        public void PostWebMessage(CustomWebMessage message)
        {
            WebView.CoreWebView2.PostWebMessageAsJson(message.ToJSON());
        }

        private void InitializeBuffer()
        {
            int bufferSize = FrameManager.FrameWidth * FrameManager.FrameHeight * 4;
            SharedBuffer = WebView.CoreWebView2.Environment.CreateSharedBuffer((ulong)bufferSize);
            WebView.CoreWebView2.PostSharedBufferToScript(SharedBuffer, CoreWebView2SharedBufferAccess.ReadWrite, "");
        }

        private async void WebMessageReceived(WebView2 sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            var msg = CustomWebMessage.FromJSON(args.WebMessageAsJson);
            if (msg == null) return;
            switch (msg.Type)
            {
                case MessageType.BufferRequested:
                    {
                        var width = msg.Data.GetProperty("width").GetInt32();
                        var height = msg.Data.GetProperty("height").GetInt32();
                        FrameManager.SetSize(width, height);
                        InitializeBuffer();
                        break;
                    }
                case MessageType.FrameRendered:
                    {
                        var frameIndex = msg.Data.GetProperty("frame").GetInt32();
                        await FrameManager.SaveFrameFromWebViewBuffer(SharedBuffer, frameIndex);
                        PostWebMessage(new CustomWebMessage { Type = MessageType.ReadyForFrame });
                        break;
                    }
            }
        }
    }
}
