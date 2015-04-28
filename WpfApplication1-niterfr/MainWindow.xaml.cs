using System;
using System.Collections.Generic;
using System.Linq;
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
using HtmlAgilityPack;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.IO;

namespace WpfApplication1_niterfr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string callername = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(callername));
            }
        }


        private ObservableCollection<ImageAndTitle> titleimag = new ObservableCollection<ImageAndTitle>();

        public ObservableCollection<ImageAndTitle> imageAndTitleList
        {
            get { return titleimag; }
            set { titleimag = value; }
        }



        //find div class action_175px_div
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();

            var doc = new HtmlDocument();
            doc.Load(DataSource.prewww + DataSource.drawings_alison);



            var wefwefwe = doc.DocumentNode.Descendants("div").Where(d =>
                                                    d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("action_175px_div")).ToList();

            foreach (var item in wefwefwe)
            {
                var ewfwe = new ImageAndTitle();
                //game inteference
                var fdjkdsf = item.Descendants("a").ToList();
                var eefff = fdjkdsf[0].Attributes["href"].Value;
                ewfwe.GameUrl = DataSource.interfhome + eefff;

                var imgsdf = item.Descendants("img").ToList();
                var imgwef = imgsdf[0].Attributes["src"].Value;
                ewfwe.ImagPath = imgwef;
                //ewfwe.Image = new BitmapImage(new Uri(ewfwe.ImagPath));

                //var eefjfj = fdjkdsf
                BitmapImage wefwe = new BitmapImage(new Uri(ewfwe.ImagPath));
                //Image1Test.Source = wefwe;

                //if(item.Attributes.cl)
                //get innerText and innerHTML
                ewfwe.Title = item.InnerText.Replace(@"\r\n","").Replace(@"\","").Replace(@":", "").Replace(@",", "").Replace("\"", "").Replace("?", "").Trim(); ;
                //var aaaa = item.
                imageAndTitleList.Add(ewfwe);
                //break;
                SavePhoto(ewfwe);
                await Task.Delay(1000);

            }

            ////var ewfefewf = doc.DocumentNode.Descendants("img").ToList();
            //var fefewwfewfefewf = doc.DocumentNode.Descendants("a").ToList();

            //foreach (var item in fefewwfewfefewf)
            //{
            //    //fefewwfewfefewf.
            //}

        }
        public void SavePhoto(string istrImagePath)
        {
            BitmapImage objImage = new BitmapImage(new Uri(istrImagePath, UriKind.RelativeOrAbsolute));

            objImage.DownloadCompleted += objImage_DownloadCompleted;
        }
        public void SavePhoto(ImageAndTitle imagandTitle)
        {
            BitmapImage objImage = new BitmapImage(new Uri(imagandTitle.ImagPath, UriKind.RelativeOrAbsolute));

            objImage.DownloadCompleted += objImage_DownloadCompleted;
        }

        private void objImage_DownloadCompleted(object sender, EventArgs e)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            Guid photoID = System.Guid.NewGuid();
            //var titleddd = imageAndTitleList.First((x) =>
            //{
            //    return x.ImagPath == (sender as BitmapImage).UriSource.ToString();
            //});
            String photolocation = DataSource.prewww + @"alison\" + photoID + ".jpg";  //file name 
            //String photolocation = DataSource.prewww + photoID.ToString() + ".jpg";  //file name 

            encoder.Frames.Add(BitmapFrame.Create((BitmapImage)sender));

            using (var filestream = new FileStream(photolocation, FileMode.Create))
                encoder.Save(filestream);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var eeff = (sender as FrameworkElement).DataContext as ImageAndTitle;
            System.Diagnostics.Process.Start(eeff.GameUrl);
        }
    }
    public class ImageAndTitle
    {

        public string Title { get; set; }
        public string ImagPath { get; set; }
        public Image Image { get; set; }
        public string GameUrl { get; set; }
    }
    public static class DataSource
    {
        public static string prewww = @"C:\Users\bryan\Documents\Visual Studio 14\Projects\WpfApplication1-niterfr\WpfApplication1-niterfr\";
        public static string profile_bryan = "interfer-profile.txt";
        public static string profile_alison = "intef-alis.txt";
        public static string games_alison = "alis-int-games.txt";
        public static string games_bryan = "interfer-games.txt";
        public static string drawings_bryan = "drawings-interf.txt";
        public static string drawings_alison = "alis-int-draw.txt";
        public static string example_game = "example-game-int.txt";
        public static string interfhome = "http://www.playinterference.com";
    }
}
