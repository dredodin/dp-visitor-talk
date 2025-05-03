namespace Homework;

public record Hero
{
    public char Symbol { get; } = 'X';
    public int Strength { get; set; } = 1;
    public int X { get; set; }
    public int Y { get; set; }

    public void Move(ConsoleKey key, int maxWidth, int maxHeight)
    {
        switch (key)
        {
            case ConsoleKey.LeftArrow:
                X = (X - 1 + maxWidth) % maxWidth;
                break;
            case ConsoleKey.RightArrow:
                X = (X + 1) % maxWidth;
                break;
            case ConsoleKey.UpArrow:
                Y = (Y - 1 + maxHeight) % maxHeight;
                break;
            case ConsoleKey.DownArrow:
                Y = (Y + 1) % maxHeight;
                break;
        }
    }
}
