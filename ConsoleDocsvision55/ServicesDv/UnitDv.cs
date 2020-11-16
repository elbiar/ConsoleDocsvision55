using System;
using System.Collections.Generic;

using DocsVision.BackOffice.ObjectModel;
using DocsVision.BackOffice.ObjectModel.Services;

namespace ConsoleDocsvision55.ServicesDv
{
    class UnitDv
    {
        public void Get(Guid idUnit)
        {
            //через API
            IStaffService staffService = DvConnection.ObjectContext.GetService<IStaffService>();
            var staffUnit = DvConnection.ObjectContext.GetObject<StaffUnit>(idUnit);
            IEnumerable<StaffEmployee> employees = staffService.GetUnitEmployees(staffUnit, true, false);
            //через object
        }
    }
}
