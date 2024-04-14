using IMDBInterface.Data;
using Microsoft.EntityFrameworkCore;
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
using IMDBInterface.Models;

namespace reel_thoughts.Pages
{
    /// <summary>
    /// Interaction logic for BrowsePage.xaml
    /// </summary>
    /// 

    public partial class BrowsePage : Page
    {
        private ImdbContext context = new ImdbContext();

        public BrowsePage()
        {
            InitializeComponent();
            InitializeMoviesListAsync(); //await. more like... DONT wait, hahaha. 
        }

        private async Task InitializeMoviesListAsync()
        {
            var movies = await context.Titles
                .OrderBy(t => t.PrimaryTitle)
                .Select(t => new
                {
                    t.TitleId,
                    PrimaryTitle = t.PrimaryTitle
                })
                .ToListAsync();

            moviesListView.ItemsSource = movies;
        }

        private async void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            var expander = sender as Expander;
            if (expander != null && expander.IsExpanded)
            {
                var title = expander.Header.ToString();
                var movieDetails = await context.Titles
                    .Where(t => t.PrimaryTitle == title)
                    .Select(t => new
                    {
                        // Handling nullable values by directly converting them to strings in the query
                        Year = t.StartYear.HasValue ? t.StartYear.Value.ToString() : "N/A",
                        Genres = t.Genres.Any() ? string.Join(", ", t.Genres.Select(g => g.Name)) : "N/A",
                        AverageRating = t.Rating.AverageRating.HasValue ? t.Rating.AverageRating.Value.ToString("F2") : "N/A"
                    }).FirstOrDefaultAsync();

                if (movieDetails != null)
                {
                    var contentPanel = expander.Content as StackPanel;
                    contentPanel.Children.Clear();
                    contentPanel.Children.Add(new TextBlock { Text = $"Year: {movieDetails.Year}", FontWeight = FontWeights.Bold, Style = (Style)Resources["TextBlockStyle"] });
                    contentPanel.Children.Add(new TextBlock { Text = $"Genres: {movieDetails.Genres}", Style = (Style)Resources["TextBlockStyle"] });
                    contentPanel.Children.Add(new TextBlock { Text = $"Rating: {movieDetails.AverageRating}", Style = (Style)Resources["TextBlockStyle"] });

                }
            }
        }
    }
}
