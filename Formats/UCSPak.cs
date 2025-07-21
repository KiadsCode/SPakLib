using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace SPakLib.Formats
{
    /// <summary>
    /// Represents a shader package format that uses 7-bit encoding, LUT table and GZip compression.
    /// </summary>
    /// <remarks>
    /// UCSP (Ultra Compressed Shader Pak) is a high-efficiency format that minimizes shader data size
    /// using 7-bit integer encoding for metadata and GZip compression for shader bytecode. 
    /// It includes a LUT (lookup table) that maps shader names to offsets and lengths within a single data block.
    /// </remarks>
    public class UCSPak : IShaderPakFormat
    {
        /// <summary>
        /// Saves the given <see cref="ShaderPak"/> to a file using the UCSP format.
        /// </summary>
        /// <param name="path">The output file path.</param>
        /// <param name="pak">The shader package to save.</param>
        /// <exception cref="IOException">Thrown if an error occurs while writing to the file.</exception>
        public void Save(string path, ShaderPak pak)
        {
            using (var fs = File.Create(path))
            using (var bw = new BinaryWriter(fs))
            {
                // Header
                bw.Write(Encoding.ASCII.GetBytes("UCSP"));
                bw.Write((byte)1); // Version
                bw.Write((byte)1); // Compression type = GZip

                Utils.Write7BitEncodedInt(bw, pak.InternalShaders.Count);

                var dataBuffer = new MemoryStream();
                var dataOffsets = new List<(string name, int offset, int length)>();

                foreach (var kvp in pak.InternalShaders)
                {
                    int start = (int)dataBuffer.Position;

                    using (var gz = new GZipStream(dataBuffer, CompressionLevel.Optimal, true))
                    {
                        gz.Write(kvp.Value, 0, kvp.Value.Length);
                    }

                    int length = (int)dataBuffer.Position - start;
                    dataOffsets.Add((kvp.Key, start, length));
                }

                // LUT Table
                foreach (var entry in dataOffsets)
                {
                    byte[] nameBytes = Encoding.UTF8.GetBytes(entry.name);
                    Utils.Write7BitEncodedInt(bw, nameBytes.Length);
                    bw.Write(nameBytes);
                    Utils.Write7BitEncodedInt(bw, entry.offset);
                    Utils.Write7BitEncodedInt(bw, entry.length);
                }

                // Data Chunk
                bw.Write(dataBuffer.ToArray());
            }
        }

        /// <summary>
        /// Loads a shader package from a UCSP file.
        /// </summary>
        /// <param name="path">The input file path.</param>
        /// <returns>A new <see cref="ShaderPak"/> instance containing the loaded shaders.</returns>
        /// <exception cref="InvalidDataException">Thrown if the file is not a valid UCSP package.</exception>
        /// <exception cref="NotSupportedException">Thrown if the UCSP version or compression type is unsupported.</exception>
        public ShaderPak Load(string path)
        {
            ShaderPak pak = new ShaderPak();

            using (var fs = File.OpenRead(path))
            using (var br = new BinaryReader(fs))
            {
                // Header
                string magic = Encoding.ASCII.GetString(br.ReadBytes(4));
                if (magic != "UCSP")
                    throw new InvalidDataException("The file is not a valid UCSP package.");

                byte version = br.ReadByte();
                if (version != 1)
                    throw new NotSupportedException($"UCSP version {version} is not supported.");

                byte compression = br.ReadByte();
                if (compression != 1)
                    throw new NotSupportedException($"Compression type {compression} is not supported (only GZip is implemented).");

                int shaderCount = Utils.Read7BitEncodedInt(br);

                var entries = new List<(string name, int offset, int length)>();
                for (int i = 0; i < shaderCount; i++)
                {
                    int nameLength = Utils.Read7BitEncodedInt(br);
                    string name = Encoding.UTF8.GetString(br.ReadBytes(nameLength));
                    int offset = Utils.Read7BitEncodedInt(br);
                    int length = Utils.Read7BitEncodedInt(br);

                    entries.Add((name, offset, length));
                }

                long dataStart = fs.Position;

                foreach (var entry in entries)
                {
                    fs.Seek(dataStart + entry.offset, SeekOrigin.Begin);
                    byte[] compressedBytes = br.ReadBytes(entry.length);

                    using (var ms = new MemoryStream(compressedBytes))
                    using (var gz = new GZipStream(ms, CompressionMode.Decompress))
                    using (var outMs = new MemoryStream())
                    {
                        gz.CopyTo(outMs);
                        byte[] shaderBytes = outMs.ToArray();

                        pak.AddShader(entry.name, shaderBytes);
                    }
                }
            }

            return pak;
        }
    }
}
