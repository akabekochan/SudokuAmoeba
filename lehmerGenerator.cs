using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuAmoebae
{
    /*LEHMER RANDOMIZATION ALGORITHM*/
    class lehmerGenerator
    {
        double a = 16807;
        double m = 2147483647; //2^31-1  //2^48;
        double seed;
        double random;
        double div = 1;
        double div1;
        double div2;
        double div3;
        double div4;
        double div5;
        double div6;
        double div7;
        double div8;

        Random r = new Random();

        public lehmerGenerator()
        {
        }

        public int lehmer()
        {
            div1 = div / 9;
            div2 = div1 * 2;
            div3 = div1 * 3;
            div4 = div1 * 4;
            div5 = div1 * 5;
            div6 = div1 * 6;
            div7 = div1 * 7;
            div8 = div1 * 8;

            seed = (r.Next(2147483646));
            seed = (a * seed) % m;
            random = (double)(seed / m);
            random = randomInt(random);
            return (int)random;
            
        }

        public int randomInt(double randInt)
        {
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
    }
}
