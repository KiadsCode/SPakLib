using System.Collections.Generic;

namespace SPakLib
{
    /// <summary>
    /// Represents a container for storing named shader bytecode blobs.
    /// </summary>
    public class ShaderPak
    {
        private readonly Dictionary<string, byte[]> _shaders = new Dictionary<string, byte[]>();

        /// <summary>
        /// Gets the names of all shaders stored in this package.
        /// </summary>
        public IEnumerable<string> ShaderNames => _shaders.Keys;

        /// <summary>
        /// Adds a shader bytecode entry to the package.
        /// If a shader with the same name already exists, it will be overwritten.
        /// </summary>
        /// <param name="name">The name of the shader.</param>
        /// <param name="bytecode">The compiled shader bytecode.</param>
        public void AddShader(string name, byte[] bytecode)
        {
            _shaders[name] = bytecode;
        }

        /// <summary>
        /// Tries to retrieve the shader bytecode for a given name.
        /// </summary>
        /// <param name="name">The name of the shader.</param>
        /// <param name="bytecode">When this method returns, contains the shader bytecode if found; otherwise, <c>null</c>.</param>
        /// <returns><c>true</c> if the shader was found; otherwise, <c>false</c>.</returns>
        public bool TryGetShader(string name, out byte[] bytecode)
        {
            return _shaders.TryGetValue(name, out bytecode);
        }

        /// <summary>
        /// Gets internal access to the raw shader dictionary.
        /// This is intended for use by SPak format handlers only.
        /// </summary>
        internal Dictionary<string, byte[]> InternalShaders => _shaders;
    }
}
