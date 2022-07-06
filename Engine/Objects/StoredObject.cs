using Engine.Objects.Components;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

#pragma warning disable

namespace Engine.Objects
{
    /// <summary>
    /// Acts as a game object that does not yet exist.
    /// </summary>
    class StoredObject
    {
        private List<Component> components = new List<Component>();
        public int Layer { get; private set; }

        public ObjectTexture texture {get; private set;}

        public Vector3 color = Vector3.One;

        public bool editor = false;
        public bool ignoreEditing = false;

        public float[] vertices;

        public StoredObject(string texture, float[] vertices, int layer)
        {
            Layer = layer;
            this.vertices = vertices;
            this.texture = new ObjectTexture(texture);
        }

        public void AddComponent(Component c)
        {
            components.Add(c);
        }

        public void SetLayer(int layer)
        {
            Layer = layer;
        }

        public void ClearComponents()
        {
            components.Clear();
        }

        public GameObject SendToGameObject(Vector2 position, Vector2 scale, float rotation)
        {
            return GameObject.CreateGameObjectSprite(position, scale, rotation, vertices, texture.textureName);
        }
    }
}
