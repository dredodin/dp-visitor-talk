using DesignPatterns.Visitor;

var useCase = new PrintToConsoleUseCases();

useCase.Print([new Rectangle(5, 4), new Circle(5), new Triangle(3, 4, 5)]);

IFigure figure = new Rectangle(5, 4);

var area1 = figure.Accept(new MatchFigure<double>(
    circle => Math.PI * Math.Pow(circle.R, 2),
    rectangle => rectangle.A * rectangle.B,
    triangle =>
    {
        var p = (triangle.A + triangle.B + triangle.C) / 2;
        return Math.Sqrt(p * (p - triangle.A) * (p - triangle.B) * (p - triangle.C));
    })
);

var area2 = figure switch
{
    Circle { R: var r } => Math.PI * Math.Pow(r, 2),
    Rectangle { A: var a, B: var b } => a * b,
    Triangle { A: var a, B: var b, C: var c } => Math.Sqrt((a + b + c) / 2 * ((a + b + c) / 2 - a) * ((a + b + c) / 2 - b) * ((a + b + c) / 2 - c))
};