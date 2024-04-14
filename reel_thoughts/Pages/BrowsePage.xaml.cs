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
    public partial class BrowsePage : Page
    {
        ImdbContext context = new ImdbContext();
        
        public BrowsePage()
        {
            InitializeComponent();

            //context.Titles.Load();

            //TitlesListView.ItemsSource = context.Titles.Local.ToObservableCollection();
            
            var movies = context.Titles
                .Select(t => new
                {
                    PrimaryTitle = t.PrimaryTitle,
                    StartYear = t.StartYear,
                    AverageRating = t.Rating.AverageRating, 
                    RuntimeMinutes = t.RuntimeMinutes,
                    //I would like to have directors. but hwere?
                    Genres = string.Join(", ", t.Genres.Select(g => g.Name)) // Concatenate genre names
                })
                .ToList();

            
            TitlesListView.ItemsSource = movies;
        }

       
    }
}
