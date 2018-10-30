using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FormsButton
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        SKBitmap resourceBitmap;

        protected override void OnAppearing()
        {
            //string resourceID = "FormsButton.Resources.button_g_normal.png";
            string resourceID = "FormsButton.Resources.dl013_button_default.9.png";
            Assembly assembly = GetType().GetTypeInfo().Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(resourceID))
            {
                resourceBitmap = SKBitmap.Decode(stream);
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            System.Diagnostics.Trace.WriteLine($"Button_Clicked {sender}");
        }

        private void SKCanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            System.Diagnostics.Trace.WriteLine($"SKCanvasView_PaintSurface {e.Info.Width}/{e.Info.Height} {resourceBitmap?.Width}/{resourceBitmap?.Height}");

            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            if (resourceBitmap != null)
            {
                //canvas.DrawBitmap(resourceBitmap, new SKRect(0, 0, e.Info.Width, e.Info.Height));

                System.Diagnostics.Debug.WriteLine($"外周を切り取る");
                var croppedWidth = resourceBitmap.Width - 2;
                var croppedHeight = resourceBitmap.Height - 2;
                var croppedBitmap = new SKBitmap(croppedWidth, croppedHeight);
                var croppedDst = new SKRect(0, 0, croppedWidth, croppedHeight);
                var croppedSrc = new SKRect(1, 1, resourceBitmap.Width - 1, resourceBitmap.Height - 1);
                using (var croppedCanvas = new SKCanvas(croppedBitmap))
                {
                    croppedCanvas.DrawBitmap(resourceBitmap, croppedSrc, croppedDst);
                }

                System.Diagnostics.Debug.WriteLine($"中央のX座標を調べる");
                var centerXs = new List<int>();
                for (int i = 0; i < resourceBitmap.Width; i++)
                {
                    var color = resourceBitmap.GetPixel(i, 0);
                    System.Diagnostics.Debug.WriteLine($"{i} {color.Alpha} {color.Red} {color.Green} {color.Blue} {color.Alpha != 0}");
                    if (i != 0 && color.Alpha != 0)
                    {
                        centerXs.Add(i - 1);
                    }
                }

                System.Diagnostics.Debug.WriteLine($"中央のY座標を調べる");
                var centerYs = new List<int>();
                for (int i = 0; i < resourceBitmap.Height; i++)
                {
                    var color = resourceBitmap.GetPixel(0, i);
                    System.Diagnostics.Debug.WriteLine($"{i} {color.Alpha} {color.Red} {color.Green} {color.Blue} {color.Alpha != 0}");
                    if (i != 0 && color.Alpha != 0)
                    {
                        centerYs.Add(i - 1);
                    }
                }

                System.Diagnostics.Debug.WriteLine($"中央の座標を決定する");
                int left = (centerXs.Count > 0) ? centerXs.Min() : 0;
                int top = (centerYs.Count > 0) ? centerYs.Min() : 0;
                int right = (centerXs.Count > 0) ? centerXs.Max() : 0;
                int bottom = (centerYs.Count > 0) ? centerYs.Max() : 0;
                System.Diagnostics.Debug.WriteLine($"{left} {top} {right} {bottom}");

                // 調整
                if (left == right)
                {
                    right = left + 1;
                }
                if (top == bottom)
                {
                    bottom = top + 1;
                }

                System.Diagnostics.Debug.WriteLine($"切り取った画像を貼る");
                var dst = new SKRect(0, 0, e.Info.Width, e.Info.Height);
                var center = new SKRectI(left, top, right, bottom);
                canvas.DrawBitmapNinePatch(croppedBitmap, center, dst);
            }

        }
    }
}
