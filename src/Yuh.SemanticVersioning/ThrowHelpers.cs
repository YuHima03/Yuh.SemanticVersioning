using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Yuh.SemanticVersioning
{
    internal static class ThrowHelpers
    {
        [DoesNotReturn]
        internal static void ThrowArgumentException(string? message = null, string? paramName = null, Exception? innerException = null)
        {
            throw new ArgumentException(message, paramName, innerException);
        }

        [DoesNotReturn]
        internal static void ThrowFormatException(string? message = null, Exception? innerException = null)
        {
            throw new FormatException(message, innerException);
        }

        internal static void ThrowIfArgumentIsNegative(int value, [CallerArgumentExpression(nameof(value))] string? argName = null)
        {
#if NET8_0_OR_GREATER
            ArgumentOutOfRangeException.ThrowIfNegative(value, argName);
#else
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(argName, "The value is negative.");
            }
#endif
        }

        internal static void ThrowIfArgumentIsEmpty(string value, [CallerArgumentExpression(nameof(value))] string? argName = null)
        {
            if (value.Length == 0)
            {
                throw new ArgumentException("The string is empty.", argName);
            }
        }
    }
}
