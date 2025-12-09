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
    private string _searchText;
    private bool _isSearching;
    private bool _isBusy;

    public ObservableCollection<Note> Notes
    {
        get => _notes;
        set
        {
            _notes = value;
            OnPropertyChanged();
        }
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
        }
    }
    
    public bool IsSearching
    {
        get => _isSearching;
        set
        {
            _isSearching = value;
            OnPropertyChanged();
        }
    }

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged();
        }
    }

    public ICommand AddNoteCommand { get; }
    public ICommand EditNoteCommand { get; }
    public ICommand DeleteNoteCommand { get; }
    public ICommand SearchCommand { get; }
    public ICommand ClearSearchCommand { get; }

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
        SearchCommand = new Command(async () => await SearchNotes());
        ClearSearchCommand = new Command(async () =>
        {
            if (!IsSearching) return;
            
            SearchText = string.Empty;
            IsSearching = false;
            await LoadNotes();
        });
    }

    public async Task LoadNotes()
    {
        if (IsBusy) return;
        IsBusy = true;
        
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
        finally
        {
            IsBusy = false;
        }
    }

    private async Task DeleteNote(Note note)
    {
        if (note == null) return;
        
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            var ok = await _api.DeleteNoteAsync(note.Id);
            if (ok) Notes.Remove(note);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting note: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task SearchNotes()
    {
        if (IsBusy) return;

        if (string.IsNullOrWhiteSpace(SearchText))
        {
            IsSearching = false;
            await LoadNotes();
            return;
        }
        
        IsBusy = true;

        try
        {
            IsSearching = true;
            Notes.Clear();
            var notes = await _api.SearchAsync(SearchText);
            foreach (var note in notes)
            {
                Notes.Add(note);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error searching notes: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}