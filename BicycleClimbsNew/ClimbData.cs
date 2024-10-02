
namespace BicycleClimbsSilverlight.Web
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;

    // TODO: Create methods containing your application logic.
    [EnableClientAccess()]
    public class ClimbData : DomainService
    {
        public string GetHelloValue(string item)
        {
            return "Hello, " + item;
        }

        public List<ClimbPathElevation> GetClimbPathElevation(int climbId)
        {
            using (BicycleClimbsDataDataContext context = new BicycleClimbsDataDataContext())
            {
                var result =
                    from c in context.ClimbPathElevations
                    where c.ClimbId == climbId
                    select c;

                List<ClimbPathElevation> resultList = result.ToList<ClimbPathElevation>();

                return resultList;
            }
        }

        public List<Climb> GetClimbs(int regionId)
        {
            using (BicycleClimbsDataDataContext context = new BicycleClimbsDataDataContext())
            {
                var result =
                    from c in context.Climbs
                    where c.RegionId == regionId
                    select c;

                List<Climb> resultList = result.ToList<Climb>();

                return resultList;
            }
        }

        public List<Region> GetRegions()
        {
            using (BicycleClimbsDataDataContext context = new BicycleClimbsDataDataContext())
            {
                var result =
                    from r in context.Regions
                    select r;

                List<Region> resultList = result.ToList<Region>();

                return resultList;
            }
        }
    }
}


