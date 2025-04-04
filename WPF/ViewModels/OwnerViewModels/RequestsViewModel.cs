﻿using BookingApp.Aplication;
using BookingApp.Aplication.UseCases;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using BookingApp.Repositories;
using BookingApp.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModels.OwnerViewModel
{
    public class RequestsViewModel
    {
        public ObservableCollection<ReservationChangeRequest> ReservationChangeRequests { get; set; }
        private readonly ChangeRequestService _requestsService;
        private readonly PropertyReservationService _reservationService;

        public RequestsViewModel()
        {
            _requestsService = new ChangeRequestService(Injector.CreateInstance<IReservationChangeRequestRepository>());
            _reservationService = new PropertyReservationService(Injector.CreateInstance<IPropertyRepository>(), Injector.CreateInstance<IPropertyReservationRepository>(), Injector.CreateInstance<IReservedDateRepository>());


            ReservationChangeRequests = new ObservableCollection<ReservationChangeRequest>();

            LoadReservationChangeRequests();
        }

        public void Accept(ReservationChangeRequest request)
        {
            _reservationService.UpdateReservation(request);
            _requestsService.UpdateChangeRequestStatus(request.Id, RequestStatus.Approved);
            ReservationChangeRequests.Remove(request);

        }
        private void LoadReservationChangeRequests()
        {
            var requests = _requestsService.GetAllRequests();

            foreach (var request in requests)
            {
                if (request.RequestStatus == RequestStatus.Processing)
                {
                    ReservationChangeRequests.Add(request);
                }
            }

            CheckAvailabilityForAllRequests();
        }

        private void CheckAvailabilityForAllRequests()
        {
            var availabilityStatus = _reservationService.CheckAvailabilityForAllRequests(ReservationChangeRequests);

            for (int i = 0; i < ReservationChangeRequests.Count; i++)
            {
                ReservationChangeRequests[i].Status = availabilityStatus[i];
            }
        }

    }
}
