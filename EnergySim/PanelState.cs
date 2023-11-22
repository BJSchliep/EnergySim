using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergySim
{
    public class PanelState
    {
        
        public int Rows { get; }
        public int Cols { get; }
        public LandValue[,] Panel { get; }

        public PanelState(int rows, int cols) 
        {
            Rows = rows;
            Cols = cols;
            Panel = new LandValue[rows, cols];
            AddStructureToPanel();
        }

        private void AddStructureToPanel()
        {
            Panel[0, 0] = LandValue.Biomass;
            Panel[1, 0] = LandValue.Geothermal;
            Panel[2, 0] = LandValue.Hydroelectric;
            Panel[3, 0] = LandValue.Nuclear;
            Panel[4, 0] = LandValue.Solar;
            Panel[5, 0] = LandValue.Turbine;
            Panel[6, 0] = LandValue.Water;
            Panel[7, 0] = LandValue.Business;
            Panel[8, 0] = LandValue.House;
        }

       /* private void AddStructureToGrid(LandValue selectedLandValue)
        {
            for (int r = 0; r < 20; r++)
            {
                for (int c = 0; c < 20; c++)
                {
                    if (energyState.LandGrid[r, c] == LandValue.Empty)
                    {
                        energyState.LandGrid[r, c] = selectedLandValue;
                        gridImages[r, c].Source = gridValToImage[selectedLandValue];
                        gridImages[r, c].Tag = selectedLandValue;

                        return;
                    }
                }
            }
        }*/
    }
}
