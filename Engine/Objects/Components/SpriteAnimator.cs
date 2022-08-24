using Engine.Game;
using System;
using System.Collections.Generic;
using System.Text;
using static Engine.Objects.Components.Component;

namespace Engine.Objects.Components
{
    /// <summary>
    /// Allows animations of textures.
    /// </summary>
    class SpriteAnimator : Component
    {
        /// <summary>
        /// How much in game time has passed since the last frame.
        /// </summary>
        protected float Frames = 0;
        /// <summary>
        /// How much in game time should pass before the next animation frame.
        /// </summary>
        protected float ReqFrames = 0.5f;

        protected int currentFrame = 0;

        public bool Sync = false;

        public List<string> frameData = new List<string>();

        bool active = true;

        public void SetReqFrames(float ReqTime)
        {
            ReqFrames = ReqTime;
        }

        public void SetActive(bool active)
        {
            this.active = active;
        }

        public GameObject parent { get; private set; } 

        /// <summary>
        /// Get the ComponentType enum value of this component.
        /// </summary>
        /// <returns>ComponentType</returns>
        public ComponentType GetComponentType()
        {
            return ComponentType.SpriteAnimator;
        }

        /// <summary>
        /// On program start.
        /// </summary>
        public void Init(GameObject parent)
        {
            this.parent = parent;
        }

        public void Reset(bool startInstant)
        {
            Frames = 0;
            currentFrame = 0;

            if (startInstant) { Frames = ReqFrames; }
        }

        /// <summary>
        /// Every frame.
        /// </summary>
        public void Update(TestGame game)
        {
            if (!active) { return; }

            if(Sync)
            {
                syncProcess();
                return;
            }

            Frames += GameTime.DeltaTime;

            if(Frames >= ReqFrames)
            {
                currentFrame++;
                if (currentFrame == frameData.Count) { currentFrame = 0; }
                parent.texture.UpdateTexture(frameData[currentFrame]);
                Frames = 0;
            }
        }

        void syncProcess()
        {
            parent.texture.UpdateTexture(frameData[Game.Game.AnimationFrames]);
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
