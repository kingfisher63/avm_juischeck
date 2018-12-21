/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) 2018 Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System.Xml.Serialization;

namespace JuisCheck
{
	public enum DeviceKind
	{
		[XmlEnum(Name = "Unknown")]
		Unknown,
		[XmlEnum(Name = "DECT")]
		DECT,
		[XmlEnum(Name = "JUIS")]
		JUIS
	}
}
