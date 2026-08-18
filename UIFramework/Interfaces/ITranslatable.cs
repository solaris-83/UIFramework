using ScriptLibraries.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace UIFramework.Interfaces
{
    public interface ITranslatable
    {
        void ResolveTranslationBindings();
    }

    public sealed class TranslationBinding
    {
        private object _lock = new object();
        private ITranslationService _translator;

        public void AttachTranslator(ITranslationService translator)
        {
            _translator = translator;
        }

        private readonly Dictionary<string, string[]> translations = new Dictionary<string, string[]>();

        public string ResolveTranslations(string propertyName)
        {
            if (translations.Count == 0 || _translator == null)
                return string.Empty;

            var sb = new StringBuilder();
            string[] kvp = translations[propertyName];
            //lock (_lock)
            //{
                foreach (string pairValue in kvp)
                {
                    sb.Append(_translator.Translate(pairValue));
                    sb.Append(" ");
                }
            //}
            return sb.ToString().Trim();
        }

        public Dictionary<string, string> ResolveTranslations()
        {
            if (translations.Count == 0 || _translator == null)
                return null;

            Dictionary<string, string> resolvedTranslations = new Dictionary<string, string>();
            foreach (var item in translations)
            {
                var tr = _translator.Translate(item.Key);
                resolvedTranslations.Add(item.Key, tr);
            }
            return resolvedTranslations;
        }

        public string SetKeysAndTryResolveTranslations(string propertyName, string key)
        {
            translations[propertyName] = key.Split(new string[] { "#<br/>#" }, StringSplitOptions.None);
            return ResolveTranslations(propertyName);
        }
    }
}
