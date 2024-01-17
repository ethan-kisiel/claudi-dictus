using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DictusClaudi.Data;
using DictusClaudi.Models;
using Newtonsoft.Json;

namespace DictusClaudi.Controllers
{
    public class DictionaryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private Dictionary<char, Dictionary<char, LinkedList<DictEntry>>> _dictionaryHashMap;

        public DictionaryController(ApplicationDbContext context)
        {
            _context = context;
            _dictionaryHashMap = new Dictionary<char, Dictionary<char, LinkedList<DictEntry>>>();

            List<DictEntry> dictEntries = _context.DictEntry.ToList();

            foreach (DictEntry entry in dictEntries)
            {
                LoadDictionaryWord(entry);
            }
        }

        private void LoadDictionaryWord(DictEntry dictEntry)
        {
            string wordStem = dictEntry.WordStem;
            char firstLetter = wordStem[0];
            char secondLetter = wordStem.Length < 2 ? '0' : wordStem[1];

            if (!this._dictionaryHashMap.ContainsKey(firstLetter)) // Brand new category
            {
                this._dictionaryHashMap[firstLetter] = new Dictionary<char, LinkedList<DictEntry>>();
                this._dictionaryHashMap[firstLetter][secondLetter] = new LinkedList<DictEntry>();
                this._dictionaryHashMap[firstLetter][secondLetter].AddFirst(dictEntry);
            }
            else if (!this._dictionaryHashMap[firstLetter].ContainsKey(secondLetter))
            {
                this._dictionaryHashMap[firstLetter][secondLetter] = new LinkedList<DictEntry>();
                this._dictionaryHashMap[firstLetter][secondLetter].AddFirst(dictEntry);
            }
            else
            {
                this._dictionaryHashMap[firstLetter][secondLetter].AddLast(dictEntry);
            }
        }

        // GET: Dictionary
        public async Task<IActionResult> Index(string? searchTerms)
        {
            //IQueryable<DictEntry> dictEntries = _context.DictEntry;
            if (!string.IsNullOrEmpty(searchTerms))
            {
                searchTerms = searchTerms.ToLower();
                char firstLetter = searchTerms[0];
                char secondLetter = searchTerms.Length < 2 ? '0' : searchTerms[1];
                LinkedList<DictEntry>? entries;
                try
                {
                    entries = _dictionaryHashMap[firstLetter][secondLetter];
                }
                catch
                {
                    entries = null;
                }

                List<DictEntry> listEntries = entries == null ? new List<DictEntry>() : entries.ToList<DictEntry>();
                IEnumerable<DictEntry> filteredEntries = listEntries.Where(entry => LatinSearch(entry, searchTerms));
                return View(filteredEntries.ToList());
                //Where(entry => StringComparator.shared.LatinSearch(entry.WordStem, searchTerms));
            }
            else
            {
                return View(new List<DictEntry>());
            }
            return NotFound();
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


        private bool LatinSearch(DictEntry entry, string searchTerms)
        {
            float longestEqualSub = 0.0f; //longest equal substring
            float wordsAverage;

            foreach (string stem in entry.AllStems)
            {
                int longestIndexStart = 0;

                HashSet<char> combinedSet = new HashSet<char>($"{searchTerms}{stem}");
                HashSet<char> searchSet = new HashSet<char>(searchTerms);
                HashSet<char> stemSet = new HashSet<char>(stem);


                wordsAverage = combinedSet.Count / (searchTerms.Length + stem.Length);

                //float score = wordsAverage / termSet.Count;

                if (wordsAverage > 0.2)
                {
                    return true;
                }

                //for (int i = 0; i < searchTerms.Length; i++)
                //{
                //    bool containsSubString = false;
                //    try
                //    {
                //        containsSubString =
                //            stem.ToLower().Contains(searchTerms.ToLower().Substring(longestIndexStart, i+1));
                //    }
                //    catch
                //    {
                //        break;
                //    }
                //    if (containsSubString)
                //    {
                //        // if the stem contains the substring of longestIndex start -> i
                //        longestEqualSub = i+1 - longestIndexStart;
                //    }
                //    else
                //    {
                //        longestIndexStart = i;
                //    }
                //}

                //float matchScore = longestEqualSub / wordsAverage;
                //if (wordsAverage <= 3 && matchScore >= 0.6)
                //{
                //    return true;
                //}
                //if (wordsAverage > 3 && wordsAverage <= 10 && matchScore >= 0.7)
                //{
                //    Console.WriteLine(matchScore);
                //    Console.WriteLine(searchTerms);
                //    Console.WriteLine(stem);
                //    return true;
                //}
                //if (wordsAverage <= 10 && wordsAverage > 6 && matchScore >= 0.45)
                //{
                //    return true;
                //}
                //if (wordsAverage > 10 && matchScore >= 0.35)
                //{
                //    return true;
                //}
            }

            return false;
        }

    }

}
