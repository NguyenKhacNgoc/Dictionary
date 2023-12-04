using Dictionary.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace Dictionary.Controllers
{
    public class HomeController : Controller
    {
        DictionaryEntities dictionaryEntities = new DictionaryEntities();
        public ActionResult Index()
        {

            var list_Language = dictionaryEntities.tblLanguages.ToList();
            return View(list_Language);

        }

        public ActionResult Search_result(string text, string lang, string lang_tran)
        {
            string[] lang_Array = { "English", "VietNamese"};

            if(lang_Array.Contains(lang) && lang_Array.Contains(lang_tran))
            {
                int type = 0;

                if (lang.Equals("English") && lang_tran.Equals("VietNamese"))
                {
                    type = 1;
                }
                else if (lang.Equals("VietNamese") && lang_tran.Equals("English"))
                {
                    type = 2;
                }
                else if(lang.Equals("English") && lang_tran.Equals("English"))
                {
                    type = 3;
                }
                else
                {
                    type = 2;
                }


                ViewData["type"] = type;
                ViewData["text"] = text;
            }
           

            using (var context = new DictionaryEntities())
            {
                var result = context.SearchWords(text, lang, lang_tran)
                                  .Select(s => new SearchWords_Result
                                  {
                                      sWord = s.sWord,
                                      sWordtype = s.sWordtype,
                                      sExample = s.sExample,
                                      sDefinition = s.sDefinition,
                                      Id = s.Id
                                  })
                                  .ToList();

                return View(result);
            }




        }

        public ActionResult Search_result_detail(string text, string lang, string lang_tran)
        {

            var result = dictionaryEntities.SearchWords(text, lang, lang_tran)
                    .Select(s => new SearchWords_Result
                    {
                        Id = s.Id,
                        sWord = s.sWord,
                        sWordtype = s.sWordtype,
                        sExample = s.sExample,
                        sDefinition = s.sDefinition
                    })
                    .ToList();

            return View(result);
        }
        /*
        public ActionResult Item_history(string idList)
        {
            List<int> ids = JsonConvert.DeserializeObject<List<int>>(idList);

            ids.Add(6);
            ids.Add(7);
            ids.Add(12);
            using (var dictionaryEntities = new DictionaryEntities())
            {
                var words = dictionaryEntities.tblWords
                    .Where(w => ids.Contains(w.Id))
                    .ToList();

                return View(words);
            }
        }*/
        

    }
}