using System.Text.Json;
using System.Text.RegularExpressions;
using APBD11.Models;

namespace APBD11.Middleware;

public class AdditionalPropertiesValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AdditionalPropertiesValidationMiddleware> _logger;
    private readonly ValidationRuleSet _rules;

    public AdditionalPropertiesValidationMiddleware(
        RequestDelegate next,
        ILogger<AdditionalPropertiesValidationMiddleware> logger,
        ValidationRuleSet rules)
    {
        _next = next;
        _logger = logger;
        _rules = rules;
    }

    public async Task InvokeAsync(HttpContext ctx)
    {
        if (ctx.Request.Path.StartsWithSegments("/api/devices") &&
            (ctx.Request.Method == "POST" || ctx.Request.Method == "PUT"))
        {
            _logger.LogInformation("Starting AdditionalProperties validation");
            ctx.Request.EnableBuffering();
            using var doc = await JsonDocument.ParseAsync(ctx.Request.Body);
            ctx.Request.Body.Position = 0;

            var root = doc.RootElement;
            var type = root.GetProperty("deviceType").GetString()!;
            var isEnabled = root.GetProperty("isEnabled").GetBoolean().ToString().ToLower();

            var matching = _rules.Validations
                                 .FirstOrDefault(r => r.Type == type
                                                   && r.PreRequestName == "isEnabled"
                                                   && r.PreRequestValue == isEnabled);
            if (matching != null)
            {
                var addProps = root.GetProperty("additionalProperties");
                foreach (var rule in matching.Rules)
                {
                    if (!addProps.TryGetProperty(rule.ParamName, out var prop))
                        throw new ArgumentException($"Missing `{rule.ParamName}` in additionalProperties");

                    var value = prop.GetRawText().Trim('"');

                    if (rule.Regex.ValueKind == JsonValueKind.String)
                    {
                        var pattern = rule.Regex.GetString()!;
                        if (!Regex.IsMatch(value, pattern))
                            throw new ArgumentException($"{rule.ParamName} does not match `{pattern}`");
                    }
                    else if (rule.Regex.ValueKind == JsonValueKind.Array)
                    {
                        var allowed = rule.Regex.EnumerateArray().Select(e => e.GetString()!).ToList();
                        if (!allowed.Contains(value))
                            throw new ArgumentException($"{rule.ParamName} must be one of [{string.Join(", ", allowed)}]");
                    }
                }
            }

            _logger.LogInformation("AdditionalProperties validation succeeded");
        }

        await _next(ctx);
    }
}