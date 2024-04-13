﻿using IMDBInterface.Data;
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
        CollectionViewSource searchViewSource = new CollectionViewSource();

        public SearchPage()
        {
            InitializeComponent();

            searchViewSource = (CollectionViewSource)FindResource(nameof(searchViewSource));

            context.Names.Load();

            searchViewSource.Source = context.Names.Local.ToObservableCollection();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var searchText = txtSearch.Text.ToLower(); // Case-insensitive search
            var query = (from name in context.Names.Local
                        where name.PrimaryName.ToLower().Contains(searchText) // Case-insensitive search
                        select name).Take(50);

            searchViewSource.Source = query.ToList();
        }
    }
}
