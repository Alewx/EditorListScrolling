using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace EditorListScrolling
{
	class EditorListScrollingConfigurationManager
	{

		/// <summary>
		/// loads the config and then puts them into the dll
		/// </summary>
		public static EditorListScrollingConfiguration LoadConfig()
		{
			EditorListScrollingConfiguration loadedConfig = new EditorListScrollingConfiguration();
			KSP.IO.PluginConfiguration configToLoad = KSP.IO.PluginConfiguration.CreateForType<EditorListScrollingConfiguration>();
			configToLoad.load();
			loadedConfig.invertMouseWheel = configToLoad.GetValue("invertMouseWheel", Constants.defaultInvertMouseWheel);
			loadedConfig.mouseWheelSensitivity = configToLoad.GetValue("mouseWheelSensitivity", Constants.defaultMouseWheelSensitivity);
			if (loadedConfig.mouseWheelSensitivity < 0.1f)
			{
				loadedConfig.mouseWheelSensitivity = 0.1f;
			}
			if (loadedConfig.mouseWheelSensitivity > 1)
			{
				loadedConfig.mouseWheelSensitivity = 1;
			}
			return loadedConfig;
		}


		/// <summary>
		/// sets the values from the dll and saves it in the config
		/// </summary>
		public static void SaveConfig(EditorListScrollingConfiguration config)
		{
			KSP.IO.PluginConfiguration configToSave = KSP.IO.PluginConfiguration.CreateForType<EditorListScrollingConfiguration>();
			configToSave.SetValue("invertMouseWheel", config.invertMouseWheel);
			configToSave.SetValue("mouseWheelSensitivity", config.mouseWheelSensitivity.ToString());
			configToSave.save();
		}

	}
}
