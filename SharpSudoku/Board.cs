using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSudoku
{
    class Board
    {
        public static int Size = 9;

        public int[,] Cells = new int[Size, Size];
        public bool[,] Editeable = new bool[Size, Size];

        int solver_direction = 0;

        public byte Row_From_Index(int elem_index)
        {
            byte outp = Convert.ToByte(Math.Floor((float)elem_index / (float)Size));
            return outp;
        }

        public byte Col_From_Index(int elem_index)
        {
            return Convert.ToByte(elem_index % Size);
        }

        public Board(string board_string)
        {
            int Row;
            int Col;
            if (board_string.Length != Size * Size)
            {
                throw new ArgumentException("Board String had invalid length");
            }
            for ( int i=0; i<Size*Size; i++)
            {
                Row = Row_From_Index(i);
                Col = Col_From_Index(i);
                Cells[Row, Col] =  (Int32)board_string[i] - 48;
                Editeable[Row, Col] = (Cells[Row, Col] == 0);
            }
            
        }

        public override string ToString()
        {
            string output_string = "";
            foreach(int cell in Cells)
            {
                output_string += cell;
            }
            return output_string;
        }

        
        private int MoveOLD(int elem_index, int direction)
        {
            while (!Editeable[Row_From_Index(elem_index), Col_From_Index(elem_index)])
            {
                elem_index += direction;
                if (elem_index > (Size * Size) - 1)
                {
                    // this means that the solver must have succefully solved the puzzle
                    return -99;
                }
                if (elem_index < -5)
                {
                    throw new Exception("Element Index became negative");
                }
            }
            return elem_index;
        }

        public void BruteSolveOLD()
        {
            Int64 counter = 0;
            int elem_index = 0;
            int direction = 1; // forward = 1, backward = -1, 0 = do not move
            byte Row;
            byte Col;
            Console.WriteLine("Starting Brute Solve");
            while (true)
            {
                counter++;
                if (counter%50 == 0)
                {
                    LogToConsole();
                }
                elem_index = Move(elem_index, direction);
                if (elem_index == -99)
                {
                    if (CheckConstraints() != 0)
                    {
                        throw new Exception("Something weird happened. Algo thought it finished but board still contained errors.");
                    }
                    Console.WriteLine("Succesfully solved");
                    break;
                }
                Row = Row_From_Index(elem_index);
                Col = Col_From_Index(elem_index);

                if (Editeable[Row, Col])
                {
                    Cells[Row, Col]++;
                    if (Cells[Row, Col] > 9)
                    {
                        // if you have tried all options and none of them succeded.
                        Cells[Row, Col] = 0;
                        direction = -1;// backtrack
                        continue;
                    };
                    if (CheckConstraints(Row, Col))
                    {
                        // if no collision at this elem, move on to the next element
                        direction = 1;
                        continue;
                    }
                    else
                    {
                        // direction = 0;
                    };

                    
                }
                
                
            }
        }

        private int Move(int elem_index, int direction)
        {
            throw new NotImplementedException();
        }

        public bool CheckConstraints(int elem_row, int elem_col)
        {
            
            return Row_Constraint_Satisfied(elem_row) && Col_Constraint_Satisfied(elem_col) && Box_Constraint_Satisfied(elem_row, elem_col);
        }

        public int CheckConstraints()
        {
            int error_count = 0;
            for (int i=0; i<Size; i++)
            {
                if (!Row_Constraint_Satisfied(i))
                {
                    Console.WriteLine("Collision in Row " + i.ToString());
                    error_count++;
                }
                if (!Col_Constraint_Satisfied(i))
                {
                    Console.WriteLine("Collision in Col " + i.ToString());
                    error_count++;
                } 
            }
            for (int i=0; i<Size; i += 3)
            {
                for (int j=0; j<Size; j += 3)
                {
                    if (!Box_Constraint_Satisfied(i,j))
                    {
                        Console.WriteLine("Collision in Box " + i.ToString() +"x" + j.ToString());
                        error_count++;
                    }
                }
            }
            Console.WriteLine("Collisions Found on Board: " + error_count.ToString());
            return error_count;
        }

        private bool Row_Constraint_Satisfied(int elem_row)
        {
            HashSet<int> elems_in_row = new HashSet<int>();
            for (int i = 0; i<Size; i++)
            {
                if (elems_in_row.Contains(Cells[elem_row, i]) && Cells[elem_row, i] !=0)
                {
                    return false; // if collision found return false
                }
                else
                {
                    elems_in_row.Add(Cells[elem_row, i]);
                }
            }
            return true; // if no collisions found it must be valid
        }
        private bool Col_Constraint_Satisfied(int elem_col)
        {
            HashSet<int> elems_in_col = new HashSet<int>();
            for (int i = 0; i < Size; i++)
            {
                if (elems_in_col.Contains(Cells[i, elem_col]) && Cells[i, elem_col] != 0)
                {
                    return false; // if collision found return false
                }
                else
                {
                    elems_in_col.Add(Cells[i, elem_col]);
                }
            }
            return true; // if no collisions found it must be valid
        }

        public bool Box_Constraint_Satisfied(int elem_row, int elem_col)
        {
            HashSet<int> elems_in_box = new HashSet<int>();
            int box_start_row = Convert.ToInt32(Math.Floor((float)elem_row / 3.0))*3;
            int box_start_col = Convert.ToInt32(Math.Floor((float)elem_col / 3.0))*3;
            for ( int i = box_start_row; i< box_start_row+3 ; i++)
            {
                for (int j = box_start_col; j< box_start_col+3; j++)
                {
                    if (elems_in_box.Contains(Cells[i, j]) && Cells[i, j] != 0)
                    {
                        return false; // if collision found return false
                    }
                    else
                    {
                        elems_in_box.Add(Cells[i, j]);
                    }
                }
            }
            return true;
        }


        public void LogToConsole()
        {
            Console.SetCursorPosition(0, 0);
            string output_string = "";
            for (int i=0; i<Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (j % 3 ==0 && j!=0)
                    {
                        output_string += "   ";
                    }
                    output_string += Cells[i,j] + " ";
                    
                }
                
                
                if (i%3 == 0)
                {
                    Console.WriteLine("");
                }
                Console.WriteLine(output_string);
                output_string = "";
            }
            Console.WriteLine("");
        }

        private void UpdateConsoleOuput()
        {

        }
    }
}
