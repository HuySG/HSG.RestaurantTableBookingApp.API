using HSG.RestaurantTableBooking.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSG.RestaurantTableBookingApp.Service
{
    public interface IReservationService
    {
        Task<int> CreateOrUpdateReservationAsync(ReservationModel reservation);
        Task<bool> TimeSlotIdExistAsync(int timeSlotId);
        Task<DiningTalbeWithTimeSlotModel> CheckInReservationAsync(DiningTalbeWithTimeSlotModel reservation);
        Task<List<ReservationDetailsModel>> GetReservationDetails();
    }
}
