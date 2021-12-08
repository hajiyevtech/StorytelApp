using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using StorytelApp.DataAccess.Concrete;
using StorytelApp.DataAccess.Context;

namespace StorytelApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<BookUC> BookUCs { get; set; } = new ObservableCollection<BookUC>();
        public ObservableCollection<BookUC> DefUCs { get; set; } = new ObservableCollection<BookUC>();
        public DataClasses1DataContext dtx { get; set; } = new DataClasses1DataContext();
        public GenericRepositoryPattern<Book> Repository = new GenericRepositoryPattern<Book>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            QueryAllBooks();
            //DataClasses1DataContext dtx2 = new DataClasses1DataContext();

        }

        private void QueryAllBooks()
        {
            var result = Repository.GetAll().ToList();
            Dispatcher.Invoke(() => BookUCs.Clear());

            foreach (var book in result)
            {
                var bookItm = new BookUC(book) { Width = 140, Height = 180 };
                Dispatcher.Invoke(() => BookUCs.Add(bookItm));
                //Dispatcher.Invoke(() => DefUCs.Add(carAd));
            }
        }

        private void UIElement_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Handled)
            {
                AraLbl.Foreground = new SolidColorBrush(Color.FromRgb(255, 85, 0));
            }
            else
            {
                AraLbl.Foreground = new SolidColorBrush(Color.FromArgb((byte)86.7, 0, 0, 0));

            }
        }

        private void AraLbl_OnMouseEnter(object sender, MouseEventArgs e)
        {
            //AraLbl.Foreground = new SolidColorBrush(Color.FromRgb(255, 85, 0));
        }

        private void AraLbl_OnMouseLeave(object sender, MouseEventArgs e)
        {
            //AraLbl.Foreground = new SolidColorBrush(Color.FromArgb((byte)86.7, 0, 0, 0));
        }

        private void WrapPanel_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                ScrlVwr.LineDown();
                ScrlVwr.LineDown();
                ScrlVwr.LineDown();
            }
            else
            {
                ScrlVwr.LineUp();
                ScrlVwr.LineUp();
                ScrlVwr.LineUp();
            }
        }
    }
}
