using Engine.Objects.Components;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Objects.Components
{
    abstract class Trigger : Component
    {
        bool triggerCheck = true;
        bool triggered = false;
        protected GameObject gameObject;
        protected GameObject track;
        public ComponentData data;

        public T GetClone<T>()
        {
            return (T)MemberwiseClone();
        }

        public abstract Component.ComponentType GetComponentType();

        public void SetRadius(float radius)
        {
            data.radius = radius;
        }

        public void Init(GameObject parent)
        {
            gameObject = parent;
        }

        public void OnDisable(TestGame game)
        {
            triggerCheck = false;
        }

        public void OnReEnable(TestGame game)
        {
            triggerCheck = true;
        }

        public void Update(TestGame game)
        {
            if(triggerCheck)
            {
                if (track != null)
                {
                    if (Vector2.Distance(track.position, gameObject.position) <= data.radius)
                    {
                        if (!triggered)
                        {
                            OnTrigger();
                            triggered = true;
                        }
                    }
                    else
                    {
                        triggered = false;
                    }
                }
                else
                {
                    Console.WriteLine("[TRIGGER] Tracking Nothing");
                }
            }
        }

        /// <summary>
        /// Trigger Constructor.
        /// </summary>
        /// <param name="track">What object to check for intersection with.</param>
        public Trigger(GameObject track)
        {
            this.data = new ComponentData();
            this.track = track; 
        }

        public Trigger()
        {
            this.data = new ComponentData();
            track = TestGame.GetDefaultTriggerObject();
        }

        public Trigger(ComponentData data)
        {
            track = TestGame.GetDefaultTriggerObject();
            this.data = data;
        }

        /// <summary>
        /// What happens when the target intersects with this trigger.
        /// </summary>
        public abstract void OnTrigger();
    }
}
