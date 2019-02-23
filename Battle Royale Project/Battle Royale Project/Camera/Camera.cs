using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Battle_Royale_Project
{
    public class Camera
    {
        public Matrix Transform { get; set; }
        public Vector2 Position { get; set; }
        private Vector2 Center;
        private Viewport Viewport;

        public Camera(Viewport viewport)
        {
            Viewport = viewport;
        }

        public void Focus(Vector2 position, int xOffset, int yOffset)
        {
            if (position.X < Viewport.Width / 2)
                Center.X = Viewport.Width / 2;
            else if (position.X > xOffset - (Viewport.Width / 2))
                Center.X = xOffset - (Viewport.Width / 2);
            else
                Center.X = position.X;

            if (position.Y < Viewport.Height / 2)
                Center.Y = Viewport.Height / 2;
            else if (position.Y > yOffset - (Viewport.Height / 2))
                Center.Y = yOffset - (Viewport.Height / 2);
            else
                Center.Y = position.Y;

            Transform = Matrix.CreateTranslation(new Vector3(-Center.X + (Viewport.Width / 2), -Center.Y + (Viewport.Height / 2), 0));
        }
    }
}
