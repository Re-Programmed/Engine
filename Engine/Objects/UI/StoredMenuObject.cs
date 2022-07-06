using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Objects.UI
{
    class StoredMenuObject
    {
        public float xPos { get; set; }
        public float yPos { get; set; }
        public float xScale { get; set; }
        public float yScale { get; set; }

        public float rotation { get; set; }

        public string texture { get; set; }

        public int Layer { get; set; } = 4;

        public StoredMenuObject()
        {

        }

        public StoredMenuObject(Vector2 pos, Vector2 scale, float rotation, string texture, int layer)
        {
            xPos = pos.X;
            yPos = pos.Y;

            xScale = scale.X;
            yScale = scale.Y;

            this.rotation = rotation;

            this.texture = texture;

            Layer = layer;
        }

        public GameObject LoadObject(TestGame game)
        {
            GameObject myObject = GameObject.CreateGameObjectSprite(new Vector2(xPos, yPos), new Vector2(xScale, yScale), rotation, game.sr.verts, texture);

            game.Instantiate(myObject, Layer);

            return myObject;
        }
    }
}
