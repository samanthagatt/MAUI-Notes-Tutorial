using System.Collections.ObjectModel;

namespace MauiNotesTutorial.Models;

internal class AllNotes
{
    public ObservableCollection<Note> Notes { get; set; } = [];

    public AllNotes() => LoadNotes();

    public void LoadNotes()
    {
        Notes.Clear();
        string appDataPath = FileSystem.AppDataDirectory;
        // Use Linq extensions to load the *.notes.txt files.
        IEnumerable<Note> notes = Directory
                                    .EnumerateFiles(appDataPath, "*.notes.txt")
                                    .Select(filename => new Note()
                                    {
                                        Filename = filename,
                                        Text = File.ReadAllText(filename),
                                        Date = File.GetLastWriteTime(filename)
                                    })
                                    .OrderBy(note => note.Date);
        foreach (Note note in notes)
            Notes.Add(note);
    }
}
