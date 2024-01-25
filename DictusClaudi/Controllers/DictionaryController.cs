using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DictusClaudi.Data;
using DictusClaudi.Models;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using Microsoft.CodeAnalysis;

namespace DictusClaudi.Controllers
{
    public class DictionaryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private Dictionary<char, LinkedList<DictEntry>> _dictionaryHashMap;
        private List<DictEntry> _dictionaryList;

        public DictionaryController(ApplicationDbContext context)
        {
            _context = context;
            _dictionaryHashMap = new Dictionary<char, LinkedList<DictEntry>>();

            List<DictEntry> dictEntries = _context.DictEntry.ToList();
            _dictionaryList = dictEntries;

            //Parallel.ForEach(dictEntries, LoadDictionaryWord);

            foreach (DictEntry entry in dictEntries)
            {
                LoadDictionaryWord(entry);
            }
        }

        private void LoadDictionaryWord(DictEntry dictEntry)
        {
            string wordStem = dictEntry.WordStem;
            char firstLetter = char.ToUpper(wordStem[0]);

            if (!this._dictionaryHashMap.ContainsKey(firstLetter)) // Brand new category
            {
                this._dictionaryHashMap[firstLetter] = new LinkedList<DictEntry>();
                this._dictionaryHashMap[firstLetter] = new LinkedList<DictEntry>();
                this._dictionaryHashMap[firstLetter].AddFirst(dictEntry);
            }
            else
            {
                this._dictionaryHashMap[firstLetter].AddLast(dictEntry);
            }
        }

        // GET: Dictionary
        public async Task<IActionResult> Index(string? searchTerms, int? page, string? searchEnglish)
        {
            PaginationResult<DictEntry> paginationObject = new PaginationResult<DictEntry>();
            //IQueryable<DictEntry> dictEntries = _context.DictEntry;
            if (!string.IsNullOrEmpty(searchTerms))
            {
                
                searchTerms = searchTerms.ToLower();
                List<DictEntry> filteredEntries = new List<DictEntry>();

                if (!searchEnglish.IsNullOrEmpty())
                {
                    var englishSearch = Parallel.ForEach(_dictionaryList, (entry) =>
                    {
                        if (EnglishSearchResult(entry, searchTerms))
                        {
                            filteredEntries.Add(entry);
                        }
                    });
                } else
                {
                    var englishSearch = Parallel.ForEach(_dictionaryList, (entry) =>
                    {
                        if (LatinSearch(entry, searchTerms))
                        {
                            filteredEntries.Add(entry);
                        }
                    });
                }

                filteredEntries = filteredEntries.OrderBy(entry => entry.WordStem).ToList();
                paginationObject.LastPageNumber = filteredEntries.Count / PaginationUtility<DictEntry>.PageSize;

                if (paginationObject.LastPageNumber == 0)
                {
                    paginationObject.LastPageNumber = 1;
                }

                paginationObject.MaxTabs = 15;
                paginationObject.PageNumber = page ?? 1;

                ViewData["paginationObject"] = paginationObject;
                ViewData["searchTerms"] = searchTerms.Split(" ");
                ViewData["searchEnglish"] = searchEnglish.IsNullOrEmpty() ? false : true;
                return View(PaginationUtility<DictEntry>.GetPage(page ?? 1, filteredEntries));
            }
            else
            {

                paginationObject.LastPageNumber = 1;

                paginationObject.MaxTabs = 15;
                paginationObject.PageNumber = page ?? 1;

                ViewData["paginationObject"] = paginationObject;
                return View(new List<DictEntry>());
            }
        }

        public async Task<IActionResult> ClassicDictionary(char? selectedLetter, int? page)
        {
            PaginationResult<DictEntry> paginationObject = new PaginationResult<DictEntry>();
            

            try
            {
                char letter;
                if (selectedLetter.HasValue)
                {
                    paginationObject.IsPrimaryParameter = false;
                    letter = (char) selectedLetter;
                }
                else
                {
                    paginationObject.IsPrimaryParameter = true;
                    letter = 'A';
                }

                //var collectEntries = Parallel.ForEach(_dictionaryHashMap[letter].Values, subEntries => { entries.AddRange(subEntries); });
                ViewData["selectedLetter"] = $"{letter}";

                //Parallel.ForEach(_dictionaryHashMap[letter].Values, entries.AddRange);

                DictEntry[] filteredEntries = PaginationUtility<DictEntry>.GetPage(page ?? 1, _dictionaryHashMap[letter].ToList());

                paginationObject.LastPageNumber = _dictionaryHashMap[letter].Count / PaginationUtility<DictEntry>.PageSize;

                if (paginationObject.LastPageNumber == 0)
                {
                    paginationObject.LastPageNumber = 1;
                }

                paginationObject.MaxTabs = 15;
                paginationObject.PageNumber = page ?? 1;

                ViewData["paginationObject"] = paginationObject;

                return View(filteredEntries);

            } catch (Exception e) { return View(_dictionaryHashMap['A']); }
        }
        
        // GET: Dictionary/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DictEntry == null)
            {
                return NotFound();
            }

            var dictEntry = await _context.DictEntry
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dictEntry == null)
            {
                return NotFound();
            }

            return View(dictEntry);
        }


        [HttpPost]
        public async Task<IActionResult> Upload([FromBody] string data)
        {
            Console.WriteLine(data);
            try
            {
                DictEntry? dictEntry = JsonConvert.DeserializeObject<DictEntry>(data);
                if (dictEntry != null)
                {
                    var existingEntry = await _context.DictEntry.FirstOrDefaultAsync(m => m.Id == dictEntry.Id);
                    if (existingEntry != null)
                    {
                        _context.Remove(existingEntry);
                    }

                    _context.Add(dictEntry);
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return await Upload(data);
                    }
                    
                    return Ok();
                }
            }
            catch (System.Text.Json.JsonException)
            {
                Console.WriteLine("Bad Json Serialization");
                return BadRequest();
            }

            Console.WriteLine("Failed to create dict entry");
            return BadRequest();
        }


        private bool DictEntryExists(int id)
        {
            return (_context.DictEntry?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        private bool EnglishSearchResult(DictEntry entry, string searchTerms)
        {
            try
            {
                if (entry.WordTranslation.IsNullOrEmpty())
                {
                    return false;
                }

                string[] searchTermsArray = searchTerms.Split(" ");

                // loop thru each search term and check if it is long enough
                foreach (string term in searchTerms.Split(" "))
                {
                    if (term.Length > entry.WordTranslation?.Length || term.Length < 3)
                    { continue; }

                    string entryTranslation = entry.WordTranslation?.ToLower() ?? "";

                    for (int i = 0; i <= entryTranslation.Length - term.Length; i++)
                    {
                        string currentClip = entryTranslation.Substring(i, term.Length);
                        HashSet<char> combinedSet = new HashSet<char>($"{term}{currentClip}");
                        HashSet<char> searchSet = new HashSet<char>(term);
                        HashSet<char> stemSet = new HashSet<char>(currentClip);


                        float wordsAverage = (float)(combinedSet.Count - (float)(searchSet.Count + stemSet.Count) / 2);

                        if (wordsAverage < 1 && searchSet.Count <= stemSet.Count)
                        {
                            
                            Console.WriteLine($"Current Clip: {currentClip}; Combined Set: {combinedSet.Count}; Search Set: {searchSet.Count}; Stem Set: {stemSet.Count}");

                            int currSearchTermIndex = 0;
                            int currMatches = 0;
                            int maxMatch = 0;

                            char[] searchArray = searchSet.ToArray();
                            char[] wordArray = stemSet.ToArray();
                        
                            for (int j = 0; j < searchArray.Length; j++)
                            {
                                if (searchArray[j] != wordArray[j])
                                {
                                    if (currMatches > maxMatch)
                                    {
                                        maxMatch = currMatches;
                                    }

                                    currMatches = 0;
                                }
                                else
                                {
                                    currMatches++;
                                    currSearchTermIndex++;
                                    if (currMatches > maxMatch)
                                    {
                                        maxMatch = currMatches;
                                    }
                                }
                            }

                            if ((float)(maxMatch / searchSet.Count) > 0.1)
                            {
                                return true;
                            }
                            else
                            {
                                Console.WriteLine($"{(float)(maxMatch / searchSet.Count)}");
                            }
                        }
                    }
                }

                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private bool LatinSearch(DictEntry entry, string searchTerms)
        {
            try
            {
                if (entry.WordTranslation.IsNullOrEmpty())
                {
                    return false;
                }

                string[] searchTermsArray = searchTerms.Split(" ");

                // loop thru each search term and check if it is long enough
                foreach (string term in searchTerms.Split(" "))
                {
                    if (term.Length > entry.WordStem?.Length || term.Length < 3)
                    { continue; }

                    string entryTranslation = entry.WordStem?.ToLower() ?? "";

                    for (int i = 0; i <= entryTranslation.Length - term.Length; i++)
                    {
                        string currentClip = entryTranslation.Substring(i, term.Length);
                        HashSet<char> combinedSet = new HashSet<char>($"{term}{currentClip}");
                        HashSet<char> searchSet = new HashSet<char>(term);
                        HashSet<char> stemSet = new HashSet<char>(currentClip);


                        float wordsAverage = (float)(combinedSet.Count - (float)(searchSet.Count + stemSet.Count) / 2);

                        if (wordsAverage < 2 && searchSet.Count <= stemSet.Count)
                        {
                            
                            Console.WriteLine($"Current Clip: {currentClip}; Combined Set: {combinedSet.Count}; Search Set: {searchSet.Count}; Stem Set: {stemSet.Count}");

                            int currSearchTermIndex = 0;
                            int currMatches = 0;
                            int maxMatch = 0;

                            char[] searchArray = searchSet.ToArray();
                            char[] wordArray = stemSet.ToArray();
                        
                            for (int j = 0; j < searchArray.Length; j++)
                            {
                                if (searchArray[j] != wordArray[j])
                                {
                                    if (currMatches > maxMatch)
                                    {
                                        maxMatch = currMatches;
                                    }

                                    currMatches = 0;
                                }
                                else
                                {
                                    currMatches++;
                                    currSearchTermIndex++;
                                    if (currMatches > maxMatch)
                                    {
                                        maxMatch = currMatches;
                                    }
                                }
                            }

                            if ((float)(maxMatch / searchSet.Count) > 0.05)
                            {
                                return true;
                            }
                            else
                            {
                                Console.WriteLine($"{(float)(maxMatch / searchSet.Count)}");
                            }
                        }
                    }
                }

                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }

}
