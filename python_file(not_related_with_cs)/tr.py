import math

def encrypt_transposition_numeric(plaintext, key_order):
    """
    Bir mesajı sayı dizisi anahtarıyla şifreler (1'den başlayan sıralama).
    
    Args:
        plaintext (str): Şifrelenecek mesaj.
        key_order (list/tuple): Sütunların okunma sırasını belirten sayı dizisi (1'den N'e kadar).
                                Örn: [3, 1, 2, 5, 4]
        
    Returns:
        str: Şifrelenmiş metin (ciphertext).
    """
    # 1. Metni hazırla ve anahtarı dönüştür
    plaintext = plaintext.upper().replace(' ', '')
    key_length = len(key_order)
    
    # Anahtarın 1'den başladığını varsayarak, 0-tabanlı indislemeye uygun hale getir.
    # [3, 1, 2, 5, 4] -> Sütun 1, Sütun 2, ...
    # Okuma sırasını (0-tabanlı indisler) belirleyen bir liste oluştur.
    # Örneğin: [3, 1, 2, 5, 4] ise:
    # 1. sıradaki sütun: 1. indis (değer 1)
    # 2. sıradaki sütun: 2. indis (değer 2)
    # 3. sıradaki sütun: 0. indis (değer 3)
    # ...
    
    # Sütun indislerini (0'dan başlayan) okuma sırasına göre tutacak liste:
    read_order_indices = [0] * key_length
    for i, position in enumerate(key_order):
        # position (okuma sırası, 1'den başlayan) -> position - 1 (0-tabanlı okuma sırası)
        # i (sütun indisi, 0'dan başlayan)
        read_order_indices[position - 1] = i
        
    # 2. Izgara boyutlarını belirle
    num_rows = math.ceil(len(plaintext) / key_length)
    
    # 3. Izgarayı oluştur ve satır satır doldur
    grid = [['' for _ in range(key_length)] for _ in range(num_rows)]
    
    text_index = 0
    for r in range(num_rows):
        for c in range(key_length):
            if text_index < len(plaintext):
                grid[r][c] = plaintext[text_index]
                text_index += 1
            
    # 4. Sütunları belirlenen okuma sırasına göre oku
    ciphertext = []
    
    # read_order_indices listesi, hangi 0-tabanlı sütunun (c) hangi sırada okunacağını söyler.
    for col_index in read_order_indices:
        for r in range(num_rows):
            char = grid[r][col_index]
            if char:
                ciphertext.append(char)
                
    return "".join(ciphertext)

def decrypt_transposition_numeric(ciphertext, key_order):
    """
    Sayı dizisi anahtarıyla şifrelenmiş bir mesajı çözer.
    
    Args:
        ciphertext (str): Çözülecek mesaj.
        key_order (list/tuple): Sütunların okunma sırasını belirten sayı dizisi (1'den N'e kadar).
        
    Returns:
        str: Çözülmüş düz metin (plaintext).
    """
    # 1. Anahtarı hazırla ve ızgara boyutlarını hesapla
    key_length = len(key_order)
    cipher_len = len(ciphertext)
    
    num_rows = math.ceil(cipher_len / key_length)
    
    # Tam dolu sütun sayısını hesapla
    num_empty_cells = (num_rows * key_length) - cipher_len
    num_full_cols = key_length - num_empty_cells
    
    # 0-tabanlı sütun indislerini okuma sırasına göre al (Şifreleme fonksiyonundaki ile aynı mantık)
    read_order_indices = [0] * key_length
    for i, position in enumerate(key_order):
        read_order_indices[position - 1] = i

    # 2. Izgara yapısını yeniden oluştur
    grid = [['' for _ in range(key_length)] for _ in range(num_rows)]
    
    # 3. Şifreli metni ızgaraya *sütun sütun* okuma sırasına göre yerleştir
    cipher_index = 0
    
    # Okuma sırasına göre sütunları doldur
    for col_index in read_order_indices:
        # Bu sütunun tam dolu olup olmadığını kontrol et (num_rows veya num_rows-1)
        # col_index, 0-tabanlı sütun indisidir.
        is_full_column = True
        
        # Hangi sütunların eksik olduğunu bulmak için, key_order'daki değerlere ihtiyacımız var.
        # En yüksek "position" değerine (yani key_length'e) sahip olan sütunlar en son gelir.
        # Key_order'daki "position" değeri, sütunun doluluk durumuyla doğrudan ilişkilidir.
        
        # Daha basit bir yol: Sütunun 0-tabanlı indisi (i) ile, 
        # o sütunun key_order'daki değeri (position) arasındaki ilişkiyi kullanırız.
        
        # Hangi sütunların dolu olduğunu belirlemek için:
        # Eğer bir sütunun indisi (i), num_full_cols'den küçükse *ve*
        # o sütunun key_order'daki değeri küçükse (yani erken okunmuşsa)...
        # Bu biraz karmaşık, bunun yerine daha basit bir kural izleyelim:
        
        # Sütunlar *mantıksal* olarak tam doludan eksik doluya doğru sıralanır.
        
        # Kolon indisi (0-tabanlı) key_order içinde kaçıncı sırada?
        # Örneğin key [3, 1, 2, 5, 4] ise,
        # Sütun 0'ın okuma sırası 3.
        # Sütun 1'in okuma sırası 1.
        
        # Hangi sütunların boş hücre içerdiğini bulmak için,
        # key_order'daki hangi $i$ (0-tabanlı indis) için $key\_order[i] > num\_full\_cols$ (değil, bu yanlış bir düşünce)
        
        # En basit kural: Sadece ilk `num_full_cols` adet sütun, en fazla sayıda karaktere (num_rows) sahiptir.
        # Ancak bu, sütunların *okunma sırasına* göre değil, *indis sırasına* göre doğru olurdu.
        
        # Doğru kural: Okunan sütunlar arasında, *indisleri* ilk `num_full_cols` içinde olanlar `num_rows` uzunluğundadır.
        
        # Yani, col_index (0-tabanlı) ilk num_full_cols sütundan biriyse tam doludur.
        # Ancak bu da yanlış. İlk num_full_cols sütun, *okunma sırasına göre* tam doludur.

        # *** Basit Çözüm: ***
        # `read_order_indices` listesini okurken, ilk `num_full_cols` kadar okunan sütun tam dolu, diğerleri eksik.
        
        current_read_position = read_order_indices.index(col_index) # col_index'in okuma sırası (0'dan başlayan)
        
        current_col_size = num_rows
        # Eğer okuma sırası, tam dolu sütun sayısından sonra geliyorsa, o sütun eksiktir.
        if current_read_position >= num_full_cols:
            current_col_size = num_rows - 1
            
        # Sütunun hücrelerini doldur
        for r in range(current_col_size):
            grid[r][col_index] = ciphertext[cipher_index]
            cipher_index += 1

    # 4. Izgarayı *satır satır* oku ve düz metni oluştur
    plaintext = []
    for r in range(num_rows):
        for c in range(key_length):
            char = grid[r][c]
            if char:
                plaintext.append(char)
                
    return "".join(plaintext)

# --- Örnek Kullanım ---

# Şifrelenecek metin
plaintext = "retreat to the hills"
# Anahtar (5 sütunlu bir şifre için: 3. sütunu oku, sonra 1.yi, sonra 2.yi, sonra 5.yi, sonra 4.yü)
key_numeric = [4, 1, 3, 2, 5] 

# Şifreleme
ciphertext = encrypt_transposition_numeric(plaintext, key_numeric)
print(f"Düz Metin:  {plaintext}")
print(f"Anahtar:    {key_numeric}")
print(f"Şifreli Metin: {ciphertext}")

print("-" * 30)

# Şifre Çözme
decrypted_plaintext = decrypt_transposition_numeric(ciphertext, key_numeric)
print(f"Şifresi Çözülmüş Metin: {decrypted_plaintext}")

# Kontrol
original_text_cleaned = plaintext.upper().replace(' ', '')
print(f"Doğrulama Başarılı: {original_text_cleaned == decrypted_plaintext}")