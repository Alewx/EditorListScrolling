using System.Collections.Generic;
using UnityEngine;

namespace EditorListScrolling
{
	[KSPAddon(KSPAddon.Startup.EditorAny, false)]
	public class EditorListScrolling : MonoBehaviour
	{
		public static EditorListScrolling Instance { get; private set; }
		private static EnumCollection.PanelToScroll _currentScrollPanel;
		private static Vector2 _currentMousePos;
		private static float _summedMouseScroll = 0;
		private static int _categoryIndex = 0;
		private static int _filterIndex = 0;
		private static EditorListScrollingConfiguration _config;

		private static Rect[] _editorScrollRectSimple =
		{
			new Rect(Constants.simpleEditorPartsX,
				Constants.editorTopOffset,
				Constants.simpleEditorPartsWidth,
				Screen.height - Constants.editorTopOffset - Constants.editorBottomOffset),
			new Rect(Constants.simpleEditorCategoryX,
				Constants.editorTopOffset,
				Constants.simpleEditorCategoryWidth,
				Screen.height - Constants.editorTopOffset - Constants.editorBottomOffset)
		};

		private static Rect[] _editorScrollRectAdvanced = 
		{
			new Rect(Constants.advancedEditorPartsX,
				Constants.editorTopOffset,
				Constants.advancedEditorPartsWidth,
				Screen.height - Constants.editorTopOffset - Constants.editorBottomOffset),
			new Rect(Constants.advancedEditorCategoryX,
				Constants.editorTopOffset,
				Constants.advancedEditorCategoryWidth,
				Screen.height - Constants.editorTopOffset - Constants.editorBottomOffset),
			new Rect(Constants.advancedEditorFilterX,
				Constants.editorTopOffset,
				Constants.advancedEditorFilterWidth,
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
			Debugger.log("Awake", true);
			Instance = this;
			if (System.IO.File.Exists(string.Concat(Constants.runtimeDirectory, "/", Constants.configFileName)))
			{
				_config = EditorListScrollingConfigurationManager.LoadConfig();
			}
			else
			{
				if (_config == null)
				{
					_config = EditorListScrollingConfigurationManager.generateDefaultConfig();
					EditorListScrollingConfigurationManager.SaveConfig(_config);
				}
			}
		}


		/// <summary>
		/// the main update method where everything happens
		/// </summary>
		private void Update()
		{
			ManageEditorScrollingSection();
			if (Input.GetKeyUp(KeyCode.Return))
			{
				pureDebug();
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
						_currentScrollPanel = EditorLogic.Mode == EditorLogic.EditorModes.SIMPLE ? (EnumCollection.PanelToScroll)getHoveredScrollPanel(_editorScrollRectSimple, _currentMousePos) : (EnumCollection.PanelToScroll)getHoveredScrollPanel(_editorScrollRectAdvanced, _currentMousePos);
						if (_currentScrollPanel != EnumCollection.PanelToScroll.NONE)
						{
							updateFiltering(mouseDirection);
						}
					}
				}
			}
		}


		/// <summary>
		/// returns the index of the enum fitting to the currently hovered Scrollpanel
		/// </summary>
		/// <param name="panels"></param>
		/// <param name="mousePosition"></param>
		/// <returns></returns>
		public static int getHoveredScrollPanel(Rect[] panels, Vector2 mousePosition)
		{
			for (int i = 0; i < panels.Length; i++)
			{
				if (panels[i].Contains(mousePosition))
				{
					return i;
				}
			}
			return (int)EnumCollection.PanelToScroll.NONE;
		}


		/// <summary>
		/// provides the direction that is beeing scrolled based on an enum
		/// </summary>
		/// <returns></returns>
		private static EnumCollection.ScrollDirection getScrollDirection()
		{
			_summedMouseScroll += Input.GetAxis("Mouse ScrollWheel") * 10 * _config.mouseWheelSensitivity;
			Debugger.log("getScrollDirection  _summedMouseScroll = " + _summedMouseScroll, true);
			if (_config.invertMouseWheel ? _summedMouseScroll <= -1 : _summedMouseScroll >= 1)
			{
				_summedMouseScroll = 0;
				return EnumCollection.ScrollDirection.NEGATIVE;
			}
			if (_config.invertMouseWheel ? _summedMouseScroll >= 1 : _summedMouseScroll <= -1)
			{
				_summedMouseScroll = 0;
				return EnumCollection.ScrollDirection.POSTITIVE;
			}
			return EnumCollection.ScrollDirection.NONE;
		}


		/// <summary>
		/// provides the Index of the active entry in the list
		/// </summary>
		/// <param name="activeList"></param>
		/// <returns></returns>
		private static int getActiveListIndex(List<PartCategorizer.Category> activeList)
		{
			Debugger.log("getActiveListIndex", _config.advancedDebugging);
			foreach (PartCategorizer.Category entry in activeList)
			{
				if (entry.button.activeButton.State == RUIToggleButtonTyped.ButtonState.TRUE)
				{
					return activeList.FindIndex(activeEntry => activeEntry.button.categoryName == entry.button.categoryName);
				}
			}
			return -1;
		}


		/// <summary>
		/// changes the sekected Category (currently broken and selecting tabs is somehow currently not possible?)
		/// </summary>
		/// <param name="step"></param>
		private static void updateFiltering(EnumCollection.ScrollDirection direction)
		{
			Debugger.log("updateFiltering", _config.advancedDebugging);
			if (_currentScrollPanel == EnumCollection.PanelToScroll.CATEGORY)
			{
				changeFilter(direction, PartCategorizer.Instance.filters[getActiveListIndex(PartCategorizer.Instance.filters)].subcategories, _categoryIndex);
			}
			else if (_currentScrollPanel == EnumCollection.PanelToScroll.FILTER)
			{
				changeFilter(direction, PartCategorizer.Instance.filters, _filterIndex);
			}
			else if (_currentScrollPanel == EnumCollection.PanelToScroll.PARTS)
			{
				if (direction == EnumCollection.ScrollDirection.POSTITIVE)
				{
					EditorPartList.Instance.NextPage();
				}
				else
				{
					EditorPartList.Instance.PrevPage();
				}
			}
		}


		/// <summary>
		/// selects the filter based on the scrolling direction
		/// </summary>
		/// <param name="direction"></param>
		/// <param name="filteringList"></param>
		/// <param name="listIndex"></param>
		private static void changeFilter(EnumCollection.ScrollDirection direction, List<PartCategorizer.Category> filteringList, int listIndex)
		{
			Debugger.log("changeFilter", _config.advancedDebugging);
			var index = getActiveListIndex(filteringList);
			listIndex = direction == EnumCollection.ScrollDirection.POSTITIVE ? index + 1 : index - 1;
			listIndex = Helpers.LoopIndex(listIndex, 0, (filteringList.Count - 1));
			filteringList[listIndex].button.activeButton.SetTrue(filteringList[listIndex].button.activeButton, RUIToggleButtonTyped.ClickType.LEFT);
		}


		/// <summary>
		/// as the name says this only throws out the state and name for every filter and category at once
		/// </summary>
		private static void pureDebug()
		{
			Debugger.log("pureDebug", true);
			foreach (PartCategorizer.Category filter in PartCategorizer.Instance.filters)
			{
				foreach (PartCategorizer.Category category in filter.subcategories)
				{
					Debugger.log(filter.button.categoryName + " - " + filter.button.activeButton.State+" | "+category.button.categoryName+" - "+category.button.activeButton.State, true);
				}
			}
		}

	}
}
