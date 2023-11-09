﻿using EnergyLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergySim
{
    public class EnergyState
    {
        public int Rows { get; }
        public int Cols { get; }
        public LandValue[,] LandGrid { get; }

        public EnergyState(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            LandGrid = new LandValue[rows, cols];
            AddStructure();
        }

        private void AddStructure()
        {
            LandGrid[3, 19] = LandValue.Turbine;
            LandGrid[5, 18] = LandValue.Biomass;
        }

        
    }
}