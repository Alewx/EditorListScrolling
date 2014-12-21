using UnityEngine;

namespace EditorListScrolling
{
	[KSPAddon(KSPAddon.Startup.EditorAny, false)]
	public class EditorListScrolling : MonoBehaviour
	{
		private static PanelToScroll currentScrollPanel;
		private static Vector2 _currentMousePos;
		private static bool _invertMouseWheel = false;
		private static float _mouseWheelSensitivity = 0.1f;
		private static float _summedMouseScroll = 0;
		private static int _currentCategoryIndex = 0;
		private EditorListScrollingConfiguration config;

		private static Rect[] _editorScrollRectSimple =
		{
			new Rect(Constants.simpleEditorCategoryX,
				Constants.editorTopOffset,
				Constants.simpleEditorCategoryWidth,
				Screen.height - Constants.editorTopOffset - Constants.editorBottomOffset),
			new Rect(Constants.simpleEditorPartsX,
				Constants.editorTopOffset,
				Constants.simpleEditorPartsWidth,
				Screen.height - Constants.editorTopOffset - Constants.editorBottomOffset)
		};

		private static Rect[] _editorScrollRectAdvanced = 
		{
			new Rect(Constants.advancedEditorFilterX,
				Constants.editorTopOffset,
				Constants.advancedEditorFilterWidth,
				Screen.height - Constants.editorTopOffset - Constants.editorBottomOffset),
			new Rect(Constants.advancedEditorCategoryX,
				Constants.editorTopOffset,
				Constants.advancedEditorCategoryWidth,
				Screen.height - Constants.editorTopOffset - Constants.editorBottomOffset),
			new Rect(Constants.advancedEditorPartsX,
				Constants.editorTopOffset,
				Constants.advancedEditorPartsWidth,
				Screen.height - Constants.editorTopOffset - Constants.editorBottomOffset)
		};

		private enum PanelToScroll
		{
			None,
			Filter,
			Category,
			Parts
		}

		public string VERSION
		{
			get { return Constants.version; }
		}

		public string MOD
		{
			get { return Constants.modname; }
		}


		/// <summary>
		/// initial mehtod that is called once the plugin is loaded
		/// </summary>
		private void Awake()
		{
			if (System.IO.File.Exists(string.Concat(Constants.runtimeDirectory, Constants.xmlFilePath, Constants.configFileName)))
			{
				config = EditorListScrollingConfigurationManager.LoadConfig();
			}
			else
			{
				EditorListScrollingConfigurationManager.SaveConfig(config);
			}
		}


		/// <summary>
		/// changes the sekected Category (currently broken and selecting tabs is somehow currently not possible?)
		/// </summary>
		/// <param name="step"></param>
		private static void updateCategory(int step)
		{
			int newIndex = _currentCategoryIndex + step;
			if (newIndex < 0)
			{
				newIndex = Constants.editorCategories.Length - 1;
			}
			if (newIndex > Constants.editorCategories.Length - 1)
			{
				newIndex = 0;
			}
			_currentCategoryIndex = newIndex;
			EditorPartList.Instance.SelectTab(Constants.editorCategories[_currentCategoryIndex]);
		}


		/// <summary>
		/// checks for a change in the editormode and changes the partlist rect properly
		/// </summary>
		private static void updateEditorScrolling()
		{
			if (EditorLogic.fetch.editorScreen == EditorScreen.Parts)
			{
				if (EditorPanels.Instance.IsMouseOver())
				{
					_currentMousePos = Mouse.screenPos;
					switch (EditorLogic.Mode)
					{
						case EditorLogic.EditorModes.SIMPLE:
							{
								if (_editorScrollRectSimple[0].Contains(_currentMousePos))
								{
									currentScrollPanel = PanelToScroll.Category;
								}
								else if (_editorScrollRectSimple[1].Contains(_currentMousePos))
								{
									currentScrollPanel = PanelToScroll.Parts;
								}
								else
								{
									currentScrollPanel = PanelToScroll.None;
								}
								updatePartList();
							}
							break;
						case EditorLogic.EditorModes.ADVANCED:
							{
								if (_editorScrollRectAdvanced[0].Contains(_currentMousePos))
								{
									currentScrollPanel = PanelToScroll.Filter;
								}
								if (_editorScrollRectAdvanced[1].Contains(_currentMousePos))
								{
									currentScrollPanel = PanelToScroll.Category;
								}
								else if (_editorScrollRectAdvanced[2].Contains(_currentMousePos))
								{
									currentScrollPanel = PanelToScroll.Parts;
								}
								else
								{
									currentScrollPanel = PanelToScroll.None;
								}
								updatePartList();
							}
							break;
					}
				}
			}
		}


		/// <summary>
		/// the main update method where everything happens
		/// </summary>
		private void Update()
		{
			updateEditorScrolling();
		}


		/// <summary>
		/// analysis the mousescrolling and sets the pages
		/// </summary>
		/// <param name="enabled"></param>
		private static void updatePartList()
		{
			if (currentScrollPanel != PanelToScroll.None)
			{
				_summedMouseScroll += Input.GetAxis("Mouse ScrollWheel");
				if (_invertMouseWheel ? _summedMouseScroll < (1 * (-_mouseWheelSensitivity)) : _summedMouseScroll > (1 * _mouseWheelSensitivity))
				{
					switch (currentScrollPanel)
					{
						case PanelToScroll.Filter:
							{
								//needs new stuff
							}
							break;
						case PanelToScroll.Category:
							{
								updateCategory(-1);
							}
							break;
						case PanelToScroll.Parts:
							{
								EditorPartList.Instance.PrevPage();
							}
							break;
					}
					_summedMouseScroll = 0;
				}
				if (_invertMouseWheel ? _summedMouseScroll > (1 * _mouseWheelSensitivity) : _summedMouseScroll < (1 * (-_mouseWheelSensitivity)))
				{
					switch (currentScrollPanel)
					{
						case PanelToScroll.Filter:
							{
							}
							break;
						case PanelToScroll.Category:
							{
								updateCategory(1);
							}
							break;
						case PanelToScroll.Parts:
							{
								EditorPartList.Instance.NextPage();
							}
							break;
					}
					_summedMouseScroll = 0;
				}
			}
			else
			{
				_summedMouseScroll = 0;
			}
		}
	}
}
