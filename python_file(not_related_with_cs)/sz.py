# caesar_bruteforce_with_counts.py
# Geliştirilmiş Caesar brute-force
# - Chi-squared scoring (İngilizce)
# - En olası 5 sonucu gösterir
# - Orijinal ciphertext ve en olası plaintext için A-Z harf sayıları ve yüzdelerini tablo halinde yazdırır

from collections import Counter
import string

ENGLISH_FREQ = {
    'a': 8.167, 'b': 1.492, 'c': 2.782, 'd': 4.253, 'e': 12.702, 'f': 2.228,
    'g': 2.015, 'h': 6.094, 'i': 6.966, 'j': 0.153, 'k': 0.772, 'l': 4.025,
    'm': 2.406, 'n': 6.749, 'o': 7.507, 'p': 1.929, 'q': 0.095, 'r': 5.987,
    's': 6.327, 't': 9.056, 'u': 2.758, 'v': 0.978, 'w': 2.360, 'x': 0.150,
    'y': 1.974, 'z': 0.074
}

ALPHABET = string.ascii_lowercase

def caesar_shift(text, shift):
    out = []
    for ch in text:
        if 'a' <= ch <= 'z':
            out.append(chr((ord(ch) - ord('a') + shift) % 26 + ord('a')))
        elif 'A' <= ch <= 'Z':
            out.append(chr((ord(ch) - ord('A') + shift) % 26 + ord('A')))
        else:
            out.append(ch)
    return ''.join(out)

def chi_squared_stat(text):
    letters = [c.lower() for c in text if 'a' <= c.lower() <= 'z']
    N = len(letters)
    if N == 0:
        return float('inf')
    counts = Counter(letters)
    chi = 0.0
    for letter, freq in ENGLISH_FREQ.items():
        observed = counts.get(letter, 0)
        expected = freq * N / 100.0
        if expected > 0:
            chi += (observed - expected) ** 2 / expected
    return chi

def letter_counts_and_perc(text):
    letters = [c.lower() for c in text if 'a' <= c.lower() <= 'z']
    N = len(letters)
    counts = Counter(letters)
    # return list of tuples (letter, count, percent)
    result = []
    for l in ALPHABET:
        c = counts.get(l, 0)
        p = (c / N * 100.0) if N > 0 else 0.0
        result.append((l, c, p))
    return result, N

def brute_force_caesar_with_counts(ciphertext):
    results = []
    for shift in range(26):
        plain = caesar_shift(ciphertext, shift)
        score = chi_squared_stat(plain)
        results.append((score, shift, plain))
    results.sort(key=lambda x: x[0])

    print("\nEn olası 5 sonuç (düşük χ² daha iyi):\n")
    for score, shift, plain in results[:5]:
        preview = plain.replace('\n',' ')[:140]
        print(f"[Shift {shift:2d}] χ²={score:8.2f} → {preview}...")

    best_score, best_shift, best_plain = results[0]
    print("\nEn olası çözüm (tam metin):\n")
    print(f"Shift {best_shift} | χ²={best_score:.2f}\n")
    print(best_plain)
    # Harf sayıları tablolarıyla göster
    cipher_stats, cipher_total = letter_counts_and_perc(ciphertext)
    plain_stats, plain_total = letter_counts_and_perc(best_plain)

    print("\nHarf dağılımı (ciphertext vs best plaintext):")
    # Tablo başlığı
    print(f"{'Letter':^6} | {'Cipher#':^7} {'Cipher%':^8} || {'Plain#':^7} {'Plain%':^8}")
    print("-"*60)
    for i in range(26):
        l = ALPHABET[i]
        c_count, c_perc = cipher_stats[i][1], cipher_stats[i][2]
        p_count, p_perc = plain_stats[i][1], plain_stats[i][2]
        print(f"{l:^6} | {c_count:7d} {c_perc:7.2f}% || {p_count:7d} {p_perc:7.2f}%")
    print("-"*60)
    print(f"Toplam harf (cipher): {cipher_total} , (plain): {plain_total}")

if __name__ == "__main__":
    ct = input("Şifreli metni gir: ").strip()
    brute_force_caesar_with_counts(ct)
