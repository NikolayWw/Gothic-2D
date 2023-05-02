using UnityEditor;

namespace CodeBase.Editor
{
    public class SaveLoadEditor
    {
        private const string SaveLoadPath = "Tools/Save/";

        [MenuItem(SaveLoadPath + "ClearSave")]
        private static void Clear()
        {
            // File.WriteAllText(GameConstants.SaveGameFilePath, string.Empty);
        }
    }
}