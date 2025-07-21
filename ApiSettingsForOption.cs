using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OptionPatternConfig.ApiSettings;

public sealed class ApiSettingsForOption
{

    internal const string SectionName = $"{nameof(ApiSettingsForOption)}";

    [Required]
    public required Uri BaseUrl { get; set; }

    [Required]

    public required string ApiKey { get; set; }

    public string Version { get; set; } = "v1";
}
