using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;


namespace SudokuAmoebae
{
   
    /*READ SUDOKU PUZZLE AND SOLVE*/
    class readPuzzle
    {
        RandomAlgo ral = new RandomAlgo();
        Amoebae amo = new Amoebae();
        CleverRandom cr = new CleverRandom();

        public readPuzzle()
        {
        }

        public string rdPuzzle(int uniquePuzzle, int algo, int rand, string level)
        {
            //Console.WriteLine(algo);
            int[,] puzzle = new int[9,9];
            int[,] puzzleAnswer = new int[9,9];

            int count = 0;
            string line;

            //read puzzle from level text file
            using (StreamReader file = new StreamReader(level))
            {
                while ((line = file.ReadLine()) != null)
                {
                    //read chosen puzzle(line)
                    if (count == uniquePuzzle)
                    {
                        int x = 0;//to read each char in a line
                        for (int i = 0; i < 9; i++)//row
                        {
                            for (int j = 0; j < 9; j++)//column
                            {
                                if (line[x].Equals('.'))
                                {
                                    puzzle[i, j] = 0;
                                }
                                else
                                    puzzle[i, j] = int.Parse(line[x].ToString());
                                x++;
                            }
                        }
                        break;
                    }
                    else
                    {
                        count++;
                    }
                }
            }

            //display puzzle question
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(puzzle[i, j]);
                    if ((j+1) % 3 == 0)
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
                if ((i + 1) % 3 == 0)
                {
                    Console.WriteLine();
                }
            }


            bool solve = false;
            int c = 0;
            int l = -1;
            //setting threshold for Algorithm Random and Amoeba
            if (algo == 1 || algo == 2)
            {
                if (level.Equals("gentle.txt"))
                {
                    if (rand == 1)
                        l = 2700000;
                    else if (rand == 2)
                        l = 2500000;
                    else
                        l = 2000000;

                }
                else if (level.Equals("moderate.txt"))
                {
                    if (rand == 2)
                        l = 11600000;
                    else if (rand == 3)
                        l = 7900000;
                }
            }

            Stopwatch s = new Stopwatch();
            s.Restart();

            
            //loop puzzle until it is solve or until reach threshold
            while (!solve)
            {
                if (algo == 1)
                {
                    puzzleAnswer = ral.nextMove(puzzle,rand);
                }
                else if (algo == 2 || algo == 3)
                {
                    puzzleAnswer = amo.nextMove(puzzle, algo, rand);
                }
                else if (algo == 4)//removed
                {
                    puzzleAnswer = cr.nextMove(puzzle, rand);
                }
                
                solve = check(puzzleAnswer);
                /*if (!solve)
                {
                    display(puzzleAnswer);
                    Console.ReadKey();
                }*/
                c++;
                if (c == l)
                    break;
                //if there are 0, answer invalid
                if(solve)
                    display(puzzleAnswer);
            }
            Console.Beep();
            s.Stop();
            string timeTaken = s.Elapsed.ToString();
            if (c == l)
                timeTaken = "0";
            return c.ToString()+" \t"+timeTaken;

        }

//display current sudoku
        public void display(int[,] p)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Console.Write(p[i, j]);
                    if ((j + 1) % 3 == 0)
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
                if ((i + 1) % 3 == 0)
                {
                    Console.WriteLine();
                }
            }
        }

//check validity of answer
        public bool check(int[,] answer)
        {
            List<int> horizontal = new List<int>();
            List<int> vertical = new List<int>();
            bool breakLoop = false;
            bool uniqueSolution = true;

            //check if horizontal is unique
            for (int y = 0; y < 9; y++)
            {
                //if there are 0, answer invalid
                for (int x = 0; x < 9; x++)
                {
                    if (answer[x, y] == 0)
                    {
                        breakLoop = true;
                    }
                    horizontal.Add(answer[x, y]);
                }

                for (int i = 0; i < 9; i++)
                {
                    int count = 0;
                    for (int j = 0; j < 9; j++)
                    {
                        if (horizontal[i] == horizontal[j])
                        {
                            count++;
                        }
                    }
                    //comparing to itself
                    if (count == 1)
                        count = 0;
                    else
                    {
                        breakLoop = true;
                        break;
                    }
                }
                if (breakLoop)
                {
                    uniqueSolution = false;
                    break;
                }
                horizontal.Clear();
            }

            //check if vertical is unique
            if (uniqueSolution == true)
            {
                for (int x = 0; x < 9; x++)
                {
                    for (int y = 0; y < 9; y++)
                    {
                        //if there are 0, answer invalid
                        if (answer[x, y] == 0)
                        {
                            breakLoop = true;
                        }
                        vertical.Add(answer[x, y]);
                    }
                    for (int i = 0; i < 9; i++)
                    {
                        int count = 0;
                        for (int j = 0; j < 9; j++)
                        {
                            if (vertical[i] == vertical[j])
                            {
                                count++;
                            }
                        }
                        //comparing to itself
                        if (count == 1)
                            count = 0;
                        else
                        {
                            breakLoop = true;
                            break;
                        }
                    }
                    if (breakLoop)
                    {
                        uniqueSolution = false;
                        break;
                    }
                    vertical.Clear();

                }

            }

            //check if box is unique
            int[] box1 = { answer[0, 0], answer[1, 0], answer[2, 0], answer[0, 1], answer[1, 1], answer[2, 1], answer[0, 2], answer[1, 2], answer[2, 2] };
            int[] box2 = { answer[3, 0], answer[4, 0], answer[5, 0], answer[3, 1], answer[4, 1], answer[5, 1], answer[3, 2], answer[4, 2], answer[5, 2] };
            int[] box3 = { answer[6, 0], answer[7, 0], answer[8, 0], answer[6, 1], answer[7, 1], answer[8, 1], answer[6, 2], answer[7, 2], answer[8, 2] };
            int[] box4 = { answer[0, 3], answer[1, 3], answer[2, 3], answer[0, 4], answer[1, 4], answer[2, 4], answer[0, 5], answer[1, 5], answer[2, 5] };
            int[] box5 = { answer[3, 3], answer[4, 3], answer[5, 3], answer[3, 4], answer[4, 4], answer[5, 4], answer[3, 5], answer[4, 5], answer[5, 5] };
            int[] box6 = { answer[6, 3], answer[7, 3], answer[8, 3], answer[6, 4], answer[7, 4], answer[8, 4], answer[6, 5], answer[7, 5], answer[8, 5] };
            int[] box7 = { answer[0, 6], answer[1, 6], answer[2, 6], answer[0, 7], answer[1, 7], answer[2, 7], answer[0, 8], answer[1, 8], answer[2, 8] };
            int[] box8 = { answer[3, 6], answer[4, 6], answer[5, 6], answer[3, 7], answer[4, 7], answer[5, 7], answer[3, 8], answer[4, 8], answer[5, 8] };
            int[] box9 = { answer[6, 6], answer[7, 6], answer[8, 6], answer[6, 7], answer[7, 7], answer[8, 7], answer[6, 8], answer[7, 8], answer[8, 8] };

            if (uniqueSolution == true)
            {
                if (checkBox(box1) && checkBox(box2) && checkBox(box3) && checkBox(box4) && checkBox(box5) && checkBox(box6) && checkBox(box7) && checkBox(box8) && checkBox(box9))
                    uniqueSolution = true;
                else
                {
                    uniqueSolution = false;
                }
            }
            return uniqueSolution;
        }

        public bool checkBox(int[] square)
        {
            bool uniqueBlock = true;

            for (int i = 0; i < 9; i++)
            {
                //aif there are 0, answer invalid
                if (square[i] == 0)
                {
                    uniqueBlock = false;
                    break;
                }
                int count = 0;
                for (int j = 0; j < 9; j++)
                {
                    if (square[i] == square[j])
                    {
                        count++;
                    }
                }
                //comparing with itself
                if (count == 1)
                    count = 0;
                else
                {
                    uniqueBlock = false;
                    break;
                }
            }
            return uniqueBlock;
        }
    }
}
