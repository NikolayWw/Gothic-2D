using UnityEditor;

namespace Plugins.Creator_Kit___RPG.Scripts.Tiles.Rule_Tile.Scripts.Editor
{
    internal static class CustomRuleTileMenu
    {
        [MenuItem("Assets/Create/Custom Rule Tile Script", false, 89)]
        private static void CreateCustomRuleTile()
        {
            CreateScriptAsset("Assets/Tilemap/Tiles/Rule Tile/ScriptTemplates/NewCustomRuleTile.cs.txt", "NewCustomRuleTile.cs");
        }

        private static void CreateScriptAsset(string templatePath, string destName)
        {
            typeof(ProjectWindowUtil)
                .GetMethod("CreateScriptAsset", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                .Invoke(null, new object[] { templatePath, destName });
        }
    }
}