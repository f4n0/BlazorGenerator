# Migrating your project to BlazorEngine v5 (NuGet consumers)

You only reference the **BlazorEngine NuGet package**. The engine internals are not yours to touch —
every change below happens in *your* code. This file is a lookup table of mechanical rewrites:

> **From** `old thing` → **Do** `new thing`. If you never had the "From", skip the row.

It is self-contained: paste it (or the relevant section) into an AI session working on your repo, or follow it by hand.

---

## 1. Package / csproj plumbing

| From | Do |
|---|---|
| `<Import Project="…/build/BlazorEngine.targets" …>` in a csproj | Deleted. The new package ships no MSBuild tasks. |
| PackageReference / project reference to `BlazorEngine.BuildTasks` | Deleted. Discontinued. |
| PackageReference / project reference to `BlazorEngine.TestHelper` | Deleted. Discontinued. |
| `BlazorEngineModuleInit.g.cs` in `obj/`, embedded `.blazorengine.dat` | Nothing to do. Update the package, rebuild, they are gone. |
| `builder.Services.AddBlazorEngine()` | Still compiles, but gives an **empty menu**. Pass an options block: `builder.Services.AddBlazorEngine(options => { … })`. See §2. |

Works identically in Server / WebAssembly / MAUI / WPF hosts — only the options argument is new.

## 2. Menu entries

| From | Do |
|---|---|
| `[AddToMenu(Title = "List Page", Route = "list")]` on the page class | Remove the attribute. In the host: `options.AddMenuItem<ListView>("List Page", ListView.Route);` |
| `OrderSequence = 1` | `order: 1` (default 999; ties keep registration order, same tie-break as before) |
| `Group = "Sub Menu"` | `group: "Sub Menu"`. No Group / `"Default"` → omit: flat menu, as before. |
| `Icon = typeof(ApprovalsApp)` | `icon: new Size16.ApprovalsApp()` — an **instance**, not a type. Filled namespace = same visuals as before. |
| Page used only as modal / edit form (`EditFormType`, `OpenModal<T>`) | **Do not register.** Unregistered = absent from menu, exactly like before. |

Rules:
- One `AddMenuItem<TPage>` per row the old menu displayed; register each page type **once** (a later registration overwrites the earlier one).
- Routes are still handled by Blazor's `Router`. Registration only decides menu visibility. A registered-but-unrouted page shows a dead link; an unrouted-but-registered link likewise — keep them in sync viaconstants (§3).

## 3. Routes

| From | Do |
|---|---|
| `[Route("list")]` and `[AddToMenu(Route = "list")]` — string written twice | `public const string Route = "list";` on the page; `[Route(Route)]` + `AddMenuItem<…>(…, ListView.Route)`. Never retype the literal. |
| `[Route("WorksheetView/{Param}")]` (extra route template) | `[Route(Route + "/{Param}")]`. Every template the old attribute list declared gets exactly one counterpart. |

Notes:
- `[Route]` is `Microsoft.AspNetCore.Components.RouteAttribute` — keep `using Microsoft.AspNetCore.Components;` on the page so it resolves after the engine attributes are removed.
- Routes must remain byte-identical to before: bookmarks and deep links keep working.

## 4. Page actions

| From | Do |
|---|---|
| `[PageAction(Caption = "ShowProgress")]` over a method | Remove the attribute. Add the override shown below; the method body stays exactly as it was. |
| `[PageAction(Caption = "Test1", Group = "grouped")]` | Entry `new("Test1", Test1, group: "grouped")` |
| `[PageAction(…, Icon = typeof(x))]` | `new(…, icon: new x())`. No Icon → omit (default stays the Filled `Run` icon, as before). |

```csharp
protected override IReadOnlyList<PageActionDefinition> CreatePageActions() =>
[
  new("ShowProgress", ShowProgress),
  new("Ask User", AskUser),                    // sync method  -> ctor takes Action
  new("Write Log", Test5Async),                // async method -> ctor takes Func<Task>
  new("No Cancellation", Test1, group: "grouped")
];
```

- `PageActionDefinition` has both `(caption, Action)` and `(caption, Func<Task>)` constructors — the sync one wraps automatically. Never hand-write `() => { Sync(); return Task.CompletedTask; }`.
- Method used as **both** page and grid action (e.g. `public void Install(Mock? rec = null)`) keeps **one body**: the ActionBar entry calls `() => Install()`, the grid entry calls `row => Install(row)`.
- Two captions may share one method; that is fine (was true with attributes too).

## 5. Grid actions

| From | Do |
|---|---|
| `[GridAction(Caption = "Install", GridIcon = typeof(AirplaneTakeOff))]` over a row method | Remove the attribute. Override `CreateGridActions()` (signature per layout, §8). |
| `GridIcon = typeof(x)` | `new("Install", Install, new Size16.AirplaneTakeOff())` — third ctor argument; omit to keep the `Run` default. |

```csharp
// ListPage<T> and Worksheet<TData, TList> (row type = TList):
protected override IReadOnlyList<GridActionDefinition<Mock>> CreateGridActions() =>
[
  new("Install", Install, new Size16.AirplaneTakeOff()),
  new("ShowProgress", _ => ShowProgress())
];

// TwoList<TFirst, TSecond>:
//   protected override … CreateFirstListGridActions()   -> GridActionDefinition<TFirst>
//   protected override … CreateSecondListGridActions()  -> GridActionDefinition<TSecond>
```

## 6. Fields (the biggest rewrite)

| From | Do |
|---|---|
| `protected override Task LoadVisibleFields()` assigning `VisibleFields` | `protected override Task ConfigureFields(FieldListBuilder<T> fields)`. **Stop overriding `LoadVisibleFields`** — the layouts call it internally; if you override both, yours wins and ConfigureFields silently doesn't run. |
| `VisibleFields = [];` initialization | Gone. The builder owns the list. |
| `VisibleFields.AddField(nameof(Mock.Id))` | `fields.Add(x => x.Id)` |
| `AddField(nameof(x), (ref prop) => prop.Href = …)` | `fields.Add(x => x.Id).Href(…)` |

`AddFieldProperty(p => …)` → fluent chain on `FieldBuilder<T>`:

| From (`p.` …) | Do (chain on `fields.Add(x => x.Prop)`) |
|---|---|
| `p.Tooltip = "…"` | `.Tooltip("…")` |
| `p.PlaceHolder = "…"` | `.Placeholder("…")` |
| `p.Group = "…"` | `.Group("…")` |
| `p.Multiline = true` | `.Multiline()` |
| `p.Required = true` | `.Required()` |
| `p.ReadOnly = true` | `.ReadOnly()` |
| `p.Additional = true` | `.Additional()` |
| `p.Immediate = true` | `.Immediate()` |
| `p.EnableSearch = true` | `.EnableSearch()` |
| `p.TextInputType = TextInputType.Password` | `.TextInputType(TextInputType.Password)` |
| `p.TextStyle = _ => TextStyle.Italic` | `.TextStyle(_ => TextStyle.Italic)` |
| `p.Color = …` | `.Color(…)` |
| `p.Href = data => "…"` | `.Href(data => "…")` |
| `p.OnLookup = _ => new Dictionary<…>` | `.OnLookup(_ => new Dictionary<…>)` |
| `p.OnDrillDown = args => …` | `.OnDrillDown(args => …)` |
| `p.OnChange = …` | `.OnChange(…)` |
| `p.Set = …` | `.Set(…)` |
| `p.Get = args => obj` | `.Get(args => obj)` |
| `p.Get = args => Task/ValueTask<object?>` | `.GetAsync(args => …)` (Task and ValueTask overloads) |
| `p.CustomContent = …` | `.CustomContent(…)` |
| `VisibleFields.AddCustomField("name", (data, field) => builder => …)` | `fields.Custom("name", (data, field) => builder => …)` |
| `VisibleFields.Add(new VisibleField<T>().Configure(…))` | `fields.Add(new VisibleField<T>().Configure(…))` — manual instances still supported |

Notes:
- The expression overload `fields.Add(x => x.Prop)` is the primary API — the member is compile-checked. `fields.Add("Prop")` (string lookup) still exists as a fallback; don't make it the norm.
- Field order = write order = the order the old list had. Captions and defaults still come from the property (`[Display]`, etc.) exactly as before.

## 7. Icons and namespaces

| From | Do |
|---|---|
| `using BlazorEngine.Attributes;` | `using BlazorEngine.Actions; using BlazorEngine.Fields;` (keep `using Microsoft.AspNetCore.Components;` for `[Route]`) |
| `using BlazorEngine.Utils;` only for the field helpers | `using BlazorEngine.Fields;` (the builder chain) |
| `…Icon = typeof(Xyz)` / `GridIcon = typeof(Xyz)` | `new Xyz()` instances |
| Model property `public Type Icon { get; set; }` rendered as an icon column | `public Icon Icon { get; set; } = new AddSquare();` with `using Microsoft.FluentUI.AspNetCore.Components;`. The engine still renders a leftover `Type`-typed value for compatibility — do not depend on that path in new code. |

Available to the whole chain: `FieldBuilder<T>` methods above all return the builder, so order of chained calls never matters.

## 8. Which overrides exist on which layout

| Layout base class | Field overrides | Action overrides |
|---|---|---|
| `ListPage<T>` | `ConfigureFields(FieldListBuilder<T>)` | `CreatePageActions()`, `CreateGridActions()` → `GridActionDefinition<T>` |
| `CardPage<T>` | `ConfigureFields(FieldListBuilder<T>)` | `CreatePageActions()` |
| `Worksheet<TData, TList>` | `ConfigureFields(FieldListBuilder<TData>)` + `ConfigureListFields(FieldListBuilder<TList>)` | `CreatePageActions()`, `CreateGridActions()` → `GridActionDefinition<TList>` |
| `TwoList<TFirst, TSecond>` | `ConfigureFirstListFields(FieldListBuilder<TFirst>)` + `ConfigureSecondListFields(FieldListBuilder<TSecond>)` | `CreatePageActions()`, `CreateFirstListGridActions()`, `CreateSecondListGridActions()` |

All are `virtual` with empty/`Task.CompletedTask` defaults — override only what the page needs.

## 9. Deliberately unchanged — do NOT rewrite

- `[Route]` routing and the `Router`; `public override string Title`; page-typed `ShowButtons` / `ShowActions` / `ShowExportToExcel` / `GridSize` / `UseVirtualization` / `EditFormType` / `ListEditFormType`.
- Lifecycle: `LoadData()`, `OnSave`, `OnDelete`, `OnSave(TList)` on Worksheet, `NewItem()`.
- Selection: `Selected`, `ListSelected`, `FirstListSelected`, `SecondListSelected` collections and their `CollectionChanged` wiring.
- `UIServices` (dialogs, progress, lock), `BackgroundExecutor`, logging, themes, Excel export, F5 keyboard refresh, `ComponentDetached` cancellation.
- `ISecurity` provider contract, session-cached permission sets, `PermissionSet`, `BlazorEngineSettings.Instance.*` (ApplicationName, Theme, LoginRoute, …).

## 10. Behavioral differences you should simply know

- **Exceptions propagate.** The old reflection invoker swallowed errors from `[PageAction]`/`[GridAction]` bodies; the new delegate pipeline surfaces them to the error boundary. If code depended on the swallowing, add try/catch inside the method itself. Do not look for an engine-level switch.
- **Permissions can only shrink.** The optional `PermissionFlags` argument of `AddMenuItem<TPage>` (values `Read, Modify, Delete, Insert, Execute, All`) is intersected with whatever your `ISecurity` provider returns — it can restrict, never grant. It only needs to be set if you previously used a custom `ISecurity` hack to hide a page; otherwise omit. `Read` exists for symmetry; the legacy engine has no separate read gate.
- **`[ContextMenu]` is retired.** No engine surface ever consumed it. Delete those methods outright.
- **Menu source moved.** Menu content now comes only from `AddMenuItem` registrations; the old "scan every assembly for attributes" behavior is gone. Nothing in page code can add a menu entry anymore.

## 11. Finish line — verification checklist

- `dotnet build` on your solution: 0 errors, no new warnings.
- Grep asserts over your repo — all must be silent:
  `\[(AddToMenu|FooterLink|PageAction|GridAction|ContextMenu)\b` · `BlazorEngine\.Attributes` · `AddFieldProperty` · `VisibleFields\.AddField` · `VisibleFields\.AddCustomField` · `Icon\s*=\s*typeof\(` · `GridIcon\s*=\s*typeof\(`
- The menu shows exactly the rows the old attributes produced: same titles, groups, order, icons; footer items match `[FooterLink]` 1:1.
- Every old `[PageAction]` caption appears in the ActionBar (same group/icon) and every old `[GridAction]` in the row kebab menu, executing the identical method body.
- Routes untouched: old bookmarks/deep links still resolve; modal/edit-only pages remain unregistered and unrouted.
