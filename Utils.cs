using System;
using System.IO;
using System.IO.Compression;

namespace SPakLib
{
    /// <summary>
    /// Provides internal utility methods for compression and encoding used by SPackLib.
    /// </summary>
    internal static class Utils
    {
        /// <summary>
        /// Writes a 32-bit integer to the stream using a 7-bit variable-length encoding.
        /// </summary>
        /// <param name="writer">The <see cref="BinaryWriter"/> to write to.</param>
        /// <param name="value">The 32-bit integer to encode and write.</param>
        /// <remarks>
        /// This encoding stores smaller values using fewer bytes and is compatible with the format used by .NET's <see cref="BinaryWriter.Write7BitEncodedInt(int)"/>.
        /// </remarks>
        public static void Write7BitEncodedInt(BinaryWriter writer, int value)
        {
            uint v = (uint)value;
            while (v >= 0x80)
            {
                writer.Write((byte)(v | 0x80));
                v >>= 7;
            }
            writer.Write((byte)v);
        }

        /// <summary>
        /// Reads a 32-bit integer from the stream that was encoded using 7-bit variable-length encoding.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> to read from.</param>
        /// <returns>The decoded 32-bit integer.</returns>
        /// <exception cref="FormatException">
        /// Thrown if the encoded integer is invalid or too large (more than 5 bytes).
        /// </exception>
        public static int Read7BitEncodedInt(BinaryReader reader)
        {
            int result = 0;
            int shift = 0;
            byte b;

            do
            {
                if (shift >= 35)
                    throw new FormatException("Too many bytes in 7-bit encoded int.");

                b = reader.ReadByte();
                result |= (b & 0x7F) << shift;
                shift += 7;
            }
            while ((b & 0x80) != 0);

            return result;
        }

        /// <summary>
        /// Compresses the input byte array using GZip compression.
        /// </summary>
        /// <param name="data">The byte array to compress.</param>
        /// <returns>A compressed byte array using GZip.</returns>
        public static byte[] GZip_Compress(byte[] data)
        {
            using (var ms = new MemoryStream())
            {
                using (var gzip = new GZipStream(ms, CompressionLevel.Optimal))
                {
                    gzip.Write(data, 0, data.Length);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Decompresses the input GZip-compressed byte array.
        /// </summary>
        /// <param name="data">The GZip-compressed byte array.</param>
        /// <returns>The decompressed byte array.</returns>
        public static byte[] GZip_Decompress(byte[] data)
        {
            using (var input = new MemoryStream(data))
            using (var gzip = new GZipStream(input, CompressionMode.Decompress))
            using (var output = new MemoryStream())
            {
                gzip.CopyTo(output);
                return output.ToArray();
            }
        }
    }
}
