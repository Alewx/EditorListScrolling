using UnityEngine;

namespace EditorListScrolling
{
	[KSPAddon(KSPAddon.Startup.EditorAny, false)]
	public class EditorListScrolling : MonoBehaviour
	{
		public static EditorListScrolling Instance { get; private set; }
		private static EnumCollection.PanelToScroll _currentScrollPanel;
		private static Vector2 _currentMousePos;
		private static bool _invertMouseWheel = Constants.defaultInvertMouseWheel;
		private static float _mouseWheelSensitivity = Constants.defaultMouseWheelSensitivity;
		private static float _summedMouseScroll = 0;
		private static int _currentCategoryIndex = 0;
		private static EditorListScrollingConfiguration _config;
		private static EditorLogic.EditorModes _editorMode;
		private static PartCategorizer.Category _filterByFunction;

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

		public string VERSION
		{
			get { return Constants.version; }
		}

		public string MOD
		{
			get { return Constants.modname; }
		}

		public EnumCollection.PanelToScroll activeScrollPanel
		{
			get { return _currentScrollPanel; }
		}


		/// <summary>
		/// initial mehtod that is called once the plugin is loaded
		/// </summary>
		private void Awake()
		{
			Instance = this;
			if (System.IO.File.Exists(string.Concat(Constants.runtimeDirectory, Constants.xmlFilePath, Constants.configFileName)))
			{
				_config = EditorListScrollingConfigurationManager.LoadConfig();
			}
			else
			{
				if (_config == null)
				{
					_config = new EditorListScrollingConfiguration();
					_config.invertMouseWheel = Constants.defaultInvertMouseWheel;
					_config.mouseWheelSensitivity = Constants.defaultMouseWheelSensitivity;
				}
				EditorListScrollingConfigurationManager.SaveConfig(_config);
			}
			_filterByFunction = PartCategorizer.Instance.filters.Find(filter => filter.button.categoryName == "Filter by Function");
		}


		/// <summary>
		/// the main update method where everything happens
		/// </summary>
		private void Update()
		{
			_editorMode = EditorLogic.Mode;
			ManageEditorScrollingSection();
			setEditorMode();
		}


		/// <summary>
		/// sets the Editor to the Mode that was used at the Frame begin.
		/// </summary>
		private void setEditorMode()
		{
			switch (_editorMode)
			{
				case EditorLogic.EditorModes.ADVANCED:
					{
						PartCategorizer.Instance.SetAdvancedMode();
					}
					break;
				case EditorLogic.EditorModes.SIMPLE:
					{
						PartCategorizer.Instance.SetSimpleMode();
					}
					break;
			}
		}


		/// <summary>
		/// checks for a change in the editormode and changes the partlist rect properly
		/// </summary>
		private static void ManageEditorScrollingSection()
		{
			if (EditorLogic.fetch.editorScreen == EditorScreen.Parts)
			{
				if (EditorPanels.Instance.IsMouseOver())
				{
					_currentMousePos = Mouse.screenPos;
					var mouseDirection = getScrollDirection();
					if (mouseDirection != EnumCollection.ScrollDirection.NONE)
					{
						switch (EditorLogic.Mode)
						{
							case EditorLogic.EditorModes.SIMPLE:
								{
									if (_editorScrollRectSimple[0].Contains(_currentMousePos))
									{
										_currentScrollPanel = EnumCollection.PanelToScroll.CATEGORY;
										updateCategories(mouseDirection); //broken
									}
									else if (_editorScrollRectSimple[1].Contains(_currentMousePos))
									{
										_currentScrollPanel = EnumCollection.PanelToScroll.PARTS;
										updatePartList(mouseDirection);
									}
									else
									{
										_currentScrollPanel = EnumCollection.PanelToScroll.NONE;
									}
								}
								break;
							case EditorLogic.EditorModes.ADVANCED:
								{
									if (_editorScrollRectAdvanced[0].Contains(_currentMousePos))
									{
										_currentScrollPanel = EnumCollection.PanelToScroll.FILTER;
										UpdateFilters(mouseDirection); //works almost
									}
									else if (_editorScrollRectAdvanced[1].Contains(_currentMousePos))
									{
										_currentScrollPanel = EnumCollection.PanelToScroll.CATEGORY;
										updateCategories(mouseDirection); //broken
									}
									else if (_editorScrollRectAdvanced[2].Contains(_currentMousePos))
									{
										_currentScrollPanel = EnumCollection.PanelToScroll.PARTS;
										updatePartList(mouseDirection);
									}
									else
									{
										_currentScrollPanel = EnumCollection.PanelToScroll.NONE;
									}
								}
								break;
						}
					}
				}
				else
				{
					if (_currentScrollPanel != EnumCollection.PanelToScroll.NONE)
					{
						_currentScrollPanel = EnumCollection.PanelToScroll.NONE;
					}
				}
			}
		}


		/// <summary>
		/// provides the direction that is beeing scrolled based on an enum
		/// </summary>
		/// <returns></returns>
		private static EnumCollection.ScrollDirection getScrollDirection()
		{
			_summedMouseScroll += Input.GetAxis("Mouse ScrollWheel");
			if (_invertMouseWheel ? _summedMouseScroll < (1 * (-_mouseWheelSensitivity)) : _summedMouseScroll > (1 * _mouseWheelSensitivity))
			{
				_summedMouseScroll = 0;
				return EnumCollection.ScrollDirection.NEGATIVE;
			}
			if (_invertMouseWheel ? _summedMouseScroll > (1 * _mouseWheelSensitivity) : _summedMouseScroll < (1 * (-_mouseWheelSensitivity)))
			{
				_summedMouseScroll = 0;
				return EnumCollection.ScrollDirection.POSTITIVE;
			}
			return EnumCollection.ScrollDirection.NONE;
		}


		/// <summary>
		/// analysis the mousescrolling and sets the pages
		/// </summary>
		/// <param name="enabled"></param>
		private static void updatePartList(EnumCollection.ScrollDirection direction)
		{
			Debugger.log("updatePartList");
			if (_currentScrollPanel != EnumCollection.PanelToScroll.NONE)
			{
				switch (direction)
				{
					case EnumCollection.ScrollDirection.POSTITIVE:
						{
							EditorPartList.Instance.NextPage();
						}
						break;
					case EnumCollection.ScrollDirection.NEGATIVE:
						{
							EditorPartList.Instance.PrevPage();
						}
						break;
				}
			}
		}


		/// <summary>
		/// changes the filter based on scroll direction
		/// </summary>
		private static void UpdateFilters(EnumCollection.ScrollDirection direction)
		{
			Debugger.log("UpdateFilters");
			foreach (PartCategorizer.Category filter in PartCategorizer.Instance.filters)
			{
				Debugger.log(" ! " + filter.button.categoryName+" - "+filter.button.activeButton.State);
			}

			if (_currentScrollPanel != EnumCollection.PanelToScroll.NONE)
			{
				switch (direction)
				{
					case EnumCollection.ScrollDirection.POSTITIVE:
						{
							_currentCategoryIndex++;
							_currentCategoryIndex = Helpers.LoopIndex(_currentCategoryIndex, 0, (PartCategorizer.Instance.filters.Count - 1));
							setPartFilter(_currentCategoryIndex);
						}
						break;
					case EnumCollection.ScrollDirection.NEGATIVE:
						{
							_currentCategoryIndex--;
							_currentCategoryIndex = Helpers.LoopIndex(_currentCategoryIndex, 0, (PartCategorizer.Instance.filters.Count - 1));
							setPartFilter(_currentCategoryIndex);
						}
						break;
				}
			}
		}


		/// <summary>
		/// marks the filter as set based on the provided index
		/// </summary>
		/// <param name="index"></param>
		private static void setPartFilter(int index)
		{
			foreach (PartCategorizer.Category filter in PartCategorizer.Instance.filters)
			{
				filter.button.activeButton.SetFalse(filter.button.activeButton, RUIToggleButtonTyped.ClickType.LEFT);
			}
			PartCategorizer.Instance.filters[_currentCategoryIndex].button.activeButton.SetTrue(PartCategorizer.Instance.filters[_currentCategoryIndex].button.activeButton, RUIToggleButtonTyped.ClickType.LEFT);
		}


		/// <summary>
		/// changes the sekected Category (currently broken and selecting tabs is somehow currently not possible?)
		/// </summary>
		/// <param name="step"></param>
		private static void updateCategories(EnumCollection.ScrollDirection direction)
		{
			Debugger.log("updateCategories");
			if (_currentScrollPanel != EnumCollection.PanelToScroll.NONE)
			{
				_filterByFunction = PartCategorizer.Instance.filters.Find(filter => filter.button.categoryName == "Filter by Function");
				if (_filterByFunction.button.activeButton.State == RUIToggleButtonTyped.ButtonState.TRUE)
				{
					switch (direction)
					{
						case EnumCollection.ScrollDirection.POSTITIVE:
							{
								_currentCategoryIndex = Helpers.LimitIndex(_currentCategoryIndex++, 0, (Constants.editorCategories.Length - 1));
								setPartCategory(_currentCategoryIndex);
							}
							break;
						case EnumCollection.ScrollDirection.NEGATIVE:
							{
								_currentCategoryIndex = Helpers.LimitIndex(_currentCategoryIndex--, 0, (Constants.editorCategories.Length - 1));
								setPartCategory(_currentCategoryIndex);
							}
							break;
					}
				}
			}
		}


		/// <summary>
		/// sets the Panel
		/// </summary>
		/// <param name="index"></param>
		private static void setPartCategory(int index)
		{
			Debugger.log("setPartCategory");
			switch (Constants.editorCategories[index])
			{
				case PartCategories.Pods:
					{
						PartCategorizer.SetPanel_FunctionPods();
					}
					break;
				case PartCategories.FuelTank:
					{
						PartCategorizer.SetPanel_FunctionFuelTank();
					}
					break;
				case PartCategories.Engine:
					{
						PartCategorizer.SetPanel_FunctionEngine();
					}
					break;
				case PartCategories.Control:
					{
						PartCategorizer.SetPanel_FunctionControl();
					}
					break;
				case PartCategories.Structural:
					{
						PartCategorizer.SetPanel_FunctionStructural();
					}
					break;
				case PartCategories.Aero:
					{
						PartCategorizer.SetPanel_FunctionAero();
					}
					break;
				case PartCategories.Utility:
					{
						PartCategorizer.SetPanel_FunctionUtility();
					}
					break;
				case PartCategories.Science:
					{
						PartCategorizer.SetPanel_FunctionScience();
					}
					break;
			}
		}


	}
}
