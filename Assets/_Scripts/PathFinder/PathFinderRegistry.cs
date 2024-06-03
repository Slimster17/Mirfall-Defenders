using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathFinderRegistry
{
    private static List<PathFinder> _pathFinders = new List<PathFinder>();

    public static void RegisterPathFinder(PathFinder pathFinder)
    {
        if (!_pathFinders.Contains(pathFinder))
        {
            _pathFinders.Add(pathFinder);
        }
    }

    public static void UnregisterPathFinder(PathFinder pathFinder)
    {
        _pathFinders.Remove(pathFinder);
    }

    public static List<PathFinder> GetAllPathFinders()
    {
        return _pathFinders;
    }
    
    public static PathFinder[] GetAllPathFindersArray()
    {
        return _pathFinders.ToArray();
    }
}

