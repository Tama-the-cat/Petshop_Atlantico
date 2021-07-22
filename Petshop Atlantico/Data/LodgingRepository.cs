﻿using Petshop_Atlantico.Enums;
using Petshop_Atlantico.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Petshop_Atlantico.Data
{
    public class LodgingRepository : ILodgingRepo
    {
        private readonly PetshopContext _context;

        public LodgingRepository(PetshopContext context)
        {
            _context = context;
        }
        
        public IEnumerable<int> GetUnnocupiedList()
        {
            return _context.Lodgings.Where(p => p.OccupationStatus == 0).Select(p =>  p.Id ).ToList();
        }

        public Lodging GetLodgingById(int id)
        {
            try
            {
                return _context.Lodgings.SingleOrDefault(p => p.Id == id);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Lodging> GetList()
        {
            return _context.Lodgings.ToList();
        }

        public Lodging GetLodgingByOccupant(int id)
        {
            try
            {
                return _context.Lodgings.SingleOrDefault(p => p.Occupant.Id == id);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateOccupationStatus(int id, OccupationStatus occupationStatus)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                Lodging lodging = GetLodgingById(id);

                lodging.OccupationStatus = occupationStatus;

                if (occupationStatus == OccupationStatus.Free)
                    lodging.Occupant = null;

                _context.Lodgings.Update(lodging);
                _context.SaveChanges();
                transaction.Commit();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
    }
}