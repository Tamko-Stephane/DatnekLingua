using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatnekLingua_API.Data.Models
{
    /// <summary>
    /// user configured language entity
    /// </summary>
    public class UserConfiguredLanguage
    {
        #region Properties

        [Key] public Guid Guid { get; set; }      //guid identifying the language in DB
        public DateTime DateCreated { get; set; }       //date of creation 
        public DateTime LastModifiedDate { get; set; }      //last date modified
        public bool IsApplicationUserLanguage { get; set; }     //indicates if language is user's default language                
        [Required] public Guid LanguageGuid { get; set; }      //reference to the language configured by the user
        [Required] public Guid NiveauParleGuid { get; set; }       //reference to the level of speech        
        [Required] public Guid NiveauEcritGuid { get; set; }       //reference to the level of writing        
        [Required] public Guid NiveauComprehensionGuid { get; set; }       //reference to the level of understanding

        [ForeignKey("LanguageGuid")] public virtual Language LanguageConcerned { get; set; }        //lazy load of instance Language        
        [ForeignKey("NiveauParleGuid")] public virtual NiveauParle NiveauParle { get; set; }        //lazy load of instance niveau parle        
        [ForeignKey("NiveauEcritGuid")] public virtual NiveauEcrit NiveauEcrit { get; set; }        //lazy load of instance niveau ecrit        
        [ForeignKey("NiveauComprehensionGuid")] public virtual NiveauComprehension NiveauComprehension { get; set; }        //lazy load of instance level understanding

        #endregion

    }

    public class Language
    {
        #region Properties

        [Key] public Guid Guid { get; set; }      //guid identifying the language in DB        
        [Required] public string Name { get; set; }        //name of the language        
        [Required] public string Code { get; set; }        //code of the language e.g. fr-FR, en-US, etc.
        public string Description { get; set; }     //language description, regions making use of, stats
        public bool IsCompulsoryToTheApplication { get; set; }      //it has to figure in user configured languages
        public ICollection<UserConfiguredLanguage> UserConfiguredLanguagesConcerned { get; set; }

        #endregion

    }

    /// <summary>
    /// Niveau parle entity
    /// </summary>
    public class NiveauParle
    {
        #region Properties
        /// <summary>
        /// guid identitying niveau parle
        /// </summary>
        [Key] public Guid Guid { get; set; }
        /// <summary>
        /// name of niveau parle
        /// </summary>
        [Required] public string Name { get; set; }
        /// <summary>
        /// languages concerned
        /// </summary>
        public ICollection<UserConfiguredLanguage> LanguagesConcerned { get; set; }
        /// <summary>
        /// Indicate if this is the default value to use
        /// </summary>
        public bool IsDefaultValueInstance { get; set; }
        #endregion
    }

    /// <summary>
    /// Niveau écrit entity
    /// </summary>
    public class NiveauEcrit
    {
        #region Properties
        /// <summary>
        /// guid identitying niveau écrit
        /// </summary>
        [Key] public Guid Guid { get; set; }
        /// <summary>
        /// name of niveau écrit
        /// </summary>
        [Required] public string Name { get; set; }
        /// <summary>
        /// languages concerned
        /// </summary>
        public ICollection<UserConfiguredLanguage> LanguagesConcerned { get; set; }
        /// <summary>
        /// Indicate if this is the default value to use
        /// </summary>
        public bool IsDefaultValueInstance { get; set; }
        #endregion 
    }

    /// <summary>
    /// Niveau compréhension entity
    /// </summary>
    public class NiveauComprehension
    {
        #region Properties
        /// <summary>
        /// guid identitying niveau comprehension
        /// </summary>
        [Key] public Guid Guid { get; set; }
        /// <summary>
        /// name of niveau comprehension
        /// </summary>
        [Required] public string Name { get; set; }
        /// <summary>
        /// languages concerned
        /// </summary>
        public ICollection<UserConfiguredLanguage> LanguagesConcerned { get; set; }
        /// <summary>
        /// indicate if this is the default value to use
        /// </summary>
        public bool IsDefaultValueInstance { get; set; }
        #endregion
    }

}
