# 02. Yorug'lik chizamiz: orbitalar, birinchi shader va Bloom

## Bu darsda nima qo'shamiz va nega aynan hozir

01-darsning oxirida qora fonda ikkita shar bor: yadro va uning atrofida aylanayotgan elektron. Ikkita kamchilik
ko'zga tashlanadi. Birinchisi, orbitalar ko'rinmaydi: elektron qayerda aylanishini faqat tasavvur qilamiz. Ikkinchisi,
sharlar plastmassa o'yinchoqqa o'xshaydi: ular `Directional Light` yoritgani uchun ko'rinadi, o'zlari nur
sochmaydi.

Bu darsda atomni tayyor loyihadagidek ko'rinishga keltiramiz:

- oltita orbitani ingichka chiziq qilib chizamiz, elektron turgan orbita yorqinroq bo'ladi;
- **birinchi shader**larimizni o'zimiz yozamiz: chiziqlar uchun va porlash uchun;
- yadro va elektronni sharlardan o'zi porlab turgan nuqtalarga aylantiramiz;
- URP'ning **Bloom** effekti kuchli nurni atrofga yoyadi, xuddi tunda chiroq atrofida ko'rinadigan gardishdek.

Nega hozir: keyingi darslarda fotonlar (04), to'lqinlar (05) va millionlab nuqtadan iborat elektron buluti (10–13)
paydo bo'ladi. Ularning hammasi shu darsdagi uch narsaga tayanadi: shader, ranglarni qo'shish va HDR yorqinlik.

Yangi tushunchalar:

- **Unity**: koddan GameObject yaratish, `LineRenderer`, massiv (array), `for` sikli, shader fayli (ShaderLab va
  HLSL), Mesh va UV, Volume va post-processing.
- **Matematika**: doirani to'g'ri kesmalar bilan chizish va uning xatosi, rang sonlar sifatida, ranglarni qo'shish,
  UV koordinatalar, markazdan masofa (Pifagor yana keladi), daraja (`pow`), yumshoq o'tish (`smoothstep`).
- **Fizika**: yorug'likning yorqinligi, monitor qancha yorug'likni ko'rsata oladi, nega yorug' chiroq atrofida
  nur gardishi ko'rinadi.

Dars oxirida loyiha shunday bo'ladi:

```
Assets/AtomSim/
├── Scenes/AtomSimulation.unity      (o'zgaradi: Quad'lar, Global Volume, kamera)
├── Materials/
│   ├── OrbitLine.mat                (yangi)
│   ├── Nucleus.mat                  (o'zgaradi: shader Glow)
│   └── Electron.mat                 (o'zgaradi: shader Glow)
├── Shaders/                         (yangi)
│   ├── AdditiveLine.shader
│   └── Glow.shader
├── Settings/                        (yangi)
│   └── AtomSimVolumeProfile.asset
└── Runtime/Bohr/ElectronOrbit.cs    (o'zgaradi)
```

## Dars qanday tuzilgan

| Qadam | Nima qilamiz | Natija |
|---|---|---|
| 1 | Orbitalarni koddan yaratamiz: `LineRenderer` va `for` | oltita oq doira |
| 2 | Birinchi shader: chiziqlar rangini koddan beramiz | joriy orbita yorqin, qolganlari xira |
| 3 | Sharlar o'rniga porlayotgan nuqtalar: Glow shader va Quad | yumshoq porlayotgan yadro va elektron |
| 4 | HDR va Bloom | nur atrofga yoyiladi |

Har qadamdan keyin **Play** ni bosib tekshirasiz. Klaviatura qisqartmalari Windows uchun yozilgan, Mac'dagisi
qavs ichida: **Ctrl+S (Mac: Cmd+S)**.

**Boshlanish nuqtasi**: 01-darsning oxiri. Kamera `(0, 0, -55)` da, `Atom` ichida `Nucleus` va `Electron`
sharlari, `ElectronOrbit` skripti strelkalar bilan `n` ni 1 dan 6 gacha o'zgartiradi.

**Kod uslubi.** Bu darsdan boshlab kodni siz so'ragan uslubda yozamiz: ochuvchi qavs `{` qatorning oxirida turadi
(Java/K&R uslubi). Loyihadagi `.editorconfig` Rider'ga shu uslubni aytadi. Rider'da **Code → Reformat Code** butun
faylni shu uslubga keltiradi.

**01-darsdagi faylingiz haqida.** Ishingizni ko'rib chiqdim, hammasi to'g'ri ishlaydi. Ikkita mayda narsa bor:
faylning boshida `using System.Reflection.Metadata;` qatori turibdi (Rider uni o'zi qo'shib qo'ygan, kodda
ishlatilmaydi), `Start` ichida esa ikkita `Debug.Log` bor. 1-qadamda faylni to'liq almashtirasiz, ikkalasi ham
o'z-o'zidan ketadi.

## 1-qadam. Orbitalarni chizamiz

### 1.1. Kompyuter doirani qanday chizadi

Grafik karta egri chiziqni "egri" holida chiza olmaydi. U faqat uchta narsani biladi: nuqta, to'g'ri kesma va
uchburchak. Shuning uchun doira ko'p burchakli shakl bilan almashtiriladi.

Tangani qog'ozga qo'yib, chetini qalam bilan bir necha joyda nuqtalab chiqing, keyin qo'shni nuqtalarni chizg'ich
bilan tutashtiring. Oltita nuqta qo'ysangiz, olti burchakli shakl (asalari uyasidek) chiqadi. Yuzta nuqta qo'ysangiz,
ko'z uni doiradan ajrata olmaydi.

Nuqtalarni qayerga qo'yishni 01-darsdan bilamiz (3.2-bo'lim): burchakni `cos` va `sin` doira ustidagi nuqtaga
aylantiradi, radiusga ko'paytirsak, kerakli kattalikdagi doira chiqadi. Bitta farq bor: endi bitta nuqta emas,
`N` ta nuqta kerak. Ular doira bo'ylab teng taqsimlanadi: to'liq aylana `2 × π = 6.2832` radian, demak qo'shni
nuqtalar orasidagi burchak:

```
qadam = 2 × π / N
i-nuqtaning burchagi = i × qadam        (i = 0, 1, 2, ..., N - 1)
i-nuqta = (cos(burchak) × radius, sin(burchak) × radius, 0)
```

`N = 6`, radius 4 uchun qo'lda hisoblaymiz. Qadam `6.2832 / 6 = 1.0472` radian, ya'ni 60°:

| `i` | Burchak | `cos` | `sin` | Nuqta |
|---|---|---|---|---|
| 0 | 0° | 1 | 0 | (4, 0) |
| 1 | 60° | 0.5 | 0.866 | (2, 3.464) |
| 2 | 120° | −0.5 | 0.866 | (−2, 3.464) |
| 3 | 180° | −1 | 0 | (−4, 0) |
| 4 | 240° | −0.5 | −0.866 | (−2, −3.464) |
| 5 | 300° | 0.5 | −0.866 | (2, −3.464) |

`i = 6` bo'lmaydi: 360° nuqtasi 0° nuqtasining o'zi. Oxirgi nuqtani birinchisiga tutashtirish kerak, buni
`LineRenderer`ning **Loop** sozlamasi qiladi (1.5-bo'lim).

### 1.2. Nechta nuqta yetadi

Ikkita qo'shni nuqta orasidagi to'g'ri kesma doira yoyidan biroz ichkarida o'tadi. Eng katta farq kesmaning
o'rtasida bo'ladi. Uni hisoblaymiz.

```
                 ●  ← doiradagi nuqta
              /  |
          r /    |
         /       |
       /  yarim  ● ← kesmaning o'rtasi
     ●───────────┤
  markaz         |   farq: doira shu yerda kesmadan biroz tashqarida
                 |
                 ●  ← keyingi nuqta
```

Markazdan kesmaning o'rtasiga chiziq tortsak, u qo'shni nuqtalar orasidagi burchakni teng ikkiga bo'ladi. Yarim
burchak `π / N`. To'g'ri burchakli uchburchak hosil bo'ladi: gipotenuzasi `r` (markazdan nuqtagacha), kesmaning
o'rtasigacha masofa esa "qancha oldinga", ya'ni `r × cos(π / N)` (01-dars, 3.2 va 5.4-bo'limlar). Doira esa markazdan
aniq `r` uzoqlikda. Demak:

```
farq = r − r × cos(π / N)
```

Eng katta orbita (`r = 24`) uchun. 01-darsning 5.4-bo'limidan: kamera 55 da turganda ekranga 63.5 birlik
balandlik sig'adi. Ekran balandligi 1080 piksel bo'lsa, 1 birlik `1080 / 63.5 = 17` piksel:

| `N` | Yarim burchak | Farq (birlik) | Farq (piksel) |
|---|---|---|---|
| 6 | 30° | `24 × (1 − 0.866) = 3.2` | 55 |
| 12 | 15° | 0.82 | 14 |
| 32 | 5.6° | 0.12 | 2 |
| 160 | 1.1° | 0.0046 | 0.08 |

32 nuqtada xato 2 piksel, diqqat bilan qarasangiz burchaklar seziladi. 160 nuqtada xato pikselning o'ndan biridan
ham kichik, ko'z uni hech qachon sezmaydi. Biz 160 ni olamiz. Bu kompyuter uchun arzon: 6 orbita × 160 = 960 nuqta.

### 1.3. `for`: bir ishni ko'p marta takrorlash

Bizga 6 ta orbita va har birida 160 ta nuqta kerak. Bir xil qatorni 960 marta yozish o'rniga kompyuterga "takrorla"
deymiz. Buning uchun **`for` sikli** (loop) bor:

```csharp
for (int level = 1; level <= MaxLevel; level++) {
    CreateOrbit(level);
}
```

Qavs ichida nuqtali vergul bilan ajratilgan uchta qism bor:

| Qism | Ma'nosi |
|---|---|
| `int level = 1` | **boshlanish**: hisoblagich `level`, birinchi qiymati 1 |
| `level <= MaxLevel` | **shart**: shart to'g'ri bo'lsa, `{ }` ichidagi ish yana bajariladi |
| `level++` | **qadam**: har aylanishdan keyin `level` ga 1 qo'shiladi (`level += 1` ning qisqasi) |

Zinadan chiqishga o'xshaydi: "1-pog'onadan boshla; 6-pog'onadan oshmaguningcha chiq; har safar bir pog'ona
ko'taril". Kompyuter shunday bajaradi:

| `level` | Shart `level <= 6` | Nima bo'ladi |
|---|---|---|
| 1 | to'g'ri | `CreateOrbit(1)` |
| 2 | to'g'ri | `CreateOrbit(2)` |
| ... | ... | ... |
| 6 | to'g'ri | `CreateOrbit(6)` |
| 7 | **noto'g'ri** | sikl tugaydi, `for` dan keyingi qatorga o'tiladi |

Nuqtalar uchun ikkinchi sikl 0 dan boshlanadi: `for (int i = 0; i < OrbitPoints; i++)`. U `i = 0, 1, ..., 159`
uchun ishlaydi, jami 160 marta. 0 dan boshlash qulay: birinchi nuqtaning burchagi `0 × qadam = 0`. Shartda `<`
turibdi, `<=` emas. `<=` bo'lsa, `i = 160` ham bajarilardi va 161-nuqta paydo bo'lardi, unga joy yo'q (dars oxiridagi
jadvalda bu xato o'lchangan).

### 1.4. GameObject'ni koddan yaratish

01-darsda obyektlarni Hierarchy'da qo'lda yaratdik. Oltita orbitani qo'lda yaratib, har biriga 160 ta nuqta kiritish
og'ir ish. Kod buni bir zumda qiladi. Editor'dagi har bir amalning kodda o'z juftligi bor:

| Editor'da | Kodda |
|---|---|
| Hierarchy → **Create Empty**, nomini yozish | `new GameObject("Orbit n=1")` |
| obyektni `Atom` ustiga sudrab, uning bolasi qilish | `orbit.transform.SetParent(transform, false)` |
| Inspector → **Add Component** → Line Renderer | `orbit.AddComponent<LineRenderer>()` |

- **`new GameObject(...)`** yangi, bo'sh obyekt yaratadi. `new` so'zi "yangisini yarat" degani.
- **`SetParent(transform, false)`**: `transform` bu skript turgan obyektning, ya'ni `Atom`ning Transform'i. Ikkinchi
  qiymat `false` "bolaning joyini otaga nisbatan o'qi" degani (01-dars, 1.6: local koordinata). Yangi obyektning
  joyi `(0, 0, 0)`, demak u atom markazida turadi.
- **`AddComponent<LineRenderer>()`**: burchak qavslar `< >` ichida qaysi komponent kerakligi yoziladi. Funksiya
  qo'shilgan komponentni qaytaradi, biz uni `line` o'zgaruvchisiga olib, sozlaymiz.

Bu obyektlar `Start` ichida, ya'ni Play bosilganda yaratiladi va Play tugashi bilan yo'qoladi: Play paytidagi
o'zgarishlar saqlanmaydi (00-dars). Shuning uchun sahna fayliga oltita ortiqcha obyekt yozilmaydi, har safar kod
ularni qaytadan quradi.

`$"Orbit n={level}"` 01-darsdagi `Debug.Log` qatoridagi kabi: `{level}` o'rniga son qo'yiladi, `Orbit n=1`,
`Orbit n=2` va hokazo.

### 1.5. LineRenderer

**LineRenderer** nuqtalar ro'yxati orqali chiziq chizadigan komponent. U nuqtalarni ketma-ket tutashtirib, eni
`widthMultiplier` bo'lgan lenta yasaydi va uni doim kameraga qaratib turadi. Bizga kerakli sozlamalar:

| Kodda | Inspector'da | Nima qiladi |
|---|---|---|
| `positionCount = 160` | Positions → Size | nechta nuqta |
| `SetPosition(i, nuqta)` | Positions → Element i | `i`-nuqtaning joyi |
| `loop = true` | Loop | oxirgi nuqtani birinchisiga tutashtiradi, doira yopiladi |
| `useWorldSpace = false` | Use World Space o'chiq | nuqtalar otaga nisbatan (local): `Atom`ni sursangiz, orbitalar ham suriladi |
| `widthMultiplier = 0.1f` | Width | lentaning eni, birlikda: 1080 pikselda `0.1 × 17 = 1.7` piksel |
| `sharedMaterial = ...` | Materials | qaysi material bilan chiziladi |

**Material** majburiy. Materialsiz chiziq pushti-binafsha rangda chiqadi. Unity bu rangni "material yo'q yoki
shader ishlamayapti" degan ogohlantirish sifatida ishlatadi. Bu rangni yaxshi eslab qoling, shader yozganda ko'p
uchraydi. Hozircha chiziq uchun URP'ning tayyor **Unlit** shader'i bilan material yasaymiz. "Unlit" "yoritilmagan"
degani: yorug'likka qaramaydi, hamma joyi bir xil rangda.

Materialni skriptga Inspector orqali beramiz, xuddi 01-darsdagi `m_Electron` kabi: `[SerializeField] Material
m_LineMaterial;`.

### 1.6. Kod

Bu qadamda faylda o'zgarish ko'p, shuning uchun `ElectronOrbit.cs` ni **to'liq** almashtiring. Rider'da faylni
oching, hammasini belgilang (**Ctrl+A**, Mac: **Cmd+A**) va quyidagi bilan almashtiring:

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

namespace AtomSim.Bohr {
    /// <summary>
    /// Elektronni yadro atrofida doira bo'ylab aylantiradi (01-dars) va orbitalarni chizadi (02-dars).
    /// Yuqori va pastki strelka orbita raqamini (n) o'zgartiradi.
    /// </summary>
    public sealed class ElectronOrbit : MonoBehaviour {
        const int MaxLevel = 6;
        const int OrbitPoints = 160;

        [SerializeField] Transform m_Electron;
        [SerializeField] Material m_LineMaterial;
        [SerializeField] float m_LevelSpacing = 4f;
        [SerializeField] float m_AngularSpeed = 2.4f;

        int m_Level = 1;
        float m_Angle;

        void Start() {
            for (int level = 1; level <= MaxLevel; level++) {
                CreateOrbit(level);
            }
        }

        void Update() {
            HandleKeyboard();

            m_Angle += m_AngularSpeed * Time.deltaTime;
            float radius = m_Level * m_LevelSpacing;
            m_Electron.localPosition = new Vector3(Mathf.Cos(m_Angle), Mathf.Sin(m_Angle), 0f) * radius;
        }

        void HandleKeyboard() {
            Keyboard keyboard = Keyboard.current;
            if (keyboard == null) return;

            if (keyboard.upArrowKey.wasPressedThisFrame) SetLevel(m_Level + 1);
            if (keyboard.downArrowKey.wasPressedThisFrame) SetLevel(m_Level - 1);
        }

        void SetLevel(int level) {
            m_Level = Mathf.Clamp(level, 1, MaxLevel);
            Debug.Log($"Orbita: n = {m_Level}, radius = {m_Level * m_LevelSpacing}");
        }

        void CreateOrbit(int level) {
            GameObject orbit = new GameObject($"Orbit n={level}");
            orbit.transform.SetParent(transform, false);

            LineRenderer line = orbit.AddComponent<LineRenderer>();
            line.sharedMaterial = m_LineMaterial;
            line.useWorldSpace = false;
            line.loop = true;
            line.widthMultiplier = 0.1f;

            float radius = level * m_LevelSpacing;
            float step = 2f * Mathf.PI / OrbitPoints;
            line.positionCount = OrbitPoints;
            for (int i = 0; i < OrbitPoints; i++) {
                float angle = i * step;
                line.SetPosition(i, new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius);
            }
        }
    }
}
```

01-darsdagi fayldan farqlari:

- **`const int OrbitPoints = 160`**: har bir orbitadagi nuqtalar soni (1.2-bo'lim).
- **`m_LineMaterial`**: chiziq materiali, Inspector'dan beriladi.
- **`Start`** endi xabar chiqarmaydi, oltita orbitani yaratadi.
- **`CreateOrbit(int level)`** yangi funksiya: bitta orbitani yaratadi. `level` uning raqami `n`, radiusi 01-darsdagi
  kabi `level × m_LevelSpacing`.
- **`Mathf.PI`** bu π soni (3.1416). `2f * Mathf.PI / OrbitPoints` 1.1-bo'limdagi qadam: `6.2832 / 160 = 0.0393`
  radian.

### 1.7. Amal: chiziq materiali

1. Project oynasida `Assets/AtomSim/Materials` ichida o'ng tugma → **Create → Material**. Nomi `OrbitLine`.
2. Inspector'ning eng tepasidagi **Shader** ro'yxatini bosing va **Universal Render Pipeline → Unlit** ni tanlang.
   Rangi oq qoladi.
3. Hierarchy'da `Atom` ni tanlang. Electron Orbit komponentida yangi **Line Material** maydoni paydo bo'ldi.
   `OrbitLine` materialini Project oynasidan sudrab, shu maydonga tashlang.
4. **Ctrl+S** (Mac: **Cmd+S**).

### 1.8. Ishga tushirib ko'ring

- **Play**. Atom atrofida oltita ingichka oq doira paydo bo'ladi. Radiuslari 4, 8, 12, 16, 20, 24. Elektron
  birinchisi ustida aylanadi. Strelkalar bilan `n` ni o'zgartirsangiz, elektron boshqa doiraga sakraydi.
- Play'dan chiqmasdan Hierarchy'ga qarang: `Atom` ichida `Nucleus` va `Electron` dan keyin `Orbit n=1` dan
  `Orbit n=6` gacha oltita yangi obyekt bor. `Orbit n=1` ni tanlang: Inspector'da Line Renderer, **Positions**
  ichida 160 ta element. **Element 0** `(4, 0, 0)`, **Element 40** `(0, 4, 0)`: 40-nuqtaning burchagi
  `40 × 0.0393 = 1.5708` radian, ya'ni 90°. **Loop** belgilangan.
- Element 40 ning `X` maydonida 0 o'rniga `-1.748456e-07` kabi g'alati son ko'rinishi mumkin. Bu `−0.0000001748`,
  ya'ni amalda 0. `e-07` "vergulni 7 xona chapga sur" degani. Kompyuter π ni cheksiz aniqlikda saqlay olmaydi,
  shuning uchun `cos(90°)` aniq 0 emas, juda kichik son chiqadi. Men o'lchaganimda shu qiymat chiqdi.
- Play'dan chiqing: oltita orbita Hierarchy'dan yo'qoladi.

Shu holatni Unity'da tekshirdim: kod 0 xato va 0 ogohlantirish bilan kompilyatsiya bo'ldi, Play'da oltita obyekt
yuqoridagi qiymatlar bilan yaratildi. Line Material maydoni bo'sh qolganda chiziqlar pushti-binafsha bo'ldi.

## 2-qadam. Birinchi shader: chiziqlarni bo'yaymiz

Hozir hamma orbita bir xil oq. Elektron turgan orbita yorqin, qolganlari xira bo'lishini xohlaymiz. Rangni koddan
`LineRenderer`ga beramiz, lekin URP'ning Unlit shader'i bunday rangni o'qimaydi. Shuning uchun o'z shader'imizni
yozamiz.

### 2.1. GPU va shader

Kompyuterda ikki xil "ishchi" bor:

- **CPU** (protsessor): bir nechta juda aqlli ishchi. Bizning C# kodimiz shu yerda ishlaydi.
- **GPU** (grafik karta): minglab oddiy ishchi. Ularning har biri bitta kichik ishni qiladi, lekin hammasi bir
  vaqtda. Masalan, ekrandagi 2 million pikselning har biri uchun "qaysi rang?" degan savolga bir paytda javob
  topadi.

**Shader** bu GPU'da ishlaydigan kichik dastur. 01-darsda ko'rdik: har bir material'da shader bor (1.7-bo'lim). Shu
paytgacha URP'ning tayyor shader'larini (Lit, Unlit) ishlatdik, endi o'zimiznikini yozamiz. Shader **HLSL** tilida
yoziladi. U C# ga o'xshaydi: o'sha `float`, `return`, `{ }` va `;`.

### 2.2. Uchburchakdan piksellargacha

GPU har bir obyektni bir xil yo'l bilan chizadi:

```
 1. Mesh                2. Vertex shader          3. Rasterlash             4. Fragment shader     5. Aralashtirish
 uchlar va              har bir uch uchun:        uchburchak qaysi          har bir piksel         yangi rang ekrandagi
 uchburchaklar   ───►   "ekranning qayerida?" ──► piksellarni qoplaydi? ──► uchun: "qaysi   ───►  rang bilan qanday
                                                                           rang?"                 birlashadi
```

1. **Mesh**: obyektning shakli, uchlar (vertex, ko'pligi vertices) va ulardan tuzilgan uchburchaklar. LineRenderer
   bizning 160 ta nuqtamizdan o'zi lenta yasaydi: har bir bo'lagi ikkita uchburchak.
2. **Vertex shader** har bir uch uchun bir marta ishlaydi. Uning asosiy vazifasi: uchning ekrandagi joyini topish.
   Buning ichida 01-darsning 5.4-bo'limidagi hisob ham bor: kamera qayerda, FOV qancha, nima ekranga sig'adi.
3. **Rasterlash**: GPU har bir uchburchak ekranning qaysi piksellarini qoplashini o'zi topadi. Bu qismni biz
   yozmaymiz.
4. **Fragment shader** har bir piksel uchun bir marta ishlaydi va uning rangini qaytaradi. "Fragment" piksel
   uchun "nomzod" degani: bir piksel ustiga bir nechta obyekt tushishi mumkin.
5. **Aralashtirish** (blending): yangi rang ekranda oldindan turgan rang bilan qanday birlashishi. Buni ham GPU
   bajaradi, biz faqat qoidasini aytamiz (2.4-bo'lim).

Biz 2- va 4-bosqichlarni yozamiz: ular ikkita funksiya bo'ladi, `Vert` va `Frag`.

### 2.3. Rang bu uchta son

Ekrandagi har bir piksel aslida uchta mitti chiroq: qizil (R, red), yashil (G, green) va ko'k (B, blue). Har birining
yorqinligi 0 dan 1 gacha son: 0 o'chiq, 1 to'liq yoniq. 01-darsda yozgan `FF7040` ham shunday uchta son: `FF` = 255,
ya'ni to'liq (1), `70` = 112 (0.44), `40` = 64 (0.25).

| R | G | B | Ko'rinishi |
|---|---|---|---|
| 0 | 0 | 0 | qora (hammasi o'chiq) |
| 1 | 0 | 0 | qizil |
| 0 | 1 | 0 | yashil |
| 1 | 1 | 0 | sariq (qizil + yashil) |
| 1 | 1 | 1 | oq |
| 0.5 | 0.5 | 0.5 | kulrang |

Unity'da rangning to'rtinchi soni ham bor: **alpha** (A). Odatda u shaffoflikni bildiradi. Biz o'z shader'imizda uni
"yorqinlik ko'paytuvchisi" sifatida ishlatamiz: alpha 0.35 bo'lsa, chiziq 35% yorqinlikda.

Bir nozik joy bor. Unity rangni shader'ga berishdan oldin biroz o'zgartiradi: 0 va 1 o'zgarmaydi, o'rtadagi qiymatlar
kichrayadi (masalan, 0.75 dan 0.52 bo'ladi). Buning sababi ko'zimiz yorqinlikni qanday sezishida (**gamma**), uni
11-darsda batafsil ko'ramiz. Hozircha shuni bilish yetadi: Inspector'da ko'rgan rangingiz ekranda o'sha rang bo'lib
chiqadi.

### 2.4. Yorug'likni qo'shish: `Blend One One`

Devorga ikkita fonar tutsangiz, ular ustma-ust tushgan joy yorqinroq bo'ladi: yorug'lik **qo'shiladi**. Qizil va
yashil fonar ustma-ust tushsa, sariq dog' chiqadi.

Shader'ga ham shunday qilishni aytamiz:

```
Blend One One       →   yangi piksel = shader rangi × 1 + ekrandagi rang × 1
```

Bu **qo'shiluvchi aralashtirish** (additive blending). Sonlar bilan, fon `(0.008, 0.008, 0.016)` va xira orbita
`(0.06, 0.075, 0.12)`:

```
(0.008 + 0.06,  0.008 + 0.075,  0.016 + 0.12) = (0.068, 0.083, 0.136)
```

Uch xil qulayligi bor:

- **Qora hech narsa qo'shmaydi.** 0 qo'shilsa, son o'zgarmaydi. Demak shaklning qora qismlari ko'rinmaydi va bizga
  alohida shaffoflik kerak emas. 3-qadamda bundan foydalanamiz.
- **Ustma-ust tushgan nur yorqinroq bo'ladi**, xuddi haqiqiy yorug'likdagidek.
- **Chizish tartibi muhim emas**: `a + b = b + a`. Qaysi chiziq oldin chizilganidan qat'i nazar, natija bir xil.

Oddiy (qo'shilmaydigan) materialda yangi rang eskisining **o'rnini egallaydi**: yangi piksel = shader rangi.

Shu bilan birga yana ikkita sozlama keladi:

- **`ZWrite Off`**: GPU'da "kim kameraga yaqinroq" degan daftar bor (**depth buffer**, chuqurlik buferi). Odatda har
  bir obyekt unga yozadi, shunda orqadagi narsa oldingisini to'smaydi. Nur hech narsani to'smaydi, shuning uchun bu
  daftarga yozmaymiz.
- **`Cull Off`**: uchburchakning ikkala tomonini ham chizadi. Odatda GPU orqasi bilan turgan uchburchaklarni
  tashlab yuboradi (**culling**). Bizning chiziq va kvadratlarimiz uchun qaysi tomon ko'rinishining farqi yo'q.

### 2.5. Shader fayli qismma-qism

Avval butun fayl (2.7-bo'limda ham bor), keyin qismlari:

```hlsl
// Chiziqlar uchun shader (02-dars): yorug'liksiz, rangi LineRenderer'dan, ekrandagi rangga qo'shiladi.
Shader "AtomSim/AdditiveLine" {
    Properties {
        _Intensity ("Intensity", Float) = 1
    }

    SubShader {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

        Pass {
            Name "AdditiveLine"
            Tags { "LightMode" = "UniversalForward" }

            Blend One One
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float _Intensity;
            CBUFFER_END

            struct Attributes {
                float4 positionOS : POSITION;
                float4 color : COLOR;
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float4 color : COLOR;
            };

            Varyings Vert(Attributes input) {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.color = input.color;
                return output;
            }

            half4 Frag(Varyings input) : SV_Target {
                return half4(input.color.rgb * input.color.a * _Intensity, 1.0);
            }
            ENDHLSL
        }
    }
}
```

Fayl ikki qavatdan iborat. Tashqi qavat **ShaderLab**: Unity'ning o'z tili, shader haqida "pasport" yozadi.
`HLSLPROGRAM` va `ENDHLSL` orasidagi ichki qavat **HLSL**: GPU bajaradigan kodning o'zi.

**ShaderLab qismi:**

- **`Shader "AtomSim/AdditiveLine"`**: shader nomi. Material'ning Shader ro'yxatida `AtomSim` papkasi ichida
  `AdditiveLine` bo'lib ko'rinadi (qiya chiziq papka hosil qiladi).
- **`Properties`**: Inspector'da ko'rinadigan maydonlar. `_Intensity ("Intensity", Float) = 1` qatorida: koddagi nomi
  `_Intensity`, Inspector'dagi nomi "Intensity", turi son, boshlang'ich qiymati 1.
- **`SubShader`** va **`Tags`**: `"Queue" = "Transparent"` "bu obyektni oddiy (shaffof bo'lmagan) obyektlardan
  keyin chiz" degani. `"RenderPipeline" = "UniversalPipeline"`: bu shader URP uchun.
- **`Pass`**: bitta chizish. `"LightMode" = "UniversalForward"`: URP obyektlarni asosiy chizishda shu Pass'ni
  ishlatadi.
- **`Blend One One`**, **`ZWrite Off`**, **`Cull Off`**: 2.4-bo'lim.

**HLSL qismi:**

- **`#pragma vertex Vert`** va **`#pragma fragment Frag`**: "vertex shader `Vert` funksiyasida, fragment shader
  `Frag` funksiyasida" degani.
- **`#include "...Core.hlsl"`**: URP'ning tayyor funksiyalarini ulaydi, C# dagi `using` kabi. Ulardan biri
  `TransformObjectToHClip`.
- **`CBUFFER_START(UnityPerMaterial)` ... `CBUFFER_END`**: material'dagi sonlar shu yerga keladi. `Properties` dagi
  har bir maydon shu yerda xuddi shu nom bilan qaytadan yoziladi. Nomlar bir xil bo'lmasa, qiymat shader'ga
  yetib bormaydi.
- **`float4`**: to'rtta sondan iborat vektor, C# dagi `Vector4` kabi. `float2` ikkita son, `float` bitta.
  **`half4`** ham to'rtta son, faqat kamroq aniqlikda: rang uchun shu yetarli va GPU'da tezroq.
- **`struct Attributes`**: har bir uch mesh'dan nima olib keladi. `struct` bir nechta qiymatni bitta qutiga
  joylaydi. Bizga ikkitasi kerak: joyi va rangi.
- **`: POSITION`**, **`: COLOR`**: ikki nuqtadan keyingi so'z **semantika**: GPU'ga "bu maydonda nima turibdi"
  degan yorliq. `POSITION` uchning joyi, `COLOR` uning rangi. LineRenderer'ga bergan rangimiz aynan shu `COLOR` ga
  yoziladi.
- **`struct Varyings`**: vertex shader fragment shader'ga nima uzatadi. `SV_POSITION` ekrandagi joy, uni GPU
  rasterlash uchun ishlatadi.
- **`Vert`**: uchning local koordinatasini (`OS`, object space) ekran koordinatasiga (`CS`, clip space) aylantiradi
  va rangni o'zgartirmasdan uzatadi.
- **`Frag`**: piksel rangini qaytaradi. `input.color.rgb` uchta son (R, G, B), alpha'ga va `_Intensity` ga
  ko'paytiriladi. Oxirgi `1.0` alpha, `Blend One One` da u ishlatilmaydi. **`: SV_Target`** "bu natija ekranga
  yoziladi" degani.

Uchlar orasidagi piksellar qaysi rangni oladi? GPU `Varyings` ni uchlar orasida silliq aralashtiradi
(**interpolyatsiya**): ikki uchning o'rtasidagi piksel ikkala rangning o'rtachasini oladi. Bizda lentaning hamma
uchlari bir xil rangda, shuning uchun piksellar ham o'sha rangda bo'ladi.

### 2.6. Rangni koddan beramiz: massiv va `?:`

Endi C# tomoni. Orbitalarning rangini keyinroq, `n` o'zgarganda almashtirish kerak. Buning uchun yaratilgan
orbitalarni bir joyda saqlab qo'yamiz.

**Massiv (array)** bu bir xil turdagi qiymatlar uchun raqamlangan qutilar qatori. Oltita kataklik javonni tasavvur
qiling:

```csharp
readonly LineRenderer[] m_Orbits = new LineRenderer[MaxLevel];
```

- `LineRenderer[]`: "LineRenderer'lar massivi" turi.
- `new LineRenderer[MaxLevel]`: 6 kataklik yangi javon, kataklar hozircha bo'sh (`null`).
- `m_Orbits[0]` birinchi katak, `m_Orbits[5]` oltinchisi. **Raqamlash 0 dan boshlanadi**, shuning uchun `n`
  raqamli orbita `m_Orbits[n - 1]` da turadi.
- `readonly`: javonning o'zi hech qachon boshqasiga almashtirilmaydi. Kataklarga esa narsa qo'yish mumkin.

| Orbita `n` | Katak |
|---|---|
| 1 | `m_Orbits[0]` |
| 2 | `m_Orbits[1]` |
| 6 | `m_Orbits[5]` |
| 7 | yo'q: `m_Orbits[6]` xato beradi |

Javonga narsa qo'yish uchun `CreateOrbit` yaratgan chiziqni **qaytarishi** kerak. Funksiya nomining oldidagi `void`
"hech narsa qaytarmaydi" degani edi. `LineRenderer CreateOrbit(...)` esa "LineRenderer qaytaradi", oxirida
`return line;` turadi.

**Qaysi orbita joriy?** `bool isCurrent = i + 1 == m_Level;` qatorida `==` "tengmi?" degan savol. Javob `true` (ha)
yoki `false` (yo'q), uni `bool` turidagi o'zgaruvchi saqlaydi. `i` katak raqami (0 dan), `i + 1` esa orbita raqami
(1 dan).

**Ikki qiymatdan birini tanlash**: `isCurrent ? a : b` "agar `isCurrent` to'g'ri bo'lsa `a`, aks holda `b`" degani.
`if`/`else` ning bir qatorli shakli:

```csharp
Color color = isCurrent ? m_CurrentOrbitColor : m_OrbitColor;
```

**Ranglar Inspector'da.** `[SerializeField] Color` maydonlari Inspector'da rang qutisi bo'lib ko'rinadi, uni bossangiz
rang tanlagich ochiladi. Xira orbita `(0.45, 0.5, 0.62)` kulrang-havorang, alpha 0.35. Joriy orbita
`(0.25, 0.75, 1)` havorang, alpha 0.9. Joriy orbita qalinroq ham bo'ladi: eni 0.12, qolganlari 0.06.

`line.startColor` va `line.endColor` chiziqning boshi va oxiridagi rang. LineRenderer ular orasidagi rangni uchlarga
o'zi taqsimlaydi. Ikkalasi bir xil bo'lsa, butun chiziq bir rangda bo'ladi.

### 2.7. Kod

**Shader.** Yangi fayl `Assets/AtomSim/Shaders/AdditiveLine.shader`, mazmuni 2.5-bo'limdagi to'liq matn.

**`ElectronOrbit.cs`.** Maydonlar va `Start`:

```diff
         [SerializeField] Transform m_Electron;
         [SerializeField] Material m_LineMaterial;
         [SerializeField] float m_LevelSpacing = 4f;
         [SerializeField] float m_AngularSpeed = 2.4f;
+        [SerializeField] Color m_OrbitColor = new Color(0.45f, 0.5f, 0.62f, 0.35f);
+        [SerializeField] Color m_CurrentOrbitColor = new Color(0.25f, 0.75f, 1f, 0.9f);
 
+        readonly LineRenderer[] m_Orbits = new LineRenderer[MaxLevel];
         int m_Level = 1;
         float m_Angle;
 
         void Start() {
             for (int level = 1; level <= MaxLevel; level++) {
-                CreateOrbit(level);
+                m_Orbits[level - 1] = CreateOrbit(level);
             }
+            UpdateOrbitColors();
         }
```

`SetLevel` va `CreateOrbit` ning boshi:

```diff
         void SetLevel(int level) {
             m_Level = Mathf.Clamp(level, 1, MaxLevel);
+            UpdateOrbitColors();
             Debug.Log($"Orbita: n = {m_Level}, radius = {m_Level * m_LevelSpacing}");
         }
 
-        void CreateOrbit(int level) {
+        LineRenderer CreateOrbit(int level) {
             GameObject orbit = new GameObject($"Orbit n={level}");
             orbit.transform.SetParent(transform, false);
 
             LineRenderer line = orbit.AddComponent<LineRenderer>();
             line.sharedMaterial = m_LineMaterial;
             line.useWorldSpace = false;
             line.loop = true;
-            line.widthMultiplier = 0.1f;
 
             float radius = level * m_LevelSpacing;
```

`CreateOrbit` ning oxiri va yangi `UpdateOrbitColors` funksiyasi (faylning oxirigacha):

```diff
                 line.SetPosition(i, new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius);
             }
+            return line;
+        }
+
+        void UpdateOrbitColors() {
+            for (int i = 0; i < MaxLevel; i++) {
+                bool isCurrent = i + 1 == m_Level;
+                Color color = isCurrent ? m_CurrentOrbitColor : m_OrbitColor;
+                LineRenderer line = m_Orbits[i];
+                line.startColor = color;
+                line.endColor = color;
+                line.widthMultiplier = isCurrent ? 0.12f : 0.06f;
+            }
         }
     }
 }
```

`line.widthMultiplier = 0.1f` qatori `CreateOrbit` dan olib tashlandi: endi enini `UpdateOrbitColors` beradi.

`UpdateOrbitColors` ikki joyda chaqiriladi: `Start` da (orbitalar yaratilgach) va `SetLevel` da (`n` o'zgarganda).
`m_Orbits` yangi maydon bo'lsa ham `[SerializeField]` emas: unga Inspector kerak emas, uni kod o'zi to'ldiradi.

### 2.8. Amal

1. Project oynasida `Assets/AtomSim` ichida **Create → Folder**, nomi `Shaders`.
2. `Shaders` ichida o'ng tugma → **Create → Shader → URP Unlit Shader**. Nomi `AdditiveLine`. Unity namunaviy
   shader yaratadi.
3. Faylni ikki marta bosing, u Rider'da ochiladi. Ichidagi hammasini o'chirib, 2.5-bo'limdagi matnni qo'ying va
   saqlang.
4. `ElectronOrbit.cs` ga 2.7-bo'limdagi o'zgarishlarni kiriting va saqlang.
5. Unity'ga qayting. `Materials/OrbitLine` ni tanlang, Inspector'dagi **Shader** ro'yxatidan **AtomSim →
   AdditiveLine** ni tanlang. Material'da endi bitta maydon bor: **Intensity**, qiymati 1. Pastdagi **Render Queue**
   2000 dan 3000 ga o'zgaradi: bu `"Queue" = "Transparent"` ning soni.
6. **Ctrl+S** (Mac: **Cmd+S**).

### 2.9. Ishga tushirib ko'ring

- **Play**. Birinchi orbita yorqin havorang va qalinroq, qolgan beshtasi xira kulrang-havorang.
- Game oynasiga bosib, yuqori strelkani bosing: yorqinlik ikkinchi orbitaga o'tadi, Console'da
  `Orbita: n = 2, radius = 8`. Pastki strelka uni qaytaradi.
- Play paytida `Atom` ni tanlab, Electron Orbit'dagi **Current Orbit Color** ni boshqa rangga o'zgartiring va
  strelkani bosing: yangi rang `UpdateOrbitColors` chaqirilganda qo'llanadi. Play'dan chiqqach, rang qaytadi.
- Play paytida `OrbitLine` materialining **Intensity** sini `3` qiling: hamma chiziqlar yorqinlashadi. Keyin `1` ga
  qaytaring. Material Play'dan keyin ham o'zgargan holda qoladi, chunki material bu sahna emas, fayl.

Unity'da tekshirdim: shader xatosiz kompilyatsiya bo'ldi, C# kod 0 xato va 0 ogohlantirish bilan. Virtual
klaviatura bilan yuqori strelkani ikki marta bosganimda yorqin rang va 0.12 en 3-orbitaga o'tdi.

<details>
<summary>Shu qadamdan keyingi to'liq <code>ElectronOrbit.cs</code> (solishtirish uchun)</summary>

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

namespace AtomSim.Bohr {
    /// <summary>
    /// Elektronni yadro atrofida doira bo'ylab aylantiradi (01-dars) va orbitalarni chizadi (02-dars).
    /// Yuqori va pastki strelka orbita raqamini (n) o'zgartiradi.
    /// </summary>
    public sealed class ElectronOrbit : MonoBehaviour {
        const int MaxLevel = 6;
        const int OrbitPoints = 160;

        [SerializeField] Transform m_Electron;
        [SerializeField] Material m_LineMaterial;
        [SerializeField] float m_LevelSpacing = 4f;
        [SerializeField] float m_AngularSpeed = 2.4f;
        [SerializeField] Color m_OrbitColor = new Color(0.45f, 0.5f, 0.62f, 0.35f);
        [SerializeField] Color m_CurrentOrbitColor = new Color(0.25f, 0.75f, 1f, 0.9f);

        readonly LineRenderer[] m_Orbits = new LineRenderer[MaxLevel];
        int m_Level = 1;
        float m_Angle;

        void Start() {
            for (int level = 1; level <= MaxLevel; level++) {
                m_Orbits[level - 1] = CreateOrbit(level);
            }
            UpdateOrbitColors();
        }

        void Update() {
            HandleKeyboard();

            m_Angle += m_AngularSpeed * Time.deltaTime;
            float radius = m_Level * m_LevelSpacing;
            m_Electron.localPosition = new Vector3(Mathf.Cos(m_Angle), Mathf.Sin(m_Angle), 0f) * radius;
        }

        void HandleKeyboard() {
            Keyboard keyboard = Keyboard.current;
            if (keyboard == null) return;

            if (keyboard.upArrowKey.wasPressedThisFrame) SetLevel(m_Level + 1);
            if (keyboard.downArrowKey.wasPressedThisFrame) SetLevel(m_Level - 1);
        }

        void SetLevel(int level) {
            m_Level = Mathf.Clamp(level, 1, MaxLevel);
            UpdateOrbitColors();
            Debug.Log($"Orbita: n = {m_Level}, radius = {m_Level * m_LevelSpacing}");
        }

        LineRenderer CreateOrbit(int level) {
            GameObject orbit = new GameObject($"Orbit n={level}");
            orbit.transform.SetParent(transform, false);

            LineRenderer line = orbit.AddComponent<LineRenderer>();
            line.sharedMaterial = m_LineMaterial;
            line.useWorldSpace = false;
            line.loop = true;

            float radius = level * m_LevelSpacing;
            float step = 2f * Mathf.PI / OrbitPoints;
            line.positionCount = OrbitPoints;
            for (int i = 0; i < OrbitPoints; i++) {
                float angle = i * step;
                line.SetPosition(i, new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius);
            }
            return line;
        }

        void UpdateOrbitColors() {
            for (int i = 0; i < MaxLevel; i++) {
                bool isCurrent = i + 1 == m_Level;
                Color color = isCurrent ? m_CurrentOrbitColor : m_OrbitColor;
                LineRenderer line = m_Orbits[i];
                line.startColor = color;
                line.endColor = color;
                line.widthMultiplier = isCurrent ? 0.12f : 0.06f;
            }
        }
    }
}
```

</details>

## 3-qadam. Sharlar o'rniga porlayotgan nuqtalar

Yadro va elektron hali ham yorug'lik tushib turgan sharlar. Ularni o'zi nur sochayotgan nuqtalarga aylantiramiz.
Buning uchun shar o'rniga eng oddiy shakl, kvadrat (**Quad**) olamiz va uning ustiga shader bilan yumaloq porlash
chizamiz.

Fizika haqida bir eslatma: haqiqiy proton va elektronning rangi yo'q, ular yorug'lik qaytaradigan darajada katta
ham emas. To'q sariq yadro va havorang elektron faqat ajratib turish uchun tanlangan shartli ranglar, ilmiy
chizmalarda ham shunday qilinadi.

### 3.1. Mesh: uchlar va uchburchaklar

2.2-bo'limda aytdik: GPU hamma narsani uchburchaklardan chizadi. Obyektning shakli **mesh** deyiladi va ikki
ro'yxatdan iborat: uchlar va ularni uchtadan birlashtirgan uchburchaklar. Unity'ning tayyor shakllarini
solishtiramiz:

| Shakl | Uchlar | Uchburchaklar |
|---|---|---|
| Sphere (shar) | 515 | 768 |
| Quad (kvadrat) | 4 | 2 |

Quad'ning to'rtta uchi `(±0.5, ±0.5, 0)` da. Ikkita uchburchak birgalikda kvadratni qoplaydi:

```
  2 (-0.5, 0.5) ●───────────● 3 (0.5, 0.5)
                │         / │
                │       /   │
                │     /     │
                │   /       │
                │ /         │
  0 (-0.5,-0.5) ●───────────● 1 (0.5, -0.5)

  1-uchburchak: 0, 3, 1        2-uchburchak: 3, 0, 2
```

Quad `x` va `y` tekisligida yotadi va `-z` tomonga qaragan. Bizning kamera aynan `-z` tomonda turibdi, shuning uchun
kvadratni to'g'ri oldidan ko'radi. Bu sonlarni Unity'ning o'zidan o'qib oldim.

### 3.2. UV: sirt ustidagi manzil

Har bir uchda joydan tashqari yana ikkita son bor: **UV**. Bu "sirt ustidagi manzil": rasm yoki naqsh sirtga qanday
yopishtirilishini aytadi. Ikkita o'q `u` (chapdan o'ngga) va `v` (pastdan yuqoriga), ikkalasi ham 0 dan 1 gacha.
Quad'da:

| Uch | Joyi | UV |
|---|---|---|
| 0 | chap-past | (0, 0) |
| 1 | o'ng-past | (1, 0) |
| 2 | chap-yuqori | (0, 1) |
| 3 | o'ng-yuqori | (1, 1) |

Uchlar orasidagi piksellar UV'ni interpolyatsiya bilan oladi (2.5-bo'lim): kvadratning aynan o'rtasidagi piksel
`(0.5, 0.5)`, chap chetining o'rtasi `(0, 0.5)`.

Bizga porlash markazda bo'lishi kerak, ya'ni markaz `(0, 0)` bo'lgani qulay. Shuning uchun vertex shader'da UV'ni
o'zgartiramiz:

```
yangi = uv × 2 − 1
```

| Joy | `uv` | `uv × 2 − 1` |
|---|---|---|
| chap-past burchak | (0, 0) | (−1, −1) |
| markaz | (0.5, 0.5) | (0, 0) |
| o'ng chetining o'rtasi | (1, 0.5) | (1, 0) |
| o'ng-yuqori burchak | (1, 1) | (1, 1) |

Endi markaz `(0, 0)`, chetlar ±1 da.

### 3.3. Markazdan masofa

Har bir piksel markazdan qancha uzoqda? 01-darsdagi Pifagor (3.3-bo'lim):

```
r = sqrt(x × x + y × y)
```

HLSL'da buning tayyor funksiyasi bor: `length(uv)` (uzunlik). Bir nechta nuqta:

| Piksel | `(x, y)` | `r` |
|---|---|---|
| markaz | (0, 0) | 0 |
| o'ng chetining o'rtasi | (1, 0) | 1 |
| (0.6, 0.8) nuqta | (0.6, 0.8) | `sqrt(0.36 + 0.64) = 1` |
| burchak | (1, 1) | `sqrt(2) = 1.414` |

`r = 1` bo'lgan hamma nuqtalar kvadratga ichki chizilgan doirani hosil qiladi. Porlashni shu doira ichida chizamiz:
`r` ga qarab yorqinlik beramiz, natijada yumaloq dog' chiqadi, garchi shakl kvadrat bo'lsa ham.

### 3.4. Porlash shakli: o'zak va gardish

Porlash ikki qismdan yig'iladi.

**O'zak (core)**: markazdagi yorqin, aniq chegarali doira. `r` 0.15 gacha to'liq yorug' (1), 0.25 dan keyin
umuman yo'q (0), orasida silliq o'tadi. Buni **`smoothstep`** qiladi. `smoothstep(a, b, r)` `r` a dan kichik bo'lsa
0, b dan katta bo'lsa 1, orasida yumshoq S-simon egri bo'ylab 0 dan 1 ga o'tadi. Bizga teskarisi kerak (markazda
1), shuning uchun 1 dan ayiramiz:

```
core = 1 − smoothstep(0.15, 0.25, r)
```

`0.15` va `0.25` qayerdan: material'dagi **Core Size** (`_Core`) 0.25, uning 0.6 qismi 0.15.

**Gardish (halo)**: o'zakdan tashqariga sekin so'nib boradigan nur:

```
halo = (1 − r) ^ 3 × 0.6
```

`^ 3` **uchinchi daraja**: son o'ziga uch marta ko'paytiriladi. `0.5 ^ 3 = 0.5 × 0.5 × 0.5 = 0.125`. 1 dan kichik son
darajaga ko'tarilsa kichrayadi, shuning uchun gardish markazdan uzoqlashgani sari tez so'nadi. HLSL'da daraja
`pow(son, 3.0)`. `saturate(x)` esa sonni 0 va 1 orasida ushlaydi, 01-darsdagi `Mathf.Clamp(x, 0, 1)` ning o'zi:
burchaklarda `r > 1`, `1 − r` manfiy bo'lib qolmasligi uchun.

Ikkalasini qo'shamiz (qo'lda hisoblangan):

| `r` | core | halo | core + halo |
|---|---|---|---|
| 0 | 1 | 0.6 | 1.6 |
| 0.15 | 1 | 0.37 | 1.37 |
| 0.2 | 0.5 | 0.31 | 0.81 |
| 0.25 | 0 | 0.25 | 0.25 |
| 0.5 | 0 | 0.075 | 0.075 |
| 0.75 | 0 | 0.009 | 0.009 |
| 1 | 0 | 0 | 0 |

```
yorqinlik
 1.6 ●
     │●
     │ ●
 0.8 │  ●
     │    ●
     │       ●
   0 ┼──────────●────●────●──── r
     0  0.15 0.25  0.5  0.75  1
```

Pikselning rangi `material rangi × Intensity × (core + halo)`. `r = 1` da va burchaklarda yorqinlik 0, qo'shiluvchi
aralashtirishda 0 hech narsa qo'shmaydi (2.4-bo'lim). Shu sababli kvadratning burchaklari ko'rinmaydi.

Bu shakl fizik formula emas, chiroyli ko'rinishi uchun tanlangan. Haqiqiy nuqtaviy yorug'lik manbaining yorqinligi
masofaning kvadratiga teskari kamayadi, uni keyingi darslarda uchratamiz.

### 3.5. Material'ning shader'ini almashtirish

Material ikki narsadan iborat: shader va uning qiymatlari (01-dars, 1.7). Material'ning shader'ini almashtirsangiz,
Unity eski qiymatlarni o'chirmaydi, ularni nomlari bo'yicha saqlab qo'yadi. Yangi shader'da xuddi shu nomli maydon
bo'lsa, u eski qiymatni oladi.

URP'ning Lit shader'ida rang maydonining nomi `_BaseColor`. Glow shader'imizdagi rangga ham aynan shu nomni beramiz.
Shunda `Nucleus.mat` ni Glow'ga o'tkazganda 01-darsda tanlagan `FF7040` rangi saqlanib qoladi, qaytadan kiritish
shart emas. Buni Unity'da tekshirdim: shader almashgandan keyin ham rang `FF7040` qoldi.

Yorqinlikni rangga emas, alohida `_Intensity` soniga beramiz, xuddi AdditiveLine'dagidek. Rangni Unity'ning `[HDR]`
belgisi bilan ham yozish mumkin edi. Lekin `[HDR]` rangni Unity boshqacha uzatadi (2.3-bo'limdagi gamma
o'zgartirishsiz), shuning uchun Lit'dan o'tgan `FF7040` och-pushti tusga kirib qolardi. Buni o'lchadim.
Alohida `_Intensity` bilan rang o'z holicha qoladi, yorqinlikni esa bitta son boshqaradi.

### 3.6. Kod

Yangi fayl `Assets/AtomSim/Shaders/Glow.shader`:

```hlsl
// Kvadrat (Quad) ustida yumaloq porlash (02-dars): yadro va elektron uchun.
Shader "AtomSim/Glow" {
    Properties {
        _BaseColor ("Color", Color) = (1, 1, 1, 1)
        _Intensity ("Intensity", Float) = 1
        _Core ("Core Size", Range(0.01, 1)) = 0.25
    }

    SubShader {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

        Pass {
            Name "Glow"
            Tags { "LightMode" = "UniversalForward" }

            Blend One One
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float _Intensity;
                float _Core;
            CBUFFER_END

            struct Attributes {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings Vert(Attributes input) {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv * 2.0 - 1.0;
                return output;
            }

            half4 Frag(Varyings input) : SV_Target {
                float r = length(input.uv);
                float core = 1.0 - smoothstep(_Core * 0.6, _Core, r);
                float halo = pow(saturate(1.0 - r), 3.0) * 0.6;
                return half4(_BaseColor.rgb * _Intensity * (core + halo), 1.0);
            }
            ENDHLSL
        }
    }
}
```

AdditiveLine'dan farqlari:

- **`Properties`**: uchta maydon. `Range(0.01, 1)` Inspector'da slayder bo'lib chiqadi.
- **`float2 uv : TEXCOORD0`**: mesh'dan UV keladi (`TEXCOORD0` UV'ning semantikasi). Rang endi mesh'dan emas,
  material'dan keladi, shuning uchun `COLOR` yo'q.
- **`Vert`**: `uv × 2 − 1` (3.2-bo'lim) va natijani `Varyings` orqali piksellarga uzatadi.
- **`Frag`**: 3.3 va 3.4-bo'limlardagi uch qator.

### 3.7. Amal

**Shader.**

1. `Assets/AtomSim/Shaders` ichida **Create → Shader → URP Unlit Shader**, nomi `Glow`. Rider'da ochib, ichini
   3.6-bo'limdagi matn bilan almashtiring, saqlang.

**Yadro.**

2. Hierarchy'da `Nucleus` ni tanlang. **Mesh Filter** komponentidagi **Mesh** maydoni hozir `Sphere`. Uning
   o'ng tomonidagi kichik doiracha tugmani (⊙) bosing, ochilgan oynada `Quad` ni toping va ikki marta bosing. Scene
   oynasida shar yassi kvadratga aylanadi.
3. Transform → Scale `(6, 6, 6)`. Kvadrat katta, lekin uning faqat markazidagi o'zak yorqin: o'zak radiusi
   `0.25 × 6 / 2 = 0.75` birlik, 01-darsdagi shar radiusi 1 edi. Qolgan joyni so'nib boruvchi gardish egallaydi.
4. `Nucleus` ning **Mesh Renderer** komponentidagi material `Nucleus` ni ikki marta bosing (yoki Project'da
   `Materials/Nucleus` ni tanlang). Inspector'dagi **Shader** ro'yxatidan **AtomSim → Glow** ni tanlang. **Color**
   `FF7040` bo'lib qoladi, **Intensity** 1, **Core Size** 0.25.

**Elektron.**

5. `Electron` uchun ham xuddi shunday: Mesh → `Quad`, Scale `(3, 3, 3)`, `Electron` materialining shader'i
   **AtomSim → Glow**.

**Yorug'lik.**

6. Endi sahnada yorug'likka qaraydigan material qolmadi. Hierarchy'da `Directional Light` ni tanlab, **Delete**
   (Mac: **Cmd+Delete**) bilan o'chiring.
7. **Ctrl+S** (Mac: **Cmd+S**).

### 3.8. Ishga tushirib ko'ring

- **Play**. Yadro o'rnida markazi yorqin to'q sariq, atrofi sekin so'nib boruvchi dog'. Elektron esa xuddi shunday
  havorang dog', u orbitalar ustida aylanadi. Kvadratning burchaklari ko'rinmaydi.
- Sharlardagi soya va yaltiroq joylar yo'qoldi: dog'lar yorug'likka bog'liq emas, `Directional Light` ni o'chirsak
  ham ular o'zgarmadi.
- Play paytida `Nucleus` materialining **Core Size** slayderini suring: 0.5 da o'zak kattalashadi, 0.05 da kichik
  yorqin nuqtaga aylanadi. Oxirida 0.25 ga qaytaring.
- Dog'lar yaxshi, lekin hali "porlamaydi": rangi oddiy, atrofga nur sochmaydi. Bunisi keyingi qadamda.

Unity'da tekshirdim: shader xatosiz kompilyatsiya bo'ldi. Ikkala material ham shader almashgach to'q sariq va
havorang ranglarini saqladi, Render Queue 2000 dan 3000 ga o'tdi.

## 4-qadam. HDR va Bloom

### 4.1. Monitor qancha yorqin bo'la oladi

2.3-bo'limda aytdik: pikseldagi har bir chiroq 0 dan 1 gacha yonadi. Monitor 1 dan yorqinini ko'rsata olmaydi.
Haqiqiy dunyoda esa yorqinlik farqi juda katta: quyoshli ko'cha xonadagi chiroqdan yuzlab marta yorug', quyoshning
o'zi esa undan ham ko'p. Ko'z va fotoapparat bunga moslashadi.

Yadroni "monitor ko'rsata oladiganidan 4 marta yorug'" qilib bersak nima bo'ladi? Material'ning **Intensity** sini
4 qilamiz. `FF7040` ning eng kuchli kanali R = 1, demak markazdagi qiymat:

```
R = rang × Intensity × (core + halo) = 1 × 4 × 1.6 = 6.4
```

Unity'da o'lchadim: kamera hisoblagan rasmda yadro markazidagi R 6.25 chiqdi. 6.4 dan biroz kam, chunki piksel
aniq markazda emas, undan sal chetroqda turibdi.

Monitorga 6.25 ni ko'rsatib bo'lmaydi. Eng oddiy yo'l uni 1 da **kesib** qo'yish (clipping). Lekin kesilganda rang
ham buziladi: to'q sariq `(6.25, 1.0, 0.32)` kesilib `(1, 1, 0.32)` bo'ladi, bu esa sariq. Rangni rang qilib turgan
kanallar orasidagi nisbat yo'qoladi.

### 4.2. HDR: 1 dan katta sonlarni saqlash

**HDR** (High Dynamic Range, "keng yorqinlik oralig'i") kamera rasmni 1 dan katta sonlarni ham saqlay oladigan
xotiraga chizishi degani. Loyihamizda bu allaqachon yoqilgan: URP sozlamalarida (`Assets/Settings/PC_RPAsset`)
**HDR** belgilangan. Rasm tayyor bo'lgach, uni 0–1 oralig'iga aqlli usulda o'tkazish kerak. Buni
**post-processing** qiladi.

### 4.3. Post-processing va Volume

**Post-processing** "keyingi ishlov" degani: kamera sahnani chizib bo'lgach, URP tayyor rasmga effektlar qo'llaydi,
xuddi telefondagi suratga filtr qo'yilgandek. Uchta effekt ishlatamiz: Bloom, Tonemapping va Vignette.

Effektlar qayerda sozlanadi:

- **Volume Profile** effektlar ro'yxatini saqlaydigan fayl. Har bir effekt **override** ("ustidan yozish") deb
  ataladi.
- **Volume** komponenti sahnadagi obyektga qo'yiladi va qaysi profil ishlashini aytadi. **Global** Volume butun
  sahnaga ta'sir qiladi.
- Kameraning **Post Processing** belgisi: o'chiq bo'lsa, kamera hech qanday effekt qo'llamaydi.

Yana bir qatlam bor. URP sozlamalarining o'zida ham standart profil turibdi: `PC_RPAsset` da u `SampleSceneProfile`.
Unda kuchsiz Bloom (Intensity 0.25), Tonemapping (Neutral) va Vignette (0.2) bor. Kamerada Post Processing yoqilishi
bilan ular ishlay boshlaydi, hatto sahnada Volume bo'lmasa ham. Bizning Global Volume ularning ustidan yozadi, lekin
faqat **belgilangan** maydonlarda. Belgilanmagan maydon standart profildan olinadi.

### 4.4. Bloom

Tunda ko'chadagi chiroqqa qarang: chiroq atrofida nur gardishi ko'rinadi. Yorug'lik ko'z gavhari va ko'z ichidagi
suyuqlikda biroz sochiladi, kuchli nur esa ko'p sochiladi. Fotoapparat linzasida ham shunday bo'ladi. **Bloom** shuni
taqlid qiladi:

1. rasmdan yorqinligi **Threshold** (chegara) dan katta piksellarni ajratib oladi;
2. ularni bir necha marta xiralashtiradi (yoyadi);
3. natijani asl rasmga qo'shadi. Qo'shish allaqachon tanish: 2.4-bo'lim.

| Maydon | Qiymat | Ma'nosi |
|---|---|---|
| Threshold | 0.85 | shundan yorqin piksellar nur sochadi |
| Intensity | 1.1 | sochilgan nur qanchalik kuchli |
| Scatter | 0.72 | nur qanchalik keng yoyiladi (0 dan 1 gacha) |
| High Quality Filtering | ✓ | silliqroq, biroz qimmatroq yoyish |

Bu qiymatlar tayyor loyihadan olingan. Yadro markazi 6.25, chegara 0.85: yadro kuchli porlaydi. Xira orbitalar
taxminan 0.1: ular porlamaydi.

### 4.5. Tonemapping

**Tonemapping** 0 dan cheksizgacha bo'lgan yorqinlikni 0–1 ga kesmasdan, silliq siqib joylaydi: qorong'i joylar
deyarli o'zgarmaydi, juda yorug' joylar esa sekin-asta 1 ga yaqinlashadi. Fotografning yorug'likni to'g'ri tanlashiga
o'xshaydi. **Neutral** rejim ranglarni iloji boricha o'zgartirmaydi.

O'lchadim. Yadro markazidagi piksel ekranda:

| Holat | Ekrandagi rang (0–1) |
|---|---|
| Post-processing o'chiq (kesish) | (1.00, 1.00, 0.60): och sariq |
| Tonemapping None | (1.00, 1.00, 0.64): xuddi shunday kesilgan |
| Tonemapping Neutral | (0.84, 0.84, 0.61): oqish, yumshoq |

Juda yorug' narsaning markazi oq-sariq bo'lib ko'rinishi tabiiy: olov yoki cho'g'lanma lampaning suratida ham eng
issiq joy oqarib ketadi.

### 4.6. Vignette

**Vignette** ekranning chetlari va burchaklarini biroz qoraytiradi, xuddi eski fotoapparat linzasidagidek. Ko'z
o'zi markazga, atomga tortiladi. **Intensity** 0.28 qanchalik qorong'i, **Smoothness** 0.45 chegaraning
yumshoqligi.

### 4.7. Amal

**Yorqinlik.**

1. `Materials/Nucleus` ni tanlang, **Intensity** ni `4` qiling. `Materials/Electron` uchun ham `4`.
2. Play'da qarang: dog'larning markazi och sariq va oq bo'lib qoldi, ranglar kesildi (4.1-bo'lim). Play'dan chiqing.

**Post-processing.**

3. `Main Camera` ni tanlang. Camera komponentining **Rendering** bo'limida **Post Processing** ni belgilang.
4. Play'da qarang: markaz yumshoqroq bo'ldi, atrofida kuchsiz nur paydo bo'ldi. Bu standart profilning ishi
   (4.3-bo'lim). Play'dan chiqing.

**O'z profilimiz.**

5. Project'da `Assets/AtomSim` ichida **Create → Folder**, nomi `Settings`.
6. `Settings` ichida o'ng tugma → **Create → Rendering → Volume Profile**. Nomi `AtomSimVolumeProfile`.
7. Profilni tanlang. Inspector'da **Add Override → Post-processing → Bloom**. Har bir maydonning chap tomonida
   belgi qutisi bor: belgilangan maydongina ustidan yozadi. Belgilab, qiymatlarni kiriting: **Threshold** `0.85`,
   **Intensity** `1.1`, **Scatter** `0.72`, **High Quality Filtering** ✓.
8. **Add Override → Post-processing → Tonemapping**: **Mode** ni belgilab, **Neutral** ni tanlang.
9. **Add Override → Post-processing → Vignette**: **Intensity** `0.28`, **Smoothness** `0.45`.

**Global Volume.**

10. Hierarchy'ning bo'sh joyida o'ng tugma → **Volume → Global Volume**. `Global Volume` obyekti paydo bo'ladi,
    uning Volume komponentida **Mode: Global**.
11. `AtomSimVolumeProfile` ni Project'dan sudrab, Volume komponentining **Profile** maydoniga tashlang.
12. **Ctrl+S** (Mac: **Cmd+S**).

### 4.8. Ishga tushirib ko'ring

- **Play**. Yadro oq-sariq markazli, atrofida keng to'q sariq nur. Elektron havorang porlaydi va aylanganda nuri
  bilan birga yuradi. Joriy orbita ham sal porlaydi (uning ko'k kanali chegara 0.85 dan biroz yuqori), xira
  orbitalar porlamaydi. Ekran burchaklari biroz qorong'iroq.
- Play paytida `Global Volume` ni tanlab, uning belgisini (Inspector'ning eng tepasidagi qutini) o'chirib-yoqing:
  o'chganda faqat standart profil qoladi, nur kuchsizroq va burchaklar yorug'roq.
- Play paytida profildagi Bloom **Threshold** ni `0` qiling: endi orbitalar ham porlaydi, chunki hamma narsa
  chegaradan yorug'. Keyin `0.85` ga qaytaring. Profil fayl bo'lgani uchun Play'dagi o'zgarish saqlanib qoladi.

Unity'da tekshirdim: kamerada Post Processing yoqilganda, bizning Volume'imiz bo'lmasa ham, standart profil
Tonemapping Neutral'ni qo'lladi. Bizning profilda Tonemapping belgisi olib tashlanganda ham natija Neutral bo'lib
qoldi, chunki qiymat standart profildan olindi.

## Nima buzilsa nima bo'ladi

Hammasini Unity'da ataylab buzib, o'lchadim:

| O'zgarish | Natija | Xabar bormi |
|---|---|---|
| Electron Orbit'da **Line Material** bo'sh | orbitalar pushti-binafsha | yo'q |
| shader'da nuqtali vergul tushib qolgan (`output.color = input.color`) | shu shader'li hamma narsa pushti-binafsha | ha: `Shader error in 'AtomSim/AdditiveLine': syntax error: unexpected token 'return' at ...AdditiveLine.shader(41) (on dx12)` |
| Glow'da `Blend One One` yo'q | elektron atrofida qora kvadrat paydo bo'lib, orqasidagi orbita chizig'ini va yadro nurini to'sadi | yo'q |
| Glow'da `uv * 2.0 - 1.0` o'rniga `uv` | porlash kvadratning chap-pastki burchagiga ko'chadi, chorak doira bo'lib ko'rinadi | yo'q |
| `for` da `i < OrbitPoints` o'rniga `i <= OrbitPoints` | orbitalar chiziladi, lekin har biri uchun Console'da qizil xato | ha: `LineRenderer.SetPosition index out of bounds!` |
| `m_Orbits[level - 1]` o'rniga `m_Orbits[level]` | `Start` 6-orbitada to'xtaydi, orbitalar oq bo'lib qoladi, ranglar qo'llanmaydi | ha: `IndexOutOfRangeException: Index was outside the bounds of the array.` |
| kamerada Post Processing o'chiq | Bloom ham, Tonemapping ham yo'q, yorug' markazlar kesilib och sariq | yo'q |

Shader xatosida qator raqami xato **keyingi** qatorni ko'rsatadi: kompilyator nuqtali vergulni kutib, keyingi so'zga
(`return`) duch kelgandagina adashganini tushunadi. Shuning uchun xato qatorining bir qator yuqorisiga ham qarang.
Mac'da xabar oxirida `(on dx12)` o'rniga `(on metal)` chiqishini kutaman.

## Git

Dars tugagach:

```
git add .
git status
git commit -m "Lesson 02: orbit lines, AdditiveLine and Glow shaders, Bloom"
git push
```

`git status` da yangi fayllar bilan birga ularning `.meta` fayllari bo'lishi kerak: `Shaders.meta`,
`AdditiveLine.shader.meta`, `Glow.shader.meta`, `OrbitLine.mat.meta`, `Settings.meta`,
`AtomSimVolumeProfile.asset.meta`. O'zgargan fayllar: `ElectronOrbit.cs`, sahna, `Nucleus.mat`, `Electron.mat`.

## Ixtiyoriy tajribalar

1. **Nuqtalar soni.** `OrbitPoints` ni `6` qiling: orbitalar olti burchakli bo'ladi, 1.2-bo'limdagi 55 piksellik
   farq ko'zga tashlanadi. `12` va `32` ni ham sinab, 160 ga qaytaring.
2. **Ranglarni qo'shish.** Play paytida `Atom` dagi Electron Orbit'ning **Level Spacing** ini `0` qiling. Elektron
   radiusi `n × 0 = 0` bo'lib, u aynan yadro ustiga tushadi. Orbitalar joyida qoladi, chunki ular faqat `Start` da
   chizilgan. Ikki dog' ustma-ust tushgan markaz oqish-pushti bo'ladi: to'q sariq bilan havorang qo'shildi
   (2.4-bo'lim). O'lchadim: post-processing'gacha markazdagi qiymat yadroniki `(6.25, 1.01, 0.32)` bilan elektronniki
   `(0.31, 2.15, 6.11)` ning yig'indisi, `(6.56, 3.16, 6.43)` bo'ldi.
3. **Gardish shakli.** `Glow.shader` dagi `pow(..., 3.0)` ni `1.0` qiling: gardish chetgacha bir tekis so'nadi
   va ancha keng ko'rinadi (`r = 0.5` da `0.5 × 0.6 = 0.3`, uchinchi darajada 0.075 edi). `6.0` da esa gardish
   o'zakka yopishib qoladi (`r = 0.5` da 0.009). 3.4-bo'limdagi jadvalni yangi daraja bilan qo'lda hisoblab,
   ko'rganingiz bilan solishtiring. Oxirida `3.0` ga qaytaring.

## Lug'at

| So'z | Ma'nosi |
|---|---|
| `LineRenderer` | nuqtalar ro'yxati orqali kameraga qaragan lenta chizadigan komponent |
| ko'pburchak bilan yaqinlashtirish | doirani ko'p nuqtali to'g'ri kesmalar bilan almashtirish; xato `r × (1 − cos(π / N))` |
| `for` sikli | boshlanish, shart va qadam bilan bir ishni ko'p marta takrorlash |
| massiv (array) | raqamlangan qutilar qatori; raqamlash 0 dan boshlanadi |
| `bool` | `true` yoki `false` qiymatli tur |
| `?:` | `shart ? a : b`: shart to'g'ri bo'lsa `a`, aks holda `b` |
| `return` | funksiyadan qiymat qaytarish |
| CPU, GPU | protsessor (kam, aqlli ishchilar) / grafik karta (ko'p, oddiy ishchilar) |
| shader | GPU'da ishlaydigan dastur |
| ShaderLab, HLSL | shader faylining tashqi "pasport" tili / GPU kodi yoziladigan til |
| mesh | shakl: uchlar (vertices) va uchburchaklar |
| vertex shader | har bir uch uchun ishlaydi: ekrandagi joyini topadi |
| fragment shader | har bir piksel uchun ishlaydi: rangini topadi |
| interpolyatsiya | uchlar orasidagi piksellar uchun qiymatlarni silliq aralashtirish |
| semantika | `POSITION`, `COLOR`, `TEXCOORD0`, `SV_Target`: GPU'ga maydonning ma'nosini aytuvchi yorliq |
| RGB, alpha | rangning qizil, yashil, ko'k qismlari (0–1) / to'rtinchi son |
| qo'shiluvchi aralashtirish (`Blend One One`) | yangi rang ekrandagi rangga qo'shiladi, xuddi yorug'likdek |
| depth buffer, `ZWrite` | "kim kameraga yaqinroq" daftari / unga yozish |
| culling, `Cull Off` | orqasi bilan turgan uchburchaklarni tashlab yuborish / tashlamaslik |
| UV | sirt ustidagi manzil, 0 dan 1 gacha |
| `length` | vektor uzunligi, Pifagor: `sqrt(x × x + y × y)` |
| `smoothstep(a, b, x)` | `x` a dan b gacha o'sganda 0 dan 1 ga silliq o'tish |
| daraja, `pow` | sonni o'ziga bir necha marta ko'paytirish: `0.5 ^ 3 = 0.125` |
| `saturate` | sonni 0 va 1 orasida ushlash |
| HDR | 1 dan yorug' qiymatlarni saqlay oladigan rasm |
| kesish (clipping) | 1 dan katta qiymatni 1 ga tenglash; rang buziladi |
| post-processing | tayyor rasmga qo'llanadigan effektlar |
| Volume, Volume Profile, override | effektlar qayerda ishlashi / effektlar ro'yxati fayli / ro'yxatdagi bitta effekt |
| Bloom | yorug' piksellarning nurini atrofga yoyish |
| Threshold | Bloom boshlanadigan yorqinlik chegarasi |
| Tonemapping | keng yorqinlikni 0–1 ga silliq siqish |
| Vignette | ekran chetlarini qoraytirish |

## Keyingi dars

03-darsda fizikaga qaytamiz: **Bor atomi**. Nega elektron yadroga qulab tushmaydi, energiya nima, nega u faqat
ma'lum orbitalarda bo'la oladi va `E = −13.6 / n²` formulasi qayerdan keladi. Kodda fizik hisoblar uchun alohida
`Hydrogen` classi va birinchi avtomatik test paydo bo'ladi, elektron esa orbitalar orasida sakramasdan, silliq o'tadi.

Shu dars tugagach yozing. Xohlasangiz, ishingizni ko'rib chiqaman.
