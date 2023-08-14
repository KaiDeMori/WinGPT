namespace WinGPT;

internal class TulpaDirectoryWatcher {
   private readonly FileSystemWatcher          FileSystemWatcher;
   private readonly Action<List<Tulpa>, Tulpa> CreateTulpaButtons;

   public TulpaDirectoryWatcher(Action<List<Tulpa>, Tulpa> createTulpaButtons) {
      this.CreateTulpaButtons = createTulpaButtons;

      Update_Tulpas_and_Buttons();

      FileSystemWatcher = new FileSystemWatcher {
         Path                  = Config.Tulpa_Directory.FullName,
         Filter                = Config.marf278down_filter,
         NotifyFilter          = NotifyFilters.FileName | NotifyFilters.LastWrite,
         IncludeSubdirectories = false
      };

      FileSystemWatcher.Created += (_, _) => Update_Tulpas_and_Buttons();
      FileSystemWatcher.Changed += (_, _) => Update_Tulpas_and_Buttons();
      FileSystemWatcher.Deleted += (_, _) => Update_Tulpas_and_Buttons();
      FileSystemWatcher.Renamed += (_, _) => Update_Tulpas_and_Buttons();
      //fileSystemWatcher.Error   += OnError;

      FileSystemWatcher.EnableRaisingEvents = true;
   }

   private void Update_Tulpas_and_Buttons() {
      var tulpas = ReadAllTulpas();

      Tulpa selected_tulpa = tulpas.FirstOrDefault(tulpa =>
                                tulpa.File?.Name == Config.Active.LastUsedTulpa ||
                                tulpa.File?.Name == Config.DefaultAssistant_Filename)
                             ?? tulpas.First();

      CreateTulpaButtons(tulpas, selected_tulpa);
   }

   private List<Tulpa> ReadAllTulpas() {
      //read all *.md files in the tulpas directory
      var files  = Config.Tulpa_Directory.GetFiles(Config.marf278down_filter);
      var tulpas = files.Select(Tulpa.CreateFrom).NotNull().ToList();
      return tulpas;
   }


   private void OnSomethingChanged(object sender, FileSystemEventArgs e) {
      var tulpas = ReadAllTulpas();
      CreateTulpaButtons(tulpas, null);
   }
}