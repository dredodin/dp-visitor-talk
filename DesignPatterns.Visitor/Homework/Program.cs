using Homework;

int _width = 20;
int _height = 10;

var hero = new Hero { X = _width / 2, Y = _height / 2 };

List<IGameObject> objects =
[
    new Fruit { X = Random.Shared.Next(_width), Y = Random.Shared.Next(_height) },
    new Fruit { X = Random.Shared.Next(_width), Y = Random.Shared.Next(_height) },
    new Fruit { X = Random.Shared.Next(_width), Y = Random.Shared.Next(_height) },
    new Monster(1) { X = Random.Shared.Next(_width), Y = Random.Shared.Next(_height) },
    new Monster(2) { X = Random.Shared.Next(_width), Y = Random.Shared.Next(_height) },
    new Monster(3) { X = Random.Shared.Next(_width), Y = Random.Shared.Next(_height) }
];

for (int i = 0; ; i++)
{
    if (i % 5 == 0)
    {
        objects.Add(new Fruit { X = Random.Shared.Next(_width), Y = Random.Shared.Next(_height) });
    }
    if (i % 10 == 0)
    {
        objects.Add(new Monster(hero.Strength * 2) { X = Random.Shared.Next(_width), Y = Random.Shared.Next(_height) });
    }

    Console.Clear();
    Draw(hero, objects);
    var key = Console.ReadKey(true).Key;
    hero.Move(key, _width, _height);

    foreach (var obj in objects.ToList())
    {
        if (obj is Monster monster)
        {
            if (Random.Shared.Next(2) == 0)
            {
                if (hero.X > monster.X) monster.X++;
                else if (hero.X < monster.X) monster.X--;
            }
            else
            {
                if (hero.Y > monster.Y) monster.Y++;
                else if (hero.Y < monster.Y) monster.Y--;
            }

            monster.X = (monster.X + _width) % _width;
            monster.Y = (monster.Y + _height) % _height;

            if (hero.X == monster.X && hero.Y == monster.Y)
            {
                if (hero.Strength >= monster.Strength)
                {
                    hero.Strength++;
                    objects.Remove(obj);
                }
                else
                {
                    Console.WriteLine("Game Over!");
                    Console.ReadLine();
                    return;
                }
            }
        }
        else if (obj is Fruit fruit)
        {
            if (hero.X == fruit.X && hero.Y == fruit.Y)
            {
                hero.Strength++;
                objects.Remove(fruit);
            }
        }
    }

    Thread.Sleep(200);
}

void Draw(Hero hero, List<IGameObject> objects)
{
    var field = new char[_height, _width];

    for (int y = 0; y < _height; y++)
    {
        for (int x = 0; x < _width; x++)
        {
            field[y, x] = '.';
        }
    }

    field[hero.Y, hero.X] = hero.Symbol;

    foreach (var obj in objects)
    {
        field[obj.Y, obj.X] = obj switch
        {
            Monster m => (char)('0' + m.Strength),
            Fruit => '0',
            _ => '?'
        }; ;
    }

    for (int y = 0; y < _height; y++)
    {
        for (int x = 0; x < _width; x++)
        {
            if (field[y, x] == hero.Symbol)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (objects.Any(m => m.X == x && m.Y == y))
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ResetColor();
            }
            Console.Write(field[y, x]);
        }
        Console.WriteLine();
    }

    Console.ResetColor();
    Console.WriteLine($"Hero Strength: {hero.Strength}");
}
