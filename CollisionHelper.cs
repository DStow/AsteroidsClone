using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Asteroids.Asteroids;

namespace Asteroids
{
    public static class CollisionHelper
    {
        public static bool CheckBasicCollision(Vector2 object1Origin, Vector2 object1Size, Vector2 object2Origin, Vector2 object2Size)
        {
            var xCompare = object1Origin.X - object2Origin.X;
            var yCompare = object1Origin.Y - object2Origin.Y;
            var distance = System.Math.Sqrt(xCompare * xCompare + yCompare * yCompare);

            if (distance < (object1Size.X / 2) + (object2Size.X / 2))
                return true;
            else
                return false;
        }
    }
}