﻿using BookingApp.Repositories;
using BookingApp.View.TouristView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using BookingApp.Domain.Models;
using BookingApp.Aplication.UseCases;
using BookingApp.Aplication.Dto;
using BookingApp.Aplication;
using BookingApp.Domain.RepositoryInterfaces;

namespace BookingApp.WPF.ViewModels.TouristViewModels
{
    public class ShowAndSearchToursViewModel : BindableBase
    {
        public static ObservableCollection<TourDto> Tours { get; set; }
        public User LoggedInUser { get; set; }
        public TourDto SelectedTour { get; set; }

        private int unreadNotificationCount;
        public int UnreadNotificationCount
        {
            get { return unreadNotificationCount; }
            set
            {
                unreadNotificationCount = value;
                OnPropertyChanged(nameof(UnreadNotificationCount));
            }
        }

        private readonly TourService tourService;
        private readonly KeyPointService keyPointService;
        private readonly TouristGuideNotificationService notificationService;

        private bool _isCancelSearchButtonVisible;

        public ShowAndSearchToursViewModel(User loggedInUser)
        {
            tourService = new TourService(Injector.CreateInstance<ITourRepository>(), Injector.CreateInstance<ILiveTourRepository>());
            keyPointService = new KeyPointService(Injector.CreateInstance<IKeyPointRepository>(), Injector.CreateInstance<ILiveTourRepository>());
            notificationService = new TouristGuideNotificationService(Injector.CreateInstance<ITouristGuideNotificationRepository>());
            Tours = new ObservableCollection<TourDto>();
            SelectedTour = new TourDto();

            IsCancelSearchButtonVisible = false;
            LoggedInUser = loggedInUser;
            GetAllTours();
            UnreadNotificationCount = notificationService.GetUnreadNotificationCount(LoggedInUser.Id);
        }

        public bool IsCancelSearchButtonVisible
        {
            get => _isCancelSearchButtonVisible;
            set
            {
                if (value != _isCancelSearchButtonVisible)
                {
                    _isCancelSearchButtonVisible = value;
                    OnPropertyChanged("IsCancelSearchButtonVisible");
                }
            }
        }

        public void GetAllTours()
        {
            Tours.Clear();
            foreach (var tour in tourService.GetAllSorted())
            {
                Tours.Add(new TourDto(tour));
            }
        }

        public void SelectedTourCard(object sender)
        {
            Border border = (Border)sender;
            SelectedTour = (TourDto)border.DataContext;
            SelectedTour.KeyPoints = keyPointService.GetTourKeyPoints(SelectedTour.Id);
            if (SelectedTour.MaxTouristNumber > 0)
            {
                TourBookingWindow tourBookingWindow = new TourBookingWindow(SelectedTour, LoggedInUser.Id);
                tourBookingWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("The tour is fully booked. Please select an alternative tour from this city.");
                ShowUnbookedToursInCity();
            }
        }

        public void Search()
        {
            SearchWindow searchWindow = new SearchWindow(Tours);
            searchWindow.ShowDialog();

            IsCancelSearchButtonVisible = searchWindow.searchViewModel.IsCancelSearchButtonVisible;
        }

        public void OpenInbox()
        {
            NotificationsWindow notificationsWindow = new NotificationsWindow(LoggedInUser);
            notificationsWindow.ShowDialog();
            UnreadNotificationCount = notificationService.GetUnreadNotificationCount(LoggedInUser.Id);
        }

        public void ShowAllTours()
        {
            IsCancelSearchButtonVisible = false;
            GetAllTours();
        }

        private void ShowUnbookedToursInCity()
        {
            List<Tour> unBookedToursInCity = tourService.GetUnBookedToursInCity(SelectedTour.LocationDto.City);

            if (unBookedToursInCity.Count > 0)
            {
                IsCancelSearchButtonVisible = true;
                Tours.Clear();
                foreach (var tour in unBookedToursInCity)
                {
                    Tours.Add(new TourDto(tour));
                }
            }
            else
            {
                MessageBox.Show("There are no tours from that city");
            }
        }

        private void HelpButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
