using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;
using System.Globalization;

namespace SudokuAmoebae
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool algoProceed = false;
        bool randProceed = false;
        bool levelProceed = false;
        int algo, rand, lvl, start, end, rep;

        public MainWindow()
        {
            InitializeComponent();

            //CHOOSE ALGORITHM
            Console.WriteLine("Choose Algorithm Method");
            Console.WriteLine("1 - Random");
            Console.WriteLine("2 - Amoebae");
            Console.WriteLine("3 - \"Clever\" Amoebae");
            //Console.WriteLine("4 - \"Clever\" Random");
            Console.Write("Algorithm :");

            while (!algoProceed)
            {
                string a = Console.ReadLine();
                try
                {
                    algo = int.Parse(a);
                    algoProceed = true;
                    if (!(algo == 1 || algo == 2 || algo == 3))//))
                    {
                        algoProceed = false;
                        Console.WriteLine("Invalid choice. Try again");
                        Console.Write("Algorithm :");
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid choice. Try again");
                    Console.Write("Algorithm :");
                }
            }

            //CHOOSE RANDOMIZATION METHOD
            Console.WriteLine("Choose Randomization Method");
            Console.WriteLine("1 - Primitive");
            Console.WriteLine("2 - Lehmer");
            Console.WriteLine("3 - Schrage");
            //Console.WriteLine("4 - Summation of Two Schrage");

            Console.Write("Randomization :");

            while (!randProceed)
            {
                string ra = Console.ReadLine();
                try
                {
                    rand = int.Parse(ra);
                    randProceed = true;
                    if (!(rand == 1 || rand == 2 || rand == 3 || rand == 4))
                    {
                        randProceed = false;
                        Console.WriteLine("Invalid choice. Try again");
                        Console.Write("Randomization :");
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid choice. Try again");
                    Console.Write("Randomization :");
                }
            }


            //CHOOSE LEVEL
            Console.WriteLine("Choose Sudoku Level");
            Console.WriteLine("1 - Gentle");
            Console.WriteLine("2 - Moderate");
            Console.WriteLine("3 - Challenging");
            Console.WriteLine("4 - Complex");
            Console.WriteLine("5 - X pert");
            Console.Write("Level :");
            while (!levelProceed)
            {
                string la = Console.ReadLine();
                try
                {
                    lvl = int.Parse(la);
                    levelProceed = true;
                    if (!(lvl == 1 || lvl == 2 || lvl == 3 || lvl == 4 || lvl == 5))
                    {
                        levelProceed = false;
                        Console.WriteLine("Invalid choice. Try again");
                        Console.Write("Level :");
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid choice. Try again");
                    Console.Write("Level :");
                }
            }
            string lvlS;
            if(lvl==1)
                lvlS = "gentle.txt";
            else if(lvl==2)
                lvlS = "moderate.txt";
            else if(lvl==3)
                lvlS = "challenging.txt";
            else if(lvl == 4)
                lvlS = "complex.txt";
            else
                lvlS = "xpert.txt";

            //CHOOSE PUZZLE
            Console.WriteLine("From which puzzle to which? (Min:1, Max:50) [write same number for both start and end to read only a single puzzle]");
            bool chooseProceed = false;

            while (!chooseProceed)
            {
                //-------------Debugging purpose---------------//
                Console.Write("Start :");
                string pa1 = Console.ReadLine();
                Console.Write("End : ");
                string pa2 = Console.ReadLine();

                try
                {
                    start = int.Parse(pa1);
                    end = int.Parse(pa2);
                    chooseProceed = true;
                    if (start<=0 || start>50 || end<=0 || end>50 || start>end)
                    {
                        chooseProceed = false;
                        Console.WriteLine("Invalid choice. Try again");
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid choice. Try again");
                }
            }

            //-------------Debugging purpose---------------//
            //CHOOSE NUMBER OF REPETITION
            Console.Write("Repeat each line (?) times? : ");
            bool repeatProceed = false;

            while (!repeatProceed)
            {
                string re = Console.ReadLine();
                try
                {
                    rep = int.Parse(re);
                    repeatProceed = true;
                    if (rep<0)
                    {
                        repeatProceed = false;
                        Console.WriteLine("Invalid choice. Try again");
                        Console.Write("Repeat each line (?) times? : ");
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid choice. Try again");
                    Console.Write("Repeat each line (?) times? : ");
                }
            }


            readPuzzle r = new readPuzzle();

            //create file to salve solving data
            string fileName = DateTime.Now.ToString()+"_"+algo.ToString()+"_"+rand.ToString()+"_"+start.ToString()+"TO"+end.ToString()+"_"+lvlS;
            List<string> finaltries = new List<string>();
            fileName = fileName.Replace("/","-");
            fileName = fileName.Replace(":", "-");
            Console.WriteLine(fileName);
            File.AppendAllText(fileName, "i\trepeat\ttries\ttime"+ Environment.NewLine);

            //solve per puzzle
            for(int i = (start-1) ; i < end ; i++)
            {
                
                //solve per repeat
                for (int repeat = 0; repeat < rep; repeat++)
                {
                    Console.WriteLine("Solving puzzle # :" + (i + 1));
                    string tries = r.rdPuzzle(i, algo, rand, lvlS);
                    Console.Beep();
                    Console.WriteLine("Solved Puzzle #:" + (i+1) + " ,repeat:"+(repeat+1)+" ,algo: " + algo + " ,random: " + rand + " ,level: " + lvlS+" ,tries:"+tries);
                    finaltries.Add("i:" + (i+1).ToString() + " \trepeat:" + (repeat+1).ToString() + " \ttries:" + tries);
                    //saving solving data
                    File.AppendAllText(fileName, (i + 1).ToString() + " \t" + (repeat + 1).ToString() + " \t" + tries + Environment.NewLine);
                }
            }
            for (int i = 0; i < finaltries.Count; i++)
            {
                Console.WriteLine(finaltries[i]);
            }
        }
    }
}

////////////////~(+_+)~//////////////////