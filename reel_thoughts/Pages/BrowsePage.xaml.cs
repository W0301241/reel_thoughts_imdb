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
            InitializeMoviesList();
        }

        private void InitializeMoviesList()
        {
            // Load only the primary titles for initial display
            var movies = context.Titles
                .Select(t => new
                {
                    t.TitleId,
                    PrimaryTitle = t.PrimaryTitle
                })
                .OrderBy(t => t.PrimaryTitle)
                .ToList();

            moviesListView.ItemsSource = movies;
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            var expander = sender as Expander;
            if (expander != null && expander.IsExpanded)
            {
                var title = expander.Header.ToString();
                var movieDetails = context.Titles
                    .Where(t => t.PrimaryTitle == title)
                    .Select(t => new
                    {
                        Year = t.StartYear,
                        Genres = string.Join(", ", t.Genres.Select(g => g.Name)),
                        AverageRating = t.Rating.AverageRating
                    }).FirstOrDefault();

                if (movieDetails != null)
                {
                    var contentPanel = expander.Content as StackPanel;
                    if (contentPanel != null)
                    {
                        contentPanel.Children.Clear();
                        contentPanel.Children.Add(new TextBlock { Text = $"Year: {movieDetails.Year}", FontWeight = FontWeights.Bold });
                        contentPanel.Children.Add(new TextBlock { Text = $"Genres: {movieDetails.Genres}" });
                        contentPanel.Children.Add(new TextBlock { Text = $"Rating: {movieDetails.AverageRating}" });
                    }
                }
            }
        }
    }
}
