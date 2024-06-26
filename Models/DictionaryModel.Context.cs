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
    
        public virtual ObjectResult<SearchWords_Result> SearchWords(string word, string lang, string lang_trans)
        {
            var wordParameter = word != null ?
                new ObjectParameter("word", word) :
                new ObjectParameter("word", typeof(string));
    
            var langParameter = lang != null ?
                new ObjectParameter("lang", lang) :
                new ObjectParameter("lang", typeof(string));
    
            var lang_transParameter = lang_trans != null ?
                new ObjectParameter("lang_trans", lang_trans) :
                new ObjectParameter("lang_trans", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SearchWords_Result>("SearchWords", wordParameter, langParameter, lang_transParameter);
        }
    
        public virtual ObjectResult<GetWordInfoById_Result> GetWordInfoById(Nullable<int> wordId)
        {
            var wordIdParameter = wordId.HasValue ?
                new ObjectParameter("WordId", wordId) :
                new ObjectParameter("WordId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetWordInfoById_Result>("GetWordInfoById", wordIdParameter);
        }
    
        public virtual int InsertOrUpdateHistorySearch(Nullable<int> id_user, Nullable<int> id_word, Nullable<System.DateTime> dDatetime)
        {
            var id_userParameter = id_user.HasValue ?
                new ObjectParameter("Id_user", id_user) :
                new ObjectParameter("Id_user", typeof(int));
    
            var id_wordParameter = id_word.HasValue ?
                new ObjectParameter("Id_word", id_word) :
                new ObjectParameter("Id_word", typeof(int));
    
            var dDatetimeParameter = dDatetime.HasValue ?
                new ObjectParameter("dDatetime", dDatetime) :
                new ObjectParameter("dDatetime", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("InsertOrUpdateHistorySearch", id_userParameter, id_wordParameter, dDatetimeParameter);
        }
    
        public virtual ObjectResult<GetWordsByUserId_Result> GetWordsByUserId(Nullable<int> id_user)
        {
            var id_userParameter = id_user.HasValue ?
                new ObjectParameter("Id_user", id_user) :
                new ObjectParameter("Id_user", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetWordsByUserId_Result>("GetWordsByUserId", id_userParameter);
        }
    
        public virtual int sp_DeleteHistorySearch(Nullable<int> userId, Nullable<int> wordId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            var wordIdParameter = wordId.HasValue ?
                new ObjectParameter("WordId", wordId) :
                new ObjectParameter("WordId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_DeleteHistorySearch", userIdParameter, wordIdParameter);
        }
    }
}
