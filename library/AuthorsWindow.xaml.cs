using System;
using System.Linq;
using System.Windows;
using library.Data;
using library.Models;

namespace library
{
    public partial class AuthorsWindow : Window
    {
        private LibraryContext _context;
        private Author _selectedAuthor;

        public AuthorsWindow()
        {
            InitializeComponent();
            _context = new LibraryContext();
            LoadAuthors();
        }

        private void LoadAuthors()
        {
            AuthorsDataGrid.ItemsSource = _context.Authors.ToList();
        }

        private void AuthorsDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (AuthorsDataGrid.SelectedItem is Author author)
            {
                _selectedAuthor = author;
                FirstNameTextBox.Text = author.FirstName;
                LastNameTextBox.Text = author.LastName;
                CountryTextBox.Text = author.Country;
                BirthDatePicker.SelectedDate = author.BirthDate;
            }
        }

        private void AddAuthor_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text) || string.IsNullOrWhiteSpace(LastNameTextBox.Text))
            {
                MessageBox.Show("Имя и фамилия обязательны!");
                return;
            }
            var newAuthor = new Author
            {
                FirstName = FirstNameTextBox.Text,
                LastName = LastNameTextBox.Text,
                Country = CountryTextBox.Text,
                BirthDate = BirthDatePicker.SelectedDate ?? DateTime.Now
            };
            _context.Authors.Add(newAuthor);
            _context.SaveChanges();
            LoadAuthors();
            ClearFields();
        }

        private void EditAuthor_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAuthor != null)
            {
                _selectedAuthor.FirstName = FirstNameTextBox.Text;
                _selectedAuthor.LastName = LastNameTextBox.Text;
                _selectedAuthor.Country = CountryTextBox.Text;
                _selectedAuthor.BirthDate = BirthDatePicker.SelectedDate ?? DateTime.Now;
                _context.SaveChanges();
                LoadAuthors();
                ClearFields();
            }
        }

        private void DeleteAuthor_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAuthor != null)
            {
                _context.Authors.Remove(_selectedAuthor);
                _context.SaveChanges();
                LoadAuthors();
                ClearFields();
            }
        }

        private void ClearFields()
        {
            _selectedAuthor = null;
            FirstNameTextBox.Text = "";
            LastNameTextBox.Text = "";
            CountryTextBox.Text = "";
            BirthDatePicker.SelectedDate = null;
            AuthorsDataGrid.SelectedItem = null;
        }
    }
}