using Engine.Objects;
using Engine.Objects.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.GameFiles.Interactables
{
    abstract class Interactable : Component
    {
        const GLFW.Keys InteractKey = GLFW.Keys.E;

        protected GameObject gameObject;

        public abstract T GetClone<T>();

        public abstract Component.ComponentType GetComponentType();

        public void Init(GameObject parent)
        {
            gameObject = parent;
        }

        bool enabled = true;
        bool EDown = false;

        public void OnDisable(TestGame game)
        {
            enabled = false;
        }

        public void OnReEnable(TestGame game)
        {
            enabled = true;
        }

        public void Update(TestGame game)
        {
            if(enabled)
            {
                if(Input.Input.GetKey(InteractKey))
                {
                    if (!EDown)
                    {
                        EDown = true;

                        if(System.Numerics.Vector2.Distance(gameObject.position, TestGame.GetDefaultTriggerObject().position) < myData.radius)
                        {
                            Interact();
                        }
                    }
                }
                else
                {
                    EDown = false;
                }
            }
        }

        public abstract void Interact();

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
