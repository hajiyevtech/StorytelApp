using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using NAudio.Wave;
using StorytelApp.DataAccess.Context;

namespace StorytelApp
{
    public partial class Info : Window
    {
        public WaveOutEvent Wo { get; set; } = new WaveOutEvent();

        public string SoundUrl { get; set; }

        public string Image
        {
            get => (string)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(string), typeof(Info));

        public Info(Book book)
        {
            InitializeComponent();
            DataContext = this;

            Image = book.ImageLink;
            AuthorsTxt.Content = book.Author;
            DescriptionText.Text = book.Descriptions;
            BookNameTxt.Content = book.BookName;
            VoiceTxt.Content = book.VoiceOver;
            LangTxt.Content = book.Language;
            CatTxt.Content = book.Category;
            RatingBar.Value = (int)book.Rating;
            if (book.SoundLink == "")
                PlayBtn.IsEnabled = false;
            else
            {
                PlayBtn.IsEnabled = true;
                SoundUrl = book.SoundLink;
            }
        }

        private async Task PlayMedia()
        {
            await Task.Run(() =>
            {
                var url = SoundUrl;
                using (var mf = new MediaFoundationReader(url))
                using (Wo)
                {
                    Wo.Init(mf);
                    Wo.Play();
                    while (Wo.PlaybackState == PlaybackState.Playing)
                    {
                        Thread.Sleep(1000);
                    }
                }
            });
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Escape) return;
            Wo.Stop();
            Close();
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (Wo.PlaybackState == PlaybackState.Playing)
            {
                Wo.Stop();
                return;
            }
            await PlayMedia();
        }
    }
}