using Maui_App.ViewModels;

namespace Maui_App.Views;

public partial class AddEditNotePage : ContentPage
{
    public AddEditNotePage(AddEditNoteViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
