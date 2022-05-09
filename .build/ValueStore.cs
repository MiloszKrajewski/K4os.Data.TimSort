#nullable enable

using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class ValueStore
{
	private static readonly JsonSerializerOptions Settings = new() {
		WriteIndented = true,
		AllowTrailingCommas = true,
	};

	private Dictionary<string, string?> Values = Empty();
	private readonly string FileName;

	public string? this[string key]
	{
		get => Values.TryGetValue(key, out var result) ? result : null;
		set => Values[key] = value;
	}

	public ValueStore(string fileName)
	{
		FileName = fileName;
		Rollback();
	}
	
	private static Dictionary<string, string?> Empty() => new();

	public void Rollback() =>
		Values = (File.Exists(FileName) ? Deserialize(FileName) : null) ?? Empty();

	public void Commit() => 
		Serialize(FileName);

	void Serialize(string fileName) =>
		File.WriteAllText(fileName, JsonSerializer.Serialize(Values, Settings));

	static Dictionary<string, string?>? Deserialize(string fileName) =>
		JsonSerializer.Deserialize<Dictionary<string, string?>>(
			File.ReadAllText(fileName), Settings);
}
