using Engine.Rendering.Sprites;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using static Engine.OpenGL.GL;
using StbImageSharp;
using GLFW;

namespace Engine.Resources
{
    class ResourceManager
    {
        public static Dictionary<string, SpriteShader> SpriteShaders = new Dictionary<string, SpriteShader>();
        public static Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();

        public static SpriteShader LoadSpriteShader(string vShaderFile, string fShaderFile, string gShaderFile, string name)
        {
            SpriteShaders[name] = loadShaderFromFile(vShaderFile, fShaderFile, gShaderFile);
            return SpriteShaders[name];
        }

        public static SpriteShader GetSpriteShader(string name)
        {
            return SpriteShaders[name];
        }

        public static Texture2D LoadTexture(string file, bool alpha, string name)
        {
            Textures.Add(name, loadTextureFromFile(file, alpha));
            return Textures[name];
        }

        public static Texture2D GetTexture(string name)
        {
            return Textures[name];
        }

        public unsafe static void Clear()
        {
            foreach(SpriteShader shader in SpriteShaders.Values)
            {
                glDeleteProgram(shader.ID);
            }

            foreach(Texture2D texture in Textures.Values)
            {
                glDeleteTexture(texture.ID);
            }
        }

        private ResourceManager()
        {

        }

        public static SpriteShader loadShaderFromFile(string vShaderFile, string fShaderFile, string gShaderFile = null)
        {
            string vertexCode = "";
            string fragmentCode = "";
            string geometryCode = "";

            using (StreamReader reader = new StreamReader(vShaderFile))
            {
                vertexCode = reader.ReadToEnd();
            }

            using (StreamReader reader = new StreamReader(fShaderFile))
            {
                fragmentCode = reader.ReadToEnd();
            }

            if(gShaderFile != null)
            {
                using (StreamReader reader = new StreamReader(gShaderFile))
                {
                    geometryCode = reader.ReadToEnd();
                }
            }

            string vShaderCode = vertexCode;
            string fShaderCode = fragmentCode;
            string gShaderCode = geometryCode;

            SpriteShader shader = new SpriteShader();
            shader.Compile(vShaderCode, fShaderCode, gShaderCode != null ? gShaderCode : null);
            return shader;
        }

        public static unsafe Texture2D loadTextureFromFile(string file, bool alpha)
        {
            Texture2D texture = new Texture2D();

            if(alpha)
            {
                texture.Internal_Format = GL_RGBA;
                texture.Image_Format = GL_RGBA;
            }

            int width, height;

            using (var stream = File.OpenRead(file))
            {
                ImageResult image = null;
                if (alpha)
                {
                    image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                }
                else
                {
                    image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlue);
                }

                width = image.Width;
                height = image.Height;
                
                texture.Generate(width, height, image.Data);
                return texture;
            }
        }
    }
}
