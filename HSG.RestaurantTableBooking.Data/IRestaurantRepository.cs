using HSG.RestaurantTableBooking.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSG.RestaurantTableBooking.Data
{
    public interface IRestaurantRepository
    {
        Task<List<RestaurantModel>> GetAllRestaurantAsync();
        Task<IEnumerable<RestaurantBranchModel>> GetRestaurantBranchesByRestaurantIdAsync(int restaurantId);
        Task<IEnumerable<DiningTalbeWithTimeSlotModel>> GetDiningTalbesByBranchAsync(int branchId, DateTime date);
        Task<IEnumerable<DiningTalbeWithTimeSlotModel>> GetDiningTalbesbyBranchAsync(int branchId);
    }
}
