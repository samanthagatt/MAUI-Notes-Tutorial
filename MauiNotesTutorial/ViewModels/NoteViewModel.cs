using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MauiNotesTutorial.ViewModels;

public class NoteViewModel : ObservableObject, IQueryAttributable
{
    private Models.Note _note;
    public string Text
    {
        get => _note.Text;
        set
        {
            if (_note.Text != value)
            {
                _note.Text = value;
                // From ObservableObject base class
                OnPropertyChanged();
            }
        }
    }
    public DateTime Date => _note.Date;
    public string Identifier => _note.Filename;
    public ICommand SaveCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; }

    public NoteViewModel()
    {
        _note = new Models.Note();
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    public NoteViewModel(Models.Note note)
    {
        _note = note;
        SaveCommand = new AsyncRelayCommand(Save);
        DeleteCommand = new AsyncRelayCommand(Delete);
    }

    private async Task Save()
    {
        _note.Date = DateTime.Now;
        _note.Save();
        await Shell.Current.GoToAsync($"..?saved={_note.Filename}");
    }

    private async Task Delete()
    {
        _note.Delete();
        await Shell.Current.GoToAsync($"..?deleted={_note.Filename}");
    }

    private void RefreshProperties()
    {
        OnPropertyChanged(nameof(Text));
        OnPropertyChanged(nameof(Date));
    }

    public void Reload()
    {
        _note = Models.Note.Load(_note.Filename);
        RefreshProperties();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (!query.TryGetValue("load", out object? value))
            return;
        if (value.ToString() is string noteId)
        {
            _note = Models.Note.Load(noteId);
            // Since _note is being set not Text or Date
            RefreshProperties();
        }
    }
}
