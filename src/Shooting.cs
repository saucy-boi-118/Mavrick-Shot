using Raylib_cs;
using System.Numerics;

namespace Shooting
{
    class Gun(int MaxBullets)
    {
        // Possible Upgrades
        public bool toggleMultiShootUpgrade=false, toggleSpeedUpgrade=false, toggleRangeUpgrade=false;
        private float MaxRange = 1000; // 100 away
        // Individual bullet
        struct Bullet(Vector2 pos, Vector2 dir) // its AoS, sue me
        {
            Vector2 Position = pos, Direction = dir;
            float dist = 0;
            byte fade = 255;

        }
        
        // Bullet list
        Bullet[] bs = new Bullet[MaxBullets]; // set number of bullets
    }   
}