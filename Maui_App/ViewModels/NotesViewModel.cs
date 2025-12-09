using System.Collections.ObjectModel;
using System.Windows.Input;
using Maui_App.Models;
using Maui_App.Views;
using Maui_App.Services;

namespace Maui_App.ViewModels;

public class NotesViewModel : BindableObject
{
    private readonly INotesApiService _api;
    private ObservableCollection<Note> _notes;
    public ObservableCollection<Note> Notes
    {
        get => _notes;
        set
        {
            _notes = value;
            OnPropertyChanged();
        }
    }

    public ICommand AddNoteCommand { get; }
    public ICommand EditNoteCommand { get; }
    public ICommand DeleteNoteCommand { get; }
    public ICommand SearchCommand { get; }
    public ICommand RefreshCommand { get; }

    public NotesViewModel(INotesApiService api)
    {
        _api = api;
        Notes = new ObservableCollection<Note>();
        AddNoteCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(AddEditNotePage)));
        EditNoteCommand = new Command<Note>(async (note) =>
        {
            if (note != null)
                await Shell.Current.GoToAsync($"{nameof(AddEditNotePage)}?noteId={note.Id}");
        });
        DeleteNoteCommand = new Command<Note>(async (note) => await DeleteNote(note));
        SearchCommand = new Command<string>(async (query) => await SearchNotes(query));
        RefreshCommand = new Command(async () => await LoadNotes());
    }

    public async Task LoadNotes()
    {
        try
        {
            Notes.Clear();
            var notes = await _api.GetNotesAsync();
            foreach (var note in notes)
            {
                Notes.Add(note);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading notes: {ex.Message}");
        }
    }

    private async Task DeleteNote(Note note)
    {
        try
        {
            if (note == null) return;
            var ok = await _api.DeleteNoteAsync(note.Id);
            if (ok) Notes.Remove(note);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting note: {ex.Message}");
        }
    }

    private async Task SearchNotes(string query)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                await LoadNotes();
                return;
            }
            var notes = await _api.SearchAsync(query);
            Notes = new ObservableCollection<Note>(notes);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error searching notes: {ex.Message}");
        }
    }
}