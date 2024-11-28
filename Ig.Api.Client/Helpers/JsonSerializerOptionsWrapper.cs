using System.Text.Json;

namespace Ig.Api.Client.Helpers;

public static class JsonSerializerOptionsWrapper
{
    public static JsonSerializerOptions Options { get; } = new(JsonSerializerDefaults.Web);
}