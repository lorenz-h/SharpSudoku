using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSudoku
{
    class BruteSolver
    {
        private Board ThisBoard;

        private int ElementIndex = 0;

        public BruteSolver(string board_string)
        {
            ThisBoard = new Board(board_string);
        }

        public void Run()
        {
            Int64 counter = 0;
            byte Row;
            byte Col;

            while (true)
            {
                // UpdateVisualization(++counter);
                if (ElementIndex > (Board.Size * Board.Size) - 1)
                {
                    // if you have reached the end of the sudoku exit the solver
                    Console.WriteLine("Solver Exited.");
                    if (ThisBoard.CheckConstraints() != 0)
                    {
                        throw new Exception("Something weird happened. Algo thought it finished but board still contained errors.");
                    }
                    break;
                }
                Row = ThisBoard.Row_From_Index(ElementIndex);
                Col = ThisBoard.Col_From_Index(ElementIndex);
                if (ThisBoard.Editeable[Row, Col])
                {
                    ThisBoard.Cells[Row, Col]++;
                    if (ThisBoard.Cells[Row, Col] > 9)
                    {
                        ThisBoard.Cells[Row, Col] = 0;
                        MoveIndex(-1);
                        continue;
                    };
                    if (ThisBoard.CheckConstraints(Row, Col))
                    {
                        MoveIndex(1);
                        continue;
                    };

                }
                else
                {
                    Console.WriteLine("This Should only happen at beginning");
                    MoveIndex(1);
                };

            }
        }

        private void UpdateVisualization(Int64 counter)
        {
            throw new NotImplementedException();
        }

        private void MoveIndex(int direction)
        {
            ElementIndex += direction;
            while (!ThisBoard.Editeable[ThisBoard.Row_From_Index(ElementIndex), ThisBoard.Col_From_Index(ElementIndex)])
            {
                ElementIndex += direction;
                if (ElementIndex < 0)
                {
                    throw new Exception("Index became smaller than 0");
                }
            }
        }
    }
}
