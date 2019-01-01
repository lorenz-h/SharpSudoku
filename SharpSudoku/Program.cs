using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSudoku
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World");
            string board_string = "003020600900305001001806400008102900700000008006708200002609500800203009005010300";
            // string board_string = "200080300060070084030500209000105408000000000402706000301007040720040060004010003";
            Grid gg = new Grid(board_string);
            Console.WriteLine(gg);
            gg.Solve();
            Console.WriteLine(gg);
            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
