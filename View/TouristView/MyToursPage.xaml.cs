﻿using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.View.TouristView;
using BookingApp.ViewModel.TouristView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace BookingApp.View.TouristView
{
    /// <summary>
    /// Interaction logic for MyToursPaige.xaml
    /// </summary>
    public partial class MyToursPage : Page
    {
        private MyToursViewModel myToursViewModel;
        public MyToursPage(User loggedInUser)
        {
            InitializeComponent();
            myToursViewModel = new MyToursViewModel(loggedInUser);
            DataContext = myToursViewModel;

            NoMyTours.SetBinding(TextBlock.VisibilityProperty, new System.Windows.Data.Binding(nameof(myToursViewModel.NoMyToursTextVisibility)));
            NoActiveTours.SetBinding(TextBlock.VisibilityProperty, new System.Windows.Data.Binding(nameof(myToursViewModel.NoActiveToursTextVisibility)));

        }
        
        private void HelpButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void RateButtonClick(object sender, RoutedEventArgs e)
        {
            myToursViewModel.RateTour(sender);
        }
    }
}
