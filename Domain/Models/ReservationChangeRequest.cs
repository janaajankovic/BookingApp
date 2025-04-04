﻿using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Models
{
    public enum RequestStatus { Approved, Declined, Processing };
    public class ReservationChangeRequest : ISerializable
    {
        public int Id { get; set; }

        public int ReservationId { get; set; }
        public DateTime OldStartDate { get; set; }
        public DateTime OldEndDate { get; set; }
        public DateTime NewStartDate { get; set; }
        public DateTime NewEndDate { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public int GuestId { get; set; }
        public string PropertyName { get; set; }
        public string Comment { get; set; }

        public string Status { get; set; }
        public ReservationChangeRequest()
        {
            RequestStatus = RequestStatus.Processing;
            //Status = "Occupied";
        }

        public ReservationChangeRequest(int id)
        {
            Id = id;
        }

        public ReservationChangeRequest(int id, int reservationId, DateTime oldStartDate, DateTime oldEndDate, DateTime newStartDate, DateTime newEndDate, RequestStatus status, int guestId, string propertyName, string comment)
        {
            Id = id;
            ReservationId = reservationId;
            OldStartDate = oldStartDate;
            OldEndDate = oldEndDate;
            NewStartDate = newStartDate;
            NewEndDate = newEndDate;
            RequestStatus = status;
            GuestId = guestId;
            PropertyName = propertyName;
            Comment = comment;

        }
        public ReservationChangeRequest(int id, int reservationId, DateTime oldStartDate, DateTime oldEndDate, DateTime newStartDate, DateTime newEndDate, RequestStatus status, int guestId, string propertyName, string comment, string status2)
        {
            Id = id;
            ReservationId = reservationId;
            OldStartDate = oldStartDate;
            OldEndDate = oldEndDate;
            NewStartDate = newStartDate;
            NewEndDate = newEndDate;
            RequestStatus = status;
            GuestId = guestId;
            PropertyName = propertyName;
            Comment = comment;
            Status = status2;
        }

        public ReservationChangeRequest(int reservationId, DateTime oldStartDate, DateTime oldEndDate, DateTime newStartDate, DateTime newEndDate, RequestStatus status, int guestId, string propertyName, string comment, string status2)
        {
            ReservationId = reservationId;
            OldStartDate = oldStartDate;
            OldEndDate = oldEndDate;
            NewStartDate = newStartDate;
            NewEndDate = newEndDate;
            RequestStatus = status;
            GuestId = guestId;
            PropertyName = propertyName;
            Comment = comment;

            Status = status2;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), ReservationId.ToString(), OldStartDate.ToString("dd.MM.yyyy HH:mm:ss"), OldEndDate.ToString("dd.MM.yyyy HH:mm:ss"), NewStartDate.ToString("dd.MM.yyyy HH:mm:ss"), NewEndDate.ToString("dd.MM.yyyy HH:mm:ss"), RequestStatus.ToString(), GuestId.ToString(), PropertyName, Comment };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            ReservationId = Convert.ToInt32(values[1]);
            OldStartDate = DateTime.ParseExact(values[2], "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            OldEndDate = DateTime.ParseExact(values[3], "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            NewStartDate = DateTime.ParseExact(values[4], "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            NewEndDate = DateTime.ParseExact(values[5], "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            RequestStatus = (RequestStatus)Enum.Parse(typeof(RequestStatus), values[6]);
            GuestId = Convert.ToInt32(values[7]);
            PropertyName = values[8];
            Comment = values[9];
        }
    }
}
