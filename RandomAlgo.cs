using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuAmoebae
{
    /*RANDOM ALGORITHM*/
    class RandomAlgo
    {
        int rr;
        int div = 2147483646;
        int div1, div2, div3, div4, div5, div6, div7, div8;
        int[,] puzzle = new int[9,9];
        //int minR, maxR, minC, maxC;
        int r, c;
        int blank;
        bool restart = false;

        lehmerGenerator l = new lehmerGenerator();
        schrageGenerator s = new schrageGenerator();
        //schrageSummingGenerator sa = new schrageSummingGenerator();
        Random rand = new Random();

        public RandomAlgo()
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

            //calculate number of blanks
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    puzzle[r, c] = puzzleQ[r, c];
                    if (puzzleQ[r, c] == 0)
                        blank++;
                }
            }

            //fill in cells until no more blank cells
            while (blank > 0 && !restart)
            {
                //randomly choose next cell
                r = randomInt();
                c = randomInt();
                //randomly fill in empty cell
                if (puzzle[r, c] == 0)
                {
                    puzzle[r, c] = randomInt()+1;
                    blank--;
                }
            }
            return puzzle;
        }

        public int randomInt()
        {
            //Primitive randomization
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
    }
}
