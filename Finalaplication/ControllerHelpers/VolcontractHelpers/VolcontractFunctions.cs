using Finalaplication.ControllerHelpers.UniversalHelpers;
using Finalaplication.Models;
using System;
using System.Collections.Generic;

namespace Finalaplication.ControllerHelpers.VolcontractHelpers
{
    public class VolcontractFunctions
    {
        internal static List<Volcontract> GetExpiringContracts(List<Volcontract> volcontracts)
        {
            int currentday = UniversalFunctions.GetDayOfYear(DateTime.Today);
            List<Volcontract> returnlistofvolcontracts = new List<Volcontract>();
            foreach (var volcontract in volcontracts)
            {
                int daytocompare = UniversalFunctions.GetDayOfYear(volcontract.ExpirationDate);
                if (UniversalFunctions.IsAboutToExpire(currentday, daytocompare))
                {
                    returnlistofvolcontracts.Add(volcontract);
                }
            }
            return returnlistofvolcontracts;
        }
    }
}