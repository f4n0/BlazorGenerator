using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.Logging.Abstractions;

namespace BlazorEngine.TestHelper;

/// <summary>
/// Minimal <see cref="Renderer"/> that satisfies the render handle contract without
/// performing any actual DOM work. Attaching a component to this renderer makes
/// <c>StateHasChanged()</c> and <c>InvokeAsync()</c> safe to call from unit tests.
/// </summary>
#pragma warning disable BL0006 // Do not use RenderTree types
internal sealed class TestRenderer : Renderer
{
  public TestRenderer()
    : base(new EmptyServiceProvider(), NullLoggerFactory.Instance) { }

  // Dispatcher.CreateDefault() binds to the creation thread, causing
  // Dispatcher.AssertAccess() to throw when StateHasChanged() is called from any
  // other thread (ThreadPool continuations, xUnit worker threads, etc.).
  // TestDispatcher reports CheckAccess() = true unconditionally and runs all work inline.
  public override Dispatcher Dispatcher { get; } = new TestDispatcher();

  /// <summary>Gives <paramref name="component"/> a valid render handle.</summary>
  public void Attach(IComponent component) => AssignRootComponentId(component);

  protected override void HandleException(Exception exception) { }

  protected override Task UpdateDisplayAsync(in RenderBatch renderBatch)
    => Task.CompletedTask;

  private sealed class EmptyServiceProvider : IServiceProvider
  {
    public object? GetService(Type serviceType) => null;
  }

  private sealed class TestDispatcher : Dispatcher
  {
    // Always report being on the correct thread — no thread affinity in tests.
    public override bool CheckAccess() => true;

    public override Task InvokeAsync(Action workItem)
    {
      workItem();
      return Task.CompletedTask;
    }

    public override Task InvokeAsync(Func<Task> workItem)
      => workItem();

    public override Task<TResult> InvokeAsync<TResult>(Func<TResult> workItem)
      => Task.FromResult(workItem());

    public override Task<TResult> InvokeAsync<TResult>(Func<Task<TResult>> workItem)
      => workItem();
  }
}
#pragma warning restore BL0006
