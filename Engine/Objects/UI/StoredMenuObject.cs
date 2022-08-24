using Engine.Objects.Components;
using Engine.Objects.Components.UIComponents;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using static Engine.Objects.Components.Component;

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

        public List<ComponentData> buttons { get; set; } = new List<ComponentData>();

        public StoredMenuObject()
        {
            
        }

        public StoredMenuObject(Vector2 pos, Vector2 scale, float rotation, string texture, int layer, List<Component> components)
        {
            xPos = pos.X;
            yPos = pos.Y;

            xScale = scale.X;
            yScale = scale.Y;

            this.rotation = rotation;

            this.texture = texture;

            Layer = layer;

            foreach (Component c in components)
            {
                if (c is Button)
                {
                    Button b = c as Button;
                    buttons.Add(b.data);
                }
            }
        }

        public GameObject LoadObject(TestGame game, GameObject parent = null, bool camRelative = true)
        {
            GameObject myObject = GameObject.CreateGameObjectSprite(new Vector2(xPos, yPos) + (camRelative ? game.cam.FocusPosition : Vector2.Zero), new Vector2(xScale, yScale) * (camRelative ? game.cam.Zoom : 1f), rotation, game.sr.verts, texture);

            myObject.SetAlwaysLoad(true);

            game.Instantiate(myObject, Layer);

            foreach (ComponentData b in buttons)
            {
                myObject.AddComponent(b.GenerateButtonFromData());
            }

            parent?.AddChild(myObject);

            return myObject;
        }
    }
}
