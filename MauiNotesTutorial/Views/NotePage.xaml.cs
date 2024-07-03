namespace MauiNotesTutorial.Views;

public partial class NotePage : ContentPage
{
	string _fileName = Path.Combine(FileSystem.AppDataDirectory, "notes.txt");

	public NotePage()
	{
		InitializeComponent();
		string appDataPath = FileSystem.AppDataDirectory;
		string randomFileName = $"{Path.GetRandomFileName()}.notes.txt";
		LoadNote(Path.Combine(appDataPath, randomFileName));
	}

	private void SaveButton_Clicked(object sender, EventArgs e)
	{
		File.WriteAllText(_fileName, TextEditor.Text);
	}

	private void DeleteButton_Clicked(object sender, EventArgs e)
	{
		if (File.Exists(_fileName))
			File.Delete(_fileName);
		TextEditor.Text = string.Empty;
	}

	private void LoadNote(string fileName)
	{
        Models.Note note = new()
        {
            Filename = fileName
        };
        if (File.Exists(fileName))
		{
			note.Date = File.GetCreationTime(fileName);
			note.Text = File.ReadAllText(fileName);
		}
		BindingContext = note;
	}
}