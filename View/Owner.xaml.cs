﻿using BookingApp.Dto;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Controls;
using BookingApp.Service;
using System.Windows.Navigation;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for Owner.xaml
    /// </summary>
    public partial class Owner : Window
    {
        public List<PropertyReservationDto> PropertyReservations { get; set; }
        public PropertyReservation SelectedReservation { get; set; }
        public User LoggedInUser { get; set; }
        public PropertyReservationRepository _propertyReservationRepository { get; set; }
        public ReviewRepository _reviewRepository;
        private List<Notification> _notifications;
        public Owner(User LoggedInUser)
        {
            InitializeComponent();
            this.LoggedInUser = LoggedInUser;
            _notifications = new List<Notification>();

            ReservationsFrame.Navigate(new ReservationsPage());
            PropertyFrame.Navigate(new PropertyPage(LoggedInUser));
            GuestReviewsFrame.Navigate(new GuestReviewsPage());

            


        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (ReservationsFrame.CanGoBack)
            {
                ReservationsFrame.GoBack();
            }
            else if (PropertyFrame.CanGoBack)
            {
                PropertyFrame.GoBack();
            }
        }

        private void NotificationsButton_Click(object sender, RoutedEventArgs e)
        {
             NotificationService notificationManager = new NotificationService();
             var unratedGuests = notificationManager.GetUnratedGuests();
             var canceledReservations = notificationManager.GetCanceledReservations();

             var allNotifications = new List<Notification>();
             allNotifications.AddRange(unratedGuests);
             allNotifications.AddRange(canceledReservations);
             NotificationWindow notificationsWindow = new NotificationWindow(allNotifications);
             notificationsWindow.ShowDialog();
           
        }
       
       

    }
}
            
                

               
            
            
            
           
        

    

