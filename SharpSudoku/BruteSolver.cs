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
            ThisBoard.LogToConsole();
            if (ThisBoard.CheckConstraints(with_output:false) != 0)
            {
                throw new Exception("The Board Input contained Constraint violations. Solve not attempted.");
            }
            byte Row;
            byte Col;
            bool ShouldContinue = true;
            
            while (ShouldContinue)
            {
                
                Row = ThisBoard.Row_From_Index(ElementIndex);
                Col = ThisBoard.Col_From_Index(ElementIndex);
                if (ThisBoard.Editeable[Row, Col])
                {
                    ThisBoard.Cells[Row, Col]++;
                    if (ThisBoard.Cells[Row, Col] > 9)
                    {
                        ThisBoard.Cells[Row, Col] = 0;
                        ShouldContinue = MoveIndex(-1);
                        continue;
                    };
                    if (ThisBoard.CheckConstraints(Row, Col))
                    {
                        ShouldContinue = MoveIndex(1);
                        continue;
                    };

                }
                else
                {
                    Console.WriteLine("This Shouldd only happen at beginning");
                    ShouldContinue = MoveIndex(1);
                };

            }
            Console.WriteLine("Solver Exited.");
            if (ThisBoard.CheckConstraints(with_output: false) != 0)
            {
                throw new Exception("Something weird happened. Algo thought it finished but board still contained errors.");
            }
            Console.WriteLine("Solution Found:");
            ThisBoard.LogToConsole();
        }
        

        private bool MoveIndex(int direction)
        {
            do
            {
                ElementIndex += direction;
                if (ElementIndex > (Board.Size * Board.Size) - 1)
                {
                    // if you have reached the end of the sudoku exit the solver
                    
                    return false;
                }
                if (ElementIndex < 0)
                {
                    throw new Exception("Index became smaller than 0");
                }
            } while (!ThisBoard.Editeable[ThisBoard.Row_From_Index(ElementIndex), ThisBoard.Col_From_Index(ElementIndex)]);
            return true;
        }
    }
}
