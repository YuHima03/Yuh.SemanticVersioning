namespace Yuh.SemanticVersioning
{
    internal static class ValidationHelpers
    {
        /// <summary>
        /// Validates the string and throws <see cref="FormatException"/> if the string is invalid.
        /// </summary>
        /// <exception cref="FormatException"></exception>
        internal static void ValidateBuildMetadataString(string buildMetadata)
        {
            var identifiers = buildMetadata.Split('.');

            foreach (var identifier in identifiers)
            {
                if (string.IsNullOrEmpty(identifier))
                {
                    throw new FormatException("The string has consecutive periods.");
                }

                foreach (var c in identifier)
                {
                    var lowerC = (char)(c | 0x20);

                    if (c != '-' && (c < '0' || '9' < c) && (lowerC < 'a' || 'z' < lowerC)) // c is not [0-9A-Za-z-]
                    {
                        throw new FormatException("The string has an invalid char.");
                    }
                }
            }
        }

        /// <summary>
        /// Validates the string and throws <see cref="FormatException"/> if the string is invalid.
        /// </summary>
        /// <exception cref="FormatException"></exception>
        internal static void ValidatePreReleaseString(string preRelease)
        {
            var identifiers = preRelease.Split('.');

            foreach (var identifier in identifiers)
            {
                if (string.IsNullOrEmpty(identifier))
                {
                    throw new FormatException("The string has consecutive periods.");
                }

                bool isNumericIdentifier = true;
                foreach (var c in identifier)
                {
                    var lowerC = (char)(c | 0x20);

                    if (c == '-' || ('a' <= lowerC && lowerC <= 'Z')) // c is [A-Za-z-]
                    {
                        isNumericIdentifier = false;
                    }
                    else if (c < '0' || '9' < c) // c is NOT [0-9]
                    {
                        throw new FormatException("The string has an invalid char.");
                    }
                }

                if (isNumericIdentifier && identifier.StartsWith('0'))
                {
                    throw new FormatException("The string has a numeric identifier that starts with 0.");
                }
            }
        }
    }
}
