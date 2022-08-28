using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Objects.Components
{
    interface PreregisteredObjectComponent
    {
        public GameObject[] GetMyObjects();
    }
}
