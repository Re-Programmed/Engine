using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Objects.Components.UIComponents
{
    abstract class Button : Component
    {
        GameObject gameObject;

        public ComponentData data { get; set; }

        public T GetClone<T>()
        {
            return (T)MemberwiseClone();
        }

        public abstract Component.ComponentType GetComponentType();

        public void Init(GameObject parent)
        {
            gameObject = parent;
        }

        public void OnDisable(TestGame game)
        {

        }

        public void OnReEnable(TestGame game)
        {

        }

        public void Update(TestGame game)
        {
            Vector2 mousePosition = game.cam.MouseToWorldCoords(Input.Input.GetMousePosition());

            if(Vector2.Distance(gameObject.position, mousePosition) <= gameObject.scale.X)
            {
                if(Input.Input.GetMouseButton(GLFW.MouseButton.Left))
                {
                    Click(game);
                }
            }
        }

        public Button(ComponentData data)
        {
            this.data = data;
            this.data.type = Component.ComponentType.UI_Button_Stage;
        }

        public Button()
        {
            data = new ComponentData();
            data.type = Component.ComponentType.UI_Button_Stage;
        }

        public abstract void Click(TestGame game);
    }
}
