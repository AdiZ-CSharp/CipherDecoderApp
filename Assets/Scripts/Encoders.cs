using System;
using System.Collections.Generic;

public class Encoders
{
    // Pre-defining the ordered alphabets and integers for ease of access in all functions that need it

    // readonly variables may be written to through Constructors
    readonly char[] upperAlphabet;
    readonly char[] lowerAlphabet;

    public Encoders()
    {
        // Fetch lists from ConstantVars.cs to ensure that it is fully updated.
        upperAlphabet = ConstantVars.upperAlphabet;
        lowerAlphabet = ConstantVars.lowerAlphabet;
    }

    #region Generic Methods
    string EncodeStringWithAlphabetDictionaries(string plaintext, Dictionary<char, char> upperAlphabetDict, Dictionary<char, char> lowerAlphabetDict)
    {
        if (string.IsNullOrEmpty(plaintext))
        {
            return "";
        }

        string str = "";
        
        foreach (char ch in plaintext)
        {
            if (Char.IsLower(ch))
            {
                // Takes the index of the char in the plaintext string, adds that (also works with negative numbers) to the index,
                // and adds the new char in the array to 'str'
                char newCh = lowerAlphabetDict[ch];
                str += newCh;
                continue;
            }

            if (Char.IsUpper(ch))
            {
                // Same as above but with a different array
                char newCh = upperAlphabetDict[ch];
                str += newCh;
                continue;
            }

            // If the character is not an alphabetical character, add it without shuffling it.
            str += ch;
        }

        return str;
    }
    #endregion

    #region Cipher Encoders
    public string CaesarCipher(string plaintext, int shuffle)
    {
        if (string.IsNullOrEmpty(plaintext))
        {
            return "";
        }
        
        string encodedStr = "";
        shuffle %= 26;
        foreach(char ch in plaintext)
        {
            if (Char.IsLower(ch))
            {
                // Same as above but with a different array
                char newCh = lowerAlphabet[(Array.IndexOf(lowerAlphabet, ch) + shuffle) % lowerAlphabet.Length];
                encodedStr += newCh;
                continue;
            }

            if (Char.IsUpper(ch))
            {
                // Takes the index of the char in the plaintext string, adds that (also works with negative numbers) to the index,
                // and adds the new char in the array to 'encodedStr'
                char newCh = upperAlphabet[(Array.IndexOf(upperAlphabet, ch) + shuffle) % upperAlphabet.Length];
                encodedStr += newCh;
                continue;
            }
            
            // If the character is not an alphabetical character, add it without shuffling it.
            encodedStr += ch;
        }

        return encodedStr;
    }

    public string MonoalphabeticCipher(string plaintext, string key)
    {
        // Empty strings return nothing
        if (string.IsNullOrEmpty(plaintext))
        {
            return "";
        }

        // Ensures that the key provided covers the full alphabet
        if (key.Length != 26)
        {
            return "Key is not 26 letters.";
        }

        // Ensures that the key provided has no non-alphabetical characters
        foreach(char ch in key)
        {
            if(!char.IsLetter(ch))
            {
                return "The key must be all letters.";
            }
        }

        string plaintextLower = plaintext.ToLower();
        string plaintextUpper = plaintext.ToUpper();

        // This ensures that the user-entered key in case-insensitive
        string keyLower = key.ToLower();
        string keyUpper = key.ToUpper();
        string encodedStr = "";

        Dictionary<char, char> keyValueDict = new();

        // Generates a dictionary containing upper and lowercase values for each of the
        // 52 possible letters (26 * 2 cases (upper and lower))
        for (int i = 0; i <= 25; i++)
        {
            keyValueDict.Add(lowerAlphabet[i], keyLower[i]);
            keyValueDict.Add(upperAlphabet[i], keyUpper[i]);
        }

        foreach(char ch in plaintext)
        {
            if (Char.IsLetter(ch))
            {
                char newCh = keyValueDict[ch];
                encodedStr += newCh;
                continue;
            }

            encodedStr += ch;
        }

        return encodedStr;
    }

    public string Rot13Cipher(string plaintext)
    {
        return EncodeStringWithAlphabetDictionaries(plaintext, ConstantVars.rot13Upper, ConstantVars.rot13Lower);
    }

    public string AtbashCipher(string plaintext)
    {
        return EncodeStringWithAlphabetDictionaries(plaintext, ConstantVars.atbashUpper, ConstantVars.atbashLower);
    }

    public string ReverseText(string plaintext)
    {
        char[] charArray = plaintext.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
    #endregion
}