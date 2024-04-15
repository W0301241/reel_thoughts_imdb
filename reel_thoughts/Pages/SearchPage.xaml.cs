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
using Microsoft.IdentityModel.Tokens;

namespace reel_thoughts.Pages
{
    /// <summary>
    /// Interaction logic for SearchPage.xaml
    /// </summary>
    public partial class SearchPage : Page
    {
        ImdbContext context = new ImdbContext();

        public SearchPage()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                MessageBox.Show("Please enter a search term.");
                return;
            }

            var results = context.Names
                                 .Where(n => n.PrimaryName.ToLower().Contains(searchText))
                                 .Select(n => new { n.PrimaryName, n.PrimaryProfession })
                                 .Take(50) // Reduce the number initially loaded to see if it resolves the freezing
                                 .ToList();

            PeopleListView.ItemsSource = results;

            var movieResults = context.Titles
                                 .Where(t => t.PrimaryTitle.ToLower().Contains(searchText))
                                 .Select(t => new { t.PrimaryTitle, t.TitleType, Genres = t.Genres.Select(g => g.Name) })
                                 .Take(50) // Reduce the number initially loaded to see if it resolves the freezing
                                 .ToList();
            //TODO: separate the movie, tv show, short and video results

            TitlesListView.ItemsSource = movieResults;
        }
    }
}
