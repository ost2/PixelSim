using System;
namespace MovementTest
{
    public class Program
    {
        public static void Main(string[]args)
        {
            Application testGame = new Application(30, 75, false, true);

            

            var rand = new Random();
            var enemySymbol = '*';
            var runnerSymbol = '*';
            var followerSymbol = '+';
            List<char> zombieSymbols = new List<char>(7);
            zombieSymbols.Add('˙');
            zombieSymbols.Add('•');
            zombieSymbols.Add('°');
            zombieSymbols.Add('.');
            zombieSymbols.Add('•');
            zombieSymbols.Add('+');
            zombieSymbols.Add('*');

            GameObject enemy = new GameObject(testGame, enemySymbol, 1, ConsoleColor.Red, true, true, true);

            GameObject runner = new GameObject(testGame, runnerSymbol, 1, ConsoleColor.Yellow, true, false, false, true);

            GameObject follower = new GameObject(testGame, followerSymbol, 1, ConsoleColor.DarkMagenta, true, false, false, false, true);

            GameObject zombie = new GameObject(testGame, zombieSymbols[testGame.Rand.Next(0, zombieSymbols.Count)], 1, ConsoleColor.DarkGreen, true, false, false, false, true, true);

            GameObject cell = new GameObject(testGame, testGame.ScreenHeight / 2, testGame.ScreenWidth / 2);

            void zombieApocalypse(int zN, int rN)
            {
                for (int i = 0; i < zN; i++)
                {
                    testGame.addGameObject(new GameObject(zombie, testGame.ScreenHeight/2, testGame.ScreenWidth/2, zombieSymbols));
                }
                for (int i = 0; i < rN; i++)
                {
                    testGame.addGameObject(new GameObject(runner, rand.Next(1, testGame.ScreenHeight), rand.Next(1, testGame.ScreenWidth)));
                }
            }

            void runnerChaser(int rN, int cN)
            {
                for (int i = 0; i < rN; i++)
                {
                    testGame.addGameObject(new GameObject(runner, rand.Next(1, testGame.ScreenHeight), rand.Next(1, testGame.ScreenWidth)));
                }
                for (int i = 0; i < cN; i++)
                {
                    ConsoleColor color = ConsoleColor.DarkBlue + rand.Next(0, 14);
                    testGame.addGameObject(new GameObject(follower, rand.Next(1, testGame.ScreenHeight), rand.Next(1, testGame.ScreenWidth)));
                }
            }

            void cellularAutomata(int cN)
            {
                if (cN == 0)
                {
                    testGame.addGameObject(new GameObject(cell, 5, 5));
                    testGame.addGameObject(new GameObject(cell, 5, 6));
                    testGame.addGameObject(new GameObject(cell, 5, 7));
                    testGame.addGameObject(new GameObject(cell, 6, 5));
                    testGame.addGameObject(new GameObject(cell, 15, 5));
                    testGame.addGameObject(new GameObject(cell, 14, 5));
                    testGame.addGameObject(new GameObject(cell, 4, 5));
                    testGame.addGameObject(new GameObject(cell, 15, 6));
                }
                else
                {
                    for (int i = 0; i < cN; i++)
                    {
                        testGame.addGameObject(new GameObject(cell, rand.Next(5, testGame.ScreenHeight - 5), rand.Next(5, testGame.ScreenWidth - 5)));
                    }
                }

            }


            //zombieApocalypse(3, 120);

            //runnerChaser(30, 130);

            cellularAutomata(400);

            testGame.runGame();
        }
    }
}


