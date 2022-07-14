using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;

namespace Engine.Resources
{
    static class ResourceReader
    {
        /// <summary>
        /// Create a resource stored in the misc folder.
        /// </summary>
        /// <param name="path">The subfolders of misc to add the file to -- with the file name. (ex. "extra/addons/file.json")</param>
        /// <param name="content">What to add to the file.</param>
        public static void GenerateResource(string path, string content)
        {
            File.WriteAllText("../../../misc/" + path, content);
        }

        /// <summary>
        /// Create a resource stored in the misc folder that is encoded to hex.
        /// </summary>
        /// <param name="path">The subfolders of misc to add the file to -- with the file name. (ex. "extra/addons/file.json")</param>
        /// <param name="content">What to add to the file.</param>
        /// <param name="encoder">What kind of encoding to use. Defaults to UTF-8.</param>
        public static void GenerateEncodedResource(string path, string content, Encoding encoder = null)
        {
            if(encoder == null) { encoder = Encoding.UTF8; }

            GenerateResource(path, GenerateHexString(content, encoder));
        }

        /// <summary>
        /// Creates a json resource, encoded in hex, from an object.
        /// </summary>
        /// <param name="path">The subfolders of misc to add the file to -- with the file name. (ex. "extra/addons/file.json")</param>
        /// <param name="content">The object that will be written to the file.</param>
        /// <param name="encoder">What kind of encoding to use. Defaults to UTF-8.</param>
        public static void GenerateEncodedJSONResource(string path, object content, Encoding encoder = null)
        {
            GenerateEncodedResource(path, JSONSerialize(content), encoder);
        }

        /// <summary>
        /// Returns the string of text in a file.
        /// </summary>
        /// <param name="path">The subfolders of misc to read the file from -- with the file name. (ex. "extra/addons/file.json")</param>
        /// <returns>The string of text in the specified file.</returns>
        public static string ReadResource(string path)
        {
            return File.ReadAllText("../../../misc/" + path);
        }

        /// <summary>
        /// Returns the string of text in an encoded file.
        /// </summary>
        /// <param name="encoder">The encoding to use.</param>
        /// <param name="path">The subfolders of misc to read the file from -- with the file name. (ex. "extra/addons/file.json")</param>
        /// <returns>The string of text in the specified file.</returns>
        public static string ReadEncodedResource(string path, Encoding encoder)
        {
            return HexToString(ReadResource(path), encoder);
        }

        /// <summary>
        /// Reads a json resource and returns an object.
        /// </summary>
        /// <typeparam name="T">The object type that the json represents.</typeparam>
        /// <param name="encoder">The encoding to use.</param>
        /// <param name="path">The subfolders of misc to read the file from -- with the file name. (ex. "extra/addons/file.json")</param>
        /// <returns>The object from the json read.</returns>
        public static T ReadEncodedJSONResource<T>(string path, Encoding encoder)
        {
            return JSONDeserialize<T>(ReadEncodedResource(path, encoder));
        }


        /// <summary>
        /// Creates a hex string from an input string.
        /// </summary>
        /// <param name="in_s">Input</param>
        /// <param name="e">Encoding type to use.</param>
        /// <returns>A hex string of the input.</returns>
        static string GenerateHexString(string in_s, Encoding e)
        {
            return Convert.ToBase64String(e.GetBytes(in_s));
        }

        /// <summary>
        /// Creates a string from a hex string.
        /// </summary>
        /// <param name="in_s">Hex string.</param>
        /// <param name="e">Encoding to use.</param>
        /// <returns>String from hex.</returns>
        static string HexToString(string in_s, Encoding e)
        {
            return e.GetString(Convert.FromBase64String(in_s));
        }

        /// <summary>
        /// Convert an object to a json string. Useful for writing class data to a file.
        /// </summary>
        /// <param name="in_s">The object to serialize.</param>
        /// <returns>JSON string.</returns>
        static string JSONSerialize(object in_s)
        {
            return JsonSerializer.Serialize(in_s);
        }

        /// <summary>
        /// Convert a json string to an object of type T.
        /// </summary>
        /// <typeparam name="T">The object type to convert the json string to.</typeparam>
        /// <param name="in_s">The json string.</param>
        /// <returns>The object converted from in_s.</returns>
        static T JSONDeserialize<T>(string in_s)
        {
            return JsonSerializer.Deserialize<T>(in_s);
        }
    }
}
