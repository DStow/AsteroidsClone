using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Asteroids.Asteroids
{
    public class SmallAsteroid : AsteroidBase
    {
        protected override Vector2 AsteroidSize()
        {
            return new Vector2(14, 15);
        }

        protected override Texture2D GetAsteroidSprite()
        {
            return AsteroidSprites.SmallAsteroidSprite;
        }
    }
}