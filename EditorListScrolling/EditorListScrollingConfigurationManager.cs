namespace EditorListScrolling
{
	static class EditorListScrollingConfigurationManager
	{

		/// <summary>
		/// loads the config and then puts them into the dll
		/// </summary>
		public static EditorListScrollingConfiguration LoadConfig()
		{
			EditorListScrollingConfiguration loadedConfig = new EditorListScrollingConfiguration();
			KSP.IO.PluginConfiguration configToLoad = KSP.IO.PluginConfiguration.CreateForType<EditorListScrollingConfiguration>();
			configToLoad.load();
			loadedConfig.invertMouseWheel = configToLoad.GetValue(Constants.configInvertMouseWheelIdentifier, Constants.defaultInvertMouseWheel);
			loadedConfig.mouseWheelSensitivity = configToLoad.GetValue(Constants.configMouseWheelSensitivityIdentifier, Constants.defaultMouseWheelSensitivity);
			loadedConfig.advancedDebugging = configToLoad.GetValue(Constants.configAdvancedDebuggingIdentifier, Constants.defaultAdvancedDebugging);
			if (loadedConfig.mouseWheelSensitivity < 0.1f)
			{
				loadedConfig.mouseWheelSensitivity = 0.1f;
			}
			if (loadedConfig.mouseWheelSensitivity > 1)
			{
				loadedConfig.mouseWheelSensitivity = 1;
			}
			Debugger.log(loadedConfig.invertMouseWheel + " - " + loadedConfig.mouseWheelSensitivity + " - " + loadedConfig.advancedDebugging, true);
			return loadedConfig;
		}


		/// <summary>
		/// sets the values from the dll and saves it in the config
		/// </summary>
		public static void SaveConfig(EditorListScrollingConfiguration config)
		{
			KSP.IO.PluginConfiguration configToSave = KSP.IO.PluginConfiguration.CreateForType<EditorListScrollingConfiguration>();
			configToSave.SetValue(Constants.configInvertMouseWheelIdentifier, config.invertMouseWheel);
			configToSave.SetValue(Constants.configMouseWheelSensitivityIdentifier, config.mouseWheelSensitivity);
			configToSave.SetValue(Constants.configAdvancedDebuggingIdentifier, config.advancedDebugging);
			configToSave.save();
		}


		/// <summary>
		/// generates a fresh config with default values and returns it
		/// </summary>
		/// <returns></returns>
		public static EditorListScrollingConfiguration generateDefaultConfig()
		{
			EditorListScrollingConfiguration generatedDefaultConfig = new EditorListScrollingConfiguration();
			generatedDefaultConfig.invertMouseWheel = Constants.defaultInvertMouseWheel;
			generatedDefaultConfig.mouseWheelSensitivity = Constants.defaultMouseWheelSensitivity;
			generatedDefaultConfig.advancedDebugging = Constants.defaultAdvancedDebugging;
			return generatedDefaultConfig;
		}

	}
}
