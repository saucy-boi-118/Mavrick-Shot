using Raylib_cs;
using Main;
using System.Numerics;

namespace EnemyShooting
{
    class Gun(int MaxBullets)
    {
        // Possible Upgrades
        // MAKE AN ENUM FOR UPGRADES
        private readonly float MaxRange = 250*250; // 250 away
        private Color BulletColor = Color.Green;
        private Vector2 Outside = new(-10000000,-10000000);
        private int BulletIndex = 0;
        // Individual bullet
        protected struct Bullet() // its AoS, sue me...
        {
            public Vector2 Position, Direction, Origin;
            public float DistanceOrigin = 0, Speed = 0, Angle = 0;
            public byte Fade = 255;
        }
        
        // Bullet list
        protected Bullet[] bs = new Bullet[MaxBullets]; // set number of bullets

        public void OverwriteBullet(Vector2 origin)
        {
            // CHANGES BASED ON THE UPGRADES 

            if (BulletIndex < MaxBullets)
            {
                // Overwrite a bullet 
                bs[BulletIndex].Position = origin; // new position
                bs[BulletIndex].Origin = origin; // set its own origin
                bs[BulletIndex].Direction = Vector2.Normalize(Raylib.GetMousePosition() - origin); // new direction
                bs[BulletIndex].Speed = 450; // new speed
                bs[BulletIndex].Angle = 0; // reset angle
                bs[BulletIndex].Fade = 255; // reset fade

                // increase counter
                BulletIndex++;  
            }
            else {BulletIndex = 0;} 
            
        }
        private int temp;
        public void UpdateBullets(float dt)
        {
            for (int i = 0; i < MaxBullets; i++)
            {
                bs[i].Position += bs[i].Direction * bs[i].Speed * dt; // move the bullet
                bs[i].DistanceOrigin = Vector2.DistanceSquared(bs[i].Origin, bs[i].Position); // distance
                bs[i].Fade -= 5; // fade the bullet as it moves
                bs[i].Angle += 5; // increase angle as it moves
                BulletColor.A = bs[i].Fade; // set the fade of the bullet
                if (bs[i].DistanceOrigin > MaxRange) 
                { // magic overwriting code
                    temp = BulletIndex; // store original bullet index
                    BulletIndex = i; // set bullet index to current index
                    OverwriteBullet(Outside); // overwrite the bullet at current index
                    BulletIndex = temp; // reset bullet index to original
                } // range checking

                // COLLISION DETECTION WITH QUADTREES
                //----
                //----
                //----
                //----
                //----
                //----
                //----//----//----//----//----//----//----

                Raylib.DrawPoly(bs[i].Position, 4, 10, bs[i].Angle, BulletColor);
            }
        }


    }

    class Enemies(int MaxEnemies)
    {
        private static readonly Random r = new(0118);
        private readonly int MaxAttack = 150*150;
        private int EnemyIndex = 0;
        protected struct Enemy
        {
            public Vector2 Position, Direction;
            public float DistanceOrigin, Speed, Accel, Angle;
        }

        public void DefineEnemies()
        {
            for (int i = 0; i < MaxEnemies; i++)
            {
                es[i].Position = RandomVector();
                es[i].Speed = (100*r.NextSingle());
            }
        }
        protected Enemy[] es = new Enemy[MaxEnemies]; // set number of enemies
        public void OverwriteEnemy()
        {
            // CHANGE BASED ON UPGRADES

            if (EnemyIndex < MaxEnemies)
            {
                // Overwrite a bullet 
                es[EnemyIndex].Position = RandomVector(); // new position
                es[EnemyIndex].Speed = (450*r.NextSingle()) + 250; // new speed
                es[EnemyIndex].Angle = 0; // reset angle

                // increase counter
                EnemyIndex++;  
            }
            else {EnemyIndex = 0;} 
            
        }

        public void UpdateEnemies(Texture2D EnemyTexture, Rectangle Source, Rectangle Dest, Vector2 Origin, Vector2 OtherOrigin, float dt)
        {
            for (int i = 0; i < MaxEnemies; i++)
            {
                // Updating Direction and Angle
                es[i].Direction = Vector2.Normalize(OtherOrigin - es[i].Position);
                es[i].Angle = MathF.Atan2(es[i].Direction.Y, es[i].Direction.X); // using the atan 2 function

                // Updating Position
                es[i].Position += es[i].Direction * (es[i].Speed + es[i].Accel) * dt;

                // Capping Angle
                if (es[i].Angle < 0) {es[i].Angle = Main.Global.FULL_CIRCLE;}
                else if (es[i].Angle > Main.Global.FULL_CIRCLE) {es[i].Angle = 0;}

                // Updating distance
                es[i].DistanceOrigin = Vector2.DistanceSquared(es[i].Position, OtherOrigin);

                // Distance checking
                if (es[i].DistanceOrigin > MaxAttack)
                {
                    es[i].Speed = 0;
                    es[i].Accel += 0.1f;
                }
                else {es[i].Speed = (450*r.NextSingle()) + 250;}

                // Decrease accel
                es[i].Accel -= 0.5f;

                // clamp accelearation
                es[i].Accel = Math.Clamp(es[i].Accel, 0, 45);
                
                
                // Drawing Enemy
                Raylib.DrawPoly(es[i].Position, 6, 20, es[i].Angle, Color.Red);
                Raylib.DrawPolyLinesEx(es[i].Position, 6, 35, es[i].Angle, 5, Color.Black);
                //Raylib.DrawTexturePro(EnemyTexture, Source, Dest, Origin, es[i].Angle, Color.White);
            }
        }

        public static Vector2 RandomVector()
        {
            return new(Raylib.GetRandomValue(-50,Main.Global.WINW+50),Raylib.GetRandomValue(-50,Main.Global.WINH+50));
        }
    }   
}