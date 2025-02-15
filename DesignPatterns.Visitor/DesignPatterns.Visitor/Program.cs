﻿using DesignPatterns.Visitor;

var useCase = new PrintToConsoleUseCases();

useCase.Print([new Rectangle(5, 4), new Circle(5), new Triangle(1, 2, 3)]);
