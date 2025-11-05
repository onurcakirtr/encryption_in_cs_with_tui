using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices; // Mouse-click GUI için gerekli

namespace ClassicCipherApp
{
    // Tüm CipherOperations metotları daha detaylı konsol çıktıları içerecek şekilde güncellendi.
    #region Cipher Operations
    public class CipherOperations
    {
        // =========================================================================
        // 1. Caesar Cipher (Shift Cipher)
        // =========================================================================
        public static string CaesarCipher(string text, int key, bool encrypt)
        {
            string processedText = Regex.Replace(text.ToUpper(), @"[^A-Z]", "");
            StringBuilder result = new StringBuilder();
            int shift = encrypt ? key : (26 - (key % 26)); // Decryption is just shifting by 26-key
            string mode = encrypt ? "Encrypt" : "Decrypt";

            Console.WriteLine($"\n[Caesar] Mode: {mode}");
            Console.WriteLine($"[Caesar] Processed Text: {processedText}");
            Console.WriteLine($"[Caesar] Key: {key} -> Final Shift: {shift}");
            Console.WriteLine("-----------------------------------");

            foreach (char character in processedText)
            {
                int charValue = character - 'A';
                int shiftedValue = (charValue + shift) % 26;
                if (shiftedValue < 0) shiftedValue += 26;
                
                char mapped = (char)('A' + shiftedValue);
                Console.WriteLine($"[Caesar] Char: '{character}' (Index: {charValue})");
                Console.WriteLine($"         Calculation: ({charValue} + {shift}) % 26 = {shiftedValue}");
                Console.WriteLine($"         Result: Index {shiftedValue} -> '{mapped}'\n");
                result.Append(mapped);
            }
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("[Caesar] Final Result: " + result.ToString());
            return result.ToString();
        }

        // ========================================================================= 
        // 2. Monoalphabetic Substitution Cipher 
        // ========================================================================= 
        public static string MonoalphabeticSubstitution(string text, string key, bool encrypt)
        {
            if (key.Length != 26 || key.ToUpper().Distinct().Count() != 26)
            {
                throw new ArgumentException("Key must contain 26 unique letters.");
            }
            string processedText = Regex.Replace(text.ToUpper(), @"[^A-Z]", "");
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string substitution = key.ToUpper();
            StringBuilder result = new StringBuilder();
            string mode = encrypt ? "Encrypt" : "Decrypt";

            Console.WriteLine($"\n[Mono] Mode: {mode}");
            Console.WriteLine($"[Mono] Processed Text: {processedText}");
            Console.WriteLine($"[Mono] Alphabet:     {alphabet}");
            Console.WriteLine($"[Mono] Substitution: {substitution}");
            Console.WriteLine("-----------------------------------");

            for (int i = 0; i < processedText.Length; i++)
            {
                char plainChar = processedText[i];
                if (plainChar >= 'A' && plainChar <= 'Z')
                {
                    if (encrypt)
                    {
                        int index = alphabet.IndexOf(plainChar);
                        char mapped = substitution[index];
                        Console.WriteLine($"[Mono] Char: '{plainChar}' -> Found at Alphabet index {index}.");
                        Console.WriteLine($"       Mapping: Substitution[{index}] -> '{mapped}'\n");
                        result.Append(mapped);
                    }
                    else
                    {
                        int index = substitution.IndexOf(plainChar);
                        char mapped = alphabet[index];
                        Console.WriteLine($"[Mono] Char: '{plainChar}' -> Found at Substitution index {index}.");
                        Console.WriteLine($"       Mapping: Alphabet[{index}] -> '{mapped}'\n");
                        result.Append(mapped);
                    }
                }
            }
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("[Mono] Final Result: " + result.ToString());
            return result.ToString();
        }

        // ========================================================================= 
        // 3. Vigenere Cipher (Polyalphabetic Cipher) 
        // ========================================================================= 
        public static string VigenereCipher(string text, string key, bool encrypt)
        {
            string processedText = Regex.Replace(text.ToUpper(), @"[^A-Z]", "");
            string processedKey = Regex.Replace(key.ToUpper(), @"[^A-Z]", "");
            if (processedKey.Length == 0)
            {
                throw new ArgumentException("The key cannot be empty.");
            }
            StringBuilder result = new StringBuilder();
            string mode = encrypt ? "Encrypt" : "Decrypt";

            Console.WriteLine($"\n[Vigenere] Mode: {mode}");
            Console.WriteLine($"[Vigenere] Processed Text: {processedText}");
            Console.WriteLine($"[Vigenere] Processed Key: {processedKey}");
            Console.WriteLine("-----------------------------------");

            for (int i = 0; i < processedText.Length; i++)
            {
                char textChar = processedText[i];
                char keyChar = processedKey[i % processedKey.Length];
                int p = textChar - 'A'; // Plaintext index
                int k = keyChar - 'A';  // Key index
                int c; // Ciphertext index

                Console.WriteLine($"[Vigenere] Char: '{textChar}' (P={p}), Key: '{keyChar}' (K={k})");

                if (encrypt)
                {
                    c = (p + k) % 26;
                    Console.WriteLine($"         Encrypt: ({p} + {k}) % 26 = {c}");
                }
                else
                {
                    c = (p - k + 26) % 26; // Add 26 to handle negative numbers
                    Console.WriteLine($"         Decrypt: ({p} - {k} + 26) % 26 = {c}");
                }
                
                char mapped = (char)('A' + c);
                Console.WriteLine($"         Result: Index {c} -> '{mapped}'\n");
                result.Append(mapped);
            }
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("[Vigenere] Final Result: " + result.ToString());
            return result.ToString();
        }

        // ========================================================================= 
        // 4. Simple Columnar Transposition Cipher 
        // ========================================================================= 
        public static string TranspositionCipher(string text, string key, bool encrypt)
        {
            string processedText = Regex.Replace(text.ToUpper(), @"[^A-Z]", "");
            if (!key.All(char.IsDigit) || key.Distinct().Count() != key.Length)
            {
                throw new ArgumentException("Transposition key must be unique digits representing column order (e.g., 2531).");
            }
            int[] keyOrder = key.Select(c => int.Parse(c.ToString())).ToArray();
            int columns = keyOrder.Length;
            int rows = (int)Math.Ceiling((double)processedText.Length / columns);
            char[,] matrix = new char[rows, columns];
            StringBuilder result = new StringBuilder();
            string mode = encrypt ? "Encrypt" : "Decrypt";
            
            Console.WriteLine($"\n[Transposition] Mode: {mode}");
            Console.WriteLine($"[Transposition] Processed Text: {processedText}");
            Console.WriteLine($"[Transposition] Key Order: {string.Join("", keyOrder)}");
            Console.WriteLine($"[Transposition] Grid Size: {rows} Rows x {columns} Columns");
            Console.WriteLine("-----------------------------------");

            if (encrypt)
            {
                Console.WriteLine("[Transposition] Step 1: Filling matrix row by row...");
                int textIndex = 0;
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < columns; c++)
                    {
                        if (textIndex < processedText.Length)
                        {
                            matrix[r, c] = processedText[textIndex++];
                        }
                        else
                        {
                            matrix[r, c] = 'X'; // Pad with 'X'
                        }
                    }
                    Console.WriteLine($"         Row {r}: {new string(Enumerable.Range(0, columns).Select(c => matrix[r, c]).ToArray())}");
                }

                Console.WriteLine("\n[Transposition] Step 2: Reading matrix by key order...");
                foreach (int order in keyOrder.OrderBy(x => x)) // Read in order 1, 2, 3...
                {
                    int colIndex = Array.IndexOf(keyOrder, order); // Find 0-based index of key number
                    
                    Console.Write($"         Reading Column {colIndex} (Key '{order}'): ");
                    for (int r = 0; r < rows; r++)
                    {
                        result.Append(matrix[r, colIndex]);
                        Console.Write(matrix[r, colIndex]);
                    }
                    Console.WriteLine();
                }
            }
            else // Decrypt
            {
                Console.WriteLine("[Transposition] Step 1: Calculating column lengths...");
                int[] colLengths = new int[columns];
                int remainder = processedText.Length % columns;
                int baseLength = processedText.Length / columns;
                for (int c = 0; c < columns; c++) colLengths[c] = baseLength;

                // The first 'remainder' columns (in key order) get an extra char
                var sortedKeyIndices = keyOrder
                    .Select((val, idx) => new { Value = val, Index = idx })
                    .OrderBy(x => x.Value)
                    .Select(x => x.Index)
                    .Take(remainder)
                    .ToList();
                foreach (int originalColIndex in sortedKeyIndices) colLengths[originalColIndex] += 1;
                
                Console.WriteLine($"         Column Lengths (by grid index 0..N): {string.Join(", ", colLengths)}");

                Console.WriteLine("\n[Transposition] Step 2: Filling matrix by key order...");
                int cipherIndex = 0;
                foreach (int order in keyOrder.OrderBy(x => x)) // Fill in order 1, 2, 3...
                {
                    int colIndex = Array.IndexOf(keyOrder, order);
                    int currentColLength = colLengths[colIndex];
                    
                    string colData = processedText.Substring(cipherIndex, currentColLength);
                    Console.WriteLine($"         Writing '{colData}' to Column {colIndex} (Key '{order}')");

                    for (int r = 0; r < rows; r++)
                    {
                        if(r < currentColLength)
                        {
                            matrix[r, colIndex] = processedText[cipherIndex++];
                        }
                        else
                        {
                            matrix[r, colIndex] = '\0'; // Mark as empty
                        }
                    }
                }

                Console.WriteLine("\n[Transposition] Step 3: Reading matrix row by row...");
                for (int r = 0; r < rows; r++)
                {
                    Console.Write($"         Reading Row {r}: ");
                    for (int c = 0; c < columns; c++)
                    {
                        if (matrix[r, c] != '\0')
                        {
                            result.Append(matrix[r, c]);
                            Console.Write(matrix[r, c]);
                        }
                    }
                    Console.WriteLine();
                }
            }
            
            string raw = result.ToString();
            string trimmed = raw.TrimEnd('X'); // Remove padding
            
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("[Transposition] Raw Result (with padding): " + raw);
            Console.WriteLine("[Transposition] Final Result (padding removed): " + trimmed);
            return trimmed;
        }

        // ========================================================================= 
        // 5. Base64 Encoding / Decoding 
        // ========================================================================= 
        public static string Base64(string text, bool encode)
        {
            string mode = encode ? "Encode" : "Decode";
            Console.WriteLine($"\n[Base64] Mode: {mode}");
            Console.WriteLine($"[Base64] Input: {text}");
            Console.WriteLine("-----------------------------------");

            if (encode)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
                Console.WriteLine("[Base64] Step 1: Convert text to UTF-8 bytes.");
                for (int i = 0; i < bytes.Length; i++)
                {
                    Console.WriteLine($"         Char: '{text[i]}' -> Byte {bytes[i]} (Binary: {Convert.ToString(bytes[i], 2).PadLeft(8, '0')})");
                }
                
                StringBuilder output = new StringBuilder();
                Console.WriteLine("\n[Base64] Step 2: Process bytes in 3-byte (24-bit) chunks...");
                for (int i = 0; i < bytes.Length; i += 3)
                {
                    byte b0 = bytes[i];
                    byte b1 = (i + 1 < bytes.Length) ? bytes[i + 1] : (byte)0;
                    byte b2 = (i + 2 < bytes.Length) ? bytes[i + 2] : (byte)0;
                    int available = Math.Min(3, bytes.Length - i);
                    int padding = 3 - available;
                    
                    Console.WriteLine($"       Processing block {i/3} (Bytes: {b0}, {b1}, {b2})");
                    
                    string b_all = Convert.ToString(b0, 2).PadLeft(8, '0') + 
                                   Convert.ToString(b1, 2).PadLeft(8, '0') + 
                                   Convert.ToString(b2, 2).PadLeft(8, '0');
                    Console.WriteLine($"         24-bit Group: {b_all.Substring(0, 8)} {b_all.Substring(8, 8)} {b_all.Substring(16, 8)}");

                    int i0 = (b0 & 0b11111100) >> 2;
                    int i1 = ((b0 & 0b00000011) << 4) | ((b1 & 0b11110000) >> 4);
                    int i2 = ((b1 & 0b00001111) << 2) | ((b2 & 0b11000000) >> 6);
                    int i3 = b2 & 0b00111111;

                    Console.WriteLine($"         6-bit Chunks: {Convert.ToString(i0, 2).PadLeft(6, '0')} {Convert.ToString(i1, 2).PadLeft(6, '0')} {Convert.ToString(i2, 2).PadLeft(6, '0')} {Convert.ToString(i3, 2).PadLeft(6, '0')}");
                    Console.WriteLine($"         Indices:      {i0,2} {i1,2} {i2,2} {i3,2}");

                    char c0 = alphabet[i0];
                    char c1 = alphabet[i1];
                    char c2 = (padding < 2) ? alphabet[i2] : '=';
                    char c3 = (padding < 1) ? alphabet[i3] : '=';
                    
                    Console.WriteLine($"         Characters:   '{c0}' '{c1}' '{c2}' '{c3}'\n");
                    output.Append(c0); output.Append(c1); output.Append(c2); output.Append(c3);
                }
                string encoded = output.ToString();
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("[Base64] Final Result: " + encoded);
                return encoded;
            }
            else
            {
                try
                {
                    string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
                    string sanitized = new string(text.Where(ch => !char.IsWhiteSpace(ch) && ch != '=').ToArray());
                    int padding = text.Length - sanitized.Length;
                    
                    Console.WriteLine($"[Base64] Step 1: Sanitized input '{sanitized}', padding chars found: {padding}");

                    List<byte> decodedBytes = new List<byte>();
                    Console.WriteLine("\n[Base64] Step 2: Process text in 4-char chunks...");

                    for (int i = 0; i < sanitized.Length; i += 4)
                    {
                        char c0 = sanitized[i];
                        char c1 = (i + 1 < sanitized.Length) ? sanitized[i + 1] : 'A';
                        char c2 = (i + 2 < sanitized.Length) ? sanitized[i + 2] : 'A';
                        char c3 = (i + 3 < sanitized.Length) ? sanitized[i + 3] : 'A';
                        
                        Console.WriteLine($"       Processing block {i/4} (Chars: '{c0}', '{c1}', '{c2}', '{c3}')");

                        int i0 = alphabet.IndexOf(c0);
                        int i1 = alphabet.IndexOf(c1);
                        int i2 = alphabet.IndexOf(c2);
                        int i3 = alphabet.IndexOf(c3);

                        Console.WriteLine($"         Indices:      {i0,2} {i1,2} {i2,2} {i3,2}");
                        Console.WriteLine($"         6-bit Chunks: {Convert.ToString(i0, 2).PadLeft(6, '0')} {Convert.ToString(i1, 2).PadLeft(6, '0')} {Convert.ToString(i2, 2).PadLeft(6, '0')} {Convert.ToString(i3, 2).PadLeft(6, '0')}");

                        int b24 = (i0 << 18) | (i1 << 12) | (i2 << 6) | i3;
                        
                        byte b0 = (byte)((b24 >> 16) & 0xFF);
                        byte b1 = (byte)((b24 >> 8) & 0xFF);
                        byte b2 = (byte)(b24 & 0xFF);

                        Console.WriteLine($"         24-bit Group: {Convert.ToString(b0, 2).PadLeft(8, '0')} {Convert.ToString(b1, 2).PadLeft(8, '0')} {Convert.ToString(b2, 2).PadLeft(8, '0')}");
                        Console.WriteLine($"         Bytes:        {b0} {b1} {b2}\n");
                        
                        decodedBytes.Add(b0);
                        decodedBytes.Add(b1);
                        decodedBytes.Add(b2);
                    }
                    
                    // Remove padding bytes
                    if (padding > 0)
                    {
                        decodedBytes.RemoveRange(decodedBytes.Count - padding, padding);
                    }

                    Console.WriteLine("[Base64] Step 3: Convert bytes back to UTF-8 string.");
                    string decoded = Encoding.UTF8.GetString(decodedBytes.ToArray());
                    Console.WriteLine("-----------------------------------");
                    Console.WriteLine("[Base64] Final Result: " + decoded);
                    return decoded;
                }
                catch (Exception)
                {
                    throw new ArgumentException("Input is not a valid Base64 string for decoding.");
                }
            }
        }

        // ========================================================================= 
        // 6. Playfair Cipher (Multi-Character Encryption) 
        // ========================================================================= 
        public static string PlayfairCipher(string text, string key, bool encrypt)
        {
            string processedText = Regex.Replace(text.ToUpper(), @"[^A-Z]", "").Replace("J", "I");
            string processedKey = Regex.Replace(key.ToUpper(), @"[^A-Z]", "").Replace("J", "I");
            string mode = encrypt ? "Encrypt" : "Decrypt";

            Console.WriteLine($"\n[Playfair] Mode: {mode}");
            Console.WriteLine($"[Playfair] Processed Text (J->I): {processedText}");
            Console.WriteLine($"[Playfair] Processed Key (J->I): {processedKey}");
            
            // --- Step 1: Build 5x5 matrix ---
            List<char> matrixChars = new List<char>();
            foreach (char c in processedKey)
            {
                if (!matrixChars.Contains(c)) matrixChars.Add(c);
            }
            for (char c = 'A'; c <= 'Z'; c++)
            {
                if (c == 'J') continue;
                if (!matrixChars.Contains(c)) matrixChars.Add(c);
            }
            char[,] matrix = new char[5, 5];
            Console.WriteLine("\n[Playfair] Step 1: Building 5x5 Matrix...");
            for (int i = 0; i < 25; i++)
            {
                matrix[i / 5, i % 5] = matrixChars[i];
            }
            for (int r = 0; r < 5; r++)
            {
                Console.WriteLine($"         [{matrix[r, 0]} {matrix[r, 1]} {matrix[r, 2]} {matrix[r, 3]} {matrix[r, 4]}]");
            }

            // --- Step 2: Prepare text into digraphs ---
            StringBuilder preparedTextBuilder = new StringBuilder();
            for (int i = 0; i < processedText.Length; i++)
            {
                char c1 = processedText[i];
                if (i + 1 == processedText.Length) // Last char
                {
                    preparedTextBuilder.Append(c1);
                    preparedTextBuilder.Append('X'); // Pad with X
                    break;
                }
                char c2 = processedText[i + 1];
                if (c1 == c2)
                {
                    preparedTextBuilder.Append(c1);
                    preparedTextBuilder.Append('X'); // Insert filler X
                }
                else
                {
                    preparedTextBuilder.Append(c1);
                    preparedTextBuilder.Append(c2);
                    i++; // Skip next char
                }
            }
            string preparedText = preparedTextBuilder.ToString();
            Console.WriteLine("\n[Playfair] Step 2: Preparing Text Digraphs...");
            for (int i = 0; i < preparedText.Length; i += 2)
            {
                Console.Write(preparedText.Substring(i, 2) + " ");
            }
            Console.WriteLine();


            // --- Step 3: Apply Playfair rules ---
            Console.WriteLine("\n[Playfair] Step 3: Applying Cipher Rules...");
            StringBuilder playfairResult = new StringBuilder();
            (int r, int c) FindCoords(char ch)
            {
                for (int r = 0; r < 5; r++)
                    for (int c = 0; c < 5; c++)
                        if (matrix[r, c] == ch) return (r, c);
                return (-1, -1);
            }

            for (int i = 0; i < preparedText.Length; i += 2)
            {
                char char1 = preparedText[i];
                char char2 = preparedText[i + 1];
                (int r1, int c1) = FindCoords(char1);
                (int r2, int c2) = FindCoords(char2);
                int shift = encrypt ? 1 : 4; // Encrypt: +1, Decrypt: +4 (-1 mod 5)
                
                Console.Write($"         Pair '{char1}{char2}' ({r1},{c1}) ({r2},{c2}): ");

                if (r1 == r2) // Same row
                {
                    char res1 = matrix[r1, (c1 + shift) % 5];
                    char res2 = matrix[r2, (c2 + shift) % 5];
                    Console.WriteLine($"Same Row -> '{res1}{res2}'");
                    playfairResult.Append(res1); playfairResult.Append(res2);
                }
                else if (c1 == c2) // Same column
                {
                    char res1 = matrix[(r1 + shift) % 5, c1];
                    char res2 = matrix[(r2 + shift) % 5, c2];
                    Console.WriteLine($"Same Column -> '{res1}{res2}'");
                    playfairResult.Append(res1); playfairResult.Append(res2);
                }
                else // Rectangle
                {
                    char res1 = matrix[r1, c2]; // Swap columns
                    char res2 = matrix[r2, c1];
                    Console.WriteLine($"Rectangle -> '{res1}{res2}'");
                    playfairResult.Append(res1); playfairResult.Append(res2);
                }
            }
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("[Playfair] Final Result: " + playfairResult.ToString());
            return playfairResult.ToString();
        }

        // =========================================================================
        // 7. XOR Cipher (Symmetric Stream Cipher)
        // =========================================================================
        public static string XorCipher(string text, string key, bool encrypt)
        {
            string mode = encrypt ? "Encrypt (to Base64)" : "Decrypt (from Base64)";
            Console.WriteLine($"\n[XOR] Mode: {mode}");
            Console.WriteLine($"[XOR] Key: {key}");
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("XOR key cannot be empty.");
            }

            if (encrypt)
            {
                Console.WriteLine($"[XOR] Input (Plaintext): {text}");
                Console.WriteLine("[XOR] Step 1: Convert Text and Key to UTF-8 Bytes");
                byte[] textBytes = Encoding.UTF8.GetBytes(text);
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                byte[] resultBytes = new byte[textBytes.Length];

                Console.WriteLine("\n[XOR] Step 2: Applying repeating XOR operation...");
                for (int i = 0; i < textBytes.Length; i++)
                {
                    byte textByte = textBytes[i];
                    byte keyByte = keyBytes[i % keyBytes.Length];
                    resultBytes[i] = (byte)(textByte ^ keyByte);
                    
                    Console.WriteLine($"         Index {i}: (Text) {textByte,3} [ {Convert.ToString(textByte, 2).PadLeft(8, '0')} ]  ^  (Key) {keyByte,3} [ {Convert.ToString(keyByte, 2).PadLeft(8, '0')} ]  =  (Result) {resultBytes[i],3} [ {Convert.ToString(resultBytes[i], 2).PadLeft(8, '0')} ]");
                }
                
                string encodedResult = Convert.ToBase64String(resultBytes);
                Console.WriteLine("\n[XOR] Step 3: Encode raw result bytes to Base64 string.");
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("[XOR] Final Result (Base64): " + encodedResult);
                return encodedResult;
            }
            else // Decrypt
            {
                Console.WriteLine($"[XOR] Input (Base64): {text}");
                Console.WriteLine("[XOR] Step 1: Decode Base64 Input to Raw Ciphertext Bytes");
                byte[] textBytes;
                try
                {
                    textBytes = Convert.FromBase64String(text);
                }
                catch (FormatException)
                {
                    throw new ArgumentException("Invalid Base64 string for XOR decryption.");
                }
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                byte[] resultBytes = new byte[textBytes.Length];

                Console.WriteLine("\n[XOR] Step 2: Applying repeating XOR operation...");
                for (int i = 0; i < textBytes.Length; i++)
                {
                    byte textByte = textBytes[i];
                    byte keyByte = keyBytes[i % keyBytes.Length];
                    resultBytes[i] = (byte)(textByte ^ keyByte);
                    Console.WriteLine($"         Index {i}: (Cipher) {textByte,3} [ {Convert.ToString(textByte, 2).PadLeft(8, '0')} ]  ^  (Key) {keyByte,3} [ {Convert.ToString(keyByte, 2).PadLeft(8, '0')} ]  =  (Result) {resultBytes[i],3} [ {Convert.ToString(resultBytes[i], 2).PadLeft(8, '0')} ]");
                }

                string decodedResult = Encoding.UTF8.GetString(resultBytes);
                Console.WriteLine("\n[XOR] Step 3: Convert result bytes to UTF-8 string.");
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("[XOR] Final Result (Plaintext): " + decodedResult);
                return decodedResult;
            }
        }

        // =========================================================================
        // 8. Hill Cipher (Polygraphic Substitution)
        // =========================================================================
        public static string HillCipher(string text, string key, bool encrypt)
        {
            string processedText = Regex.Replace(text.ToUpper(), @"[^A-Z]", "");
            string processedKey = Regex.Replace(key.ToUpper(), @"[^A-Z]", "");
            string mode = encrypt ? "Encrypt" : "Decrypt";

            Console.WriteLine($"\n[Hill] Mode: {mode}");
            Console.WriteLine($"[Hill] Processed Text: {processedText}");
            Console.WriteLine($"[Hill] Processed Key: {processedKey}");

            // --- Step 1: Validate Key and build matrix ---
            Console.WriteLine("\n[Hill] Step 1: Creating 2x2 Key Matrix (K)...");
            if (processedKey.Length != 4)
            {
                throw new ArgumentException("Hill Cipher key must be 4 letters for a 2x2 matrix.");
            }
            int[,] keyMatrix = new int[2, 2];
            keyMatrix[0, 0] = processedKey[0] - 'A'; keyMatrix[0, 1] = processedKey[1] - 'A';
            keyMatrix[1, 0] = processedKey[2] - 'A'; keyMatrix[1, 1] = processedKey[3] - 'A';
            Console.WriteLine($"         K = | {keyMatrix[0, 0],2}  {keyMatrix[0, 1],2} |");
            Console.WriteLine($"             | {keyMatrix[1, 0],2}  {keyMatrix[1, 1],2} |");

            // --- Step 2: Check Determinant ---
            Console.WriteLine("\n[Hill] Step 2: Calculating Determinant (det(K) mod 26)...");
            int det = (keyMatrix[0, 0] * keyMatrix[1, 1] - keyMatrix[0, 1] * keyMatrix[1, 0]);
            int detMod26 = (det % 26 + 26) % 26; // Handle negative det
            Console.WriteLine($"         det(K) = ({keyMatrix[0, 0]}*{keyMatrix[1, 1]}) - ({keyMatrix[0, 1]}*{keyMatrix[1, 0]}) = {det}");
            Console.WriteLine($"         det(K) mod 26 = {detMod26}");
            
            int detInverse = ModInverse(detMod26, 26);
            if (detInverse == -1)
            {
                throw new ArgumentException($"Invalid key. Determinant {detMod26} is not invertible modulo 26 (not coprime to 26). Try a different key.");
            }
            Console.WriteLine($"         Modular Inverse of det(K) = {detInverse}");

            // --- Step 3: Get Operation Matrix ---
            int[,] opMatrix;
            if (encrypt)
            {
                opMatrix = keyMatrix;
                Console.WriteLine("\n[Hill] Step 3: Using Key Matrix (K) for Encryption.");
            }
            else
            {
                Console.WriteLine("\n[Hill] Step 3: Calculating Inverse Matrix (K_inv) for Decryption...");
                // K_inv = det_inv * adj(K) mod 26
                // adj(K) for 2x2 = [[K[1,1], -K[0,1]], [-K[1,0], K[0,0]]]
                int[,] adjMatrix = new int[2, 2];
                adjMatrix[0, 0] = (keyMatrix[1, 1] + 26) % 26;
                adjMatrix[0, 1] = (-keyMatrix[0, 1] + 26) % 26;
                adjMatrix[1, 0] = (-keyMatrix[1, 0] + 26) % 26;
                adjMatrix[1, 1] = (keyMatrix[0, 0] + 26) % 26;
                Console.WriteLine($"         adj(K) = | {adjMatrix[0, 0],2}  {adjMatrix[0, 1],2} |");
                Console.WriteLine($"                  | {adjMatrix[1, 0],2}  {adjMatrix[1, 1],2} |");

                opMatrix = new int[2, 2];
                opMatrix[0, 0] = (detInverse * adjMatrix[0, 0]) % 26;
                opMatrix[0, 1] = (detInverse * adjMatrix[0, 1]) % 26;
                opMatrix[1, 0] = (detInverse * adjMatrix[1, 0]) % 26;
                opMatrix[1, 1] = (detInverse * adjMatrix[1, 1]) % 26;
                
                Console.WriteLine($"         K_inv = {detInverse} * adj(K) mod 26");
                Console.WriteLine($"         K_inv = | {opMatrix[0, 0],2}  {opMatrix[0, 1],2} |");
                Console.WriteLine($"                 | {opMatrix[1, 0],2}  {opMatrix[1, 1],2} |");
            }

            // --- Step 4: Pad text ---
            if (processedText.Length % 2 != 0)
            {
                processedText += "X";
                Console.WriteLine($"\n[Hill] Step 4: Padded text to even length: {processedText}");
            } else {
                Console.WriteLine("\n[Hill] Step 4: Text length is even, no padding needed.");
            }

            // --- Step 5: Process text blocks ---
            Console.WriteLine("\n[Hill] Step 5: Processing text in 2-char blocks (P * K_op)...");
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < processedText.Length; i += 2)
            {
                int p1 = processedText[i] - 'A';
                int p2 = processedText[i + 1] - 'A';
                Console.Write($"         Block {i/2} ('{processedText[i]}{processedText[i+1]}') -> [ {p1,2}  {p2,2} ] * K_op");

                // C = P * K_op
                // c1 = (p1*k00 + p2*k10) mod 26
                // c2 = (p1*k01 + p2*k11) mod 26
                int c1 = (p1 * opMatrix[0, 0] + p2 * opMatrix[1, 0]) % 26;
                int c2 = (p1 * opMatrix[0, 1] + p2 * opMatrix[1, 1]) % 26;
                if (c1 < 0) c1 += 26; if (c2 < 0) c2 += 26;

                char res1 = (char)('A' + c1);
                char res2 = (char)('A' + c2);
                
                Console.WriteLine($" = [ {c1,2}  {c2,2} ] -> '{res1}{res2}'");
                result.Append(res1); result.Append(res2);
            }
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("[Hill] Final Result: " + result.ToString());
            return result.ToString();
        }

        // --- Hill Cipher Helpers ---
        private static int Gcd(int a, int b)
        {
            while (b != 0) { int temp = b; b = a % b; a = temp; }
            return a;
        }
        private static int ModInverse(int a, int m)
        {
            a = (a % m + m) % m;
            if (Gcd(a, m) != 1) return -1; // No inverse
            for (int x = 1; x < m; x++)
            {
                if ((a * x) % m == 1) return x;
            }
            return -1;
        }
    }
    #endregion

    class Program
    {
        #region Windows Console GUI API (for mouse clicks)

        // --- Win32 API Constants ---
        private const int STD_INPUT_HANDLE = -10;
        private const uint ENABLE_PROCESSED_INPUT = 0x0001;
        private const uint ENABLE_MOUSE_INPUT = 0x0010;
        private const uint ENABLE_EXTENDED_FLAGS = 0x0080;
        private const uint ENABLE_QUICK_EDIT_MODE = 0x0040;
        private const uint ENABLE_WINDOW_INPUT = 0x0008; 

        // --- Event Types ---
        private const ushort KEY_EVENT = 0x0001;
        private const ushort MOUSE_EVENT = 0x0002;
        private const ushort WINDOW_BUFFER_SIZE_EVENT = 0x0004;
        
        private const uint MOUSE_MOVED = 0x0001;
        private const uint FROM_LEFT_1ST_BUTTON_PRESSED = 0x0001;

        // --- Key Codes ---
        private const ushort VK_UP = 0x0026;
        private const ushort VK_DOWN = 0x0028;
        private const ushort VK_RETURN = 0x000D; // Enter key
        private const ushort VK_ESCAPE = 0x001B; // Escape key

        // --- Win32 API Imports ---
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool ReadConsoleInput(IntPtr hConsoleIn, [Out] INPUT_RECORD[] lpBuffer, uint nLength, out uint lpNumberOfEventsRead);

        // --- Win32 API Structures ---
        [StructLayout(LayoutKind.Explicit)]
        public struct INPUT_RECORD
        {
            [FieldOffset(0)] public ushort EventType;
            [FieldOffset(4)] public KEY_EVENT_RECORD KeyEvent;
            [FieldOffset(4)] public MOUSE_EVENT_RECORD MouseEvent;
            [FieldOffset(4)] public WINDOW_BUFFER_SIZE_RECORD WindowBufferSizeEvent;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct KEY_EVENT_RECORD
        {
            public bool bKeyDown;
            public ushort wRepeatCount;
            public ushort wVirtualKeyCode;
            public ushort wVirtualScanCode;
            public char UnicodeChar;
            public uint dwControlKeyState;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSE_EVENT_RECORD
        {
            public COORD dwMousePosition;
            public uint dwButtonState;
            public uint dwControlKeyState;
            public uint dwEventFlags;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct COORD { public short X; public short Y; }
        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOW_BUFFER_SIZE_RECORD { public COORD dwSize; }
        private struct MenuOptionRect
        {
            public int Left, Top, Right, Bottom;
            public bool IsInside(COORD pos) => pos.Y >= Top && pos.Y <= Bottom && pos.X >= Left && pos.X <= Right;
        }
        #endregion

        #region Console Mode Helpers
        /// <summary>
        /// Gets the current console mode.
        /// </summary>
        private static bool GetConsoleMode(out uint mode)
        {
            IntPtr consoleHandle = GetStdHandle(STD_INPUT_HANDLE);
            return GetConsoleMode(consoleHandle, out mode);
        }

        /// <summary>
        /// Sets the console mode to enable mouse/window input and disable quick edit.
        /// </summary>
        private static bool SetMouseMode(uint originalMode)
        {
            IntPtr consoleHandle = GetStdHandle(STD_INPUT_HANDLE);
            // ENABLE_PROCESSED_INPUT eklendi - klavye girişi için gerekli
            uint newMode = (originalMode | ENABLE_PROCESSED_INPUT | ENABLE_MOUSE_INPUT | ENABLE_WINDOW_INPUT | ENABLE_EXTENDED_FLAGS) & ~ENABLE_QUICK_EDIT_MODE;
            return SetConsoleMode(consoleHandle, newMode);
        }

        /// <summary>
        /// Restores the console mode to its original state (e.g., for typing).
        /// </summary>
        private static bool RestoreConsoleMode(uint originalMode)
        {
            IntPtr consoleHandle = GetStdHandle(STD_INPUT_HANDLE);
            return SetConsoleMode(consoleHandle, originalMode);
        }
        #endregion

        #region GUI Elements

        /// <summary>
        /// A new MenuSelect that is centered, has a GUI-like border, and supports mouse/keyboard.
        /// </summary>
        static int MenuSelect(string title, string[] options)
        {
            if (!GetConsoleMode(out uint _))
            {
                return OldMenuSelect(title, options); // Fallback
            }

            int selected = 0;
            MenuOptionRect[] optionRects = new MenuOptionRect[options.Length];
            
            // --- Render Function (Tasarımın yapıldığı yer) ---
            void Render()
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                int windowWidth = Console.WindowWidth;
                int windowHeight = Console.WindowHeight;

                int contentWidth = 0;
                contentWidth = Math.Max(contentWidth, title.Length);
                foreach (string option in options)
                {
                    contentWidth = Math.Max(contentWidth, option.Length);
                }
                int boxWidth = contentWidth + 8; // e.g., "  » Option «  "
                int boxHeight = options.Length + 6; 

                int startX = (windowWidth - boxWidth) / 2;
                int startY = (windowHeight - boxHeight) / 2;

                if (startX < 0 || startY < 0)
                {
                    Console.Write("Window is too small...");
                    return;
                }

                // --- Draw Box ---
                Console.SetCursorPosition(startX, startY);
                Console.Write("╔" + new string('═', boxWidth - 2) + "╗");
                Console.SetCursorPosition(startX, startY + 1);
                Console.Write("║" + new string(' ', boxWidth - 2) + "║");
                
                Console.SetCursorPosition(startX + (boxWidth - title.Length) / 2, startY + 2);
                Console.ForegroundColor = ConsoleColor.Yellow; // Title color
                Console.Write(title);
                Console.ResetColor();
                Console.SetCursorPosition(startX, startY + 2); Console.Write("║");
                Console.SetCursorPosition(startX + boxWidth - 1, startY + 2); Console.Write("║");
                Console.SetCursorPosition(startX, startY + 3);
                Console.Write("╠" + new string('═', boxWidth - 2) + "╣"); // Separator

                // --- Draw Options ---
                for (int i = 0; i < options.Length; i++)
                {
                    int currentY = startY + 4 + i;
                    Console.SetCursorPosition(startX, currentY);
                    Console.Write("║");
                    string text = options[i];
                    string line;

                    if (i == selected)
                    {
                        line = $"  » {text} «".PadRight(boxWidth - 2); 
                        Console.SetCursorPosition(startX + 1, currentY);
                        Console.BackgroundColor = ConsoleColor.DarkCyan;
                        Console.ForegroundColor = ConsoleColor.White; 
                        Console.Write(line);
                        Console.ResetColor();
                    }
                    else
                    {
                        line = $"    {text}    ".PadRight(boxWidth - 2);
                        Console.SetCursorPosition(startX + 1, currentY);
                        Console.ForegroundColor = ConsoleColor.Gray; // Dimmed text
                        Console.Write(line);
                        Console.ResetColor(); 
                    }
                    
                    optionRects[i] = new MenuOptionRect { Left = startX + 1, Top = currentY, Right = startX + boxWidth - 2, Bottom = currentY };
                    Console.SetCursorPosition(startX + boxWidth - 1, currentY);
                    Console.Write("║");
                }

                Console.SetCursorPosition(startX, startY + 4 + options.Length);
                Console.Write("╠" + new string('═', boxWidth - 2) + "╣"); // Bottom separator
                
                string instructions = "Use ↑/↓, Click, or Esc"; // English instructions
                Console.SetCursorPosition(startX, startY + 5 + options.Length);
                Console.Write("║" + new string(' ', boxWidth - 2) + "║");
                Console.SetCursorPosition(startX + (boxWidth - instructions.Length) / 2, startY + 5 + options.Length);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(instructions);
                Console.ResetColor();
                Console.SetCursorPosition(startX, startY + 6 + options.Length);
                Console.Write("╚" + new string('═', boxWidth - 2) + "╝");
            }

            Render();
            
            // --- Input Loop ---
            // Küçük tampon kullan - her olay hemen işlensin
            INPUT_RECORD[] inputBuffer = new INPUT_RECORD[1];
            while (true)
            {
                // Her seferinde 1 olay oku - daha responsive
                if (ReadConsoleInput(GetStdHandle(STD_INPUT_HANDLE), inputBuffer, 1, out uint eventsRead) && eventsRead > 0)
                {
                    var input = inputBuffer[0];
                    bool needsRender = false;
                    
                    switch (input.EventType)
                    {
                        // --- Keyboard Input ---
                        case KEY_EVENT:
                            if (input.KeyEvent.bKeyDown) // Only on key down
                            {
                                switch (input.KeyEvent.wVirtualKeyCode)
                                {
                                    case VK_UP: 
                                        selected = (selected - 1 + options.Length) % options.Length; 
                                        needsRender = true; 
                                        break;
                                    case VK_DOWN: 
                                        selected = (selected + 1) % options.Length; 
                                        needsRender = true; 
                                        break;
                                    case VK_RETURN: // Enter
                                        return selected;
                                    case VK_ESCAPE: // Escape
                                        return -1;
                                }
                            }
                            break;

                        // --- Mouse Input ---
                        case MOUSE_EVENT:
                            var mouse = input.MouseEvent;
                            
                            // Check for Hover (Mouse Moved)
                            if (mouse.dwEventFlags == MOUSE_MOVED)
                            {
                                for (int j = 0; j < optionRects.Length; j++)
                                {
                                    if (optionRects[j].IsInside(mouse.dwMousePosition) && selected != j)
                                    {
                                        selected = j; 
                                        needsRender = true; 
                                        break;
                                    }
                                }
                            }
                            // Check for Click
                            else if (mouse.dwEventFlags == 0 && mouse.dwButtonState == FROM_LEFT_1ST_BUTTON_PRESSED)
                            {
                                for (int j = 0; j < optionRects.Length; j++)
                                {
                                    if (optionRects[j].IsInside(mouse.dwMousePosition)) return j;
                                }
                            }
                            break;
                        
                        // --- Window Resize Event ---
                        case WINDOW_BUFFER_SIZE_EVENT:
                            needsRender = true;
                            break;
                    }

                    if (needsRender)
                    {
                        Render(); // Olay işlendikten hemen sonra render et
                    }
                }
            }
        }

        /// <summary>
        /// Draws a framed box and prompts the user for text input.
        /// </summary>
        static string FramedPrompt(string title, string prompt, uint originalMode)
        {
            Console.Clear();
            int windowWidth = Console.WindowWidth;
            int windowHeight = Console.WindowHeight;
            int boxWidth = Math.Max(title.Length, prompt.Length + 2) + 12;
            int boxHeight = 7;
            int startX = (windowWidth - boxWidth) / 2;
            int startY = (windowHeight - boxHeight) / 2;

            if (startX < 0 || startY < 0)
            {
                Console.Write("Window too small. " + prompt);
                return Console.ReadLine() ?? "";
            }

            // --- Draw Box ---
            Console.SetCursorPosition(startX, startY);
            Console.Write("╔" + new string('═', boxWidth - 2) + "╗");
            Console.SetCursorPosition(startX, startY + 1);
            Console.Write("║" + new string(' ', boxWidth - 2) + "║");
            Console.SetCursorPosition(startX + (boxWidth - title.Length) / 2, startY + 2);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(title);
            Console.ResetColor();
            Console.SetCursorPosition(startX, startY + 2); Console.Write("║");
            Console.SetCursorPosition(startX + boxWidth - 1, startY + 2); Console.Write("║");
            Console.SetCursorPosition(startX, startY + 3);
            Console.Write("╠" + new string('═', boxWidth - 2) + "╣");
            Console.SetCursorPosition(startX, startY + 4);
            Console.Write("║  ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(prompt); // Write the prompt text
            Console.ResetColor();
            string spaces = new string(' ', boxWidth - 4 - prompt.Length);
            Console.Write(spaces + "║");
            Console.SetCursorPosition(startX, startY + 5);
            Console.Write("╚" + new string('═', boxWidth - 2) + "╝");

            // --- Get Input ---
            RestoreConsoleMode(originalMode); // Restore normal typing
            Console.CursorVisible = true;
            Console.SetCursorPosition(startX + 3 + prompt.Length, startY + 4);
            string input = Console.ReadLine() ?? "";
            
            Console.CursorVisible = false;
            SetMouseMode(originalMode); // Re-set mouse mode
            return input;
        }

        /// <summary>
        /// Displays the About information in a framed box and waits for input.
        /// </summary>
        static void ShowAbout()
        {
            Console.Clear();
            int windowWidth = Console.WindowWidth;
            int windowHeight = Console.WindowHeight;

            string[] lines = {
                "This console app demonstrates classic ciphers:",
                "- Caesar, Monoalphabetic, Vigenere (substitution)",
                "- Transposition (permutation)",
                "- Playfair, Hill (polygraphic)",
                "- XOR (byte-level stream cipher)",
                "- Base64 (binary-to-text encoding)",
                "",
                "Created for educational purposes."
            };
            string title = "About This Application";
            
            int boxWidth = lines.Max(s => s.Length) + 4;
            boxWidth = Math.Max(boxWidth, title.Length + 4);
            int boxHeight = lines.Length + 6;
            
            int startX = (windowWidth - boxWidth) / 2;
            int startY = (windowHeight - boxHeight) / 2;
            
            if (startX < 0 || startY < 0) {
                Console.WriteLine(title); Console.WriteLine(string.Join("\n", lines));
                WaitForAck("Window too small. Press any key..."); return;
            }

            // --- Draw Box ---
            Console.SetCursorPosition(startX, startY);
            Console.Write("╔" + new string('═', boxWidth - 2) + "╗");
            Console.SetCursorPosition(startX, startY + 1);
            Console.Write("║" + new string(' ', boxWidth - 2) + "║");
            Console.SetCursorPosition(startX + (boxWidth - title.Length) / 2, startY + 2);
            Console.ForegroundColor = ConsoleColor.Yellow; Console.Write(title); Console.ResetColor();
            Console.SetCursorPosition(startX, startY + 2); Console.Write("║");
            Console.SetCursorPosition(startX + boxWidth - 1, startY + 2); Console.Write("║");
            Console.SetCursorPosition(startX, startY + 3);
            Console.Write("╠" + new string('═', boxWidth - 2) + "╣");

            for(int i = 0; i < lines.Length; i++)
            {
                int currentY = startY + 4 + i;
                Console.SetCursorPosition(startX, currentY); Console.Write("║");
                Console.SetCursorPosition(startX + 2, currentY); Console.Write(lines[i].PadRight(boxWidth - 4));
                Console.SetCursorPosition(startX + boxWidth - 1, currentY); Console.Write("║");
            }
            
            Console.SetCursorPosition(startX, startY + 4 + lines.Length);
            Console.Write("╠" + new string('═', boxWidth - 2) + "╣");
            string instructions = "Click or Press any key to return";
            Console.SetCursorPosition(startX, startY + 5 + lines.Length);
            Console.Write("║" + instructions.PadRight(boxWidth-2) + "║");
            Console.SetCursorPosition(startX, startY + 6 + lines.Length);
            Console.Write("╚" + new string('═', boxWidth - 2) + "╝");

            // --- Wait for Input ---
            WaitForInputLoop();
        }

        /// <summary>
        /// Waits for a single key press or mouse click.
        /// </summary>
        static void WaitForInputLoop()
        {
            // Her seferinde 1 olay oku - daha responsive
            INPUT_RECORD[] inputBuffer = new INPUT_RECORD[1];
            while (true)
            {
                if (ReadConsoleInput(GetStdHandle(STD_INPUT_HANDLE), inputBuffer, 1, out uint eventsRead) && eventsRead > 0)
                {
                    var input = inputBuffer[0];
                    
                    // Yalnızca tuşa basma VEYA fare tıklaması (hareketleri yoksay)
                    if (input.EventType == KEY_EVENT && input.KeyEvent.bKeyDown) return;
                    if (input.EventType == MOUSE_EVENT && input.MouseEvent.dwEventFlags == 0 && input.MouseEvent.dwButtonState == FROM_LEFT_1ST_BUTTON_PRESSED) return;
                }
            }
        }

        /// <summary>
        /// Displays a short message (like an acknowledgement) and waits for any key or click.
        /// This was missing and referenced from `ShowAbout` when the window is too small.
        /// </summary>
        static void WaitForAck(string message)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(message);
            Console.ResetColor();
            WaitForInputLoop();
        }
        
        /// <summary>
        /// Fallback menu for when Win32 API fails.
        /// </summary>
        static int OldMenuSelect(string title, string[] options)
        {
            Console.Clear();
            Console.CursorVisible = false;
            int selected = 0;
            void Render()
            {
                Console.Clear(); Console.SetCursorPosition(2, 2);
                Console.ForegroundColor = ConsoleColor.Cyan; Console.WriteLine(title); Console.ResetColor();
                for (int i = 0; i < options.Length; i++)
                {
                    Console.SetCursorPosition(2, 4 + i);
                    if (i == selected)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkCyan; Console.ForegroundColor = ConsoleColor.Black;
                        Console.Write($"> {options[i]} "); Console.ResetColor();
                    }
                    else { Console.Write($"  {options[i]}  "); }
                }
                Console.SetCursorPosition(2, 6 + options.Length);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("Use Up/Down arrows, Enter to select, Esc to go back/exit."); Console.ResetColor();
            }
            Render();
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow) { selected = (selected - 1 + options.Length) % options.Length; Render(); }
                else if (key.Key == ConsoleKey.DownArrow) { selected = (selected + 1) % options.Length; Render(); }
                else if (key.Key == ConsoleKey.Enter) { Console.CursorVisible = true; return selected; }
                else if (key.Key == ConsoleKey.Escape) { Console.CursorVisible = true; return -1; }
            }
        }

        #endregion

        static void Main(string[] args)
        {
            Console.Title = "Classic Cipher Application";
            Console.CursorVisible = false;

            // --- Setup Console Mode ---
            if (!GetConsoleMode(out uint originalMode)) {
                Console.WriteLine("Failed to get console mode. Exiting.");
                return;
            }
            if (!SetMouseMode(originalMode))
            {
                Console.WriteLine("Failed to set console mode. Mouse input disabled.");
                // We can continue, but menu will be keyboard-only
            }
            // --- End Setup ---

            #nullable enable
            bool running = true;
            while (running)
            {
                string[] mainMenuOptions = {
                    "Encrypt Text",
                    "Decrypt Text",
                    "About",
                    "Exit"
                };
                int mainSel = MenuSelect("Main Menu", mainMenuOptions);

                if (mainSel == -1 || mainSel == 3) // -1 is Esc
                {
                    break;
                }

                if (mainSel == 2)
                {
                    ShowAbout();
                    continue;
                }

                bool encrypt = (mainSel == 0);
                string[] cipherMenuOptions = {
                    "Caesar Cipher",
                    "Monoalphabetic Substitution",
                    "Vigenere Cipher",
                    "Columnar Transposition",
                    "Base64 (Encode/Decode)",
                    "Playfair Cipher",
                    "XOR Cipher",
                    "Hill Cipher (2x2)"
                };
                int cipherSel = MenuSelect("Select Cipher Method", cipherMenuOptions);

                if (cipherSel == -1) // -1 is Esc
                {
                    continue;
                }
                
                string cipherTitle = cipherMenuOptions[cipherSel];
                string? textInput = null;
                string? keyInput = null;
                string result = string.Empty;

                try
                {
                    // --- Framed Input ---
                    string textPrompt = encrypt ? "Enter Plaintext: " : "Enter Ciphertext: ";
                    textInput = FramedPrompt(cipherTitle, textPrompt, originalMode);

                    if (string.IsNullOrWhiteSpace(textInput)) {
                        ShowAbout(); // Show an error message box
                        continue;
                    }
                    
                    bool requiresKey = (cipherSel != 4); // Base64 doesn't need a key
                    if(requiresKey)
                    {
                        string keyPrompt = "Enter Key: ";
                        if (cipherSel == 0) keyPrompt = "Enter Key (e.g., 3): ";
                        else if (cipherSel == 1) keyPrompt = "Enter Key (26 unique letters): ";
                        else if (cipherSel == 3) keyPrompt = "Enter Key (e.g., 25314): ";
                        else if (cipherSel == 5) keyPrompt = "Enter Key (e.g., PLAYFAIR): ";
                        else if (cipherSel == 7) keyPrompt = "Enter Key (4 letters, e.g., GYBN): ";
                        
                        keyInput = FramedPrompt(cipherTitle, keyPrompt, originalMode);
                        if (string.IsNullOrWhiteSpace(keyInput))
                        {
                            // Show error in "About" box style
                            ShowAbout(); // This is a placeholder, you'd want a real error message
                            continue;
                        }
                    }

                    // --- Clear input frame, show logs ---
                    Console.Clear();
                    Console.CursorVisible = false;
                    string text = textInput;
                    string key = keyInput ?? "";

                    // --- Run Operation ---
                    switch (cipherSel)
                    {
                        case 0: // Caesar
                            if (int.TryParse(key, out int intKey))
                                result = CipherOperations.CaesarCipher(text, intKey, encrypt);
                            else throw new ArgumentException("Invalid key. Must be an integer.");
                            break;
                        case 1: // Mono
                            result = CipherOperations.MonoalphabeticSubstitution(text, key, encrypt);
                            break;
                        case 2: // Vigenere
                            result = CipherOperations.VigenereCipher(text, key, encrypt);
                            break;
                        case 3: // Transposition
                            result = CipherOperations.TranspositionCipher(text, key, encrypt);
                            break;
                        case 4: // Base64
                            result = CipherOperations.Base64(text, encrypt);
                            break;
                        case 5: // Playfair
                            result = CipherOperations.PlayfairCipher(text, key, encrypt);
                            break;
                        case 6: // XOR
                            result = CipherOperations.XorCipher(text, key, encrypt);
                            break;
                        case 7: // Hill
                            result = CipherOperations.HillCipher(text, key, encrypt);
                            break;
                    }

                    // --- Show Final Result ---
                    Console.WriteLine($"\n--- FINAL RESULT ---");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(encrypt ? $"Ciphertext: {result}" : $"Plaintext: {result}");
                    Console.ResetColor();
                    
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("\nPress any key or click to return to the menu...");
                    Console.ResetColor();
                    WaitForInputLoop(); // Wait for user ack
                }
                catch (ArgumentException ex)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nError: {ex.Message}");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("\nPress any key or click to return...");
                    Console.ResetColor();
                    WaitForInputLoop();
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nAn unexpected error occurred: {ex.Message}");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("\nPress any key or click to return...");
                    Console.ResetColor();
                    WaitForInputLoop();
                }
            }

            // --- Final Cleanup ---
            RestoreConsoleMode(originalMode);
            Console.ResetColor();
            Console.CursorVisible = true;
            Console.Clear();
            Console.WriteLine("Exiting Classic Cipher App...");
        }
    }
}