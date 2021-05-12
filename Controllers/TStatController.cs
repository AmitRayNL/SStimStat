using SStimStat.BAL;
using SStimDataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SStimStat.Controllers
{
    public class TStatController : ApiController
    {
        [Route("api/TStat/GetPValue")]
        [HttpPost]
        public double GetPValue(PValueInputTTable pValueInput)
        {
            TableLookupT tableLookupT = new TableLookupT();
            return tableLookupT.GetPValue(pValueInput.TValue, pValueInput.Df, pValueInput.TailProp);
        }

        [Route("api/TStat/GetCValue")]
        [HttpPost]
        public LookupResult GetCValue(PValueInputTTable pValueInput)
        {
            TableLookupT tableLookupT = new TableLookupT();
            return tableLookupT.GetCValue(pValueInput.TValue, pValueInput.Df);
        }

        [Route("api/TStat/GetTValue")]
        [HttpPost]
        public double GetTValue(TValueInputTTable tValueInputTTable)
        {
            TableLookupT tableLookupT = new TableLookupT();
            return tableLookupT.GetTValue(
                tValueInputTTable.SampleSize,
                tValueInputTTable.SampleMean,
                tValueInputTTable.PopulationMean,
                tValueInputTTable.SampleSd);
        }

        [Route("api/TStat/GetCriticalTValue")]
        [HttpPost]
        public double GetCriticalTValue(CritValueInputTTable critValueInputTTable)
        {
            TableLookupT tableLookupT = new TableLookupT();
            return tableLookupT.GetCriticalTValue(
                critValueInputTTable.ReqdConfidenceLevel,
                critValueInputTTable.Df);
        }

        [Route("api/TStat/GetValuesInTheRequiredCI")]
        [HttpPost]
        public LookupResult GetValuesInTheRequiredCI(
            ValueRangeWithinCiInputTTable valueRangeWithinCiInputTTable)
        {
            TableLookupT tableLookupT = new TableLookupT();
            return tableLookupT.GetValuesInTheRequiredCI(
                valueRangeWithinCiInputTTable.SampleSize,
                valueRangeWithinCiInputTTable.SampleMean,
                valueRangeWithinCiInputTTable.SampleSd,
                valueRangeWithinCiInputTTable.ReqdConfidenceLevel);
        }

        [Route("api/TStat/GetPValueFromHypothesis")]
        [HttpPost]
        public double GetPValueFromHypothesis(HypothesisPValueInputTTable hypothesisPValueInputTTable)
        {
            TableLookupT tableLookupT = new TableLookupT();

            var tValue = tableLookupT.GetTValue(
                hypothesisPValueInputTTable.SampleSize,
                hypothesisPValueInputTTable.SampleMean,
                hypothesisPValueInputTTable.PopulationMean,
                hypothesisPValueInputTTable.SampleSd);

            return tableLookupT.GetPValue(tValue, hypothesisPValueInputTTable.SampleSize - 1, hypothesisPValueInputTTable.TailProp);
        }

        [Route("api/TStat/TestMethod")]
        [HttpPost]
        public bool TestMethod(TestData testData)
        {
            return true;
        }

    }
}