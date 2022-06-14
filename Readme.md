# Pathfinding variants
This project focuses on different pathfinding variants, including an unique variant I made during my internship.

![Dijkstra](https://i.imgur.com/9vZ8zlO.png)
## Dijkstra
</br></br>

![A*](https://i.imgur.com/m14E6CY.png)
## Weighted A* (pwXU)
</br></br>

![Depth-First Search](https://i.imgur.com/UCfOsXz.png)
## Normal Depth-First Search algorithm
</br></br>

![Custom Depth-First Search algorithm.](https://i.imgur.com/E1qnAWP.png)
## Custom Depth-First Search algorithm.
Unlike the default Depth-First search algorithm, this variant has a depth limit and continues searching for alternative paths after finding the goal.
### Found 924 paths leading from start (green) to finish (red), with a depth of 13 in 76ms.
</br></br>

![Custom Depth-First Search algorithm with heuristic.](https://i.imgur.com/cbyGyQD.png)
## Custom Depth-First Search algorithm with heuristic.
An modification on the previous algorithm. Contains a Manhattan distance heuristic in order to check if the goal is still reachable with the remaining depth. 
### Found 924 paths leading from start (green) to finish (red), with a depth of 13 in 3ms.
</br></br>

# Sources:
- Heuristic types (A*) [https://www.movingai.com/SAS/SUB/](https://www.movingai.com/SAS/SUB/)
- Heuristics and Tiebreakers (A*) [http://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html](http://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html)