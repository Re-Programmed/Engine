using Engine.Rendering.Sprites;
using Engine.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Objects
{
    class ObjectTexture
    {
        public string textureName { get; private set; }

        public bool flipped = false;

        public float textureRotation = 0f;
        public ObjectTexture(string tName)
        {
            textureName = tName;
        }

        public Texture2D getTexture()
        {
            return ResourceManager.GetTexture(textureName);
        }

        public void UpdateTexture(string newtextureName)
        {
            textureName = newtextureName;
        }
    }
}
