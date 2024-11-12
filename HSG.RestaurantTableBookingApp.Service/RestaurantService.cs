using HSG.RestaurantTableBooking.Core.ViewModels;
using HSG.RestaurantTableBooking.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSG.RestaurantTableBookingApp.Service
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository restaurantRepository;
        public RestaurantService(IRestaurantRepository restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
        }
        public Task<List<RestaurantModel>> GetAllRestaurantAsync()
        {
            return restaurantRepository.GetAllRestaurantAsync();
        }

        public async Task<IEnumerable<DiningTalbeWithTimeSlotModel>> GetDiningTalbesByBranchAsync(int branchId, DateTime date)
        {
            return await restaurantRepository.GetDiningTalbesByBranchAsync(branchId,date);
        }

        public async Task<IEnumerable<DiningTalbeWithTimeSlotModel>> GetDiningTalbesbyBranchAsync(int branchId)
        {
            return await restaurantRepository.GetDiningTalbesbyBranchAsync(branchId);
        }

        public async Task<IEnumerable<RestaurantBranchModel>> GetRestaurantBranchesByRestaurantIdAsync(int restaurantId)
        {
            return await restaurantRepository.GetRestaurantBranchesByRestaurantIdAsync(restaurantId);
        }
    }
}
