using Engine.misc;
using Engine.Objects;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.GameFiles.ObjectScripts
{
    class ParallaxGroup : UpdateDependent
    {
        readonly Dictionary<GameObject, Vector2> myObjects = new Dictionary<GameObject, Vector2>();

        readonly float offset = 0.5f;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="myObjects">Objects to parallax.</param>
        /// <param name="offset">Multiple to apply to positions of objects.</param>
        public ParallaxGroup(GameObject[] myObjects, float offset = 0.5f)
            : base()
        {
            this.offset = offset;
            foreach(GameObject go in myObjects)
            {
                this.myObjects.Add(go, go.position);
            }
        }

        public override void Update()
        {
            foreach(KeyValuePair<GameObject, Vector2> go in myObjects)
            {
                go.Key.SetPosition(TestGame.INSTANCE.cam.FocusPosition * offset + go.Value);
            }
        }
    }
}
