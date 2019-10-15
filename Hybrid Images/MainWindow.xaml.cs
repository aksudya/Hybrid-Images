using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Hybrid_Images.CScode;
using OpenCvSharp;
using Window = System.Windows.Window;

namespace Hybrid_Images
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Hybrid hybrid=new Hybrid();
            InitializeComponent();
        }

        private void img1_click(object sender, RoutedEventArgs e)
        {
            string storepath;
            storepath = chooseimage(sender, e);
       
            if (storepath != "")
            {
               Hybrid.instance.img1 = new Mat(@storepath, ImreadModes.AnyColor | ImreadModes.AnyDepth);
            }

            img1.Source = Hybrid.instance.MatToBitmapImage(Hybrid.instance.img1);
            if (Hybrid.instance.img1 != null)
            {
                Hybrid.instance.caculate();
                imgDst.Source = Hybrid.instance.img_tar;
            }

        }

        private void img2_click(object sender, RoutedEventArgs e)
        {
            string storepath;
            storepath = chooseimage(sender, e);
            if (storepath != "")
            {
                Hybrid.instance.img2= new Mat(@storepath, ImreadModes.AnyColor | ImreadModes.AnyDepth);
            }

            img2.Source = Hybrid.instance.MatToBitmapImage(Hybrid.instance.img2);
            if (Hybrid.instance.img2 != null)
            {
                Hybrid.instance.caculate();
                imgDst.Source= Hybrid.instance.img_tar;
            }
        }

        private void switch_click(object sender, RoutedEventArgs e)
        {
            Mat temp = Hybrid.instance.img2;
            Hybrid.instance.img2 = Hybrid.instance.img1;
            Hybrid.instance.img1 = temp;

            img1.Source = Hybrid.instance.MatToBitmapImage(Hybrid.instance.img1);
            img2.Source = Hybrid.instance.MatToBitmapImage(Hybrid.instance.img2);

            Hybrid.instance.caculate();
            imgDst.Source = Hybrid.instance.img_tar;
        }

        private string chooseimage(object sender, RoutedEventArgs e)
        {
            string storepath = "";
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "image file|*.jpg;*.png;*.jpeg;*.bmp"
            };
            var result = openFileDialog.ShowDialog();

            if (result == true)
            {
                storepath = openFileDialog.FileName;
            }
            return storepath;
        }

        private void fre_change(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Hybrid.instance.frequency = frequent.Value;
            if (Hybrid.instance.img1 != null&& Hybrid.instance.img2 != null)
            {
                Hybrid.instance.caculate();
                imgDst.Source = Hybrid.instance.img_tar;
            }
            frequent_lable.Content = Hybrid.instance.frequency.ToString();
        }

        private void ksize_change(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Hybrid.instance.ksize = (int)ksize.Value*2+1;
            if (Hybrid.instance.img1 != null && Hybrid.instance.img2 != null)
            {
                Hybrid.instance.caculate();
                imgDst.Source = Hybrid.instance.img_tar;
            }

            ksize_lable.Content = Hybrid.instance.ksize.ToString();
        }
    }
}
