using DesignPatterns.Visitor;

var useCase = new PrintToConsoleUseCases();

useCase.Print([new Rectangle(5, 4), new Circle(5), new Triangle(3, 4, 5)]);

var calculateArea = new MatchFigure<double>(
    circle => Math.PI * Math.Pow(circle.R, 2),
    rectangle => rectangle.A * rectangle.B,
    triangle =>
    {
        var p = (triangle.A + triangle.B + triangle.C) / 2;
        return Math.Sqrt(p * (p - triangle.A) * (p - triangle.B) * (p - triangle.C));
    });

var area = new Rectangle(5, 4).Accept(calculateArea);