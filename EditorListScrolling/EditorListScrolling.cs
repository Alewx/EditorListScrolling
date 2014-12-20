using UnityEngine;

namespace EditorListScrolling
{
	[KSPAddon(KSPAddon.Startup.EditorAny, false)]
	public class EditorListScrolling : MonoBehaviour
	{

		private static EditorLogic.EditorModes _activeEditorMode;
		private static Vector2 _currentMousePos;
		private static bool _invertMouseWheel = false;
		private static float _mouseWheelSensitivity = 0.1f;
		private static float _summedMouseScroll = 0;
		private static int _currentCategoryIndex = 0;
		private static Rect _editorPartListScrollRect = new Rect(Constants.baseEditorPartListXOffset,
			Constants.editorPartListTopOffset,
			Constants.baseEditorPartListWidth - Constants.baseEditorPartListXOffset,
			Screen.height - Constants.editorPartListTopOffset - Constants.editorPartListBottomOffset);
		private static Rect _editorCategoryScrollRect = new Rect(Constants.baseEditorCategoryXOffset,
			Constants.editorPartListTopOffset,
			Constants.baseEditorPartListXOffset,
			Screen.height - Constants.editorPartListTopOffset - Constants.editorPartListBottomOffset);

		private static void updateCategory(int step)
		{
			int newIndex = _currentCategoryIndex + step;
			if (newIndex < 0)
			{
				newIndex = Constants.editorCategories.Length-1;
			}
			if (newIndex > Constants.editorCategories.Length-1)
			{
				newIndex = 0;
			}
			_currentCategoryIndex = newIndex;
			EditorPartList.Instance.SelectTab(Constants.editorCategories[_currentCategoryIndex]);
		}


		/// <summary>
		/// loads the config and then puts them into the dll
		/// </summary>
		private static void LoadConfig()
		{
			KSP.IO.PluginConfiguration config = KSP.IO.PluginConfiguration.CreateForType<EditorListScrollingConfiguration>();
			config.load();
			_invertMouseWheel = config.GetValue("invertMouseWheel", _invertMouseWheel);
			_mouseWheelSensitivity = config.GetValue("mouseWheelSensitivity", _mouseWheelSensitivity);
			if (_mouseWheelSensitivity < 0.1f)
			{
				_mouseWheelSensitivity = 0.1f;
			}
			if (_mouseWheelSensitivity > 1)
			{
				_mouseWheelSensitivity = 1;
			}
		}


		/// <summary>
		/// sets the values from the dll and saves it in the config
		/// </summary>
		private static void SaveConfig()
		{
			KSP.IO.PluginConfiguration config = KSP.IO.PluginConfiguration.CreateForType<EditorListScrollingConfiguration>();
			config.SetValue("invertMouseWheel", _invertMouseWheel);
			config.SetValue("mouseWheelSensitivity", _mouseWheelSensitivity.ToString());
			config.save();
		}


		/// <summary>
		/// initial mehtod that is called once the plugin is loaded
		/// </summary>
		public void Awake()
		{
			if (System.IO.File.Exists(string.Concat(Constants.runtimeDirectory, Constants.xmlFilePath, Constants.configFileName)))
			{
				LoadConfig();
			}
			else
			{
				SaveConfig();
			}
		}


		/// <summary>
		/// checks for a change in the editormode and changes the partlist rect properly
		/// </summary>
		private static void updateEditorMode()
		{
			if (_activeEditorMode != EditorLogic.Mode)
			{
				switch (EditorLogic.Mode)
				{
					case EditorLogic.EditorModes.SIMPLE:
						{
							_editorPartListScrollRect = new Rect(Constants.baseEditorPartListXOffset,
								Constants.editorPartListTopOffset,
								Constants.baseEditorPartListWidth - Constants.baseEditorPartListXOffset,
								Screen.height - Constants.editorPartListTopOffset - Constants.editorPartListBottomOffset);

							_editorCategoryScrollRect = new Rect(Constants.baseEditorCategoryXOffset,
								Constants.editorPartListTopOffset,
								Constants.baseEditorPartListXOffset,
								Screen.height - Constants.editorPartListTopOffset - Constants.editorPartListBottomOffset);
						}
						break;
					case EditorLogic.EditorModes.ADVANCED:
						{
							_editorPartListScrollRect = new Rect(Constants.advancedEditorPartListXOffset,
								Constants.editorPartListTopOffset,
								Constants.advancedEditorPartListWidth - Constants.advancedEditorPartListXOffset,
								Screen.height - Constants.editorPartListTopOffset - Constants.editorPartListBottomOffset);

							_editorCategoryScrollRect = new Rect(Constants.advancedEditorCategoryXOffset,
								Constants.editorPartListTopOffset,
								Constants.baseEditorPartListXOffset,
								Screen.height - Constants.editorPartListTopOffset - Constants.editorPartListBottomOffset);
						}
						break;
				}
				_activeEditorMode = EditorLogic.Mode;
			}
		}


		/// <summary>
		/// the main update method where everything happens
		/// </summary>
		public void Update()
		{
			_currentMousePos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
			updateEditorMode();
			managePartListMouseScroll(_editorPartListScrollRect.Contains(_currentMousePos), _editorCategoryScrollRect.Contains(_currentMousePos));
			Debug.Log(string.Format("{0} -|- {1} -|- {2} -|- {3} -|- {4}", _currentMousePos, _activeEditorMode, _invertMouseWheel, _editorPartListScrollRect, Constants.editorCategories[_currentCategoryIndex]));
		}


		/// <summary>
		/// analysis the mousescrolling and sets the pages
		/// </summary>
		/// <param name="enabled"></param>
		private static void managePartListMouseScroll(bool PartListScrollingEnabled, bool categoryScrollingEnabled)
		{
			if (PartListScrollingEnabled || categoryScrollingEnabled)
			{
				InputLockManager.SetControlLock(ControlTypes.CAMERACONTROLS, Constants.lockKey);
				_summedMouseScroll += Input.GetAxis("Mouse ScrollWheel");
				if (_invertMouseWheel ? _summedMouseScroll < (1 * (-_mouseWheelSensitivity)) : _summedMouseScroll > (1 * _mouseWheelSensitivity))
				{
					if (PartListScrollingEnabled)
					{
						EditorPartList.Instance.PrevPage();
					}
					else if (categoryScrollingEnabled)
					{
						updateCategory(_invertMouseWheel ? 1 : -1);
					}
					_summedMouseScroll = 0;
				}
				if (_invertMouseWheel ? _summedMouseScroll > (1 * _mouseWheelSensitivity) : _summedMouseScroll < (1 * (-_mouseWheelSensitivity)))
				{
					if (PartListScrollingEnabled)
					{
						EditorPartList.Instance.NextPage();
					}
					else if (categoryScrollingEnabled)
					{
						updateCategory(_invertMouseWheel ? -1 : 1);
					}
					_summedMouseScroll = 0;
				}
			}
			else
			{
				InputLockManager.RemoveControlLock(Constants.lockKey);
				_summedMouseScroll = 0;
			}
		}
	}
}
