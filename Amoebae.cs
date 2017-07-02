using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//~(0_0~) ~(0_0)~ (~0_0)~
namespace SudokuAmoebae
{
    class Amoebae
    {
        Random rand = new Random();
        int[,] puzzle = new int[9, 9];
        int prevR;
        int prevC;
        int[] conc = new int[9];
        int valid;
        int minC = 0;
        int maxC = 8;
        int minR = 0;
        int maxR = 8;
        int emptyBoxesQ;
        int completedBoxesA;
        int div = 2147483646;
        int div1;
        int div2;
        int div3;
        int div4;
        int div5;
        int div6;
        int div7;
        int div8;
        int row;
        int col;

        int algo;
        int rr;
        lehmerGenerator l = new lehmerGenerator();
        schrageGenerator s = new schrageGenerator();
        //schrageSummingGenerator sa = new schrageSummingGenerator();

        bool restart = false;

        List<int> emptyWithFoodR = new List<int>();
        List<int> emptyWithFoodC = new List<int>();
        List<int> filledWithFoodR = new List<int>();
        List<int> filledWithFoodC = new List<int>();
        List<int> emptyWoFoodR = new List<int>();
        List<int> emptyWoFoodC = new List<int>();
        List<int> filledWoFoodR = new List<int>();
        List<int> filledWoFoodC = new List<int>();
        List<int> fullR = new List<int>();
        List<int> fullC= new List<int>();

        public Amoebae()
        {
            
        }
        
        public int[,] nextMove(int[,] puzzleQ, int al, int ra)
        {
            valid = 0;
            emptyBoxesQ = 0;//number of empty  in puzzle
            completedBoxesA = 9;//number of completed 
            restart = false; //restart puzzle
            //-boxConcentratedWgivens
            emptyWithFoodR.Clear(); 
            emptyWithFoodC.Clear();
            filledWithFoodR.Clear();
            filledWithFoodC.Clear();
            emptyWoFoodR.Clear();
            emptyWoFoodC.Clear();
            filledWoFoodR.Clear();
            filledWoFoodC.Clear();
            fullR.Clear();
            fullC.Clear();
            algo = al;
            rr = ra;

            //for pseudorandom division
            div1 = div / 9;
            div2 = div1 * 2;
            div3 = div1 * 3;
            div4 = div1 * 4;
            div5 = div1 * 5;
            div6 = div1 * 6;
            div7 = div1 * 7;
            div8 = div1 * 8;

            //copy question puzzle
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    puzzle[r, c] = puzzleQ[r, c];
                }
            }

            //calculating boxes concentration in puzzle
            conc = new int[9];
            int count = 0; //number of givens
            int cnt = 0;   //number of blocks
            for (int r = 0; r < 9; r += 3)
            {
                for (int c = 0; c < 9; c += 3)
                {
                    count = 0;
                    for (int i = r; i < r + 3; i++)
                    {
                        for (int j = c; j < c + 3; j++)
                        {
                            if (puzzle[i, j] != 0)
                                count++;
                        }
                    }
                    conc[cnt] = count;
                    if(count==0)
                    {
                        emptyBoxesQ++;
                    }
                    cnt++;
                }
            }

            // choose random starting point
            //if not Clever Amoeba, 
            if (algo!=3)
            {
                row = randomInt() - 1;
                col = randomInt() - 1;
            }
            //if Clever Amoeba algorithm
            else
            {
                row = randomIntIn();
                col = randomIntIn();
            }

            // check its box category
            int cate = boxesCategory(row, col);

            // leave a random number if box is empty (add to conc number if block have 'food') --> DONE
            if (puzzle[row, col] == 0)
            {
                puzzle[row, col] = randomInt();
                if (conc[cate] != 0)
                    conc[cate]++;
            }

            // if concentrated max, minus completed box count
            if (conc[cate] == 9)
                completedBoxesA--;

            display();
            
//FIRST MOVE
// if chosen box is in a conc box, move randomly and fill in all Empty cells in the box
            if (!(conc[cate] == 0 || conc[cate] == 9))
            {
                prevR = row;
                prevC = col;
                bool fillBox = false;
                while (!fillBox)
                {
                    BoxMinMax(row, col, true);
                    deNovoNeighbour(row, col);
                    
// neighbour priority
//1) empty cell, conc box
//fill in cell, add conc, next cell
                    if (emptyWithFoodR.Count > 0)
                    {
                        //Console.WriteLine("Empty cell ; Food box");
                        int next = rand.Next(emptyWithFoodR.Count);
                        prevR = row;
                        prevC = col;
                        row = emptyWithFoodR[next];
                        col = emptyWithFoodC[next];
                        puzzle[row, col] = randomInt();
                        conc[cate]++;
                    }
//2) filled cell, conc box
//next cell
                    else if (filledWithFoodC.Count > 0)
                    {
                        //Console.WriteLine("Filled cell ; Food box");
                        int next = rand.Next(filledWithFoodR.Count);
                        prevR = row;
                        prevC = col;
                        row = filledWithFoodR[next];
                        col = filledWithFoodC[next];
                    }
                    
                    //if filled, reduced completed box count & exit
                    if (conc[cate] == 9)
                    {
                        fillBox = true;
                        completedBoxesA--;
                    }

                    //if 
                    display();
                    if (restart)
                        goto start;
                }
            }
//if its in Empty box
            else
            {
                BoxMinMax(row, col, false);
                deNovoNeighbour(row, col);

//priority neighbour
//1) empty cell, conc box
//fill in cell, add conc if its nt originally empty box
//next cell
                if(emptyWithFoodR.Count>0)
                {
                    int next = rand.Next(emptyWithFoodR.Count);
                    prevR = row;
                    prevC = col;
                    row = emptyWithFoodR[next];
                    col = emptyWithFoodC[next];
                    puzzle[row, col] = randomInt();
                    int cat = boxesCategory(row, col);
                    if(!(conc[cat]==0  || conc[cat]==9))
                    conc[cat]++;
                    if (conc[cat] == 9)
                        completedBoxesA--;
                }
//2) filled cell, conc box
//next cell
                else if (filledWithFoodC.Count > 0)
                {
                    int next = rand.Next(filledWithFoodR.Count);
                    prevR = row;
                    prevC = col;
                    row = filledWithFoodR[next];
                    col = filledWithFoodC[next];
                }
//3) empty cell, empty box
// fill in cell, next cell
                else
                {
                    int next = rand.Next(emptyWoFoodR.Count);
                    prevR = row;
                    prevC = col;
                    row = emptyWoFoodR[next];
                    col = emptyWoFoodC[next];
                    puzzle[row, col] = randomInt();
                    //if empty box becomes fully conc, reduce completed count
                    if (checkBox(row, col) == 9 && conc[boxesCategory(row,col)]==0)
                    {
                        emptyBoxesQ--;
                        completedBoxesA--;
                        conc[boxesCategory(row, col)] = 9;
                    }
                }
                display();
                //restart if candidate invalid
                if (restart)
                    goto start;
            }

//SECOND MOVE
            bool sudokuSolve = false;
            while(!sudokuSolve)
            {
                deNovoNeighbour(row,col);
                cate = boxesCategory(row, col);

//empty boxes (checked last)
                if (completedBoxesA == emptyBoxesQ && emptyBoxesQ!=0)
                {
                    //fill in cells in empty box 
                    BoxMinMax(row, col, true);
                    if (conc[cate] == 0)
                    {
                        conc[cate] = checkBox(row, col);
                        bool fillBox = false;

                        //box completed if have pass all cells in the box
                        if (conc[cate] == 9)
                        {
                            completedBoxesA--;
                            emptyBoxesQ--;
                            fillBox = true;
                        }
                        else
                        {
                            while (!fillBox)
                            {
                                BoxMinMax(row, col, true);
                                deNovoNeighbour(row, col);

                                //Neighbour priority
                                //1) empty cell, empty box
                                // fill in cell, add conc, next cell
                                if (emptyWoFoodR.Count > 0)
                                {
                                    int next = rand.Next(emptyWoFoodR.Count);
                                    prevR = row;
                                    prevC = col;
                                    row = emptyWoFoodR[next];
                                    col = emptyWoFoodC[next];
                                    puzzle[row, col] = randomInt();
                                    conc[cate]++;
                                }
                                //2) empty cell, conc box,
                                // fill in cell, add conc, next cell
                                else if (emptyWithFoodR.Count > 0)
                                {
                                    int next = rand.Next(emptyWithFoodR.Count);
                                    prevR = row;
                                    prevC = col;
                                    row = emptyWithFoodR[next];
                                    col = emptyWithFoodC[next];
                                    puzzle[row, col] = randomInt();
                                    conc[cate]++;
                                }
                                //3) filled cell, conc box
                                //next cell
                                else if (filledWithFoodC.Count > 0)
                                {
                                    int next = rand.Next(filledWithFoodR.Count);
                                    prevR = row;
                                    prevC = col;
                                    row = filledWithFoodR[next];
                                    col = filledWithFoodC[next];
                                }

                                //reduce completed count when box is filled
                                if (conc[cate] == 9)
                                {
                                    completedBoxesA--;
                                    emptyBoxesQ--;
                                    fillBox = true;
                                }

                                display();
                                if (restart)
                                    goto start;
                            }
                        }
                    }
                    //if not in empty box
                    else
                    {
                        BoxMinMax(row, col, false);
                        deNovoNeighbour(row, col);

                        //neighbour Priority
                        //1) empty cell, conc box
                        //fill in cell, add conc, next cell
                        if (emptyWithFoodC.Count > 0)
                        {
                            int next = rand.Next(emptyWithFoodC.Count);
                            prevR = row;
                            prevC = col;
                            row = emptyWithFoodR[next];
                            col = emptyWithFoodC[next];
                            puzzle[row, col] = randomInt();
                            int cat = boxesCategory(row, col);
                            conc[cat]++;
                            //if completed
                            if (conc[cat] == 9)
                                completedBoxesA--;
                        }
                        //2) filled cell, conc box
                        //next cell
                        else if (filledWithFoodC.Count > 0)
                        {
                            ////////Console.WriteLine("move to neighbour with Filled cell");
                            int next = rand.Next(filledWithFoodC.Count);
                            prevR = row;
                            prevC = col;
                            row = filledWithFoodR[next];
                            col = filledWithFoodC[next];
                        }
                        //3) empty box or completed box
                        else
                        {
                            splittingNeighbour(row, col, prevR, prevC);
                            //do de nove if splitting cells unavailable (no conc, or full)
                            if (valid == 0)
                            {
                                BoxMinMax(row, col, false);

                                //neighbour priority
                                //1) empty cell, empty box
                                //fill in cell, next cell
                                if (emptyWoFoodC.Count > 0)
                                {
                                    int next = rand.Next(emptyWoFoodC.Count);
                                    prevR = row;
                                    prevC = col;
                                    row = emptyWoFoodR[next];
                                    col = emptyWoFoodC[next];
                                    puzzle[row, col] = randomInt();
                                    //only completed when 0->9
                                    if (checkBox(row, col) == 9)
                                    {
                                        completedBoxesA--;
                                        emptyBoxesQ--;
                                        conc[boxesCategory(row, col)]=9;
                                    }
                                }
                                //2) filled cell, empty box
                                //next cell
                                else if (filledWoFoodC.Count > 0)
                                {
                                    int next = rand.Next(filledWoFoodC.Count);
                                    prevR = row;
                                    prevC = col;
                                    row = filledWoFoodR[next];
                                    col = filledWoFoodC[next];
                                }
                                //3) filled cell, complete box
                                //next cell
                                else
                                {
                                    int next = rand.Next(fullC.Count);
                                    prevR = row;
                                    prevC = col;
                                    row = fullR[next];
                                    col = fullC[next];
                                }
                            }
                            //do splitting
                            else
                            {
                                //priority
                                //1) Empty cell, Empty box
                                if (emptyWoFoodR.Count > 0)
                                {
                                    ////////Console.WriteLine("Empty cell ; Not Food box");
                                    int next = rand.Next(emptyWoFoodC.Count);
                                    prevR = row;
                                    prevC = col;
                                    row = emptyWoFoodR[next];
                                    col = emptyWoFoodC[next];
                                    puzzle[row, col] = randomInt();
                                    if (checkBox(row, col) == 9)
                                    {
                                        completedBoxesA--;
                                        emptyBoxesQ--;
                                        conc[boxesCategory(row, col)] = 9;
                                    }
                                    ////////Console.WriteLine("category " + cate + " and concentration " + conc[cate]);
                                    ////////Console.WriteLine("Current point " + row + "," + col);
                                }
                                //2) filled cell, empty box
                                else if (filledWoFoodC.Count > 0)
                                {
                                    ////////Console.WriteLine("Empty cell ; Not Food box");
                                    int next = rand.Next(filledWoFoodC.Count);
                                    prevR = row;
                                    prevC = col;
                                    row = filledWoFoodR[next];
                                    col = filledWoFoodC[next];
                                    //puzzle[row, col] = randomInt();
                                    ////////Console.WriteLine("category " + cate + " and concentration " + conc[cate]);
                                    ////////Console.WriteLine("Current point " + row + "," + col);
                                }
                                //3) filled cell, conc box
                                else if (filledWithFoodR.Count > 0)
                                {
                                    ////////Console.WriteLine("Empty cell ; Not Food box");
                                    int next = rand.Next(filledWithFoodC.Count);
                                    prevR = row;
                                    prevC = col;
                                    row = filledWithFoodR[next];
                                    col = filledWithFoodC[next];
                                    //puzzle[row, col] = randomInt();
                                    ////////Console.WriteLine("category " + cate + " and concentration " + conc[cate]);
                                    ////////Console.WriteLine("Current point " + row + "," + col);
                                }
                                //4) Filled cell, conc box
                                else
                                {
                                    ////////Console.WriteLine("Filled cell ; Food box");
                                    int next = rand.Next(fullC.Count);
                                    prevR = row;
                                    prevC = col;
                                    row = fullR[next];
                                    col = fullC[next];
                                }
                            }
                        }
                        display();
                    }
                }
                //empty box check end

//if in conc box, move in randomly and fill in whole box
                else if (!(conc[cate] == 0 || conc[cate] == 9))
                {
                    //Console.WriteLine(row + "," + col);
                    //Console.WriteLine("fill in uncomplete whole block");
                    bool fillBox = false;

                    //filling whole box (de novo)
                    while (!fillBox)
                    {
                        BoxMinMax(row, col, true);
                        deNovoNeighbour(row, col);
                        //priority
                        //1) empty cell, conc box
                        if (emptyWithFoodR.Count > 0)
                        {
                            //Console.WriteLine("fill in Empty cell");
                            int next = rand.Next(emptyWithFoodR.Count);
                            prevR = row;
                            prevC = col;
                            row = emptyWithFoodR[next];
                            col = emptyWithFoodC[next];
                            puzzle[row, col] = randomInt();
                            int cat = boxesCategory(row, col);
                            conc[cat]++;
                            if (checkBox(row, col) == 9)
                            {
                                completedBoxesA--;
                                conc[cat] = 9;
                                fillBox = true;
                            }
                            
                          //Console.WriteLine("category " + cate + " and concentration " + conc[cate]);
                          //Console.WriteLine("Current point " + row + "," + col);               
                        }
                        //2) filled cell, conc box
                        else if (filledWithFoodC.Count > 0)
                        {
                          ////////Console.WriteLine("move into Filled cell");
                            int next = rand.Next(filledWithFoodR.Count);
                            prevR = row;
                            prevC = col;
                            row = filledWithFoodR[next];
                            col = filledWithFoodC[next];
                          ////////Console.WriteLine("category " + cate + " and concentration " + conc[cate]);
                          ////////Console.WriteLine("Current point " + row + "," + col);   
                        }
                        display();
                        if (restart)
                            goto start;
                    }
                }
                
//if in empty box or filled box
                else
                {
                    //priority
                    //1) check for nearest food box, empty first, then filled (without splitting)
                    BoxMinMax(row, col, false);
                    deNovoNeighbour(row, col);
                    if (emptyWithFoodC.Count > 0)
                    {
                        int next = rand.Next(emptyWithFoodC.Count);
                        prevR = row;
                        prevC = col;
                        row = emptyWithFoodR[next];
                        col = emptyWithFoodC[next];
                        puzzle[row, col] = randomInt();
                        int cat = boxesCategory(row, col);
                        conc[cat]++;
                        if (conc[cat] == 9)
                            completedBoxesA--;
                    }
                    else if (filledWithFoodC.Count > 0)
                    {
                       ////////Console.WriteLine("move to neighbour with Filled cell");
                        int next = rand.Next(filledWithFoodC.Count);
                        prevR = row;
                        prevC = col;
                        row = filledWithFoodR[next];
                        col = filledWithFoodC[next];
                       ////////Console.WriteLine("category " + cate + " and concentration " + conc[cate]);
                       ////////Console.WriteLine("Current point " + row + "," + col);  
                    }
                    //2) if no food box, move towards empty (by splitting)
                    //priority : empty cell>filled cell>completed box
                    else
                    {
                        splittingNeighbour(row, col, prevR, prevC);

                        //do de nove if splitting cells unavailable (no conc, or full)
                        //priority: empty cell(empty box)>filled cell(empty box)>completed box
                        if (valid == 0)
                        {
                            BoxMinMax(row, col, false);
                            if (emptyWoFoodC.Count > 0)
                            {
                                int next = rand.Next(emptyWoFoodC.Count);
                                prevR = row;
                                prevC = col;
                                row = emptyWoFoodR[next];
                                col = emptyWoFoodC[next];
                                puzzle[row, col] = randomInt();
                                if (checkBox(row, col) == 9)
                                {
                                    conc[boxesCategory(row, col)] = 9;
                                    completedBoxesA--;
                                    emptyBoxesQ--;
                                }
                            }
                            else if (filledWoFoodC.Count > 0)
                            {
                                int next = rand.Next(filledWoFoodC.Count);
                                prevR = row;
                                prevC = col;
                                row = filledWoFoodR[next];
                                col = filledWoFoodC[next];
                            }
                            else
                            {
                                int next = rand.Next(fullC.Count);
                                prevR = row;
                                prevC = col;
                                row = fullR[next];
                                col = fullC[next];
                            }
                        }
                        //splitting
                        //priority : empty cell (empty box)>filled cell(empty box)>filled cell(conc box)>completed box
                        else
                        {
                            if (emptyWoFoodR.Count > 0)
                            {
                                int next = rand.Next(emptyWoFoodC.Count);
                                prevR = row;
                                prevC = col;
                                row = emptyWoFoodR[next];
                                col = emptyWoFoodC[next];
                                
                                puzzle[row, col] = randomInt();
                                if (checkBox(row, col) == 9)
                                {
                                    conc[boxesCategory(row, col)] = 9;
                                    completedBoxesA--;
                                    emptyBoxesQ--;
                                }
                            }
                            else if (filledWoFoodC.Count > 0)
                            {
                               ////////Console.WriteLine("Empty cell ; Not Food box");
                                int next = rand.Next(filledWoFoodC.Count);
                                prevR = row;
                                prevC = col;
                                row = filledWoFoodR[next];
                                col = filledWoFoodC[next];
                            }
                            else if (filledWithFoodR.Count > 0)
                            {
                                ////////////Console.WriteLine("Empty cell ; Not Food box");
                                int next = rand.Next(filledWithFoodC.Count);
                                prevR = row;
                                prevC = col;
                                row = filledWithFoodR[next];
                                col = filledWithFoodC[next];
                            }
                            else
                            {
                               ////////Console.WriteLine("Filled cell ; Food box");
                                int next = rand.Next(fullC.Count);
                                prevR = row;
                                prevC = col;
                                row = fullR[next];
                                col = fullC[next];
                            }
                        }
                    }
                    display();
                }
                //if all box completed
                if (completedBoxesA == 0)
                {
                    sudokuSolve = true;
                }
                //if puzzle terminated (solve bt not finished)
                if (restart)
                    goto start;
            }
            start:
            return puzzle;
        }

//box boxesCategory
// 0 | 1 | 2
// 3 | 4 | 5
// 6 | 7 | 8
        public int boxesCategory(int row, int col)
        {
            if (row >= 0 && row <= 2)
            {
                if (col >= 0 && col <= 2)
                {
                    return 0;
                }
                else if (col >= 3 && col <= 5)
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
            else if (row >= 3 && row <= 5)
            {
                if (col >= 0 && col <= 2)
                {
                    return 3;
                }
                else if (col >= 3 && col <= 5)
                {
                    return 4;
                }
                else
                {
                    return 5;
                }
            }
            else
            {
                if (col >= 0 && col <= 2)
                {
                    return 6;
                }
                else if (col >= 3 && col <= 5)
                {
                    return 7;
                }
                else
                {
                    return 8;
                }
            }
            
        }

// get min and max for each coordinate
// if its in concentrated box, the min and max is in the box (boxFood = TRUE)
// if not in concentrated box, min max is its neighbour (boxfood = FALSE)
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

//check neighbour and save coordinates - 2 groups
//group 1 = Empty cells
//group 2 = Filled cells
//priority in randomization for group 1

//RANDOM(de novo) neighbours
        public void deNovoNeighbour(int row, int col)
        {
            ////////////Console.WriteLine("Do de novo");

            emptyWithFoodC = new List<int>();
            emptyWithFoodR = new List<int>();
            emptyWoFoodC = new List<int>();
            emptyWoFoodR = new List<int>();
            filledWithFoodC = new List<int>();
            filledWithFoodR = new List<int>();
            filledWoFoodC = new List<int>();
            filledWoFoodR = new List<int>();
            fullR = new List<int>();
            fullC = new List<int>();

            for (int i = row - 1; i < row + 2; i++)
            {
                for (int j = col - 1; j < col + 2; j++)
                {
                    if (!(i == row && j == col))
                    {
                        if (i >= minR && i <= maxR && j >= minC && j <= maxC)
                        {
                            int tempCate = boxesCategory(i, j);
                            if (puzzle[i, j] == 0)
                            {
                                if (conc[tempCate] == 0)
                                {
                                    emptyWoFoodR.Add(i);
                                    emptyWoFoodC.Add(j);
                                }
                                
                                else
                                {
                                    emptyWithFoodR.Add(i);
                                    emptyWithFoodC.Add(j);
                                }
                            }
                            else
                            {
                                if(conc[tempCate]==0)
                                {
                                    filledWoFoodR.Add(i);
                                    filledWoFoodC.Add(j);
                                }
                                else if (conc[tempCate] == 9)
                                {
                                    fullR.Add(i);
                                    fullC.Add(j);
                                }
                                else
                                {
                                    filledWithFoodR.Add(i);
                                    filledWithFoodC.Add(j);
                                }
                            }
                        }
                    }
                }
            }
        }

//SPLITTING Neighbours - when no conc box available
        public void splittingNeighbour(int row, int col, int prevR, int prevC)
        {
            int[] c = new int[3];
            int[] r = new int[3];
            //prev = center

            //curr = SE
            //new = E, S or SE
            if ((row - prevR) == 1 && (col - prevC) == 1)
            {
                r[0] = row; c[0] = col + 1;
                r[1] = row + 1; c[1] = col;
                r[2] = row + 1; c[2] = col + 1;
            }
            //curr = S
            //new = SW, S or SE
            else if ((row - prevR) == 1 && (col - prevC) == 0)
            {
                r[0] = row + 1; c[0] = col - 1;
                r[1] = row + 1; c[1] = col;
                r[2] = row + 1; c[2] = col + 1;
            }
            //curr = SW
            //new = W, SW or S
            else if ((row - prevR) == 1 && (col - prevC) == -1)
            {
                r[0] = row; c[0] = col - 1;
                r[1] = row + 1; c[1] = col - 1;
                r[2] = row + 1; c[2] = col;
            }
            //curr = W
            //new = NW, W or SW
            else if ((row - prevR) == 0 && (col - prevC) == -1)
            {
                r[0] = row - 1; c[0] = col - 1;
                r[1] = row; c[1] = col - 1;
                r[2] = row + 1; c[2] = col - 1;
            }
            //curr = NW
            //new = NW, W, N
            else if ((row - prevR) == -1 && (col - prevC) == -1)
            {
                r[0] = row - 1; c[0] = col - 1;
                r[1] = row - 1; c[1] = col;
                r[2] = row; c[2] = col - 1;
            }
            //curr = N
            //new = NW,N,NE
            else if ((row - prevR) == -1 && (col - prevC) == 0)
            {
                r[0] = row - 1; c[0] = col - 1;
                r[1] = row - 1; c[1] = col;
                r[2] = row - 1; c[2] = col + 1;
            }
            //curr = NE
            //new = N,NE,E
            else if ((row - prevR) == -1 && (col - prevC) == 1)
            {
                r[0] = row - 1; c[0] = col;
                r[1] = row - 1; c[1] = col + 1;
                r[2] = row; c[2] = col + 1;
            }
            //curr = E
            //new = NE, E, SE
            else if ((row - prevR) == 0 && (col - prevC) == 1)
            {
                r[0] = row - 1; c[0] = col +1;
                r[1] = row; c[1] = col + 1;
                r[2] = row + 1; c[2] = col + 1;
            }
            splittingCat(r, c, row, col);
        }

//Put splitting in categories
        public void splittingCat(int[] r, int[] c, int row, int col)
        {
            emptyWithFoodC = new List<int>();
            emptyWithFoodR = new List<int>();
            emptyWoFoodC = new List<int>();
            emptyWoFoodR = new List<int>();
            filledWithFoodC = new List<int>();
            filledWithFoodR = new List<int>();
            filledWoFoodC = new List<int>();
            filledWoFoodR = new List<int>();
            fullR = new List<int>();
            fullC = new List<int>();

            valid = 0;//checking whether there are cells available for splitting
            for (int i = 0; i < r.Length; i++)
            {
                if (r[i] >= minR && r[i] <= maxR && c[i] >= minC && c[i] <= maxC)
                {
                    valid++;
                    int tempCate = boxesCategory(r[i], c[i]);
                    //Empty cell, empty box
                    if (puzzle[r[i], c[i]] == 0 && conc[tempCate]==0)
                    {
                        emptyWoFoodR.Add(r[i]);
                        emptyWoFoodC.Add(c[i]);
                    }
                    //filled cell, empty box
                    else if(puzzle[r[i],c[i]]!=0 && conc[tempCate]==0)
                    {
                        filledWithFoodR.Add(r[i]);
                        filledWithFoodC.Add(c[i]);
                    }
                    //filled cell, completed box
                    else if(puzzle[r[i],c[i]]!=0 && conc[tempCate]==9)
                    {
                        fullR.Add(r[i]);
                        fullC.Add(c[i]);
                    }
                }
            }

            //if no cells available, save (available)8-neighbours for de novo
            if (valid == 0)
            {
                emptyWithFoodC = new List<int>();
                emptyWithFoodR = new List<int>();
                emptyWoFoodC = new List<int>();
                emptyWoFoodR = new List<int>();
                filledWithFoodC = new List<int>();
                filledWithFoodR = new List<int>();
                filledWoFoodC = new List<int>();
                filledWoFoodR = new List<int>();
                fullR = new List<int>();
                fullC = new List<int>();
                for (int i = row - 1; i < row + 2; i++)
                {
                    for (int j = col - 1; j < col + 2; j++)
                    {
                        if(i>=0 && i<9 && j>=0 && j<9 && !(i==row && j==col))
                        {
                            if (puzzle[i, j] == 0)
                            {
                                //empty cell, empty box
                                if (conc[boxesCategory(i, j)] == 0)
                                {
                                    emptyWoFoodR.Add(i);
                                    emptyWoFoodC.Add(j);
                                }
                            }
                            else
                            {
                                //filled cell, empty box
                                if (conc[boxesCategory(i, j)] == 0)
                                {
                                    filledWoFoodR.Add(i);
                                    filledWoFoodC.Add(j);
                                }
                                //filled cell, completed box
                                else
                                {
                                    fullR.Add(i);
                                    fullC.Add(j);
                                }
                            }
                        }
                    }
                }
            }
        }

//displays current sudoku
        public void display()
        {
            //Console.WriteLine(row + "," + col);
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    //Console.Write(puzzle[i, j]);
                    if ((j + 1) % 3 == 0)
                    {
                        //Console.Write(" ");
                    }
                }
                //Console.WriteLine();
                if ((i + 1) % 3 == 0)
                {
                   //Console.WriteLine();
                }
            }
            
            //Console.ReadKey();
        }

//check box concentration
        public int checkBox(int row, int col)
        {
            int food = 0;
            BoxMinMax(row, col, true);

            for (int i = minR; i <=maxR; i++)
            {
                for (int j = minC; j <=maxC; j++)
                {
                    if (puzzle[i, j] != 0)
                        food++;
                }
            }
            return food;

        }

//for direct choose random starting point (to avoid clashing with Clever Amoeba 's choosing candidate, heuristic)
        public int randomIntIn()
        {
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

//pick a random number from a range of highest number divisible by 9 possible
        public int randomInt()
        {
            div1 = div / 9;
            div2 = div1 * 2;
            div3 = div1 * 3;
            div4 = div1 * 4;
            div5 = div1 * 5;
            div6 = div1 * 6;
            div7 = div1 * 7;
            div8 = div1 * 8;

            //if algorithm = Clever Amoeba
            if (algo == 3)
            {
                //check what number is not available
                int[] num = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                List<int> options = new List<int>(num);

                //check in box to remove existing numbers in options list
                BoxMinMax(row, col, true);
                for (int i = minR; i <= maxR; i++)
                {
                    for (int j = minC; j <= maxC; j++)
                    {
                        if (puzzle[i, j] != 0)
                            options.Remove(puzzle[i, j]);
                    }
                }

                //check horizontally to remove existing numbers in options list
                for (int i = 0; i < 9; i++)
                {
                    if (puzzle[row, i] != 0)
                    {
                        options.Remove(puzzle[row, i]);
                    }
                }

                //check vertically to remove existing numbers in options list
                for (int i = 0; i < 9; i++)
                {
                    if (puzzle[i, col] != 0)
                    {
                        options.Remove(puzzle[i, col]);
                    }
                }

                //if no option, restart puzzle
                if (options.Count == 0)
                {
                    if (puzzle[row, col] != 0)
                        return puzzle[row, col];
                    else
                    {
                        restart = true;
                        return 0;
                    }
                }
                else
                {
                    int index = 0;
                    bool inRange = false; //number selection 
                    while (!inRange)
                    {
                        //rr == 1 = primitive randomization
                        //rr == 2 = lehmer randomization
                        //rr == 3 = schrage randomization
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
                            else if (randInt >= div4 && randInt < div5)
                            {
                                index = 4;
                            }
                            else if (randInt >= div5 && randInt < div6)
                            {
                                index = 5;
                            }
                            else if (randInt >= div6 && randInt < div7)
                            {
                                index = 6;
                            }
                            else if (randInt >= div7 && randInt < div8)
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
                        
                        
                        //only return if candidate is valid
                        if (index < options.Count)
                        {
                            inRange = true;
                        }
                    }
                    return options[index];
                }
            }

            //if algorithm = Amoeba
            else
            {
                //rr == 1 = primitive randomization
                //rr == 2 = lehmer randomization
                //rr == 3 = schrage randomization
                if (rr == 1)
                {
                    int randInt = rand.Next(div);

                    if (randInt >= 0 && randInt < div1)
                    {
                        return 1;
                    }
                    else if (randInt >= div1 && randInt < div2)
                    {
                        return 2;
                    }
                    else if (randInt >= div2 && randInt < div3)
                    {
                        return 3;
                    }
                    else if (randInt >= div3 && randInt < div4)
                    {
                        return 4;
                    }
                    else if (randInt >= div4 && randInt < div5)
                    {
                        return 5;
                    }
                    else if (randInt >= div5 && randInt < div6)
                    {
                        return 6;
                    }
                    else if (randInt >= div6 && randInt < div7)
                    {
                        return 7;
                    }
                    else if (randInt >= div7 && randInt < div8)
                    {
                        return 8;
                    }
                    else
                        return 9;
                }
                else if (rr == 2)
                {
                    return l.lehmer() + 1;
                }
                else 
                    return s.schrage() + 1;
             
            }
        }
    }
}
