using Engine.Objects;
using Engine.Rendering.Sprites;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Engine.DevUtils
{
    class ObjEditor
    {
        static bool mouseWasDown = false;
        static bool mouseWasDownL = false;

        static GameObject selectedObject;

        public static void Update(Game.Game game)
        {
            Vector2 smouse = game.cam.MouseToWorldCoords(Input.Input.GetMousePosition());

            selectedObject?.SetPosition(new Vector2(smouse.X - selectedObject.scale.X / 2, smouse.Y - selectedObject.scale.Y / 2));

            bool end = false;

            if (Input.Input.GetMouseButton(GLFW.MouseButton.Left))
            {
                if (mouseWasDownL) { return; }
                mouseWasDownL = true;

                if(selectedObject == null)
                {
                    foreach (ObjectLayer ol in game.objects.Values)
                    {
                        foreach (GameObject obj in ol.objects)
                        {
                            if (!obj.editor) { continue; }
                            Vector2 mouse = game.cam.MouseToWorldCoords(Input.Input.GetMousePosition());

                            if (obj.position.X + obj.scale.X > mouse.X && obj.position.X < mouse.X)
                            {
                                if (obj.position.Y + obj.scale.Y > mouse.Y && obj.position.Y < mouse.Y)
                                {
                                    selectedObject = obj;
                                    end = true;
                                    break;
                                }
                            }
                        }

                        if (end)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    GameObject obj = selectedObject.children[0];

                    obj.SetPosition(selectedObject.position);
                    obj.SetScale(selectedObject.scale);
                    obj.SetRotation(selectedObject.rotation);

                    selectedObject.RemoveChild(obj);

                    selectedObject.Destroy();

                    game.objects[5].objects.Remove(selectedObject);

                    selectedObject = null;
                }
            }
            else
            {
                mouseWasDownL = false;
            }

            end = false;

            if (Input.Input.GetMouseButton(GLFW.MouseButton.Right))
            {
                if (mouseWasDown) { return; }
                mouseWasDown = true;

                foreach (ObjectLayer ol in game.objects.Values)
                {
                    foreach(GameObject obj in ol.objects)
                    {
                        if (obj.ignoreEditing) { continue; }
                        Vector2 mouse = game.cam.MouseToWorldCoords(Input.Input.GetMousePosition());

                        if (obj.position.X + obj.scale.X > mouse.X && obj.position.X < mouse.X)
                        {
                            if (obj.position.Y + obj.scale.Y > mouse.Y && obj.position.Y < mouse.Y)
                            {
                                GameObject edobj = GameObject.CreateGameObjectSprite(obj.position, obj.scale, obj.rotation, SpriteRenderer.quadTextureVerts, "editor_sprite");
                                game.objects[5].objects.Add(edobj);
                                edobj.editor = true;

                                edobj.AddChild(obj);
                                obj.SetPosition(Vector2.Zero);
                                obj.SetScale(Vector2.One);
                                obj.SetRotation(0f);

                                end = true;
                                break;
                            }
                        }
                    }

                    if(end)
                    {
                        break;
                    }
                }
            }
            else
            {
                mouseWasDown = false;
            }
        }
    }
}
