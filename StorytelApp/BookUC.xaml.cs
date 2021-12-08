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
using StorytelApp.DataAccess.Context;

namespace StorytelApp
{
    public partial class BookUC : UserControl
    {
        public DataClasses1DataContext dtx { get; set; } = new DataClasses1DataContext();



        public string BkName
        {
            get => (string)GetValue(BkNameProperty);
            set { SetValue(BkNameProperty, value); }
        }

        public static readonly DependencyProperty BkNameProperty =
            DependencyProperty.Register("BkName", typeof(string), typeof(BookUC));




        public Book BookItm
        {
            get => (Book)GetValue(BookProperty);
            set => SetValue(BookProperty, value);
        }


        public static readonly DependencyProperty BookProperty =
            DependencyProperty.Register("Book", typeof(Book), typeof(BookUC));


        public BookUC(Book book)
        {
            InitializeComponent();
            DataContext = this;
            BookItm = book;
            BkName = BookItm.BookName;
        }
    }
}
