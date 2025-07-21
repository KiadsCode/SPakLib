namespace SPakLib
{
    /// <summary>
    /// Defines methods for saving and loading <see cref="ShaderPak"/> instances in a specific binary format.
    /// </summary>
    public interface IShaderPakFormat
    {
        /// <summary>
        /// Saves the specified <see cref="ShaderPak"/> to a file at the given path.
        /// </summary>
        /// <param name="path">The file system path where the shader package will be saved.</param>
        /// <param name="pak">The <see cref="ShaderPak"/> instance to save.</param>
        void Save(string path, ShaderPak pak);

        /// <summary>
        /// Loads a <see cref="ShaderPak"/> from a file at the given path.
        /// </summary>
        /// <param name="path">The file system path from which to load the shader package.</param>
        /// <returns>The loaded <see cref="ShaderPak"/> instance.</returns>
        ShaderPak Load(string path);
    }
}