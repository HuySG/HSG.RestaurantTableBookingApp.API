using HSG.RestaurantTableBooking.Core.ViewModels;
using HSG.RestaurantTableBooking.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSG.RestaurantTableBooking.Data
{
    public interface IReservationRepository
    {
        Task<int> CreateOrUpdateReservationAsync(ReservationModel reservation);
        Task<TimeSlot> GetTimeSlotByIdAsync(int timeSlotId);

        Task<DiningTalbeWithTimeSlotModel> UpdateReservationAsync(DiningTalbeWithTimeSlotModel reservation);
        Task<List<ReservationDetailsModel>> GetReservationDetailsAsync();
    }
}
