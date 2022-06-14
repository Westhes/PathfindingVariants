using System;
using System.Collections;
using System.Collections.Generic;

public interface IPathfindAlgorithm
{
    public int MapSize { set; }
    public void Search(Node start, Node finish);
    public Node[] SearchPath(Node start, Node finish);

    public IEnumerator DebugTravelPath(Node goal, float delayInSeconds);

    public void ResetPath(Node goal);
}
