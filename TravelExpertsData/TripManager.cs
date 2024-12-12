using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelExpertsData.Models;
using TravelExpertsData.ViewModels;

namespace TravelExpertsData
{
    public class TripManager
    {
        //get all tripTypes
        public static List<TripType> GetTripTypes(TravelExpertsContext db)
        {
            return db.TripTypes.ToList();

        }
        //get trip type by Id
        public static TripType GetTripTypeById(TravelExpertsContext db, string tripId)
        {
            TripType trip = db.TripTypes.FirstOrDefault(p => p.TripTypeId == tripId);
            return trip;
            
        }
    }
}
