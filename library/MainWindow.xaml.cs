using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using library.Data;
using library.Models;

namespace library
{
    public partial class MainWindow : Window
    {
        private LibraryContext _context;

        public MainWindow()
        {
            InitializeComponent();
            _context = new LibraryContext();
            _context.Database.EnsureCreated();
            LoadData();
        }

        private void LoadData()
        {
            var genres = _context.Genres.ToList();
            genres.Insert(0, new Genre { Id = 0, Name = "Все жанры" });
            GenreFilterComboBox.ItemsSource = genres;
            GenreFilterComboBox.DisplayMemberPath = "Name";
            GenreFilterComboBox.SelectedIndex = 0;

            var authors = _context.Authors.ToList();
            authors.Insert(0, new Author { Id = 0, LastName = "Все авторы" });
            AuthorFilterComboBox.ItemsSource = authors;
            AuthorFilterComboBox.DisplayMemberPath = "LastName";
            AuthorFilterComboBox.SelectedIndex = 0;

            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (_context == null) return;

            var query = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                query = query.Where(b => b.Title.Contains(SearchTextBox.Text));
            }

            if (GenreFilterComboBox.SelectedItem is Genre selectedGenre && selectedGenre.Id != 0)
            {
                query = query.Where(b => b.GenreId == selectedGenre.Id);
            }

            if (AuthorFilterComboBox.SelectedItem is Author selectedAuthor && selectedAuthor.Id != 0)
            {
                query = query.Where(b => b.AuthorId == selectedAuthor.Id);
            }

            BooksDataGrid.ItemsSource = query.ToList();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e) => ApplyFilters();
        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e) => ApplyFilters();

        private void ResetFilter_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = "";
            GenreFilterComboBox.SelectedIndex = 0;
            AuthorFilterComboBox.SelectedIndex = 0;
        }

        private void ManageAuthors_Click(object sender, RoutedEventArgs e)
        {
            AuthorsWindow authorsWindow = new AuthorsWindow();
            authorsWindow.Owner = this;
            authorsWindow.ShowDialog();
            _context = new LibraryContext();
            LoadData();
        }

        private void ManageGenres_Click(object sender, RoutedEventArgs e)
        {
            GenresWindow genresWindow = new GenresWindow();
            genresWindow.Owner = this;
            genresWindow.ShowDialog();
            _context = new LibraryContext();
            LoadData();
        }

        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            var editWin = new BookEditWindow { Owner = this };
            if (editWin.ShowDialog() == true) ApplyFilters();
        }

        private void EditBook_Click(object sender, RoutedEventArgs e)
        {
            if (BooksDataGrid.SelectedItem is Book selectedBook)
            {
                var editWin = new BookEditWindow(selectedBook) { Owner = this };
                if (editWin.ShowDialog() == true) ApplyFilters();
            }
        }

        private void DeleteBook_Click(object sender, RoutedEventArgs e)
        {
            if (BooksDataGrid.SelectedItem is Book selectedBook)
            {
                if (MessageBox.Show("Точно удалить?", "Удаление", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _context.Books.Remove(selectedBook);
                    _context.SaveChanges();
                    ApplyFilters();
                }
            }
        }
    }
}