-- Kentimce demo data
-- Run once on an empty database, after `dotnet ef database update`.
-- All demo users share the password: Demo123! (stored as a BCrypt hash)

BEGIN;

-- Provinces (il) and districts (ilce)
INSERT INTO iller ("Id", "Isim", "ip") VALUES
  ('11000000-0000-0000-0000-000000000001', 'İstanbul', 'seed'),
  ('11000000-0000-0000-0000-000000000002', 'Ankara', 'seed');

INSERT INTO ilceler ("Id", "IlId", "Isim", "ip") VALUES
  ('12000000-0000-0000-0000-000000000001', '11000000-0000-0000-0000-000000000001', 'Kadıköy', 'seed'),
  ('12000000-0000-0000-0000-000000000002', '11000000-0000-0000-0000-000000000001', 'Beşiktaş', 'seed'),
  ('12000000-0000-0000-0000-000000000003', '11000000-0000-0000-0000-000000000002', 'Çankaya', 'seed');

-- Municipalities (belediye)
INSERT INTO belediyeler ("Id", "IlId", "IlceId", "Isim", "Tip", "Adres", "Domain", "ip") VALUES
  ('13000000-0000-0000-0000-000000000001', '11000000-0000-0000-0000-000000000001', NULL,
   'İstanbul Büyükşehir Belediyesi', 'Büyükşehir', 'Fatih, İstanbul', 'ibb.istanbul', 'seed'),
  ('13000000-0000-0000-0000-000000000002', '11000000-0000-0000-0000-000000000001', '12000000-0000-0000-0000-000000000001',
   'Kadıköy Belediyesi', 'İlçe', 'Kadıköy, İstanbul', 'kadikoy.bel.tr', 'seed'),
  ('13000000-0000-0000-0000-000000000003', '11000000-0000-0000-0000-000000000001', '12000000-0000-0000-0000-000000000002',
   'Beşiktaş Belediyesi', 'İlçe', 'Beşiktaş, İstanbul', 'besiktas.bel.tr', 'seed'),
  ('13000000-0000-0000-0000-000000000004', '11000000-0000-0000-0000-000000000002', '12000000-0000-0000-0000-000000000003',
   'Çankaya Belediyesi', 'İlçe', 'Çankaya, Ankara', 'cankaya.bel.tr', 'seed');

-- Roles: Vatandas = citizen, Personel = municipal staff, Admin = moderator
INSERT INTO roller ("Id", "Isim", "ip") VALUES
  ('14000000-0000-0000-0000-000000000001', 'Vatandas', 'seed'),
  ('14000000-0000-0000-0000-000000000002', 'Personel', 'seed'),
  ('14000000-0000-0000-0000-000000000003', 'Admin', 'seed');

-- Complaint categories
INSERT INTO "SikayetKategorileri" ("Id", "Ad", "Ikon", "RenkKodu", "AktifMi", "ip", "silindi") VALUES
  ('15000000-0000-0000-0000-000000000001', 'Yol ve Kaldırım', 'ri-road-map-line', 'warning', true, 'seed', false),
  ('15000000-0000-0000-0000-000000000002', 'Aydınlatma', 'ri-lightbulb-line', 'info', true, 'seed', false),
  ('15000000-0000-0000-0000-000000000003', 'Temizlik ve Çöp', 'ri-delete-bin-line', 'danger', true, 'seed', false),
  ('15000000-0000-0000-0000-000000000004', 'Park ve Bahçe', 'ri-tree-line', 'success', true, 'seed', false),
  ('15000000-0000-0000-0000-000000000005', 'Su ve Kanalizasyon', 'ri-drop-line', 'primary', true, 'seed', false),
  ('15000000-0000-0000-0000-000000000006', 'Toplu Ulaşım', 'ri-bus-line', 'secondary', true, 'seed', false);

-- Demo users (fake data, email already verified)
INSERT INTO kullanicilar ("Id", "Email", "Telefon", "Ad", "Soyad", "Tc", "ParolaHash", "EmailDogrulandiMi", "BelediyeId", "ip") VALUES
  ('16000000-0000-0000-0000-000000000001', 'admin@kentimce.demo', '05550000001', 'Deniz', 'Moderatör',
   '10000000001', '$2a$11$yH7ooAtcYDSKGhIgiv7lKuOQdYWVB3jQEz1eSGY3EEVk2Oduxf4Sy', true, NULL, 'seed'),
  ('16000000-0000-0000-0000-000000000002', 'personel@kentimce.demo', '05550000002', 'Zeynep', 'Kaya',
   '10000000002', '$2a$11$yH7ooAtcYDSKGhIgiv7lKuOQdYWVB3jQEz1eSGY3EEVk2Oduxf4Sy', true, '13000000-0000-0000-0000-000000000002', 'seed'),
  ('16000000-0000-0000-0000-000000000003', 'ayse@kentimce.demo', '05550000003', 'Ayşe', 'Yılmaz',
   '10000000003', '$2a$11$yH7ooAtcYDSKGhIgiv7lKuOQdYWVB3jQEz1eSGY3EEVk2Oduxf4Sy', true, NULL, 'seed'),
  ('16000000-0000-0000-0000-000000000004', 'mehmet@kentimce.demo', '05550000004', 'Mehmet', 'Demir',
   '10000000004', '$2a$11$yH7ooAtcYDSKGhIgiv7lKuOQdYWVB3jQEz1eSGY3EEVk2Oduxf4Sy', true, NULL, 'seed');

INSERT INTO "kullaniciRolleri" ("KullaniciId", "RolId", "ip") VALUES
  ('16000000-0000-0000-0000-000000000001', '14000000-0000-0000-0000-000000000003', 'seed'),
  ('16000000-0000-0000-0000-000000000002', '14000000-0000-0000-0000-000000000002', 'seed'),
  ('16000000-0000-0000-0000-000000000003', '14000000-0000-0000-0000-000000000001', 'seed'),
  ('16000000-0000-0000-0000-000000000004', '14000000-0000-0000-0000-000000000001', 'seed');

-- Complaints: published, resolved and waiting for moderation
INSERT INTO sikayetler ("Id", "KullaniciId", "BelediyeId", "KategoriId", "Baslik", "Aciklama", "Adres", "Durum", "OnInceleme", "OlusturmaTarihi", "eklemeTarihi", "ip") VALUES
  ('17000000-0000-0000-0000-000000000001', '16000000-0000-0000-0000-000000000003', '13000000-0000-0000-0000-000000000002', '15000000-0000-0000-0000-000000000001',
   'Moda Caddesi''nde derin çukur',
   'Moda Caddesi üzerinde, eczanenin önünde yaklaşık yarım metre çapında bir çukur var. Akşamları görünmüyor, araçlar ve bisikletliler için tehlikeli.',
   'Moda Cd., Kadıköy / İstanbul', 'Yayinda', false, now() - interval '3 days', now() - interval '3 days', 'seed'),
  ('17000000-0000-0000-0000-000000000002', '16000000-0000-0000-0000-000000000004', '13000000-0000-0000-0000-000000000002', '15000000-0000-0000-0000-000000000002',
   'Yoğurtçu Parkı girişinde lambalar yanmıyor',
   'Parkın sahil tarafındaki girişinde üç sokak lambası bir haftadır yanmıyor. Akşam yürüyüşü yapanlar için güvenlik sorunu oluşturuyor.',
   'Yoğurtçu Parkı, Kadıköy / İstanbul', 'Yayinda', false, now() - interval '2 days', now() - interval '2 days', 'seed'),
  ('17000000-0000-0000-0000-000000000003', '16000000-0000-0000-0000-000000000004', '13000000-0000-0000-0000-000000000003', '15000000-0000-0000-0000-000000000003',
   'Çarşı''da çöp konteynerleri taşıyor',
   'Beşiktaş Çarşı''daki konteynerler hafta sonu boyunca boşaltılmadı. Çöpler kaldırıma taşmış durumda ve kötü koku var.',
   'Beşiktaş Çarşı, Beşiktaş / İstanbul', 'Yayinda', false, now() - interval '1 day', now() - interval '1 day', 'seed'),
  ('17000000-0000-0000-0000-000000000004', '16000000-0000-0000-0000-000000000003', '13000000-0000-0000-0000-000000000002', '15000000-0000-0000-0000-000000000004',
   'Çocuk parkındaki salıncak kırık',
   'Fenerbahçe Parkı''ndaki çocuk oyun alanında salıncaklardan birinin zinciri kopmuş. Çocuklar için tehlikeli.',
   'Fenerbahçe Parkı, Kadıköy / İstanbul', 'Cozuldu', false, now() - interval '12 days', now() - interval '12 days', 'seed'),
  ('17000000-0000-0000-0000-000000000005', '16000000-0000-0000-0000-000000000004', '13000000-0000-0000-0000-000000000004', '15000000-0000-0000-0000-000000000005',
   'Tunalı Hilmi''de su borusu patladı',
   'Cadde üzerinde su borusu patladı, yol su altında kaldı. Çevredeki binalarda su kesintisi var.',
   'Tunalı Hilmi Cd., Çankaya / Ankara', 'Cozuldu', false, now() - interval '20 days', now() - interval '20 days', 'seed'),
  ('17000000-0000-0000-0000-000000000006', '16000000-0000-0000-0000-000000000004', '13000000-0000-0000-0000-000000000002', '15000000-0000-0000-0000-000000000006',
   'Otobüs durağının camı kırık',
   'Söğütlüçeşme yönündeki durağın yan camı kırılmış, cam parçaları hâlâ yerde duruyor.',
   'Kadıköy Rıhtım Durağı, Kadıköy / İstanbul', 'Yayinda', false, now() - interval '6 hours', now() - interval '6 hours', 'seed'),
  ('17000000-0000-0000-0000-000000000007', '16000000-0000-0000-0000-000000000003', '13000000-0000-0000-0000-000000000003', '15000000-0000-0000-0000-000000000001',
   'Kaldırım taşları yerinden çıkmış',
   'Barbaros Bulvarı''nda kaldırım taşları yerinden çıkmış, yaşlılar ve bebek arabaları için yürümek zor.',
   'Barbaros Blv., Beşiktaş / İstanbul', 'OnayBekliyor', true, now() - interval '2 hours', now() - interval '2 hours', 'seed'),
  ('17000000-0000-0000-0000-000000000008', '16000000-0000-0000-0000-000000000004', '13000000-0000-0000-0000-000000000004', '15000000-0000-0000-0000-000000000002',
   'Kuğulu Park çevresi çok karanlık',
   'Parkın arka tarafındaki yürüyüş yolunda aydınlatma yok. Akşam saatlerinde geçmek tedirgin edici.',
   'Kuğulu Park, Çankaya / Ankara', 'OnayBekliyor', true, now() - interval '5 hours', now() - interval '5 hours', 'seed');

-- Status history (1 = Yayinda, 3 = Cozuldu)
INSERT INTO "sikayetDurumlari" ("SikayetId", "Durum", "Aciklama", "IslemYapanId", "IslemTarihi", "ip") VALUES
  ('17000000-0000-0000-0000-000000000001', 1, 'Moderatör tarafından onaylandı.', '16000000-0000-0000-0000-000000000001', now() - interval '3 days' + interval '1 hour', 'seed'),
  ('17000000-0000-0000-0000-000000000004', 1, 'Moderatör tarafından onaylandı.', '16000000-0000-0000-0000-000000000001', now() - interval '12 days' + interval '1 hour', 'seed'),
  ('17000000-0000-0000-0000-000000000004', 3, 'Salıncak zinciri değiştirildi, oyun alanı kontrol edildi.', '16000000-0000-0000-0000-000000000002', now() - interval '9 days', 'seed'),
  ('17000000-0000-0000-0000-000000000005', 1, 'Moderatör tarafından onaylandı.', '16000000-0000-0000-0000-000000000001', now() - interval '20 days' + interval '1 hour', 'seed'),
  ('17000000-0000-0000-0000-000000000005', 3, 'Arıza giderildi, su verilmeye başlandı.', '16000000-0000-0000-0000-000000000001', now() - interval '19 days', 'seed');

-- Comments (the staff comment is shown as an official reply)
INSERT INTO "sikayetYorumlari" ("SikayetId", "KullaniciId", "Yorum", "eklemeTarihi", "ip") VALUES
  ('17000000-0000-0000-0000-000000000001', '16000000-0000-0000-0000-000000000004', 'Dün akşam ben de aynı çukura denk geldim, acil bakılması lazım.', now() - interval '2 days', 'seed'),
  ('17000000-0000-0000-0000-000000000001', '16000000-0000-0000-0000-000000000002', 'Talebiniz ilgili ekibimize iletildi. Yarın sabah bölgede çalışma yapılacaktır.', now() - interval '1 day', 'seed'),
  ('17000000-0000-0000-0000-000000000004', '16000000-0000-0000-0000-000000000003', 'Hızlı çözüm için teşekkürler!', now() - interval '8 days', 'seed');

-- Likes
INSERT INTO "sikayetBegenileri" ("SikayetId", "KullaniciId", "ip") VALUES
  ('17000000-0000-0000-0000-000000000001', '16000000-0000-0000-0000-000000000004', 'seed'),
  ('17000000-0000-0000-0000-000000000002', '16000000-0000-0000-0000-000000000003', 'seed'),
  ('17000000-0000-0000-0000-000000000003', '16000000-0000-0000-0000-000000000003', 'seed'),
  ('17000000-0000-0000-0000-000000000006', '16000000-0000-0000-0000-000000000003', 'seed');

-- Notifications for the citizen user
INSERT INTO "Bildirimler" ("Id", "KullaniciId", "Mesaj", "Link", "OkunduMu", "Tarih", "ip", "silindi") VALUES
  (gen_random_uuid(), '16000000-0000-0000-0000-000000000003',
   '''Moda Caddesi''nde derin çukur'' başlıklı şikayetiniz onaylandı ve yayına alındı.',
   '/sikayet-detay/17000000-0000-0000-0000-000000000001', false, now() - interval '3 days' + interval '1 hour', 'seed', false),
  (gen_random_uuid(), '16000000-0000-0000-0000-000000000003',
   '''Çocuk parkındaki salıncak kırık'' başlıklı şikayetiniz çözüldü olarak işaretlendi.',
   '/sikayet-detay/17000000-0000-0000-0000-000000000004', true, now() - interval '9 days', 'seed', false);

-- Profanity filter sample entries
INSERT INTO "kotuKelimeListesi" ("Kelime", "ip") VALUES
  ('aptal', 'seed'),
  ('salak', 'seed'),
  ('gerizekalı', 'seed');

COMMIT;
