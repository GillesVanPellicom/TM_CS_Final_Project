using Maui_App.ViewModels;
using Maui_App.Models;

namespace Maui_App.Views;

public partial class NotesPage {
  private readonly NotesViewModel _vm;

  public NotesPage(NotesViewModel vm) {
    InitializeComponent();
    _vm = vm;
    BindingContext = _vm;
  }

  protected override async void OnAppearing() {
    base.OnAppearing();
    await _vm.LoadNotes();
  }

  private void OnNoteSelected(object sender, SelectedItemChangedEventArgs e) {
    if (e.SelectedItem is Note note) {
      _vm.EditNoteCommand.Execute(note);
    }

    // deselect the item
    if (sender is ListView listView) {
      listView.SelectedItem = null;
    }
  }
}