using Dictionary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

            using (var context = new DictionaryEntities())
            {
                var result = context.SearchWords(text, lang, lang_tran)
                                  .Select(s => new SearchWords_Result
                                  {
                                      sWord = s.sWord,
                                      sWordtype = s.sWordtype,
                                      sExample = s.sExample,
                                      sDefinition = s.sDefinition
                                  })
                                  .ToList();

                return View(result);
            }




        }

        public ActionResult Search_result_detail(string text, string lang, string lang_tran)
        {

            using (var context = new DictionaryEntities())
            {
                var result = context.SearchWords(text, lang, lang_tran)
                                  .Select(s => new SearchWords_Result
                                  {
                                      sWord = s.sWord,
                                      sWordtype = s.sWordtype,
                                      sExample = s.sExample,
                                      sDefinition = s.sDefinition
                                  })
                                  .ToList();

                return View(result);
            }

        }

    }
}