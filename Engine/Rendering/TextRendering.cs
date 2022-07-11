using Engine.Objects;
using Engine.Rendering.Sprites;
using Engine.Resources;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Rendering
{
    class TextRendering
    {
       
        public static void Draw1dNumber(Vector2 position, float scale, int value, TestGame game, Vector3 color, int layer = 1, GameObject parent = null)
        {
            GameObject go = GameObject.CreateGameObjectSprite(position, Vector2.One * scale, 0f, SpriteRenderer.quadTextureVerts, value.ToString() + "_white");

            go.color = color;

            if(parent != null)
            {
                parent.AddChild(go);
            }

            game.Instantiate(go, layer);
        }

        public static GameObject DrawNumber(Vector2 startPosition, float letterSize, float letterSpacing, int value, TestGame game, Vector3 color, bool rightleft = false, int layer = 1)
        {
            GameObject text = GameObject.CreateGameObjectSprite(startPosition, Vector2.One, 0f, SpriteRenderer.quadTextureVerts, "");
            if(rightleft)
            {
                switch (value.ToString().Length)
                {
                    case 1:
                        startPosition = new Vector2(startPosition.X + (2 * (5f + letterSpacing)), startPosition.Y);
                        break;
                    case 2:
                        startPosition = new Vector2(startPosition.X + letterSpacing + 5f, startPosition.Y);
                        break;
                    case 3:
                        break;
                }
            }

            for (int x = 0; x < value.ToString().Length; x++)
            {
                Draw1dNumber(new Vector2(startPosition.X + (letterSpacing + 5f) * x, startPosition.Y), letterSize, int.Parse(value.ToString()[x].ToString()), game, color, layer, text);
            }

            return text;
        }
    }
}
