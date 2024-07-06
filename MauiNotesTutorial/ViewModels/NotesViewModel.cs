using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace MauiNotesTutorial.ViewModels;

public class NotesViewModel : IQueryAttributable
{
    public ObservableCollection<NoteViewModel> AllNotes { get; }
    public ICommand NewCommand { get; }
    public ICommand SelectNoteCommand { get; }

    public NotesViewModel()
    {
        AllNotes = new ObservableCollection<NoteViewModel>(Models.Note.LoadAll().Select(n => new NoteViewModel(n)));
        NewCommand = new AsyncRelayCommand(NewNoteAsync);
        SelectNoteCommand = new AsyncRelayCommand<NoteViewModel>(SelectNoteAsync);
    }

    private async Task NewNoteAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.NotePage));
    }

    private async Task SelectNoteAsync(NoteViewModel? note)
    {
        if (note != null)
            await Shell.Current.GoToAsync($"{nameof(Views.NotePage)}?load={note.Identifier}");
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        bool TryGetQueryValueAsStringOrEmpty(string key, out string value)
        {
            value = "";
            if (!query.TryGetValue(key, out object? objectValue))
                return false;
            if (objectValue.ToString() is string noteId)
            {
                value = noteId;
                return true;
            }
            return false;
        }
        if (TryGetQueryValueAsStringOrEmpty("deleted", out string noteId))
        {
            NoteViewModel? matchedNote = FirstOrDefaultNote(noteId);
            if (matchedNote != null)
                AllNotes.Remove(matchedNote);
        }
        else if (TryGetQueryValueAsStringOrEmpty("saved", out noteId))
        {
            NoteViewModel? matchedNote = FirstOrDefaultNote(noteId);
            if (matchedNote != null)
            {
                matchedNote.Reload();
                // Move it to the top since it was just updated
                AllNotes.Move(AllNotes.IndexOf(matchedNote), 0);
            }
            else
                AllNotes.Insert(0, new NoteViewModel(Models.Note.Load(noteId)));
        }
    }

    private NoteViewModel? FirstOrDefaultNote(string noteId)
        => AllNotes.FirstOrDefault((n) => n.Identifier == noteId);
}
