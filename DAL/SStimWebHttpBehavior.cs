using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace SStimStat.DAL
{
    public class SStimWebHttpBehavior: WebHttpBehavior
    {
        protected override QueryStringConverter GetQueryStringConverter(OperationDescription operationDescription)
        {
            return new JsonQueryStringConverter();
        }
    }
}