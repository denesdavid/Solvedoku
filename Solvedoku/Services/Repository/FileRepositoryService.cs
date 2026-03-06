using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Solvedoku.Classes;

namespace Solvedoku.Services.Repository
{
    public class FileRepositoryService : IRepositoryService<SudokuFile>
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true,
            IncludeFields = true,
            ReferenceHandler = ReferenceHandler.Preserve,
            Converters = { new JsonStringEnumConverter() }
        };

        /// <summary>
        /// Loads any SudokuFile from disk, automatically detecting the correct derived type.
        /// </summary>
        public SudokuFile Load(string path)
        {
            var json = File.ReadAllText(path);
            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("Type", out var typeElement))
                throw new InvalidOperationException("Invalid file: missing type information.");

            string? typeName = typeElement.GetString();
            if (typeName is null)
                throw new InvalidOperationException("Invalid file: type name is null.");

            // Use reflection to get the correct Type object
            var type = Type.GetType(typeName, throwOnError: true)!;

            var dataJson = doc.RootElement.GetProperty("Data").GetRawText();
            var obj = (SudokuFile?)JsonSerializer.Deserialize(dataJson, type, JsonOptions);

            if (obj == null)
                throw new InvalidOperationException("Failed to deserialize Sudoku file.");

            return obj;
        }

        /// <summary>
        /// Saves any SudokuFile (including derived types) with type metadata.
        /// </summary>
        public void Save(SudokuFile sudokuFile, string path)
        {
            var wrapper = new
            {
                Type = sudokuFile.GetType().AssemblyQualifiedName, // Full name + assembly for reflection lookup
                Data = sudokuFile
            };

            var json = JsonSerializer.Serialize(wrapper, JsonOptions);
            File.WriteAllText(path, json);
        }
    }
}
