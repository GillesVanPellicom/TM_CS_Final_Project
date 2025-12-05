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

    public AddEditNoteViewModel(INotesApiService api)
    {
        _api = api;
        Note = new Note();
        SaveNoteCommand = new Command(async () => await SaveNote());
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
            // Handle exceptions
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
            // Handle exceptions
        }
    }
}
