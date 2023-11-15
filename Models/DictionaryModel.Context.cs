﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dictionary.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class DictionaryEntities : DbContext
    {
        public DictionaryEntities()
            : base("name=DictionaryEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tblHistory_add> tblHistory_add { get; set; }
        public virtual DbSet<tblHistory_search> tblHistory_search { get; set; }
        public virtual DbSet<tblLanguage> tblLanguages { get; set; }
        public virtual DbSet<tblUser> tblUsers { get; set; }
        public virtual DbSet<tblWord> tblWords { get; set; }
        public virtual DbSet<tblWord_type> tblWord_type { get; set; }
    
        public virtual ObjectResult<SearchWords_Result> SearchWords(string p_word, string p_lang, string p_lang_trans)
        {
            var p_wordParameter = p_word != null ?
                new ObjectParameter("p_word", p_word) :
                new ObjectParameter("p_word", typeof(string));
    
            var p_langParameter = p_lang != null ?
                new ObjectParameter("p_lang", p_lang) :
                new ObjectParameter("p_lang", typeof(string));
    
            var p_lang_transParameter = p_lang_trans != null ?
                new ObjectParameter("p_lang_trans", p_lang_trans) :
                new ObjectParameter("p_lang_trans", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SearchWords_Result>("SearchWords", p_wordParameter, p_langParameter, p_lang_transParameter);
        }
    }
}
