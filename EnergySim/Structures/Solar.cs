using EnergySim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyLogic
{
    public class Solar : Structure
    {
        public override LandValue Type => LandValue.Solar;
    }
}
