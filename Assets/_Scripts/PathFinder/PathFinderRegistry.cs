using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PathFinderRegistry
{
    private static List<PathFinder> _pathFinders = new List<PathFinder>();
    
    // Registers a PathFinder instance if it is not already registered
    public static void RegisterPathFinder(PathFinder pathFinder)
    {
        if (!_pathFinders.Contains(pathFinder))
        {
            _pathFinders.Add(pathFinder);
        }
    }
    // Unregisters a PathFinder instance
    public static void UnregisterPathFinder(PathFinder pathFinder)
    {
        _pathFinders.Remove(pathFinder);
    }
    // Returns a list of all registered PathFinder instances
    public static List<PathFinder> GetAllPathFinders()
    {
        return _pathFinders;
    }
    // Returns an array of all registered PathFinder instances
    public static PathFinder[] GetAllPathFindersArray()
    {
        return _pathFinders.ToArray();
    }
}

