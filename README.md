# Introduction 
In the courses Computer Graphics and Software Engineering we have to develop a game using OpenGL. We decided on a medieval strategy game where you have to defeat the enemy fortress.
The enemy has towers and armies that make the way difficult. You also have towers and armies that you can use to attack.

# Getting Started
1. Clone git repository to your local machine.
Note: The project '2D-Strategy' is the prototype of the game. The actual game can be found in the project 'StrategyGame'.
2. Open the Visual Studio project. You can find it at `StrategyGame/2D-Strategy/2D-Strategy.sln`
3. Download packages needed for the project.
    Right click on `Solution '2D-Strategy' (4 projects)` and select `Manage NuGet Packages for Solution...`.
    At the top a message should show up where you can restore the packages.
4. Select `StrategyGame` as the startup project and run the game.

# Build & Test
To build the project, right click the project `StrategyGame` and select `Build`.
The tests for the game can be found in the project `StrategyGame-Test`. To run the tests, right click the project `StrategyGame-Test` and select `Run Unit Tests`.

# What I have learned
- Working in a team
- Working in a agile approache to software development
- Planing the project with scrum and carry out sprint planning (manage backlogitems, tasks, etc.), sprint reviews
- Defining a Definition of Done
- Using a build server (team foundation server) for continous integration (code analysis, unit tests, code coverage)
- Performing the software engineering phases analysis, software design, implementation and tests.
- Using versioning (git) in a team with branching strategy
- Implementing MVC design pattern and seeing its strengths and weaknesses
- Implementing ECS design pattern (very usefull for a game architecture)
- Implementing other design patterns like factory pattern, service provider pattern, etc.
- Creating a prototyp of a game
- Understanding the math behind computer graphics
- Using OpenGL for rendering
- Using Tiled and TiledSharp to creating individual maps
- Implementing pathfinding with A* algorithm