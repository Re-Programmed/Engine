using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Objects.Components
{
    class BoxCollider : Component
    {
        protected static List<BoxCollider> boxColliders = new List<BoxCollider>();

        GameObject parent;

        public Vector2 position { get; protected set; }
        public Vector2 size { get; protected set; }

        public bool Static = false;

        public bool Right = false;
        public bool Left = false;
        public bool Bottom = false;
        public bool Top = false;

        public Component.ComponentType GetComponentType()
        {
            return Component.ComponentType.BoxCollider;
        }

        public void SetPosition(Vector2 pos)
        {
            this.position = pos;
        }

        public void SetSize(Vector2 size)
        {
            this.size = size;
        }

        public void Init(GameObject parent)
        {
            this.parent = parent;

            position = parent.position;
            size = parent.scale;

            boxColliders.Add(this);
        }

        public void Update(TestGame game)
        {

        }
        public void OnReEnable(TestGame game)
        {

        }

        public void OnDisable(TestGame game)
        {

        }
        public void CheckCollision()
        {
            Left = false;
            Right = false;
            Top = false;
            Bottom = false;

            if (Static) { return; }

            bool CollidingXP;
            bool CollidingXN;
            bool CollidingYP;
            bool CollidingYN;

            foreach(BoxCollider col in boxColliders)
            {
                if(col != this)
                {

                    CollidingXP = GetDistance(col.position.X, position.X) < (col.size.X/2 + size.X/2) / 3f && col.position.X > position.X;
                   
                    CollidingXN = GetDistance(col.position.X, position.X) < (col.size.X / 2 + size.X / 2) / 3f && col.position.X < position.X;
                   
                    CollidingYP = GetDistance(col.position.Y, position.Y) < (col.size.Y/2 + size.Y/2) / 3f && col.position.Y < position.Y;
                   
                    CollidingYN = GetDistance(col.position.Y, position.Y) < (col.size.Y / 2 + size.Y / 2) / 3f && col.position.Y > position.Y;


                    if (CollidingXN || CollidingXP)
                    {
                        if (CollidingYP && !Top)
                        {
                            Top = true;
                            Right = true;
                            Left = true;
                        }

                        if (CollidingYN && !Bottom)
                        {
                            Bottom = true;
                            Right = true;
                            Left = true;
                        }
                    }

                    if (CollidingYN || CollidingYP)
                    {
                        if (CollidingXP && !Right)
                        {
                            Right = true;
                            Top = true;
                            Bottom = true;
                        }

                        if (CollidingXN && !Left)
                        {
                            Left = true;
                            Top = true;
                            Bottom = true;
                        }
                    }
                }
            }

            if(Top && Bottom && Left && Right)
            {
                Left = false;
                Right = false;
                Top = false;
                Bottom = false;
            }
        }

        protected float GetDistance(float x1, float x2)
        {
            return (float)Math.Abs(x1 - x2);
        }

        public T GetClone<T>()
        {
            return (T)MemberwiseClone();
        }

        protected ComponentData myData;

        public Component SetComponentData(ComponentData data)
        {
            myData = data;
            data.type = GetComponentType();
            return this;
        }

        public ComponentData GetComponentData()
        {
            if (myData == null) { return new ComponentData(); }
            return myData;
        }
    }
}
