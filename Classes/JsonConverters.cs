using System.Text.Json;
using System.Text.Json.Serialization;
using System.Numerics;

public class Vector2JsonConverter : JsonConverter<Vector2>
{
    public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        float x = 0, y = 0;
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            string propertyName = reader.GetString()!;
            reader.Read();

            if (propertyName.Equals("X", StringComparison.OrdinalIgnoreCase))
                x = (float)reader.GetDouble();
            else if (propertyName.Equals("Y", StringComparison.OrdinalIgnoreCase))
                y = (float)reader.GetDouble();
        }

        return new Vector2(x, y);
    }

    public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("X", value.X);
        writer.WriteNumber("Y", value.Y);
        writer.WriteEndObject();
    }
}

public class TileMapJsonConverter : JsonConverter<List<List<(int, int)>>>
{
    public override List<List<(int, int)>> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException();

        var tileMap = new List<List<(int, int)>>();

        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();

            var row = new List<(int, int)>();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                if (reader.TokenType != JsonTokenType.StartArray)
                    throw new JsonException();

                reader.Read();
                int num1 = reader.GetInt32();
                reader.Read();
                int num2 = reader.GetInt32();
                reader.Read();

                row.Add((num1, num2));
            }

            tileMap.Add(row);
        }

        return tileMap;
    }

    public override void Write(Utf8JsonWriter writer, List<List<(int, int)>> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        foreach (var row in value)
        {
            writer.WriteStartArray();
            foreach (var (num1, num2) in row)
            {
                writer.WriteStartArray();
                writer.WriteNumberValue(num1);
                writer.WriteNumberValue(num2);
                writer.WriteEndArray();
            }
            writer.WriteEndArray();
        }

        writer.WriteEndArray();
    }
}
