This repository serves as an example of how the `Visitor` pattern can be implemented. Each branch represents a step in the refactoring process.

## 01-init
- Add `IFigure` with implementations `Circle` and `Rectangle`
- Add `PrintToConsoleUseCases` using `is` pattern matching
## 02-triangle
- Add `Triangle` but forget of usecase
## 03-switch-pattern-matching
- Convert `is` to `switch`
## 04-visitor
- Temp comment `Triangle`
- Create `IFigureVisitor` and add method `Accept`
- Convert `switch` to `PrintToConsoleVisitor`
## 05-triangle-v2
- Add `Triangle` no way to forget of usecase
## 06-visitor-with-state
- Add `PublishAsyncVisitor` with state
## 07-visitor-with-currying
- Add `PublishAsyncVisitor` with currying
## 08-universal-visitor
- Add `MatchFigure`
- Add `calculateArea` example
## 08-compare-with-switch
- Add `calculateArea` logic via switch expression
