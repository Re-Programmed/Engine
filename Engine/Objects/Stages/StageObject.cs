﻿using Engine.Objects.Components;
using Engine;
using Engine.Objects;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using static Engine.Objects.Components.Component;

namespace Engine.Objects.Stages
{
    /// <summary>
    /// Stage objects are GameObjects that do not exist, but can be loaded in.
    /// </summary>
    class StageObject
    {
        public float xPos { get; set; }
        public float yPos { get; set; }
        public float xScale { get; set; }
        public float yScale { get; set; }

        public float rotation { get; set; }

        public string texture { get; set; }

        public int Layer { get; set; } = 4;

        public bool physics { get; set; } = false;

        public List<ComponentData> triggers { get; set; } = new List<ComponentData>();

        public List<ComponentData> comps { get; set; } = new List<ComponentData>();

        public Physics.PhysicsObjectSettings PhysicsSettings { get; set; }

        internal StageObject SetPositionScaleAndRotation(Vector2 position, Vector2 scale, float rotation)
        {
            this.xPos = position.X;
            this.yPos = position.Y;
            this.xScale = scale.X;
            this.yScale = scale.Y;
            this.rotation = rotation;

            return this;
        }

        internal StageObject SetLayer(int layer)
        {
            this.Layer = layer;
            return this;
        }

        public StageObject(Vector2 position, Vector2 scale, float rotation, string textureName, int layer, List<Component> components)
        {
            xPos = position.X;
            yPos = position.Y;

            xScale = scale.X;
            yScale = scale.Y;

            this.rotation = rotation;

            texture = textureName;
            Layer = layer;

            foreach(Component c in components)
            {
                if (c.GetComponentType() == ComponentType.PhysicsRel)
                {
                    physics = true;
                    PhysicsSettings = ((Physics.PhysicsAffected)c).settings;
                }
                else
                {
                    if (c is Trigger)
                    {
                        Trigger t = c as Trigger;
                        triggers.Add(t.data);
                    }
                    else
                    {
                        comps.Add(c.GetComponentData());
                    }
                }
            }
        }

        public StageObject(GameObject obj)
        {
            xPos = obj.position.X;
            yPos = obj.position.Y;

            xScale = obj.scale.X;
            yScale = obj.scale.Y;

            rotation = obj.rotation;

            texture = obj.texture.textureName;

            Layer = obj.Layer;

            foreach (Component c in obj.GetComponents())
            {
                if (c.GetComponentType() == ComponentType.PhysicsRel)
                {
                    physics = true;
                    PhysicsSettings = ((Physics.PhysicsAffected)c).settings;
                }
                else
                {
                    if (c is Trigger)
                    {
                        Trigger t = c as Trigger;
                        triggers.Add(t.data);
                    }
                    else
                    {
                        comps.Add(c.GetComponentData());
                    }
                }
            }
        }

        public StageObject()
        {
            
        }

        public GameObject LoadObject(TestGame game)
        {
            GameObject myObject = GameObject.CreateGameObjectSprite(new Vector2(xPos, yPos), new Vector2(xScale, yScale), rotation, game.sr.verts, texture);

            if(physics)
            {
                Physics.PhysicsAffected phy = new Physics.PhysicsAffected();
                phy.settings = PhysicsSettings;
                myObject.AddComponent(phy);
            }

            foreach(ComponentData t in triggers)
            {
                myObject.AddComponent(t.GenerateTriggerFromData());
            }

            foreach(ComponentData cd in comps)
            {
                myObject.AddComponent(cd.GenerateGeneralFromData());
            }

            game.Instantiate(myObject, Layer);

            return myObject;
            
        }
    }


}
