using Raylib_cs;
using System.Numerics;

namespace Particles
{
    class ParticleSystem(int MaxParticles)
{
    public struct Particles()
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public float Size;
        public Color Color;
        public float Age; 
        public bool Alive;          
    }

    public Particles[] particles = new Particles[MaxParticles];
    public static Vector2 grav = new(0, -1500f);
    private static Vector2 squareSize;
    private readonly Random random = new();

    private void OverWriteParticle(Vector2 origin, int i, float size)
    {
        particles[i].Position = origin; // set it to the origin

        // set the velocity | speed in a direction
        particles[i].Velocity = Vector2.Normalize(new((random.NextSingle()+0.1f) * 2 - 1, random.NextSingle() * 2 - 1)) * ((random.NextSingle()+0.1f) * 100);

        particles[i].Size = size; // size to seven

        particles[i].Age = 0; // reset age

        particles[i].Alive = true; // set alive to true
    }

    public void UpdateParticleSMOKE(Vector2 origin, Vector2 dir, float size, float dt, int Lifespan)
    {
            for (int i = 0; i < particles.Length - 1; i++)
            {
                if (particles[i].Alive == true)
                {
                    // Moving
                    particles[i].Velocity += dir * dt; // add acceleration 
                    particles[i].Position += particles[i].Velocity * dt; // move with velocity

                    particles[i].Size += 0.3f; // increase size

                    // make it fade
                    particles[i].Color.A -= 4; if (particles[i].Color.A < 4) {particles[i].Alive = false;}

                    particles[i].Age += dt; // increase age

                    // checking its lifespan
                    if (particles[i].Age > Lifespan) {particles[i].Alive = false;}   
                }
                else if (particles[i].Alive == false && random.NextSingle() < 0.1)
                {
                    particles[i].Color = Color.Gray; // color to grey
                    OverWriteParticle(origin, i, size); // overwrite the particle
                }

                // Drawing
                Raylib.DrawCircleV(particles[i].Position, particles[i].Size, particles[i].Color);
            }
    }

    public void UpdateParticleSmokeSquare(Vector2 origin, Vector2 direction, float size, float dt, float lifespan)
    {
        for (int i = 0; i < particles.Length - 1; i++)
            {
                if (particles[i].Alive == true)
                {
                    // Setting the size
                    squareSize.X = size;
                    squareSize.Y = size;

                    // Moving
                    particles[i].Velocity += direction * dt; // add acceleration 
                    particles[i].Position += particles[i].Velocity * dt; // move with velocity

                    particles[i].Size += 0.3f; // increase size

                    // make it fade
                    particles[i].Color.A -= 4; if (particles[i].Color.A < 4) {particles[i].Alive = false;}

                    particles[i].Age += dt; // increase age

                    // checking its lifespan
                    if (particles[i].Age > lifespan) {particles[i].Alive = false;}   
                }
                else if (particles[i].Alive == false && random.NextSingle() < 0.1)
                {
                    particles[i].Color = Color.Gray; // color to grey
                    OverWriteParticle(origin, i, size); // overwrite the particle
                }

                // Drawing
                Raylib.DrawRectangleV(particles[i].Position, squareSize, particles[i].Color);
            }  
    }

    public static void UpdateParticleEXPLODE(ref Particles[] p, Vector2 origin, Random r, float dt, Color color, int WINHEIGHT)
    {
        for (int i = 0; i < p.Length - 1; i++)
            {
                if (p[i].Alive == true)
                {
                    // Moving
                    p[i].Velocity -= grav * dt; // add acceleration 
                    p[i].Position += p[i].Velocity * dt; // move with velocity

                    p[i].Color.A -= 2; if (p[i].Color.A <= 4) {p[i].Alive = false;}

                    if (p[i].Position.Y >= WINHEIGHT+50) {p[i].Alive = false;}  
                }
                else if (p[i].Alive == false && r.NextSingle() < 0.1)
                {
                    p[i].Color = color; // color to grey
                    //OverWriteParticle(origin, i, 7);
                    p[i].Velocity.Y -= 550;
                }

                // Drawing
                Raylib.DrawCircleV(p[i].Position, p[i].Size, p[i].Color);
            }
    }
}   
}
