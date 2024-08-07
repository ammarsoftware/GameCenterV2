using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Storage;
using GameCenterV2.Models;
using GameCenterV2.ViewModels;
using SkiaSharp;
using SkiaSharp.HarfBuzz;


using System.Drawing.Printing;
using System.Text.Json;

namespace GameCenterV2.Services
{
    public static class ReceiptPrintService
    {
        private static async Task<SKTypeface> GetTypefaceFromAppPackageAsync(string fontFileName)
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(fontFileName);
            using var skStream = new SKManagedStream(stream);
            return SKTypeface.FromStream(skStream);
        }
        public static async Task PrintReceipt(List<TbService> orderItems, double total)
        {
            var surface = SKSurface.Create(new SKImageInfo(300, 500 + (orderItems.Count * 20)));

            var canvas = surface.Canvas;

            using (var paint = new SKPaint())
            {
                // رسم خلفية بيضاء
                canvas.DrawColor(SKColors.White);
                // تحميل خط يدعم العربية
                var typeface = await GetTypefaceFromAppPackageAsync("times.ttf");
                paint.Typeface = typeface;
                paint.IsAntialias = true;
                // إعداد HarfBuzzShaper لدعم تشكيل النص العربي
                var shaper = new SKShaper(typeface);

                // رسم اللوغو
                var logo = SKBitmap.Decode("dotnet_bot.png");
                if (logo != null)
                {
                    canvas.DrawBitmap(logo, new SKRect(0, 0, 300, 100));
                }

                // رسم العنوان
                paint.Color = SKColors.Black;
                paint.TextSize = 24;
                paint.IsAntialias = true;
                var title = "فاتورة الطعام";
                var titleWidth = paint.MeasureText(title);
                canvas.DrawShapedText(shaper, title, 150 - titleWidth / 2, 110, paint);
                // رسم تفاصيل الطلب
                paint.TextSize = 14;
                float y = 60;
                foreach (var item in orderItems)
                {
                    var itemName = item.SeI?.IName ?? "غير معروف";
                    var quantity = $"{item.SeQty}x";
                    var price = $"{item.SePrice:F2}";

                    canvas.DrawShapedText(shaper, itemName, 200, y, paint);
                    canvas.DrawShapedText(shaper, quantity, 150, y, paint);
                    canvas.DrawText(price, 10, y, paint);

                    paint.Color = SKColors.Gray;
                    canvas.DrawLine(10, y + 5, 290, y + 5, paint);
                    paint.Color = SKColors.Black;

                    y += 20;
                }

                // رسم المبلغ الإجمالي
                y += 20;
                paint.FakeBoldText = true;
                var totalText = "المبلغ الإجمالي:";
                var totalAmount = $"{total:F2}";
                //canvas.DrawShapedText(shaper, totalText, 290 - paint.MeasureText(totalText), y, paint);
                //canvas.DrawText(totalAmount, 10, y, paint);

                paint.TextSize = 18;
                canvas.DrawShapedText(shaper, totalText, 200, y, paint);
                canvas.DrawText(totalAmount, 10, y, paint);



                // رسم عنوان المطعم
                paint.FakeBoldText = false;
                paint.TextSize = 12;
                var address = "عنوان المطعم: شارع الرئيسي، المدينة";
                var addressWidth = paint.MeasureText(address);
                canvas.DrawShapedText(shaper, address, 150 - addressWidth / 2, 480, paint);
            }
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                await ShareReceiptOnAndroid(surface);
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                PrintReceiptOnWindows(surface);
            }
        }

        private static async Task ShareReceiptOnAndroid(SKSurface surface)
        {
            var image = surface.Snapshot();
            var data = image.Encode(SKEncodedImageFormat.Png, 100);

            using (var stream = new MemoryStream(data.ToArray()))
            {
                var filename = $"receipt_{DateTime.Now:yyyyMMddHHmmss}.png";
                var fileSaverResult = await FileSaver.Default.SaveAsync(filename, stream, CancellationToken.None);

                if (fileSaverResult.IsSuccessful)
                {
                    await Share.Default.RequestAsync(new ShareFileRequest
                    {
                        Title = "فاتورة الطعام",
                        File = new ShareFile(fileSaverResult.FilePath)
                    });
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("خطأ", "فشل في حفظ الفاتورة", "موافق");
                }
            }
        }

        private static void PrintReceiptOnWindows(SKSurface surface)
        {
            var image = surface.Snapshot();
            var data = image.Encode(SKEncodedImageFormat.Png, 100);

            var printDocument = new PrintDocument();
            printDocument.PrintPage += (sender, e) =>
            {
                using (var ms = new MemoryStream(data.ToArray()))
                using (var bitmap = new System.Drawing.Bitmap(ms))
                {
                    e.Graphics.DrawImage(bitmap, e.MarginBounds);
                }
            };
            try
            {
                //اختر اسم الطابعة
                string printerName = SettingsViewModel.GetSavedPrinterName();
                PrinterSettings printerSettings = new PrinterSettings();
                printerSettings.PrinterName = printerName;
                printDocument.PrinterSettings = printerSettings;                
                printDocument.Print();
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.Dispatcher.Dispatch(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert("خطأ في الطباعة", ex.Message, "موافق");
                });
            }
        }

        
    }
}
