﻿using System;
using System.Text;

public static class StringExtensions
{
    public static string WithSpaceBetweenWords(this string s)
    {
        const char space = ' ';
        var sb = new StringBuilder();
        var lastChar = space;
        for (var i = 0; i < s.Length; i++)
        {
            var ch = s[i];
            
            if (lastChar != space // No Double Space
                && char.IsUpper(ch) // Add Space if new word is started
                && i < s.Length - 1 && !char.IsUpper(s[i + 1])) // If contiguous capitals then not a word
            {
                lastChar = space;
                sb.Append(space);
            }

            if (ch == space && lastChar == space) // No Double Space
                continue;
            
            lastChar = ch;
            sb.Append(ch);
        }
        return sb.ToString();
    }

    public static string SkipThroughFirstDash(this string s) => s.Substring(s.IndexOf('-') + 1);
    public static string SkipThroughFirstUnderscore(this string s) => s.Substring(s.IndexOf('_') + 1);
    public static bool ContainsAnyCase(this string s, string term) => s != null && s.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0;
    public static T EnumVal<T>(this string s) => (T)Enum.Parse(typeof(T), s);
    public static Maybe<T> MaybeEnumVal<T>(this string s) where T : struct => Enum.TryParse(s, true, out T r) ? new Maybe<T>(r, true) : Maybe<T>.Missing();
    public static string ToTitleCase(this string s) => System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(s);

    public static string SafeFormat(this string str, params object[] args)
    {
        try
        {
            return string.Format(str, args);
        }
        catch (Exception ex)
        {
            Log.Error(new Exception($"String Format Broken! Original English Text: {str}", ex));
            return str;
        }
    }

    public static string SafeFormatWithDefault(this string str, string backupStr, params object[] args)
    {
        try
        {
            return string.Format(str, args);
        }
        catch (Exception ex)
        {
            Log.Error(new Exception($"String Format Broken! Original English Text: {str}", ex));
            return string.Format(backupStr, args);;
        }
    }
}
