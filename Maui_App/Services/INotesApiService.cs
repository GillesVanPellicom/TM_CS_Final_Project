#nullable enable

using Maui_App.Models;

namespace Maui_App.Services;

public interface INotesApiService {
  Task<List<Note>> GetNotesAsync(CancellationToken ct = default);
  Task<Note?> GetNoteAsync(int id, CancellationToken ct = default);
  Task<Note?> CreateNoteAsync(Note note, CancellationToken ct = default);
  Task<bool> UpdateNoteAsync(Note note, CancellationToken ct = default);
  Task<bool> DeleteNoteAsync(int id, CancellationToken ct = default);
  Task<List<Note>> SearchAsync(string query, CancellationToken ct = default);
}