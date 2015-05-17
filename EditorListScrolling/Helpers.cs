using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EditorListScrolling
{
	class Helpers
	{

		/// <summary>
		/// limits the integer to the provided limits
		/// </summary>
		/// <param name="index"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public static int LimitIndex(int index, int min, int max )
		{
			if(index < min)
			{
				index = min;
			}
			if(index > max)
			{
				index = max;
			}
			return index;
		}

	}
}
