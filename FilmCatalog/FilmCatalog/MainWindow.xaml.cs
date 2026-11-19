using FilmCatalog.Data;
using FilmCatalog.Models;
using System.Windows;
using System.Windows.Controls;

namespace FilmCatalog
{
    public partial class MainWindow : Window
    {
        private readonly MovieRepository _repo = new MovieRepository();

        public MainWindow()
        {
            InitializeComponent();
            LoadMovies();
        }

        private void LoadMovies()
        {
            var search = SearchBox?.Text;
            var sortByGenre = SortByGenreCheck?.IsChecked == true;
            MoviesGrid.ItemsSource = _repo.GetAllMovies(search, sortByGenre);
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e) => LoadMovies();
        private void Sort_Changed(object sender, RoutedEventArgs e) => LoadMovies();
        private void Refresh_Click(object sender, RoutedEventArgs e) => LoadMovies();

        private void MoviesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MoviesGrid.SelectedItem is Movie m)
            {
                TitleBox.Text = m.Title;
                YearBox.Text = m.ReleaseYear?.ToString();
                GenreBox.Text = m.Genre;
                DurationBox.Text = m.Duration?.ToString();
                RatingBox.Text = m.Rating?.ToString();
                DescriptionBox.Text = m.Description;
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleBox.Text))
            {
                MessageBox.Show("Введите название фильма.");
                return;
            }
            _repo.AddMovie(BuildMovieFromFields(null));
            LoadMovies();
            ClearFields();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (MoviesGrid.SelectedItem is not Movie selected)
            {
                MessageBox.Show("Выберите фильм для обновления.");
                return;
            }
            _repo.UpdateMovie(BuildMovieFromFields(selected.Id));
            LoadMovies();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MoviesGrid.SelectedItem is not Movie selected)
            {
                MessageBox.Show("Выберите фильм для удаления.");
                return;
            }
            if (MessageBox.Show($"Удалить «{selected.Title}»?", "Подтверждение",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _repo.DeleteMovie(selected.Id);
                LoadMovies();
                ClearFields();
            }
        }

        // Кнопка "Подробнее" — открывает вторую форму
        private void Details_Click(object sender, RoutedEventArgs e)
        {
            if (MoviesGrid.SelectedItem is not Movie selected)
            {
                MessageBox.Show("Выберите фильм, чтобы увидеть подробности.");
                return;
            }

            var movie = _repo.GetById(selected.Id) ?? selected;
            var detailsWindow = new MovieDetailsWindow(movie) { Owner = this };
            detailsWindow.ShowDialog();
        }

        private Movie BuildMovieFromFields(int? id) => new Movie
        {
            Id = id ?? 0,
            Title = TitleBox.Text.Trim(),
            ReleaseYear = ParseNullableInt(YearBox.Text),
            Genre = GenreBox.Text.Trim(),
            Duration = ParseNullableInt(DurationBox.Text),
            Rating = ParseNullableDecimal(RatingBox.Text),
            Description = DescriptionBox.Text.Trim()
        };

        private void ClearFields()
        {
            TitleBox.Clear(); YearBox.Clear(); GenreBox.Clear();
            DurationBox.Clear(); RatingBox.Clear(); DescriptionBox.Clear();
        }

        private int? ParseNullableInt(string s) =>
            int.TryParse(s, out var v) ? v : (int?)null;

        private decimal? ParseNullableDecimal(string s) =>
            decimal.TryParse(s, out var v) ? v : (decimal?)null;
    }
}