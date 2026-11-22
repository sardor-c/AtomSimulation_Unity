# 00. Muhit va loyiha: Unity, Rider va Git

## Bu darsda nima qilamiz va nega aynan hozir

Bu darsda atom ham, fizika ham yo'q. Natija bitta: Unity loyihasini ochasiz, uning qanday tuzilganini tushunasiz,
Rider'ni ulaysiz, o'z sahnangizni yaratasiz va hammasini Git orqali GitHub'ga yuklaysiz.

Kurs shundan boshlanishining sababi oddiy: keyingi darslarning hammasi shu poydevor ustida turadi. Elektron
aylanmasa yoki shader qora chiqsa, sababni kod va fizikadan qidiramiz. Shu paytda "Rider skriptni ochmayapti",
"fayl qayoqqadir yo'qoldi" yoki "boshqa kompyuterda skript topilmadi" degan muammolar aralashib ketmasligi kerak.

Loyihani men siz uchun yaratib qo'ydim (Unity 6000.6.9f1, URP shabloni, yonidagi `AtomSimulation` bilan bir xil
versiya va paketlar). Bundan keyingi hamma ishni o'zingiz qilasiz.

Dars oxirida loyiha shunday ko'rinadi (faqat muhim qismlari):

```
AtomSimulation_Scratch/
├── .gitignore              (siz yozasiz)
├── CLAUDE.md               (Claude yozadi: kurs holati)
├── docs/                   (Claude yozadi: darslar)
├── Assets/
│   ├── AtomSim/            (siz yaratasiz: bizning hamma narsamiz shu yerda)
│   │   └── Scenes/
│   │       └── AtomSimulation.unity
│   ├── Scenes/SampleScene.unity     (shablondan)
│   └── Settings/                     (shablondan: URP sozlamalari)
├── Packages/
│   └── manifest.json
└── ProjectSettings/
```

## Dars qanday tuzilgan

Bu va keyingi darslar **qadam-baqadam qo'llanma** ko'rinishida. Har bir qadam uch qismdan iborat:

1. **Nazariya**: bu narsa nima uchun kerak va qanday ishlaydi.
2. **Amal**: Editor'da nima qilish yoki qaysi faylga qanday kod yozish.
3. **Ishga tushirib ko'ring**: nima ko'rinishi kerakligini tekshirasiz.

Har bir qadamdan keyin loyiha ishlaydigan holatda bo'ladi. Biror qadamda kutilgan natija chiqmasa, keyingisiga
o'tmang: xato aynan shu qadamda qilingan bir necha ishda.

| Qadam | Nima qilamiz | Natija |
|---|---|---|
| 1 | Unity nima, loyihani ochamiz | Editor ochiladi |
| 2 | Loyiha papkalarini o'rganamiz | qaysi fayl nimaga kerakligini bilasiz |
| 3 | Editor oynalari va Play rejimi | namunaviy sahnani Play'da ko'rasiz |
| 4 | Rider'ni ulaymiz | skript ikki marta bosilganda Rider'da ochiladi |
| 5 | O'z sahnamizni yaratamiz | Play'da deyarli qora "kosmos" |
| 6 | Git va GitHub | loyiha GitHub'da |

**Kod qanday ko'rsatiladi** (01-darsdan boshlab kerak bo'ladi). Yangi fayl to'liq beriladi. Mavjud faylga
kiritiladigan o'zgarish esa **diff** ko'rinishida ko'rsatiladi:

```diff
         void Update()
         {
-            m_Angle += 2.4f * Time.deltaTime;
+            m_Angle += m_AngularSpeed * Time.deltaTime;
         }
```

- `-` bilan boshlangan qatorni **o'chiring**.
- `+` bilan boshlangan qatorni **qo'shing**.
- Belgisiz qatorlar faylda allaqachon bor. Ular faqat joyni topish uchun ko'rsatilgan, ularga tegmang.
- `+` va `-` belgisining o'zini yozmang, faqat undan keyingi matnni bo'shliqlari bilan birga yozing.

Har bir qadam oxirida o'zgargan faylning to'liq holati yashirin blokda turadi: "▶" belgili qatorni bossangiz
ochiladi. Kodingizni u bilan solishtirishingiz mumkin.

## 1-qadam. Unity nima va loyiha qanday ochiladi

### 1.1. O'yin dvigateli (game engine)

Ekranda 3D atomni ko'rsatish uchun juda ko'p ish kerak: oyna ochish, videokarta bilan gaplashish, sichqoncha va
klaviaturani o'qish, matn va tugmalar chizish, kamerani hisoblash, tayyor dasturni `.exe` qilib yig'ish. Bularning
hech biri atom fizikasiga aloqador emas, lekin hammasi kerak.

**Unity** shularning hammasini tayyor qilib beradigan *o'yin dvigateli* (game engine). Uy qurishni tasavvur qiling.
Har bir g'ishtni o'zingiz pishirishingiz mumkin (OpenGL kursida shunday qildingiz: oynani GLFW bilan ochdingiz,
shader'ni o'zingiz kompilyatsiya qildingiz). Yoki tayyor g'isht, sement va kran olib, kuchingizni uyning o'ziga
sarflashingiz mumkin. Unity bu tayyor g'isht va kran. Bizning "uy" esa atom simulyatsiyasi.

Unity ichida ham GPU bilan aynan OpenGL'dagi kabi ishlar bo'ladi: bufer, shader, chizish buyrug'i. Farqi shundaki,
ularning ko'pini Unity o'zi qiladi, bizga esa kerak bo'lganda pastroq darajaga tushish imkonini beradi. 10-darsda
GPU'ga to'g'ridan-to'g'ri compute shader yozamiz.

### 1.2. Editor va Player

Unity ikki qismdan iborat:

- **Editor**: siz ishlaydigan dastur. Unda sahnani yig'asiz, sozlamalarni o'zgartirasiz, "Play" tugmasi bilan
  natijani darhol sinaysiz.
- **Player**: tayyor dastur. Kurs oxirida (16-dars) loyihani `.exe` qilib yig'amiz va u Editor'siz, istalgan
  Windows kompyuterida ishlaydi.

Editor ichidagi "Play" rejimi Player'ning deyarli aynan o'zi, shuning uchun kurs davomida faqat Editor bilan
ishlaymiz.

### 1.3. Unity Hub va versiya

Kompyuterda bir nechta Unity versiyasi o'rnatilgan bo'lishi mumkin. Ularni **Unity Hub** boshqaradi: qaysi
versiyalar o'rnatilgan, qaysi loyiha qaysi versiyada ochiladi.

Bizning versiya **6000.6.9f1** (Unity 6.6). Bu raqamlar shunday o'qiladi:

| Qism | Ma'nosi |
|---|---|
| `6000` | Unity 6 avlodi |
| `6` | Unity 6 ichidagi 6-yangilanish (6.6) |
| `9` | tuzatishlar raqami |
| `f1` | "final", ya'ni barqaror reliz. `a` (alpha) va `b` (beta) sinov versiyalari |

Loyiha qaysi versiyada yaratilganini `ProjectSettings/ProjectVersion.txt` fayli eslab qoladi:

```
m_EditorVersion: 6000.6.9f1
m_EditorVersionWithRevision: 6000.6.9f1 (8c45277fb9c5)
```

Versiya muhim, chunki loyihani boshqa versiyada ochsangiz, Unity uni o'sha versiyaga "ko'chiradi" va ortga qaytarib
bo'lmasligi mumkin. Kurs davomida versiyani o'zgartirmaymiz.

### 1.4. Loyihani oching

1. **Unity Hub**'ni oching, chapda **Projects**.
2. Ro'yxatda `AtomSimulation_Scratch` turishi kerak (yaratganimda Hub'ga qo'shib qo'yildi). "Editor version"
   ustunida `6000.6.9f1`.
3. Agar ro'yxatda bo'lmasa: yuqoridagi **Add** tugmasi yonidagi strelka → **Add project from disk** →
   `C:\dev\UNITY\AtomSimulation_Scratch` papkasini tanlang.
4. Loyihani bosing. Birinchi ochilish biroz vaqt oladi.

### 1.5. Ishga tushirib ko'ring

- Unity Editor ochiladi, sarlavhada `AtomSimulation_Scratch - SampleScene - Windows, Mac, Linux - Unity 6.6 ...`
  ko'rinishidagi yozuv bo'ladi (aniq matn biroz farq qilishi mumkin).
- O'ngdagi **Inspector** oynasida shablonning tanishtiruv sahifasi (Readme) ochilgan bo'lishi mumkin. Unga
  e'tibor bermang, darsning oxirida xohlasangiz o'chirasiz.
- Pastdagi **Console** oynasida qizil xato bo'lmasligi kerak. Console ko'rinmasa: **Window → General → Console**.

## 2-qadam. Loyiha papkalari

Unity loyihasi oddiy papka. Uni Explorer'da oching (`C:\dev\UNITY\AtomSimulation_Scratch`) va har bir qism nima
ekanini ko'rib chiqamiz. Qaysi papka nimaga kerakligini bilish 6-qadamda (Git) hal qiluvchi bo'ladi.

### 2.1. `Assets`: loyihaning o'zi

Bu papkada siz yaratadigan hamma narsa turadi: sahnalar, skriptlar, materiallar, shader'lar, rasmlar. Editor'dagi
**Project** oynasi aynan shu papkani ko'rsatadi. Hozir unda shablondan kelgan narsalar bor:

| Narsa | Nima |
|---|---|
| `Scenes/SampleScene.unity` | namunaviy sahna |
| `Settings/` | URP sozlamalari (render pipeline, 3-qadamda) |
| `InputSystem_Actions.inputactions` | Input System uchun namunaviy tugmalar xaritasi (biz ishlatmaymiz) |
| `TutorialInfo/`, `Readme.asset` | shablonning tanishtiruv sahifasi |

**Muhim qoida: `Assets` ichidagi fayllarni Explorer'da emas, Unity'ning Project oynasida ko'chiring va nomini
o'zgartiring.** Sababi keyingi bo'limda.

### 2.2. `.meta` fayllar va GUID

Explorer'da `Assets` ichiga qarang: har bir fayl va papka yonida xuddi shu nomli, oxiri `.meta` bo'lgan fayl bor.
Masalan, `SampleScene.unity` yonida `SampleScene.unity.meta`:

```
fileFormatVersion: 2
guid: 99c9720ab356a0642a771bea13969a05
DefaultImporter:
  ...
```

**GUID** (Globally Unique Identifier) bu faylning "pasport raqami": 32 ta belgidan iborat tasodifiy son, dunyoda
takrorlanmaydi. Unity fayllarni bir-biriga **nomi bilan emas, GUID bilan** bog'laydi. Sahna "bu GameObject'da
`ElectronOrbit.cs` skripti bor" deb yozmaydi, "bu GameObject'da `3472aeba...` raqamli skript bor" deb yozadi.

Telefoningizdagi kontaktlarni tasavvur qiling. Do'stingiz ismini o'zgartirsa ham, raqami o'sha bo'lgani uchun
telefon uni taniydi. Raqam o'zgarsa, ism o'sha bo'lsa ham, eski kontakt endi ishlamaydi. Unity'da ism bu fayl nomi,
raqam esa GUID.

Shuning uchun:

- Faylni **Unity ichida** ko'chirsangiz, `.meta` u bilan birga ko'chadi, GUID saqlanadi, hamma bog'lanishlar
  ishlayveradi.
- `.meta` faylni **yo'qotsangiz**, fayl yangi GUID oladi va unga qilingan hamma havolalar uziladi.

Buni sinab ko'rdim. Qiziq tomoni: shu kompyuterda `.meta`ni o'chirsangiz, Unity uni **o'sha GUID bilan** qayta
yaratadi, chunki `Library` papkasida (2.5-bo'lim) eski GUID eslab qolingan. Muammo boshqa kompyuterda chiqadi:
`.meta`ni Git'ga qo'shishni unutsangiz, u yerda `Library` yangi, Unity faylga yangi tasodifiy GUID beradi va
sahnadagi skript yo'qoladi. GUID'ni qo'lda o'zgartirib, aynan shu holatni yasadim. Skript turgan GameObject'da
komponent "Missing" bo'lib qoldi, Play'da esa Console'da shu ogohlantirish chiqdi:

```
The referenced script (Unknown) on this Behaviour is missing!
```

Xulosa: **`.meta` fayllar Git'ga albatta kiradi.**

### 2.3. `Packages`: tashqi kutubxonalar

Unity'ning ko'p qismi alohida **paket** (package) bo'lib keladi. Qaysi paketlar kerakligi
`Packages/manifest.json` faylida yozilgan. Bizda muhimlari:

| Paket | Nima uchun |
|---|---|
| `com.unity.render-pipelines.universal` 17.6.0 | URP: sahnani rasmga aylantiruvchi tizim (3-qadamda) |
| `com.unity.inputsystem` 1.20.1 | klaviatura va sichqonchani o'qish (01-darsda) |
| `com.unity.ide.rider` 3.1.0 | Unity va Rider orasidagi ko'prik (4-qadamda) |
| `com.unity.ugui` | eski UI tizimi (biz yangisini, UI Toolkit'ni ishlatamiz, 14-dars) |
| `com.unity.cinemachine` | murakkab kameralar uchun (bizga kerak emas, zarari yo'q) |
| `com.unity.pipeline` | Claude Code'ga ochiq Editor'ga ulanib, ishingizni tekshirishga imkon beradi |
| `com.unity.modules.*` | dvigatelning o'z qismlari (audio, fizika, UI ...) |

Paketlarning o'zi internetdan yuklanadi va `Library/PackageCache` ga tushadi. Git'ga faqat ro'yxat
(`manifest.json` va `packages-lock.json`) kiradi. Bu OpenGL kursidagi `CMakeLists.txt` ga o'xshaydi: unda GLFW'ning
o'zi emas, "GLFW kerak" degan yozuv turardi.

Paketlarni Editor'da ko'rish: **Window → Package Management → Package Manager**.

### 2.4. `ProjectSettings`: loyiha sozlamalari

Editor'dagi **Edit → Project Settings...** oynasida o'zgartiradigan hamma narsa shu papkadagi fayllarga yoziladi:
grafika sifati, kirish (input) tizimi, loyiha nomi, Unity versiyasi. Bu ham Git'ga kiradi.

Ikkita sozlama keyingi darslarda kerak bo'ladi, hozir faqat bilib qo'ying:

- **Player → Other Settings → Active Input Handling = Input System Package (New)**. Eski `Input.GetKey` usuli
  o'chirilgan, faqat yangi Input System ishlaydi. 01-darsda shunga ko'ra kod yozamiz.
- **Editor → Enter Play Mode Settings** da **Reload Scene only** tanlangan, ya'ni "domain reload" o'chiq. Natijada
  Play juda tez boshlanadi. Bir oqibati bor: `static` o'zgaruvchilar Play'lar orasida qiymatini saqlab qoladi. Bunga
  kerak bo'lganda qaytamiz.

### 2.5. `Library`, `Temp`, `Logs`, `UserSettings`: Unity yaratadigan narsalar

**`Library`** bu Unity'ning keshi. Har bir rasm, model va shader'ni Unity bir marta o'zi uchun qulay formatga
o'giradi (bunga *import* deyiladi) va natijani shu yerda saqlaydi. Paketlar ham shu yerda. Loyiha yaratilgandan
keyin o'lchadim:

| Papka | Hajmi | Fayllar soni |
|---|---|---|
| `Library` | 986 MB | 22 085 |
| `Assets` | 220 KB | 38 |
| `ProjectSettings` | 117 KB | 28 |
| `Packages` | 20 KB | 2 |

Odam yozgan qism taxminan 360 KB, Unity yaratgani esa deyarli 1 GB. `Library`ni o'chirsangiz, Unity keyingi ochishda
uni `Assets` va `Packages`dan qaytadan yaratadi (biroz vaqt oladi). Shuning uchun u Git'ga kirmaydi.

- **`Temp`**: Unity ochiq turganda vaqtinchalik fayllar.
- **`Logs`**: Unity'ning jurnal fayllari.
- **`UserSettings`**: shaxsan sizning Editor sozlamalaringiz (oynalar joylashuvi kabi).

Bularning hech biri Git'ga kirmaydi.

### 2.6. `.csproj`, `.slnx` va `.vscode`

Loyiha ildizida `Assembly-CSharp.csproj`, `Assembly-CSharp-Editor.csproj` va `AtomSimulation_Scratch.slnx` bor.
Bular Rider (yoki Visual Studio) uchun: "qaysi `.cs` fayllar bor, ular qaysi kutubxonalarni ko'radi". Ularni
**Unity o'zi yaratadi** va har safar skript qo'shilganda qayta yozadi. Qo'lda tahrirlamang: o'zgarishingiz keyingi
safar o'chib ketadi. `.vscode` papkasi esa VS Code uchun xuddi shunday sozlamalar. Bular ham Git'ga kirmaydi.

`.slnx` bu yangi formatdagi "solution" fayli (eskisi `.sln` edi).

### 2.7. Ishga tushirib ko'ring

Editor'dagi **Project** oynasida `Assets/Scenes` papkasini oching. U yerda `SampleScene` bor, lekin
`SampleScene.unity.meta` yo'q. Project oynasi `.meta` fayllarni ko'rsatmaydi, chunki ular Unity'ning ichki ishi.
Explorer'da esa ikkalasi ham ko'rinadi.

## 3-qadam. Editor oynalari va Play rejimi

### 3.1. Asosiy oynalar

| Oyna | Odatda qayerda | Nima ko'rsatadi |
|---|---|---|
| **Hierarchy** | chapda | ochiq sahnadagi hamma obyektlar ro'yxati |
| **Scene** | o'rtada | sahna, uni aylanib ko'rish va obyektlarni joylashtirish uchun |
| **Game** | o'rtada, Scene yonida | kamera ko'rayotgan rasm, ya'ni foydalanuvchi ko'radigan narsa |
| **Inspector** | o'ngda | tanlangan obyekt yoki faylning sozlamalari |
| **Project** | pastda | `Assets` va `Packages` papkalari |
| **Console** | pastda, Project yonida | xabarlar, ogohlantirishlar va xatolar |

Biror oyna yo'qolib qolsa, hammasini **Window → General** menyusidan qayta ochish mumkin. Oynalarni sichqoncha
bilan sudrab, istalgancha joylashtiring.

**Scene oynasida harakatlanish:**

| Amal | Natija |
|---|---|
| o'ng tugmani bosib turib sichqonchani surish | atrofga qarash |
| o'ng tugma bosilgan holda W A S D | "uchib" yurish |
| g'ildirak | yaqinlashish / uzoqlashish |
| o'rta tugmani bosib surish | surish (pan) |
| Alt + chap tugma | tanlangan nuqta atrofida aylanish |
| Hierarchy'da obyektni tanlab, Scene ustida **F** | obyektga "yaqinlashib" olish |

### 3.2. Play rejimi

Editor tepasidagi uchta tugma:

| Tugma | Tezkor tugma | Nima qiladi |
|---|---|---|
| ▶ Play | Ctrl+P | sahnani ishga tushiradi |
| ❚❚ Pause | Ctrl+Shift+P | to'xtatib turadi |
| ▶❚ Step | Ctrl+Alt+P | pauzada bitta kadr oldinga |

**Eng muhim qoida: Play rejimida qilingan o'zgarishlar Play'dan chiqqanda yo'qoladi.** Play paytida Inspector'da
biror sonni o'zgartirib, natijani ko'rish juda qulay. Lekin Play'dan chiqqach, qiymat eski holiga qaytadi. Yangi
boshlaganlar ko'p marta shunga tushadi: yarim soat sahnani bezab, keyin hammasi yo'qolganini ko'radi.

Bundan saqlanishning oson yo'li: **Edit → Preferences... → Colors → Playmode tint** rangini sezilarli qilib qo'ying
(masalan, och qizil). Shunda Play rejimida Editor butunlay shu rangga bo'yaladi va qaysi rejimda ekaningiz doim
ko'rinib turadi.

### 3.3. Render pipeline: URP haqida qisqacha

Sahna bu obyektlar ro'yxati: qayerda nima turibdi, qanday rangda. Ekran esa piksellar. Birinchisidan ikkinchisini
yasaydigan bosqichlar zanjiri **render pipeline** deyiladi: qaysi obyekt ko'rinadi, qanday tartibda chiziladi,
yorug'lik qanday hisoblanadi, oxirida qanday effektlar qo'shiladi.

Unity'da bir nechta pipeline bor. Bizniki **URP** (Universal Render Pipeline): tez, zamonaviy va har xil
qurilmada ishlaydi. Uning sozlamalari `Assets/Settings` da:

- `PC_RPAsset` va `PC_Renderer`: kompyuter uchun sozlamalar (biz shuni ishlatamiz);
- `Mobile_RPAsset` va `Mobile_Renderer`: telefon uchun;
- `DefaultVolumeProfile`, `SampleSceneProfile`: effektlar (Bloom va boshqalar, 02-darsda).

Hozircha bularga tegmaymiz. 02-darsda yorug'lik "porlashi" (Bloom) uchun va 11-darsda o'z shader'larimizni yozganda
URP bilan yaqinroq tanishamiz.

### 3.4. Ishga tushirib ko'ring

1. Project oynasida `Assets/Scenes/SampleScene` ni ikki marta bosing (ochiq bo'lmasa).
2. **Game** oynasining yuqori qismidagi "Free Aspect" ro'yxatidan **16:9 Aspect** ni tanlang. Shunda rasm
   proporsiyasi monitorniki bilan bir xil bo'ladi.
3. **Play** ni bosing. Game oynasida osmon (ko'k gradient) va gorizont ko'rinadi. Editor Playmode tint rangiga
   bo'yaladi.
4. Game oynasi yuqorisidagi **Stats** tugmasini bosing: soniyadagi kadrlar soni (FPS) va boshqa raqamlar chiqadi.
5. Yana **Play** ni bosib, rejimdan chiqing.

## 4-qadam. Rider'ni ulash

### 4.1. Nega Unity'ning o'zida emas, Rider'da

Unity'da kod muharriri yo'q. Skript oddiy `.cs` matn fayli, uni istalgan dasturda yozish mumkin. Lekin yaxshi
muharrir juda ko'p narsada yordam beradi: nomni yozishni boshlasangiz, davomini taklif qiladi; xatoni Unity'ga
qaytmasdan qizil chiziq bilan ko'rsatadi; funksiya qayerda ishlatilganini topadi.

**Rider** Unity'ni ayniqsa yaxshi biladi: `Update` kabi Unity chaqiradigan funksiyalarni alohida belgi bilan
ko'rsatadi, Inspector'da qanday qiymat turganini kod yonida yozadi, Play'ni o'zidan boshqarishga va kodni
qadamma-qadam bajarib kuzatishga (debug) imkon beradi.

Ikkalasini `com.unity.ide.rider` paketi bog'laydi. Unity Rider'ga qaysi fayllar borligini aytadi (`.csproj` va
`.slnx` orqali), Rider esa Unity bilan doimiy aloqada turadi.

### 4.2. Amal

1. **Edit → Preferences... → External Tools**.
2. **External Script Editor** ro'yxatidan Rider'ni tanlang (kompyuteringizda `Rider 2026.3...` deb turadi).
3. Pastdagi **Regenerate project files** tugmasini bir marta bosing. Unity `.csproj` va `.slnx` fayllarini
   qaytadan yozadi.
4. Preferences oynasini yoping.
5. **Assets → Open C# Project**. Rider ochiladi va `AtomSimulation_Scratch.slnx` ni yuklaydi.

Rider birinchi marta loyihani "indekslaydi" (barcha kutubxonalarni o'qib chiqadi), bu bir-ikki daqiqa olishi mumkin.

### 4.3. Ishga tushirib ko'ring

- Rider'ning pastki o'ng burchagida (status bar) Unity belgisi bor. Uning ustiga sichqonchani olib borsangiz,
  Unity Editor bilan ulangani yoziladi.
- Rider'da chapdagi oynada (Explorer) **Unity** ko'rinishini tanlasangiz, `Assets`, `Packages` va boshqa
  papkalar Unity'dagidek ko'rinadi.
- Unity'da Project oynasida `Assets/TutorialInfo/Readme.cs` ni ikki marta bosing: fayl Unity'da emas, Rider'da
  ochilishi kerak. Ochilgan faylni o'zgartirmasdan yoping.

Shu paytdan boshlab skriptlarni Rider'da yozasiz, Unity esa saqlangan faylni o'zi sezadi va kompilyatsiya qiladi
(Unity oynasiga qaytganingizda pastki o'ng burchakda aylanayotgan belgi ko'rinadi).

## 5-qadam. O'z sahnamiz

### 5.1. Nega alohida papka

Bizning hamma narsamiz `Assets/AtomSim` ichida turadi. Sabab oddiy: shablondan kelgan, Unity paketlari qo'shgan va
bizniki aralashib ketmasligi kerak. Yonidagi tayyor `AtomSimulation` loyihasi ham shunday tuzilgan, kurs oxirida
`AtomSim` ichida shular bo'ladi:

```
Assets/AtomSim/
├── Scenes/        sahna
├── Materials/     materiallar (01-dars)
├── Shaders/       shader'lar (02-dars)
├── Runtime/       Play paytida ishlaydigan C# kod (01-dars)
├── UI/            boshqaruv paneli (14-dars)
└── Tests/         testlar (03-dars)
```

Hozircha faqat `Scenes` kerak. Qolganlarini kerak bo'lgan darsda yaratamiz.

### 5.2. Sahna nima

**Sahna** (scene) bu bitta "dunyo": unda qaysi obyektlar borligi, ular qayerda turgani va qanday sozlangani.
O'yinlarda har bir bosqich odatda alohida sahna bo'ladi. Bizga bitta sahna yetadi.

Sahna `.unity` kengaytmali matn fayli (YAML formatida). Uni Rider'da ochib ko'rishingiz mumkin, lekin qo'lda
tahrirlamaymiz: buni Editor qiladi.

### 5.3. Amal: papka va sahna

1. Project oynasida `Assets` ustida sichqonchaning o'ng tugmasi → **Create → Folder**, nomi `AtomSim`.
2. `AtomSim` ichida xuddi shunday **Create → Folder**, nomi `Scenes`.
3. **File → New Scene**. Ochilgan oynada **Basic (URP)** ni tanlab, **Create** ni bosing. Bu shablonda ikkita
   obyekt bor: `Main Camera` (kamera) va `Directional Light` (quyosh kabi yorug'lik).
4. **File → Save As...**, papka `Assets/AtomSim/Scenes`, nomi `AtomSimulation`. Saqlang.

Hierarchy'ning eng tepasida sahna nomi `AtomSimulation` bo'lib qoladi. Saqlanmagan o'zgarish bo'lsa, nomi yonida
`*` belgisi chiqadi. **Ctrl+S** sahnani saqlaydi, uni tez-tez bosing.

### 5.4. Amal: kosmos foni

Basic (URP) sahnasining kamerasi fonga osmon (*skybox*) chizadi. Atom esa qorong'i fonda yaxshi ko'rinadi.

1. Hierarchy'da `Main Camera` ni tanlang.
2. Inspector'da **Camera** komponentini toping, undagi **Environment** bo'limini oching.
3. **Background Type** ni **Solid Color** ga o'zgartiring.
4. Pastdagi **Background** rangini bosing va ochilgan oynada **Hexadecimal** maydoniga `020204` yozing.

**Rang qanday yoziladi.** Ekrandagi har bir piksel uchta yorug'lik qo'shilmasidan iborat: qizil (R), yashil (G) va
ko'k (B). Har biri 0 dan (o'chiq) 255 gacha (to'liq yonib turibdi). `020204` uchta juftlik: `02` qizil, `02` yashil,
`04` ko'k. Bu 16 lik sanoq tizimidagi sonlar (*hexadecimal*), 0–9 dan keyin A–F harflari 10–15 ni bildiradi.
`02 = 2`, `04 = 4`, `FF = 255`. Demak fon rangi `(2, 2, 4)`: deyarli qora, ko'k esa ikki baravar ko'proq, ya'ni
ozgina ko'kish. Yonidagi `AtomSimulation` loyihasi ham shu rangni ishlatadi.

5. **Ctrl+S**.

### 5.5. Amal: sahnani asosiy qilish

Tayyor dastur ishga tushganda qaysi sahna ochilishi **Build Profiles** da yoziladi.

1. **File → Build Profiles**.
2. Chapdagi ro'yxatdan **Scene List** ni tanlang.
3. **Add Open Scenes** tugmasini bosing: `AtomSim/Scenes/AtomSimulation` ro'yxatga qo'shiladi.
4. `Scenes/SampleScene` yonidagi belgini olib tashlang, `AtomSimulation` ni sichqoncha bilan eng tepaga suring.
   Ro'yxatdagi birinchi sahna (0-raqamli) dastur ochilganda birinchi yuklanadi.
5. Oynani yoping.

### 5.6. Ishga tushirib ko'ring

- **Play**. Game oynasi deyarli qora. Osmon yo'q, chunki fonni o'zimiz tanladik. Hozircha sahnada ko'rinadigan
  hech narsa yo'q, keyingi darsda yadro va elektron qo'shamiz.
- Console'da qizil xato yo'q.
- Play'dan chiqing, Unity'ni yoping va qayta oching: `AtomSimulation` sahnasi ochilgan bo'lishi kerak (Unity
  oxirgi ochiq sahnani eslab qoladi).

## 6-qadam. Git va GitHub

### 6.1. Nega Git

Git loyihaning tarixini saqlaydi: har bir commit butun loyihaning bir lahzadagi surati. Biror narsa buzilsa, oldingi
holatga qaytasiz. GitHub esa shu tarixning internetdagi nusxasi: kompyuter buzilsa ham, loyiha yo'qolmaydi, Mac'da
ishlamoqchi bo'lsangiz, u yerdan tortib olasiz.

```
fayllar ──(git add)──► staging ──(git commit)──► mahalliy tarix ──(git push)──► GitHub
```

OpenGL kursidagi qoida bu yerda ham o'sha: Git'ga **odam yaratgan narsa** kiradi. Qayta yaratish mumkin bo'lgan narsa
va shaxsiy sozlamalar kirmaydi. 2-qadamda ko'rdik: Unity loyihasida qayta yaratiladigan qism juda katta.

### 6.2. `.gitignore`

Loyiha ildizida (`C:\dev\UNITY\AtomSimulation_Scratch`) `.gitignore` faylini yarating. Fayl nomi nuqta bilan
boshlanadi va kengaytmasi yo'q. Uni Rider'da yaratish qulay: Explorer oynasida **File System** ko'rinishini
tanlang, loyiha ildizi ustida o'ng tugma → **Add → File**.

```gitignore
# Unity qayta yaratadigan papkalar
/[Ll]ibrary/
/[Tt]emp/
/[Oo]bj/
/[Bb]uild/
/[Bb]uilds/
/[Ll]ogs/
/[Uu]ser[Ss]ettings/
/[Mm]emoryCaptures/
/[Rr]ecordings/

# Muharrirlar fayllari (Unity ularni o'zi yaratadi)
.vs/
.vscode/
.idea/
*.csproj
*.sln
*.slnx
*.user
*.suo
*.pidb

# Tizim fayllari
.DS_Store
Thumbs.db
```

Qismma-qism:

**`/[Ll]ibrary/`**: boshidagi `/` "faqat loyiha ildizidagi `Library`" degani. `Assets` ichida kimdir `Library`
degan papka yaratsa, u Git'dan tushib qolmasligi kerak. `[Ll]` "katta yoki kichik L" degani: Windows'da harf
katta-kichikligi farq qilmaydi, boshqa tizimlarda farq qiladi, shuning uchun ikkalasini ham yozamiz.

**`Temp`, `Obj`, `Logs`, `UserSettings`, `MemoryCaptures`, `Recordings`**: 2.5-bo'limdagi kabi Unity yaratadigan
papkalar. `Build`/`Builds` 16-darsda tayyor `.exe` tushadigan joy.

**`.vs/`, `.vscode/`, `.idea/`**: Visual Studio, VS Code va Rider'ning shaxsiy sozlamalari.

**`*.csproj`, `*.sln`, `*.slnx`**: 2.6-bo'limda ko'rdik, Unity ularni o'zi yaratadi. `*` "istalgan nom" degani.

**`.DS_Store`, `Thumbs.db`**: macOS va Windows har xil papkalarda yaratadigan keraksiz fayllar.

`.meta` fayllar ro'yxatda yo'q, demak ular Git'ga kiradi. Aynan shunday bo'lishi kerak (2.2-bo'lim).

### 6.3. Repo yaratish va birinchi commit

Rider'ning pastidagi **Terminal** oynasini oching (yoki PowerShell'da loyiha papkasiga o'ting) va:

```
git init -b main
git add .
git status
```

`git init -b main` loyihani Git repo'siga aylantiradi, asosiy branch nomi `main` bo'ladi. `git add .` hamma
o'zgarishni "staging"ga qo'yadi.

`git status` ro'yxatini diqqat bilan o'qing. Unda **bo'lishi kerak**: `.gitignore`, `CLAUDE.md`, `docs/`, `Assets/`
ichidagi fayllar va ularning `.meta`lari, `Packages/manifest.json`, `Packages/packages-lock.json`, `ProjectSettings/`.
Unda **bo'lmasligi kerak**: `Library/`, `Temp/`, `Logs/`, `UserSettings/`, `.vscode/`, `.csproj`, `.slnx`. Agar
`Library` ichidagi fayllar ro'yxatda bo'lsa, `.gitignore` noto'g'ri joyda yoki nomi xato (`.gitignore.txt` bo'lib
qolgan bo'lishi mumkin). To'g'rilab, `git reset` qiling va `git add .` ni qaytadan bajaring.

`git add` paytida `LF will be replaced by CRLF` degan ogohlantirishlar chiqishi mumkin, zarari yo'q. Windows qator
oxirini ikki belgi bilan, Unity esa bir belgi bilan yozadi. Git bu farqni o'zi tekislaydi, Unity esa ikkalasini ham
o'qiy oladi.

Hammasi to'g'ri bo'lsa:

```
git commit -m "Lesson 00: Unity project, own scene, .gitignore"
```

### 6.4. GitHub

1. [github.com/new](https://github.com/new) sahifasida nomi `AtomSimulation_Scratch` bo'lgan repo yarating.
   **Bo'sh** qolsin: README, `.gitignore` va litsenziya qo'shmang, aks holda birinchi push rad etiladi.
2. Terminalda (`<akkaunt>` o'rniga GitHub foydalanuvchi nomingizni yozing):

```
git remote add origin https://github.com/<akkaunt>/AtomSimulation_Scratch.git
git push -u origin main
```

`-u` buyrug'i `main` branch'ini GitHub'dagisi bilan bog'laydi. Keyingi safar faqat `git push` yetadi.

### 6.5. Ishga tushirib ko'ring

- GitHub sahifasida repo ochilganda `Assets`, `Packages`, `ProjectSettings`, `docs`, `.gitignore`, `CLAUDE.md`
  ko'rinadi, `Library` esa yo'q.
- Terminalda `git status` "nothing to commit, working tree clean" deydi.

**Har kungi tartib:** ishni `git pull` bilan boshlang (boshqa kompyuterda ishlagan bo'lsangiz), har bir dars
oxirida `git add .`, `git commit`, `git push`.

## Ixtiyoriy tajribalar

1. `Assets/AtomSim/Scenes/AtomSimulation.unity` ni Rider'da oching (Rider'da faylni oching, Unity'da emas: Unity
   sahnani ochadi). `Main Camera` va `m_BackGroundColor` qatorlarini toping. Siz Inspector'da tanlagan `020204`
   rang bu yerda `0..1` oralig'idagi sonlar bilan yozilgan: `2 / 255 = 0.0078`. Hech narsani o'zgartirmay yoping.
2. Shablonning tanishtiruv sahifasi endi kerak emas. Project oynasida `Assets/TutorialInfo` papkasini va
   `Assets/Readme` faylini tanlab, **Delete** bilan o'chiring. Unity `.meta` fayllarni o'zi o'chiradi. Keyin
   `git status` qiling: o'chirilgan fayllar va ularning `.meta`lari ro'yxatda. Commit qilsangiz, bu holat ham
   tarixda qoladi.

## Lug'at

| So'z | Ma'nosi |
|---|---|
| game engine (o'yin dvigateli) | oyna, grafika, kirish, UI va yig'ishni tayyor beradigan tizim |
| Editor | Unity'ning ish dasturi, unda loyihani yig'asiz |
| Player | tayyor dastur (`.exe`) |
| Unity Hub | Unity versiyalari va loyihalarni boshqaruvchi dastur |
| asset | `Assets` papkasidagi har qanday fayl: sahna, skript, material, rasm |
| `.meta` fayl | har bir asset yonidagi fayl, unda GUID va import sozlamalari |
| GUID | asset'ning takrorlanmas "pasport raqami", havolalar shu bilan bog'lanadi |
| package (paket) | alohida yuklanadigan Unity qismi, ro'yxati `manifest.json` da |
| import | Unity asset'ni o'zi uchun qulay formatga o'girishi, natija `Library` da |
| `Library` | Unity keshi, qayta yaratiladi, Git'ga kirmaydi |
| sahna (scene) | obyektlar to'plami, `.unity` fayli |
| Play rejimi | sahnani Editor ichida ishga tushirish, undagi o'zgarishlar saqlanmaydi |
| render pipeline | sahnani piksellarga aylantiruvchi bosqichlar zanjiri |
| URP | Universal Render Pipeline, bizning render pipeline |
| skybox | kamera fonidagi osmon tasviri |
| hexadecimal | 16 lik sanoq tizimi, ranglar `RRGGBB` ko'rinishida yoziladi |
| Build Profiles | tayyor dasturga qaysi sahnalar kirishi va qaysi platforma uchun yig'ilishi |
| `.gitignore` | Git e'tiborsiz qoldiradigan fayllar ro'yxati |
| commit | loyihaning bir lahzadagi surati |

## Keyingi dars

01-darsda birinchi C# skriptni yozamiz: sahnaga yadro va elektron qo'yamiz va elektronni yadro atrofida doira bo'ylab
aylantiramiz. Shu bilan birga atom nima ekanini, burchak, `sin` va `cos` qanday ishlashini noldan o'rganamiz.

Shu dars tugagach yozing. Xohlasangiz, ishingizni ko'rib chiqaman.
