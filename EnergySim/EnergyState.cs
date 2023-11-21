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
        }
    }
}
