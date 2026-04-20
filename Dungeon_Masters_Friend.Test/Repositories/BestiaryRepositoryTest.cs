using Dungeon_Masters_Friend.Models;
using Dungeon_Masters_Friend.Repositories;
using Dungeon_Masters_Friend.Utilities;
using System.Reflection;
using System.Text.Json;

namespace Dungeon_Masters_Friend.Test.Repositories
{
    public class BestiaryRepositoryTest
    {
        private readonly BestiaryRepository _repository;

        public BestiaryRepositoryTest()
        {
            _repository = new BestiaryRepository();
        }

        [Fact]
        public async Task LoadAsync()
        {
            ClearCache();

            // Prepare file with unsorted creatures
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            if (string.IsNullOrEmpty(folder)) folder = AppContext.BaseDirectory ?? Directory.GetCurrentDirectory();
            var dataFolder = Path.Combine(folder, Constants.AppDataFolderName);
            Directory.CreateDirectory(dataFolder);
            var filePath = Path.Combine(dataFolder, Constants.BestiaryFileName);

            var models = new List<Creature>
            {
                new() { Name = "Zombie" },
                new() { Name = "Aaxolotl" }
            };

            await File.WriteAllTextAsync(filePath, JsonSerializer.Serialize(models));

            try
            {
                var loaded = await _repository.LoadAsync();

                Assert.Equal(2, loaded.Count);
                Assert.Equal(["Aaxolotl", "Zombie"], loaded.Select(c => c.Name));
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [Fact]
        public async Task LoadAsync_CacheHit()
        {
            ClearCache();

            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            if (string.IsNullOrEmpty(folder)) folder = AppContext.BaseDirectory ?? Directory.GetCurrentDirectory();
            var dataFolder = Path.Combine(folder, Constants.AppDataFolderName);
            Directory.CreateDirectory(dataFolder);
            var filePath = Path.Combine(dataFolder, Constants.BestiaryFileName);

            var firstModels = new List<Creature> { new() { Name = "Cached" } };
            await File.WriteAllTextAsync(filePath, JsonSerializer.Serialize(firstModels));

            try
            {
                var repo1 = new BestiaryRepository();
                var loaded1 = await repo1.LoadAsync();

                // Now replace file contents
                var otherModels = new List<Creature> { new() { Name = "New" } };
                await File.WriteAllTextAsync(filePath, JsonSerializer.Serialize(otherModels));

                var repo2 = new BestiaryRepository();
                var loaded2 = await repo2.LoadAsync();

                Assert.Equal(loaded1, loaded2);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [Fact]
        public async Task LoadAsync_FileDoesNotExist()
        {
            ClearCache();

            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            if (string.IsNullOrEmpty(folder)) folder = AppContext.BaseDirectory ?? Directory.GetCurrentDirectory();
            var dataFolder = Path.Combine(folder, Constants.AppDataFolderName);
            Directory.CreateDirectory(dataFolder);
            var filePath = Path.Combine(dataFolder, Constants.BestiaryFileName);

            if (File.Exists(filePath)) File.Delete(filePath);

            var loaded = await _repository.LoadAsync();

            Assert.Empty(loaded);
        }

        [Fact]
        public async Task LoadAsync_JsonDeserializationError()
        {
            ClearCache();

            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            if (string.IsNullOrEmpty(folder)) folder = AppContext.BaseDirectory ?? Directory.GetCurrentDirectory();
            var dataFolder = Path.Combine(folder, Constants.AppDataFolderName);
            Directory.CreateDirectory(dataFolder);
            var filePath = Path.Combine(dataFolder, Constants.BestiaryFileName);

            await File.WriteAllTextAsync(filePath, "this is not json");

            try
            {
                var loaded = await _repository.LoadAsync();

                Assert.Empty(loaded);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [Fact]
        public async Task SaveAsync()
        {
            ClearCache();

            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            if (string.IsNullOrEmpty(folder)) folder = AppContext.BaseDirectory ?? Directory.GetCurrentDirectory();
            var dataFolder = Path.Combine(folder, Constants.AppDataFolderName);
            Directory.CreateDirectory(dataFolder);
            var filePath = Path.Combine(dataFolder, Constants.BestiaryFileName);

            if (File.Exists(filePath)) File.Delete(filePath);

            var models = new List<Creature>
            {
                new() { Name = "Zombie" },
                new() { Name = "Aaxolotl" }
            };

            await _repository.SaveAsync(models);

            // File should exist and contain sorted entries
            var text = await File.ReadAllTextAsync(filePath);
            Assert.Contains("Aaxolotl", text);
            Assert.Contains("Zombie", text);

            var loaded = await _repository.LoadAsync();
            Assert.Equal(2, loaded.Count);
            Assert.Equal(["Aaxolotl", "Zombie"], loaded.Select(c => c.Name));

            File.Delete(filePath);
        }

        [Fact]
        public async Task SaveAsync_IOError()
        {
            ClearCache();

            // Create a file that will act as a "directory" to provoke an IO error when repository attempts to write inside it.
            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".file");
            await File.WriteAllTextAsync(tempFile, "x");

            var repo = new BestiaryRepository();

            // Replace the private readonly _filePath to point under the tempFile (so the path includes a file as a directory)
            var filePathField = typeof(BestiaryRepository).GetField("_filePath", BindingFlags.Instance | BindingFlags.NonPublic);
            var badPath = Path.Combine(tempFile, Constants.BestiaryFileName);
            filePathField.SetValue(repo, badPath);

            var models = new List<Creature> { new() { Name = "Creature 1" } };

            // Should not throw despite IO error; cache should be updated
            await repo.SaveAsync(models);

            var loaded = await repo.LoadAsync();

            Assert.Single(loaded);
            Assert.Equal("Creature 1", loaded[0].Name);

            File.Delete(tempFile);
        }

        private static void ClearCache()
        {
            var f = typeof(BestiaryRepository).GetField("s_cache", BindingFlags.Static | BindingFlags.NonPublic);
            f.SetValue(null, null);
        }
    }
}
