using System;
using System.Numerics;
using Raylib_cs;
using static Global;

class Global // define global variables here
{
    // WINDOW VARIABLES
    public const int WINW = 1024;
    public const int WINH = 512;
    public const float FULL_CIRCLE = 2 * MathF.PI; // Full Circle in radians
    public const float TO_DEGREE = 180 / MathF.PI; // conversion to degree multiplier
    public const float TO_RADIAN = MathF.PI / 180; // conversion to radian multiplier
    public static readonly Vector2 CENTER = new(WINW / 2, WINH / 2);
}

class Program
{
    struct Player
    {
        // Movement
        public Vector2 Position, Offset, Origin, Direction, Velocity;
        public float Speed = 250, Angle = 0, Width, Height; 

        // Visuals
        public Texture2D plane;
        public Rectangle Source, Dest;

        public Player(Texture2D texture, float scale)
        {
            // Setting up the plane
            Position = CENTER;
            Offset = this.Position;

            // visual setup
            plane = texture;
            Width = this.plane.Width;
            Height = this.plane.Height;

            // Source and Dest Rectangles
            Source = new(0,0, Width, Height);
            Dest = new(this.Position, this.Width / scale, this.Height / scale);
            Origin = new(Width / (scale * 2), Height / (scale * 2));
        }
    }
    public static void Main()
    {
        // Initialization
        Raylib.InitWindow(WINW, WINH, "Basic Window");
        Raylib.SetTargetFPS(60);
        float dt;

        // Player
        string path = Path.Combine("..","assets/plane.png");
        Texture2D plane = Raylib.LoadTexture(path);
        Player p = new(plane, 7);

        // Screen Bounds and other
        int padding = 25;
        Rectangle Screen = new(padding,padding,WINW-padding,WINH-padding);

        while (!Raylib.WindowShouldClose())
        {
            dt = Raylib.GetFrameTime(); // delta time

            // PLAYER MOVEMENT
            if (Raylib.IsKeyDown(KeyboardKey.Left) || Raylib.IsKeyDown(KeyboardKey.A)) p.Angle -= 0.02f;
            else if (Raylib.IsKeyDown(KeyboardKey.Right) || Raylib.IsKeyDown(KeyboardKey.D)) p.Angle += 0.02f;

            // Cap Angle
            if (p.Angle > FULL_CIRCLE) p.Angle = 0;
            else if (p.Angle < 0) p.Angle = FULL_CIRCLE;

            // Set Direction
            p.Direction.X = MathF.Cos(p.Angle);
            p.Direction.Y = MathF.Sin(p.Angle);

            // Set position and velocity
            p.Velocity += p.Direction * p.Speed * dt;
            p.Velocity *= 0.99f; // reduce velocity just a litte
            p.Position += p.Velocity * dt;
            p.Dest.Position = p.Position;

            // Cap bounds
            if (!Raylib.CheckCollisionCircleRec(p.Position, p.Width, Screen)) p.Velocity *= 0;

            // drawing
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            // Drawing the player
            Raylib.DrawTexturePro(p.plane, p.Source, p.Dest, p.Origin, (p.Angle * TO_DEGREE) + 90, Color.White);



            Raylib.EndDrawing();
        }

        // closing and unloading assets
        Raylib.UnloadTexture(plane);
        Raylib.CloseWindow();
    }
}