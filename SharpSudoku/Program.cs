using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSudoku
{
    class Program
    {
        public class Game
        {
            public static int size = 9;
            public List<Cell> cells = new List<Cell>();
            int iterations = 0;

            public Game(string Cells_vals)
            {
                int[] board_vals = ConvertBoardToIntArray(Cells_vals);
                for (int i = 0; i < size * size; i++)
                {

                    cells.Add(new Cell(i, size, board_vals[i]));
                }


            }
            private int[] ConvertBoardToIntArray(string BoardString)
            {
                List<int> int_list = new List<int>();
                foreach (char c in BoardString)
                {
                    int_list.Add((Int32)c - 48);
                }
                int[] int_array = int_list.ToArray();
                return int_array;
            }

            public override string ToString()
            {
                string output_string = "";
                foreach (Cell cell in cells)
                {
                    output_string = output_string + cell.value.ToString();
                }
                return output_string;
            }

            public bool Solve()
            {
                Explorer(); //starts exploring the sudoku in the top left corner
                return true;
            }

            private bool Explorer()
            {
                int cell_index = 0;
                bool solved = false;
                while (!solved)
                {
                    iterations++;
                    if (iterations % 100000 == 0)
                    {
                        Console.WriteLine(iterations);
                    }
                    if (cell_index > (size * size) - 1)
                    {
                        solved = true;
                        continue;
                    }

                    if (!cells[cell_index].editeable)
                    {
                        cell_index += 1;
                        continue;

                    }

                    cells[cell_index].value += 1;

                    if (cells[cell_index].value > 9) // if you have tried all combinations and none of them worked go one step back
                    {
                        cells[cell_index].value = 0;
                        int i = 1;
                        while (i <= cell_index)
                        {
                            if (cells[cell_index - i].editeable)
                            {
                                cell_index -= i;
                                break;
                            }
                            i++;
                        }
                        continue;

                    }

                    if (constraints_satisfied(cell_index))
                    {
                        cell_index += 1;
                        continue;
                    }
                    else
                    {
                        continue;
                    }
                }
                return true;
            }

            private bool constraints_satisfied(int changed_cell_index)
            // Checks only for row and column constraint.
            {
                var cells_in_row = cells.Where(c => c.row == cells[changed_cell_index].row);
                var cells_in_col = cells.Where(c => c.row == cells[changed_cell_index].col);



                int block_col_start = Convert.ToInt32(Math.Floor((double)cells[changed_cell_index].col / 3.0));
                int block_col_end = block_col_start + 3;

                int block_row_start = Convert.ToInt32(Math.Floor((double)cells[changed_cell_index].row / 3.0));
                int block_row_end = block_col_start + 3;

                var cells_in_cols_in_block = cells.Where(c => ((c.col >= block_col_start) && (c.col < block_col_end)));
                var cells_in_block = cells_in_cols_in_block.Where(c => ((c.row >= block_row_start) && (c.row < block_row_end)));



                if (cells_in_block.Where(c => c.value == cells[changed_cell_index].value).Count() > 1)
                {
                    return false;
                }

                if (cells_in_row.Where(c => c.value == cells[changed_cell_index].value).Count() > 1)
                {
                    return false;
                }
                if (cells_in_col.Where(c => c.value == cells[changed_cell_index].value).Count() > 1)
                {
                    return false;
                }
                else
                {
                    return true;
                };
            }
        };


        public class Cell
        {
            public int row { get; }
            public int col { get; }
            public int index { get; }
            public int value;
            public bool editeable;

            public Cell(int Index, int game_size, int Val)
            {
                index = Index;

                row = Convert.ToInt32(Math.Floor((float)Index / (float)game_size));
                col = Index % game_size;

                value = Val;
                editeable = (Val == 0);
            }

            public override string ToString()
            {
                return value.ToString();
            }


        };




        static void Main(string[] args)
        {
            Console.WriteLine("Hello World");
            //string board_string = "003020600900305001001806400008102900700000008006708200002609500800203009005010300";
            string board_string = "200080300060070084030500209000105408000000000402706000301007040720040060004010003";
            Game gg = new Game(board_string);
            Console.WriteLine(gg);
            gg.Solve();
            Console.WriteLine(gg);
            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
