#nullable enable
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace SpineViewer.lib
{
    internal class PickerCreator
    {
        static public async Task<StorageFile?> OpenFilePicker(Window window, List<string>? fileTypes = null)
        {
            var openPicker = new FileOpenPicker();

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            if (fileTypes != null)
            {
                fileTypes.ForEach(fileType => { openPicker.FileTypeFilter.Add(fileType); });
            } else
            {
                openPicker.FileTypeFilter.Add("*");
            }

            return await openPicker.PickSingleFileAsync();
        }
    }
}
