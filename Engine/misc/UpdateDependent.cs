using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.misc
{
    /// <summary>
    /// Has a function, Update, that is called every frame.
    /// </summary>
    abstract class UpdateDependent
    {
        /// <summary>
        /// Set to true to remove this update method.
        /// </summary>
        public bool Disabled { get; protected set; }

        public UpdateDependent()
        {
            TestGame.RegisterUpdateDependent(this); 
        }

        /// <summary>
        /// Called each frame.
        /// </summary>
        public abstract void Update();
    }
}
