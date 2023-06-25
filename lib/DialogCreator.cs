using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpineViewer.lib
{
    internal class DialogCreator
    {
        public static ContentDialog CreateDialog(Window window, dynamic message, string title = "Message")
        {
            ContentDialog dialog = new()
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK",
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                XamlRoot = window.Content.XamlRoot
            };
            return dialog;
        }
    }
}
