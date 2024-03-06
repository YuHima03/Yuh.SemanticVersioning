using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Yuh.SemanticVersioning
{
    /// <summary>
    /// Represents a version that follows <seealso href="https://semver.org/spec/v2.0.0.html">Semantic-Versioning-2.0.0</seealso>.
    /// </summary>
    public readonly struct SemanticVersion
#if NET7_0_OR_GREATER
        : IComparable, IComparable<SemanticVersion>, IComparisonOperators<SemanticVersion, SemanticVersion, bool>, IEqualityOperators<SemanticVersion, SemanticVersion, bool>, IEquatable<SemanticVersion>, IParsable<SemanticVersion>, ISpanParsable<SemanticVersion>
#else
        : IEquatable<SemanticVersion>
#endif
    {
        private readonly int _major;
        private readonly int _minor;
        private readonly int _patch;
        private readonly string _preRelease = "";
        private readonly string _build = "";

        /// <summary>
        /// Gets the major version.
        /// </summary>
        /// <returns>
        /// The major version.
        /// </returns>
        public int Major
        {
            get => _major;

            private init
            {
                ThrowHelpers.ThrowIfArgumentIsNegative(value);
                _major = value;
            }
        }

        /// <summary>
        /// Gets the minor version.
        /// </summary>
        /// <returns>
        /// The minor version.
        /// </returns>
        public int Minor
        {
            get => _minor;

            private init
            {
                ThrowHelpers.ThrowIfArgumentIsNegative(value);
                _minor = value;
            }
        }

        /// <summary>
        /// Gets the patch version.
        /// </summary>
        /// <returns>
        /// The patch version.
        /// </returns>
        public int Patch
        {
            get => _patch;

            private init
            {
                ThrowHelpers.ThrowIfArgumentIsNegative(value);
                _patch = value;
            }
        }

        /// <summary>
        /// Gets the pre-release version.
        /// </summary>
        /// <returns>
        /// The pre-release version.
        /// </returns>
        public string PreRelease
        {
            get => _preRelease;

            private init
            {
                ArgumentNullException.ThrowIfNull(value);
                if (value.Length > 0)
                {
                    ValidationHelpers.ValidatePreReleaseString(value);
                }
                _preRelease = value;
            }
        }

        /// <summary>
        /// Gets the build metadata.
        /// </summary>
        /// <returns>
        /// The build metadata.
        /// </returns>
        public string Build
        {
            get => _build;

            private init
            {
                ArgumentNullException.ThrowIfNull(value);
                if (value.Length > 0)
                {
                    ValidationHelpers.ValidateBuildMetadataString(value);
                }
                _build = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SemanticVersion"/> class that has specified major, minor, patch versions, and optionally pre-release version and build metadata.
        /// </summary>
        /// <param name="major">Major version.</param>
        /// <param name="minor">Minor version.</param>
        /// <param name="patch">Patch version.</param>
        /// <param name="preRelease">A pre-release version.</param>
        /// <param name="build">Build metadata.</param>
        public SemanticVersion(int major, int minor, int patch, string preRelease = "", string build = "")
        {
            Major = major;
            Minor = minor;
            Patch = patch;
            PreRelease = preRelease;
            Build = build;
        }

        /// <summary>
        /// Compares the <see cref="SemanticVersion"/> with another one and returns a integer that indicates whether the <see cref="SemanticVersion"/> precedes, follows or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">An object to compare with the <see cref="SemanticVersion"/>.</param>
        /// <returns><inheritdoc/></returns>
        public int CompareTo(SemanticVersion other)
        {
            return (this == other) switch
            {
                true => 0,
                false => (this < other) ? -1 : 1
            };
        }

        /// <summary>
        /// Compares the <see cref="SemanticVersion"/> with another object and returns a integer that indicates whether the <see cref="SemanticVersion"/> precedes, follows or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with the <see cref="SemanticVersion"/>.</param>
        /// <returns><inheritdoc/></returns>
        /// <exception cref="ArgumentException"><paramref name="obj"/> is not the same type of the <see cref="SemanticVersion"/>.</exception>
        public int CompareTo(object? obj)
        {
            if (obj is SemanticVersion other)
            {
                return CompareTo(other);
            }
            else
            {
                ThrowHelpers.ThrowArgumentException("The specified object is not the same type of this instance.");
                return 1;
            }
        }

        /// <summary>
        /// Returns a value that indicates whether the <see cref="SemanticVersion"/> is equal to the another one.
        /// </summary>
        /// <param name="other">An object to compare with the <see cref="SemanticVersion"/>.</param>
        /// <returns><see langword="true"/> if the <see cref="SemanticVersion"/> is equal to the another one; otherwise, <see langword="false"/>.</returns>
        public bool Equals(SemanticVersion other)
        {
            return this == other;
        }

        /// <summary>
        /// Returns a value that indicates whether the <see cref="SemanticVersion"/> is equal to the specified object. 
        /// </summary>
        /// <param name="obj">An object to compare with the <see cref="SemanticVersion"/>.</param>
        /// <returns><see langword="true"/> if the <see cref="SemanticVersion"/> is equal to the <paramref name="obj"/>; otherwise, <see langword="false"/>.</returns>
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return (obj is SemanticVersion semVer) && Equals(semVer);
        }

        /// <summary>
        /// Returns the hash code for the <see cref="SemanticVersion"/>.
        /// </summary>
        /// <returns>A 32-bit integer that is the hash code for the <see cref="SemanticVersion"/>.</returns>
        public override int GetHashCode()
        {
            return _major.GetHashCode() ^ _minor.GetHashCode() ^ _patch.GetHashCode() ^ _preRelease.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the <see cref="SemanticVersion"/>.
        /// </summary>
        /// <returns>A string that represents the <see cref="SemanticVersion"/>.</returns>
        public override string ToString()
        {
            DefaultInterpolatedStringHandler handler = new(4, 5);
            handler.AppendFormatted(_major);
            handler.AppendLiteral(".");
            handler.AppendFormatted(_minor);
            handler.AppendLiteral(".");
            handler.AppendFormatted(_patch);

            if (_preRelease.Length > 0)
            {
                handler.AppendLiteral("-");
                handler.AppendFormatted(_preRelease);
            }

            if (_build.Length > 0)
            {
                handler.AppendLiteral("+");
                handler.AppendFormatted(_build);
            }

            return handler.ToString();
        }

        /// <summary>
        /// Parses a span of characters into a <see cref="SemanticVersion"/> and returns it.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The result of parsing <paramref name="s"/>.</returns>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="OverflowException"></exception>
        public static SemanticVersion Parse(ReadOnlySpan<char> s)
        {
            Span<int> idx = [-1, -1, -1, -1];  // {. . - +} の位置 

            for (int i = 0; i < s.Length; i++)
            {
                switch (s[i])
                {
                    case '.':
                    {
                        if (idx[1] == -1)
                        {
                            if (idx[0] == -1)
                            {
                                idx[0] = i;
                            }
                            else
                            {
                                idx[1] = i;
                            }
                        }
                        break;
                    }

                    case '-':
                    {
                        if (idx[2] == idx[3])  // same as (idx[2] == -1 && idx[3] == -1)
                        {
                            idx[2] = i;
                        }
                        break;
                    }

                    case '+':
                    {
                        if (idx[3] == -1)
                        {
                            idx[3] = i;
                        }
                        break;
                    }
                }
            }

            if (idx[0] == -1 || idx[1] == -1)
            {
                ThrowHelpers.ThrowFormatException("The string must consist of at least a major version, a minor version and a patch version separated by periods.");
            }

            int major = int.Parse(s[..idx[0]]);
            int minor = int.Parse(s[(idx[0] + 1)..idx[1]]);

            return idx.Slice(2, 2) switch
            {
                [-1, -1] => new(major, minor, int.Parse(s[(idx[1] + 1)..])),
                [_, -1] => new(major, minor, int.Parse(s[(idx[1] + 1)..idx[2]]), s[(idx[2] + 1)..].ToString()),
                [-1, _] => new(major, minor, int.Parse(s[(idx[1] + 1)..idx[3]]), "", s[(idx[3] + 1)..].ToString()),
                _ => new(major, minor, int.Parse(s[(idx[1] + 1)..idx[2]]), s[(idx[2] + 1)..idx[3]].ToString(), s[(idx[3] + 1)..].ToString())
            };
        }

        /// <summary>
        /// Parses a string into a <see cref="SemanticVersion"/> and returns it.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The result of parsing <paramref name="s"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is null.</exception>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="OverflowException"></exception>
        public static SemanticVersion Parse(string s)
        {
            ArgumentNullException.ThrowIfNull(s);
            return Parse(s.AsSpan());
        }

        public static bool TryParse(ReadOnlySpan<char> s, out SemanticVersion result)
        {
            try
            {
                result = Parse(s);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }

        public static bool TryParse([NotNullWhen(true)] string? s, out SemanticVersion result)
        {
            return TryParse(s.AsSpan(), out result);
        }

#if NET7_0_OR_GREATER
        static SemanticVersion IParsable<SemanticVersion>.Parse(string s, IFormatProvider? provider) => Parse(s);
        static bool IParsable<SemanticVersion>.TryParse(string? s, IFormatProvider? provider, out SemanticVersion result) => TryParse(s, out result);
        static SemanticVersion ISpanParsable<SemanticVersion>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => Parse(s);
        static bool ISpanParsable<SemanticVersion>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out SemanticVersion result) => TryParse(s, out result);
#endif

        /// <inheritdoc/>
        public static bool operator ==(SemanticVersion l, SemanticVersion r)
        {
            return l._major == r._major
                && l._minor == r._minor
                && l._patch == r._patch
                && l._preRelease == r._preRelease;
        }

        /// <inheritdoc/>
        public static bool operator !=(SemanticVersion l, SemanticVersion r)
        {
            return !(l == r);
        }

        /// <inheritdoc/>
        public static bool operator <(SemanticVersion l, SemanticVersion r)
        {
            if (l._major != r._major)
                return l._major < r._major;

            if (l._minor != r._minor)
                return l._minor < r._minor;

            if (l._patch != r._patch)
                return l._patch < r._patch;

            if (l._preRelease.Length == 0)
            {
                return false;
            }
            else if (r._preRelease.Length == 0)
            {
                return true;
            }
            else if (l._preRelease == r._preRelease)
            {
                return false;
            }
            else {
                var l_split_preRelease = l._preRelease.Split('.');
                var r_split_preRelease = r._preRelease.Split('.');

                int minLength = Math.Min(l_split_preRelease.Length, r_split_preRelease.Length);
                for (int i = 0; i < minLength; i++)
                {
                    var l_str = l_split_preRelease[i];
                    var r_str = r_split_preRelease[i];

                    var strComp = string.CompareOrdinal(l_str, r_str);
                    if (strComp == 0)
                    {
                        continue;
                    }

                    bool l_parseRes = BigInteger.TryParse(l_str, out var l_num);
                    bool r_parseRes = BigInteger.TryParse(r_str, out var r_num);

                    return (l_parseRes == r_parseRes) switch
                    {
                        true => l_parseRes ? (l_num < r_num) : (strComp < 0),
                        false => l_parseRes
                    };
                }

                // in any circumstance, the following line is unreachable.
                return false;
            }
        }

        /// <inheritdoc/>
        public static bool operator <=(SemanticVersion l, SemanticVersion r)
        {
            return l < r || l == r;
        }

        /// <inheritdoc/>
        public static bool operator >(SemanticVersion l, SemanticVersion r)
        {
            return !(l <= r);
        }

        /// <inheritdoc/>
        public static bool operator >=(SemanticVersion l, SemanticVersion r)
        {
            return !(l < r);
        }
    }
}
