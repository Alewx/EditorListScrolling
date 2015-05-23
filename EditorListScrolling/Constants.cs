using System.IO;
using System.Reflection;

namespace EditorListScrolling
{
	static class Constants
	{
		public static readonly string version = "1.2.0";
		public static readonly string modname = "EditorListScrolling";

		public static readonly string debugPrefix = "[EditorListScrolling]";

		public static readonly bool defaultInvertMouseWheel = false;
		public static readonly float defaultMouseWheelSensitivity = 1.0f;
		public static readonly bool defaultAdvancedDebugging = false;

		public const string descriptionInvertMouseWheel = "bool, invertes the mousewheel direction";
		public const string descriptionMouseWheelSensitivity = "float, ranges from 0.1 to 2, defines the sensitivity of the mousewheel";
		public const string descriptionAdvancedDebugging = "bool, enables/disables the debug logging";

		public static readonly int editorTopOffset = 26;
		public static readonly int editorBottomOffset = 73;

		public static readonly int simpleEditorCategoryX = 0;
		public static readonly int simpleEditorCategoryWidth = 34;
		public static readonly int simpleEditorPartsX = simpleEditorCategoryWidth;
		public static readonly int simpleEditorPartsWidth = 230;

		public static readonly int advancedEditorFilterX = 0;
		public static readonly int advancedEditorFilterWidth = 34;
		public static readonly int advancedEditorCategoryX = advancedEditorFilterWidth;
		public static readonly int advancedEditorCategoryWidth = 34;
		public static readonly int advancedEditorPartsX = advancedEditorFilterWidth + advancedEditorFilterWidth;
		public static readonly int advancedEditorPartsWidth = simpleEditorPartsWidth;

		public static readonly string lockKey = "EditorListScrolling_Lock";
		public static readonly string runtimeDirectory = Assembly.GetExecutingAssembly().Location.Replace(new FileInfo(Assembly.GetExecutingAssembly().Location).Name, "");
		public static readonly string xmlFilePath = "PluginData/EditorListScrolling/";
		public static readonly string configFileName = "config.xml";
		public static readonly PartCategories[] editorCategories = { PartCategories.Pods, PartCategories.FuelTank, PartCategories.Engine, PartCategories.Control, PartCategories.Structural, PartCategories.Aero, PartCategories.Utility, PartCategories.Science };

	}
}
