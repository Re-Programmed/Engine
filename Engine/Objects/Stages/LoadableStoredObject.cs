using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Objects.Stages
{
    class LoadableStoredObject
    {
        //ID
        public int i { get; set; }

        //Position
        public float px { get; set; }
        public float py { get; set; }

        //Scale
        public float sx { get; set; }
        public float sy { get; set; }
        //Rotation
        public float r { get; set; }

        //Layer
        public int l { get; set; }

        public StageObject GetObject()
        {
            return StoredIDObject.GetObject(i).obj.SetPositionScaleAndRotation(new Vector2(px, py), new Vector2(sx, sy), r).SetLayer(l);
        }
    }
}
