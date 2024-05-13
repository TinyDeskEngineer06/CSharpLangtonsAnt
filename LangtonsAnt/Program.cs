namespace LangtonsAnt;

class Program
{
    class Ant
    {
        public enum Direction : sbyte
        {
            North,
            East,
            South,
            West
        }

        bool[,] grid;

        int size;

        int x;
        int y;

        Direction direction = Direction.West;

        public Direction MoveDirection
        {
            get => direction;
            private set
            {
                direction = value switch
                {
                    (Direction)(-1) => Direction.West,
                    (Direction)(4)  => Direction.North,
                    _ => value
                };
            }
        }

        int steps = 0;

        public int Steps => steps;

        public Ant(int size)
        {
            grid = new bool[size, size];

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    grid[x, y] = true;
                }
            }

            this.size = size;

            x = (int)MathF.Round(size / 2f, MidpointRounding.ToZero);
            y = x;
        }

        public void ShowGrid()
        {
            Console.SetCursorPosition(0, 0);

            ConsoleColor fg = Console.ForegroundColor;
            ConsoleColor bg = Console.BackgroundColor;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Console.ForegroundColor = GetColorFG(x, y);
                    Console.BackgroundColor = GetColorBG(x, y);

                    if (x == this.x && y == this.y)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;

                        Console.Write(direction switch
                        {
                            Direction.North => '^',
                            Direction.East  => '>',
                            Direction.South => 'v',
                            Direction.West  => '<',
                            _ => '�'
                        });

                        continue;
                    }

                    Console.Write('.');
                }

                Console.WriteLine();
            }

            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;
        }

        public ConsoleColor GetColorFG(int x, int y)
        {
            return grid[x, y] switch
            {
                false => ConsoleColor.White,
                true  => ConsoleColor.Black
            };
        }

        public ConsoleColor GetColorBG(int x, int y)
        {
            return grid[x, y] switch
            {
                false => ConsoleColor.Black,
                true  => ConsoleColor.White
            };
        }

        public void Step()
        {
            int offX, offY;

            switch (grid[x, y])
            {
                case false:
                    MoveDirection--;
                    break;
                case true:
                    MoveDirection++;
                    break;
            }

            switch (direction)
            {
                case Direction.North:
                    offX = 0;
                    offY = -1;
                    break;
                case Direction.East:
                    offX = 1;
                    offY = 0;
                    break;
                case Direction.South:
                    offX = 0;
                    offY = 1;
                    break;
                case Direction.West:
                    offX = -1;
                    offY = 0;
                    break;
                default:
                    offX = 0;
                    offY = 0;
                    break;
            }

            grid[x, y] = !grid[x, y];

            x += offX;
            y += offY;

            x = Math.Clamp(x, 0, size - 1);
            y = Math.Clamp(y, 0, size - 1);

            steps++;
        }
    }

    static void Main(string[] args)
    {
        Console.Write("Enter grid size: ");

        string? response = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(response))
        {
            Console.WriteLine("No value entered. Press any key to exit.");

            Console.ReadKey(true);
            return;
        }

        if (!int.TryParse(response, out int size))
        {
            Console.WriteLine($"\"{response}\" could not be parsed as an integer. Press any key to exit.");

            Console.ReadKey(true);
            return;
        }

        if (size < 1)
        {
            Console.WriteLine($"{size} is below the grid size limit. Press any key to exit.");

            Console.ReadKey(true);
            return;
        }

        Console.Write("Enter starting step count: ");

        response = Console.ReadLine();
        int steps = 0;

        if (string.IsNullOrWhiteSpace(response)) goto StartSim;

        if (!int.TryParse(response, out steps))
        {
            steps = 0;
        }

        StartSim:

        Ant ant = new Ant(size);

        for (int i = 0; i < steps; i++)
        {
            ant.Step();
        }

        Console.Clear();

        while (true)
        {
            ant.ShowGrid();
            Console.WriteLine(ant.MoveDirection);
            Console.WriteLine(ant.Steps);

            ant.Step();
        }
    }
}