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
            // Load only a minimal set of data, ensure this query is correct and optimal.
            var results = context.Names
                                 .Where(n => n.PrimaryName.ToLower().Contains(searchText))
                                 .Select(n => new { n.PrimaryName, n.PrimaryProfession })
                                 .Take(20) // Reduce the number initially loaded to see if it resolves the freezing
                                 .ToList();

            PeopleListView.ItemsSource = results;


            // Load only a minimal set of data, ensure this query is correct and optimal.
            var movieResults = context.Titles
                                 .Where(t => t.PrimaryTitle.ToLower().Contains(searchText))
                                 .Select(t => new { t.PrimaryTitle })
                                 .Take(20) // Reduce the number initially loaded to see if it resolves the freezing
                                 .ToList();

            TitlesListView.ItemsSource = movieResults;
        }
    }
}
