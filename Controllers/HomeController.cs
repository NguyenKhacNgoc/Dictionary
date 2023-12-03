using Dictionary.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

        public ActionResult Search_result_detail(int id)
        {

            var result = dictionaryEntities.GetWordInfoById(id)
                    .Select(s => new GetWordInfoById_Result
                    {
                        WordId = s.WordId,
                        sWord = s.sWord,
                        sWordtype = s.sWordtype,
                        sExample = s.sExample,
                        sDefinition = s.sDefinition,
                        sLanguage = s.sLanguage
                    })
                    .ToList();

            return View(result);
        }

        public ActionResult Item_history(string idList)
        {
            List<int> ids = JsonConvert.DeserializeObject<List<int>>(idList);

            //System.Diagnostics.Debug.WriteLine("Values in ids array:");
            //foreach (var id in ids)
            //{
            //    System.Diagnostics.Debug.WriteLine(id);
            //}

            using (var dictionaryEntities = new DictionaryEntities())
            {
                var words = dictionaryEntities.tblWords
                    .Where(w => ids.Contains(w.Id))
                    .ToList();

                words = words.OrderBy(w => ids.IndexOf(w.Id)).ToList();

                return View(words);
            }
        }

        public ActionResult Add_history(int id_user, int id_word, DateTime datetime)
        {
            dictionaryEntities.InsertOrUpdateHistorySearch(id_user, id_word, datetime);
            var word = dictionaryEntities.GetWordsByUserId(id_user)
                .Select(s => new GetWordsByUserId_Result { 
                       WordId= s.WordId,
                       sWord = s.sWord,
                       sWordtype = s.sWordtype,
                       sExample = s.sExample,
                       sDefinition = s.sDefinition,
                       OriginalLanguage = s.OriginalLanguage,
                       Id_Language_trans = s.Id_Language_trans,
                       TranslationLanguage = s.TranslationLanguage,
                       dDatetime = s.dDatetime,
                       HistoryUserId = s.HistoryUserId
                }).ToList();
            return View(word);
        }



        public ActionResult History_return(int id_user)
        {
            var word = dictionaryEntities.GetWordsByUserId(id_user)
                .Select(s => new GetWordsByUserId_Result
                {
                    WordId = s.WordId,
                    sWord = s.sWord,
                    sWordtype = s.sWordtype,
                    sExample = s.sExample,
                    sDefinition = s.sDefinition,
                    OriginalLanguage = s.OriginalLanguage,
                    Id_Language_trans = s.Id_Language_trans,
                    TranslationLanguage = s.TranslationLanguage,
                    dDatetime = s.dDatetime,
                    HistoryUserId = s.HistoryUserId
                }).ToList();
            return View(word);
        }

        public ActionResult Remove_from_history_search(int id_user, int id_word)
        {
            dictionaryEntities.sp_DeleteHistorySearch(id_user, id_word);

            return Json(id_user, JsonRequestBehavior.AllowGet);
        }
    }
}