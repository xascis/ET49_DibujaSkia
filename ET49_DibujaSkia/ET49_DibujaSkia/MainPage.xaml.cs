using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;

namespace ET49_DibujaSkia
{
	public partial class MainPage : ContentPage
	{
        const double tiempoDeCiclo = 1000; // ms
        Stopwatch temporizador = new Stopwatch();
        bool paginaActiva;
        float tick;
        SKPaint paintAnimation = new SKPaint
        {
            Style = SKPaintStyle.Stroke
        };

        public MainPage()
		{
			InitializeComponent();

            // medidor del tiempo
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            paginaActiva = true;
            temporizador.Start();

            Device.StartTimer(TimeSpan.FromMilliseconds(25), () =>
            {
                tick = (float)(temporizador.Elapsed.TotalMilliseconds % tiempoDeCiclo / tiempoDeCiclo);
                areaDibujo.InvalidateSurface();
                if (!paginaActiva)
                {
                    temporizador.Stop();
                }
                return paginaActiva;
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            paginaActiva = false;
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();


            // Ejemplo simple
            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Red.ToSKColor(),
                StrokeWidth = 25
            };
            canvas.DrawCircle(info.Width / 2, info.Height / 4, 100, paint);

            paint.Style = SKPaintStyle.Fill;
            paint.Color = SKColors.Blue;
            canvas.DrawCircle(info.Width / 2, (info.Height / 4), 50, paint);

            // Animación simple
            SKPoint center = new SKPoint(info.Width / 2, (info.Height / 3) * 2);
            float baseRadius = Math.Min(info.Width, info.Height) / 12;

            for (int circle = 0; circle < 5; circle++)
            {
                float radius = baseRadius * (circle + tick);

                paintAnimation.StrokeWidth = baseRadius / 2 * (circle == 0 ? tick : 1);
                paintAnimation.Color = new SKColor(0, 0, 255, (byte)(255 * (circle == 4 ? (1 - tick) : 1)));
                canvas.DrawCircle(center.X, center.Y, radius, paintAnimation);
            }
        }
	}
}
