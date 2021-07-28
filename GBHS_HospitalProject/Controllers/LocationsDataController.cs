﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GBHS_HospitalProject.Models;

namespace GBHS_HospitalProject.Controllers
{
    public class LocationsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// return all locations in the db
        /// </summary>
        /// <returns></returns>
        // GET: api/LocationsData
        [HttpGet]
        [ResponseType(typeof(LocationDto))]
        public IHttpActionResult ListLocations()
        {
            List<Location> Locations = db.Locations.ToList();
            List<LocationDto> locationDtos = new List<LocationDto>();

            Locations.ForEach(l => locationDtos.Add(new LocationDto()
            {
                LocationID = l.LocationID,
                LocationName = l.LocationName,
                LocationPhone = l.LocationPhone,
                LocationEmail = l.LocationEmail,
                LocationAddress = l.LocationAddress,
                LocationHasPic = l.LocationHasPic,
                PicExtension = l.PicExtension,
                Services = l.Services
            }));
            return Ok(locationDtos);
        }
        /// <summary>
        /// returns list of locations for a particular service
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(LocationDto))]
        public IHttpActionResult ListLocationsforService (int id)
        {
            List<Location> Locations = db.Locations.Where(
                    l => l.Services.Any(
                        s => s.ServiceID == id)).ToList();
            List<LocationDto> LocationDtos = new List<LocationDto>();

            Locations.ForEach(l => LocationDtos.Add(new LocationDto()
            {
                LocationID = l.LocationID,
                LocationName = l.LocationName,
                LocationPhone = l.LocationPhone,
                LocationEmail = l.LocationEmail,
                LocationAddress = l.LocationAddress,
                LocationHasPic = l.LocationHasPic,
                PicExtension = l.PicExtension,
                Services = l.Services
            }));
            return Ok(LocationDtos);
        }
        /// <summary>
        /// list of locations without the particular serviceid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(LocationDto))]
        public IHttpActionResult ListLocationsWithoutService(int id)
        {
            List<Location> Locations = db.Locations.Where(
                    l => !l.Services.Any(
                        s => s.ServiceID == id)).ToList();
            List<LocationDto> LocationDtos = new List<LocationDto>();

            Locations.ForEach(l => LocationDtos.Add(new LocationDto()
            {
                LocationID = l.LocationID,
                LocationName = l.LocationName,
                LocationPhone = l.LocationPhone,
                LocationEmail = l.LocationEmail,
                LocationAddress = l.LocationAddress,
                LocationHasPic = l.LocationHasPic,
                PicExtension = l.PicExtension,
                Services = l.Services
            }));
            return Ok(LocationDtos);
        }
        /// <summary>
        /// returns details for a particular location
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/LocationsData/5
        [ResponseType(typeof(LocationDto))]
        [HttpGet]
        public IHttpActionResult FindLocation(int id)
        {
            Location location = db.Locations.Find(id);
            LocationDto LocationDto = new LocationDto()
            {
                LocationID = location.LocationID,
                LocationName = location.LocationName,
                LocationPhone = location.LocationPhone,
                LocationEmail = location.LocationEmail,
                LocationAddress = location.LocationAddress,
                LocationHasPic = location.LocationHasPic,
                PicExtension = location.PicExtension,
                Services = location.Services
            };
            if (location == null)
            {
                return NotFound();
            }

            return Ok(LocationDto);
        }
        /// <summary>
        /// updates a particular location information
        /// </summary>
        /// <param name="id"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        // PUT: api/LocationsData/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateLocation(int id, Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != location.LocationID)
            {
                return BadRequest();
            }

            db.Entry(location).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
        /// <summary>
        /// adds a new location to the database
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        // POST: api/LocationsData
        [ResponseType(typeof(Location))]
        [HttpPost]
        public IHttpActionResult AddLocation(Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Locations.Add(location);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = location.LocationID }, location);
        }
        /// <summary>
        /// deletes a location given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/LocationsData/5
        [ResponseType(typeof(Location))]
        [HttpPost]
        public IHttpActionResult DeleteLocation(int id)
        {
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return NotFound();
            }

            db.Locations.Remove(location);
            db.SaveChanges();

            return Ok(location);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LocationExists(int id)
        {
            return db.Locations.Count(e => e.LocationID == id) > 0;
        }
    }
}