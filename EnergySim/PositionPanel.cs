using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergySim
{
    public class PositionPanel
    {
        public int Row { get; }
        public int Column { get; }
        public PositionPanel(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public override bool Equals(object obj)
        {
            return obj is PositionPanel panel &&
                   Row == panel.Row &&
                   Column == panel.Column;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
        }

        public static bool operator ==(PositionPanel left, PositionPanel right)
        {
            return EqualityComparer<PositionPanel>.Default.Equals(left, right);
        }

        public static bool operator !=(PositionPanel left, PositionPanel right)
        {
            return !(left == right);
        }
    }
}
