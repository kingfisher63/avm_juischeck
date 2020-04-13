/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System.Collections.Generic;

namespace JuisCheck
{
	public class RecentItemsCollection : List<string>
	{
		/**********************/
		/* Class constructors */
		/**********************/

		public RecentItemsCollection()
		{
		}

		public RecentItemsCollection( IEnumerable<string> items ) : base(items)
		{
		}

		/*****************/
		/* Other methods */
		/*****************/

		public List<string> GetMaxItems( int maxCount )
		{
			return GetRange(0, maxCount < Count ? maxCount : Count);
		}

		public void InsertRecent( string item )
		{
			Remove(item);
			Insert(0, item);
		}
	}
}
