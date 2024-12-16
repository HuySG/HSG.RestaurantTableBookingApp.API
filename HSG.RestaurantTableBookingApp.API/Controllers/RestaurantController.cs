using HSG.RestaurantTableBooking.Core.ViewModels;
using HSG.RestaurantTableBookingApp.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HSG.RestaurantTableBookingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]//the end points from this controller is used by any user without login so it should be ananymous

    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IReservationService _reservationService;
        private readonly IEmailNotification emailNotification;

        public RestaurantController(IRestaurantService restaurantService, IReservationService reservationService, IEmailNotification emailNotification)
        {
            _restaurantService = restaurantService;
            this._reservationService = reservationService;
            this.emailNotification = emailNotification;
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

        [HttpGet("getreservations")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReservationDetailsModel>))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ReservationDetailsModel>>> GetReservationDetails(int branchId, DateTime date)
        {
            var reservations = await _reservationService.GetReservationDetails();

            return Ok(reservations);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ReservationModel))]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ReservationModel>> CreateReservationAsync(ReservationModel reservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the selected time slot exists
            var timeSlot = await _reservationService.TimeSlotIdExistAsync(reservation.TimeSlotId);
            if (!timeSlot)
            {
                return NotFound("Selected time slot not found.");
            }

            // Create a new reservation
            var newReservation = new ReservationModel
            {
                UserId = reservation.UserId,
                FirstName = reservation.FirstName,
                LastName = reservation.LastName,
                EmailId = reservation.EmailId,
                PhoneNumber = reservation.PhoneNumber,
                TimeSlotId = reservation.TimeSlotId,
                ReservationDate = reservation.ReservationDate,
                ReservationStatus = reservation.ReservationStatus
            };

            var createdReservation = await _reservationService.CreateOrUpdateReservationAsync(newReservation);
            await emailNotification.SendBookingEmailAsync(reservation);

            return new CreatedResult("GetReservation", new { id = createdReservation });
        }

    }
}
