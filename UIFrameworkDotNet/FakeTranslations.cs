using ScriptLibraries.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace UIFrameworkDotNet
{
    // === ATTENZIONE: TODO non portare in BCA
    internal class FakeTranslationService : ITranslationService
    {
        public ITranslations SharedTranslations => new FakeTranslations(); 

        public ITranslations CurrentTranslations => new FakeTranslations();

        public ITranslations ResultsTranslations => new FakeTranslations();
    }


    public class FakeTranslations : ITranslations
    {
        public Dictionary<string, string> Translations { get; }

        public FakeTranslations()
        {
            Translations = new Dictionary<string, string>
            {
                { FakeStrings.PAR1, "Paragraph #1." },
                { FakeStrings.PAR2, "Paragraph #2." },
                { FakeStrings.ITEM_BULLETED_1, "#1 bulleted item" },
                { FakeStrings.ITEM_BULLETED_2, "#2 bulleted item" },
                { FakeStrings.ITEM_1, "Item 1" },
                { FakeStrings.ITEM_2, "Item 2" },
                { FakeStrings.ITEM_3, "Item 3" },
                { FakeStrings.PAR1_UPDATED, "Paragraph #1 - UPDATED." }
            };
        }
        public List<string> Keys => new List<string>();

        public string GetId(string idStrApp)
        {
            return idStrApp;
        }

        public string GetIntl(string idStrApp)
        {
            return idStrApp;
        }

        public string GetIntlOrDefault(string idStrApp)
        {
            return idStrApp;
        }

        public string GetLocal(string idStrApp)
        {
            return idStrApp;
        }

        public string GetLocalOrDefault(string idStrApp)
        {
            Translations.TryGetValue(idStrApp, out string translatedString);
            return translatedString?? idStrApp;
        }
    }

    public class FakeStrings
    {
        public static string PAR1 = "PAR_1";
        public static string PAR1_UPDATED = "PAR1_UPDATED";
        public static string PAR2 = "PAR_2";
        public static string ITEM_BULLETED_1 = "BULLETED_ITEM_1";
        public static string ITEM_BULLETED_2 = "BULLETED_ITEM_2";
        public static string ITEM_1 = "ITEM_1";
        public static string ITEM_2 = "ITEM_2";
        public static string ITEM_3 = "ITEM_3";
    }
}
