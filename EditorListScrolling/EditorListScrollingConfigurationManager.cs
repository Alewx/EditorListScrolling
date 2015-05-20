using System;
using System.IO;
using System.Xml.Serialization;

namespace EditorListScrolling
{
	static class EditorListScrollingConfigurationManager
	{

		private static string _configFile = string.Concat(Constants.runtimeDirectory,"/", Constants.configFileName);


		/// <summary>
		/// catches unknown nodes
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void serializer_UnknownNode
		(object sender, XmlNodeEventArgs e)
		{
			Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
		}


		/// <summary>
		/// catches unknown attribute nodes
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void serializer_UnknownAttribute
		(object sender, XmlAttributeEventArgs e)
		{
			System.Xml.XmlAttribute attr = e.Attr;
			Console.WriteLine("Unknown attribute " +
			attr.Name + "='" + attr.Value + "'");
		}


		/// <summary>
		/// loads the config and then puts them into the dll
		/// </summary>
		public static EditorListScrollingConfiguration LoadConfig()
		{
			EditorListScrollingConfiguration loadedConfig = new EditorListScrollingConfiguration();
			FileStream FileStream;
			if (System.IO.File.Exists(_configFile))
			{
				XmlSerializer configSerializer = new XmlSerializer(typeof(EditorListScrollingConfiguration));
				configSerializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
				configSerializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);
				FileStream = new FileStream(_configFile, FileMode.Open);
				loadedConfig = (EditorListScrollingConfiguration)configSerializer.Deserialize(FileStream);
				FileStream.Close();
			}
			if (loadedConfig.mouseWheelSensitivity < 0.1f)
			{
				loadedConfig.mouseWheelSensitivity = 0.1f;
			}
			if (loadedConfig.mouseWheelSensitivity > 2)
			{
				loadedConfig.mouseWheelSensitivity = 2;
			}
			Debugger.log("config loaded "+ loadedConfig.invertMouseWheel + " - " + loadedConfig.mouseWheelSensitivity + " - " + loadedConfig.advancedDebugging, true);
			return loadedConfig;
		}


		/// <summary>
		/// sets the values from the dll and saves it in the config
		/// </summary>
		public static void SaveConfig(EditorListScrollingConfiguration configToSave)
		{
			EditorListScrollingConfiguration config = (EditorListScrollingConfiguration)configToSave.clone();
			TextWriter fileStreamWriter;
			XmlSerializer configSerializer = new XmlSerializer(typeof(EditorListScrollingConfiguration));
			fileStreamWriter = new StreamWriter(_configFile);
			configSerializer.Serialize(fileStreamWriter, config);
			fileStreamWriter.Close();
			Debugger.log("config saved", true);
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
