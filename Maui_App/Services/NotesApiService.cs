using System.Net.Http.Json;
using Maui_App.Models;

namespace Maui_App.Services;

public class NotesApiService : INotesApiService
{
    private readonly HttpClient _http;

    public NotesApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Note>> GetNotesAsync(CancellationToken ct = default)
        => await _http.GetFromJsonAsync<List<Note>>("/notes", ct) ?? new List<Note>();

    public async Task<Note?> GetNoteAsync(int id, CancellationToken ct = default)
        => await _http.GetFromJsonAsync<Note>($"/notes/{id}", ct);

    public async Task<Note?> CreateNoteAsync(Note note, CancellationToken ct = default)
    {
        var res = await _http.PostAsJsonAsync("/notes", note, ct);
        if (!res.IsSuccessStatusCode) return null;
        return await res.Content.ReadFromJsonAsync<Note>(cancellationToken: ct);
    }

    public async Task<bool> UpdateNoteAsync(Note note, CancellationToken ct = default)
    {
        var res = await _http.PutAsJsonAsync($"/notes/{note.Id}", note, ct);
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteNoteAsync(int id, CancellationToken ct = default)
    {
        var res = await _http.DeleteAsync($"/notes/{id}", ct);
        return res.IsSuccessStatusCode;
    }

    public async Task<List<Note>> SearchAsync(string query, CancellationToken ct = default)
        => await _http.GetFromJsonAsync<List<Note>>($"/notes/search?query={Uri.EscapeDataString(query ?? string.Empty)}", ct)
           ?? new List<Note>();
}
