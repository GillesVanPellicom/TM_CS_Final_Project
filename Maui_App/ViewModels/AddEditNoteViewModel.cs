using System.Windows.Input;
using Maui_App.Models;
using Maui_App.Services;

namespace Maui_App.ViewModels;

[QueryProperty(nameof(NoteId), "noteId")]
public class AddEditNoteViewModel : BindableObject
{
    private readonly INotesApiService _api;
    private Note _note;
    public Note Note
    {
        get => _note;
        set
        {
            _note = value;
            OnPropertyChanged();
        }
    }

    private int _noteId;
    public int NoteId
    {
        set
        {
            _noteId = value;
            if (_noteId != 0)
            {
                LoadNote();
            }
        }
    }

    public ICommand SaveNoteCommand { get; }
    public ICommand DeleteNoteCommand { get; }

    public AddEditNoteViewModel(INotesApiService api)
    {
        _api = api;
        Note = new Note();
        SaveNoteCommand = new Command(async () => await SaveNote());
        DeleteNoteCommand = new Command(async () => await DeleteNote());
    }

    private async void LoadNote()
    {
        try
        {
            var loaded = await _api.GetNoteAsync(_noteId);
            if (loaded != null)
                Note = loaded;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading note: {ex.Message}");
        }
    }

    private async Task SaveNote()
    {
        try
        {
            if (Note.Id == 0)
            {
                Note.Date = DateTime.UtcNow;
                var created = await _api.CreateNoteAsync(Note);
                if (created != null)
                    await Shell.Current.GoToAsync("..");
            }
            else
            {
                var ok = await _api.UpdateNoteAsync(Note);
                if (ok)
                    await Shell.Current.GoToAsync("..");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving note: {ex.Message}");
        }
    }

    private async Task DeleteNote()
    {
        try
        {
            if (Note.Id != 0)
            {
                var ok = await _api.DeleteNoteAsync(Note.Id);
                if (ok)
                    await Shell.Current.GoToAsync("..");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting note: {ex.Message}");
        }
    }
}
