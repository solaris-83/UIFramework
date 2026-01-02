
using Newtonsoft.Json;
using UIFramework;
using UIFramework.PredefinedPages;

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

//class Program
//{
//    static void Main()
//    {
//        var parser = new ArithmeticParser();

//        Console.WriteLine("=== Test con formula applicata a lista di valori ===\n");

//        // Formula: (x - 40) / 100
//        string formula = "(x - 40) / 100";
//        List<double> valori = new List<double> { 40, 50, 60, 140, 240 };

//        Console.WriteLine($"Formula: {formula}");
//        Console.WriteLine($"Valori:  {string.Join(", ", valori)}\n");

//        List<double> risultati = parser.ParseList(formula, "x", valori);

//        Console.WriteLine("Risultati:");
//        for (int i = 0; i < valori.Count; i++)
//        {
//            Console.WriteLine($"  x = {valori[i],6:F1}  =>  {risultati[i]:F3}");
//        }

//        List<double> risultati2 = parser.ParseList(formula, valori);

//        Console.WriteLine("Risultati:");
//        for (int i = 0; i < valori.Count; i++)
//        {
//            Console.WriteLine($"  x = {valori[i],6:F1}  =>  {risultati2[i]:F3}");
//        }


//        Console.WriteLine("\n=== Altri esempi ===\n");

//        // Altri esempi di formule
//        var esempi = new Dictionary<string, (string formula, List<double> valori)>
//        {
//            { "Conversione Fahrenheit->Celsius", ("(x - 32) * 5 / 9", new List<double> { 32, 50, 68, 86, 104 }) },
//            { "Percentuale", ("x / 100", new List<double> { 10, 25, 50, 75, 100 }) },
//            { "Quadrato + 10", ("x * x + 10", new List<double> { 1, 2, 3, 4, 5 }) }
//        };

//        foreach (var esempio in esempi)
//        {
//            Console.WriteLine($"{esempio.Key}: {esempio.Value.formula}");
//            var ris = parser.ParseList(esempio.Value.formula, "x", esempio.Value.valori);
//            for (int i = 0; i < esempio.Value.valori.Count; i++)
//            {
//                Console.WriteLine($"  x = {esempio.Value.valori[i],6:F1}  =>  {ris[i]:F3}");
//            }
//            Console.WriteLine();
//        }

//        Console.WriteLine("\n=== Test con variabili multiple ===\n");

//        // Puoi anche usare più variabili contemporaneamente
//        var vars = new Dictionary<string, double>
//        {
//            { "x", 10 },
//            { "y", 20 }
//        };

//        double risultato = parser.Parse("(x + y) * 2", vars);
//        Console.WriteLine($"(x + y) * 2 con x=10, y=20  =>  {risultato}");
//    }
//}

//class Program
//{
//    static void Main()
//    {

//        var serializer = new XmlSerializer(typeof(EslxComposer));
//        using var fs = File.OpenRead("config.xml");

//        var composer = (EslxComposer)serializer.Deserialize(fs);

//        var model = composer.Models.First(m => m.Name.Contains("EV"));

//        var finalSteps = StepResolver.ResolveSteps(model, composer.StepSets);

//        foreach (var step in finalSteps.Where(s => s.Enabled))
//        {
//            Console.WriteLine($"{step.Id} -> {step.Execute}");
//        }
//    }
//}

class Program
{
    static void Main()
    {
        // Istanzio pagina
        var page = new Page();
        // Creo una sezione per contenere l'header
        page.SetTitle("Benvenuto nella mia applicazione", "title");
      
        // Creo un tabcontrol con 2 tab
        var tabs = new UITabControl();
        var tab1 = new Tab("Generale");
        tab1.Add(new UIButton("Salva", enabled: true));  // mettiamo per forza una section o direttamente dentro il tab?
        var tab2 = new Tab("Avanzate");
        tab2.Add(new UILabel("Opzioni avanzate"));

        // Creo una sezione con checkbox e textbox che aggiungo a tab1
        var section1 = new UISection("Preferenze");
        var chk = new UICheckbox("Abilita notifiche", false);
        var txt = new UITextbox("Email", "");
        var feedbackCountdown = new UIFeedback(FeedbackMode.Countdown, text: "Operazione in corso...", milliseconds: 23000);
        
        section1.Add(feedbackCountdown);
        section1.Add(chk);
        section1.Add(txt);
        tab1.Add(section1);

        // Creo una sezione con dropdown, textbox e lista unordered che aggiungo a tab2
        var section2 = new UISection("Dati");
        var dropdown = new UIDropDown(
        [
            new DropDownOption("IT", "Italia"),
            new DropDownOption("FR", "Francia"),
            new DropDownOption("DE", "Germania")
        ])
        {
            Id = "dd-country"
        };

        var name = new UITextbox("Nome") { Id = "txt-name" };
        section2.Add(dropdown);
        section2.Add(name);

        var unorderedList = new UIListElement(ListKind.Unordered);
        var label = new UILabel("Elemento 3");
        unorderedList.Add(new UILabel("Elemento 1"));
        unorderedList.Add(new UILabel("Elemento 2"));
        unorderedList.Add(label);
        section2.Add(unorderedList);
        tab2.Add(section2);

        // Aggiungo i tab al tabcontrol e lo aggiungo alla pagina
        tabs.Add(tab1);
        tabs.Add(tab2);
        tabs.SelectedTabId = tab1.Id;
        page.Add(tabs);

        var dispatcher = new UiCommandDispatcher(page);
        Console.WriteLine("=== LOAD PAGE INIZIALE ===");
        Console.WriteLine(JsonConvert.SerializeObject(page, Formatting.Indented));

        var diffs = dispatcher.EvaluateDiff();
        Console.WriteLine("\n>>> EvaluateDiff");
        Console.WriteLine(JsonConvert.SerializeObject(diffs, Formatting.Indented));

        unorderedList.Remove(label.Id);
        diffs = dispatcher.EvaluateDiff();
        Console.WriteLine("\n>>> EvaluateDiff");
        Console.WriteLine(JsonConvert.SerializeObject(diffs, Formatting.Indented));

        // Simulo evento JS (checkbox click)
        SimulateJsEvent(dispatcher, chk.Id, "change",
            new Dictionary<string, object> { ["checked"] = true });

        // Simulo evento JS (textbox input)
        SimulateJsEvent(dispatcher, txt.Id, "change",
            new Dictionary<string, object> { ["value"] = "Mario" });

        // Simulo evento JS (dropdown choice)
        SimulateJsEvent(dispatcher, dropdown.Id, "change",
            new Dictionary<string, object> { ["selected"] = "FR" });

        feedbackCountdown.StartCountdown();
        // TODO da migliorare perché va capito a chi agganciare l'evento in eslx
        feedbackCountdown.TickElapsed += (s, remaining) =>
        {
            diffs = dispatcher.EvaluateDiff();
            Console.WriteLine("\n>>> EvaluateDiff");
            Console.WriteLine(JsonConvert.SerializeObject(diffs, Formatting.Indented));
        };

        Console.ReadKey();
    }

      
    private static void SimulateJsEvent(
        UiCommandDispatcher dispatcher,
        string elementId,
        string eventType,
        Dictionary<string, object> payload)
    {
        Console.WriteLine("\n>>> EVENTO JS");
        var evt = new UiEvent
        {
            ElementId = elementId,
            EventType = eventType,
            Payload = payload
        };

        var diffs = dispatcher.HandleEvent(evt);

        Console.WriteLine(">>> DIFF PRODOTTI:");
        Console.WriteLine(JsonConvert.SerializeObject(diffs, Formatting.Indented));
    }
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
public static class SigmoidGenerator
    {

        public static IEnumerable<double> SigmoidGrowth(
        double target,
        int totalSteps,
        double steepness = 10.0)
        {
            for (int step = 0; step <= totalSteps; step++)
            {
                // normalizza 0–1
                double t = (double)step / totalSteps;

                // funzione sigmoide
                double s = 1.0 / (1.0 + Math.Exp(-steepness * (t - 0.5)));

                // scala verso il target
                yield return s * target;
            }
        }


        public static IEnumerable<double> RandomDouble(double min, double max)
        {
            var _rnd = new Random();
            while (true)
            {
                yield return _rnd.NextDouble() * (max - min) + min;
            }
        }
    }

