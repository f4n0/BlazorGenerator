using BlazorEngine.Enum;

namespace BlazorEngine.Models;


public class CircularLogBuffer
{
  private readonly int _maxSize;
  private readonly Queue<(DateTime Timestamp, string Message, LogType Type)> _logs = new();

  public CircularLogBuffer(int maxSize)
  {
    _maxSize = maxSize;
  }

  public void Add(string message, LogType type)
  {
    var now = DateTime.UtcNow;
    _logs.Enqueue((now, message, type));
    Cleanup(now);
  }

  public List<(string, LogType)> GetLogs()
  {
    return _logs.Select(l => (l.Message, l.Type)).ToList();
  }

  private void Cleanup(DateTime now)
  {
    // Remove oldest if over max size
    while (_logs.Count > _maxSize)
      _logs.Dequeue();
  }
}
