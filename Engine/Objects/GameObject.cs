using Engine.Rendering.Shaders;
using System;
using static Engine.OpenGL.GL;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using Engine.Objects.Components;

namespace Engine.Objects
{
    class GameObject
    {
        public GameObject parent = null;
        public int Layer {get; private set;}
        public Vector2 position { get; private set; }

        public Vector2 totalposition { get; private set; }

        public Vector2 scale { get; private set; }
        public float rotation { get; private set; }

        public Vector3 color = Vector3.One;

        public bool editor = false;
        public bool ignoreEditing = false;

        public bool AlwaysLoad { get; private set; }

        public bool ignoreStageSaving = false;

        public float[] vertices;

        public List<GameObject> children = new List<GameObject>();

        protected List<Component> Components = new List<Component>();

        public bool Loaded = false;

        public ObjectTexture texture { get; private set; }

        public GameObject GetMemberwiseClone()
        {
            return (GameObject)this.MemberwiseClone();
        }

        public GameObject(Vector2 position, Vector2 scale, float rotation, float[] vertices)
        {
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
            this.vertices = vertices;

            Init();
        }

        public GameObject(Vector2 position, Vector2 scale, float rotation, float[] vertices, string textureName)
        {
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
            this.vertices = vertices;
            this.texture = new ObjectTexture(textureName);

            Init();
        }

        public GameObject()
        {

        }

        public Component[] GetComponents()
        {
            return Components.ToArray();
        }

        public void Initilize(Vector2 position, Vector2 scale, float rotation, float[] vertices, string textureName, float textureRotation = 0f)
        {
            this.position = position;
            this.scale = scale;
            this.rotation = rotation;
            this.vertices = vertices;
            this.texture = new ObjectTexture(textureName);

            this.texture.textureRotation = textureRotation;

            Init();
        }

        public void DestroyWithChildren(TestGame game)
        {
            if(children.Count != 0)
            {
                foreach (GameObject c in children)
                {
                    if (c != null)
                    {
                        c.DestroyWithChildren(game);
                    }
                }
            }

            game.Destroy(this);
        }

        public void SetAlwaysLoad(bool value)
        {
            AlwaysLoad = value;
        }

        public void OnReEnable(TestGame game)
        {
            foreach (Component c in Components)
            {
                c.OnReEnable(game);
            }
        }

        public void OnDisable(TestGame game)
        {
            foreach(Component c in Components)
            {
                c.OnDisable(game);
            }
        }

        List<Component> compremoves = new List<Component>();
        List<Component> compadd = new List<Component>();

        public void RemoveComponent(Component c)
        {
            if(Components.Contains(c))
            {
                compremoves.Add(c);  
            }
        }

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public void SetLayer(int layer)
        {
            this.Layer = layer;
            Physics.PhysicsAffected c = GetComponent<Physics.PhysicsAffected>(Component.ComponentType.PhysicsRel);
            if(c != null)
            {
                c.UpdateLayer(layer);
            }
        }

        /// <summary>
        /// Moves the object.
        /// </summary>
        /// <param name="change">What to add to the position.</param>
        public void Translate(Vector2 change)
        {
            SetPosition(position + change);
        }

        /// <summary>
        /// Moves the object.
        /// </summary>
        /// <param name="x">X amount to move.</param>
        /// <param name="y">Y amount to move.</param>
        public void Translate(float x, float y)
        {
            SetPosition(position + new Vector2(x, y));
        }

        public void SetScale(Vector2 scale)
        {
            this.scale = scale;
        }

        public void SetRotation(float rotation)
        {
            this.rotation = rotation;
        }

        public static unsafe GameObject CreateGameObject(float[] vertices, uint vao, uint vbo, Vector2 position, float rotation, Vector2 scale)
        {
            glBindVertexArray(vao);

            glBindBuffer(GL_ARRAY_BUFFER, vbo);

            fixed (float* v = &vertices[0])
            {
                glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, v, GL_STATIC_DRAW);
            }

            glVertexAttribPointer(0, 2, GL_FLOAT, false, 5 * sizeof(float), (void*)0);
            glEnableVertexAttribArray(0);

            glVertexAttribPointer(1, 3, GL_FLOAT, false, 5 * sizeof(float), (void*)(2 * sizeof(float)));
            glEnableVertexAttribArray(1);

            glBindBuffer(GL_ARRAY_BUFFER, 0);
            glBindVertexArray(0);
            
            return new GameObject(position, scale, rotation, vertices);
        }

        public static unsafe GameObject CreateGameObjectSprite(Vector2 position, Vector2 scale, float rotation, float[] vertices, string textureName)
        {
            return new GameObject(position, scale, rotation, vertices, textureName);
        }

        public virtual void Update()
        {
            
        }


        public virtual void Update(TestGame game)
        {

        }

        public virtual void Init()
        {

        }

        public void UpdateComponents(TestGame game)
        {
            foreach(Component c in compremoves)
            {
                c.OnDisable(game);
                Components.Remove(c);
            }

            foreach(Component add in compadd)
            {
                Components.Add(add);
                add.Init(this);
            }

            compadd.Clear();
            compremoves.Clear();

            if(parent != null)
            {
                totalposition = position + parent.totalposition;
            }
            else
            {
                totalposition = position;
            }

            foreach(Component c in Components)
            {
                c.Update(game);
            }
        }

        /// <summary>
        /// Gets the specified component stored on this object. If there is none, returns null.
        /// </summary>
        /// <param name="type">What component to check for.</param>
        /// <returns>Component</returns>
        public T GetComponent<T>(Component.ComponentType type)
        {
            foreach(Component c in Components)
            {
                if(c.GetComponentType() == type)
                {
                    return (T)c;
                }
            }

            return default(T);
        }

        /// <summary>
        /// Adds a component to the object.
        /// </summary>
        /// <param name="add">What component to add.</param>
        /// <returns>If the component was added successfully.</returns>
        public bool AddComponent(Component add)
        {
            if(add.GetComponentType() != Component.ComponentType.Script)
            {
                if (GetComponent<Component>(add.GetComponentType()) != null) { return false; }
            }

            compadd.Add(add);

            return true;
        }

        /// <summary>
        /// Make this the parent of a GameObject.
        /// </summary>
        /// <param name="child">What to make the child.</param>
        public void AddChild(GameObject child)
        {
            children.Add(child);
            child.parent = this;
        }

        /// <summary>
        /// Make the specified child independent of this object.
        /// </summary>
        public void RemoveChild(GameObject child)
        {
            children.Remove(child);
            child.parent = null;
        }

        public virtual void Destroy(TestGame game)
        {
            OnDisable(game);
        }
    }
}
