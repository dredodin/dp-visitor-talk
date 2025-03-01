namespace ProductionExamples;

public sealed record CellLocation(int Row, int Column, string SheetName);

public sealed record RangeLocation(CellLocation TopLeft, CellLocation BottomRight);
