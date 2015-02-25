using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheeseNibblerV2
{
    class Program
    {
        static void Main(string[] args)
        {
            bool playAgain = true;
            while (playAgain)
            {
                CheeseNibbler cheese = new CheeseNibbler();
                cheese.PlayGame();

                Console.WriteLine("Do you wanna play again? (Y/N)");
                ConsoleKeyInfo input = Console.ReadKey();
                if (input.Key != ConsoleKey.Y)
                {
                    playAgain = false;
                }
            }
            
            Console.ReadKey();
        }
    }

    public class Point
    {
        public enum PointStatus
        {
            Empty, Cheese, Mouse, Cat, CatAndCheese
        }
        public int X { get; set; }
        public int Y { get; set; }

        public PointStatus Status { get; set; }

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.Status = PointStatus.Empty;
        }
    }


    public class Mouse
    {
        public Point PointPosition { get; set; }
        public int Energy { get; set; }
        public  bool HasBeenPouncedOn { get; set; }
        public Mouse()
        {
            this.Energy = 50;
            this.HasBeenPouncedOn = false;
        }
    }

    public class Cat
    {
        public Point PointPosition { get; set; }

        public Cat() { }                        
    }


    public class CheeseNibbler
    {
        public Point[,] Grid { get; set; }
        public Mouse Mouse { get; set; }
        public Point Cheese { get; set; }
        public List<Cat> Cats { get; set; }
        public int CheeseCount { get; set; }

        public CheeseNibbler()
        {
            int x, y;
            //initialize the Grid
            this.Grid = new Point[10, 10];
            for (y = 0; y < 10; y++)
            {
                for (x = 0; x < 10; x++)
                {
                    this.Grid[x, y] = new Point(x, y);
                }
            }

            //initialize the Mouse            
            this.Mouse = new Mouse();
           
            //give mouse a position
            Random gnr = new Random();
            x = gnr.Next(0, 10);
            y = gnr.Next(0, 10);

            //place the mouse
            this.Mouse.PointPosition = this.Grid[x, y];

            //this will set the status point into the grid as mouse
            this.Mouse.PointPosition.Status = Point.PointStatus.Mouse;

            this.CheeseCount = -1;
           
            PlaceCheese();

            //initialize list of cats
            this.Cats = new List<Cat>();           
        }

        public void PlaceCheese()
        {
            int x, y;
            Random gnr = new Random();

            //choose a different position for the cheese
            do
            {
                x = gnr.Next(0, 10);
                y = gnr.Next(0, 10);
            }
            while (this.Grid[x,y].Status != Point.PointStatus.Empty);
           
            //put cheese into the grid            
            this.Cheese = this.Grid[x, y];

            //set status of the grid to cheese
            this.Cheese.Status = Point.PointStatus.Cheese;
           
            this.CheeseCount++;
        }

        public void drawGrid()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Use the numeric keypad to move\n");
            Console.ResetColor();
            for (int i = 0; i < 50; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();
            for (int y = 0; y < this.Grid.GetLength(1); y++)
            {
                for (int x = 0; x < this.Grid.GetLength(0); x++)
                {
                    Point myPoint = this.Grid[x, y];
                    if (myPoint.Status == Point.PointStatus.Mouse)
                    {
                        Console.Write("[ M ]");
                    }
                    else if (myPoint.Status == Point.PointStatus.Cheese)
                    {
                        Console.Write("[ ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("C");
                        Console.ResetColor();
                        Console.Write(" ]");
                    }
                    else if (myPoint.Status == Point.PointStatus.Cat)
                    {
                        Console.Write("[ ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("C");
                        Console.ResetColor();
                        Console.Write(" ]");
                    }
                    else if (myPoint.Status == Point.PointStatus.CatAndCheese)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("[ X ]");
                        Console.ResetColor();
                    }
                    else if (myPoint.Status == Point.PointStatus.Empty)
                    {
                        Console.Write("[   ]");
                    }
                }
                Console.WriteLine();
            }
            for (int i = 0; i < 50; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine("\nMouse energy: {0}",this.Mouse.Energy);
        }

        ConsoleKeyInfo input; 
        public ConsoleKey GetUserMove()
        {
            bool validMove = false;

            while (!validMove)
            {
                input = Console.ReadKey(true);
                if (input.Key == ConsoleKey.NumPad1 || input.Key == ConsoleKey.NumPad4 || input.Key == ConsoleKey.NumPad7 || input.Key == ConsoleKey.NumPad8 || input.Key == ConsoleKey.NumPad9 || input.Key == ConsoleKey.NumPad6 || input.Key == ConsoleKey.NumPad3 || input.Key == ConsoleKey.NumPad2)               
                {
                    if (ValidMove(input.Key))
                    {
                        validMove = true;
                    }
                }
                else {Console.WriteLine("Invalid move");}
            }
            return input.Key;
        }

        /// <summary>
        /// Check if the move goes outside the Grid
        /// </summary>
        /// <param name="input">The key presses</param>
        /// <returns>True if it is a valid move</returns>
        public bool ValidMove(ConsoleKey input)
        {
            int mousePositionX = this.Mouse.PointPosition.X;
            int mousePositionY = this.Mouse.PointPosition.Y;

            switch (input)
            {
                case ConsoleKey.NumPad1:
                    if (mousePositionX == 0 || mousePositionY == 9)
                    {
                        Console.WriteLine("Invalid Move. Out of the grid");
                        return false;
                    }
                    break;
                case ConsoleKey.NumPad4:
                    if (mousePositionX == 0)
                    {
                        Console.WriteLine("Invalid Move. Out of the grid");
                        return false;
                    }
                    break;
                case ConsoleKey.NumPad7:
                    if (mousePositionX == 0 || mousePositionY == 0)
                    {
                        Console.WriteLine("Invalid Move. Out of the grid");
                        return false;
                    }
                    break;
                case ConsoleKey.NumPad8:
                    if (mousePositionY == 0)
                    {
                        Console.WriteLine("Invalid Move. Out of the grid");
                        return false;
                    }
                    break;
                case ConsoleKey.NumPad9:
                    if (mousePositionX == 9 || mousePositionY == 0)
                    {
                        Console.WriteLine("Invalid Move. Out of the grid");
                        return false;
                    }
                    break;
                case ConsoleKey.NumPad6:
                    if (mousePositionX == 9)
                    {
                        Console.WriteLine("Invalid Move. Out of the grid");
                        return false;
                    }
                    break;
                case ConsoleKey.NumPad3:
                    if (mousePositionX == 9 || mousePositionY == 9)
                    {
                        Console.WriteLine("Invalid Move. Out of the grid");
                        return false;
                    }
                    break;
                case ConsoleKey.NumPad2:
                    if (mousePositionY == 9)
                    {
                        Console.WriteLine("Invalid Move. Out of the grid");
                        return false;
                    }
                    break;
            }
            return true;
        }

        /// <summary>
        /// Move the mouse
        /// </summary>
        /// <param name="input">The direction</param>

        public void MoveMouse(ConsoleKey input)
        {
            //save the original position of the Mouse
            int previousPositionX = this.Mouse.PointPosition.X;
            int previousPositionY = this.Mouse.PointPosition.Y;
      
            //Set the new coordinates
            switch (input)
            {
                case ConsoleKey.NumPad1: previousPositionX--; previousPositionY++;
                    break;
                case ConsoleKey.NumPad4: previousPositionX--;
                    break;
                case ConsoleKey.NumPad7: previousPositionX--; previousPositionY--;
                    break;
                case ConsoleKey.NumPad8: previousPositionY--;
                    break;
                case ConsoleKey.NumPad9: previousPositionX++; previousPositionY--; 
                    break;
                case ConsoleKey.NumPad6: previousPositionX++; 
                    break;
                case ConsoleKey.NumPad3: previousPositionX++; previousPositionY++;
                    break;
                case ConsoleKey.NumPad2: previousPositionY++; 
                    break;

            }
            // Get the point from the grid for the new position
            Point targetPosition = this.Grid[previousPositionX, previousPositionY];

            // Check if the target position is a cheese
            if (targetPosition.Status == Point.PointStatus.Cheese)
            {
                this.Mouse.Energy += 10;
                PlaceCheese();
                if (CheeseCount % 2 == 0)
                {
                    AddCat();
                }

            }
                //if the move is towards the cat
            else if (targetPosition.Status == Point.PointStatus.Cat)
            {
                this.Mouse.Energy = 0;
            }

            // clearing the old status
            this.Mouse.PointPosition.Status = Point.PointStatus.Empty;

            // set the new position to the type Mouse
            targetPosition.Status = Point.PointStatus.Mouse;

            // Update the property to have the x,y values
            this.Mouse.PointPosition = targetPosition;

            if(this.Mouse.Energy>0)
                this.Mouse.Energy--;         
        }


        public void PlaceCat(Cat cat)
        {
            int x, y;
            Random GNR =new Random();

            //Choose an empty position
            do
            {
                x = GNR.Next(0,10);
                y = GNR.Next(0, 10);
            } while (this.Grid[x, y].Status != Point.PointStatus.Empty);

            //Put the new cat into the Grid
            cat.PointPosition = this.Grid[x, y];
            //set status of the grid to cat
            cat.PointPosition.Status = Point.PointStatus.Cat;            
        }

        public void AddCat()
        {
            Cat newCat = new Cat();
            PlaceCat(newCat);
            this.Cats.Add(newCat);
        }

        public void MoveCat(Cat cat)
        {
            Random GNR = new Random();
            if (GNR.Next(0, 11) < 8)
            {
                //find the relative position of the Cat respect to the mouse
                int xRelativePosition = this.Mouse.PointPosition.X - cat.PointPosition.X;
                int yRelativePosition = this.Mouse.PointPosition.Y - cat.PointPosition.Y;

                bool tryLeft = xRelativePosition < 0;
                bool tryRight = xRelativePosition > 0;
                bool tryUp = yRelativePosition < 0;
                bool tryDown = yRelativePosition > 0;
               
                int xStartingPosition = cat.PointPosition.X;
                int yStartingPosition = cat.PointPosition.Y;
                bool validMove = false;
                while (!validMove && (tryLeft || tryRight || tryDown || tryUp))
                {
                    xStartingPosition = cat.PointPosition.X;
                    yStartingPosition = cat.PointPosition.Y;

                    if (tryLeft)
                    {
                        xStartingPosition--;                        
                        tryLeft = false;
                        
                    }
                    else if (tryRight)
                    {
                        xStartingPosition++;                     
                        tryRight = false;
                    }
                    else if (tryDown)
                    {
                        yStartingPosition++;                      
                        tryDown = false;
                    }
                    else if (tryUp)
                    {
                        yStartingPosition--;                       
                        tryUp = false;
                    }
                    //check if it's a valid move
                    validMove = IsValidCatMove(this.Grid[xStartingPosition, yStartingPosition]);
                }
                Point targetPosition = this.Grid[xStartingPosition, yStartingPosition];
                
                if (cat.PointPosition.Status == Point.PointStatus.CatAndCheese)
                    {
                        //walk off the cheese position
                        cat.PointPosition.Status = Point.PointStatus.Cheese;                       
                    }
                    else
                    {   //leave the position empty                       
                        cat.PointPosition.Status = Point.PointStatus.Empty;
                    }
                    
                    //if the cat get the mouse
                    if (targetPosition.Status == Point.PointStatus.Mouse)
                    {
                        this.Mouse.HasBeenPouncedOn = true;
                        targetPosition.Status = Point.PointStatus.Cat;
                    }
                    else if (targetPosition.Status == Point.PointStatus.Cheese)
                    {
                        targetPosition.Status = Point.PointStatus.CatAndCheese;
                    }
                    else
                    {
                        targetPosition.Status = Point.PointStatus.Cat;
                    }                   
                    cat.PointPosition = targetPosition;
                
            }
        }

        /// <summary>
        /// Chck if it's a valid move
        /// </summary>
        /// <param name="target">The target point</param>
        /// <returns>return true if the cat attempt a valid move</returns>
        public bool IsValidCatMove(Point target)
        {
            return (target.Status == Point.PointStatus.Empty) || (target.Status == Point.PointStatus.Mouse);
        }

        public void PlayGame()
        {                    
                while (this.Mouse.Energy > 0 && !this.Mouse.HasBeenPouncedOn)
                {
                    drawGrid();
                    //get the move from the user
                    ConsoleKey userMove = GetUserMove();
                    //move the mouse
                    MoveMouse(userMove);
                    foreach (Cat cat in this.Cats)
                    {
                        MoveCat(cat);
                    }
                }
                drawGrid();
                for (int i = 0; i < 30; i++)
                {
                    Console.Write("-");
                }
                Console.WriteLine("\nYou have eaten {0} ", this.CheeseCount);                        
        }
    }
}
