# DevBridgeSquares

## Functionality:

+ Create a list of points.
+ Show all point lists.
+ Show points in list.
+ Add a point to list.
+ Delete point.
+ Import points from a file to current/new list.
+ Clear list.
+ Delete list.
+ Download list to .txt file.
+ Find all squares in current list.

## Issues / solutions ( basically a TODO list ):

+ (Problem) PointRepository shouldn't contain logic that manages lists.
+ (Solution) ListRepository that would contain simple lists and an additional point to Point model with list Id. 

+ (Problem) Some constants are defined in services/repositories and are repeating themselves. 
+ (Solution) Global config file should do it.

+ (Problem) Find squares method might be slow/basic.
+ (Solution) Check for potential optimizations

## Major issues ( what was i thinking ):

+ (Problem) When a list of squares is presented, paging through it makes server recalculate squares.
+ (Solution) Store squares data clientside. 
+ (Solution) Store additional list of squares that is tied to points it was calculated from, manage updates by checking when points were last modified.

