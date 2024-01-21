using Microsoft.Net.Http.Headers;
using System;
namespace DictusClaudi.Models
{
	public class CPartOfSpeech
	{
		const string Noun = "N";
		const string Verb = "V";
		const string Adjective = "ADJ";
		const string Adverb = "ADV";


		public CPartOfSpeech()
		{

		}

	
	}

	public class DictEntry
	{
		public int Id { get; set; }

		/// <summary>
		/// Integers specific to certain types
		/// </summary>
        public int? Declension { get; set; }
        public int? Variant { get; set; }


		/// <summary>
		/// Generic Members
		/// </summary>
        public string? WordStem { get; set; }
		public string? WordTranslation { get; set; }
		public string? PartOfSpeech { get; set; }

		public char? Age { get; set; }
		public char? Area { get; set; }
		public char? Geo { get; set; }
		public char? Frequency { get; set; }
		public char? Source { get; set; }

		/// <summary>
		/// Strings Specific to certain Types
		/// </summary>
		public string? Connotation { get; set; } // specific to adverbs and adjectives
		public string? Form { get; set; }
        public char? Gender { get; set; }

		public DictEntry()
		{

		}
        
		public string DispGender
		{
			get { return this.DisplayGender();  }
		}
		public string DisplayPartOfSpeech
		{
			get { return this.PartOfSpeech != null ? this.DisplayPOS(this.PartOfSpeech) : ""; }
		}

		public string DispAge
		{
			get { return this.DisplayAge();  }
		}

		public string AgeDetails
		{
			get { return this.DisplayAge(true); }
		}

		public string DispArea
		{
			get { return this.DisplayArea(); }
		}

		public string DispGeo
		{
			get { return this.DisplayGeo(); }
		}

		public string DispFrequency
		{
			get { return this.DisplayFreq(); }
		}

		public string FrequencyDetails
		{
			get { return this.DisplayFreq(true); }
		}


		public string[] AllStems
		{
			get { return this.WordStem.Split(","); }
		}


		private string DisplayGender()
		{
			switch (this.Gender)
			{
				case 'M':
					return "Masculine";
				case 'F':
					return "Feminine";
				case 'N':
					return "Neuter";
				case 'C':
					return "Common (Masculine or Feminine)";
				default:
					return "";
			}
		}

        private string DisplayPOS(string typeCode)
        {

            switch (typeCode)
            {
                case "N":
                    return "Noun";

                case "V":
                    return "Verb";

                case "ADJ":
                    return "Adjective";

                case "ADV":
                    return "Adverb";

				case "PRON":
					return "Pronoun";
				
				case "NUM":
					return "Number";
				
				case "INTERJ":
					return "Interjection";

				case "PREP":
					return "Preposition";

                default:
                    return typeCode;
            }
        }
        

		private string DisplayAge(bool isDetailed=false)
		{
	/*
	type AGE_TYPE is (
	X,   --              --  In use throughout the ages/unknown -- the default
	A,   --  archaic     --  Very early forms, obsolete by classical times
	B,   --  early       --  Early Latin, pre-classical, used for effect/poetry
	C,   --  classical   --  Limited to classical (~150 BC - 200 AD)
	D,   --  late        --  Late, post-classical (3rd-5th centuries)
	E,   --  later       --  Latin not in use in Classical times (6-10) Christian
	F,   --  medieval    --  Medieval (11th-15th centuries)
	G,   --  scholar     --  Latin post 15th - Scholarly/Scientific   (16-18)
	H    --  modern      --  Coined recently, words for new things (19-20)
									);
	*/
			if (!isDetailed)
			{
				switch (this.Age)
				{
					case 'X':
						return "Unknown";
					case 'A':
						return "Archaic";
					case 'B':
						return "Early";
					case 'C':
						return "Classical";
					case 'D':
						return "Late";
					case 'E':
						return "Later";
					case 'F':
						return "Medieval";
					case 'G':
						return "Scholar";
					case 'H':
						return "Modern";

					default:
						return "";
				}
			}
			else
			{
				switch (this.Age)
				{
                    case 'X':
                        return "In use throughout the ages/unknown";
                    case 'A':
                        return "Very early forms, obsolete by classical times";
                    case 'B':
                        return "Early Latin, pre-classical, used for effect/poetry";
                    case 'C':
                        return "Limited to classical (~150 BC - 200 AD)";
                    case 'D':
                        return "Late, post-classical (3rd-5th centuries)";
                    case 'E':
                        return "Latin not in use in Classical times (6-10) Christian";
                    case 'F':
                        return "Medieval (11th-15th centuries)";
                    case 'G':
                        return "Latin post 15th - Scholarly/Scientific   (16-18)";
                    case 'H':
                        return "Coined recently, words for new things (19-20)";

                    default:
						return "";
				}
			}
		}

		private string DisplayArea()
		{
/*
type AREA_TYPE is (
X,      --  All or none
A,      --  Agriculture, Flora, Fauna, Land, Equipment, Rural
B,      --  Biological, Medical, Body Parts
D,      --  Drama, Music, Theater, Art, Painting, Sculpture
E,      --  Ecclesiastic, Biblical, Religious
G,      --  Grammar, Rhetoric, Logic, Literature, Schools
L,      --  Legal, Government, Tax, Financial, Political, Titles
P,      --  Poetic
S,      --  Science, Philosophy, Mathematics, Units/Measures
T,      --  Technical, Architecture, Topography, Surveying
W,      --  War, Military, Naval, Ships, Armor
Y       --  Mythology
);
*/
			switch (this.Area)
			{
				case 'X':
					return "All or none";
				case 'A':
					return "Agriculture, Flora, Fauna, Land, Equipment, Rural";
				case 'B':
					return "Biological, Medical, Body Parts";
				case 'D':
					return "Drama, Music, Theater, Art, Painting, Sculpture";
				case 'E':
					return "Ecclesiastic, Biblical, Religious";
				case 'G':
					return "Grammar, Rhetoric, Logic, Literature, Schools";
				case 'L':
					return "Legal, Government, Tax, Financial, Political, Titles";
				case 'P':
					return "Poetic";
				case 'S':
					return "Science, Philosophy, Mathematics, Units/Measures";
				case 'T':
					return "Technical, Architecture, Topography, Surveying";
				case 'W':
					return "War, Military, Naval, Ships, Armor";
				case 'Y':
					return "Mythology";

                default:
					return "";
			}
		}

		private string DisplayGeo()
		{
/*
type GEO_TYPE is (
X,      --  All or none
A,      --  Africa
B,      --  Britain
C,      --  China
D,      --  Scandinavia
E,      --  Egypt
F,      --  France, Gaul
G,      --  Germany
H,      --  Greece
I,      --  Italy, Rome
J,      --  India
K,      --  Balkans
N,      --  Netherlands
P,      --  Persia
Q,      --  Near East
R,      --  Russia
S,      --  Spain, Iberia
U       --  Eastern Europe
);
*/
			switch (this.Geo)
			{
				case 'X':
					return "All or none";
				case 'A':
					return "Africa";
				case 'B':
					return "Britain";
				case 'C':
					return "China";
				case 'D':
					return "Scandinavia";
				case 'E':
					return "Egypt";
				case 'F':
					return "France, Gaul";
                case 'G':
                    return "Germany";
                case 'H':
                    return "Greece";
                case 'I':
                    return "Italy, Rome";
                case 'J':
                    return "India";
                case 'K':
                    return "Balkans";
                case 'N':
                    return "Netherlands";
                case 'P':
                    return "Persia";
                case 'Q':
                    return "Near East";
                case 'R':
                    return "Russia";
                case 'S':
                    return "Spain, Iberia";
                case 'U':
                    return "Eastern Europe";

                default:
					return "";
			}
		}

		private string DisplayFreq(bool isDetailed=false)
		{
			/*
					type FREQUENCY_TYPE is (     --  For dictionary entries
			X,    --              --  Unknown or unspecified
			A,    --  very freq   --  Very frequent, in all Elementary Latin books, top 1000+ words
			B,    --  frequent    --  Frequent, next 2000+ words
			C,    --  common      --  For Dictionary, in top 10,000 words
			D,    --  lesser      --  For Dictionary, in top 20,000 words
			E,    --  uncommon    --  2 or 3 citations
			F,    --  very rare   --  Having only single citation in OLD or L+S
			I,    --  inscription --  Only citation is inscription
			M,    --  graffiti    --  Presently not much used
			N     --  Pliny       --  Things that appear only in Pliny Natural History
						);
			*/
			if (!isDetailed)
			{
				switch (this.Frequency)
				{
					case 'X':
						return "Unknown or unspecified";
					case 'A':
						return "very frequent";
					case 'B':
						return "frequent";
					case 'C':
						return "common";
					case 'D':
						return "lesser";
					case 'E':
						return "uncommon";
					case 'F':
						return "very rare";
					case 'I':
						return "inscription";
					case 'M':
						return "graffiti";
					case 'N':
						return "Pliny";

					default:
						return "";
				}
			}
			else
			{
                switch (this.Frequency)
                {
                    case 'X':
                        return "Unknown or unspecified";
                    case 'A':
                        return "Very frequent, in all Elementary Latin books, top 1000+ words";
                    case 'B':
                        return "Frequent, next 2000+ words";
                    case 'C':
                        return "For Dictionary, in top 10,000 words";
                    case 'D':
                        return "For Dictionary, in top 20,000 words";
                    case 'E':
                        return "2 or 3 citations";
                    case 'F':
                        return "Having only single citation in OLD or L+S";
                    case 'I':
                        return "Only citation is inscription";
                    case 'M':
                        return "Presently not much used";
                    case 'N':
                        return "Things that appear only in Pliny Natural History";

                    default:
                        return "";
                }
            }
		}

		private string DisplaySource(char source)
		{
/*
type SOURCE_TYPE is (
X,      --  General or unknown or too common to say
A,
B,      --  C.H.Beeson, A Primer of Medieval Latin, 1925 (Bee)
C,      --  Charles Beard, Cassell's Latin Dictionary 1892 (CAS)
D,      --  J.N.Adams, Latin Sexual Vocabulary, 1982 (Sex)
E,      --  L.F.Stelten, Dictionary of Eccles. Latin, 1995 (Ecc)
F,      --  Roy J. Deferrari, Dictionary of St. Thomas Aquinas, 1960 (DeF)
G,      --  Gildersleeve + Lodge, Latin Grammar 1895 (G+L)
H,      --  Collatinus Dictionary by Yves Ouvrard
I,      --  Leverett, F.P., Lexicon of the Latin Language, Boston 1845
J,
K,      --  Calepinus Novus, modern Latin, by Guy Licoppe (Cal)
L,      --  Lewis, C.S., Elementary Latin Dictionary 1891
M,      --  Latham, Revised Medieval Word List, 1980
N,      --  Lynn Nelson, Wordlist
O,      --  Oxford Latin Dictionary, 1982 (OLD)
P,      --  Souter, A Glossary of Later Latin to 600 A.D., Oxford 1949
Q,      --  Other, cited or unspecified dictionaries
R,      --  Plater & White, A Grammar of the Vulgate, Oxford 1926
S,      --  Lewis and Short, A Latin Dictionary, 1879 (L+S)
T,      --  Found in a translation  --  no dictionary reference
U,      --  Du Cange
V,      --  Vademecum in opus Saxonis - Franz Blatt (Saxo)
W,      --  My personal guess
Y,      --  Temp special code
Z       --  Sent by user --  no dictionary reference
--  Mostly John White of Blitz Latin

--  Consulted but used only indirectly
--  Liddell + Scott Greek-English Lexicon

--  Consulted but used only occasionally, separately referenced
--  Allen + Greenough, New Latin Grammar, 1888 (A+G)
--  Harrington/Pucci/Elliott, Medieval Latin 2nd Ed 1997 (Harr)
--  C.C./C.L. Scanlon Latin Grammar/Second Latin, TAN 1976 (SCANLON)
--  W. M. Lindsay, Short Historical Latin Grammar, 1895 (Lindsay)
        );
*/
			return "";
		}
    }
}

