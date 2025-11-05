import string

# ----------------------------------------------------
# 1. Anahtar Kareyi Oluşturma
# ----------------------------------------------------

def create_playfair_square(key):
    """Verilen anahtar kelime ile 5x5 Playfair karesini oluşturur."""
    
    # J harfini I ile değiştir ve tüm harfleri büyük harf yap
    key = key.upper().replace('J', 'I')
    
    # Temiz alfabeyi tanımla (I/J birleşik)
    alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ"
    
    # Anahtar kelime harflerini benzersiz olarak listeye ekle
    square_list = []
    for char in key:
        if char not in square_list and char in alphabet:
            square_list.append(char)
            
    # Kalan alfabe harflerini ekle
    for char in alphabet:
        if char not in square_list:
            square_list.append(char)
            
    # Listeyi 5x5 matrise dönüştür
    square = [square_list[i:i + 5] for i in range(0, 25, 5)]
    return square

# ----------------------------------------------------
# 2. Harflerin Konumunu Bulma
# ----------------------------------------------------

def find_position(square, char):
    """Bir harfin matristeki (satır, sütun) koordinatlarını bulur."""
    for row in range(5):
        for col in range(5):
            if square[row][col] == char:
                return row, col
    return -1, -1 # Bulunamazsa -1 döndür

# ----------------------------------------------------
# 3. Düz Metni Hazırlama (Şifreleme İçin)
# ----------------------------------------------------

def prepare_text(plaintext):
    """Düz metni şifrelemeye hazır hale getirir (temizleme, I/J, digraf, dolgu)."""
    
    # Harfleri temizle ve J -> I
    text = "".join(c for c in plaintext.upper() if c.isalpha()).replace('J', 'I')
    
    # Aynı harfleri ayırma ve çiftlere ayırma
    prepared_text = ""
    i = 0
    while i < len(text):
        a = text[i]
        
        # Tek harf kalırsa (sonunda veya aynı harf denk gelirse)
        if i == len(text) - 1:
            prepared_text += a + 'X' # Sona X ekle
            i += 1
        else:
            b = text[i+1]
            if a == b:
                prepared_text += a + 'X'
                i += 1
            else:
                prepared_text += a + b
                i += 2
                
    return prepared_text

# ----------------------------------------------------
# 4. Şifreleme Fonksiyonu
# ----------------------------------------------------

def playfair_encrypt(plaintext, key):
    square = create_playfair_square(key)
    text = prepare_text(plaintext)
    ciphertext = ""
    
    for i in range(0, len(text), 2):
        a, b = text[i], text[i + 1]
        r1, c1 = find_position(square, a)
        r2, c2 = find_position(square, b)
        
        # Kural 1: Aynı Satır
        if r1 == r2:
            new_a = square[r1][(c1 + 1) % 5]
            new_b = square[r2][(c2 + 1) % 5]
        # Kural 2: Aynı Sütun
        elif c1 == c2:
            new_a = square[(r1 + 1) % 5][c1]
            new_b = square[(r2 + 1) % 5][c2]
        # Kural 3: Dikdörtgen
        else:
            new_a = square[r1][c2]
            new_b = square[r2][c1]
            
        ciphertext += new_a + new_b
        
    return ciphertext

# ----------------------------------------------------
# 5. Çözme Fonksiyonu
# ----------------------------------------------------

def playfair_decrypt(ciphertext, key):
    square = create_playfair_square(key)
    plaintext = ""
    
    for i in range(0, len(ciphertext), 2):
        a, b = ciphertext[i], ciphertext[i + 1]
        r1, c1 = find_position(square, a)
        r2, c2 = find_position(square, b)
        
        # Kural 1: Aynı Satır (Çözme: Sola Kaydır)
        if r1 == r2:
            new_a = square[r1][(c1 - 1) % 5]
            new_b = square[r2][(c2 - 1) % 5]
        # Kural 2: Aynı Sütun (Çözme: Yukarı Kaydır)
        elif c1 == c2:
            new_a = square[(r1 - 1) % 5][c1]
            new_b = square[(r2 - 1) % 5][c2]
        # Kural 3: Dikdörtgen (Çözme: Aynı kural)
        else:
            new_a = square[r1][c2]
            new_b = square[r2][c1]
            
        plaintext += new_a + new_b

    # Çözülen metinden dolgu 'X' harflerini kaldırmak için (basit bir yaklaşım)
    final_plaintext = ""
    i = 0
    while i < len(plaintext):
        char = plaintext[i]
        
        # X'i kaldır: 
        # 1. Eğer X sonda ise (tekrar bir harf gelmeyecekse)
        # 2. Eğer X kendisinden önce veya sonra gelen harfle aynı ise (örneğin A X A) 
        #    - bu kural aynı harf dolgusundan gelen X'ler için kullanılır (örneğin 'BALXLO' -> 'BALLO')
        if char == 'X' and (i == len(plaintext) - 1 or plaintext[i-1] == plaintext[i+1]):
             pass
        else:
             final_plaintext += char
             
        i += 1
        
    # Sonda X varsa onu da kontrol et
    if final_plaintext.endswith('X'):
        final_plaintext = final_plaintext[:-1]

    # I harflerini J'ye geri dönüştürme (isteğe bağlı, bağlama göre)
    # Bu adım genelde şifreleme öncesinde J'ler I yapıldığı için metin içinde I olarak kalabilir.
    # Ancak basitlik için burada metne dokunmuyoruz.
    
    return final_plaintext

# ----------------------------------------------------
# ÖRNEK KULLANIM
# ----------------------------------------------------

# Anahtar Kelime
KEY = "EDUCATION"

# Düz Metin
PLAINTEXT = "we are hackers as you know"

# Şifreleme
print(f"Anahtar: {KEY}")
print(f"Düz Metin: {PLAINTEXT}")
CIPHERTEXT = playfair_encrypt(PLAINTEXT, KEY)
print(f"Şifreli Metin: {CIPHERTEXT}")

# Çözme
DECRYPTED_TEXT = playfair_decrypt(CIPHERTEXT, KEY)
print(f"Çözülen Metin: {DECRYPTED_TEXT}")

#E D U C A  
#T I O N B  
#F G H K L  
#M P Q R S  
#V W X Y Z 
#we ar eh ac ke rs as yo uk no wx
#VD CS UF EA FC SM BZ XN CH BN XY