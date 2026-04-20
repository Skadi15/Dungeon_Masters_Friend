using Dungeon_Masters_Friend.Models;
using Dungeon_Masters_Friend.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;

namespace Dungeon_Masters_Friend.Repositories
{
    /// <inheritdoc cref="IBestiaryRepository"/>
    public class BestiaryRepository : IBestiaryRepository
    {
        private static readonly JsonSerializerOptions WriteOptions = new() { WriteIndented = true };
        private static readonly JsonSerializerOptions ReadOptions = new() { PropertyNameCaseInsensitive = true };

        private static List<Creature>? s_cache;
        private static readonly Lock s_cacheLock = new();

        private readonly string _filePath;

        public BestiaryRepository()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            // Fallback to the application's base directory if special folder is unavailable
            if (string.IsNullOrEmpty(folder))
            {
                folder = AppContext.BaseDirectory ?? Directory.GetCurrentDirectory();
            }

            folder = Path.Combine(folder, Constants.AppDataFolderName);

            // Ensure the directory exists
            Directory.CreateDirectory(folder);

            _filePath = Path.Combine(folder, Constants.BestiaryFileName);
        }

        public async Task<List<Creature>> LoadAsync()
        {
            if (s_cache != null)
            {
                return s_cache;
            }

            List<Creature> loaded;
            if (!File.Exists(_filePath))
            {
                loaded = [];
            }
            else
            {
                try
                {
                    await using var stream = File.OpenRead(_filePath);
                    var result = await JsonSerializer.DeserializeAsync<List<Creature>>(stream, ReadOptions);
                    loaded = result ?? [];
                }
                catch
                {
                    // On any deserialization/IO error return an empty list rather than throwing to keep repository usage simple.
                    loaded = [];
                }
            }

            loaded = [.. loaded.OrderBy(c => c.Name ?? string.Empty, StringComparer.OrdinalIgnoreCase)];

            lock (s_cacheLock)
            {
                s_cache ??= loaded;
            }

            return s_cache;
        }

        public async Task SaveAsync(IEnumerable<Creature> creatures)
        {
            var list = (creatures ?? []).OrderBy(c => c.Name ?? string.Empty, StringComparer.OrdinalIgnoreCase).ToList();

            lock (s_cacheLock)
            {
                s_cache = list;
            }

            // Serialize to a temporary file and then move to final path to avoid corrupting the saved file on failure.
            var tempPath = _filePath + ".tmp";

            try
            {
                await using (var stream = File.Create(tempPath))
                {
                    await JsonSerializer.SerializeAsync(stream, list, WriteOptions);
                    await stream.FlushAsync();
                }

                File.Move(tempPath, _filePath, overwrite: true);
            }
            catch
            {
                // Swallow IO/serialization errors to keep repository usage simple. Cache remains updated.
            }
        }
    }

    /// <summary>
    /// Stores and retrieves the bestiary data from disk. The bestiary is a collection of creatures that the user has created or imported.
    /// </summary>
    public interface IBestiaryRepository
    {
        /// <summary>
        /// Loads the bestiary from disk. If no file exists an empty list is returned.
        /// </summary>
        /// <returns>A list of creatures from the bestiary file, or an empty list if the file doesn't exist or fails to load.</returns>
        Task<List<Creature>> LoadAsync();

        /// <summary>
        /// Saves the given creatures to disk, overwriting any existing bestiary file.
        /// </summary>
        /// <param name="creatures">The collection of creatures to save to the bestiary file.</param>
        /// <returns>A task representing the asynchronous save operation.</returns>
        Task SaveAsync(IEnumerable<Creature> creatures);
    }
}
