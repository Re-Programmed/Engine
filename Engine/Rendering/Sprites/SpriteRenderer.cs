using Engine.Objects;
using Engine.Rendering.Display;
using GLFW;
using System;
using System.Collections.Generic;
using System.Numerics;
using Engine.Utils;
using System.Text;
using static Engine.OpenGL.GL;

namespace Engine.Rendering.Sprites
{
    class SpriteRenderer
    {
        private SpriteShader shader;

        public float[] verts;

        public static float[] quadTextureVerts = { 
        // pos      // tex
        0.0f, 0.0f, 0.0f, 0.0f,
        0.0f, 1.0f, 0.0f, 1.0f,
        1.0f, 0.0f, 1.0f, 0.0f,

        0.0f, 1.0f, 0.0f, 1.0f,
        1.0f, 1.0f, 1.0f, 1.0f,
        1.0f, 0.0f, 1.0f, 0.0f
    };

    public uint quadVAO;

        public SpriteRenderer(SpriteShader shader)
        {
            this.shader = shader;
        }

        public SpriteRenderer()
        {

        }

        public void DrawSprite(Cameras.Camera2d cam, GameObject obj, Texture2D texture, Vector3 color, bool UI = false)
        {
            Matrix4x4 trans = Matrix4x4.CreateTranslation(obj.position.X, obj.position.Y, 0);
            Matrix4x4 sca = Matrix4x4.CreateScale(obj.scale.X, obj.scale.Y, 1);
            Matrix4x4 rot = Matrix4x4.CreateRotationZ(obj.rotation);

            Matrix4x4 flipMat = Matrix4x4.CreateRotationY(Engine.Utils.Math.DegToRad(180));

            if(UI)
            {
                trans = Matrix4x4.CreateTranslation(obj.position.X / cam.Zoom + cam.FocusPosition.X, obj.position.Y / cam.Zoom + cam.FocusPosition.Y, 0);
                sca = Matrix4x4.CreateScale(obj.scale.X / cam.Zoom, obj.scale.Y / cam.Zoom, 1);
            }

            if(obj.parent != null)
            {
                trans = Matrix4x4.CreateTranslation(obj.position.X + obj.parent.position.X, obj.position.Y + obj.parent.position.Y, 0);
                sca = Matrix4x4.CreateScale(obj.scale.X * obj.parent.scale.X, obj.scale.Y * obj.parent.scale.Y, 1);
                rot = Matrix4x4.CreateRotationZ(obj.rotation + obj.parent.rotation);
            }

            if(obj.texture.flipped)
            {
                if (obj.parent != null)
                {
                    trans = Matrix4x4.CreateTranslation(obj.position.X + obj.parent.position.X + (obj.scale.X * obj.parent.scale.X), obj.position.Y + obj.parent.position.Y, 0);
                }
                else
                {
                    trans = Matrix4x4.CreateTranslation(obj.position.X + obj.scale.X, obj.position.Y, 0);
                }

                shader.SetMatrix4x4("model", sca * flipMat * rot * trans, false);
            }
            else
            {
                shader.SetMatrix4x4("model", sca * rot * trans, false);
            }

            shader.Use();
            shader.SetMatrix4x4("projection", cam.GetProjectionMatrix(), false);
            shader.SetVector3f("spriteColor", color, false);

            glActiveTexture(GL_TEXTURE0);
            texture.Bind();

            glDrawArrays(GL_TRIANGLES, 0, obj.vertices.Length);
        }

        public void DrawSprite(Cameras.Camera2d cam, Vector2 pos, Vector2 scale, float rotation, float[] vertices, Texture2D texture, Vector3 color)
        {
            Matrix4x4 trans = Matrix4x4.CreateTranslation(pos.X, pos.Y, 0);
            Matrix4x4 sca = Matrix4x4.CreateScale(scale.X, scale.Y, 1);
            Matrix4x4 rot = Matrix4x4.CreateRotationZ(rotation);

            shader.SetMatrix4x4("model", sca * rot * trans, false);

            shader.Use();
            shader.SetMatrix4x4("projection", cam.GetProjectionMatrix(), false);
            shader.SetVector3f("spriteColor", color, false);

            glActiveTexture(GL_TEXTURE0);
            texture.Bind();

            glDrawArrays(GL_TRIANGLES, 0, vertices.Length);
        }

        public unsafe void initRenderData()
        {
            uint VBO = 0;

            float[] vertices = { 
        // pos      // tex
        0.0f, 0.0f, 0.0f, 0.0f,
        0.0f, 1.0f, 0.0f, 1.0f,
        1.0f, 0.0f, 1.0f, 0.0f,

        0.0f, 1.0f, 0.0f, 1.0f,
        1.0f, 1.0f, 1.0f, 1.0f,
        1.0f, 0.0f, 1.0f, 0.0f
    };
            verts = vertices;
            quadVAO = glGenVertexArray();
            VBO = glGenBuffer();

            glBindBuffer(GL_ARRAY_BUFFER, VBO);

            fixed (float* v = &vertices[0])
            {
                glBufferData(GL_ARRAY_BUFFER, sizeof(float) * vertices.Length, v, GL_STATIC_DRAW);
            }

            glBindVertexArray(quadVAO);

            glEnableVertexAttribArray(0);
            glVertexAttribPointer(0, 4, GL_FLOAT, false, 4 * sizeof(float), (void*)0);
            glBindBuffer(GL_ARRAY_BUFFER, 0);
            glBindVertexArray(0);
        }
    }
}
