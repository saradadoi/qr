using OpenCvSharp;
using OpenCvSharp.Extensions; // これ追加しておく
using System.Threading;
using System.Windows;
using ZXing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using WpfApplication2;
using Microsoft.Win32;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;




namespace CameraCapture
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public bool IsExitCapture { get; set; }

        public MainWindow()
        {
            this.InitializeComponent();
        }


        /// <summary>
        /// カメラ画像を取得して次々に表示を切り替える
        /// </summary>
        public virtual void Capture(object state)
        {
            var camera = new VideoCapture(0/*0番目のデバイスを指定*/)
           {
                // キャプチャする画像のサイズフレームレートの指定
                FrameWidth = 1200, 
                FrameHeight =2400,
                Fps = 60
            };
            
            using (var img = new Mat()) // 撮影した画像を受ける変数
            using (camera)
            {
                while (true)
                {
                    if (this.IsExitCapture)
                    {
                        this.Dispatcher.Invoke(() => this._Image.Source = null);
                        break;
                    }

                    camera.Read(img); // Webカメラの読み取り（バッファに入までブロックされる

                    if (img.Empty())
                    {
                        break;
                    }

                    this.Dispatcher.Invoke(() =>
                    {
                        this._Image.Source = img.ToWriteableBitmap(); // WPFに画像を表示
                        img.SaveImage("C:/Users/Desktop/result.bmp");
                        const string CAPTURE_PATH = "C:/Users/Desktop/result.bmp";

                        var source = new BitmapImage(new Uri(CAPTURE_PATH));
                       this.qr.Source = source;

                     if (source != null)
                    {
                        // コードの解析
                       ZXing.Presentation.BarcodeReader reader = new ZXing.Presentation.BarcodeReader();
                       ZXing.Result result = reader.Decode(qr.Source as BitmapImage);
                      if (result != null)
                      {
                            MessageBox.Show(result.Text);
                       } 
                     }

                    });

              

                    
                    
                }
            }
        }

        // ---- EventHandlers ----

        /// <summary>
        /// Windowがロードされた時
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(this.Capture);
        }

        /// <summary>
        /// Exit Captureボタンが押され時
        /// </summary>
        protected virtual void Button_Click(object sender, RoutedEventArgs e)
        {

            this.IsExitCapture = true;

            // 指定された画像ファイルをImageコントロール「Image1」に表示
            // var source = new BitmapImage(new Uri("……省略（画像ファイルのパス）……"));
            // this._Image.Source = source;

        }

        protected virtual void Button(object sender, RoutedEventArgs e)
        {

            

            MessageBox.Show("efrwer");

            // 指定された画像ファイルをImageコントロール「Image1」に表示
            // var source = new BitmapImage(new Uri("……省略（画像ファイルのパス）……"));
            // this._Image.Source = source;

        }



    }
}
