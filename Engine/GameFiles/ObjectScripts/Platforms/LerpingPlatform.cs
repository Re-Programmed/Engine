using Engine.Game;
using Engine.Objects;
using Engine.Objects.Components;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.GameFiles.ObjectScripts.Platforms
{
    class LerpingPlatform : ScriptComponent, PreregisteredObjectComponent
    {
        readonly Vector2 travelDistance;
        Vector2 startPosition;
        readonly float speed;

        bool direction = false; //To end if false, start if true.

        public override void ScriptUpdate(TestGame game)
        {
            Vector2 d = direction ? startPosition : startPosition + travelDistance;
            gameObject.SetPosition(Vector2.Lerp(gameObject.position, d, speed * GameTime.TimeScale));

            if(Vector2.Distance(gameObject.position, d) < 0.03f)
            {
                direction = !direction;
            }
        }

        public override void Start()
        {
            startPosition = gameObject.position;
        }

        public LerpingPlatform(Vector2 travelDistance, float speed = 0.002f)
        {
            this.travelDistance = travelDistance;
            this.speed = speed;
        }

        public LerpingPlatform()
        {

        }

        public GameObject[] GetMyObjects()
        {
            GameObject rx20 = GameObject.CreateGameObjectSprite(Vector2.Zero, new Vector2(20f, 5f), 0f, TestGame.INSTANCE.sr.verts, "black");
            rx20.AddComponent(new LerpingPlatform(Vector2.UnitX * 20f));
            Physics.PhysicsAffected pa = new Physics.PhysicsAffected();
            pa.SetStatic(true);
            rx20.AddComponent(pa);

            return new GameObject[] { rx20 };
        }

    }
}
