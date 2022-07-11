using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.Objects.Components.UIComponents
{
    abstract class Button : Component
    {
        /// <summary>
        /// Lists all buttons in order for selection.
        /// </summary>
        public static List<Button> buttons_ordered { get; protected set; } = new List<Button>();

        protected GameObject gameObject;

        public ComponentData data { get; set; }

        public bool selected { get; protected set; } = false;

        public T GetClone<T>()
        {
            return (T)MemberwiseClone();
        }

        public abstract Component.ComponentType GetComponentType();

        public void Init(GameObject parent)
        {
            gameObject = parent;
            buttons_ordered.Add(this);
        }

        public void OnDisable(TestGame game)
        {

        }

        public void OnReEnable(TestGame game)
        {

        }

        /// <summary>
        /// Called every frame the mouse is over the object.
        /// </summary>
        public virtual void Hover() { }

        /// <summary>
        /// Called every frame the mouse is not over the object.
        /// </summary>
        public virtual void ReleaseHover() { }

        /// <summary>
        /// Called when the object is selected using the keyboard.
        /// </summary>
        public virtual void Select()
        {
            selected = true;
        }

        /// <summary>
        /// Called when the object is deselected using the keyboard.
        /// </summary>
        public virtual void Deselect()
        {
            selected = false;
        }

        public void Update(TestGame game)
        {
            if(ButtonSelectionManager.UsingSelection)
            {
                if(selected)
                {
                    if (Input.Input.GetMouseButton(GLFW.MouseButton.Left))
                    {
                        Click(game);
                    }
                }
            }
            else
            {
                Vector2 mousePosition = game.cam.MouseToWorldCoords(Input.Input.GetMousePosition());

                if (Vector2.Distance(gameObject.position + (gameObject.scale/2f), mousePosition) <= gameObject.scale.X)
                {
                    Hover();
                    if (Input.Input.GetMouseButton(GLFW.MouseButton.Left))
                    {
                        Click(game);
                    }
                }
                else
                {
                    ReleaseHover();
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
