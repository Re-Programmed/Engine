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
        public static void LoadTextures()
        {
            ResourceManager.LoadTexture("../../../textures/text/numbers/0.png", true, "0");
            ResourceManager.LoadTexture("../../../textures/text/numbers/1.png", true, "1");
            ResourceManager.LoadTexture("../../../textures/text/numbers/2.png", true, "2");
            ResourceManager.LoadTexture("../../../textures/text/numbers/3.png", true, "3");
            ResourceManager.LoadTexture("../../../textures/text/numbers/4.png", true, "4");
            ResourceManager.LoadTexture("../../../textures/text/numbers/5.png", true, "5");
            ResourceManager.LoadTexture("../../../textures/text/numbers/6.png", true, "6");
            ResourceManager.LoadTexture("../../../textures/text/numbers/7.png", true, "7");
            ResourceManager.LoadTexture("../../../textures/text/numbers/8.png", true, "8");
            ResourceManager.LoadTexture("../../../textures/text/numbers/9.png", true, "9");

            ResourceManager.LoadTexture("../../../textures/text/numbers/0_white.png", true, "0_white");
            ResourceManager.LoadTexture("../../../textures/text/numbers/1_white.png", true, "1_white");
            ResourceManager.LoadTexture("../../../textures/text/numbers/2_white.png", true, "2_white");
            ResourceManager.LoadTexture("../../../textures/text/numbers/3_white.png", true, "3_white");
            ResourceManager.LoadTexture("../../../textures/text/numbers/4_white.png", true, "4_white");
            ResourceManager.LoadTexture("../../../textures/text/numbers/5_white.png", true, "5_white");
            ResourceManager.LoadTexture("../../../textures/text/numbers/6_white.png", true, "6_white");
            ResourceManager.LoadTexture("../../../textures/text/numbers/7_white.png", true, "7_white");
            ResourceManager.LoadTexture("../../../textures/text/numbers/8_white.png", true, "8_white");
            ResourceManager.LoadTexture("../../../textures/text/numbers/9_white.png", true, "9_white");

        }

        public static void Draw1dNumber(Vector2 position, float scale, int value, Game.Game game, bool white = false, int layer = 1)
        {
            GameObject go = GameObject.CreateGameObjectSprite(position, Vector2.One * scale, 0f, SpriteRenderer.quadTextureVerts, value.ToString());
            if(white)
            {
                go.texture.UpdateTexture(value.ToString() + "_white");
            }
            game.UI[layer].objects.Add(go);
        }

        public static void DrawNumber(Vector2 startPosition, float letterSize, int value, Game.Game game, bool white = false, bool rightleft = false, int layer = 1)
        {
            if(rightleft)
            {
                switch (value.ToString().Length)
                {
                    case 1:
                        startPosition = new Vector2(startPosition.X + (2 * (5f + letterSize)), startPosition.Y);
                        break;
                    case 2:
                        startPosition = new Vector2(startPosition.X + letterSize + 5f, startPosition.Y);
                        break;
                    case 3:
                        break;
                }
            }

                for (int x = 0; x < value.ToString().Length; x++)
                {
                    Draw1dNumber(new Vector2(startPosition.X + (letterSize + 5f) * (x + 1), startPosition.Y), letterSize, int.Parse(value.ToString()[x].ToString()), game, white, layer);
                }
            
        }
    }
}
