using System;
using System.Linq;
using System.Windows;
using library.Data;
using library.Models;

namespace library
{
    public partial class BookEditWindow : Window
    {
        private LibraryContext _context;
        public Book Book { get; private set; }

        public BookEditWindow(Book book = null)
        {
            InitializeComponent();
            _context = new LibraryContext();

            AuthorComboBox.ItemsSource = _context.Authors.ToList();
            GenreComboBox.ItemsSource = _context.Genres.ToList();

            if (book != null)
            {
                Book = _context.Books.Find(book.Id);
                TitleTextBox.Text = Book.Title;
                YearTextBox.Text = Book.PublishYear.ToString();
                IsbnTextBox.Text = Book.ISBN;
                QuantityTextBox.Text = Book.QuantityInStock.ToString();
                AuthorComboBox.SelectedItem = _context.Authors.FirstOrDefault(a => a.Id == Book.AuthorId);
                GenreComboBox.SelectedItem = _context.Genres.FirstOrDefault(g => g.Id == Book.GenreId);
            }
            else
            {
                Book = new Book();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorComboBox.SelectedItem == null || GenreComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите автора и жанр!");
                return;
            }

            Book.Title = TitleTextBox.Text;
            Book.ISBN = IsbnTextBox.Text;
            Book.AuthorId = ((Author)AuthorComboBox.SelectedItem).Id;
            Book.GenreId = ((Genre)GenreComboBox.SelectedItem).Id;

            if (int.TryParse(YearTextBox.Text, out int year)) Book.PublishYear = year;
            if (int.TryParse(QuantityTextBox.Text, out int qty)) Book.QuantityInStock = qty;

            if (Book.Id == 0) _context.Books.Add(Book);
            _context.SaveChanges();

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}