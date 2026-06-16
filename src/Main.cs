using System;
using System.Numerics;
using Raylib_cs;
using static Global;

class Global // define global variables here
{
    // WINDOW VARIABLES
    public const int WINW = 1024;
    public const int WINH = 512;
}

class Program
{
    public static void Main()
    {
        // Initialization
        Raylib.InitWindow(WINW, WINH, "Basic Window");
        Raylib.SetTargetFPS(60);
        float dt;

        while (!Raylib.WindowShouldClose())
        {
            dt = Raylib.GetFrameTime(); // delta time

            // PLAYER MOVEMENT

            // drawing
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);
            Raylib.EndDrawing();
        }

        // closing and unloading assets
        Raylib.CloseWindow();
    }
}