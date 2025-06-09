using System.Text.Json;

namespace APBD11.Models;

public class ValidationRuleSet
{
    public List<TypeRule> Validations { get; set; } = new();
}

public class TypeRule
{
    public string Type { get; set; } = null!;
    public string PreRequestName { get; set; } = null!;
    public string PreRequestValue { get; set; } = null!;
    public List<PropertyRule> Rules { get; set; } = new();
}

public class PropertyRule
{
    public string ParamName { get; set; } = null!;
    public JsonElement Regex { get; set; }       
}