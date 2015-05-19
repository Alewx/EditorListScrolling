using UnityEngine;

namespace EditorListScrolling
{
	//[KSPAddon(KSPAddon.Startup.EditorAny, false)]
	class OnScreenDebug : MonoBehaviour
	{
		private void Update()
		{
			ScreenMessages.PostScreenMessage("OnScreenDebug\nMode = " + EditorLogic.Mode.ToString() + " Mouseover = " + EditorPanels.Instance.IsMouseOver().ToString() + "\nMouse Position = " + Mouse.screenPos.ToString() + "\nHovered Panel = " + EditorListScrolling.Instance.activeScrollPanel.ToString(), Time.deltaTime, ScreenMessageStyle.UPPER_CENTER);
		}
	}
}
