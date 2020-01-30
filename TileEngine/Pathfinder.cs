using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TileEngine
{
    public class SearchNode
    {
        public SearchNode[] Neighbours;
        public SearchNode Parent;

        public Point Position;

        public float DistanceToTarget;
        public float DistanceTravelled;
        public bool Walkable;
        public bool InOpenList;
        public bool InClosedList;
    }

    public class Pathfinder
    {
        public SearchNode[,] Nodes;

        private List<SearchNode> openList;
        private List<SearchNode> closedList;

        private int levelWidth;
        private int levelHeight;

        public Pathfinder()
        {
            levelWidth = TileMap.CollisionLayer.Width;
            levelHeight = TileMap.CollisionLayer.Height;

            openList = new List<SearchNode>();
            closedList = new List<SearchNode>();

            //Create nodes
            Nodes = new SearchNode[levelWidth, levelHeight];
            for (int x = 0; x < levelWidth; x++)
            {
                for (int y = 0; y < levelHeight; y++)
                {
                    SearchNode node = new SearchNode();
                    node.Position = new Point(x, y);
                    node.Walkable = TileMap.CollisionLayer.GetCellIndex(new Point(x, y)) == 0;

                    if (node.Walkable == true)
                    {
                        node.Neighbours = new SearchNode[8];
                        Nodes[x, y] = node;
                    }
                }
            }

            //Populate neighbours
            for (int x = 0; x < levelWidth; x++)
            {
                for (int y = 0; y < levelHeight; y++)
                {
                    SearchNode node = Nodes[x, y];

                    if (node == null || node.Walkable == false)
                        continue;

                    Point[] neighbours = new Point[]
                    {
                        new Point (x, y - 1), //Top
                        new Point (x, y + 1), //Bottom
                        new Point (x - 1, y), //Left
                        new Point (x + 1, y), //Right

                        new Point (x - 1, y - 1), //TopLeft
                        new Point (x + 1, y - 1), //TopRight
                        new Point (x - 1, y + 1), //BottomLeft
                        new Point (x + 1, y + 1), //BottomRight
                    };

                    //Loop through neighbours
                    for (int i = 0; i < neighbours.Length; i++)
                    {
                        Point position = neighbours[i];

                        if (position.X < 0 || position.X > levelWidth - 1 ||
                            position.Y < 0 || position.Y > levelHeight - 1)
                            continue;

                        SearchNode neighbour = Nodes[position.X, position.Y];

                        if (neighbour == null || neighbour.Walkable == false)
                            continue;

                        node.Neighbours[i] = neighbour;
                    }
                }
            }
        }

        private float Heuristic(Point p1, Point p2)
        {
            return Math.Abs(p1.X - p2.X) +
                   Math.Abs(p1.Y - p2.Y);
        }

        private void ResetSearchNodes()
        {
            openList.Clear();
            closedList.Clear();

            for (int x = 0; x < levelWidth; x++)
            {
                for (int y = 0; y < levelHeight; y++)
                {
                    SearchNode node = Nodes[x, y];

                    if (node == null)
                        continue;

                    node.InOpenList = false;
                    node.InClosedList = false;

                    node.DistanceTravelled = float.MaxValue;
                    node.DistanceToTarget = float.MaxValue;
                }
            }
        }

        private SearchNode FindBestNode()
        {
            SearchNode currentTile = openList[0];

            float smallestDistanceToTarget = float.MaxValue;

            // Find closest node
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].DistanceToTarget < smallestDistanceToTarget)
                {
                    currentTile = openList[i];
                    smallestDistanceToTarget = currentTile.DistanceToTarget;
                }
            }
            return currentTile;
        }

        private List<Vector2> FindFinalPath(SearchNode startNode, SearchNode endNode)
        {
            closedList.Add(endNode);

            SearchNode parentTile = endNode.Parent;

            // Trace back through the nodes using the parent fields
            // to find the best path
            while (parentTile != startNode)
            {
                closedList.Add(parentTile);
                parentTile = parentTile.Parent;
            }

            List<Vector2> finalPath = new List<Vector2>();

            // Reverse the path and transform into world space.
            for (int i = closedList.Count - 1; i >= 0; i--)
            {
                finalPath.Add(new Vector2((closedList[i].Position.X * Engine.TileWidth) + ((Engine.TileWidth / 2)),
                                          (closedList[i].Position.Y * Engine.TileHeight) + ((Engine.TileHeight / 2))));
            }

            finalPath.Add(new Vector2((startNode.Position.X * Engine.TileWidth) + ((Engine.TileWidth / 2)),
                                      (startNode.Position.Y * Engine.TileHeight) + ((Engine.TileHeight / 2))));

            return finalPath;
        }

        /// <summary>
        /// Finds the optimal path from one point to another.
        /// </summary>
        public List<Vector2> FindPath(Point startPoint, Point endPoint)
        {
            // Only try to find a path if the start and end points are different.
            if (startPoint == endPoint)
            {
                return new List<Vector2>();
            }

            /////////////////////////////////////////////////////////////////////
            // Step 1 : Clear the Open and Closed Lists and reset each node’s F 
            //          and G values in case they are still set from the last 
            //          time we tried to find a path. 
            /////////////////////////////////////////////////////////////////////
            ResetSearchNodes();

            // Store references to the start and end nodes for convenience.
            SearchNode startNode = Nodes[startPoint.X, startPoint.Y];
            SearchNode endNode = Nodes[endPoint.X, endPoint.Y];

            /////////////////////////////////////////////////////////////////////
            // Step 2 : Set the start node’s G value to 0 and its F value to the 
            //          estimated distance between the start node and goal node 
            //          (this is where our H function comes in) and add it to the 
            //          Open List. 
            /////////////////////////////////////////////////////////////////////
            startNode.InOpenList = true;

            startNode.DistanceToTarget = Heuristic(startPoint, endPoint);
            startNode.DistanceTravelled = 0;

            openList.Add(startNode);

            /////////////////////////////////////////////////////////////////////
            // Setp 3 : While there are still nodes to look at in the Open list : 
            /////////////////////////////////////////////////////////////////////
            while (openList.Count > 0)
            {
                /////////////////////////////////////////////////////////////////
                // a) : Loop through the Open List and find the node that 
                //      has the smallest F value.
                /////////////////////////////////////////////////////////////////
                SearchNode currentNode = FindBestNode();

                /////////////////////////////////////////////////////////////////
                // b) : If the Open List empty or no node can be found, 
                //      no path can be found so the algorithm terminates.
                /////////////////////////////////////////////////////////////////
                if (currentNode == null)
                    break;

                /////////////////////////////////////////////////////////////////
                // c) : If the Active Node is the goal node, we will 
                //      find and return the final path.
                /////////////////////////////////////////////////////////////////
                if (currentNode == endNode)
                {
                    // Trace our path back to the start.
                    return FindFinalPath(startNode, endNode);
                }

                /////////////////////////////////////////////////////////////////
                // d) : Else, for each of the Active Node’s neighbours :
                /////////////////////////////////////////////////////////////////
                for (int i = 0; i < currentNode.Neighbours.Length; i++)
                {
                    SearchNode neighbour = currentNode.Neighbours[i];

                    //////////////////////////////////////////////////
                    // i) : Make sure that the neighbouring node can 
                    //      be walked across. 
                    //////////////////////////////////////////////////
                    if (neighbour == null || neighbour.Walkable == false)
                    {
                        continue;
                    }

                    //////////////////////////////////////////////////
                    // ii) Calculate a new G value for the neighbouring node.
                    //////////////////////////////////////////////////
                    float distanceTraveled = currentNode.DistanceTravelled + 1;

                    // An estimate of the distance from this node to the end node.
                    float heuristic = Heuristic(neighbour.Position, endPoint);

                    //////////////////////////////////////////////////
                    // iii) If the neighbouring node is not in either the Open 
                    //      List or the Closed List : 
                    //////////////////////////////////////////////////
                    if (neighbour.InOpenList == false && neighbour.InClosedList == false)
                    {
                        // (1) Set the neighbouring node’s G value to the G value 
                        //     we just calculated.
                        neighbour.DistanceTravelled = distanceTraveled;
                        // (2) Set the neighbouring node’s F value to the new G value + 
                        //     the estimated distance between the neighbouring node and
                        //     goal node.
                        neighbour.DistanceToTarget = distanceTraveled + heuristic;
                        // (3) Set the neighbouring node’s Parent property to point at the Active 
                        //     Node.
                        neighbour.Parent = currentNode;
                        // (4) Add the neighbouring node to the Open List.
                        neighbour.InOpenList = true;
                        openList.Add(neighbour);
                    }
                    //////////////////////////////////////////////////
                    // iv) Else if the neighbouring node is in either the Open 
                    //     List or the Closed List :
                    //////////////////////////////////////////////////
                    else if (neighbour.InOpenList || neighbour.InClosedList)
                    {
                        // (1) If our new G value is less than the neighbouring 
                        //     node’s G value, we basically do exactly the same 
                        //     steps as if the nodes are not in the Open and 
                        //     Closed Lists except we do not need to add this node 
                        //     the Open List again.
                        if (neighbour.DistanceTravelled > distanceTraveled)
                        {
                            neighbour.DistanceTravelled = distanceTraveled;
                            neighbour.DistanceToTarget = distanceTraveled + heuristic;

                            neighbour.Parent = currentNode;
                        }
                    }
                }

                /////////////////////////////////////////////////////////////////
                // e) Remove the Active Node from the Open List and add it to the 
                //    Closed List
                /////////////////////////////////////////////////////////////////
                openList.Remove(currentNode);
                currentNode.InClosedList = true;
            }

            // No path could be found.
            return new List<Vector2>();
        }
    }
}
