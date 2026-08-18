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

## Project Structure (UIFramework Only)

```
UIFramework/
├── UIFramework/                  # Main class library (.NET 4.5.2)
│   ├── UIElements/               # UI control implementations
│   │   ├── Base/                 # Base classes (UIElement, ContainerElement, Style, Point, ProgressValue)
│   │   ├── Adapters/             # Adapter interfaces (ISectionMeterAdapter, ISectionChartAdapter, ITableAdapter, ISequenceAdapter)
│   │   ├── Reactive/             # Reactive programming (conditions, bindings, reactions)
│   │   ├── Helpers/              # Utility classes (ComparableHelper, ContainerElementExtensions, PropertyPathResolver)
│   │   ├── Validation/           # Validation rules (EmailValidationRule, RangeValidationRule, etc.)
│   │   ├── SpecializedPages/     # Specialized page types (Page, SpecializedPage, PageCountdown, etc.)
│   │   ├── Interfaces/           # UI element interfaces
│   │   └── [controls].cs         # Individual UI controls (buttons, inputs, displays, etc.)
│   ├── UICommandDispatcher.cs    # Event dispatcher with command registry
│   ├── UIEventDispatcher.cs      # UI event handling
│   ├── UIPropertyChange.cs       # Property change notification data
│   ├── UIFramework.csproj        # Project file
│   └── UIFramework.csproj.ReadLinesOnly
├── UIFramework.UnitTest/         # Unit tests
├── UIFramework.sln               # Solution file
└── .git/                        # Git repository
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
public class UIElement : INotifyPropertyChanged, IDisposable
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
var inputBox = new UIInputBoxBase();
inputBox.AddValidationRule(new RangeValidationRule(0, 100));
inputBox.ApplyValidationRules(userValue);
```

### 6. Translation System

`TranslationBinding` handles multi-language support:

```csharp
var inputBox = new UIInputBoxBase();
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

## Page Templates

### `Page` (Base Page)

The base page class with complete UI structure:

- `UITabControl` - Required, must contain at least one tab
- `UICommandArea` - Bottom area with command buttons (including Stop/Exit)
- `UIFeedbackArea` - Area for feedback (countdown, progress, messages)
- `UITitleArea` - Top area with title label
- `UIOverlay` - Overlay for modals/loading

**Key Methods**:
- `SetTitle(string tag, string idStr, string style)` - Set title with appearance
- `AddButton(string idStr, bool isEnabled, string style, string text)` - Add button to command area
- `AddButtonStop()` - Add Stop button (danger appearance)
- `AddFeedbackCountdown(int ms, bool isManual)` - Add countdown feedback
- `AddFeedbackProgress(int perc)` - Add progress feedback
- `AddFeedbackMessage(string msg)` - Add message feedback
- `CreateBinding(ICondition condition, IReaction reaction)` - Create reactive binding
- `Validate()` - Validate page structure (tabs, sections, buttons, feedback)

### `SpecializedPage`

Page with a single central section (`UISection`), useful for simpler pages. Provides:
- `AddImage(string imageName)` - Add image
- `AddBulletedItem(string idStr)` - Add bulleted list item
- `AddOrderedItem(string idStr, int index)` - Add ordered list item
- `AddParagraph(string idStr)` - Add paragraph text

### `PageCountdown`

Page showing a countdown/spinner:

```csharp
var page = new PageCountdown("countdown-id", 15000, uiContext);
// 15 second countdown, not auto-starting
```

### `PageDisclaimer`

Disclaimer page with "Requires Complete Read" feature:

```csharp
var page = new PageDisclaimer(uiContext);
page.RequiresCompleteRead = true; // Enable scroll-to-end to enable Continue button
```

### `PageResult`

Result page with exit button, typically shown after form completion.

### `PageMenu`

Menu page with choice selection:

```csharp
var page = new PageMenu(uiContext);
page.IsMultipleSelection = true; // Allow multiple selections
page.SetMessage("S_COMP"); // Set message text
var item = page.AddItem("Activate_Injectors", "Activate_Injectors"); // Add choice
```

---

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
using UIFramework;
using UIFramework.UIElements;
using UIFramework.Interfaces;

// Create context (implementation depends on host)
var uiContext = new UIContext(new TranslationService());

// Create a page
var page = new Page(uiContext);

// Set title
page.SetTitle("TITOLO", "information");

// Add tabs
var tab = page.AddTab("mytab", 2, 2); // 2 rows, 2 columns

// Add sections to tab
var section1 = new UISection(1, 1, uiContext);
section1.AddParagraph("Hello World", "paragraph", "gray");
tab.Add(section1, 0, 0); // Row 0, Column 0

var section2 = new UISection(1, 1, uiContext);
section2.AddImage("Screenshot.png");
tab.Add(section2, 0, 1); // Row 0, Column 1

var section3 = new UISection(1, 1, uiContext);
section3.AddButton("Click Me", true, ElementStatusEnum.Primary.GetDescription(), "btn1");
tab.Add(section3, 1, 0); // Row 1, Column 0, spanning both columns

section3.GridPosition.ColumnSpan = 2;

// Add command button
var cancelBtn = page.AddButton("CANCEL", true, ElementStatusEnum.Danger.GetDescription(), "Cancel");

// Add feedback
var feedback = page.AddFeedbackCountdown(10000); // 10 seconds

// Validate page structure
page.Validate();

// Show page (host implementation)
libraryUI.ShowAndWait(page);
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

The solution includes the main project:

1. **UIFramework.csproj** - Main library targeting .NET Framework 4.5.2
   - References: log4net, Newtonsoft.Json, ScriptLibraries.Data.Interfaces, etc.
   - Contains all UI element implementations, interfaces, and reactive model

### Dependencies

Package references (from `.gitignore` and `.csproj` files):
- `log4net` 2.0.17 - Logging
- `Newtonsoft.Json` 13.0.4 - JSON serialization
- `ScriptLibraries.Data.Interfaces` - Data interface contracts

### Build Commands

```bash
dotnet build UIFramework/UIFramework.csproj
# or
msbuild UIFramework/UIFramework.csproj
```

---

## Unit Tests

The project includes unit tests in `UIFramework.UnitTest`:

- `SingleSelection_SelectsSingleItemAndUpdatesSelectedProperties` - Tests single choice selection
- `MultipleSelection_AllowsMultipleCheckedItemsAndReportsCountsAndContainment` - Tests multiple selection
- `CreatePageDisclaimer_Test` - Tests disclaimer page creation and updates
- `Page_UITab_UISection_Composition` - Tests tab/section composition
- `PageDisclaimer_Paragraph_UpdatedByJs` - Tests paragraph updates from JS

Tests use `LibraryUI` to create pages and `SyncModelAndNotifyUI` to simulate JS events.

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