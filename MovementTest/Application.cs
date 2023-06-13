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

		public bool HasPlayer { get; set; }

		public Application(bool hasPlayer)
		{
			Rand = new Random();
			ScreenHeight = Console.WindowHeight - 1;
			ScreenWidth = Console.WindowWidth;
			RefreshRate = 75;
            GameBoard = new AnimationFrame(ScreenHeight, ScreenWidth);
            GameObjects = new List<GameObject>();
			HasPlayer = hasPlayer;

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
                    updateObjectMovements();
                    updateObjectPositions();
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

		public void updateObjectMovements()
		{
            foreach (GameObject gameObject in GameObjects)
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
        }

		public void updateObjectPositions()
		{
            foreach (GameObject gameObject in GameObjects)
            {
				gameObject.collisionCheck();
				gameObject.OOBCheck();
				gameObject.moveObject(gameObject.yMovement, gameObject.xMovement);
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

