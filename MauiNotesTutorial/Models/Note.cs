namespace MauiNotesTutorial.Models;

public class Note
{
    public string Filename { get; set; }
    public string Text { get; set; }
    public DateTime Date { get; set; }

    public Note()
    {
        Filename = $"{Path.GetRandomFileName()}.notes.txt";
        Date = DateTime.Now;
        Text = "";
    }

    public void Save() =>
        File.WriteAllText(Path.Combine(FileSystem.AppDataDirectory, Filename), Text);

    public void Delete() =>
        File.Delete(Path.Combine(FileSystem.AppDataDirectory, Filename));

    public static Note Load(string filename)
    {
        filename = Path.Combine(FileSystem.AppDataDirectory, filename);

        if (!File.Exists(filename))
            throw new FileNotFoundException("Unable to find file on local storage.", filename);

        return new()
        {
            Filename = Path.GetFileName(filename),
            Text = File.ReadAllText(filename),
            Date = File.GetLastWriteTime(filename)
        };
    }

    public static IEnumerable<Note> LoadAll()
    {
        // Use Linq extensions to load the *.notes.txt files.
        return Directory
                // Get all files that end in ".notes.txt"
                .EnumerateFiles(FileSystem.AppDataDirectory, "*.notes.txt")
                // Equivalent to map
                .Select(filename => Note.Load(Path.GetFileName(filename)))
                .OrderByDescending(note => note.Date);
    }
}
