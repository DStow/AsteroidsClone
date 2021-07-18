using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Asteroids.Asteroids
{
    public class MediumAsteroid : AsteroidBase
    {
        protected override Vector2 AsteroidSize()
        {
            return new Vector2(30, 35);
        }

        protected override Texture2D GetAsteroidSprite()
        {
            return AsteroidSprites.MediumAsteroidSprite;
        }
    }
}