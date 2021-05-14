/// AUTHOR
/// TAMKO STEPHANE,
/// @contact: https://twitter.com/FlywingsS
using DatnekLingua_API.Data.Models;
using DatnekLingua_API.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DatnekLingua_API.Data
{
    public static class DbSeeder
    {
        public static void Seed(LanguagesDbContext context)
        {
            if (context != null)
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //seed different levels
                        CreateNiveauParle(context);
                        CreateNiveauEcrit(context);
                        CreateNiveauComprehension(context);
                        //context.SaveChanges();

                        //seed languages
                        CreateLanguages(context);
                        //context.SaveChanges();

                        //seed default userconfigured languages
                        CreateDefaultUserConfiguredLanguages(context);
                        //context.SaveChanges();

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private static void CreateNiveauParle(LanguagesDbContext context)
        {
            if (context != null && !context.NiveauxParles.Any())
            {
                //create levels of comprehension
                var base_level = new NiveauParle()
                {
                    Guid = Guid.NewGuid(),
                    Name = "bas",
                    IsDefaultValueInstance = true
                };
                var fluent_level = new NiveauParle()
                {
                    Guid = Guid.NewGuid(),
                    Name = "courant"
                };
                var excelent_level = new NiveauParle()
                {
                    Guid = Guid.NewGuid(),
                    Name = "excellent"
                };

                //add them to table
                context.NiveauxParles.AddRange(new NiveauParle[] { base_level, fluent_level, excelent_level });
                context.SaveChanges();
            }
        }
        private static void CreateNiveauEcrit(LanguagesDbContext context)
        {
            if (context != null && !context.NiveauxEcrits.Any())
            {
                //create levels of comprehension
                var base_level = new NiveauEcrit()
                {
                    Guid = Guid.NewGuid(),
                    Name = "bas",
                    IsDefaultValueInstance = true
                };
                var fluent_level = new NiveauEcrit()
                {
                    Guid = Guid.NewGuid(),
                    Name = "courant"
                };
                var excelent_level = new NiveauEcrit()
                {
                    Guid = Guid.NewGuid(),
                    Name = "excellent"
                };

                //add them to table
                context.NiveauxEcrits.AddRange(new NiveauEcrit[] { base_level, fluent_level, excelent_level });
                context.SaveChanges();
            }
        }
        private static void CreateNiveauComprehension(LanguagesDbContext context)
        {
            if (context != null && !context.NiveauxComprehensions.Any())
            {
                //create levels of comprehension

                var base_level = new NiveauComprehension()
                {
                    Guid = Guid.NewGuid(),
                    Name = "bas",
                    IsDefaultValueInstance = true
                };

                var fluent_level = new NiveauComprehension()
                {
                    Guid = Guid.NewGuid(),
                    Name = "courant"
                };
                var excellent_level = new NiveauComprehension()
                {
                    Guid = Guid.NewGuid(),
                    Name = "excellent"
                };

                //add them to table
                context.NiveauxComprehensions.AddRange(new NiveauComprehension[] { base_level, fluent_level, excellent_level });
                context.SaveChanges();
            }
        }

        private static void CreateLanguages(LanguagesDbContext context)
        {
            if (context != null && !context.Languages.Any())
            {
                var directory = Environment.CurrentDirectory;
                var pathToFile = Path.Combine(directory, @"Helpers\Files\",
                    LanguageRessources.LanguagesListFileName);
                var languagesToSave = Transformer.GetLanguagesFromFile<Language>(pathToFile);
                if (languagesToSave != null && languagesToSave.Count > 0)
                {
                    context.Languages.AddRange(languagesToSave.ToArray());
                    context.SaveChanges();
                }
            }
        }

        private static void CreateDefaultUserConfiguredLanguages(LanguagesDbContext context)
        {
            if (context != null && !context.UserConfiguredLanguages.Any())
            {
                //get the compulsory languages and configure default
                var compulsoryLanguages = context.Languages.Where(l => l.IsCompulsoryToTheApplication).ToList();
                if (compulsoryLanguages != null && compulsoryLanguages.Count > 0)
                {
                    List<UserConfiguredLanguage> defaultConfiguredLanguages = new List<UserConfiguredLanguage>();
                    //get default levels
                    var defaultTalkLevel = context.NiveauxParles.FirstOrDefault(np => np.IsDefaultValueInstance);
                    var defaultWriteLevel = context.NiveauxEcrits.FirstOrDefault(ne => ne.IsDefaultValueInstance);
                    var defaultUnderstandLevel = context.NiveauxComprehensions.FirstOrDefault(nc => nc.IsDefaultValueInstance);
                    if (defaultTalkLevel != null && defaultWriteLevel != null && defaultUnderstandLevel != null)
                    {
                        //configure the user compulsory languages
                        foreach (var compulsoryLanguage in compulsoryLanguages)
                        {
                            if (compulsoryLanguage != null)
                            {
                                defaultConfiguredLanguages.Add(new UserConfiguredLanguage()
                                {
                                    Guid = Guid.NewGuid(),
                                    DateCreated = DateTime.Now,
                                    LastModifiedDate = DateTime.Now,
                                    LanguageGuid = compulsoryLanguage.Guid,
                                    LanguageConcerned = compulsoryLanguage,
                                    NiveauParleGuid = defaultWriteLevel.Guid,
                                    NiveauEcritGuid = defaultWriteLevel.Guid,
                                    NiveauComprehensionGuid = defaultUnderstandLevel.Guid,
                                    NiveauParle = defaultTalkLevel,
                                    NiveauEcrit = defaultWriteLevel,
                                    IsApplicationUserLanguage = compulsoryLanguage.Code.ToLower() == "fr",
                                    NiveauComprehension = defaultUnderstandLevel,
                                });
                            }

                        }
                        context.UserConfiguredLanguages.AddRange(defaultConfiguredLanguages.ToArray());
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
