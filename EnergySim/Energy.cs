using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergySim
{
    public class Energy
    {
        private readonly Dictionary<LandValue, int> gridEnergy = new()
        {
            { LandValue.Empty, 0 },
            { LandValue.Nuclear, 5 },
            { LandValue.Turbine, 3 },
            { LandValue.House, -10 },
            { LandValue.Business, -15 },
            { LandValue.Geothermal, 6 },
            { LandValue.Hydroelectric, 4 },
            { LandValue.Solar, 1 },
            { LandValue.Biomass, 2 },
            { LandValue.Water, 0 }
        };

        public int totalEnergy;

        public LandValue LandValue;

        public Energy(int energy)
        {
            totalEnergy = energy;
        }

        public void AddEnergy(LandValue landValue)
        {
            totalEnergy += gridEnergy[landValue];
        }

        public void SubtractEnergy(LandValue landValue)
        {
            totalEnergy -= gridEnergy[landValue];
        }

        public int GetEnergy()
        {
            return totalEnergy;
        }

        public bool NotEnoughEnergy()
        {
            return totalEnergy < 0;
        }
    }
}
