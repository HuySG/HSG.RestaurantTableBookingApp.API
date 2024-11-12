using HSG.RestaurantTableBooking.Core.ViewModels;
using HSG.RestaurantTableBookingApp.Service;
using Microsoft.AspNetCore.Mvc;

namespace HSG.RestaurantTableBookingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }
        [HttpGet("restaurants")]
        [ProducesResponseType(200, Type =typeof(List<RestaurantModel>))]
        public async Task<ActionResult > GetAllRestaurant()
        {
            var restaurants = await _restaurantService.GetAllRestaurantAsync();
            return Ok(restaurants);
        }

        [HttpGet("branches/{restaurantId}")]
        [ProducesResponseType(200,Type = typeof(IEnumerable<RestaurantBranchModel>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<RestaurantBranchModel>>> GetRestaurantBranchsByRestaurantIdAsync(int restaurantId)
        {
            var branches = await _restaurantService.GetRestaurantBranchesByRestaurantIdAsync(restaurantId);
            if (branches == null)
            {
                return NotFound();
            }
            return Ok(branches); 
        }
        [HttpGet("diningtables/{branchId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<DiningTalbeWithTimeSlotModel>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<DiningTalbeWithTimeSlotModel>>> GetDiningTableByBranchAsync(int branchId)
        {
            var diningTables = await _restaurantService.GetDiningTalbesbyBranchAsync(branchId);
            if (diningTables == null)
            {
                return NotFound();
            }
            return Ok(diningTables);
        }

        [HttpGet("diningtables/{branchId}/{date}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<DiningTalbeWithTimeSlotModel>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<DiningTalbeWithTimeSlotModel>>> GetDiningTableByBranchAndDateAsync(int branchId, DateTime date)
        {
            var diningTables = await _restaurantService.GetDiningTalbesByBranchAsync(branchId,date);
            if (diningTables == null)
            {
                return NotFound();
            }
            return Ok(diningTables);
        }





    }
}
