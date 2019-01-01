using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSudoku
{
    class Grid
    {
        public static int size = 9;
        public List<Cell> cells = new List<Cell>();
        private int steps = 0;

        public Grid(string board_string)
        {
            for (int i = 0; i < size * size; i++)
            {

                cells.Add(new Cell(i, size, board_string[i]));
            }

        }

        public override string ToString()
        {
            string output_string = "";
            foreach (Cell cell in cells)
            {
                output_string = output_string + cell.Value.ToString();
            }
            return output_string;
        }

        public void Solve()
        {
            int cell_index = ForwardTrack(-1);
            Console.WriteLine("Started Solving the Sudoku....");
            while (true)
            {
                steps++;
                if (steps % 10000 == 0)
                {
                    Console.Write("\r{0}%   ", ToString());
                }

                cells[cell_index].Value += 1;

                if (cells[cell_index].Value > 9)
                {
                    // if you have tried all options and all of them did infringed a constraint, go back
                    cell_index = BackTrack(cell_index);
                    continue;
                }
                    
                if (constraints_satisfied(cell_index))
                {
                    cell_index = ForwardTrack(cell_index);
                    if (cell_index == -1)
                    {
                        Console.WriteLine("Optimization Finished");
                        break;
                    }
                }
                
            }
            
        }

        private int ForwardTrack(int cell_index)
        {
            while (cell_index < size*size)
            {
                cell_index += 1;
                if (cells[cell_index].Editeable)
                {
                    return cell_index;
                }
                
            }
            return -1;
        }

        private int BackTrack(int cell_index)
        {
            cells[cell_index].Value = 0;
            cell_index -=1;
            if (cell_index < 0)
            {
                return 0;
            }
            if (cells[cell_index].Editeable)
            {
                return cell_index;
            }

            else
            {
                return BackTrack(cell_index);
            }
        }


        private bool constraints_satisfied(int changed_cell_index)
        // Checks only for row and column constraint.
        {
            var cells_in_row = cells.Where(c => c.Row == cells[changed_cell_index].Row);
            var cells_in_col = cells.Where(c => c.Col == cells[changed_cell_index].Col);



            int block_col_start = Convert.ToInt32(Math.Floor((double)cells[changed_cell_index].Col / 3.0));
            int block_col_end = block_col_start + 3;

            int block_row_start = Convert.ToInt32(Math.Floor((double)cells[changed_cell_index].Row / 3.0));
            int block_row_end = block_col_start + 3;

            var cells_in_cols_in_block = cells.Where(c => ((c.Col >= block_col_start) && (c.Col < block_col_end)));
            var cells_in_block = cells_in_cols_in_block.Where(c => ((c.Row >= block_row_start) && (c.Row < block_row_end)));



            if (cells_in_block.Where(c => c.Value == cells[changed_cell_index].Value).Count() > 1)
            {
                return false;
            }

            if (cells_in_row.Where(c => c.Value == cells[changed_cell_index].Value).Count() > 1)
            {
                return false;
            }
            if (cells_in_col.Where(c => c.Value == cells[changed_cell_index].Value).Count() > 1)
            {
                return false;
            }
            else
            {
                return true;
            };
        }
    }
}
