using System.IO;
using System.Text;

namespace SPakLib.Formats
{
    /// <summary>
    /// Represents a shader package format using 7-bit encoded integers and GZip compression.
    /// </summary>
    /// <remarks>
    /// The Alternative format reduces file size slightly by replacing 32-bit integers with 7-bit variable-length encoding.
    /// This format is faster and smaller than <see cref="DefaultPak"/>, but less efficient than <see cref="UCSPak"/>.
    /// </remarks>
    public class Alt7BitPak : IShaderPakFormat
    {
        /// <summary>
        /// Saves the given <see cref="ShaderPak"/> to a file using the 7-bit encoded format.
        /// </summary>
        /// <param name="path">The file path to save the package.</param>
        /// <param name="pak">The shader package to write.</param>
        /// <exception cref="IOException">Thrown if an error occurs during file I/O operations.</exception>
        public void Save(string path, ShaderPak pak)
        {
            using (var fs = File.Create(path))
            using (var bw = new BinaryWriter(fs, Encoding.UTF8, leaveOpen: true))
            {
                // Write number of shaders with 7-bit encoding
                Utils.Write7BitEncodedInt(bw, pak.InternalShaders.Count);

                foreach (var kvp in pak.InternalShaders)
                {
                    byte[] compressed = Utils.GZip_Compress(kvp.Value);
                    byte[] nameBytes = Encoding.UTF8.GetBytes(kvp.Key);

                    // Write shader name length and name
                    Utils.Write7BitEncodedInt(bw, nameBytes.Length);
                    bw.Write(nameBytes);

                    // Write compressed shader bytecode
                    Utils.Write7BitEncodedInt(bw, compressed.Length);
                    bw.Write(compressed);
                }
            }
        }

        /// <summary>
        /// Loads a <see cref="ShaderPak"/> from a file encoded in the 7-bit encoded format.
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
                // Read number of shaders
                int count = Utils.Read7BitEncodedInt(br);

                for (int i = 0; i < count; i++)
                {
                    int nameLen = Utils.Read7BitEncodedInt(br);
                    string name = Encoding.UTF8.GetString(br.ReadBytes(nameLen));

                    int compressedLen = Utils.Read7BitEncodedInt(br);
                    byte[] compressed = br.ReadBytes(compressedLen);

                    byte[] decompressed = Utils.GZip_Decompress(compressed);
                    pak.AddShader(name, decompressed);
                }
            }

            return pak;
        }
    }
}
