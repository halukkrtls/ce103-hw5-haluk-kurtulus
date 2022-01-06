using System;
using System.Threading;

namespace ce103_hw5_snake_dll
{
    public class Class1
    {
        public const int SNAKE_ARRAY_SIZE = 310;
        public const ConsoleKey UP_ARROW = ConsoleKey.UpArrow;
        public const ConsoleKey LEFT_ARROW = ConsoleKey.LeftArrow;
        public const ConsoleKey RIGHT_ARROW = ConsoleKey.RightArrow;
        public const ConsoleKey DOWN_ARROW = ConsoleKey.DownArrow;
        public const ConsoleKey ENTER_KEY = ConsoleKey.Enter;
        public const ConsoleKey EXIT_BUTTON = ConsoleKey.Escape; // ESC
        public const ConsoleKey PAUSE_BUTTON = ConsoleKey.P; //p
        const char SNAKE_HEAD = (char)177;
        const char SNAKE_BODY = (char)178;
        const char WALL = (char)219;
        const char FOOD = (char)254;
        const char BLANK = ' ';

        public ConsoleKey waitForAnyKey()
        {
            ConsoleKey prsd;

            while (!Console.KeyAvailable) ;

            prsd = Console.ReadKey(false).Key;
            //pressed = tolower(pressed);
            return prsd;
        }

        public int getGameSpeed()
        {
            int supra = 0;
            Console.Clear();
            Console.SetCursorPosition(10, 5);
            Console.Write("Select The game speed between 1 and 9 and press enter\n");
            Console.SetCursorPosition(10, 6);
            int sllct = Convert.ToInt32(Console.ReadLine());
            switch (sllct)
            {
                case 1:
                    supra = 90;
                    break;
                case 2:
                    supra = 80;
                    break;
                case 3:
                    supra = 70;
                    break;
                case 4:
                    supra = 60;
                    break;
                case 5:
                    supra = 50;
                    break;
                case 6:
                    supra = 40;
                    break;
                case 7:
                    supra = 30;
                    break;
                case 8:
                    supra = 20;
                    break;
                case 9:
                    supra = 10;
                    break;

            }
            return supra;
        }

        public void pauseMenu()
        {
            int i;
            Console.SetCursorPosition(28, 23);
            Console.Write("*Paused*");

            waitForAnyKey();
            Console.SetCursorPosition(28, 23);
            Console.Write("            ");
            return;
        }

        //This function checks if a key has pressed, then checks if its any of the arrow keys/ p/esc key. It changes direction acording to the key pressed.
        public ConsoleKey checkKeysPressed(ConsoleKey drct)
        {
            ConsoleKey pressd;

            if (Console.KeyAvailable == true) //If a key has been pressed
            {
                pressd = Console.ReadKey(false).Key;
                if (drct != pressd)
                {
                    if (pressd == DOWN_ARROW && drct != UP_ARROW)
                    {
                        drct = pressd;
                    }
                    else if (pressd == UP_ARROW && drct != DOWN_ARROW)
                    {
                        drct = pressd;
                    }
                    else if (pressd == LEFT_ARROW && drct != RIGHT_ARROW)
                    {
                        drct = pressd;
                    }
                    else if (pressd == RIGHT_ARROW && drct != LEFT_ARROW)
                    {
                        drct = pressd;
                    }
                    else if (pressd == EXIT_BUTTON || pressd == PAUSE_BUTTON)
                    {
                        pauseMenu();
                    }
                }
            }
            return (drct);
        }
        //Cycles around checking if the x y coordinates ='s the snake coordinates as one of this parts
        //One thing to note, a snake of length 4 cannot collide with itself, therefore there is no need to call this function when the snakes length is <= 4
        public bool collisionSnake(int x, int y, int[,] snkyx, int snktll, int dcttt)
        {
            int i;
            for (i = dcttt; i < snktll; i++) //Checks if the snake collided with itself
            {
                if (x == snkyx[0, i] && y == snkyx[1, i])
                    return true;
            }
            return false;
        }
        //Generates food & Makes sure the food doesn't appear on top of the snake <- This sometimes causes a lag issue!!! Not too much of a problem tho
        public void generateFood(int[] yummuyxy, int wtdh, int hvyy, int[,] snkyx, int snktll)
        {
            Random numberrndm = new Random();
            do
            {
                //RandomNumbers.Seed(time(null));
                yummuyxy[0] = numberrndm.Next() % (wtdh - 2) + 2;
                //RandomNumbers.Seed(time(null));
                yummuyxy[1] = numberrndm.Next() % (hvyy - 6) + 2;
            } while (collisionSnake(yummuyxy[0], yummuyxy[1], snkyx, snktll, 0)); //This should prevent the "Food" from being created on top of the snake. - However the food has a chance to be created ontop of the snake, in which case the snake should eat it...

            Console.SetCursorPosition(yummuyxy[0], yummuyxy[1]);
            Console.Write(FOOD);
        }

        /*
        Moves the snake array forward, i.e. 
        This:
         x 1 2 3 4 5 6
         y 1 1 1 1 1 1
        Becomes This:
         x 1 1 2 3 4 5
         y 1 1 1 1 1 1

         Then depending on the direction (in this case west - left) it becomes:

         x 0 1 2 3 4 5
         y 1 1 1 1 1 1

         snakeXY[0][0]--; <- if direction left, take 1 away from the x coordinate
        */
        public void MoveSnakeArray(int[,] snkxy, int snktll, ConsoleKey dcttt)
        {
            int i;
            for (i = snktll - 1; i >= 1; i--)
            {
                snkxy[0, i] = snkxy[0, i - 1];
                snkxy[1, i] = snkxy[1, i - 1];
            }

            /*
            because we dont actually know the new snakes head x y, 
            we have to check the direction and add or take from it depending on the direction.
            */
            switch (dcttt)
            {
                case DOWN_ARROW:
                    snkxy[1, 0]++;
                    break;
                case RIGHT_ARROW:
                    snkxy[0, 0]++;
                    break;
                case UP_ARROW:
                    snkxy[1, 0]--;
                    break;
                case LEFT_ARROW:
                    snkxy[0, 0]--;
                    break;
            }

            return;
        }

        /**
        *
        *	  @name   Move Snake Body (move)
        *
        *	  @brief Move snake body
        *
        *	  Moving snake body
        *
        *	  @param  [in] snakeXY [\b int[,]]  snake coordinates
        *	  
        *	  @param  [in] snakeLength [\b int]  index of fibonacci number in the serie
        *	  
        *	  @param  [in] direction [\b ConsoleKey]  index of fibonacci number in the serie
        **/
        public void move(int[,] snkyx, int snktll, ConsoleKey dcttt)
        {
            int x;
            int y;

            //Remove the tail ( HAS TO BE DONE BEFORE THE ARRAY IS MOVED!!!!! )
            x = snkyx[0, snktll - 1];
            y = snkyx[1, snktll - 1];

            Console.SetCursorPosition(x, y);
            Console.Write(BLANK);

            //Changes the head of the snake to a body part
            Console.SetCursorPosition(snkyx[0, 0], snkyx[1, 0]);
            Console.Write(SNAKE_BODY);

            MoveSnakeArray(snkyx, snktll, dcttt);

            Console.SetCursorPosition(snkyx[0, 0], snkyx[1, 0]);
            Console.Write(SNAKE_HEAD);

            Console.SetCursorPosition(1, 1); //Gets rid of the darn flashing underscore.

            return;
        }

        //This function checks if the snakes head his on top of the food, if it is then it'll generate some more food...
        public bool eatFood(int[,] snkyx, int[] yummyxy)
        {
            if (snkyx[0, 0] == yummyxy[0] && snkyx[1, 0] == yummyxy[1])
            {
                yummyxy[0] = 0;
                yummyxy[1] = 0; //This should prevent a nasty bug (loops) need to check if the bug still exists...

                return true;
            }

            return false;
        }

        public bool collisionDetection(int[,] snkyx, int cnswtdh, int cnstll, int snklng) //Need to Clean this up a bit
        {
            bool colision = false;
            if ((snkyx[0, 0] == 1) || (snkyx[1, 0] == 1) || (snkyx[0, 0] == cnswtdh) || (snkyx[1, 0] == cnstll - 4)) //Checks if the snake collided wit the wall or it's self
                colision = true;
            else
                if (collisionSnake(snkyx[0, 0], snkyx[1, 0], snkyx, snklng, 1)) //If the snake collided with the wall, theres no point in checking if it collided with itself.
                colision = true;

            return (colision);
        }

        public void refreshInfoBar(int scr, int spd)
        {
            Console.SetCursorPosition(5, 23);
            Console.Write("Score: " + scr);

            Console.SetCursorPosition(5, 24);
            switch (spd)
            {
                case 90:
                    Console.Write("Speed: 1");
                    break;
                case 80:
                    Console.Write("Speed: 2");
                    break;
                case 70:
                    Console.Write("Speed: 3");
                    break;
                case 60:
                    Console.Write("Speed: 4");
                    break;
                case 50:
                    Console.Write("Speed: 5");
                    break;
                case 40:
                    Console.Write("Speed: 6");
                    break;
                case 30:
                    Console.Write("Speed: 7");
                    break;
                case 20:
                    Console.Write("Speed: 8");
                    break;
                case 10:
                    Console.Write("Speed: 9");
                    break;
            }

            Console.SetCursorPosition(52, 23);
            Console.Write("Coder: Ramazan Serhat UYGUN");

            Console.SetCursorPosition(52, 24);
            Console.Write("Version: 0.5");

            return;
        }

        ////*****HIGHSCORE STUFF*****//

        ////-> The highscores system seriously needs to be clean. There are some bugs, entering a name etc

        //public void createHighScores()
        //{
        //    FILE* file;
        //    int i;

        //    file = fopen("highscores.txt", "w+");

        //    if (file == null)
        //    {
        //        Console.Write("FAILED TO CREATE HIGHSCORES!!! EXITING!");
        //        Environment.Exit(0);
        //    }

        //    for (i = 0; i < 5; i++)
        //    {
        //        Console.Write(file, "%d", i + 1);
        //        Console.Write(file, "%s", "\t0\t\t\tEMPTY\n");
        //    }

        //    fclose(file);
        //    return;
        //}

        //public int getLowestScore()
        //{
        //    FILE* fp;
        //    char[] str = new char[128];
        //    int lowestScore = 0;
        //    int i;
        //    int intLength;

        //    if ((fp = fopen("highscores.txt", "r")) == null)
        //    {
        //        //Create the file, then try open it again.. if it fails this time exit.
        //        createHighScores(); //This should create a highscores file (If there isn't one)
        //        if ((fp = fopen("highscores.txt", "r")) == null)
        //            Environment.Exit(1);
        //    }

        //    while (!feof(fp))
        //    {
        //        gets(str, 126, fp);
        //    }
        //    object p = close(fp);

        //    i = 0;

        //    //Gets the Int length
        //    while (str[2 + i] != '\t')
        //    {
        //        i++;
        //    }

        //    intLength = i;

        //    //Gets converts the string to int
        //    for (i = 0; i < intLength; i++)
        //    {
        //        lowestScore = lowestScore + ((int)str[2 + i] - 48) * pow(10, intLength - i - 1);
        //    }

        //    return (lowestScore);
        //}

        //public void inputScore(int score) //This seriously needs to be cleaned up
        //{
        //    FILE* fp;
        //    FILE* file;
        //    char[] str = new char[20];
        //    int fScore;
        //    int i, s, y;
        //    int intLength;
        //    int[] scores = new int[5];
        //    int x;
        //    char[] highScoreName = new char[20];
        //    char[,] highScoreNames = new char[5,20];

        //    char[] name = new char[20];

        //    int entered = 0;

        //    Console.Clear(); //clear the console

        //    if ((fp = fopen("highscores.txt", "r")) == null)
        //    {
        //        //Create the file, then try open it again.. if it fails this time exit.
        //        createHighScores(); //This should create a highscores file (If there isn't one)
        //        if ((fp = fopen("highscores.txt", "r")) == null)
        //            Environment.Exit(1);
        //    }
        //    Console.SetCursorPosition(10, 5);
        //    Console.Write("Your Score made it into the top 5!!!");
        //    Console.SetCursorPosition(10, 6);
        //    Console.Write("Please enter your name: ");
        //    gets(name);

        //    x = 0;
        //    while (!feof(fp))
        //    {
        //        fgets(str, 126, fp);  //Gets a line of text

        //        i = 0;

        //        //Gets the Int length
        //        while (str[2 + i] != '\t')
        //        {
        //            i++;
        //        }

        //        s = i;
        //        intLength = i;
        //        i = 0;
        //        while (str[5 + s] != '\n')
        //        {
        //            //Console.Write("%c",str[5+s]);
        //            highScoreName[i] = str[5 + s];
        //            s++;
        //            i++;
        //        }
        //        //Console.Write("\n");

        //        fScore = 0;
        //        //Gets converts the string to int
        //        for (i = 0; i < intLength; i++)
        //        {
        //            //Console.Write("%c", str[2+i]);
        //            fScore = fScore + ((int)str[2 + i] - 48) * pow(10, intLength - i - 1);
        //        }

        //        if (score >= fScore && entered != 1)
        //        {
        //            scores[x] = score;
        //            strcpy(highScoreNames[x], name);

        //            //Console.Write("%d",x+1);
        //            //Console.Write("\t%d\t\t\t%s\n",score, name);		
        //            x++;
        //            entered = 1;
        //        }

        //        //Console.Write("%d",x+1);
        //        //Console.Write("\t%d\t\t\t%s\n",fScore, highScoreName);
        //        //strcpy(text, text+"%d\t%d\t\t\t%s\n");
        //        strcpy(highScoreNames[x], highScoreName);
        //        scores[x] = fScore;

        //        //highScoreName = "";
        //        for (y = 0; y < 20; y++)
        //        {
        //            highScoreName[y] = 0x00; //NULL
        //        }

        //        x++;
        //        if (x >= 5)
        //            break;
        //    }

        //    fclose(fp);

        //    file = fopen("highscores.txt", "w+");

        //    for (i = 0; i < 5; i++)
        //    {
        //        //Console.Write("%d\t%d\t\t\t%s\n", i+1, scores[i], highScoreNames[i]);
        //        Console.Write(file, "%d\t%d\t\t\t%s\n", i + 1, scores[i], highScoreNames[i]);
        //    }

        //    fclose(file);

        //    return;
        //}

        //public void displayHighScores() //NEED TO CHECK THIS CODE!!!
        //{
        //    FILE* fp;
        //    char[] str = new char[128];
        //    int y = 5;

        //    Console.Clear(); //clear the console

        //    if ((fp = fopen("highscores.txt", "r")) == NULL)
        //    {
        //        //Create the file, then try open it again.. if it fails this time exit.
        //        createHighScores(); //This should create a highscores file (If there isn't one)
        //        if ((fp = fopen("highscores.txt", "r")) == NULL)
        //            Environment.Exit(1);
        //    }

        //    Console.SetCursorPosition(10, y++);
        //    Console.Write("High Scores");
        //    Console.SetCursorPosition(10, y++);
        //    Console.Write("Rank\tScore\t\t\tName");
        //    while (!feof(fp))
        //    {
        //        Console.SetCursorPosition(10, y++);
        //        if (fgets(str, 126, fp))
        //            Console.Write("%s", str);
        //    }

        //    fclose(fp); //Close the file
        //    Console.SetCursorPosition(10, y++);

        //    Console.Write("Press any key to continue...");
        //    waitForAnyKey();
        //    return;
        //}

        //*****END HIGHSCORE STUFF*****//

        public void youWinScreen()
        {
            Console.Clear();
            int x = 6, y = 7;
            Console.SetCursorPosition(x, y++);
            Console.Write("'##:::'##::'#######::'##::::'##::::'##:::::'##:'####:'##::: ##:'####:");
            Console.SetCursorPosition(x, y++);
            Console.Write(". ##:'##::'##.... ##: ##:::: ##:::: ##:'##: ##:. ##:: ###:: ##: ####:");
            Console.SetCursorPosition(x, y++);
            Console.Write(":. ####::: ##:::: ##: ##:::: ##:::: ##: ##: ##:: ##:: ####: ##: ####:");
            Console.SetCursorPosition(x, y++);
            Console.Write("::. ##:::: ##:::: ##: ##:::: ##:::: ##: ##: ##:: ##:: ## ## ##:: ##::");
            Console.SetCursorPosition(x, y++);
            Console.Write("::: ##:::: ##:::: ##: ##:::: ##:::: ##: ##: ##:: ##:: ##. ####::..:::");
            Console.SetCursorPosition(x, y++);
            Console.Write("::: ##:::: ##:::: ##: ##:::: ##:::: ##: ##: ##:: ##:: ##:. ###:'####:");
            Console.SetCursorPosition(x, y++);
            Console.Write("::: ##::::. #######::. #######:::::. ###. ###::'####: ##::. ##: ####:");
            Console.SetCursorPosition(x, y++);
            Console.Write(":::..::::::.......::::.......:::::::...::...:::....::..::::..::....::");
            Console.SetCursorPosition(x, y++);

            waitForAnyKey();
            Console.Clear(); //clear the console
            return;
        }

        public void gameOverScreen()
        {
            int x = 17, y = 3;
            Console.Beep(2500, 275); //Beep
            Console.Clear();
            //http://www.network-science.de/ascii/ <- Ascii Art Gen

            Console.SetCursorPosition(x, y++);
            Console.Write(":'######::::::'###::::'##::::'##:'########:\n");
            Console.SetCursorPosition(x, y++);
            Console.Write("'##... ##::::'## ##::: ###::'###: ##.....::\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(" ##:::..::::'##:. ##:: ####'####: ##:::::::\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(" ##::'####:'##:::. ##: ## ### ##: ######:::\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(" ##::: ##:: #########: ##. #: ##: ##...::::\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(" ##::: ##:: ##.... ##: ##:.:: ##: ##:::::::\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(". ######::: ##:::: ##: ##:::: ##: ########:\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(":......::::..:::::..::..:::::..::........::\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(":'#######::'##::::'##:'########:'########::'####:\n");
            Console.SetCursorPosition(x, y++);
            Console.Write("'##.... ##: ##:::: ##: ##.....:: ##.... ##: ####:\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(" ##:::: ##: ##:::: ##: ##::::::: ##:::: ##: ####:\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(" ##:::: ##: ##:::: ##: ######::: ########::: ##::\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(" ##:::: ##:. ##:: ##:: ##...:::: ##.. ##::::..:::\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(" ##:::: ##::. ## ##::: ##::::::: ##::. ##::'####:\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(". #######::::. ###:::: ########: ##:::. ##: ####:\n");
            Console.SetCursorPosition(x, y++);
            Console.Write(":.......::::::...:::::........::..:::::..::....::\n");

            waitForAnyKey();
            Console.Clear(); //clear the console
            return;
        }

        //Messy, need to clean this function up
        public void startGame(int[,] snkyx, int[] yummyxy, int cnswtdh, int cnsltll, int snktall, ConsoleKey dcttt, int scr, int fst)
        {
            bool gameOver = false;
            ConsoleKey oldcttt = ConsoleKey.NoName;
            bool cnchngedcttt = true;
            int loser = 1;
            do
            {
                if (cnchngedcttt)
                {
                    oldcttt = dcttt;
                    dcttt = checkKeysPressed(dcttt);
                }

                if (oldcttt != dcttt)//Temp fix to prevent the snake from colliding with itself
                    cnchngedcttt = false;

                if (true) //haha, it moves according to how fast the computer running it is...
                {
                    //Console.SetCursorPosition(1,1);
                    //Console.Write("%d - %d",clock() , endWait);
                    move(snkyx, snktall, dcttt);
                    cnchngedcttt = true;


                    if (eatFood(snkyx, yummyxy))
                    {
                        generateFood(yummyxy, cnswtdh, cnsltll, snkyx, snktall); //Generate More Food
                        snktall++;
                        switch (fst)
                        {
                            case 90:
                                scr += 5;
                                break;
                            case 80:
                                scr += 7;
                                break;
                            case 70:
                                scr += 9;
                                break;
                            case 60:
                                scr += 12;
                                break;
                            case 50:
                                scr += 15;
                                break;
                            case 40:
                                scr += 20;
                                break;
                            case 30:
                                scr += 23;
                                break;
                            case 20:
                                scr += 25;
                                break;
                            case 10:
                                scr += 30;
                                break;
                        }

                        refreshInfoBar(scr, fst);
                    }
                    Thread.Sleep(fst);
                }

                gameOver = collisionDetection(snkyx, cnswtdh, cnsltll, snktall);

                if (snktall >= SNAKE_ARRAY_SIZE - 5) //Just to make sure it doesn't get longer then the array size & crash
                {
                    loser = 2;//You Win! <- doesn't seem to work - NEED TO FIX/TEST THIS
                    scr += 1500; //When you win you get an extra 1500 points!!!
                }

            } while (!gameOver);

            switch (loser)
            {
                case 1:
                    gameOverScreen();

                    break;
                case 2:
                    youWinScreen();
                    break;
            }

            //if (score >= getLowestScore() && score != 0)
            //{
            //    inputScore(score);
            //    displayHighScores();
            //}

            return;
        }

        public void loadEnviroment(int cnlwtdh, int cnstll)//This can be done in a better way... FIX ME!!!! Also i think it doesn't work properly in ubuntu <- Fixed
        {
            int i;
            int x = 1, y = 1;
            int rcttll = cnstll - 4;
            Console.Clear(); //clear the console

            Console.SetCursorPosition(x, y); //Top left corner

            for (; y < rcttll; y++)
            {
                Console.SetCursorPosition(x, y); //Left Wall 
                Console.Write("|", WALL);

                Console.SetCursorPosition(cnlwtdh, y); //Right Wall
                Console.Write("|", WALL);
            }

            y = 1;
            for (; x < cnlwtdh + 1; x++)
            {
                Console.SetCursorPosition(x, y); //Left Wall 
                Console.Write("-", WALL);

                Console.SetCursorPosition(x, rcttll); //Right Wall
                Console.Write("-", WALL);
            }

            /*
                for (i = 0; i < 80; i++)
                {
                    Console.Write("%c",WALL);
                }

                for (i = 0; i < 17; i++)
                {
                    Console.Write("%c\n",WALL);
                }

                for (i = 0; i < 21; i++)
                {
                    Console.Write("%c\n",WALL);
                    Console.SetCursorPosition(80,i);
                }

                for (i = 0; i < 81; i++)
                {
                    Console.Write("%c",WALL);
                }	
            */
            return;
        }

        public void loadSnake(int[,] snkyx, int snkhvy)
        {
            int i;
            /*
            First off, The snake doesn't actually have enough XY coordinates (only 1 - the starting location), thus we use
            these XY coordinates to "create" the other coordinates. For this we can actually use the function used to move the snake.
            This helps create a "whole" snake instead of one "dot", when someone starts a game.
            */
            //moveSnakeArray(snakeXY, snakeLength); //One thing to note ATM, the snake starts of one coordinate to whatever direction it's pointing...

            //This should print out a snake :P
            for (i = 0; i < snkhvy; i++)
            {
                Console.SetCursorPosition(snkyx[0, i], snkyx[1, i]);
                Console.Write(SNAKE_BODY); //Meh, at some point I should make it so the snake starts off with a head...
            }

            return;
        }

        /* NOTE, This function will only work if the snakes starting direction is left!!!! 
        Well it will work, but the results wont be the ones expected.. I need to fix this at some point.. */
        public void prepairSnakeArray(int[,] snkyx, int snkhvy)
        {
            int i, x;
            int snakeX = snkyx[0, 0];
            int snakeY = snkyx[1, 0];

            // this is used in the function move.. should maybe create a function for it...
            /*switch(direction)
            {
                case DOWN_ARROW:
                    snakeXY[1][0]++;
                    break;
                case RIGHT_ARROW:
                    snakeXY[0][0]++;
                    break;
                case UP_ARROW:
                    snakeXY[1][0]--;
                    break;
                case LEFT_ARROW:
                    snakeXY[0][0]--;
                    break;			
            }
            */


            for (i = 1; i <= snkhvy; i++)
            {
                snkyx[0, i] = snakeX + i;
                snkyx[1, i] = snakeY;
            }

            return;
        }

        //This function loads the enviroment, snake, etc
        public void loadGame()
        {
            int[,] snkyx = new int[2, SNAKE_ARRAY_SIZE]; //Two Dimentional Array, the first array is for the X coordinates and the second array for the Y coordinates

            int snkehvy = 4; //Starting Length

            ConsoleKey dctt = ConsoleKey.LeftArrow; //DO NOT CHANGE THIS TO RIGHT ARROW, THE GAME WILL INSTANTLY BE OVER IF YOU DO!!! <- Unless the prepairSnakeArray function is changed to take into account the direction....

            int[] yummyxy = { 5, 5 };// Stores the location of the food

            int scr = 0;
            //int level = 1;

            //Window Width * Height - at some point find a way to get the actual dimensions of the console... <- Also somethings that display dont take this dimentions into account.. need to fix this...
            int cnslwtdh = 80;
            int cnslwgh = 25;

            int supra = getGameSpeed();

            //The starting location of the snake
            snkyx[0, 0] = 40;
            snkyx[1, 0] = 10;

            loadEnviroment(cnslwtdh, cnslwgh); //borders
            prepairSnakeArray(snkyx, snkehvy);
            loadSnake(snkyx, snkehvy);
            generateFood(yummyxy, cnslwtdh, cnslwgh, snkyx, snkehvy);
            refreshInfoBar(scr, supra); //Bottom info bar. Score, Level etc
            startGame(snkyx, yummyxy, cnslwtdh, cnslwgh, snkehvy, dctt, scr, supra);

            return;
        }

        //*****MENU STUFF*****//

        public int menuSelector(int x, int y, int letsgo)
        {
            char key;
            int i = 0;
            x = x - 2;
            Console.SetCursorPosition(x, letsgo);

            Console.Write(">");

            Console.SetCursorPosition(1, 1);


            do
            {
                key = (char)waitForAnyKey();
                //Console.Write("%c %d", key, (int)key);
                if (key == (char)UP_ARROW)
                {
                    Console.SetCursorPosition(x, letsgo + i);
                    Console.Write(" ");

                    if (letsgo >= letsgo + i)
                        i = y - letsgo - 2;
                    else
                        i--;
                    Console.SetCursorPosition(x, letsgo + i);
                    Console.Write(">");
                }
                else
                    if (key == (char)DOWN_ARROW)
                {
                    Console.SetCursorPosition(x, letsgo + i);
                    Console.Write(" ");

                    if (i + 2 >= y - letsgo)
                        i = 0;
                    else
                        i++;
                    Console.SetCursorPosition(x, letsgo + i);
                    Console.Write(">");
                }
                //Console.SetCursorPosition(1,1);
                //Console.Write("%d", key);
            } while (key != (char)ENTER_KEY); //While doesn't equal enter... (13 ASCII code for enter) - note ubuntu is 10
            return (i);
        }

        public void welcomeArt()
        {
            Console.Clear(); //clear the console
                             //Ascii art reference: http://www.chris.com/ascii/index.php?art=animals/reptiles/snakes
            Console.Write("\n");
            Console.Write("\t\t    ___         ___ 			\n");
            Console.Write("\t\t   /         \\       /         \\ 			\n");
            Console.Write("\t\t  /  /~~\\  \\     /  /~~\\  \\ 			\n");
            Console.Write("\t\t  |  |     |  |     |  |     |  | 			\n");
            Console.Write("\t\t  |  |     |  |     |  |     |  | 			\n");
            Console.Write("\t\t  |  |     |  |     |  |     |  |         /	\n");
            Console.Write("\t\t  |  |     |  |     |  |     |  |       //	\n");
            Console.Write("\t\t (o  o)    \\  \\__/  /     \\  \\__/ / 	\n");
            Console.Write("\t\t  \\__/      \\         /       \\        / 	\n");
            Console.Write("\t\t    |        ~~~         ~~~~ 		\n");
            Console.Write("\t\t    ^											\n");
            Console.Write("\t		Welcome To The Snake Game!			\n");
            Console.Write("\t			    Press Any Key To Continue...	\n");
            Console.Write("\n");

            waitForAnyKey();
            return;
        }

        public void controls()
        {
            int x = 10, y = 5;
            Console.Clear(); //clear the console
            Console.SetCursorPosition(x, y++);
            Console.Write("\t\t-- Controls --");
            Console.SetCursorPosition(x++, y++);
            Console.Write(" Use the following arrow keys to direct the snake to the food: ");
            Console.SetCursorPosition(x, y++);
            Console.Write("- Right Arrow");
            Console.SetCursorPosition(x, y++);
            Console.Write("- Left Arrow");
            Console.SetCursorPosition(x, y++);
            Console.Write("- Top Arrow");
            Console.SetCursorPosition(x, y++);
            Console.Write("- Bottom Arrow");
            Console.SetCursorPosition(x, y++);
            Console.SetCursorPosition(x, y++);
            Console.Write("- P & Esc pauses the game.");
            Console.SetCursorPosition(x, y++);
            Console.SetCursorPosition(x, y++);
            Console.Write("Press any key to continue...");
            Console.ReadKey();
            return;
        }

        public void exitYN()
        {
            char prssd;
            Console.SetCursorPosition(9, 8);
            Console.Write("Are you sure you want to exit(Y/N)\n");

            do
            {
                prssd = (char)waitForAnyKey();
                prssd = char.ToLower(prssd);
            } while (!(prssd == 'y' || prssd == 'n'));

            if (prssd == 'y')
            {
                Console.Clear(); //clear the console
                Environment.Exit(0);
            }
            return;
        }

        public int mainMenu()
        {
            int x = 10, y = 5;
            int yStart = y;

            int slctd;

            Console.Clear(); //clear the console
                             //Might be better with arrays of strings???
            Console.SetCursorPosition(x, y++);
            Console.Write("New Game\n");
            Console.SetCursorPosition(x, y++);
            Console.Write("High Scores\n");
            Console.SetCursorPosition(x, y++);
            Console.Write("Controls\n");
            Console.SetCursorPosition(x, y++);
            Console.Write("Exit\n");
            Console.SetCursorPosition(x, y++);

            slctd = menuSelector(x, y, yStart);

            return (slctd);
        }

        //*****END MENU STUFF*****//
        public int main() //Need to fix this up
        {

            welcomeArt();

            do
            {
                switch (mainMenu())
                {
                    case 0:
                        loadGame();
                        break;
                    case 1:
                        //displayHighScores();
                        break;
                    case 2:
                        controls();
                        break;
                    case 3:
                        exitYN();
                        break;
                }
            } while (true);    //
        }

    }
}