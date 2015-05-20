using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EditorListScrolling
{
	public static class EnumCollection
	{
		public enum PanelToScroll
		{
			PARTS,
			CATEGORY,
			FILTER,
			NONE
		}

		public enum ScrollDirection
		{
			NONE,
			POSTITIVE,
			NEGATIVE
		}


	}
}
