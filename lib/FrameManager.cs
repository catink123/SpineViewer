using Microsoft.Web.WebView2.Core;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace SpineViewer.lib
{
    internal class FrameManager
    {
        public int FrameWidth = 1;
        public int FrameHeight = 1;

        public FrameManager() { }

        public async Task SaveFrameFromWebViewBuffer(CoreWebView2SharedBuffer buffer, int frameIndex) {
            using var stream = buffer.OpenStream();
            using var reader = new DataReader(stream);

            byte[] bytes = new byte[buffer.Size];
            await reader.LoadAsync((uint)buffer.Size);
            reader.ReadBytes(bytes);

            using var img = Image.LoadPixelData<Rgba32>(bytes, FrameWidth, FrameHeight);

            string saveDir = Path.GetTempPath() + "SpineViewer";
            if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);
            await img.SaveAsPngAsync(saveDir + $"\\frame{frameIndex}.png");
        }

        public void SetSize(int width, int height)
        {
            FrameWidth = width;
            FrameHeight = height;
        }
    }
}
