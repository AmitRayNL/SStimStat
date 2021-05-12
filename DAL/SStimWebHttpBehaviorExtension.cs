using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Configuration;

namespace SStimStat.DAL
{
    public class SStimWebHttpBehaviorExtension : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get { return typeof(SStimWebHttpBehavior); }
        }

        protected override object CreateBehavior()
        {
            return new SStimWebHttpBehavior();
        }
    }
}