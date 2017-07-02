using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuAmoebae
{
    /*RANDOMIZATION WITH HEURISTICS - removed*/
    class CleverRandom
    {
        int rr;
        int div = 2147483646;
        int div1, div2, div3, div4, div5, div6, div7, div8;
        int[,] puzzle = new int[9, 9];
        int minR, maxR, minC, maxC;
        int r, c;
        int blank;
        bool restart = false;

        lehmerGenerator l = new lehmerGenerator();
        schrageGenerator s = new schrageGenerator();
        //schrageSummingGenerator sa = new schrageSummingGenerator();
        Random rand = new Random();

        public CleverRandom()
        {

        }

        public int[,] nextMove(int[,] puzzleQ, int rand)
        {
            restart = false;

            rr = rand;
            div1 = div / 9;
            div2 = div1 * 2;
            div3 = div1 * 3;
            div4 = div1 * 4;
            div5 = div1 * 5;
            div6 = div1 * 6;
            div7 = div1 * 7;
            div8 = div1 * 8;
            blank = 0;

            //calculate number of empty cells in puzzle
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    puzzle[r, c] = puzzleQ[r, c];
                    if (puzzleQ[r, c] == 0)
                        blank++;
                }
            }

            while (blank > 0 && !restart)
            {
                //randomly choose cell
                r = randomInt();
                c = randomInt();
                //if cell empty, fill in
                if (puzzle[r, c] == 0)
                {
                    puzzle[r, c] = randomInt2();
                }
            }
            return puzzle;
        }

//to choose next cell
        public int randomInt()
        {
            //rr==1 = primitive
            //rr==2 = lehmer
            //rr==3 = schrage
            if (rr == 1)
            {
                int randInt = rand.Next(div);

                if (randInt >= 0 && randInt < div1)
                {
                    return 0;
                }
                else if (randInt >= div1 && randInt < div2)
                {
                    return 1;
                }
                else if (randInt >= div2 && randInt < div3)
                {
                    return 2;
                }
                else if (randInt >= div3 && randInt < div4)
                {
                    return 3;
                }
                else if (randInt >= div4 && randInt < div5)
                {
                    return 4;
                }
                else if (randInt >= div5 && randInt < div6)
                {
                    return 5;
                }
                else if (randInt >= div6 && randInt < div7)
                {
                    return 6;
                }
                else if (randInt >= div7 && randInt < div8)
                {
                    return 7;
                }
                else
                    return 8;
            }
            else if (rr == 2)
            {
                return l.lehmer();
            }
            else
            {
                return s.schrage();
            }
           
        }

//get boundary for box
        public void BoxMinMax(int row, int col, bool boxFood)
        {
            if (boxFood)
            {
                if (row >= 0 && row <= 2)
                {
                    minR = 0;
                    maxR = 2;
                    if (col >= 0 && col <= 2)
                    {
                        minC = 0;
                        maxC = 2;
                    }
                    else if (col >= 3 && col <= 5)
                    {
                        minC = 3;
                        maxC = 5;
                    }
                    else
                    {
                        minC = 6;
                        maxC = 8;
                    }
                }
                else if (row >= 3 && row <= 5)
                {
                    minR = 3;
                    maxR = 5;
                    if (col >= 0 && col <= 2)
                    {
                        minC = 0;
                        maxC = 2;
                    }
                    else if (col >= 3 && col <= 5)
                    {
                        minC = 3;
                        maxC = 5;
                    }
                    else
                    {
                        minC = 6;
                        maxC = 8;
                    }
                }
                else
                {
                    minR = 6;
                    maxR = 8;
                    if (col >= 0 && col <= 2)
                    {
                        minC = 0;
                        maxC = 2;
                    }
                    else if (col >= 3 && col <= 5)
                    {
                        minC = 3;
                        maxC = 5;
                    }
                    else
                    {
                        minC = 6;
                        maxC = 8;
                    }
                }

            }
            else
            {
                minR = 0;
                maxR = 8;
                minC = 0;
                maxC = 8;
            }
        }

//to choose candidate
        public int randomInt2()
        {
            div1 = div / 9;
            div2 = div1 * 2;
            div3 = div1 * 3;
            div4 = div1 * 4;
            div5 = div1 * 5;
            div6 = div1 * 6;
            div7 = div1 * 7;
            div8 = div1 * 8;
            {
                //check what number is not available
                int[] num = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                List<int> options = new List<int>(num);

                //check in block
                BoxMinMax(r, c, true);
                for (int i = minR; i <= maxR; i++)
                {
                    for (int j = minC; j <= maxC; j++)
                    {
                        if (puzzle[i, j] != 0)
                            options.Remove(puzzle[i, j]);
                    }
                }

                //check horizontally
                for (int i = 0; i < 9; i++)
                {
                    if (puzzle[r, i] != 0)
                    {
                        options.Remove(puzzle[r, i]);
                    }
                }

                //check vertically
                for (int i = 0; i < 9; i++)
                {
                    if (puzzle[i, c] != 0)
                    {
                        options.Remove(puzzle[i, c]);
                    }
                }

                //if no valid candidate, restart puzzle
                if (options.Count == 0)
                {
                    restart = true;
                    return 0;
                }
                //if valid candidate available, reduce blank cells count
                else
                {
                    blank--;
                    int index = 0;
                    bool inRange = false;
                    while (!inRange)
                    {
                        if (rr == 1)
                        {
                            int randInt = rand.Next(div);

                            if (randInt >= 0 && randInt < div1)
                            {
                                index = 0;
                            }
                            else if (randInt >= div1 && randInt < div2)
                            {
                                index = 1;
                            }
                            else if (randInt >= div2 && randInt < div3)
                            {
                                index = 2;
                            }
                            else if (randInt >= div3 && randInt < div4)
                            {
                                index = 3;
                            }
                            else if (randInt >= 4 && randInt < div5)
                            {
                                index = 4;
                            }
                            else if (randInt >= 5 && randInt < div6)
                            {
                                index = 5;
                            }
                            else if (randInt >= 6 && randInt < div7)
                            {
                                index = 6;
                            }
                            else if (randInt >= 7 && randInt < div8)
                            {
                                index = 7;
                            }
                            else
                                index = 8;
                        }
                        else if (rr == 2)
                        {
                            index = l.lehmer();
                        }
                        else 
                        {
                            index = s.schrage();
                        }
                        

                        if (index < options.Count)
                        {
                            inRange = true;
                        }
                    }
                    return options[index];
                }
            }
        }
    
//display current puzzle
        public void display(int[,] p)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    //Console.Write(p[i, j]);
                    if ((j + 1) % 3 == 0)
                    {
                        //Console.Write(" ");
                    }
                }
                Console.WriteLine();
                if ((i + 1) % 3 == 0)
                {
                    //Console.WriteLine();
                }
            }
        }
    }
}
