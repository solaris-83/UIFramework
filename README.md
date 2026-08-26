# UIFramework - C# UI Framework for Desktop Applications

## Overview

UIFramework is a comprehensive C# UI framework for building desktop applications, particularly suited for industrial, embedded, and point-of-sale interfaces. The framework provides a complete set of UI components, a reactive programming model, a command dispatching system, and predefined page templates to accelerate development.

**Target Framework**: .NET Framework 4.5.2

**Key Features**:
- Component-based UI hierarchy
- Reactive programming with conditions and bindings
- Command pattern for event handling
- Comprehensive validation system
- Multi-language support (translation binding)
- Grid-based layout system
- Predefined page templates (disclaimer, result, menu, etc.)
- Status displays (gauges, meters, charts)
- File input handling
- Popup and modal dialog support

---

## Table of Contents

- [Overview](#overview)
- [Solution Architecture](#solution-architecture)
- [Project Structure](#project-structure)
- [Core Concepts](#core-concepts)
- [Usage Examples](#usage-examples)
- [How to Use a Page](#how-to-use-a-page)
- [Page Templates](#page-templates)
- [Data Grid](#data-grid)
- [File Input](#file-input)
- [Range-Based Status Display](#range-based-status-display-gaugemeter)
- [Configuration and Build](#configuration-and-build)
- [Unit Tests](#unit-tests)

---

## Solution Architecture

`UIFramework.sln` now contains a **single project: `UIFramework`** (namespace `UIFramework`, e.g. `UIFramework.UIElements`, `UIFramework.Reactive`, `UIFramework.Interfaces`). This is the actively maintained library and the one you should reference for all new development.

> Note: the repository still contains a few sibling folders (`UIFrameworkDotNet`, `ConsoleApp`, `UIFramework.UnitTest`) left over from an earlier, unrelated implementation. These have been **removed from `UIFramework.sln`** and are no longer built as part of the solution — treat them as legacy/unmaintained and do not reference them from new code. All examples in this README target the `UIFramework` project only.

## Project Structure

```
UIFramework/
├── UIFramework/                  # Class library (.NET 4.5.2) — the only project in UIFramework.sln
│   ├── UIElements/                # UI control implementations
│   │   └── Base/                  # Base classes (UIElement, ContainerElement, Grid, GridPosition, Point, ProgressValue, PopupResult)
│   ├── Interfaces/                # Core interfaces (ICommand, IUIContext, IValidationRule, ITranslatable, ...)
│   │   ├── Adapters/               # IPageAdapter, ISectionChartAdapter, ISectionMeterAdapter, ISequenceAdapter, ITableAdapter
│   │   └── Reactive/                # ICondition, IReaction
│   ├── Reactive/                  # Conditions, reactions and bindings (EqualsCondition, Binding, Reaction, ...)
│   ├── Commands/                  # CommandRegistry
│   ├── Helpers/                   # ComparableHelper, ContainerElementExtensions, PropertyPathResolver
│   ├── Validation/                # Validation rules (EmailValidationRule, RangeValidationRule, etc.)
│   ├── SpecializedPages/          # Page, SpecializedPage, PageCountdown, PageDisclaimer, PageMenu, PageResult
│   ├── SpecializedPopups/         # UISimpleModalPopup, UIWaitPopup
│   ├── UICommandDispatcher.cs     # Event dispatcher with command registry
│   ├── UIEventDispatcher.cs       # UI event handling
│   ├── UIPropertyChange.cs       # Property change notification data
│   └── UIFramework.csproj        # Project file
├── UIFramework.sln                # Solution file (single project: UIFramework)
└── .git/                          # Git repository
```

---

## Core Concepts

### 1. UI Element Hierarchy

All UI elements inherit from `UIElement`, which provides:
- `Id` - Unique identifier (GUID string)
- `Type` - Element type name (e.g., "UIButton", "UISection")
- `Style` - Styling (Appearance, BackgroundColor, ForegroundColor, CssClassName)
- `Tag` - Optional tag string for identification
- `Props` - Dictionary of persistent properties (serializable)
- `States` - Dictionary of mutable state values
- `PropertyChanged` - Event for property changes (controlled, single subscriber)
- `Visible` / `Enabled` - Visibility and enabled state
- `Dispose()` - Cleanup method

```csharp
public abstract class UIElement : INotifyPropertyChanged, IDisposable
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public string Type { get; }
    public Style Style { get; set; }
    public object Tag { get; set; }
    public Dictionary<string, object> Props { get; } = new();
    public Dictionary<string, object> States { get; } = new();
    public event Action<object, UIPropertyChange> PropertyChanged;
    public bool Visible { get; set; } = true;
    public bool Enabled { get; set; } = true;
    public virtual void Dispose() { /* ... */ }
}
```

### 2. Container Elements

`ContainerElement` extends `UIElement` and adds child management:

```csharp
public class ContainerElement : UIElement
{
    public List<UIElement> Children { get; }
    public event Action<ContainerElement, UIElement> ItemAdded;
    public event Action<ContainerElement, UIElement> ItemRemoved;
    public void Add(UIElement element) { /* ... */ }
    public bool Remove(string id) { /* ... */ }
}
```

### 3. Reactive Programming

The framework includes a reactive model with `ICondition`, `IReaction`, and `Binding`:

**Conditions** (`ICondition`): Evaluate to a boolean result
- `EqualsCondition` - Check if a property equals a value
- `GreaterThanCondition` / `LessThanCondition` - Compare numeric values
- `GreaterThanOrEqualsCondition` / `LessThanOrEqualsCondition` - Numeric comparison with equality
- `NotEqualsCondition` - Check inequality
- `AndCondition` - All child conditions must be true
- `OrCondition` - Any child condition must be true

**Reactions** (`IReaction`): Act on a boolean condition result
- `Reaction` - Set a property value on a target element
- `CompositeReaction` - Execute one of two reactions based on condition result

**Bindings** (`Binding`): Connect a condition to a reaction
```csharp
var condition = new EqualsCondition(element, "PropertyName", expectedValue);
var reaction = new Reaction(targetElement, "PropertyName", newValue);
var binding = new Binding(condition, reaction);
condition.GetTargetElement().AddBinding(binding);
```

### 4. Command Pattern

UI events are dispatched via a command registry:

```csharp
// Register a command for a specific event type
_registry.Register<UIButton>(
    UIEventType.OnButtonClicked,
    (btn) => new ButtonCommand(btn)
);

// Resolve and execute a command
var cmd = _registry.Resolve(element, eventType);
cmd.Execute(states);
```

The `UICommandDispatcher` manages the snapshot of UI elements and resolves commands based on element type and event type.

### 5. Validation

Extensive validation rule system implementing `IValidationRule`:

| Rule Type | Description |
|-----------|-------------|
| `EmailValidationRule` | Validate email format |
| `ExactLengthValidationRule` | Validate exact string length |
| `MaxLengthValidationRule` | Maximum string length |
| `MaxValueValidationRule` | Maximum numeric value |
| `MinLengthValidationRule` | Minimum string length |
| `MinValueValidationRule` | Minimum numeric value |
| `RangeValidationRule` | Value within range |
| `RegexValidationRule` | Regex pattern match |

```csharp
// UIInputBoxBase is abstract; use a concrete subclass such as UIInputBox
var inputBox = new UIInputBox(false);
inputBox.AddValidationRule(new RangeValidationRule(0, 100));
inputBox.ApplyValidationRules(userValue);
```

### 6. Translation System

`TranslationBinding` handles multi-language support:

```csharp
// UIInputBoxBase is abstract; use a concrete subclass such as UIInputBox
var inputBox = new UIInputBox(false);
inputBox.AttachContext(uiContext); // Attaches translator
inputBox.ResolveTranslationBindings(); // Resolves all translatable properties
inputBox.Name = "my_name"; // Automatically translates and sets Props["name"]
```

---

## User Interface Controls

### Interactive Elements

| Control | Description |
|---------|-------------|
| `UIButton` | Button with click event, appearance support |
| `UIChoice` | Choice item with checked state |
| `UIChoiceGroup` | Group of choices (radiobuttons or checkboxes), supports single/multiple selection |
| `UIInputBox` | Base text input with translation support |
| `UITextBox` | General text input with hex/byte conversion |
| `UISecureTextBox` | Password/secure text input |
| `UINumericBox` | Numeric input with min/max validation and spinners |
| `UIDropDown` | Dropdown selection (basic skeleton) |
| `UIFileInputBox` | File selection dialog |

### Display Elements

| Control | Description |
|---------|-------------|
| `UILabel` | Label with text, supports translation |
| `UIStatus` | Status display with look-and-feel (success, error, warning, etc.) |
| `UIGauge` | Gauge display with warning/error/valid ranges |
| `UIMeter` | Meter with point tracking, ranges, and units |
| `UIChart` | Chart with axes and signals |
| `UIChartAxis` | Chart axis (X/Y with title, unit, min/max) |
| `UIChartSignal` | Chart signal with point tracking |
| `UIHeadingElement` | Base for heading displays |
| `UIOverlay` | Overlay for popups/modals |

### Layout Elements

| Control | Description |
|---------|-------------|
| `UISection` | Grid-based section with rows/columns, orientation, wrap |
| `UISectionMeter` | Horizontal section for meters/gauges |
| `UISectionChart` | Horizontal section for charts |
| `UISectionCard` | Section with title property |
| `UITab` | Tab with title and grid-based content |
| `UITabControl` | Management of multiple tabs |
| `UICommandArea` | Area for command buttons (typically bottom of page) |

### Feedback Elements

| Control | Description |
|---------|-------------|
| `UIFeedbackCountdown` | Countdown timer |
| `UIFeedbackProgress` | Progress bar |
| `UIFeedbackMessage` | Message display |

---

## Usage examples

The snippets below use the public types in the `UIFramework` project (the only project in `UIFramework.sln`), primarily under the `UIFramework.UIElements` namespace.

### Interactive elements

```csharp
using System.Text;
using UIFramework.UIElements;

var button = new UIButton("SAVE", true, "primary", "Save");
button.Clicked += (_, __) => Console.WriteLine("Save clicked");

var choiceGroup = new UIChoiceGroup(uiContext);
choiceGroup.SetAppearance("radiobutton");
choiceGroup.AddItem("AUTO", "Automatic", true);
choiceGroup.AddItem("MANUAL", "Manual");

var inputBox = new UIInputBox(false)
{
    Name = "user_name",
    Description = "BCA_USER_NAME",
    IsMandatory = true
};
inputBox.AttachContext(uiContext);

var textBox = new UITextBox("Hello");
var hex = textBox.ToHex();

var secure = new UISecureTextBox();
secure.SetValue(Encoding.UTF8.GetBytes("secret"));

var numeric = new UINumericBox(10)
{
    MinValue = 0,
    MaxValue = 100,
    StepSize = 5,
    ShowSpinners = true
};

var dropDown = new UIDropDown(); // basic skeleton in this project; population API is still commented out
var option = new DropDownOption("low", "Low");

var fileInput = new UIFileInputBox
{
    AcceptPdf = true,
    AcceptTxt = true
};
fileInput.AttachContext(uiContext);
```

### Display elements

```csharp
using UIFramework.UIElements;

var label = new UILabel("Ready");

var status = new UIStatus();
status.Title = "Machine";
status.Text = "Idle";
status.Update("Machine ready", "success");

var gauge = new UIGauge("temperature");
gauge.AddValidRange(20, 30);
gauge.AddWarningRange(30, 40);
gauge.AddErrorRange(40, 100);
gauge.SendUpdate(35);

var meter = new UIMeter("pressure");
meter.Unit = "bar";

var chart = new UIChart(uiContext);
var xAxis = chart.AddXAxis("Time", "s", 0, 60);
var yAxis = chart.AddYAxis("RPM", "rpm", 0, 5000);
var signal = chart.AddSignal("rpm", "Engine RPM", "#00ff00");
signal.SetYAxis(yAxis);
chart.SendUpdate("rpm", 1, 1200);

var heading = new UIHeadingElement { Title = "Overview", SubTitle = "Live data" };
var overlay = new UIOverlay(uiContext);
```

### Layout elements

```csharp
using UIFramework.UIElements;

var tab = new UITab("main", 2, 2);

var section = new UISection(1, 1, uiContext);
section.AddParagraph("Hello world");
tab.Add(section, 0, 0);

var card = new UISectionCard(1, 1, uiContext);
card.Title = "Summary";
card.AddParagraph("Card content");

var meterSection = new UISectionMeter(uiContext);
var speedGauge = meterSection.AddGauge("speed");

var chartSection = new UISectionChart(uiContext);
var tempChart = chartSection.AddChart();

var commandArea = new UICommandArea(uiContext);
commandArea.Add(new UIButton("CANCEL", true, "danger", "Cancel"));

var titleArea = new UITitleArea(uiContext);
titleArea.Add(new UILabel("Main screen"));
```

### Feedback elements

```csharp
using UIFramework.UIElements;

var countdown = new UIFeedbackCountdown(15000, isManual: false);
countdown.StartCountdown();

var progress = new UIFeedbackProgress(25, "Loading");
progress.SendUpdate(50, "Halfway there");

var message = new UIFeedbackMessage("Ready");
message.UpdateText("Processing...");
```

### Composite / specialized elements

```csharp
using System.Collections.Generic;
using UIFramework.UIElements;

var device = new UIDevice("injector");
device.SetStatus("active", "Ready");

var sequence = new UISequence(uiContext);
var step1 = sequence.AddStep("Connect");
sequence.UpdateStep(step1.Id, "active");

var table = new UITable();
table.LoadData(df);

var template = new DataGridTemplate();
template.AddColumn("Name");
template.AddColumn("Value");
var grid = new UIDataGrid(template, uiContext);
var row = new UIDataGridRow(uiContext);
row.AddCell("Name", new UILabel("Pressure"));
row.AddCell("Value", new UITextBox("12"));
grid.AddRow(row);

var html = new UIHTMLViewer(htmlPath);
html.AttachContext(uiContext);

var loader = new UILoader("spinner-big");

var popup = new UIPopup(true, uiContext);
popup.Title = "Confirm";
popup.AddButton("OK", "OK", true);

var thermo = new UIThermometer("cabin");
thermo.SendUpdate(22);
```

## How to use a page

1. Create a `Page` (or one of the `SpecializedPage` subclasses), passing an `IUIContext`.
2. Set the page title.
3. Add a tab and one or more sections.
4. Populate sections with elements.
5. Subscribe to `PropertyChanged` / element events to react to UI interactions, or update element properties directly to push changes to the UI.

```csharp
using UIFramework.SpecializedPages;
using UIFramework.UIElements;

var page = new Page(uiContext);

page.SetTitle("title", "Dashboard", "info");

var tab = page.AddTab("main", 2, 2);

var topLeft = new UISection(1, 1, uiContext);
topLeft.Add(new UIButton("REFRESH", true, "primary", "Refresh"));
topLeft.AddParagraph("System ready");
tab.Add(topLeft, 0, 0);

var topRight = new UISection(1, 1, uiContext);
topRight.AddImage("Screenshot.png");
tab.Add(topRight, 0, 1);

var bottom = new UISection(1, 1, uiContext);
var status = bottom.AddParagraph("Waiting for updates...");
tab.Add(bottom, 1, 0);

// Push an update to the element (e.g. in response to an external event)
status.Text = "Updated from JS";
```

## Page Templates

### `Page` (Base Page)

The base page class with complete UI structure:

- `UITabControl` - Required, must contain at least one tab
- `UICommandArea` - Bottom area with command buttons (including Stop/Exit)
- `UIFeedbackArea` - Area for feedback (countdown, progress, messages)
- `UITitleArea` - Top area with title label
- `UIOverlay` - Overlay for modals/loading

**Typical use**

```csharp
var page = libraryUI.CreatePage();
var tab = page.AddTab("main", 2, 2);
tab.Add(libraryUI.CreateSection(), 0, 0);
```

### `SpecializedPage`

Page with a single central section (`UISection`), useful for simpler pages.

**Typical use**

```csharp
var page = new PageCountdown("countdown-id", 15000, uiContext);
page.AddParagraph("Loading...");
```

### `PageCountdown`

Page showing a countdown/spinner:

```csharp
var page = new PageCountdown("countdown-id", 15000, uiContext);
// 15 second countdown, not auto-starting
```

### `PageDisclaimer`

Disclaimer page with scroll-to-end support and a Continue button:

```csharp
var page = new PageDisclaimer(uiContext);
page.RequiresCompleteRead = true;
page.AddParagraph("Read everything before continuing.");
page.AddBulletedItem("Item 1");
```

### `PageResult`

Result page with exit button, typically shown after form completion:

```csharp
var page = new PageResult(uiContext);
page.AddParagraph("Operation completed successfully.");
```

### `PageMenu`

Menu page with choice selection:

```csharp
var page = new PageMenu(uiContext);
page.HasCheckboxes = true;
page.IsMultipleSelection = true;
page.SetMessage("Choose one or more items");
page.AddItem("Activate_Injectors", "Activate Injectors");
page.AddItem("Activate_Coils", "Activate Coils");
```

## Data Grid

### `UIDataGrid`

Table display with configurable columns and rows:

```csharp
var template = new DataGridTemplate();
template.Columns.Add(new DataGridColumn("Name", "Name", "UILabel"));
template.Columns.Add(new DataGridColumn("Value", "Value", "UINumericBox"));

// Create grid with 3 rows
var grid = new UIDataGrid(template, uiContext);
grid.AddRow(new UIDataGridRow(uiContext) { /* ... */ });
grid.AddRow(new UIDataGridRow(uiContext) { /* ... */ });
grid.AddRow(new UIDataGridRow(uiContext) { /* ... */ });
```

### `DataGridTemplate`

Defines column structure:

```csharp
var template = new DataGridTemplate();
template.Columns.Add(new DataGridColumn("Header1", "Display Name", "ElementType", "BindingProperty"));
```

### `DataGridRow`, `DataGridHeaderRow`

Individual row and header elements within the data grid.

---

## File Input

### `UIFileInputBox`

File selection dialog with filter support:

```csharp
var fileInput = new UIFileInputBox();
fileInput.AcceptImage = true;
fileInput.AcceptPdf = true;
fileInput.AcceptTxt = true;

// Trigger file selection (from JS)
fileInput.OnFileSelected();
// After selection, properties are populated:
// - FileName: selected file name
// - FileSize: file size in bytes
// - FileType: file extension
// - LastModifiedDate: last modification date
// - HasFile: true if a file was selected
```

Command to execute file selection:

```csharp
var cmd = new FileInputBoxCommand(fileInput);
cmd.Execute(states); // states may contain "contents" byte array
```

---

## Range-Based Status Display (Gauge/Meter)

### `UIMeter` and `UIGauge`

Both support range-based status coloring through the `Point` property:

```csharp
var meter = new UIGauge("temperature");
// Add valid range (green): 20-30 degrees
meter.AddValidRange(20, 30);
// Add warning range (yellow): 30-40 degrees
meter.AddWarningRange(30, 40);
// Add error range (red): 40+ degrees
meter.AddErrorRange(40, 100);

// Setting the point automatically colors based on ranges
meter.Point = new Point(0, 35); // Will show Warning (yellow)

// The ValueStatus property reflects the current color:
// ElementStatusEnum.Normal, Warning, Error, Success, Danger, Primary
```

### `Range` class:

```csharp
public class Range
{
    public double From { get; set; }
    public double To { get; set; }
}
```

---

## Example: Creating a Complete Page

```csharp
using UIFramework.SpecializedPages;

var page = new PageDisclaimer(uiContext);

page.RequiresCompleteRead = true;
var intro = page.AddParagraph("Read the disclaimer and continue.");
page.AddBulletedItem("First bullet");
page.AddOrderedItem("First step", 1);
page.AddImage("Screenshot.png");

// Push an update to the element (e.g. in response to an external event)
intro.Text = "Updated after show";
```

---

## Example: Reactive Binding (Condition + Reaction)

```csharp
using UIFramework;
using UIFramework.UIElements;
using UIFramework.Interfaces;
using UIFramework.Reactive;

// Create condition: check if property equals expected value
var condition = new EqualsCondition(temperatureMeter, "ValueStatus", ElementStatusEnum.Error.GetDescription());

// Create reaction: change some other property when condition is true
var reaction = new Reaction(otherElement, "SomeProperty", newValue);

// Create binding
var binding = new Binding(condition, reaction);

// Evaluate (can be called from property changed handler)
binding.Evaluate();
```

---

## Example: Validation with Numeric Box

```csharp
using UIFramework;
using UIFramework.UIElements;

// Create numeric box with range validation
var numericBox = new UINumericBox(50); // Value 50, step 1

// Set min/max validation
numericBox.MinValue = 0;      // Minimum allowed value
numericBox.MaxValue = 100;    // Maximum allowed value

// Set step size
numericBox.StepSize = 5;      // Step increment

// Add custom validation rule
numericBox.AddValidationRule(new RangeValidationRule(0, 100));

// Value will be validated automatically on change
numericBox.Value = 75; // Valid (within 0-100)
numericBox.Value = 150; // Invalid (exceeds max), triggers DataErrorInfo
```

---

## Example: File Input Box

```csharp
using UIFramework;
using UIFramework.UIElements;

// Create file input box
var fileInput = new UIFileInputBox();

// Configure accepted file types
fileInput.AcceptImage = true;
fileInput.AcceptPdf = true;
fileInput.AcceptTxt = true;
fileInput.AcceptLog = true;
fileInput.AcceptCertificate = false;

// Attach to UI context
fileInput.AttachContext(uiContext);

// Trigger file selection (from JS event handler)
fileInput.OnFileSelected();

// After selection, read properties:
bool hasFile = fileInput.HasFile;       // True if file was selected
string fileName = fileInput.FileName;   // Selected file name
long fileSize = fileInput.FileSize;     // File size in bytes
string fileType = fileInput.FileType;   // File extension (e.g., ".pdf")
DateTime? lastModified = fileInput.LastModifiedDate;
byte[] fileContents = fileInput.Value.ToArray(); // Encoded file data
```

---

## Configuration and Build

### Project Files

`UIFramework.sln` contains a single project targeting **.NET Framework 4.5.2**:

- **UIFramework.csproj** - The class library described throughout this README (`UIFramework` namespace).

### Dependencies

Package references (from `UIFramework.csproj`):
- `log4net` - Logging
- `Newtonsoft.Json` - JSON serialization
- `ScriptLibraries.Data.Interfaces` - Data interface contracts
- `HtmlAgilityPack` - HTML parsing (used by `UIHTMLViewer`)
- `BaseCustomApp.Helpers` - Shared helper utilities

### Build Commands

Since this is a classic .NET Framework (non-SDK-style) project, build it with MSBuild rather than `dotnet build`:

```powershell
# Build the solution
msbuild UIFramework.sln /p:Configuration=Debug

# Or build the project directly
msbuild UIFramework\UIFramework.csproj
```

---

## Unit Tests

`UIFramework.sln` currently has **no unit test project**. The repository still contains a `UIFramework.UnitTest` folder with MSTest tests, but it targets the legacy `UIFrameworkDotNet` library and is no longer part of the solution — it is not applicable to the `UIFramework` project described in this README.

---

## Logging

The framework uses `log4net` for logging. Default configuration is via `log4net` conventions.

Both the project and solution include `log4net` references.

---

## Version History

Refer to git commits for change history:
- `3c2a510` - Merge pull request #2 from solaris-83/dotnet452
- `8367388` - Last official version
- `5683b4b` - Merge pull request #1 from solaris-83/dotnet452
- `a1a35f5` - Communication between C# <--> JS
- `3ba4b79` - Possibility to add a title to a tab
- `a1a35f5` - Initial communication setup