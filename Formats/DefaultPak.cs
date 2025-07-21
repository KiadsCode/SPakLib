using System.IO;
using System.Text;

namespace SPakLib.Formats
{
    /// <summary>
    /// Represents a basic shader package format using unoptimized layout and GZip compression.
    /// </summary>
    /// <remarks>
    /// The Default format stores each shader individually with its UTF-8 encoded name and GZip-compressed bytecode.
    /// It is simple and human-readable but not optimized for performance or size.
    /// </remarks>
    public class DefaultPak : IShaderPakFormat
    {
        /// <summary>
        /// Saves the given <see cref="ShaderPak"/> to a file using the default format.
        /// </summary>
        /// <param name="path">The file path to save the package.</param>
        /// <param name="pak">The shader package to write.</param>
        /// <exception cref="IOException">Thrown if an error occurs during file I/O operations.</exception>
        public void Save(string path, ShaderPak pak)
        {
            using (var fs = File.Create(path))
            using (var bw = new BinaryWriter(fs, Encoding.UTF8, leaveOpen: true))
            {
                // Write number of shaders
                bw.Write(pak.InternalShaders.Count);

                foreach (var kvp in pak.InternalShaders)
                {
                    byte[] compressed = Utils.GZip_Compress(kvp.Value);

                    // Write shader name
                    byte[] nameBytes = Encoding.UTF8.GetBytes(kvp.Key);
                    bw.Write(nameBytes.Length);
                    bw.Write(nameBytes);

                    // Write compressed shader bytecode
                    bw.Write(compressed.Length);
                    bw.Write(compressed);
                }
            }
        }

        /// <summary>
        /// Loads a <see cref="ShaderPak"/> from a file encoded in the default format.
        /// </summary>
        /// <param name="path">The path to the shader package file.</param>
        /// <returns>The loaded <see cref="ShaderPak"/>.</returns>
        /// <exception cref="IOException">Thrown if the file is unreadable or corrupted.</exception>
        public ShaderPak Load(string path)
        {
            var pak = new ShaderPak();

            using (var fs = File.OpenRead(path))
            using (var br = new BinaryReader(fs, Encoding.UTF8, leaveOpen: true))
            {
                int count = br.ReadInt32();

                for (int i = 0; i < count; i++)
                {
                    int nameLen = br.ReadInt32();
                    string name = Encoding.UTF8.GetString(br.ReadBytes(nameLen));

                    int compressedLen = br.ReadInt32();
                    byte[] compressed = br.ReadBytes(compressedLen);

                    byte[] decompressed = Utils.GZip_Decompress(compressed);
                    pak.AddShader(name, decompressed);
                }
            }

            return pak;
        }
    }
}
