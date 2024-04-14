using IMDBInterface.Data;
using IMDBInterface.Models;
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

namespace reel_thoughts.Pages
{
    /// <summary>
    /// Interaction logic for RatedPage.xaml
    /// </summary>
    public partial class RatedPage : Page
    {
        ImdbContext context = new ImdbContext();
        CollectionViewSource ratingViewSource = new CollectionViewSource();

        public RatedPage()
        {
            InitializeComponent();
            ratingViewSource = (CollectionViewSource)FindResource(nameof(ratingViewSource));

            context.Ratings.Load();
            context.Titles.Load();

            ratingViewSource.Source = context.Ratings.Local.ToObservableCollection();

            var query = (from rating in context.Ratings
                                                join title in context.Titles on rating.TitleId equals title.TitleId
                                                orderby rating.AverageRating descending
                                                select new { TitleName = title.PrimaryTitle, AverageRating = rating.AverageRating }).Take(50);


            if (query.Any())
            {
                ratingViewSource.Source = query.ToList();
            }
            else
            {
                MessageBox.Show("No results found");
            }

        }

        private void LoadTopRated()
        {
            var topRated = (from rating in context.Ratings
                            join title in context.Titles on rating.TitleId equals title.TitleId
                            orderby rating.AverageRating descending
                            select new { TitleName = title.PrimaryTitle, AverageRating = rating.AverageRating })
                           .Take(75)
                           .ToList();

            
            ratingViewSource.Source = topRated.ToList();
        }

        private void LoadBottomRated()
        {
            var bottomRated = (from rating in context.Ratings
                               join title in context.Titles on rating.TitleId equals title.TitleId
                               orderby rating.AverageRating ascending
                               select new { TitleName = title.PrimaryTitle, AverageRating = rating.AverageRating })
                              .Take(75)
                              .ToList();

            ratingViewSource.Source = bottomRated.ToList();
        }

        private void TopRated_Click(object sender, RoutedEventArgs e)
        {
            LoadTopRated();
        }

        private void BottomRated_Click(object sender, RoutedEventArgs e)
        {
            LoadBottomRated();
        }
    }
}
