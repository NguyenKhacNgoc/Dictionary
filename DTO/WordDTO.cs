using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dictionary.DTO
{
    public class WordDTO
    {
        public int Id { get; set; }
        public int Id_Language { get; set; }
        public int Id_Language_trans { get; set; }
        public int Id_wordtype { get; set; }
        public int Id_user { get; set; }
        public string sWord { get; set; }
        public string sExample { get; set; }
        public string sDefinition { get; set; }
    }
}