using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UIFramework.Interfaces;
using UIFramework.UIElements.Base;

namespace UIFramework.UIElements
{
    public class UIHTMLViewer : UIElement, IAttachableContext, ITranslatable
    {
        private readonly TranslationBinding _translationBinding = new TranslationBinding();
        private IUIContext _context;
        private readonly string _srcPath;
        private string _htmlTemplate;
        private HashSet<string> _translationKeys;

        public UIHTMLViewer(string srcPath)
        {
            if (string.IsNullOrWhiteSpace(srcPath))
                throw new ArgumentException("HTML source path cannot be null or empty", nameof(srcPath));

            _srcPath = srcPath;
            LoadHtmlTemplate();
            RegisterTranslationKeys();
            SetHtmlContent(_htmlTemplate);
        }

        #region Props

        private string _htmlContent;
        [JsonIgnore]
        public string HtmlContent
        {
            get => _htmlContent;
            set => throw new InvalidOperationException("HtmlContent property is read-only. HTML content is set automatically from the source file.");
        }

        #endregion

        #region HTML Loading & Translation Key Extraction

        /// <summary>
        /// Loads the HTML template from file and caches it.
        /// </summary>
        private void LoadHtmlTemplate()
        {
            var doc = new HtmlDocument();
            doc.Load(_srcPath);
            _htmlTemplate = doc.DocumentNode.OuterHtml;
        }

        /// <summary>
        /// Extracts and registers all data-language keys for translation.
        /// </summary>
        private void RegisterTranslationKeys()
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(_htmlTemplate);

            var nodesWithDataLanguage = doc.DocumentNode.SelectNodes("//*[@data-language]");
            
            _translationKeys = nodesWithDataLanguage != null
                ? new HashSet<string>(nodesWithDataLanguage
                    .Select(node => node.GetAttributeValue("data-language", string.Empty))
                    .Where(key => !string.IsNullOrWhiteSpace(key)))
                : new HashSet<string>();

            // Register each key with the translation binding
            foreach (var key in _translationKeys)
            {
                _translationBinding.SetKeysAndTryResolveTranslations(key, key);
            }
        }

        #endregion

        #region Translation

        /// <summary>
        /// Translates the HTML content by replacing data-language keys with their resolved translations.
        /// </summary>
        private string TranslateHtml(string htmlContent)
        {
            if (string.IsNullOrWhiteSpace(htmlContent))
                return htmlContent;

            var doc = new HtmlDocument();
            doc.LoadHtml(htmlContent);

            var nodesWithDataLanguage = doc.DocumentNode.SelectNodes("//*[@data-language]");
            
            if (nodesWithDataLanguage == null)
                return htmlContent;

            var translations = _translationBinding.ResolveTranslations();

            foreach (var node in nodesWithDataLanguage)
            {
                var key = node.GetAttributeValue("data-language", string.Empty);
                
                if (string.IsNullOrWhiteSpace(key))
                    continue;

                // Use translated text if available, otherwise keep the key as fallback
                node.InnerHtml = translations.TryGetValue(key, out var translatedText) 
                    ? translatedText 
                    : key;
            }

            return doc.DocumentNode.OuterHtml;
        }

        /// <summary>
        /// Sets the HTML content (internal use only).
        /// </summary>
        private void SetHtmlContent(string htmlContent)
        {
            _htmlContent = htmlContent;
            byte[] bytes = Encoding.UTF8.GetBytes(_htmlContent);
            // Convert to Base64
            string base64String = Convert.ToBase64String(bytes);
            Props["htmlContent"] = base64String;
        }

        #endregion

        #region IAttachableContext & ITranslatable

        /// <summary>
        /// Attaches the UI context and triggers translation resolution.
        /// </summary>
        public new void AttachContext(IUIContext uIContext)
        {
            _context = uIContext ?? throw new ArgumentNullException(nameof(uIContext));
            _translationBinding.AttachTranslator(_context.Translator);
            ResolveTranslationBindings();
        }

        /// <summary>
        /// Resolves all translation bindings and updates the HTML content.
        /// </summary>
        public new void ResolveTranslationBindings()
        {
            var translatedHtml = TranslateHtml(_htmlTemplate);
            SetHtmlContent(translatedHtml);
        }

        #endregion
    }
}
