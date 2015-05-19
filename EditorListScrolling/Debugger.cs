using UnityEngine;


namespace EditorListScrolling
{
	public static class Debugger
	{


		/// <summary>
		/// Prints the wanted text into the Logfile
		/// </summary>
		/// <param name="debugText"></param>
		public static void log(string debugText, bool advancedDebug)
		{
			if (advancedDebug)
			{
				Debug.Log(string.Format("{0} - {1}", Constants.debugPrefix, debugText));
			}
		}


	}
}
