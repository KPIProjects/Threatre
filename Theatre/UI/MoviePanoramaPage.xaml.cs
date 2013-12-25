using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Theatre.Storage;
using Theatre.Storage.Movies;
using System.Windows.Navigation;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Theatre.Storage.Session;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Tasks;

namespace Theatre.UI
{
    public partial class MoviePanoramaPage : PhoneApplicationPage
    {
        public MoviePanoramaPage()
        {
            InitializeComponent();
            ContentPanel_Content.Visibility = Visibility.Collapsed; //HIDDEN!
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("idx") && NavigationContext.QueryString.ContainsKey("type"))
            {
                int.TryParse(NavigationContext.QueryString["idx"].ToString(), out movieIdx);
                movieType = NavigationContext.QueryString["type"].ToString();

                if (movieType == "now")
                {
                    this.movie = AppSettings.Instance.Storage.NowMovies[movieIdx];
                }
                else if (movieType == "upcoming")
                {
                    this.movie = AppSettings.Instance.Storage.UpcomingMovies[movieIdx];
                }
                AppSettings.Instance.Storage.GetExtendMovieInformation(movie, (resultMovie) =>
                {
                    this.movie = resultMovie;

                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        UpdateDescription();
                        UpdateImageCollection();
                    });
                });

                PanoramaControl.Title = this.movie.Title;
                PanoramaControl.DefaultItem = PanoramaDescriprionItem;

            }
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            this.movie.LengthDidLoadEvent -= UpdateLength;
            this.movie.DescriptionDidLoadEvent -= UpdateDescription;
            this.movie.ReleaseDateDidLoadEvent -= UpdateReleaseDate;

            base.OnBackKeyPress(e);
        }

        private Movie movie;
        private string movieType;
        private int movieIdx;

        /* DESCRIPTION */
        private void UpdateDescription()
        {
            if (movie.Type == MovieType.Anounce)
            {
                BuyTicketButton.Visibility = Visibility.Collapsed;
            }
            this.movie.LengthDidLoadEvent += UpdateLength;
            this.movie.DescriptionDidLoadEvent += UpdateDescription;
            this.movie.ReleaseDateDidLoadEvent += UpdateReleaseDate;


            Poster.Source = movie.PosterThumbnail;;

            // Genres //
            Genre.Text = "";
            for (int i = 0; i < movie.Genres.Count; i++)
            {
                Genre.Text += movie.Genres[i];
                if (i < movie.Genres.Count - 1) Genre.Text += ", ";
            }

            // Description //
            Description.Text = movie.Description;

            // Release date //
            Date.Text = "Премьера: " + movie.ReleaseDate;

            // Length
            Length.Text = movie.Length;

            // Companies //
            Director.Text = "";
            for (int i = 0; i < movie.Directors.Count; i++)
            {
                Director.Text += movie.Directors.Keys.ElementAt(i);
                if (i < movie.Directors.Count - 1) Director.Text += ", ";
            }

            // Countries //
            if (movie.Countries.Count > 0)
                Country.Text = "Страна: " + movie.Countries[0];
            else
                Country.Text = "Страна: N/A";

            ContentPanel_Content.Visibility = Visibility.Visible; //VISIBLE!
            ContentPanel_Loading.Visibility = Visibility.Collapsed; //HIDDEN!

        }

        private void UpdateDescription(object sender, EventArgs e)
        {
            Description.Text = movie.Description;
        }

        private void UpdateReleaseDate(object sender, EventArgs e)
        {
            Date.Text = "Премьера: " + movie.ReleaseDate;
        }

        private void UpdateLength(object sender, EventArgs e)
        {
            Length.Text = movie.Length;
        }

        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/UI/PosterView.xaml?url=" + movie.PosterFullsizeURL, UriKind.Relative));
        }

        private void BuyTicketButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //PanoramaControl.DefaultItem = PanoramaControl.Items[1];
           NavigationService.Navigate(new Uri("/UI/ChooseCinema.xaml?idx=" + movieIdx.ToString(), UriKind.Relative));
        }

        /* IMAGE COLLECTION */
        ObservableCollection<ObservableCollection<ImageCellPack>> imagesToCollection;
        private void UpdateImageCollection()
        {
            // fill up images list 
            imagesToCollection = new ObservableCollection<ObservableCollection<ImageCellPack>>();
            imagesToCollection.Add(new ObservableCollection<ImageCellPack>());
            int completed = 0;
            foreach (string imageLink in movie.MovieImageURLs)
            {
                AppSettings.Instance.ImageManager.GetScreenWithURL(imageLink, 0, (img, idx) =>
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        ImageCellPack cell = new ImageCellPack();
                        cell.Bitmap = img;
                        cell.URL = imageLink;
                        imagesToCollection[0].Add(cell);
                        completed++;
                        if (completed == movie.MovieImageURLs.Count)
                        {
                            ImageList.ItemsSource = imagesToCollection;
                            ImageList.Tap += ScreenList_Tap;
                        }
                    });
                });
            }
        }
        
        void ScreenList_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            ImageCellPack selected = (ImageCellPack)ImageList.SelectedItem;
            NavigationService.Navigate(new Uri("/UI/PosterView.xaml?url=" + selected.URL, UriKind.Relative));
        }
        

        public class ImageCellPack
        {
            public BitmapImage Bitmap { get; set; }
            public string URL { get; set; }
        }

                
    }
}