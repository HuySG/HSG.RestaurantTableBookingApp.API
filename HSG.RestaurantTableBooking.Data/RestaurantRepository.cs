﻿using HSG.RestaurantTableBooking.Core.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSG.RestaurantTableBooking.Data
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly RestaurantTableBookingDbContext _db;
        public RestaurantRepository(RestaurantTableBookingDbContext dbContext)
        {
            _db = dbContext;
        }
        public  Task<List<RestaurantModel>> GetAllRestaurantAsync()
        {
            
           var restaurants = _db.Restaurants
                .OrderBy(x => x.Name)
                .Select(r => new RestaurantModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Address = r.Address,
                    Phone = r.Phone,
                    Email = r.Email,
                    ImageUrl = r.ImageUrl,
                }).ToListAsync(); 
            return restaurants;
        }

        public async Task<IEnumerable<DiningTalbeWithTimeSlotModel>> GetDiningTalbesByBranchAsync(int branchId, DateTime date)
        {
            var diningTables = await _db.DiningTables
                .Where(dt => dt.RestaurantBranchId == branchId)
                .SelectMany(dt => dt.TimeSlots, (dt, ts) => new
                {
                    dt.RestaurantBranchId,
                    dt.TableName,
                    dt.Capacity,
                    ts.ReservationDay,
                    ts.MealType,
                    ts.TableStatus,
                    ts.Id,
                }).Where(ts => ts.ReservationDay.Date == date.Date)
                .OrderBy(ts=> ts.Id)
                .ThenBy(ts => ts.MealType)
                .ToListAsync();
            return diningTables.Select(dt => new DiningTalbeWithTimeSlotModel
            {
                BranchId = dt.RestaurantBranchId,
                ReservationDay = dt.ReservationDay,
                TableName = dt.TableName,
                Capactiy = dt.Capacity,
                MealType = dt.MealType,
                TableStatus = dt.TableStatus,
                TimeSlotId = dt.Id,
            });
        }

        public async Task<IEnumerable<DiningTalbeWithTimeSlotModel>> GetDiningTalbesbyBranchAsync(int branchId)
        {
            var data = await(
                from rb in _db.RestaurantBranches
                join dt in _db.DiningTables on rb.Id equals dt.RestaurantBranchId
                join ts in _db.TimeSlots on dt.Id equals ts.DiningTableId
                where dt.RestaurantBranchId == branchId && ts.ReservationDay >= DateTime.Now.Date
                orderby ts.Id, ts.MealType
                select new DiningTalbeWithTimeSlotModel()
                {
                    BranchId = rb.Id,
                    Capactiy = dt.Capacity,
                    TableName= dt.TableName,
                    MealType = ts.MealType,
                    ReservationDay = ts.ReservationDay,
                    TableStatus = ts.TableStatus,
                    TimeSlotId = ts.Id,
                }).ToListAsync();
            return data;
        }

        public async Task<IEnumerable<RestaurantBranchModel>> GetRestaurantBranchesByRestaurantIdAsync(int restaurantId)
        {
            var branchs = await _db.RestaurantBranches
                .Where(rb => rb.RestaurantId == restaurantId)
                .Select(rb => new RestaurantBranchModel
                {
                    Id =  rb.Id,
                    RestaurantId = rb.RestaurantId,
                    Name=rb.Name,
                    Address = rb.Address,
                    Phone = rb.Phone,
                    Email = rb.Email,
                    ImageUrl = rb.ImageUrl,
                }).ToListAsync();
            return branchs;
        }
    }
}