using SStimStat.BAL;
using SStimDataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SStimStat.Controllers
{
    public class ZStatController : ApiController
    {
        [Route("api/ZStat/GetPValue")]
        [HttpPost]
        public double GetPValue(PValueInputZTable pValueInput)
        {
            TableLookupZ tableLookupZ = new TableLookupZ();
            return tableLookupZ.GetPValue(pValueInput.ZValue, pValueInput.TailProp);
        }

        [Route("api/ZStat/GetValuesInTheRequiredCI")]
        [HttpPost]
        public LookupResult GetValuesInTheRequiredCI(
            ValueRangeWithinCiInputZTable valueRangeWithinCiInputZTable)
        {
            TableLookupZ tableLookupZ = new TableLookupZ();
            return tableLookupZ.GetValuesInTheRequiredCI(
                valueRangeWithinCiInputZTable.SampleSize,
                valueRangeWithinCiInputZTable.SampleProportion,
                valueRangeWithinCiInputZTable.ReqdConfidenceLevel);
        }

        [Route("api/ZStat/GetPValueFromHypothesis")]
        [HttpPost]
        public double GetPValueFromHypothesis(HypothesisPValueInputZTable hypothesisPValueInputZTable)
        {
            TableLookupZ tableLookupZ = new TableLookupZ();

            var zValue = tableLookupZ.GetZValue(
                hypothesisPValueInputZTable.SampleSize,
                hypothesisPValueInputZTable.SampleProportion,
                hypothesisPValueInputZTable.PopulationProportion);

            return tableLookupZ.GetPValue(zValue, hypothesisPValueInputZTable.TailProp);
        }


    }
}