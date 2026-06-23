using Raylib_cs;
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
}