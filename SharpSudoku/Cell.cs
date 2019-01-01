using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSudoku
{
    class Cell
    {
        public int Row { get; }
        public int Col { get; }
        public int Index { get; }
        public int Value;
        public bool Editeable;

        public Cell(int cell_index, int game_size, char cell_value)
        {
            Index = cell_index;

            Row = Convert.ToInt32(Math.Floor((float)Index / (float)game_size));
            Col = Index % game_size;

            Value = (Int32)cell_value - 48;
            Editeable = (cell_value == 0);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
