namespace BTD6E_Module_Helper;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = true)]
public sealed class HelpedAttribute : Attribute {
    private readonly string str1;

    public HelpedAttribute(string message) { str1 = message; }
}