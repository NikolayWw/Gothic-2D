namespace Plugins.Creator_Kit___RPG.Scripts.Core
{
    /// <summary>
    /// A static class which maps a type to a single instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal static class InstanceRegister<T> where T : class, new()
    {
        public static T instance = new T();
    }
}