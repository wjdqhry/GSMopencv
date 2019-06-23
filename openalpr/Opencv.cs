using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using Tesseract;

namespace openalpr
{
    class Opencv
    {
        private static readonly HttpClient client = new HttpClient();
        Mat tmp;
        Mat src;
        public Opencv(string file)
        {
            tmp = new Mat(@file, ImreadModes.AnyColor);
            src = new Mat(@file, ImreadModes.GrayScale);
        }

        public Bitmap convertbinp(string file, int weight)
        {
            Mat dst = new Mat();
            Cv2.Threshold(src, dst, weight, 255, ThresholdTypes.Binary);
            
            Bitmap bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(dst);
            bitmap.Save(@"D:\binaryimage.png", System.Drawing.Imaging.ImageFormat.Png);
            return bitmap;
        }
        public Bitmap convertbinp(string file)
        {
            Mat dst = new Mat();
            Cv2.Threshold(src, dst, 255, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);
            
            Bitmap bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(dst);
            bitmap.Save(@"D:\binaryimage.png", System.Drawing.Imaging.ImageFormat.Png);
            return bitmap;
        }
        public string Method1()
        {
            Task<string> recognizeTask = Task.Run(() => ProcessImage(@"D:\binaryimage.png"));
            recognizeTask.Wait();
            string task_result = recognizeTask.Result;
            string result = task_result.Split(':')[11].Split(',')[0];
            result = result.Replace("\"", "");
            result = result.Trim();

            return result;
        }
        public string Method2(Bitmap img)
        {
            var ocr = new TesseractEngine("./tessdata", "eng", EngineMode.TesseractAndCube);
            var texts = ocr.Process(img);
            return texts.GetText();
        }
        public static async Task<string> ProcessImage(string image_path)
        {
            string SECRET_KEY = "sk_11483b352b7ac0c0a5653a4a";

            Byte[] bytes = File.ReadAllBytes(image_path);
            string imagebase64 = Convert.ToBase64String(bytes);

            var content = new StringContent(imagebase64);

            var response = await client.PostAsync("https://api.openalpr.com/v2/recognize_bytes?recognize_vehicle=1&country=us&secret_key=" + SECRET_KEY, content).ConfigureAwait(false);

            var buffer = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            var byteArray = buffer.ToArray();
            var responseString = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);

            return responseString;
        }
    }
}
