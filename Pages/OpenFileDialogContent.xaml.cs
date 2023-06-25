#nullable enable
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SpineViewer.lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SpineViewer.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OpenFileDialogContent : Page
    {
        private readonly List<string> SkeletonFileTypes = new() { ".skel" };
        private readonly List<string> AtlasFileTypes = new() { ".atlas" };
        public string? SpineVersion => ((ComboBoxItem)spineVersionCB.SelectedValue).Content.ToString();
        public StorageFile? SkeletonFile;
        public StorageFile? AtlasFile;
        private Window wnd;

        public OpenFileDialogContent(Window window)
        {
            wnd = window;
            this.InitializeComponent();
        }

        private async void OpenSkeletonClick(object sender, RoutedEventArgs e)
        {
            SkeletonFile = await PickerCreator.OpenFilePicker(wnd, SkeletonFileTypes);
        }

        private async void OpenAtlasClick(object sender, RoutedEventArgs e)
        {
            AtlasFile = await PickerCreator.OpenFilePicker(wnd, AtlasFileTypes);
        }
    }
}
