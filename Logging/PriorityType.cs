namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class ExtensionAttribute : Attribute { }
}

namespace Logging
{
    public enum PriorityType { Emergency = 7, Alert = 6, Critical = 5, Error = 4, Warning = 3, Notice = 2, Informational = 1, Debug = 0}

    public static class PriorityTypeToString
    {
        public static string Name(this PriorityType prio)
        {
            switch (prio)
            {
                case PriorityType.Alert:
                    return "Alert ";
                case PriorityType.Critical:
                    return "Critic";
                case PriorityType.Debug:
                    return "Debug ";
                case PriorityType.Emergency:
                    return "Emerg ";
                case PriorityType.Error:
                    return "Error ";
                case PriorityType.Informational:
                    return "Info  ";
                case PriorityType.Notice:
                    return "Notice";
                case PriorityType.Warning:
                    return "Warn'g";
            }
            return "Unknown";
        }
    }
}
