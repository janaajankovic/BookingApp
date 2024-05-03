﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Navigation;
using BookingApp.View.GuideView;
using BookingApp.Domain.Models;
using BookingApp.Repositories;
using BookingApp.View;
using BookingApp.Command;
using System.Linq;
using BookingApp.Aplication.UseCases;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Aplication;
using LiveCharts;
using LiveCharts.Wpf;


namespace BookingApp.WPF.ViewModels.GuidesViewModel
{
    internal class TouristsNumberPageViewModel : BaseViewModel
    {
        private Tour selectedTour;
        private int tourId;
        private int under18Count;
        private int between18And50Count;
        private int over50Count;
        private readonly TouristService touristService;
        private readonly TourService tourService;
        private readonly TourReservationService tourReservationService;
        private readonly TouristExperienceService touristExperienceService;
        private RelayCommand navigateHomeCommand;
        private RelayCommand navigateBackCommand;
        private RelayCommand sideMenuCommand;

        public TouristsNumberPageViewModel(int tourId)
        {
            this.tourId = tourId;
            tourService = new TourService(Injector.CreateInstance<ITourRepository>(), Injector.CreateInstance<ILiveTourRepository>());
            SelectedTour=tourService.GetTourById(tourId);
            touristExperienceService = new TouristExperienceService(Injector.CreateInstance<ITouristExperienceRepository>());
            touristService = new TouristService(Injector.CreateInstance<ITouristRepository>());
            tourReservationService = new TourReservationService(Injector.CreateInstance<ITourReservationRepository>());
            navigateHomeCommand = new RelayCommand(ExecuteNavigateHome);
            navigateBackCommand = new RelayCommand(ExecuteNavigateBack);
            sideMenuCommand = new RelayCommand(ExecuteSideMenuClick);
            CountTouristsByAge();


            HistogramData = new SeriesCollection
{
    new ColumnSeries
    {
        Title = "Under 18",
        Values = new ChartValues<int> { Under18Count }
    },
    new ColumnSeries
    {
        Title = "Between 18 and 50",
        Values = new ChartValues<int> { Between18And50Count }
    },
    new ColumnSeries
    {
        Title = "Over 50",
        Values = new ChartValues<int> { Over50Count }
    }
};

            Labels = new[] { "Age Groups" };

        }


       
        public Tour SelectedTour
        {
            get { return selectedTour; }
            set
            {
                if (selectedTour != value)
                {
                    selectedTour = value;
                    OnPropertyChanged(nameof(SelectedTour));
                }
            }
        }

        public int Under18Count
        {
            get { return under18Count; }
            set
            {
                if (under18Count != value)
                {
                    under18Count = value;
                    OnPropertyChanged(nameof(Under18Count));
                }
            }
        }
        


        public int Between18And50Count
        {
            get { return between18And50Count; }
            set
            {
                if (between18And50Count != value)
                {
                    between18And50Count = value;
                    OnPropertyChanged(nameof(Between18And50Count));
                }
            }
        }

        public int Over50Count
        {
            get { return over50Count; }
            set
            {
                if (over50Count != value)
                {
                    over50Count = value;
                    OnPropertyChanged(nameof(Over50Count));
                }
            }
        }

        private void CountTouristsByAge()
        {

            var tourists = tourReservationService.GetByTourId(tourId).Where(t => !string.IsNullOrWhiteSpace(t.JoinedKeyPoint.Name));

            foreach (var tourist in tourists)
            {

                int age = tourist.TouristAge;
                if (age < 18)
                {
                    Under18Count++;
                }
                else if (age >= 18 && age <= 50)
                {
                    Between18And50Count++;
                }
                else
                {
                    Over50Count++;
                }
            }
        }
        public RelayCommand NavigateHomeCommand
        {
            get { return navigateHomeCommand; }
            set
            {
                if (navigateHomeCommand != value)
                {
                    navigateHomeCommand = value;
                    OnPropertyChanged();
                }
            }
        }
        public RelayCommand NavigateBackCommand
        {
            get { return navigateBackCommand; }
            set
            {
                if (navigateBackCommand != value)
                {
                    navigateBackCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private void ExecuteNavigateHome()
        {
            var mainPage = new GuideMainPage();
            GuideMainWindow.MainFrame.Navigate(mainPage);

        }

        private void ExecuteNavigateBack()
        {
            var tourStatisticPage = new TourStatistic();
            GuideMainWindow.MainFrame.Navigate(tourStatisticPage);
        }


        public RelayCommand SideManuCommand
        {
            get { return sideMenuCommand; }
            set
            {
                if (sideMenuCommand != value)
                {
                    sideMenuCommand = value;
                    OnPropertyChanged();
                }
            }
        }

        private void ExecuteSideMenuClick()
        {

            var sideMenuPage = new SideMenuPage();
            GuideMainWindow.MainFrame.Navigate(sideMenuPage);

        }


        public SeriesCollection HistogramData { get; set; }
        public string[] Labels { get; set; }





    }
}
