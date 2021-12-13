using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using StorytelApp.Annotations;
using StorytelApp.DataAccess.Concrete;
using StorytelApp.DataAccess.Context;

namespace StorytelApp
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<BookUC> BookUCs { get; set; } = new ObservableCollection<BookUC>();
        public readonly GenericRepositoryPattern<Book> Repository = new GenericRepositoryPattern<Book>();
        public IQueryable<Book> Query { get; set; }
        public IQueryable<Book> DefaultQuery { get; set; }

        private string _categorySelected;
        public string CategorySelected
        {
            get => _categorySelected;
            set
            {
                _categorySelected = value;
                OnPropertyChanged();
            }
        }

        private string _sortSelected;
        public string SortSelected
        {
            get => _sortSelected;
            set
            {
                _sortSelected = value;
                OnPropertyChanged();
            }
        }

        private string _languageSelected;
        public string LanguageSelected
        {
            get => _languageSelected;
            set
            {
                _languageSelected = value;
                OnPropertyChanged();
            }
        }

        //create full property for SelectedBook
        private BookUC _selectedBook;

        public BookUC SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ObservableCollection<string> Categories { get; set; }
        public ObservableCollection<string> Languages { get; set; }

        //create property for Info window
        public Info Info { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
            DefaultQuery = Repository.GetAll();
            Query = DefaultQuery;

            QueryAllBooks();

            Categories = new ObservableCollection<string>();
            Languages = new ObservableCollection<string>();

            foreach (var language in Query.Select(l => l.Language).Distinct())
                Languages.Add(language);
            Languages.Add("All");

            foreach (var category in Query.Select(c => c.Category).Distinct())
                Categories.Add(category);
            Categories.Add("All");

            SortCmb.Items.Add("KİTAP ADI");
            SortCmb.Items.Add("YAZAR");
            SortCmb.Items.Add("YAYIN TARİHİ");
            CategorySelected = "All";
            LanguageSelected = "All";
        }

        private void QueryAllBooks()
        {
            BookUCs.Clear();
            var result = Query.ToList();
            foreach (var book in result)
            {
                var bookUc = new BookUC(book);
                bookUc.Width = 140;
                bookUc.Height = 180;
                BookUCs.Add(bookUc);
            }
        }

        private void WrapPanel_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
                for (int i = 0; i < 3; i++)
                    ScrlVwr.LineDown();
            else
                for (int i = 0; i < 3; i++)
                    ScrlVwr.LineUp();
        }

        private void CategoriesCmb_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchBox.Text = "";
            BookUCs.Clear();

            if (CategorySelected != "All")
                Query = Query.Where(c => c.Category == CategorySelected);

            if (LanguageSelected != "All")
                Query = Query.Where(l => l.Language == LanguageSelected);

            if (CategorySelected == "All")
            {
                Query = DefaultQuery;
                SortSelected = null;
                QueryAllBooks();
            }

            if (Query.Any() != true)
                ErrorMethod();
            else
                QueryAllBooks();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchBox.Text = "";

            BookUCs.Clear();
            if (CategorySelected != "All")
                Query = Query.Where(c => c.Category == CategorySelected);

            if (LanguageSelected != "All")
                Query = Query.Where(l => l.Language == LanguageSelected);

            switch (SortSelected)
            {
                case "KİTAP ADI":
                    Query = Query.OrderBy(o => o.BookName);
                    break;
                case "YAZAR":
                    Query = Query.OrderBy(o => o.Author);
                    break;
                case "YAYIN TARİHİ":
                    Query = Query.OrderBy(o => o.PublishedDate);
                    break;
            }

            if (Query.Any() != true)
                ErrorMethod();
            else
                QueryAllBooks();
        }

        private void ErrorMethod()
        {
            MessageBox.Show("Sorry, no results were found for your query. Please try again");
            LanguageSelected = "All";
            CategorySelected = "All";
            SortSelected = null;
            Query = DefaultQuery;
            QueryAllBooks();
        }

        private void LangCmb_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BookUCs.Clear();
            if (CategorySelected != "All")
                Query = Query.Where(c => c.Category == CategorySelected);
            if (LanguageSelected != "All")
                Query = Query.Where(l => l.Language == LanguageSelected);

            if (LanguageSelected == "All")
            {
                Query = DefaultQuery;
                SortSelected = null;
                QueryAllBooks();
            }

            if (Query.Any() != true)
                ErrorMethod();
            else
                QueryAllBooks();
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(sender is TextBox txt)) return;
            var result = Query.Where(b => b.BookName.ToLower().Contains(txt.Text.ToLower())).ToList();
            BookUCs.Clear();
            foreach (var bookUc in result.Select(book => new BookUC(book)))
            {
                bookUc.Width = 140;
                bookUc.Height = 180;
                BookUCs.Add(bookUc);
            }
        }

        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is ListBox lst)) return;
            if (!(lst.SelectedItem is BookUC book)) return;
            var bookInfo = new Info(book.BookItm);
            bookInfo.ShowDialog();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}