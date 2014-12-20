using System.IO;
using System.Reflection;

namespace EditorListScrolling
{
	public static class Constants
	{

		public static readonly int baseEditorCategoryXOffset = 0;
		public static readonly int baseEditorPartListXOffset = 34;
		public static readonly int baseEditorPartListWidth = 263;
		public static readonly int advancedEditorCategoryXOffset = 34;
		public static readonly int advancedEditorPartListXOffset = 68;
		public static readonly int advancedEditorPartListWidth = 297;
		public static readonly int editorPartListTopOffset = 25;
		public static readonly int editorPartListBottomOffset = 56;
		public static readonly string lockKey = "EditorListScrolling_Lock";
		public static readonly string runtimeDirectory = Assembly.GetExecutingAssembly().Location.Replace(new FileInfo(Assembly.GetExecutingAssembly().Location).Name, "");
		public static readonly string xmlFilePath = "/PluginData/EditorListScrolling/";
		public static readonly string configFileName = "config.xml";

		public static readonly PartCategories[] editorCategories = { PartCategories.Pods, PartCategories.Propulsion, PartCategories.Engine, PartCategories.Control, PartCategories.Structural, PartCategories.Aero, PartCategories.Utility, PartCategories.Science };

	}
}
