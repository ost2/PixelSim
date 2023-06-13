using System;
namespace MovementTest
{
    public class GameObject
    {
        public string Name { get; set; }
        public Application App { get; set; }
        public int yPos { get; set; }
        public int xPos { get; set; }
        public int yMovement { get; set; }
        public int xMovement { get; set; }
        public int MovementSpeed { get; set; }

        public char Symbol { get; set; }

        public bool IsSolid { get; set; }
        public bool IsHostile { get; set; }
        public bool FollowsPlayer { get; set; }
        public bool AvoidsNearest { get; set; }
        public bool FollowsNearest { get; set; }
        public bool IsInfectious { get; set; }

        public GameObject(Application app, char symbol, int movementSpeed = 1, bool isSolid = true, bool isHostile = false, bool followsPlayer = false, bool avoidsNearest = false, bool followsNearest = false, bool isInfectious = false)
        {
            App = app;
            MovementSpeed = movementSpeed;
            Symbol = symbol;
            IsSolid = isSolid;
            IsHostile = isHostile;
            FollowsPlayer = followsPlayer;
            AvoidsNearest = avoidsNearest;
            FollowsNearest = followsNearest;
            IsInfectious = isInfectious;
        }

        public GameObject(GameObject gameObject, int y, int x, List<char> randomSymbols = null)
        {
            App = gameObject.App;
            yPos = y;
            xPos = x;

            if (randomSymbols == null)
            {
                Symbol = gameObject.Symbol;
            }
            else
            {
                Symbol = randomSymbols[App.Rand.Next(0, randomSymbols.Count)];
            }

            IsSolid = gameObject.IsSolid;
            IsHostile = gameObject.IsHostile;
            FollowsPlayer = gameObject.FollowsPlayer;
            FollowsNearest = gameObject.FollowsNearest;
            AvoidsNearest = gameObject.AvoidsNearest;
            IsInfectious = gameObject.IsInfectious;
            MovementSpeed = gameObject.MovementSpeed;
        }

        public void moveObject(int dy, int dx)
        {
            yPos += dy;
            xPos += dx;
        }

        public void doCollision(GameObject Mover, GameObject Stander)
        {
            if (Stander.IsSolid)
            {
                Mover.setMovementAwayFromObject(Stander);
                Stander.setMovementAwayFromObject(Mover);
                //Mover.yMovement = 0;
                //Mover.xMovement = 0;
            }
            if (Mover.IsHostile || Stander.IsHostile)
            {
                if (Stander == App.PlayerChar || Mover == App.PlayerChar)
                {
                    App.playerLose();
                }
            }
            if (Mover.IsInfectious && Stander != App.PlayerChar && !Stander.IsInfectious)
            {
                Stander.infectObject(Mover);
            }
            else if (Stander.IsInfectious && Mover != App.PlayerChar && !Mover.IsInfectious)
            {
                Mover.infectObject(Stander);
            }
        }

        public void infectObject(GameObject infector)
        {
            Symbol = infector.Symbol;
            IsSolid = infector.IsSolid;
            IsHostile = infector.IsHostile;
            IsInfectious = infector.IsInfectious;
            AvoidsNearest = infector.AvoidsNearest;
            FollowsNearest = infector.FollowsNearest;
            MovementSpeed = infector.MovementSpeed;
        }


        public void deleteObject()
        {
            Symbol = ' ';
            IsSolid = false;
            IsHostile = false;
            IsInfectious = false;
            AvoidsNearest = false;
            FollowsNearest = false;
            MovementSpeed = 0;
        }

        public void collisionCheck()
        {
            int nextYPos = yPos + yMovement;
            int nextXPos = xPos + xMovement;

            foreach (GameObject gameObject in App.GameObjects)
            {
                if (gameObject.yPos == nextYPos && gameObject.xPos == nextXPos)
                {
                    doCollision(this, gameObject);
                }
                if (gameObject.yPos == nextYPos - (yMovement / 2) && gameObject.xPos == nextXPos - (xMovement / 2))
                {
                    doCollision(this, gameObject);
                }
            }
        }

        public void OOBCheck()
        {
            int newYPos = yPos + yMovement;
            int newXPos = xPos + xMovement;

            if (newYPos < 0 || newYPos >= App.ScreenHeight || newXPos < 0 || newXPos >= App.ScreenWidth)
            {
                reverseMovement();
                //nullifyMovement();
                //teleportRandom();
                //teleportToMiddle();
            }
        }

        public void reverseMovement()
        {
            yMovement = -yMovement;
            xMovement = -xMovement;
        }
        public void nullifyMovement()
        {
            yMovement = 0;
            xMovement = 0;
        }

        public void teleportRandom()
        {
            yPos = App.Rand.Next(0, App.ScreenHeight - 1);
            xPos = App.Rand.Next(0, App.ScreenWidth - 1);
        }

        public void teleportToMiddle()
        {
            yPos = App.ScreenHeight / 2;
            xPos = App.ScreenWidth / 2;
        }

        public void setMovementAwayFromObject(GameObject objectToAvoid)
        {
            int dy = yPos - objectToAvoid.yPos;
            int dx = xPos - objectToAvoid.xPos;

            int yMove = 0;
            int xMove = 0;

            if (dy > 0)
            {
                yMove = MovementSpeed;
            }
            else if (dy < 0)
            {
                yMove = -MovementSpeed;
            }
            if (dx > 0)
            {
                xMove = MovementSpeed;
            }
            else if (dx < 0)
            {
                xMove = -MovementSpeed;
            }
            if (Math.Abs(dy) > Math.Abs(dx) * 1.5)
            {
                xMove = 0;
            }
            else if (Math.Abs(dx) > Math.Abs(dy) * 1.5)
            {
                yMove = 0;
            }
            setObjectMovement(yMove, xMove);
        }

        public void setMovementTowardsObject(GameObject objectToFollow)
        {
            int dy = yPos - objectToFollow.yPos;
            int dx = xPos - objectToFollow.xPos;

            int yMove = 0;
            int xMove = 0;

            if (dy > 0)
            {
                yMove = -MovementSpeed;
            }
            else if (dy < 0)
            {
                yMove = MovementSpeed;
            }
            if (dx > 0)
            {
                xMove = -MovementSpeed;
            }
            else if (dx < 0)
            {
                xMove = MovementSpeed;
            }
            if (Math.Abs(dy) > Math.Abs(dx) * 1.5)
            {
                xMove = 0;
            }
            else if (Math.Abs(dx) > Math.Abs(dy) * 1.5)
            {
                yMove = 0;
            }
            setObjectMovement(yMove, xMove);
        }

        double[] getPytDistances()
        {
            List<int> yDistances = new List<int>(App.GameObjects.Count - 1);
            List<int> xDistances = new List<int>(App.GameObjects.Count - 1);
            List<double> pytDistances = new List<double>(App.GameObjects.Count - 1);

            for (int i = 0; i < App.GameObjects.Count; i++)
            {
                if (App.GameObjects[i].yPos != yPos || App.GameObjects[i].xPos != xPos)
                {
                    pytDistances.Add(Math.Round(Math.Sqrt(Math.Pow(Math.Abs(yPos) - Math.Abs(App.GameObjects[i].yPos), 2) + Math.Pow(Math.Abs(xPos) - Math.Abs(App.GameObjects[i].xPos), 2)), 1));
                }
            }
            pytDistances.Sort();
            return pytDistances.ToArray();
        }

        List<GameObject> nearestToFurthest()
        {
            List<GameObject> nearestToFurthestObjects = new List<GameObject>(getPytDistances().Count());
            foreach (Double distance in getPytDistances())
            {
                foreach (GameObject gameObject in App.GameObjects)
                {
                    if (gameObject != this)
                    {
                        double objectPytDistance = Math.Round(Math.Sqrt(Math.Pow(Math.Abs(yPos) - Math.Abs(gameObject.yPos), 2) + Math.Pow(Math.Abs(xPos) - Math.Abs(gameObject.xPos), 2)), 1);
                        if (distance > objectPytDistance - 1 && distance < objectPytDistance + 1)
                        {
                            nearestToFurthestObjects.Add(gameObject);
                        }
                    }
                }
            }
            return nearestToFurthestObjects;
        }

        public GameObject nearestNotFollower()
        {
            for (int i = 0; i < nearestToFurthest().Count; i++)
            {
                if (!nearestToFurthest()[i].FollowsNearest)
                {
                    return nearestToFurthest()[i];
                }
            }
            return App.GameObjects[App.Rand.Next(0, App.GameObjects.Count)];
        }

        public GameObject nearestNotRunner()
        {
            for (int i = 0; i < nearestToFurthest().Count; i++)
            {
                if (!nearestToFurthest()[i].AvoidsNearest)
                {
                    return nearestToFurthest()[i];
                }
            }
            return App.GameObjects[App.Rand.Next(0, App.GameObjects.Count)];
        }



        public GameObject nearestObjectBackup()
        {
            int[] yDistances = new int[App.GameObjects.Count];
            int[] xDistances = new int[App.GameObjects.Count];
            double[] pytDistances = new double[App.GameObjects.Count];

            for (int i = 0; i < App.GameObjects.Count; i++)
            {
                if (App.GameObjects[i] != this)
                {
                    yDistances[i] = yPos - App.GameObjects[i].yPos;
                    xDistances[i] = xPos - App.GameObjects[i].xPos;
                    pytDistances[i] = Math.Sqrt(Math.Pow(yDistances[i], 2) + Math.Pow(xDistances[i], 2));
                }
            }
            int lowestYDiff = yDistances.Min();
            int lowestXDiff = xDistances.Min();

            foreach (GameObject gameObject in App.GameObjects)
            {
                if (lowestXDiff <= lowestYDiff && (xPos - gameObject.xPos) == lowestXDiff)
                {
                    return gameObject;
                }
                else if (lowestYDiff <= lowestXDiff && (yPos - gameObject.yPos) == lowestYDiff)
                {
                    return gameObject;
                }
                if (xPos - gameObject.xPos == lowestXDiff || yPos - gameObject.yPos == lowestYDiff)
                {
                    return gameObject;
                }
            }
            return App.NullObject;
        }

        public void setObjectMovement(int yMove, int xMove)
        {
            yMovement = yMove;
            xMovement = xMove;
        }
    }
}

