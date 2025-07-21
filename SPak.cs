namespace SPakLib
{
    /// <summary>
    /// Static loader for shader packs with generic format support.
    /// </summary>
    public static class SPak
    {
        /// <summary>
        /// Loads a shader pack using a given format implementation.
        /// </summary>
        public static ShaderPak Load<T>(string path) where T : IShaderPakFormat, new()
        {
            var format = new T();
            return format.Load(path);
        }

        /// <summary>
        /// Saves a shader pack using a given format implementation.
        /// </summary>
        public static void Save<T>(string path, ShaderPak pak) where T : IShaderPakFormat, new()
        {
            var format = new T();
            format.Save(path, pak);
        }
    }
}
