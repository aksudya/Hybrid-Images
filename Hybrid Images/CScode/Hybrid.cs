using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using OpenCvSharp;
using OpenCvSharp.XFeatures2D;
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

        public BitmapImage img_tar1;
        public BitmapImage img_tar2;
        public BitmapImage img_tar3;

        public Mat img_target1;
        public Mat img_target2;
        public Mat img_target3;

        public double frequency;
        public int ksize;
        public int cur_select;

        public double psize;

        public Hybrid()
        {
            frequency = 0.1;
            psize = 0.1;
            ksize = 1;
            cur_select = 1;
            instance = this;
        }

        public void clear()
        {
            img1 = null;
            img2 = null;
            img_target = null;
            img_tar = null;
            img_tar1 = null;
            img_tar2 = null;
            img_tar3 = null;

            img_target1 = null;
            img_target2 = null;
            img_target3 = null;

            cur_select = 1;

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

            possin();
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

        public bool check(Mat a, Mat b)
        {
            if (a == null || b == null)
            {
                return true;
            }
            if (a.Cols != b.Cols || a.Rows != b.Rows)
            {
                return false;
            }
            else
                return true;
        }

        public unsafe void possin()
        {
            Mat src_mask =new Mat(img1.Rows, img1.Cols,img1.Depth(),255);
            OpenCvSharp.Point center= new OpenCvSharp.Point(img1.Cols / 2, img1.Rows / 2);

            Mat src_mask1 = new Mat(img1.Rows, img1.Cols, img1.Depth(), 0);
            int mask_1 = (int)(psize * img1.Rows);
            int mask_2 = (int)((1- psize) * img1.Rows);
            int mask_3 = (int)(psize * img1.Cols);
            int mask_4 = (int)((1 - psize) * img1.Cols);

            for (int i = 1; i < img1.Rows - 1; i++)
            {
                IntPtr a = src_mask1.Ptr(i);
                byte* b = (byte*)a.ToPointer();
                for (int j = 1; j < img1.Cols - 1; j++)
                {
                    if (i >= mask_1 && i <= mask_2 && j >= mask_3 && j <= mask_4)
                    {
                        b[j] = 255;
                    }
                }
            }

            img_target1 = img1.Clone();
            img_target2 = img1.Clone();
            img_target3 = img1.Clone();

            Cv2.SeamlessClone(img1, img2, src_mask, center, img_target1, SeamlessCloneMethods.MixedClone);
            Cv2.SeamlessClone(img1, img2, src_mask1, center, img_target2, SeamlessCloneMethods.MonochromeTransfer);
            Cv2.SeamlessClone(img1, img2, src_mask1, center, img_target3, SeamlessCloneMethods.NormalClone);

            img_tar1 = MatToBitmapImage(img_target1);
            img_tar2 = MatToBitmapImage(img_target2);
            img_tar3 = MatToBitmapImage(img_target3);
            //Cv2.ImShow("normal_clone", normal_clone);
        }
    }
}
