using System;
using System.Numerics;
using Raylib_cs;
using Particles;
using GUIComponentSystem;
using static Global;
using static ImageFunctions;

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

class ImageFunctions
{
    private static string combinedPath = "";
    public static Texture2D LoadTextureOutsideDirectory(string path)
    {
        combinedPath = Path.Combine("..", path);
        Texture2D loaded = Raylib.LoadTexture(combinedPath);
        return loaded;
    }
    public static Rectangle GenerateSource(Texture2D texture)
    {
        Rectangle source = new(0,0,texture.Width,texture.Height);
        return source;
    }
    public static Rectangle GenerateDest(Texture2D texture, int scale)
    {
        Rectangle dest = new(Vector2.Zero,texture.Width/scale,texture.Height/scale);
        return dest;
    }
    public static Vector2 GenerateOrigin(Texture2D texture, int scale)
    {
        Vector2 origin = new(texture.Width / (scale * 2), texture.Height / (scale * 2));
        return origin;
    }
}
class Program
{
    struct Player
    {
        // Movement
        public Vector2 Position, Origin, Direction, Velocity;
        public float Speed = 250, Angle = 0, Width, Height, Accerlation = 0; 

        // Visuals
        public Texture2D plane;
        public Rectangle Source, Dest;

        public Player(Texture2D texture, int scale)
        {
            // Setting up the plane
            Position = CENTER;

            // visual setup
            plane = texture;
            Width = this.plane.Width;
            Height = this.plane.Height;

            // Source and Dest Rectangles
            Source = GenerateSource(this.plane);
            Dest = GenerateDest(this.plane, scale);
            Origin = GenerateOrigin(this.plane, scale);
        }
    }
    public enum Diffuculty{ Easy, Medium, Hard }
    struct Enemy
    {
        // Movement
        public Vector2 Position, Direction, Velocity, Origin; 
        public float Speed, Width, Height, Angle=0;

        // Visuals
        public Texture2D enemy;
        public Rectangle Source, Dest;
        public Enemy(Texture2D texture, int scale, Vector2 position, Vector2 direction, Diffuculty diffuculty)
        {
            // Setting up direction and position
            Position = position;
            Direction = direction;
            Velocity = Vector2.Zero;

            // Visuals
            // visual setup
            enemy = texture;
            Width = this.enemy.Width;
            Height = this.enemy.Height;

            // Source and Dest Rectangles
            Source = GenerateSource(this.enemy);
            Dest = GenerateDest(this.enemy, scale);
            Origin = GenerateOrigin(this.enemy, scale);

            // Setting the Speed
            switch(diffuculty)
            {
                case Diffuculty.Easy:
                Speed = 150;
                break;

                case Diffuculty.Medium:
                Speed = 250;
                break;

                case Diffuculty.Hard:
                Speed = 350;
                break;
            }
        }

    }
    struct Crosshair(Texture2D texture, int scale)
    {
        public Texture2D CrossTex = texture;
        public Rectangle Source = GenerateSource(texture), Dest = GenerateDest(texture, scale);
        public Vector2 Origin = GenerateOrigin(texture, scale);
        public float Angle = 0;
        public int Factor = 1;
    }
    public static void Main()
    {
        // Initialization
        Raylib.InitWindow(WINW, WINH, "Basic Window");
        Raylib.SetTargetFPS(60);
        float dt;

        // Level Variables
        //Diffuculty levelDiffuculty = Diffuculty.Easy;

        // Player
        Player p = new(LoadTextureOutsideDirectory("assets/Player.png"), 7);
        ParticleSystem playerParticles = new(15);

        // Crosshair
        Crosshair c = new(LoadTextureOutsideDirectory("assets/Crosshair.png"), 7);
        float mouseSensitivity = 0.7f;

        // Screen Bounds and other
        int padding = 25;
        Rectangle Screen = new(padding,padding,WINW-padding,WINH-padding);

        while (!Raylib.WindowShouldClose())
        {
            dt = Raylib.GetFrameTime(); // delta time

            // ------------------------------------
            // PLAYER MOVEMENT
            // ------------------------------------
            if (Raylib.IsKeyDown(KeyboardKey.Left) || Raylib.IsKeyDown(KeyboardKey.A)) p.Angle -= 0.02f;
            else if (Raylib.IsKeyDown(KeyboardKey.Right) || Raylib.IsKeyDown(KeyboardKey.D)) p.Angle += 0.02f;

            // Player accerlation
            if (Raylib.IsKeyDown(KeyboardKey.Up) || Raylib.IsKeyDown(KeyboardKey.W)) {p.Accerlation += 0.25f;}
            else if (Raylib.IsKeyDown(KeyboardKey.Down) || Raylib.IsKeyDown(KeyboardKey.S)) {p.Accerlation -= 0.25f;}

            // Cap accerlation
            p.Accerlation = Math.Clamp(p.Accerlation, 0, 1);

            // Cap Angle
            if (p.Angle > FULL_CIRCLE) p.Angle = 0;
            else if (p.Angle < 0) p.Angle = FULL_CIRCLE;

            // Set Direction
            p.Direction.X = MathF.Cos(p.Angle);
            p.Direction.Y = MathF.Sin(p.Angle);

            // Set position and velocity
            p.Velocity += p.Direction * (p.Speed*p.Accerlation) * dt;
            p.Velocity *= 0.99f; // reduce velocity just a litte
            p.Position += p.Velocity * dt;
            p.Dest.Position = p.Position;

            // Cap bounds
            if (!Raylib.CheckCollisionCircleRec(p.Position, 10, Screen)) p.Velocity *= 0;

            // ------------------------------------
            // Crosshair -> rotates in degrees
            // ------------------------------------
            c.Angle += (Raylib.GetMouseDelta().X+Raylib.GetMouseDelta().Y) * mouseSensitivity;
            if (c.Angle > 360) {c.Angle = 0; c.Factor = 1;}
            else if (c.Angle < 0) {c.Angle = 360; c.Factor = -1;}

            // moves with the mouse
            c.Dest.Position = Raylib.GetMousePosition();

            // crosshair rotates naturally
            c.Angle += c.Factor;

            // ------------------------------------
            // Shooting
            // ------------------------------------

            // drawing
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);

            // Drawing the player
            playerParticles.UpdateParticleSmokeSquare(p.Position, p.Direction*-1, 15, dt, 65); 
            Raylib.DrawTexturePro(p.plane, p.Source, p.Dest, p.Origin, (p.Angle * TO_DEGREE) + 90, Color.White);
            
            // Drawing the Crosshair
            Raylib.DrawTexturePro(c.CrossTex, c.Source, c.Dest, c.Origin, c.Angle, Color.White);

            

            Raylib.EndDrawing();
        }

        // closing and unloading assets
        Raylib.UnloadTexture(p.plane);
        Raylib.UnloadTexture(c.CrossTex);
        Raylib.CloseWindow();
    }
}