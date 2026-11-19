using FilmCatalog.Models;
using System.Windows;

namespace FilmCatalog
{
    public partial class MovieDetailsWindow : Window
    {
        public MovieDetailsWindow(Movie movie)
        {
            InitializeComponent();
            TitleText.Text = movie.Title;
            YearText.Text = movie.ReleaseYear.HasValue ? $"Год: {movie.ReleaseYear}" : "Год: —";
            GenreText.Text = $"Жанр: {movie.Genre ?? "—"}";
            DurationText.Text = movie.Duration.HasValue ? $"Длительность: {movie.Duration} мин" : "Длительность: —";
            RatingText.Text = movie.Rating.HasValue ? $"Рейтинг: {movie.Rating}" : "Рейтинг: —";
            DescriptionText.Text = string.IsNullOrWhiteSpace(movie.Description)
                ? "Описание отсутствует."
                : movie.Description;
        }

        private void Close_Click(object sender, RoutedEventArgs e) => Close();
    }
}