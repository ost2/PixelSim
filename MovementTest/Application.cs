using System;
namespace MovementTest
{
    public class Application
    {
        public AnimationFrame GameBoard { get; set; }
        public List<GameObject> GameObjects { get; set; }

        public GameObject PlayerChar { get; set; }
        public GameObject NullObject { get; set; }

        public DateTime LastRefresh { get; set; }
        public TimeSpan GameTimer { get; set; }

        public Random Rand { get; set; }

        public int ScreenHeight { get; set; }
        public int ScreenWidth { get; set; }
        public int RefreshRate { get; set; }
        public int ObjectUpdateLimit { get; set; }

        public bool CellularAutomata { get; set; }
        public bool HasPlayer { get; set; }

        public Application(int updateLimit, int refreshRate, bool hasPlayer = false, bool cellAuto = false)
        {
            Rand = new Random();
            ScreenHeight = Console.WindowHeight - 1;
            ScreenWidth = Console.WindowWidth;
            GameBoard = new AnimationFrame(ScreenHeight, ScreenWidth, this);
            GameObjects = new List<GameObject>();
            HasPlayer = hasPlayer;
            RefreshRate = refreshRate;
            ObjectUpdateLimit = updateLimit;
            CellularAutomata = cellAuto;

            if (HasPlayer)
            {
                PlayerChar = new GameObject(this, '#');
                PlayerChar.MovementSpeed = 2;
                addGameObject(PlayerChar);
            }
        }

        public void runGame()
        {
            LastRefresh = DateTime.Now;
            runGameLoop();
        }

        void runGameLoop()
        {
            while (true)
            {
                parseInput();

                if ((DateTime.Now - LastRefresh).TotalMilliseconds >= RefreshRate)
                {
                    LastRefresh = DateTime.Now;
                    GameTimer += (new TimeSpan(0, 0, 0, 0, RefreshRate));
                    if (CellularAutomata)
                    {
                        updateCellularAutomata();
                    }
                    else
                    {
                        selectObjectsToMove();
                        updateObjectPositions();
                    }
                    generateFrame();
                    drawFrame();
                }
            }
        }

        public void addGameObject(GameObject gameObject)
        {
            GameObjects.Add(gameObject);
        }

        ConsoleKey takeInput()
        {
            return Console.ReadKey(true).Key;
        }

        public void parseInput()
        {
            if (Console.KeyAvailable && HasPlayer)
            {
                switch (takeInput())
                {
                    case ConsoleKey.W:
                        PlayerChar.setObjectMovement(-PlayerChar.MovementSpeed, 0);
                        break;
                    case ConsoleKey.A:
                        PlayerChar.setObjectMovement(0, -PlayerChar.MovementSpeed);
                        break;
                    case ConsoleKey.S:
                        PlayerChar.setObjectMovement(PlayerChar.MovementSpeed, 0);
                        break;
                    case ConsoleKey.D:
                        PlayerChar.setObjectMovement(0, PlayerChar.MovementSpeed);
                        break;
                    default:
                        break;
                }
            }
        }
        public void updateObjectMovement(GameObject gameObject)
        {
            if (gameObject == PlayerChar)
            {
                parseInput();
            }

            else if (gameObject.AvoidsNearest)
            {
                gameObject.setMovementAwayFromObject(gameObject.nearestNotRunner());
            }

            else if (gameObject.FollowsNearest)
            {
                gameObject.setMovementTowardsObject(gameObject.nearestNotFollower());
            }

            else if (gameObject.FollowsPlayer)
            {
                gameObject.setMovementTowardsObject(PlayerChar);
            }

        }

        public void selectObjectsToMove()
        {
            for (int i = 0; i < ObjectUpdateLimit; i++)
            {
                updateObjectMovement(GameObjects[Rand.Next(0, GameObjects.Count)]);
            }
        }

        public int[][] getAllAround(int[] pos)
        {
            var surroundingPixels = new int[8][];
            for (int g = 0; g < 8;  g++)
            {
                surroundingPixels[g] = new int[2];
            }
            int y = -1;
            int x = -1;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (j == 0)
                    {
                        if (x == 1)
                        {
                            x = -1;
                            y++;
                        }
                        surroundingPixels[i][j] = pos[j] + y;
                    }
                    else
                    {
                        surroundingPixels[i][j] = pos[j] + x;
                        if (x < 1) { x++; }
                    }                    
                }
            }
            return surroundingPixels;
        }

        public void updateCellularAutomata()
        {
            var objToAdd = new List<GameObject>(ScreenHeight * ScreenWidth);
            var objToRemove = new List<GameObject>(ScreenHeight * ScreenWidth);

            for (int i = 0; i < ScreenHeight; i++)
            {
                for (int j = 0; j < ScreenWidth; j++)
                {
                    if (i > 1 && i < ScreenHeight - 1 && j > 1 && j < ScreenWidth - 1)
                    {
                        var pos = new int[2];
                        pos[0] = i; pos[1] = j;

                        int numberOfSurrounding = 0;

                        var surroundingPixels = getAllAround(pos);

                        foreach (var adjPos in surroundingPixels)
                        {
                            foreach (var gameObject in GameObjects)
                            {
                                var posVector = new int[2];
                                posVector[0] = gameObject.yPos; posVector[1] = gameObject.xPos;

                                if (adjPos[0] == posVector[0] && adjPos[1] == posVector[1])
                                {
                                    numberOfSurrounding++;
                                }
                            }
                        }
                        GameObject objOnPos = null;
                        foreach (var gameObject in GameObjects)
                        {
                            var posVector = new int[2];
                            posVector[0] = gameObject.yPos; posVector[1] = gameObject.xPos;
                            if (pos[0] == posVector[0] && pos[1] == posVector[1])
                            {
                                objOnPos = gameObject;
                            }
                        }

                        if (objOnPos != null)
                        {
                            if (numberOfSurrounding == 2 || numberOfSurrounding == 3)
                            {

                            }
                            else
                            {
                                foreach (var gameObject in GameObjects)
                                {
                                    if (gameObject.yPos == pos[0] && gameObject.xPos == pos[1])
                                    {
                                        objToRemove.Add(gameObject);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (numberOfSurrounding == 3)
                            {
                                var newCell = new GameObject(this, i, j);
                                objToAdd.Add(newCell);
                            }
                        }
                    }
                }
            }
            foreach (var obj in objToAdd)
            {
                addGameObject(obj);
            }
            foreach (var obj in objToRemove)
            {
                GameObjects.Remove(obj);
            }
        }

        public void updateObjectPositions()
        {
            {
                foreach (GameObject gameObject in GameObjects)
                {
                    gameObject.collisionCheck();
                    gameObject.OOBCheck();
                    gameObject.moveObject(gameObject.yMovement, gameObject.xMovement);
                }
            }

        }

        public void playerLose()
        {
            Console.Clear();
            Writer.output("YOU DIED");
            Writer.output("Survived " + GameTimer.TotalSeconds + "s");
            Console.ReadKey();
        }

        public void generateFrame()
        {
            GameBoard.clearFrame();

            foreach (GameObject gameObject in GameObjects)
            {
                GameBoard.addPixel(gameObject.yPos, gameObject.xPos, gameObject.Symbol);
            }
        }

        public void drawFrame()
        {
            GameBoard.printFrame(0);
        }
    }
}

