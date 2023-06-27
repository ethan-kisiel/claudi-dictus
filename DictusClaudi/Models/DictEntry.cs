using System;
namespace DictusClaudi.Models
{
	public class DictEntry
	{
		public int Id { get; set; }
		public string? WordStem { get; set; }
		public string? WordTranslation { get; set; }

		public DictEntry()
		{

		}
	}
}

