using System.IO;
using System.Reflection;

namespace EditorListScrolling
{
	static class Constants
	{
		public static readonly string version = "0.1";
		public static readonly string modname = "EditorListScrolling";

		public static readonly float defaultMouseWheelSensitivity = 0.1f;
		public static readonly bool defaultInvertMouseWheel = false;

		public static readonly int editorTopOffset = 25;
		public static readonly int editorBottomOffset = 56;
		public static readonly int simpleEditorCategoryX = 0;
		public static readonly int simpleEditorCategoryWidth = 34;
		public static readonly int simpleEditorPartsX = simpleEditorCategoryWidth + 1;
		public static readonly int simpleEditorPartsWidth = 265;
		public static readonly int advancedEditorFilterX = 0;
		public static readonly int advancedEditorFilterWidth = 34;
		public static readonly int advancedEditorCategoryX = advancedEditorFilterWidth + 1;
		public static readonly int advancedEditorCategoryWidth = 34;
		public static readonly int advancedEditorPartsX = advancedEditorFilterWidth + 1 + advancedEditorFilterWidth + 1;
		public static readonly int advancedEditorPartsWidth = simpleEditorPartsWidth;

		public static readonly string lockKey = "EditorListScrolling_Lock";
		public static readonly string runtimeDirectory = Assembly.GetExecutingAssembly().Location.Replace(new FileInfo(Assembly.GetExecutingAssembly().Location).Name, "");
		public static readonly string xmlFilePath = "/PluginData/EditorListScrolling/";
		public static readonly string configFileName = "config.xml";
		public static readonly PartCategories[] editorCategories = { PartCategories.Pods, PartCategories.FuelTank, PartCategories.Engine, PartCategories.Control, PartCategories.Structural, PartCategories.Aero, PartCategories.Utility, PartCategories.Science };

	}
}
