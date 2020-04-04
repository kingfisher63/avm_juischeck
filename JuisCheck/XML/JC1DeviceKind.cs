/*
 * Program   : JuisCheck for Windows
 * Copyright : Copyright (C) Roger Hünen
 * License   : GNU General Public License version 3 (see LICENSE)
 */

using System.Xml.Serialization;

namespace JuisCheck.XML
{
	public enum JC1DeviceKind
	{
		[XmlEnum(Name = "Unknown")]
		Unknown,
		[XmlEnum(Name = "DECT")]
		DECT,
		[XmlEnum(Name = "JUIS")]
		JUIS
	}
}
