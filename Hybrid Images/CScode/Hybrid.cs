using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using OpenCvSharp;
using Size = OpenCvSharp.Size;

namespace Hybrid_Images.CScode
{
    class Hybrid
    {
        public static Hybrid instance;
        public Mat img1;
        public Mat img2;
        public Mat img_target;
        public BitmapImage img_tar;

        public double frequency;
        public int ksize;

        public Hybrid()
        {
            frequency = 0.1;
            ksize = 1;
            instance = this;
        }

        public void caculate()
        {
            if (img1 == null || img2 == null)
            {
                return;
            }

            Mat img11=img1.Clone();

            Mat img22 = img2.Clone();
           
            Cv2.GaussianBlur(img1, img11, new Size(ksize,ksize), frequency);
            Cv2.GaussianBlur(img2, img22, new Size(ksize,ksize), frequency);
            img22 = img2 - img22;
            img_target = img22 + img11;
            img_tar= MatToBitmapImage(img_target);
        }

        public Bitmap MatToBitmap(Mat image)
        {
            return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image);
        }

        public BitmapImage MatToBitmapImage(Mat image)
        {
            Bitmap bitmap = MatToBitmap(image);
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }

    }
}
