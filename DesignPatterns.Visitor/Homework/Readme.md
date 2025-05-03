# Game Overview

This game is a simple console-based adventure game where the player navigates through a grid, encountering various objects and obstacles.

## Game Rules

1. The player can move up, down, left, or right.
2. The player may fight monsters gather fruit.

## Features

- Grid-based movement
- Various obstacles with different effects
- Console-based display of the game state

## Homework Assignment

1. Uncomment the `Obstacle` class in the `GameObjects` file
2. Add several Obstacles into `objects` list in `Program` file.
3. Implement the game logic for the `Obstacle` class correctly.
4. Apply the Visitor pattern to refactor the movement, collision, and display logic using the following visitors:
   - `MovementVisitor`
   - `CollisionVisitor`
   - `PrintToConsoleVisitor`
