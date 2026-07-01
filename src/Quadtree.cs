using System.Numerics;
using Raylib_cs;
using static Main.Global;

class Quadtree(Rectangle bounds)
{
    Rectangle Boundary = bounds; // What part the quadtree covers
    List<Vector2> Points = []; // Points in the Quadtree
    bool Divided = false; // If the Point has or hasn't been subdivided

    // Other variables
    const int CAPACITY = 4; // make number of points in a section
    int Depth = 0; // Division counter
    int MAXDEPTH = 10000; // Max amount of divisions that you are able to do

    // Quadtree Functions
    public bool Insert(Vector2 point)
    {
        // Does the Quadtree have the point?
        if (!Raylib.CheckCollisionPointRec(point, Boundary)) return false; // No? then leave the function


        if (Points.Count < CAPACITY) // Points is less than capacity
        {
            // Insert the point
            Points.Add(point);
            return true;
        } 
        else 
        {
            if (!Divided && Depth <= MAXDEPTH) // otherwise subdivide if it hasn't been divided before 
            {
                Subdivide(point);
            }

            if      (    TopLeft?.Insert(point) == true) return true;
            else if (   TopRight?.Insert(point) == true) return true;
            else if ( BottomLeft?.Insert(point) == true) return true;
            else if (BottomRight?.Insert(point) == true) return true;
        }

        return false;
    }

    private float X,Y,W,H; // predefine

    // Get the Boundaries for drawing
    private Rectangle TopLeftB, TopRightB, BottomLeftB, BottomRightB;

    // Get the Trees
    private Quadtree? TopLeft, TopRight, BottomLeft, BottomRight;
    // Set its subtrees in the SUBDIVIDE function
    protected void Subdivide(Vector2 point) // take point from insert function
    {
        
        // Define variables for bounds / readability
        // NOTE: RECTANGLES START AT TOP LEFT CORNER IN RAYLIB
        X = Boundary.Center.X;
        Y = Boundary.Center.Y;
        W = Boundary.Width  / 2; // Not Full Width
        H = Boundary.Height / 2; // Not Full Height

        // Create 4 new Quadtrees Top-Left, Top-Right, Bottom-Left, Bottom-Right
        // Then Insert Points for each one

        // North West
        TopLeftB = new(X - W, Y - H, W, H);
        TopLeft =      new(TopLeftB);
        TopLeft.Insert(point);
        
        // North East
        TopRightB = new(X    , Y - H, W, H);
        TopRight =     new(TopRightB);
        TopRight.Insert(point);

        // South West
        BottomLeftB = new(X - W, Y, W, H);
        BottomLeft =   new(BottomLeftB);
        BottomLeft.Insert(point);

        // South East
        BottomRightB = new(X,Y, W, H);
        BottomRight =  new(BottomRightB);
        BottomRight.Insert(point);

        Divided = true; // it is now subdivided

        Depth++; // increase depth counter
    }

    public void DebugDraw()
    {
        // Draw the Original
        Raylib.DrawRectangleLinesEx(Boundary, 0.5f, Color.White);

        // If it has been divided draw its children
        if (Divided == true)
        {
            TopLeft?.DebugDraw();
            TopRight?.DebugDraw();
            BottomLeft?.DebugDraw();
            BottomRight?.DebugDraw();
        }
        
    }
    
    // QUERYING THE QUADTREE
    List<Vector2> Found = [];
    public List<Vector2> QueryArea(Rectangle area)
    {
        Found = []; // Reset Found points

        // if it doesn't intersect with the boundary return the empty list
        if (!Raylib.CheckCollisionRecs(area, Boundary)) return Found;

        // Loop through its points if it intersects the area
        Points.ForEach(delegate(Vector2 p) // lamba function -> using a delegate                                     
        {                                  // Found this trick in the Microsoft Docs for C#

            // if the point collides with the rectangle -> add it to the found list
            if (Raylib.CheckCollisionPointRec(p, area)) Found.Add(p);
        });

        // if its divided add its children query into the query
        if (Divided)
        {
            // Check if its not null then add range
            if (TopLeft     != null) Found.AddRange(TopLeft.    QueryArea(area)); 
            if (TopRight    != null) Found.AddRange(TopRight.   QueryArea(area));
            if (BottomLeft  != null) Found.AddRange(BottomLeft. QueryArea(area));
            if (BottomRight != null) Found.AddRange(BottomRight.QueryArea(area));
        }

        return Found; // return the found points
    }
    public List<Vector2> QueryCircle(Vector2 Center, float Radius)
    {
        Found = []; // Reset Found points

        // if it doesn't intersect with the boundary return the empty list
        if (!Raylib.CheckCollisionCircleRec(Center,Radius,Boundary)) return Found;

        // Loop through its points if it intersects the area
        Points.ForEach(delegate(Vector2 p) // lamba function -> using a delegate                                     
        {                                  // Found this trick in the Microsoft Docs for C#

            // if the point collides with the rectangle -> add it to the found list
            if (Raylib.CheckCollisionPointCircle(p,Center,Radius)) Found.Add(p);
        });

        // if its divided add its children query into the query
        if (Divided)
        {
            // Check if its not null then add range
            if (TopLeft     != null) Found.AddRange(TopLeft.    QueryCircle(Center, Radius)); 
            if (TopRight    != null) Found.AddRange(TopRight.   QueryCircle(Center, Radius));
            if (BottomLeft  != null) Found.AddRange(BottomLeft. QueryCircle(Center, Radius));
            if (BottomRight != null) Found.AddRange(BottomRight.QueryCircle(Center, Radius));
        }

        return Found; // return the found points
    }
    // CLEARING THE QUADTREE
    public void Clear()
    {
        Points = []; // Clear Points in original quadtree
        Divided = false; // it has not been divided
        Depth = 0; // reset depth

        // Clear Children 
        TopLeft?.Clear();
        TopRight?.Clear();
        BottomLeft?.Clear();
        BottomRight?.Clear();
    }
}