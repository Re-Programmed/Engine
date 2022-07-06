using Engine.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Utils
{
    class IDObject
    {
        public int ID;
        public GameObject gameObject;

        public IDObject(int ID, GameObject gameObject)
        {
            this.ID = ID;
            this.gameObject = gameObject;
        }
    }
}
