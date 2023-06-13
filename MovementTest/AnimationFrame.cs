using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MovementTest
{
	public struct AnimationFrame
	{
        public int Width { get; set; }
        public int Height { get; set; }
		public List<List<char>> AllLines { get; set; }


		public AnimationFrame(int height, int width)
		{
			Height = height;
			Width = width;
            AllLines = new List<List<char>>();

            for (int i = 0; i < height; i++)
			{               
                AllLines.Add(new List<char>(new char[Width]));

                for (int j = 0; j < Width; j++)
                {
					AllLines[i][j] = ' '; 
                }
            }
		}

		public AnimationFrame(AnimationFrame frame)
		{
			Height = frame.Height;
			Width = frame.Width;
            AllLines = new List<List<char>>();

            for (int i = 0; i < Height; i++)
            {
                AllLines.Add(new List<char>(new char[Width]));

                for (int j = 0; j < Width; j++)
                {
                    AllLines[i][j] = frame.getPixel(i, j);
                }
            }
        }

		char getPixel(int h, int w)
		{
			return AllLines[h][w];
		}

		public void addPixel(int h, int w, char symbol)
		{
			AllLines[h][w] = symbol;
		}

		public void addHorizontalLine(int h, char symbol, int shortenBy = 0, char left = ' ', char right = ' ')
		{
			for (int i = shortenBy; i < Width - shortenBy; i++)
			{
				if (i == shortenBy)
				{
					AllLines[h][i] = left;
				}
				else if (i == Width - shortenBy - 1)
				{
					AllLines[h][i] = right;				}
				else
				{
                    AllLines[h][i] = symbol;
                }

			}
		}

        public void addVerticalLine(int w, char symbol)
        {
            for (int i = 0; i < Height; i++)
            {
                AllLines[i][w] = symbol;
            }
        }

		public void clearFrame()
		{
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    AllLines[i][j] = ' ';
                }
            }
        }

        public void printFrame(int ms, bool randomColors = false, ConsoleColor drawColor = ConsoleColor.White, int colorH = 0, bool clearFrame = true)
		{
			if (clearFrame)
			{
                Console.Clear();
            }

			ConsoleColor color;

            for (int i = 0; i < Height; i++)
			{
				if (randomColors)
				{
					Random rand = new Random();
					int colorNumber = rand.Next(0, 14);
					if (colorNumber == 8 || colorNumber == 7 || colorNumber == 15)
					{
						colorNumber += 1;
					}
					color = ConsoleColor.DarkBlue + colorNumber;
				}
				else
				{
					if (colorH == 0 || i >= colorH)
					{
						color = drawColor;
					}
					else color = ConsoleColor.White;
				}
				string pixelRow = new string(AllLines[i].ToArray());
				Writer.output(pixelRow, 0, 0, color);
			}
			Thread.Sleep(ms);
		}

		public void addSymbolImage(SymbolImage image, int h, int w)
		{
			int i = h;
            foreach (string s in image.FullImage)
            {
                int j = w;
                foreach (char c in s)
                {
					addPixel(i, j, c);
					j++;
                }
                i++;
            }
        }
	}
}

