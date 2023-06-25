using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using SpineViewer.lib;
using SpineViewer.Pages;
using System;
using Windows.Storage;

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

        private async void CommandBarButtonClick(object sender, RoutedEventArgs e)
        {
            AppBarButton btn = (AppBarButton)sender;
            switch(btn.Name)
            {
                case "openButton":
                    {
                        var dialogContent = new OpenFileDialogContent(this);
                        var dialog = DialogCreator.CreateDialog(this, dialogContent, "Open Files");
                        dialog.PrimaryButtonText = "Open";
                        dialog.CloseButtonText = "Cancel";
                        dialog.DefaultButton = ContentDialogButton.Primary;
                        if (await dialog.ShowAsync() is ContentDialogResult.Primary)
                        {
                            var version = dialogContent.SpineVersion;
                            var skelFile = dialogContent.SkeletonFile;
                            var atlasFile = dialogContent.AtlasFile;
                            if (skelFile == null || atlasFile == null)
                            {
                                await DialogCreator.CreateDialog(this, "Invalid Skeleton or Atlas file specified!", "Error").ShowAsync();
                                return;
                            }

                            StorageFile textureFile;
                            try
                            {
                                textureFile = await StorageFile.GetFileFromPathAsync(atlasFile.Path.Replace(atlasFile.FileType, ".png"));
                            }
                            catch(Exception ex)
                            {
                                await DialogCreator.CreateDialog(this, $"Unable to access the Texture file!\nDetails: {ex.Message}", "Error").ShowAsync();
                                return;
                            }

                            await DialogCreator.CreateDialog(this, $"Selected file paths:\n{skelFile.Path}\n{atlasFile.Path}\n{textureFile.Path}\n\nSelected Spine Version: {version}").ShowAsync();
                            // WebViewController.PostWebMessage(new CustomWebMessage { Type = MessageType.OpenFile, Data = new OpenFileProperties { Version = version } });
                        }
                        break;
                    }
            }
        }
    }
}
