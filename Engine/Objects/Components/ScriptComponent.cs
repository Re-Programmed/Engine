using Engine;
using Engine.Objects;
using Engine.Objects.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Objects.Components
{
    class ScriptComponent : Component
    {
        protected GameObject gameObject;

        public Component.ComponentType GetComponentType()
        {
            return Component.ComponentType.Script;
        }

        public void Init(GameObject parent)
        {
            gameObject = parent;
            Start();
        }

        public virtual void Start()
        {

        }

        public void Update(TestGame game)
        {
            ScriptUpdate(game);
        }

        public virtual void ScriptUpdate(TestGame game)
        {

        }

        public void OnReEnable(TestGame game)
        {

        }

        public void OnDisable(TestGame game)
        {

        }

        public T GetClone<T>()
        {
            return (T)MemberwiseClone();
        }

        public Component SetComponentData(ComponentData data)
        {
            return this;
        }

        public ComponentData GetComponentData()
        {
            return null;
        }
    }
}
