using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UIFrameworkDotNet;
using UIFrameworkDotNet.Helpers;


namespace ConsoleApp
{


    /*
    class Program
    {
        static void Main()
        {
          //  var enumerator = SigmoidGenerator.SigmoidGrowth(50, 50).GetEnumerator();

            var enumerator = SigmoidGenerator.RandomDouble(19.5, 20.5).GetEnumerator();
            while (!(Console.KeyAvailable))
            {
                enumerator.MoveNext();
                Console.WriteLine(enumerator.Current);
                Thread.Sleep(300);
            }
            enumerator.Dispose();
        }
    }
    */

    // ========== ESEMPIO DI UTILIZZO ==========

    /*  public class Program
      {
          public static void Main()
          {
          // Esempio 1: Creazione manuale
          var doc = new Document("My Application");

          var page1 = new Page("page1", "Home Page");
          var section1 = new Section("header", "Header");
          section1.Add(new Label("title", "Benvenuto nella mia applicazione"));
          section1.Add(new Button("btnLogin", "Login"));

          var section2 = new Section("content", "Content");
          section2.Layout = LayoutType.Grid;
          section2.Add(new TextBox("txtUsername", "Username"));
          section2.Add(new TextBox("txtPassword", "Password"));

          page1.AddSection(section1);
          page1.AddSection(section2);
          doc.AddPage(page1);

          // Esempio 2: Utilizzo del Builder Pattern
          var page2 = new PageBuilder("Settings Page")
              .WithSize(1024, 768)
              .AddSection("General", LayoutType.Vertical)
              .AddLabel("Impostazioni Generali")
              .AddTextBox("Nome Utente")
              .AddButton("Salva", (s, e) => Console.WriteLine("Salvato!"))
              .AddSection("Advanced", LayoutType.Horizontal)
              .AddTab(tab => tab
                  .AddTab("Tab 1", t => t
                      .AddLabel("Contenuto Tab 1")
                      .AddButton("Azione 1"))
                  .AddTab("Tab 2", t => t
                      .AddLabel("Contenuto Tab 2")
                      .AddButton("Azione 2")))
              .Build();

          doc.AddPage(page2);

          // Rendering del documento
          Console.WriteLine(doc.Render());
          Console.WriteLine($"\nDocumento contiene {doc.GetPageCount()} pagine");

          // Navigazione
          Console.WriteLine($"\nPagina corrente: {doc.GetCurrentPage()?.Title}");
          doc.NextPage();
          Console.WriteLine($"Dopo NextPage(): {doc.GetCurrentPage()?.Title}");

          // Esempio 2: Creazione manuale con CheckBox
          doc = new Document("My Application");

          page1 = new Page("page1", "Home Page");
          section1 = new Section("header", "Header");
          section1.Add(new ProvaFiltroInteri.Label("title", "Benvenuto nella mia applicazione"));
          section1.Add(new Button("btnLogin", "Login"));

          section2 = new Section("content", "Content");
          section2.Layout = LayoutType.Grid;
          section2.Add(new TextBox("txtUsername", "Username"));
          section2.Add(new TextBox("txtPassword", "Password"));

          // Aggiungo CheckBox singole
          var rememberMe = new CheckBox("cbRemember", "Ricordami");
          rememberMe.CheckedChanged += (s, e) =>
          {
              var cb = s as CheckBox;
              Console.WriteLine($"CheckBox '{cb.Text}' stato: {cb.IsChecked}");
          };
          section2.Add(rememberMe);

          page1.AddSection(section1);
          page1.AddSection(section2);
          doc.AddPage(page1);

          // Esempio 2: Utilizzo del Builder Pattern con CheckBox Group
          page2 = new PageBuilder("Settings Page")
              .WithSize(1024, 768)
              .AddSection("Preferences", LayoutType.Vertical)
              .AddLabel("Seleziona le tue preferenze:")
              .AddCheckBox("Ricevi notifiche email", true)
              .AddCheckBox("Ricevi notifiche push", false)
              .AddCheckBox("Modalità dark mode", true)
              .AddSection("Interests", LayoutType.Vertical)
              .AddLabel("Seleziona i tuoi interessi:")
              .AddCheckBoxGroup("interests", "Sport", "Tecnologia", "Musica", "Cinema", "Viaggi")
              .AddButton("Salva Preferenze", (s, e) =>
              {
                  Console.WriteLine("Preferenze salvate!");
              })
              .Build();

          doc.AddPage(page2);

          // Esempio 3: Gestione CheckBoxGroup
          Console.WriteLine("\n=== Esempio CheckBoxGroup ===");
          var preferencesGroup = new CheckBoxGroup("preferences");

          var cb1 = new CheckBox("pref1", "Opzione 1");
          var cb2 = new CheckBox("pref2", "Opzione 2");
          var cb3 = new CheckBox("pref3", "Opzione 3");

          preferencesGroup.Add(cb1);
          preferencesGroup.Add(cb2);
          preferencesGroup.Add(cb3);

          // Seleziono alcune checkbox
          cb1.SetChecked(true);
          cb3.SetChecked(true);

          Console.WriteLine($"CheckBox selezionate: {preferencesGroup.CountChecked()}");
          Console.WriteLine("Elementi selezionati:");
          foreach (var text in preferencesGroup.GetCheckedTexts())
          {
              Console.WriteLine($"  - {text}");
          }

          Console.WriteLine("\nIDs delle checkbox selezionate:");
          foreach (var id in preferencesGroup.GetCheckedIds())
          {
              Console.WriteLine($"  - {id}");
          }

          // Rendering del documento
          Console.WriteLine("\n=== Rendering Documento ===");
          Console.WriteLine(doc.Render());
          Console.WriteLine($"\nDocumento contiene {doc.GetPageCount()} pagine");

          // Test Toggle
          Console.WriteLine("\n=== Test Toggle ===");
          rememberMe.Toggle();
          Console.WriteLine($"Dopo Toggle: {rememberMe.IsChecked}");
          rememberMe.Toggle();
          Console.WriteLine($"Dopo secondo Toggle: {rememberMe.IsChecked}");

          // Test CheckAll / UncheckAll
          Console.WriteLine("\n=== Test CheckAll/UncheckAll ===");
          preferencesGroup.CheckAll();
          Console.WriteLine($"Dopo CheckAll: {preferencesGroup.CountChecked()} selezionate");
          preferencesGroup.UncheckAll();
          Console.WriteLine($"Dopo UncheckAll: {preferencesGroup.CountChecked()} selezionate");
          }
      }*/

    /* 
    class Program
    {
        static void Main()
        {
            var parser = new ArithmeticParser();

            Console.WriteLine("=== Test con formula applicata a lista di valori ===\n");

            // Formula: (x - 40) / 100
            string formula = "(x - 40) / 100";
            List<double> valori = new List<double> { 40, 50, 60, 140, 240 };

            Console.WriteLine($"Formula: {formula}");
            Console.WriteLine($"Valori:  {string.Join(", ", valori)}\n");

            List<double> risultati = parser.ParseList(formula, "x", valori);

            Console.WriteLine("Risultati:");
            for (int i = 0; i < valori.Count; i++)
            {
                Console.WriteLine($"  x = {valori[i],6:F1}  =>  {risultati[i]:F3}");
            }

            List<double> risultati2 = parser.ParseList(formula, valori);

            Console.WriteLine("Risultati:");
            for (int i = 0; i < valori.Count; i++)
            {
                Console.WriteLine($"  x = {valori[i],6:F1}  =>  {risultati2[i]:F3}");
            }


            Console.WriteLine("\n=== Altri esempi ===\n");

            // Altri esempi di formule
            var esempi = new Dictionary<string, (string formula, List<double> valori)>
            {
                { "Conversione Fahrenheit->Celsius", ("(x - 32) * 5 / 9", new List<double> { 32, 50, 68, 86, 104 }) },
                { "Percentuale", ("x / 100", new List<double> { 10, 25, 50, 75, 100 }) },
                { "Quadrato + 10", ("x * x + 10", new List<double> { 1, 2, 3, 4, 5 }) }
            };

            foreach (var esempio in esempi)
            {
                Console.WriteLine($"{esempio.Key}: {esempio.Value.formula}");
                var ris = parser.ParseList(esempio.Value.formula, "x", esempio.Value.valori);
                for (int i = 0; i < esempio.Value.valori.Count; i++)
                {
                    Console.WriteLine($"  x = {esempio.Value.valori[i],6:F1}  =>  {ris[i]:F3}");
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n=== Test con variabili multiple ===\n");

            // Puoi anche usare più variabili contemporaneamente
            var vars = new Dictionary<string, double>
            {
                { "x", 10 },
                { "y", 20 }
            };

            double risultato = parser.Parse("(x + y) * 2", vars);
            Console.WriteLine($"(x + y) * 2 con x=10, y=20  =>  {risultato}");
        }
    }
    */

    class Program
    {
        // =================== DA FARE DOMANI 
        // 1. (OBSOLETE) Correggere Addbulleteditem, OrderedItem perché devono usare una UIListElement ecc, 
        // 2. (OK) testare Feedback con countdown e progress dentro una pagina con/senza Start automatico.
        // 3. (OK mi sembra, ricontrollare, magari servirsi della dispose?) Bisogna registrarsi agli eventi tick una sola volta per la stessa feedback
        // 4. TODO Implementare dispose (tra cui eventi) 
        // 5. TODO Implementare sequence  ( ci sono rif. a sequence nella app di tutorial e bundle update => si possono dismettere i vecchi metodi)
        // 6. TODO Terminare le page (property del disclaimer es. IsScrolledToBottom)
        // 7. TODO rimuovere Style in favore di un'unica property di nome CssClassName
        // 8. TODO introdurre gli ultimi SetProperty<T> e rimuovere States["..."]/Props["..."] dai CTORs e terminare i Command
        // 9. TODO improvement Dropdown e testarla
        // 10. TODO testare le Textbox (cioè il two-way binding), magari ha senso una pagina con le form

        static void Main()
        {
            var libraryUI = new LibraryUI();

            var page = libraryUI.CreatePage();
            var titolo = page.SetTitle("TITOLO", "information");
            page.Title = "TITOLO2";
            libraryUI.ShowAndWait(page);

            page.Title = "TITOLO3";
            libraryUI.SimulateJsEvent(titolo.Id, "propertyChanged", new Dictionary<string, object> { ["text"] = "__NOTFOUND__" });


            page = libraryUI.CreatePage();
            page.SetTitle("TITOLO", "information");
            var myTab = page.AddTab("tab", 2, 2);
            var mySection111 = libraryUI.CreateSectionDocument();
            // prevedo una section 1x1 visto che non specifico nient'altro
            mySection111.AddParagraph("Sono un paragraph");
            var mySection222 = libraryUI.CreateSectionDocument(); // prevedo una section 1x1 visto che non specifico nient'altro
            mySection222.AddImage("Screenshot.png");
            var mySection333 = libraryUI.CreateSectionDocument(); // prevedo una section 1x1 visto che non specifico nient'altro mySection3.AddChart()
            myTab.Add(mySection111, 0, 0); // (quadrante in alto sx)
            myTab.Add(mySection222, 0, 1); // (quadrante in alto sx)
            mySection333.GridPosition.ColumnSpan = 2;
            mySection333.Grid.Rows = 1;
            mySection333.Grid.Columns = 3;
            myTab.Add(mySection333, 1, 0); // mySection3 è posizionato alla riga di indice 1 e colonna di indice 0 per un Colspan pari a 2

            libraryUI.ShowAndWait(page);

            var numSections = page.FindAllByType<UISection>();

            var numTabs = page.FindAllByType<UITab>();

            var numTabControls = page.FindAllByType<UITabControl>();

            var page1 = libraryUI.CreatePageDisclaimer();
            var par111 = page1.AddParagraph(FakeStrings.PAR1, "paragraph", "gray");
            libraryUI.ShowAndWait(page1);
            libraryUI.SimulateJsEvent(par111.Id, "propertyChanged", new Dictionary<string, object> { ["text"] = "__NOTFOUND__" });

           


            var newCustomPage = libraryUI.CreatePage();
            newCustomPage.AddButton("CONTINUE", true);
            newCustomPage.Title = "This is a title";
            // === CUSTOM PAGE WITH SECTIONS ===
            myTab = newCustomPage.AddTab("tab", 2, 2);
            var mySection1 = libraryUI.CreateSectionDocument();
            mySection1.Tag = nameof(mySection1);
            // prevedo una section 1x1 visto che non specifico nient'altro
            mySection1.AddParagraph("Sono un paragraph");
            var mySection2 = libraryUI.CreateSectionDocument(); // prevedo una section 1x1 visto che non specifico nient'altro
            mySection2.Tag = nameof(mySection2);
            mySection2.AddImage("Screenshot.png");
            var mySection3 = libraryUI.CreateSectionDocument(); // prevedo una section 1x1 visto che non specifico nient'altro mySection3.AddChart()
            mySection3.Tag = nameof(mySection3);
            myTab.Add(mySection1, 0, 0); // (quadrante in alto sx)
            myTab.Add(mySection2, 0, 1); // (quadranti superiori)
            mySection3.GridPosition.ColumnSpan = 2;
            mySection3.Grid.Rows = 1;
            mySection3.Grid.Columns = 3;
            myTab.Add(mySection3, 1, 0); // mySection3 è posizionato alla riga di indice 1 e colonna di indice 0 per un Colspan pari a 2

            libraryUI.ShowAndWait(newCustomPage);

            // ==== DISCLAIMER PAGE ====
            // Istanzio disclaimer che crea 2 buttons EXIT_WITHOUT_REPORT e CONTINUE e un tab
            var pageDisclaimer = libraryUI.CreatePageDisclaimer();
            var buttons = pageDisclaimer.FindAllByType<UIButton>();
            var continueBtn = buttons.Single(b => b.Tag.ToString() == "CONTINUE");
            pageDisclaimer.RequiresCompleteRead = true;
            var par1 = pageDisclaimer.AddParagraph("Paragraph #1.", "paragraph", "gray");
            var par2 = pageDisclaimer.AddParagraph("Paragraph #2.", "paragraph", "gray");
            pageDisclaimer.AddBulletedItem("#1 bulletted item");
            pageDisclaimer.AddBulletedItem("#2 bulletted item");
            pageDisclaimer.AddOrderedItem("Item 1");
            pageDisclaimer.AddOrderedItem("Item 2");
            pageDisclaimer.AddOrderedItem("Item 3");
            var btn = pageDisclaimer.AddButton("CLICK ON ME!!", true);
            pageDisclaimer.UpdateParagraph(par1.Id, "Paragraph #1 - UPDATED.");
            pageDisclaimer.Remove(par2.Id);
            pageDisclaimer.AddImage("Screenshot.png");
            libraryUI.ShowAndWait(pageDisclaimer);

            // il js ha preparato un json e io l'ho parsificato

            libraryUI.SimulateJsEvent(continueBtn.Id, "propertyChanged",
                    new Dictionary<string, object> { ["enabled"] = true });

            var par3 = pageDisclaimer.AddParagraph("Paragraph #3 after ShowAndWait");

            pageDisclaimer.UpdateParagraph(par3.Id, "Paragraph #3 - BIS -  after ShowAndWait");


            // ==== RESULT PAGE ====
            var pageResult = libraryUI.CreatePageResult();
            pageResult.AddParagraph("Questa è la pagina di risultato.", "result-paragraph", "blue");
            libraryUI.ShowAndWait(pageResult);

            // =========== MENU PAGE ===========
            // Singola selezione con checkbox
            var menuPage = libraryUI.CreatePageMenu();
            menuPage.HasCheckboxes = true;
            menuPage.IsMultipleSelection = false;
            menuPage.SetMessage("S_COMP");
            var chbx1 = menuPage.AddItem("Activate_Injectors", "Activte_Injectors");
            var chbx2 = menuPage.AddItem("Activate_Coils", "Activte_Ignition_Coil");
            var chbx3 = menuPage.AddItem("Deactivate_Injectors", "Deactivte_Injectors");
            libraryUI.ShowAndWait(menuPage);

            var chbx4 = menuPage.AddItem("NewCheckbox", "New Checkbox");

            Console.WriteLine("HasCheckBoxes " + menuPage.HasCheckboxes);
            Console.WriteLine("IsMultipleSelection " + menuPage.IsMultipleSelection);
            Console.WriteLine("SelectedId " + menuPage.SelectedId);
            Console.WriteLine("SelectedIndex " + menuPage.SelectedIndex);
            Console.WriteLine("SelectedIds " + menuPage.SelectedIds.ContainsAny("Activate_Injectors"));
            Console.WriteLine("SelectedIndexes " + menuPage.SelectedIndexes.ContainsAny(0));

            // Simulo evento JS (selezione di una checkbox)
            libraryUI.SimulateJsEvent(chbx1.Id, "propertyChanged",
                new Dictionary<string, object> { ["checked"] = true });

            Console.WriteLine("HasCheckBoxes " + menuPage.HasCheckboxes);
            Console.WriteLine("IsMultipleSelection " + menuPage.IsMultipleSelection);
            Console.WriteLine("SelectedId " + menuPage.SelectedId);
            Console.WriteLine("SelectedIndex " + menuPage.SelectedIndex);
            Console.WriteLine("SelectedIds " + menuPage.SelectedIds.ContainsAny("Activate_Injectors"));
            Console.WriteLine("SelectedIndexes " + menuPage.SelectedIndexes.ContainsAny(0));

            // Multipla selezione con checkbox
            menuPage = libraryUI.CreatePageMenu();
            menuPage.HasCheckboxes = true;
            menuPage.IsMultipleSelection = true;
            menuPage.SetMessage("S_COMP");
            chbx1 = menuPage.AddItem("Activate_Injectors", "Activte_Injectors");
            chbx2 = menuPage.AddItem("Activate_Coils", "Activte_Ignition_Coil");
            chbx3 = menuPage.AddItem("Deactivate_Injectors", "Deactivte_Injectors");
            libraryUI.ShowAndWait(menuPage);

            chbx4 = menuPage.AddItem("NewCheckbox", "New Checkbox");

            Console.WriteLine("HasCheckBoxes " + menuPage.HasCheckboxes);
            Console.WriteLine("IsMultipleSelection " + menuPage.IsMultipleSelection);
            try
            {
                Console.WriteLine("SelectedId " + menuPage.SelectedId);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            try
            {
                Console.WriteLine("SelectedIndex " + menuPage.SelectedIndex);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("SelectedIds " + menuPage.SelectedIds.ContainsAny("Activate_Injectors"));
            Console.WriteLine("SelectedIndexes " + menuPage.SelectedIndexes.ContainsAny(0));
            /*
            for (int i = 0; i <= 500; i++)
            {
                // Simulo evento JS (selezione di un tab)
                libraryUI.SimulateJsEvent(chbx1.Id, "propertyChanged",
                    new Dictionary<string, object> { ["checked"] = true });
                libraryUI.SimulateJsEvent(chbx2.Id, "propertyChanged",
                    new Dictionary<string, object> { ["checked"] = true });
                libraryUI.SimulateJsEvent(chbx4.Id, "propertyChanged",
                    new Dictionary<string, object> { ["checked"] = true });

                Thread.Sleep(100);
                // Simulo evento JS (selezione di un tab)
                libraryUI.SimulateJsEvent(chbx1.Id, "propertyChanged",
                    new Dictionary<string, object> { ["checked"] = false });
                libraryUI.SimulateJsEvent(chbx2.Id, "propertyChanged",
                    new Dictionary<string, object> { ["checked"] = false });
                libraryUI.SimulateJsEvent(chbx4.Id, "propertyChanged",
                    new Dictionary<string, object> { ["checked"] = false });
                Thread.Sleep(100);
            }
            */

            Console.WriteLine("HasCheckBoxes " + menuPage.HasCheckboxes);
            Console.WriteLine("IsMultipleSelection " + menuPage.IsMultipleSelection);
            Console.WriteLine("SelectedIds Count " + menuPage.SelectedIds.Count);
            Console.WriteLine("SelectedIndexes Count " + menuPage.SelectedIndexes.Count);
            Console.WriteLine("SelectedIds " + menuPage.SelectedIds.ContainsAny("Activate_Injectors", "Activate_Coils"));
            Console.WriteLine("SelectedIndexes " + menuPage.SelectedIndexes.ContainsAny(0, 1));

            Console.WriteLine("SelectedIds " + menuPage.SelectedIds.ContainsAll("Activate_Injectors", "Activate_Coils")); 
            Console.WriteLine("SelectedIndexes " + menuPage.SelectedIndexes.ContainsAll(0, 1));

            Console.WriteLine("SelectedIds " + menuPage.SelectedIds.ContainsAny("NewCheckbox"));
            Console.WriteLine("SelectedIndexes " + menuPage.SelectedIndexes.ContainsAny(3));

            Console.WriteLine("SelectedIds " + menuPage.SelectedIds.ContainsAll("Activate_Injectors", "Activate_Coils", "NewCheckbox"));
            Console.WriteLine("SelectedIndexes " + menuPage.SelectedIndexes.ContainsAll(0, 1, 3));


            // TODO caso interessante, portarlo in uno unit test: verificare che la composizione di UITabe e UISection sia quella attesa

            // ==== PAGINA CUSTOM ====
            // Istanzio pagina
            var customPage = libraryUI.CreatePage();
            // Creo l'header
            customPage.SetTitle("Benvenuto nella mia applicazione", "title");
            // oppure set di Title direttamente
            customPage.Title = "Demo UIFramework C#";
            // Aggiungo un tab
            var firstTab = customPage.AddTab("mytab", 1, 1);
            // Aggiungo una section
            var section = libraryUI.CreateSectionDocument();
            // Aggiungo paragrafo alla section e bottone di STOP
            section.AddParagraph("Questa è una demo di UIFramework in C#.", "paragraph", "gray");
            firstTab.Add(section, 0, 0);

            customPage.AddButtonStop(true);
            var continueButton = customPage.AddButton("CONTINUE", true);
            
            var tab1 = customPage.AddTab("Generale", 1, 1);
            // Aggiungo una section
            var section2 = libraryUI.CreateSection();
            section2.AddOrderedItem("pippo");
            var tab2 = customPage.AddTab("Avanzate", 1, 1);
            // Aggiungo una section
            var section3 = libraryUI.CreateSection();
            section3.AddOrderedItem("Opzioni avanzate");
           
            var feedback = customPage.AddFeedbackCountdown(15000);  // 15 secondi, non parte automaticamente
            libraryUI.ShowAndWait(customPage);

            // Simulo evento JS (selezione di un tab)
            libraryUI.SimulateJsEvent(page.TabControl.Id, "propertyChanged",
                new Dictionary<string, object> { ["activeTabId"] = firstTab.Id });

            // Simulo evento JS (enable/disable di un button)
            libraryUI.SimulateJsEvent(continueButton.Id, "propertyChanged",
                new Dictionary<string, object> { ["enabled"] = false });

            // Simulo evento JS (visibilità di un button)
            libraryUI.SimulateJsEvent(continueButton.Id, "propertyChanged",
                new Dictionary<string, object> { ["visible"] = false });

            // Faccio partire il countdown
            feedback.StartCountdown();
            Thread.Sleep(20000);
            feedback.StopCountdown();

            Thread.Sleep(5000);
            // Faccio partire il countdown
            feedback.StartCountdown();

            feedback.RestartCountdown();

            Thread.Sleep(5000);
            feedback.StopCountdown();

            // === CUSTOM PAGE CON SEQUENCE ===
            var sequencePage = libraryUI.CreatePage();
            sequencePage.SetTitle("Titolo della pagina", "title");
            // Aggiungo un tab
            var tabSequence = sequencePage.AddTab("mytab", 1, 1);
            var sequence = libraryUI.CreateSequence();
            var step1 = sequence.AddStep("1", "Faults reading");
            sequence.AddStep("2", "Ecu feature reading");
            sequence.AddStep("3", "Probing");
            tabSequence.Add(sequence);
            libraryUI.ShowAndWait(sequencePage);

            sequence.UpdateStatusStep(step1.Id, "active");

            Console.ReadKey();
        }

        //private static void SimulateJsEvent(
        //    UiCommandDispatcher dispatcher,
        //    string elementId,
        //    string eventType,
        //    Dictionary<string, object> states)
        //{
        //    Console.WriteLine("\n>>> EVENTO JS");
        //    var evt = new UiEvent
        //    {
        //        ElementId = elementId,
        //        EventType = eventType, 
        //        States = states
        //    };

        //    var diffs = dispatcher.HandleEvent(evt);

        //    Console.WriteLine(">>> DIFF PRODOTTI:");
        //    Console.WriteLine(JsonConvert.SerializeObject(diffs, Formatting.Indented));
        //}
        /*

            JS UI
              │
              │  evento(change, click)
              ▼
            UiEvent
              │
              ▼
            Command Pattern
              │   (logica applicativa)
              ▼
            Page(MODEL aggiornato)
              │
              ▼
            Diff Engine
              │   (calcola delta)
              ▼
            JSON Diff → JS
        */
    }
    //public static class SigmoidGenerator
    //    {

    //        public static IEnumerable<double> SigmoidGrowth(
    //        double target,
    //        int totalSteps,
    //        double steepness = 10.0)
    //        {
    //            for (int step = 0; step <= totalSteps; step++)
    //            {
    //                // normalizza 0–1
    //                double t = (double)step / totalSteps;

    //                // funzione sigmoide
    //                double s = 1.0 / (1.0 + Math.Exp(-steepness * (t - 0.5)));

    //                // scala verso il target
    //                yield return s * target;
    //            }
    //        }


    //        public static IEnumerable<double> RandomDouble(double min, double max)
    //        {
    //            var _rnd = new Random();
    //            while (true)
    //            {
    //                yield return _rnd.NextDouble() * (max - min) + min;
    //            }
    //        }
    //    }


}
