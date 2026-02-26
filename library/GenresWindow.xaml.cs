using System.Linq;
using System.Windows;
using library.Data;
using library.Models;

namespace library
{
    public partial class GenresWindow : Window
    {
        private LibraryContext _context;
        private Genre _selectedGenre;

        public GenresWindow()
        {
            InitializeComponent();
            _context = new LibraryContext();
            LoadGenres();
        }

        private void LoadGenres() => GenresDataGrid.ItemsSource = _context.Genres.ToList();

        private void GenresDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (GenresDataGrid.SelectedItem is Genre genre)
            {
                _selectedGenre = genre;
                NameTextBox.Text = genre.Name;
                DescriptionTextBox.Text = genre.Description;
            }
        }

        private void AddGenre_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text)) return;
            _context.Genres.Add(new Genre { Name = NameTextBox.Text, Description = DescriptionTextBox.Text });
            _context.SaveChanges();
            LoadGenres();
            ClearFields();
        }

        private void EditGenre_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedGenre != null)
            {
                _selectedGenre.Name = NameTextBox.Text;
                _selectedGenre.Description = DescriptionTextBox.Text;
                _context.SaveChanges();
                LoadGenres();
                ClearFields();
            }
        }

        private void DeleteGenre_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedGenre != null)
            {
                _context.Genres.Remove(_selectedGenre);
                _context.SaveChanges();
                LoadGenres();
                ClearFields();
            }
        }

        private void ClearFields()
        {
            _selectedGenre = null;
            NameTextBox.Text = "";
            DescriptionTextBox.Text = "";
        }
    }
}