import string

# Alfabeyi tanımlıyoruz (A=0, B=1, ..., Z=25)
ALPHABET = string.ascii_uppercase

def vigenere_encrypt(plaintext, key):
    """
    Vigenère şifrelemesi yapar.
    Formül: C_i = (P_i + K_i) mod 26
    """
    # Düz metni temizle ve büyük harfe çevir
    plaintext = "".join(c for c in plaintext.upper() if c.isalpha())
    key = key.upper()
    
    ciphertext = ""
    key_index = 0
    
    for char in plaintext:
        # Düz metin harfinin sayısal değerini bul
        P_val = ALPHABET.find(char)
        
        # Anahtar harfinin sayısal değerini bul (anahtarı döngüsel olarak kullan)
        K_val = ALPHABET.find(key[key_index % len(key)])
        
        # Şifreleme işlemini yap: (P + K) mod 26
        C_val = (P_val + K_val) % len(ALPHABET)
        
        # Sayısal değeri tekrar harfe çevir
        ciphertext += ALPHABET[C_val]
        
        # Anahtar döngüsünün indeksini bir artır
        key_index += 1
        
    return ciphertext

def vigenere_decrypt(ciphertext, key):
    """
    Vigenère deşifrelemesi yapar.
    Formül: P_i = (C_i - K_i) mod 26
    """
    # Şifreli metni temizle ve büyük harfe çevir
    ciphertext = "".join(c for c in ciphertext.upper() if c.isalpha())
    key = key.upper()
    
    plaintext = ""
    key_index = 0
    
    for char in ciphertext:
        # Şifreli metin harfinin sayısal değerini bul
        C_val = ALPHABET.find(char)
        
        # Anahtar harfinin sayısal değerini bul (anahtarı döngüsel olarak kullan)
        K_val = ALPHABET.find(key[key_index % len(key)])
        
        # Deşifreleme işlemini yap: (C - K) mod 26
        # Python'da negatif sayıların modulo işlemi doğru sonucu verir.
        P_val = (C_val - K_val) % len(ALPHABET)
        
        # Sayısal değeri tekrar harfe çevir
        plaintext += ALPHABET[P_val]
        
        # Anahtar döngüsünün indeksini bir artır
        key_index += 1
        
    return plaintext

# --- ÖRNEK KULLANIM ---
KEY = "abracadabra"
PLAINTEXT = "HELLO WORLD"

# Şifreleme
CIPHERTEXT = vigenere_encrypt(PLAINTEXT, KEY)
print(f"Anahtar: {KEY}")
print(f"Düz Metin: {PLAINTEXT}")
print(f"Şifreli Metin: {CIPHERTEXT}")

# Çözme
DECRYPTED_TEXT = vigenere_decrypt("YPL ATE WHF SESU JTWDHNUJ I HPGE AOX WJCL HBME C VHRZ XOOE WUVUUE", KEY)
print(f"Çözülen Metin: {DECRYPTED_TEXT}")

#YPL ATE WHF SESU JTWDHNUJ I HPGE AOX WJCL HBME C VHRZ XOOE WUVUUE
#abr aca dab raab racadabr a abra cad abra abra c adab raab racada
#
#you are the best students i hope you will have a very good future