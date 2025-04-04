﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BookingApp.Repositories;
using System.Windows.Controls;
using BookingApp.Domain.Models;
using BookingApp.Aplication.Dto;
using System.Text.RegularExpressions;
using System.Windows.Input;
using BookingApp.WPF.Views.GuestView;

namespace BookingApp.GuestView
{
    /// <summary>
    /// Interaction logic for PropertyViewWindow.xaml
    /// </summary>
    public partial class PropertyView : Page
    {
        public Property SelectedProperty { get; set; }
        public LocationDto Location { get; set; }
        public Guest LoggedInGuest { get; set; }
        public GuestRepository GuestRepository { get; set; }
        public PropertyRepository PropertyRepository { get; set; }
        public PropertyReservationRepository PropertyReservationRepository { get; set; }
        public OwnerRepository OwnerRepository { get; set; }
        public List<int> SueprOwnersIds {  get; set; }
        public PropertyView(User user)
        {
            InitializeComponent();
            DataContext = this;
            GuestRepository = new GuestRepository();
            LoggedInGuest = GuestRepository.GetByUserId(user.Id);
            SelectedProperty = new Property();
            PropertyRepository = new PropertyRepository();
            PropertyReservationRepository = new PropertyReservationRepository();
            SueprOwnersIds = new List<int>();
            OwnerRepository = new OwnerRepository();
            foreach (Owner owner in OwnerRepository.GetAll())
            {
                if (owner.IsSuperOwner == true)
                {
                    SueprOwnersIds.Add(owner.Id);
                }
            }
            var superOwnerProperties = PropertyRepository.GetAllProperties().Where(property => SueprOwnersIds.Contains(property.OwnerId));
            var otherProperties = PropertyRepository.GetAllProperties().Except(superOwnerProperties);
            var sortedProperties = superOwnerProperties.Concat(otherProperties);
            propertiesData.ItemsSource = sortedProperties;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]+");
            return !regex.IsMatch(text);
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Tab || e.Key == Key.Enter || e.Key == Key.Escape)
            {
                e.Handled = false;
            }
        }
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string name = SearchByName.Text.Trim();
            string location = SearchByLocation.Text.Trim();
            string type = SearchByType.Text.Trim();
            string guests = SearchByGuests.Text.Trim();
            string days = SearchByDays.Text.Trim();


            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(location) && string.IsNullOrEmpty(type) && string.IsNullOrEmpty(guests) && string.IsNullOrEmpty(days))
            {
                MessageBox.Show("Please enter at least one search parameter.");
                return;
            }

            List<Property> rezultatiPretrage = SearchProperty(name, location, type, guests, days);

            propertiesData.ItemsSource = rezultatiPretrage;
            propertiesData.Items.Refresh();
        }
       private List<Property> SearchProperty(string name, string location, string type, string guests, string days)
        {
            var results = new List<Property>();
            PropertyRepository = new PropertyRepository();
            results = PropertyRepository.GetAllProperties();
            if (!string.IsNullOrEmpty(name))
              {
                results = results.Where(property => property.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
              }
              
              if (!string.IsNullOrEmpty(location))
              {
                results = results.Where(property => (property.Location.City.Contains(location, StringComparison.OrdinalIgnoreCase) || property.Location.Country.Contains(location, StringComparison.OrdinalIgnoreCase))).ToList();
              }

            if (!string.IsNullOrEmpty(type))
            {
                PropertyType result = (PropertyType)Enum.Parse(typeof(PropertyType), type);
                results = results.Where(property => property.Type.Equals(result)).ToList();
            }

            if (!string.IsNullOrEmpty(guests))
            {
                int guestsNumber = Convert.ToInt32(guests);
                results = results.Where(property => property.MaxGuests >= guestsNumber).ToList();
            }

            if (!string.IsNullOrEmpty(days))
            {
                int daysNumber = Convert.ToInt32(days);
                results = results.Where(property => property.MinReservationDays <= daysNumber).ToList();
            }

            return results;
        }

        private void propertiesData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (propertiesData.SelectedItem != null && propertiesData.SelectedItem is Property)
            {
                SelectedProperty = propertiesData.SelectedItem as Property;

                SelectedPropertyView selectedPropertyView = new SelectedPropertyView(SelectedProperty, LoggedInGuest, PropertyRepository, PropertyReservationRepository);
                NavigationService.Navigate(selectedPropertyView);
            }
        }

        private void AnywhereAnytime_Click(object sender, RoutedEventArgs e)
        {
            AnywhereAnytime anywhereAnytime = new AnywhereAnytime(LoggedInGuest);
            NavigationService.Navigate(anywhereAnytime);
        }
    }

}
