# 01. Birinchi skript: yadro atrofida aylanayotgan elektron

## Bu darsda nima qo'shamiz va nega aynan hozir

00-darsda bo'sh, deyarli qora sahna yaratdik. Endi unga atom qo'yamiz: o'rtada yadro, atrofida doira bo'ylab
aylanayotgan elektron. Klaviaturadagi strelkalar bilan elektronni uzoqroq yoki yaqinroq orbitaga o'tkazasiz.

Bu kursdagi eng sodda atom rasmi va u **noto'g'ri**. Haqiqiy elektron yadro atrofida sayyora kabi aylanmaydi. Lekin
kurs aynan shu rasmdan boshlanadi, chunki tarixda fizika ham shundan boshlangan: avval shu model paydo bo'ldi, keyin
uning muammolari topildi va ularni tuzatish 100 yil oldin butun kvant mexanikasini tug'dirdi. Biz ham shu yo'ldan
boramiz: 03-darsda Bor modeli, 06-darsdan boshlab haqiqiy kvant "bulut".

Unity tomonida bu dars poydevor: GameObject, komponent, Transform, C# skript, `Update` va `Time.deltaTime`,
Inspector'dagi maydonlar va klaviatura. Keyingi har bir darsdagi har bir skript shularga tayanadi.

**Bu darsni o'qish uchun matematika yoki fizika bilish shart emas.** Har bir tushuncha noldan tushuntiriladi: avval
kundalik misol, keyin kichik sonlar bilan qo'lda hisob, oxirida kodda qayerda turgani. Bo'limlarni tartib bilan
o'qing. Dars oxirida qisqa lug'at bor.

Dars oxirida loyiha shunday bo'ladi:

```
Assets/AtomSim/
├── Scenes/AtomSimulation.unity     (o'zgaradi: yadro, elektron, kamera)
├── Materials/                       (yangi)
│   ├── Nucleus.mat
│   └── Electron.mat
└── Runtime/
    └── Bohr/
        └── ElectronOrbit.cs         (yangi: bizning birinchi skript)
```

## Dars qanday tuzilgan

| Qadam | Nima qilamiz | Natija |
|---|---|---|
| 1 | Atom nima; sahnaga yadro va elektron qo'yamiz | qora fonda ikkita shar |
| 2 | Birinchi skript: Unity bizning kodni qachon chaqiradi | Console'da har kadrda xabar |
| 3 | Elektronni doira bo'ylab aylantiramiz | elektron aylanadi |
| 4 | Sozlanadigan qiymatlar: radius va tezlik Inspector'da | Play paytida sozlash |
| 5 | Klaviatura: orbitani almashtiramiz | strelkalar bilan n = 1..6 |

Har bir qadamdan keyin **Play** ni bosib tekshirasiz. Kod Rider'da yoziladi, faylni saqlab Unity oynasiga
qaytganingizda Unity uni o'zi kompilyatsiya qiladi. Kod qanday ko'rsatilishi (yangi fayl to'liq, o'zgarish diff
bilan, har qadamdan keyin yashirin blokda to'liq fayl) 00-darsning boshida tushuntirilgan.

**Boshlanish nuqtasi**: 00-darsning oxiridagi holat. `AtomSimulation` sahnasida `Main Camera` (fon rangi `020204`)
va `Directional Light` bor.

## 1-qadam. Atom nima va sahnaga yadro bilan elektron qo'yamiz

### 1.1. Atom: hamma narsa nimadan tuzilgan

Qo'lingizdagi telefon, havo, suv, o'zingiz: hammasi **atom**lardan tuzilgan. Atom juda kichik. Eng kichigi, vodorod
atomining diametri taxminan 0.1 nanometr. Nanometr bu metrning milliarddan biri. Bir tola sochning qalinligi taxminan
0.07 millimetr, ya'ni 70 000 nanometr. Demak soch qalinligiga taxminan `70 000 / 0.1 = 700 000` ta vodorod atomi
qator bo'lib sig'adi.

Har bir atomning ikki qismi bor:

- O'rtada **yadro** (nucleus): og'ir va juda kichik.
- Atrofida **elektron**lar (electron): yengil.

Biz **vodorod** atomini simulyatsiya qilamiz, chunki u eng oddiysi: yadrosi bitta **proton**dan iborat, atrofida
bitta elektron. Koinotdagi atomlarning taxminan 90 foizi vodorod. Eng muhimi, vodorod uchun kvant mexanikasi
tenglamalarini aniq yechish mumkin. Kursning III qismida aynan shu yechimlarni chizamiz.

Ikki son bu rasmni to'ldiradi:

- Proton elektrondan **1836 marta og'ir**. Shuning uchun yadro deyarli qimirlamaydi, harakatni elektron qiladi.
  Bizning kodda ham faqat elektron harakatlanadi.
- Yadro atomdan **taxminan 60 000 marta kichik**. Agar atom katta stadion kattaligida bo'lsa, yadro uning o'rtasidagi
  qum donasidek bo'ladi. Atomning deyarli hamma joyi bo'shliq.

Shuning uchun **sahnamizdagi o'lchamlar haqiqiy emas**. Elektron orbitasi radiusi 4 birlik bo'lsa, haqiqiy yadro
`4 / 60 000 = 0.00007` birlik bo'lardi va hech qachon ko'rinmasdi. Biz yadroni ko'rinadigan qilib katta chizamiz.
Fizik chizmalarda bu odatiy hol, lekin buni doim yodda tuting.

### 1.2. Zaryad va tortishish

Proton va elektronda **elektr zaryadi** (charge) bor. Zaryad ikki xil bo'ladi: musbat (+) va manfiy (−). Proton
musbat, elektron manfiy. Qoida juda oddiy:

- har xil zaryadlar bir-birini **tortadi** (+ va −);
- bir xil zaryadlar bir-birini **itaradi** (+ va +, − va −).

Bu magnitning ikki qutbiga o'xshaydi: qarama-qarshi qutblar yopishadi, bir xillari qochadi. Zaryadlar qanchalik
yaqin bo'lsa, tortishish shunchalik kuchli. Aniq formulasini 03-darsda ko'ramiz.

Demak yadro elektronni o'ziga tortadi. Unda nega elektron yadroga yopishib qolmaydi?

### 1.3. Sayyoraviy model va uning muammosi

1911 yilda Rezerford atomning o'rtasida kichik og'ir yadro borligini tajribada topdi va shunday rasm taklif qildi:
elektron yadro atrofida Yer Quyosh atrofida aylangandek aylanadi. Bu **sayyoraviy model**.

Nega aylanayotgan narsa markazga tushmaydi? Ipga bog'langan toshni boshingiz atrofida aylantiryapsiz deb tasavvur
qiling:

- Tosh har lahzada **yon tomonga**, to'g'ri chiziq bo'ylab ketmoqchi. Ipni qo'yib yuborsangiz, aynan shunday qiladi:
  to'g'ri chiziq bo'ylab uchib ketadi.
- Ip esa uni doim **markazga** tortadi.
- Ikkalasi birga: tosh markazga tushmaydi ham, uchib ketmaydi ham, doira chizadi.

Atomda ip vazifasini zaryadlarning tortishishi bajaradi. Muhim xulosa: **doira bo'ylab harakat uchun markazga
yo'nalgan kuch kerak**. Kuch bo'lmasa, harakat to'g'ri chiziqli bo'ladi.

Bu darsda kuchni hisoblamaymiz: natijani, ya'ni doirani `sin` va `cos` bilan to'g'ridan-to'g'ri chizamiz. Kuch va
energiya 03-darsda keladi.

**Muammo.** O'sha davrning fizikasiga ko'ra, aylanayotgan zaryad doim yorug'lik chiqaradi va energiya yo'qotadi.
Energiyasi kamaygan elektron spiral bo'ylab yadroga tushishi kerak edi, hisob bo'yicha soniyaning o'n milliarddan
biridan ham tezroq. Unda hech bir atom mavjud bo'lmasdi. Atomlar esa milliardlab yil barqaror turibdi. Demak bu
modelda nimadir tubdan noto'g'ri. Shu jumboq 03-darsning mavzusi.

### 1.4. Koordinatalar: Unity'da uchta o'q

Sahnadagi har bir nuqtaning joyi uchta son bilan aytiladi. Xonaning bir burchagini boshlang'ich nuqta deb oling:
"o'ngga 3 metr, yuqoriga 1 metr, oldinga 2 metr" degan gap har qanday joyni aniq ko'rsatadi.

Unity'da:

| O'q | Yo'nalish | Scene oynasidagi rangi |
|---|---|---|
| `x` | o'ngga (manfiy bo'lsa chapga) | qizil |
| `y` | yuqoriga (manfiy bo'lsa pastga) | yashil |
| `z` | oldinga, ekran ichiga (manfiy bo'lsa o'zingizga qarab) | ko'k |

Markaz `(0, 0, 0)`. Birlik metr deb hisoblanadi, lekin bizning sahnada u shunchaki "birlik": haqiqiy atom
o'lchamlariga hech qanday aloqasi yo'q (1.1-bo'lim).

```
                y
                ▲
                │
                │        ● nuqta (3, 2, 0): o'ngga 3, yuqoriga 2
                │
   ─────────────●─────────────► x
             markaz (0, 0, 0)

   z o'qi ekran ichiga qarab ketadi.
   Kamera z = -30 da, ya'ni bizning tomonda turib, ekran ichiga (+z ga) qaraydi.
```

Atomimiz `z = 0` tekisligida, ya'ni `x` va `y` o'qlari hosil qilgan tekislikda yotadi. Kamera unga to'g'ridan-to'g'ri
qaraydi, shuning uchun biz uni xuddi qog'ozdagi rasmdek ko'ramiz. Bu 2D modelimiz uchun qulay. 09-darsda kamerani
aylantirib, 3D ga o'tamiz.

**Vektor.** Uchta son doim birga yuradi, shuning uchun ularni bitta narsa deb qaraymiz va `(x, y, z)` deb yozamiz.
Bunday guruhni **vektor** deyiladi. Uni markazdan shu nuqtagacha chizilgan strelka deb tasavvur qilish qulay. Unity'da
uning turi `Vector3` (3 ta son).

Vektor bilan bitta amal hozir kerak bo'ladi: **songa ko'paytirish**. Har bir sonni alohida ko'paytiramiz:

```
(0.5, 0.866, 0) * 4 = (0.5 * 4, 0.866 * 4, 0 * 4) = (2, 3.464, 0)
```

Strelka yo'nalishi o'zgarmaydi, uzunligi 4 baravar oshadi. 3-qadamda aynan shu amal elektronni kerakli masofaga
qo'yadi.

### 1.5. GameObject, Component va Transform

Unity sahnasidagi har bir narsa **GameObject**. GameObject'ning o'zi deyarli hech narsa qila olmaydi, u bo'sh
"quti". Unga **komponent**lar (component) qo'shiladi va har bir komponent bitta ish qiladi.

Masalan, shar (Sphere) aslida to'rtta komponentli GameObject:

| Komponent | Ishi |
|---|---|
| **Transform** | qayerda turibdi, qanday burilgan, qanchalik katta |
| **Mesh Filter** | shakli: qaysi uchburchaklardan iborat (bizda shar) |
| **Mesh Renderer** | shu shaklni qaysi material bilan chizish |
| **Sphere Collider** | fizika uchun: urilish va sichqoncha bosilishini sezish |

LEGO'ni tasavvur qiling: GameObject bu asos plastinka, komponentlar esa unga taqiladigan detallar. G'ildirak taqsangiz
yuradi, chiroq taqsangiz yonadi. Bizning skript ham shunday "detal" bo'ladi (2-qadam).

**Transform** har bir GameObject'da majburiy turadi va uchta qismdan iborat:

- **Position**: o'rni, `(x, y, z)`;
- **Rotation**: burilishi, gradusda;
- **Scale**: o'lchami. `(1, 1, 1)` asl o'lcham, `(2, 2, 2)` ikki baravar katta.

Unity'ning standart shari `Scale = (1, 1, 1)` da diametri 1 birlik.

Sphere Collider bizga kerak emas: atomda hech narsa urilmaydi. Ortiqcha komponent ortiqcha ish, shuning uchun uni olib
tashlaymiz. Bu ishlashga xalaqit bermaydi, faqat tartib uchun.

### 1.6. Ota va bola: local va world koordinatalar

Hierarchy'da bir GameObject'ni boshqasining ustiga sudrasangiz, u **bola** (child) bo'ladi, ikkinchisi esa **ota**
(parent). Bolaning Transform'i otasiga nisbatan yoziladi.

Avtobusdagi yo'lovchini tasavvur qiling. "Haydovchidan 3 metr orqada o'tiribdi" bu yo'lovchining avtobusga nisbatan
joyi (**local** koordinata). Avtobus shahar bo'ylab yursa ham, bu gap to'g'ri bo'lib qolaveradi. "Toshkentning qaysi
ko'chasida" esa uning dunyodagi joyi (**world** koordinata), u avtobus bilan birga o'zgaradi.

Bizda `Atom` ota bo'ladi, `Nucleus` va `Electron` uning bolalari. Elektron joyini atomga nisbatan beramiz
(`localPosition`). Shunda `Atom`ni sahnaning boshqa joyiga sursangiz, yadro ham, elektron orbitasi ham u bilan birga
ko'chadi. Inspector'dagi Transform **local** qiymatlarni ko'rsatadi.

### 1.7. Material

Mesh Renderer shaklni chizadi, lekin qanday rangda va qanday yorug'lik bilan? Buni **material** aytadi. Material
ikki narsadan iborat: **shader** (sirt qanday hisoblanishi haqidagi GPU dasturi) va shu shader'ning sozlamalari
(rang, yaltiroqlik va hokazo).

URP'da standart shader `Universal Render Pipeline/Lit`. "Lit" yoritilgan degani: sirtga yorug'lik tushgan tomoni
yorug', narigi tomoni qorong'i bo'ladi. Shuning uchun sahnada `Directional Light` turibdi. 02-darsda o'z shader'imizni
yozamiz va yadro bilan elektronni o'zi porlaydigan qilamiz.

### 1.8. Amal: atom

**Atom (ota).**

1. Hierarchy'ning bo'sh joyida o'ng tugma → **Create Empty**. Nomini `Atom` qiling (F2 yoki ikki marta sekin
   bosish).
2. Inspector'da Transform'ni tekshiring: Position `(0, 0, 0)`, Rotation `(0, 0, 0)`, Scale `(1, 1, 1)` bo'lishi
   kerak. Unity yangi obyektni Scene oynasining o'rtasiga qo'yadi, shuning uchun Position boshqa son bo'lishi mumkin.
   Shunda Transform sarlavhasidagi **⋮** menyusidan **Reset** ni tanlang.

**Yadro.**

3. Hierarchy'da `Atom` ustida o'ng tugma → **3D Object → Sphere**. Shar `Atom`ning bolasi bo'lib yaratiladi:
   Hierarchy'da u `Atom` ostida, biroz o'ngga surilgan holda turadi. Nomi `Nucleus`.
4. Transform: Position `(0, 0, 0)`, Scale `(2, 2, 2)`.
5. **Sphere Collider** komponentining **⋮** menyusidan **Remove Component**.

**Elektron.**

6. Yana `Atom` ustida o'ng tugma → **3D Object → Sphere**. Nomi `Electron`.
7. Transform: Position `(4, 0, 0)`, Scale `(1, 1, 1)`.
8. Sphere Collider'ni olib tashlang.

Hierarchy shunday ko'rinishi kerak:

```
AtomSimulation
├── Main Camera
├── Directional Light
└── Atom
    ├── Nucleus
    └── Electron
```

**Materiallar.**

9. Project oynasida `Assets/AtomSim` ichida **Create → Folder**, nomi `Materials`.
10. `Materials` ichida o'ng tugma → **Create → Material**, nomi `Nucleus`. Inspector'da Shader
    `Universal Render Pipeline/Lit` ekanini tekshiring.
11. **Surface Inputs → Base Map** yonidagi rang qutisini bosing, Hexadecimal `FF7040` (to'q sariq).
12. Xuddi shunday ikkinchi material: nomi `Electron`, rang `40A0FF` (havorang).
13. `Nucleus` materialini Project oynasidan sudrab, Hierarchy'dagi `Nucleus` obyektiga tashlang. `Electron`
    materialini esa `Electron` obyektiga.

**Kamera.**

14. `Main Camera` ni tanlang. Transform: Position `(0, 0, -30)`, Rotation `(0, 0, 0)`. Basic (URP) shablonida kamera
    `(0, 1, -10)` da turardi. Endi u markazdan 30 birlik orqada, aniq `z` o'qi bo'ylab atomga qaraydi.

15. **Ctrl+S**.

### 1.9. Ishga tushirib ko'ring

- **Scene** oynasida Hierarchy'dan `Atom`ni tanlab, sichqoncha Scene ustida turganda **F** ni bosing: atom
  ko'rinadigan qilib yaqinlashtiriladi. Scene oynasi tepasidagi **2D** tugmasini bossangiz, sahnaga xuddi
  kameradagidek `z` o'qi bo'ylab qaraysiz. Qaytadan bosib, 3D ko'rinishga qayting.
- **Play**. Game oynasida qora fonda katta to'q sariq shar (yadro) va uning o'ng tomonida kichikroq havorang shar
  (elektron) ko'rinadi. Ikkalasining yuqori o'ng tomoni yorug'roq: `Directional Light` (Rotation `50, -30, 0`)
  nurni o'ngdan, yuqoridan va kamera tomondan tushiradi.
- Hozircha hech narsa qimirlamaydi. Play'dan chiqing.

Tajriba: Hierarchy'da `Atom`ni tanlab, Position `x` ni `10` qiling. Yadro ham, elektron ham birga suriladi, chunki
ular `Atom`ning bolalari. Ularning o'z Position qiymatlari esa o'zgarmadi: `(0, 0, 0)` va `(4, 0, 0)`. Bu local
koordinata. Sinab bo'lgach, `x` ni `0` ga qaytaring.

## 2-qadam. Birinchi skript

### 2.1. Skript bu komponent

Unity'da o'z komponentingizni yozish mumkin. Buning uchun C# tilida **class** yozasiz va uni `MonoBehaviour` dan
**meros** qilib olasiz:

```csharp
public sealed class ElectronOrbit : MonoBehaviour
```

**Class** bu "qolip" (chizma): unda qanday ma'lumotlar (o'zgaruvchilar) va qanday amallar (funksiyalar) borligi
yoziladi. Unity sahnadagi har bir `ElectronOrbit` komponenti uchun shu qolipdan bitta nusxa (**obyekt**) yasaydi.

`: MonoBehaviour` degani "bu class Unity'ning `MonoBehaviour` classi bilan bir xil imkoniyatlarga ega, ustiga
yana o'zimizniki". Aynan shu qism classni komponentga aylantiradi: uni GameObject'ga taqish mumkin bo'ladi, Unity esa
uni o'zi chaqiradi. Meros (inheritance) mavzusini shu yerda to'xtatamiz, bizga shuning o'zi kerak.

`sealed` "bu classdan boshqa class meros ololmaydi" degani. Bizga meros kerak emas va `sealed` buni ochiq aytadi.
Yonidagi tayyor loyihada hamma komponentlar shunday yozilgan.

**Qoida: fayl nomi va class nomi bir xil bo'ladi.** `ElectronOrbit.cs` faylidagi class `ElectronOrbit` deb ataladi.
Eski Unity versiyalarida bu majburiy edi: nomlar farq qilsa, Unity skriptni komponent sifatida qo'sha olmasdi. Unity
6.6 da sinab ko'rdim: nomlar farq qilganda ham komponent qo'shildi va sahnada saqlandi. Baribir bir xil yozamiz:
faylni class nomidan topish oson, internetdagi deyarli hamma qo'llanma va eski Unity versiyalari shunga tayanadi.

### 2.2. Unity bizning kodni qachon chaqiradi

Biz `main` funksiyasini yozmaymiz. Dasturning asosiy sikli Unity'ning o'zida, OpenGL kursidagi kabi
`while (!glfwWindowShouldClose(...))` siklini Unity bizning o'rnimizga aylantiradi. Biz faqat ma'lum nomli
funksiyalar yozamiz, Unity esa ularni kerakli paytda chaqiradi:

| Funksiya | Qachon chaqiriladi |
|---|---|
| `Awake` | obyekt yaratilganda, birinchi bo'lib |
| `Start` | birinchi `Update`dan oldin, bir marta |
| `Update` | **har kadrda**, bir marta |
| `LateUpdate` | har kadrda, hamma `Update`lardan keyin |
| `OnDestroy` | obyekt yo'q qilinganda |

Bu darsda ikkitasi kerak: `Start` va `Update`. Unity bu funksiyalarni **nomi bo'yicha** topadi, shuning uchun nom
harfma-harf to'g'ri bo'lishi shart. `update` (kichik harf bilan) deb yozsangiz, xato ham, ogohlantirish ham
chiqmaydi, funksiya esa hech qachon chaqirilmaydi. Buni sinab ko'rdim: elektron joyidan qimirlamadi. Rider bu yerda
yordam beradi: to'g'ri yozilgan Unity funksiyalari yonida Unity belgisi turadi.

`public` yozish shart emas: Unity funksiyani `private` bo'lsa ham topadi. C# da `public` yozilmagan narsa avtomatik
`private`, ya'ni faqat shu class ichida ko'rinadi.

### 2.3. Kadr va `Time.deltaTime`

**Kadr** (frame) ekranga chizilgan bitta rasm. Multfilm alohida rasmlardan iborat bo'lgani kabi, Unity ham soniyasiga
o'nlab yoki yuzlab rasm chizadi. **FPS** (frames per second) soniyada nechta kadr chizilishini aytadi.

Kadrlar orasidagi vaqt har doim bir xil emas: kompyuter band bo'lsa, kadr uzoqroq chiziladi. Bundan tashqari har bir
kompyuterning tezligi har xil. Shuning uchun Unity har kadrda **`Time.deltaTime`** ni beradi: "oldingi kadrdan beri
necha soniya o'tdi". 165 Hz monitorda bu taxminan `1 / 165 = 0.00606` soniya, 60 Hz da `1 / 60 = 0.01667`.

3-qadamda ko'ramiz: harakatni hisoblaganda `deltaTime` hal qiluvchi rol o'ynaydi.

**`Debug.Log`** Console oynasiga xabar yozadi. Bu Unity'dagi eng oddiy va eng ko'p ishlatiladigan tekshiruv usuli.

### 2.4. Amal: skript yaratish

1. Project oynasida `Assets/AtomSim` ichida `Runtime` papkasini, uning ichida `Bohr` papkasini yarating.
   `Runtime` "Play paytida ishlaydigan kod" degani (03-darsda testlar uchun alohida papka paydo bo'ladi). `Bohr` esa
   Bor modeli kodlari uchun (03-dars).
2. `Bohr` ustida o'ng tugma → **Create → Scripting → MonoBehaviour Script**.
3. **Darhol**, Enter bosishdan oldin, nomini `ElectronOrbit` deb yozing. Unity faylning ichidagi class nomini
   aynan shu paytda fayl nomidan oladi. Avval `NewMonoBehaviourScript` deb yaratib, keyin faylni qayta nomlasangiz,
   ichidagi class nomi eskicha (`NewMonoBehaviourScript`) qoladi. Biz baribir hamma matnni almashtiramiz, lekin buni
   bilib qo'ying.
4. Faylni ikki marta bosing: Rider'da ochiladi. Unity yozgan namunaviy matnni to'liq o'chirib, quyidagini yozing.

**`Assets/AtomSim/Runtime/Bohr/ElectronOrbit.cs`** (yangi fayl):

```csharp
using UnityEngine;

namespace AtomSim.Bohr
{
    public sealed class ElectronOrbit : MonoBehaviour
    {
        void Start()
        {
            Debug.Log("ElectronOrbit ishga tushdi");
        }

        void Update()
        {
            Debug.Log($"Kadr {Time.frameCount}: dt = {Time.deltaTime} s");
        }
    }
}
```

Qismma-qism:

**`using UnityEngine;`** Unity'ning asosiy turlarini (`MonoBehaviour`, `Debug`, `Time`, `Vector3`) to'liq nomisiz
ishlatishga ruxsat beradi. Usiz `UnityEngine.Debug.Log(...)` deb yozishga to'g'ri kelardi.

**`namespace AtomSim.Bohr`** classni "familiya" bilan ataydi: uning to'liq nomi `AtomSim.Bohr.ElectronOrbit`.
Katta loyihada yoki tashqi paketlarda ham `ElectronOrbit` degan class bo'lishi mumkin. Namespace ular to'qnashmasligini
ta'minlaydi. Nomlar papkalarga mos: `AtomSim` loyiha, `Bohr` papka.

**`void Start()`**: `void` "funksiya hech narsa qaytarmaydi" degani. Ichidagi kod bir marta bajariladi.

**`$"Kadr {Time.frameCount}: ..."`**: satr oldidagi `$` belgisi qavs `{ }` ichidagi ifodalarni hisoblab, natijasini
matnga qo'yadi. `Time.frameCount` Play boshlanganidan beri chizilgan kadrlar soni.

**Skriptni GameObject'ga taqing.** Faylni saqlang (Ctrl+S) va Unity oynasiga qayting. Unity kompilyatsiya qiladi
(pastki o'ng burchakda aylanayotgan belgi). Keyin:

5. Hierarchy'da `Atom`ni tanlang.
6. Inspector'ning eng pastidagi **Add Component** → qidiruvga `Electron Orbit` deb yozing → tanlang.
   (Yoki `ElectronOrbit.cs` faylini Project oynasidan sudrab, `Atom`ga tashlang.)

Inspector'da `Atom`ning Transform'idan pastda yangi **Electron Orbit (Script)** komponenti paydo bo'ladi.

### 2.5. Ishga tushirib ko'ring

**Play** ni bosing va Console'ga qarang:

- Birinchi qatorda bir marta: `ElectronOrbit ishga tushdi`.
- Keyin har kadrda bittadan: `Kadr 1: dt = 0.01999999 s`, `Kadr 2: dt = 0.01999999 s`, `Kadr 3: ...` va hokazo.
  Xabarlar shunchalik tez yog'iladiki, Console to'xtovsiz pastga suriladi.

Ko'rganingizni o'qish uchun **Pause** ni bosing (yoki Play'dan chiqing) va Console'ni yuqoriga suring.

Uchta narsaga e'tibor bering:

1. **Birinchi ikki kadrda `dt` doim `0.02`.** Bu haqiqiy o'lchov emas: Unity'da hali "oldingi kadr" yo'q, shuning uchun
   u boshlang'ich qiymat sifatida 0.02 soniyani beradi. Uchinchi kadrdan boshlab haqiqiy vaqt keladi.
2. **`dt` kadrdan kadrga biroz farq qiladi.** Kompyuter hech qachon aynan bir xil tezlikda ishlamaydi.
3. **`dt` kompyuteringizga bog'liq.** Editor odatda monitor chastotasiga bog'lanmagan, shuning uchun qiymat 0.002 dan
   0.02 gacha bo'lishi mumkin. Mening sinovimda (oynasiz, eng tez rejimda) kadr 0.0002–0.0005 soniya edi.

Play'dan chiqing. Har kadrda `Debug.Log` yozish sekin ish: keyingi qadamda uni olib tashlaymiz.

<details>
<summary>Shu qadamdan keyingi to'liq <code>ElectronOrbit.cs</code> (solishtirish uchun)</summary>

```csharp
using UnityEngine;

namespace AtomSim.Bohr
{
    public sealed class ElectronOrbit : MonoBehaviour
    {
        void Start()
        {
            Debug.Log("ElectronOrbit ishga tushdi");
        }

        void Update()
        {
            Debug.Log($"Kadr {Time.frameCount}: dt = {Time.deltaTime} s");
        }
    }
}
```

</details>

## 3-qadam. Elektronni aylantiramiz

Maqsad: har kadrda elektronni doira ustidagi keyingi nuqtaga qo'yish. Buning uchun uchta tushuncha kerak: burchak,
`sin`/`cos` va burchak tezligi.

### 3.1. Burchak: gradus va radian

Soat millarini tasavvur qiling. Mil to'liq aylanib, joyiga qaytsa, u **to'liq aylana** bosib o'tgan bo'ladi. Uni
360 bo'lakka bo'lish odat bo'lib qolgan, har bir bo'lak **1 gradus** (1°). Chorak aylana 90°, yarim aylana 180°.

Kompyuter esa burchakni boshqa o'lchovda, **radian**da o'lchaydi. Uni tushunish uchun avval bitta sonni ko'raylik.

**π (pi) soni.** Istalgan dumaloq narsani oling: stakan, tarelka, g'ildirak. Uning atrofini ip bilan o'lchang
(aylana uzunligi), keyin bir chetidan ikkinchi chetigacha to'g'ri chiziq bilan o'lchang (diametr). Aylana uzunligini
diametrga bo'lsangiz, narsaning kattaligidan qat'i nazar, doim bir xil son chiqadi: **3.1416**. Bu son **π** (pi)
deb ataladi. Demak:

```
aylana uzunligi = π × diametr = 2 × π × radius
```

Radius bu markazdan aylanagacha masofa, diametrning yarmi. Radiusi 4 bo'lgan aylananing uzunligi
`2 × 3.1416 × 4 = 25.13` birlik.

**Radian.** Radius uzunligidagi ipni olib, aylana ustiga yotqizing. Ipning ikki uchidan markazga chiziq torting. Shu
ikki chiziq orasidagi burchak **1 radian**. Butun aylanaga bunday iplardan nechta sig'adi? Aylana uzunligi `2 × π ×
radius`, demak `2 × π = 6.2832` ta. Shuning uchun:

```
to'liq aylana = 360° = 6.2832 radian
1 radian = 360 / 6.2832 = 57.3°
```

| Gradus | Radian |
|---|---|
| 0° | 0 |
| 30° | 0.5236 |
| 45° | 0.7854 |
| 90° | 1.5708 (π / 2) |
| 180° | 3.1416 (π) |
| 270° | 4.7124 |
| 360° | 6.2832 (2π) |

Gradusdan radianga o'tish: `radian = gradus × π / 180`. Unity'da buning uchun tayyor son bor: `Mathf.Deg2Rad`
(`0.01745`).

**Muhim:** Unity'ning `Mathf.Cos` va `Mathf.Sin` funksiyalari burchakni **faqat radianda** qabul qiladi.
`Mathf.Cos(90)` 90 gradusning emas, 90 radianning kosinusini beradi: `-0.448`. 90° ning kosinusi esa `0`. Bu xatoni
kompilyator ham, Unity ham sezmaydi, faqat natija noto'g'ri bo'ladi.

Radian nega qulay ekanini 4-qadamda ko'ramiz.

### 3.2. `sin` va `cos`: burchakdan nuqtaga

Radiusi 1 bo'lgan doirani tasavvur qiling, markazi `(0, 0)` da. Markazdan o'ngga qarab strelka chizing, keyin uni
soat miliga **teskari** yo'nalishda biror burchakka buring. Strelkaning uchi doira ustidagi biror nuqtaga tushadi.
Shu nuqtaning koordinatalari maxsus nomga ega:

```
x = cos(burchak)
y = sin(burchak)
```

```
                 90°: (0, 1)
                  .-'''-.
                /    ●    \       30°: (0.866, 0.5)
              /       ↗     \
  180°: (-1, 0) ●     ●──────● 0°: (1, 0)
              \    markaz   /
                \         /
                  '-...-'
                270°: (0, -1)
```

`cos` (kosinus) "strelka uchi qancha o'ngda", `sin` (sinus) "qancha yuqorida" degani. Ikkalasi ham −1 dan +1 gacha
o'zgaradi. Bizga hozircha shu ma'nosi yetarli: **`cos` va `sin` burchakni doira ustidagi nuqtaga aylantiradi.**

Radiusi 4 bo'lgan doira kerak bo'lsa, natijani 4 ga ko'paytiramiz (1.4-bo'limdagi vektorni songa ko'paytirish):

| Burchak | Radian | `cos` | `sin` | Nuqta × 4 |
|---|---|---|---|---|
| 0° | 0 | 1 | 0 | (4, 0) |
| 30° | 0.5236 | 0.866 | 0.5 | (3.464, 2) |
| 45° | 0.7854 | 0.707 | 0.707 | (2.828, 2.828) |
| 60° | 1.0472 | 0.5 | 0.866 | (2, 3.464) |
| 90° | 1.5708 | 0 | 1 | (0, 4) |
| 180° | 3.1416 | −1 | 0 | (−4, 0) |
| 270° | 4.7124 | 0 | −1 | (0, −4) |

Burchak 0 dan 6.2832 gacha o'sib borsa, nuqta doirani soat miliga teskari yo'nalishda to'liq bir marta aylanib
chiqadi. 6.2832 dan keyin hammasi qaytadan takrorlanadi: 6.2832 + 0.5236 burchakdagi nuqta 0.5236 dagisi bilan bir
xil.

### 3.3. Pifagor: nega bu aniq doira

Jadvaldagi nuqtalar haqiqatan markazdan 4 birlik uzoqdami? Buni tekshirishning eng qadimiy usuli bor.

**To'g'ri burchakli uchburchak** bu bir burchagi aniq 90° bo'lgan uchburchak, xuddi kitobning burchagi kabi. Uning
90° burchak yonidagi ikki tomoni **katet**lar (a va b), qarshisidagi eng uzun tomoni **gipotenuza** (c). Pifagor
teoremasi:

```
a × a + b × b = c × c
```

Misol: katetlari 3 va 4. `3 × 3 + 4 × 4 = 9 + 16 = 25`, demak `c × c = 25` va `c = 5` (chunki `5 × 5 = 25`).
Sonning o'ziga ko'paytmasi **kvadrat** deyiladi, kvadratdan sonni qaytarib topish esa **kvadrat ildiz** (`sqrt`):
`sqrt(25) = 5`.

Markazdan `(x, y)` nuqtagacha chiziq torting, keyin nuqtadan `x` o'qiga tik tushiring. To'g'ri burchakli uchburchak
hosil bo'ladi: katetlari `x` va `y`, gipotenuzasi esa aynan markazgacha masofa:

```
masofa = sqrt(x × x + y × y)
```

Jadvalni tekshiramiz:

```
(3.464, 2):      3.464 × 3.464 + 2 × 2         = 12 + 4  = 16,  sqrt(16) = 4
(2.828, 2.828):  2.828 × 2.828 + 2.828 × 2.828 = 8 + 8   = 16,  sqrt(16) = 4
```

Har doim 4 chiqadi, chunki istalgan burchak uchun `cos × cos + sin × sin = 1` (radiusi 1 doiradagi nuqtaning
masofasi 1). Ya'ni `sin` va `cos` bilan yasalgan har bir nuqta aniq doira ustida yotadi. Play'da elektronning
markazdan masofasini har xil paytda bir necha marta o'lchadim: har safar `4.0000`.

### 3.4. Burchak tezligi va aylanish davri

Elektronni aylantirish uchun burchakni har kadrda biroz oshirib borish kerak. Qanchaga?

**Burchak tezligi** "bir soniyada burchak qanchaga o'zgaradi" degani, radian/soniyada o'lchanadi. Biz **2.4**
radian/soniya olamiz (yonidagi tayyor loyihaning eng ichki orbitasidagi qiymat).

Bitta to'liq aylanish qancha vaqt oladi? To'liq aylana 6.2832 radian, har soniyada 2.4 radian:

```
aylanish davri = 6.2832 / 2.4 = 2.618 soniya
```

Demak 10 marta aylanish 26.2 soniya oladi. Bu son keyin tekshirish uchun kerak.

Har kadrda burchakka nima qo'shamiz? "Tezlik × vaqt" (yo'l kabi: soatiga 60 km tezlik bilan 2 soat yursangiz,
`60 × 2 = 120` km). Bitta kadrning vaqti `Time.deltaTime`:

```csharp
m_Angle += 2.4f * Time.deltaTime;
```

`a += b` "a ga b ni qo'shib, natijani a da qoldir" degani. `2.4f` dagi `f` harfi "bu `float` son" degani (`float`
o'nli kasrli son turi, Unity asosan shuni ishlatadi).

Bitta haqiqiy kadrni qo'lda hisoblaymiz. Burchak hozir 1.0 radian, monitor 165 Hz, `dt = 0.00606`:

```
yangi burchak = 1.0 + 2.4 × 0.00606              = 1.01454
x = cos(1.01454) × 4 = 0.5280 × 4                = 2.112
y = sin(1.01454) × 4 = 0.8492 × 4                = 3.397
tekshiruv: sqrt(2.112 × 2.112 + 3.397 × 3.397)   = 4.000
```

**Nega `deltaTime` shart.** Uni yozmasak, burchak har **kadrda** 2.4 radianga oshadi. Unda tezlik kompyuterga bog'liq
bo'lib qoladi:

| FPS | `deltaTime` bilan: kadrda | 1 soniyada | `deltaTime`siz: 1 soniyada |
|---|---|---|---|
| 60 | 0.0400 rad | 2.4 rad | 144 rad (soniyasiga 23 aylanish) |
| 165 | 0.0146 rad | 2.4 rad | 396 rad (soniyasiga 63 aylanish) |
| 500 | 0.0048 rad | 2.4 rad | 1200 rad (soniyasiga 191 aylanish) |

`deltaTime` bilan har qanday kompyuterda soniyasiga aynan 2.4 radian. Usiz esa elektron har kadrda 137.5° ga sakraydi:
ko'z buni aylanish deb emas, doira ustida tartibsiz miltillayotgan nuqtalar deb ko'radi.

### 3.5. Inspector'dan havola: `[SerializeField] Transform`

Skript `Atom`da turibdi, harakatlantirishimiz kerak bo'lgan narsa esa `Electron`. Skript elektronni qayerdan biladi?

Classga maydon qo'shamiz:

```csharp
[SerializeField] Transform m_Electron;
```

- **`Transform m_Electron`**: "bu yerda bitta Transform'ga havola saqlanadi". Havola (reference) bu obyektning o'zi
  emas, uning manzili: "elektron mana u yerda". Telefon kitobidagi yozuv kabi: odamning o'zi emas, unga qanday
  bog'lanish.
- **`[SerializeField]`**: Unity'ga "bu maydonni Inspector'da ko'rsat va sahna bilan birga saqla" deydi. Maydon
  `private` bo'lib qoladi, ya'ni boshqa skriptlar uni o'zgartira olmaydi, lekin Inspector'da sozlanadi. Bunday
  maydonni **serialized** maydon deyiladi: Unity uning qiymatini sahna fayliga yozib qo'yadi.
- **`m_`** old qo'shimchasi: "class a'zosi" (member). Unity'ning o'z kodida shunday yoziladi, yonidagi tayyor loyihada
  ham. Inspector esa `m_`ni tashlab, nomni chiroyli ko'rsatadi: `m_Electron` → **Electron**.

Havolani Inspector'da qo'lda ulaymiz: `Electron` obyektini Hierarchy'dan sudrab, Electron Orbit komponentidagi
**Electron** maydoniga tashlaysiz.

**Burchak** esa Inspector'ga kerak emas, u faqat kod ichida o'zgaradi. Shuning uchun `[SerializeField]`siz:

```csharp
float m_Angle;
```

C# da sonli maydonning boshlang'ich qiymati 0. Demak elektron 0° dan, ya'ni `(4, 0, 0)` nuqtadan boshlaydi.

**Joyni o'rnatish.** 3.2-bo'limdagi formula kodda:

```csharp
m_Electron.localPosition = new Vector3(Mathf.Cos(m_Angle), Mathf.Sin(m_Angle), 0f) * 4f;
```

`new Vector3(x, y, z)` uchta sondan vektor yasaydi: radiusi 1 doiradagi nuqta, `z = 0`. `* 4f` uni radiusi 4 doiraga
ko'chiradi. `localPosition` esa `Atom`ga nisbatan joy (1.6-bo'lim).

### 3.6. Kod

`ElectronOrbit.cs` da:

```diff
 {
     public sealed class ElectronOrbit : MonoBehaviour
     {
+        [SerializeField] Transform m_Electron;
+
+        float m_Angle;
+
         void Start()
         {
-            Debug.Log("ElectronOrbit ishga tushdi");
+            Debug.Log($"ElectronOrbit ishga tushdi. Elektron: {m_Electron.name}");
         }
 
         void Update()
         {
-            Debug.Log($"Kadr {Time.frameCount}: dt = {Time.deltaTime} s");
+            m_Angle += 2.4f * Time.deltaTime;
+            m_Electron.localPosition = new Vector3(Mathf.Cos(m_Angle), Mathf.Sin(m_Angle), 0f) * 4f;
         }
     }
 }
```

`Start` endi elektronning nomini yozadi (`m_Electron.name`). Bu kichik tekshiruv: havola ulanmagan bo'lsa, xato aynan
shu qatorda, Play boshlanishi bilanoq chiqadi.

Saqlang va Unity'ga qayting. Keyin:

1. `Atom`ni tanlang. Electron Orbit komponentida yangi **Electron** maydoni paydo bo'ldi, qiymati `None (Transform)`.
2. Hierarchy'dagi `Electron`ni sudrab, shu maydonga tashlang. Maydonda `Electron (Transform)` yoziladi.
3. **Ctrl+S** (havola sahnada saqlanadi).

### 3.7. Ishga tushirib ko'ring

- **Play**. Elektron yadro atrofida soat miliga teskari yo'nalishda aylanadi.
- Console'da bitta qator: `ElectronOrbit ishga tushdi. Elektron: Electron`.
- Telefondagi sekundomer bilan 10 ta aylanishni sanang: taxminan 26 soniya chiqishi kerak (3.4-bo'lim). Men burchakni
  Play'ning 12, 13 va 13.4-soniyalarida o'lchadim: har safar `2.4 × vaqt` dan 0.047 radian oldinda edi. Bu
  2.5-bo'limdagi birinchi ikki kadrning `0.02` lik `dt` si: ular burchakka `2 × 2.4 × 0.02 = 0.096` qo'shdi, vaqt
  esa shu ikki kadrda faqat `0.02` ga o'sdi, farq `0.096 − 0.048 = 0.048`.
- Play paytida Hierarchy'da `Electron`ni tanlang: Inspector'da uning Position qiymati har kadrda o'zgarib turadi,
  lekin `x × x + y × y` doim 16 atrofida.

<details>
<summary>Shu qadamdan keyingi to'liq <code>ElectronOrbit.cs</code> (solishtirish uchun)</summary>

```csharp
using UnityEngine;

namespace AtomSim.Bohr
{
    public sealed class ElectronOrbit : MonoBehaviour
    {
        [SerializeField] Transform m_Electron;

        float m_Angle;

        void Start()
        {
            Debug.Log($"ElectronOrbit ishga tushdi. Elektron: {m_Electron.name}");
        }

        void Update()
        {
            m_Angle += 2.4f * Time.deltaTime;
            m_Electron.localPosition = new Vector3(Mathf.Cos(m_Angle), Mathf.Sin(m_Angle), 0f) * 4f;
        }
    }
}
```

</details>

## 4-qadam. Sozlanadigan qiymatlar

### 4.1. Sonlarni Inspector'ga chiqaramiz

Kodda ikkita "sehrli son" bor: `4f` (radius) va `2.4f` (burchak tezligi). Ularni o'zgartirish uchun har safar kodni
tahrirlash, saqlash va Unity kompilyatsiyasini kutish kerak. Ularni ham `[SerializeField]` maydonga aylantirsak,
Inspector'da Play paytida ham o'zgartirib, natijani darhol ko'rish mumkin bo'ladi:

```csharp
[SerializeField] float m_Radius = 4f;
[SerializeField] float m_AngularSpeed = 2.4f;
```

`= 4f` bu **boshlang'ich qiymat**. U faqat komponent birinchi marta qo'shilganda (yoki ⋮ → **Reset** qilinganda)
ishlatiladi. Shundan keyin qiymat sahna faylida saqlanadi va Inspector'dagi son asosiy bo'ladi. Ya'ni keyinchalik
koddagi `4f` ni `5f` ga o'zgartirsangiz, sahnadagi komponent baribir 4 bilan qoladi. Yangi boshlaganlar buni ko'p
uchratadi: "kodni o'zgartirdim, hech narsa o'zgarmadi".

Hozirgi holatda bu bizga xalaqit bermaydi: `Atom`dagi komponentda bu maydonlar hali yo'q edi, shuning uchun Unity
ularga koddagi boshlang'ich qiymatlarni beradi.

### 4.2. Chiziqli tezlik: radian nega qulay

Elektron doira bo'ylab soniyasiga qancha **masofa** bosib o'tadi? Bunga **chiziqli tezlik** deyiladi.

3.1-bo'limdagi radian ta'rifini eslang: 1 radian burchakka radius uzunligidagi yoy to'g'ri keladi. Demak 2 radianga
ikki radius, 2.4 radianga 2.4 radius. Umuman olganda:

```
yoy uzunligi = burchak (radianda) × radius
chiziqli tezlik = burchak tezligi × radius
```

Gradusda bunday oddiy formula yo'q, `π / 180` ga ko'paytirish kerak bo'lardi. Radianning qulayligi shunda.

| Radius | Burchak tezligi | Chiziqli tezlik |
|---|---|---|
| 4 | 2.4 | 9.6 birlik/soniya |
| 8 | 2.4 | 19.2 |
| 24 | 2.4 | 57.6 |

Burchak tezligi bir xil bo'lsa, katta doiradagi elektron tezroq yuradi: aylanish uchun bir xil vaqt bor, yo'l esa
uzunroq. Karuselga o'xshaydi: chetidagi ot markazdagisidan tezroq yuguradi. 03-darsda ko'ramiz: haqiqiy atomda esa
aksincha, tashqi orbitadagi elektron sekinroq.

### 4.3. Kod

```diff
     public sealed class ElectronOrbit : MonoBehaviour
     {
         [SerializeField] Transform m_Electron;
+        [SerializeField] float m_Radius = 4f;
+        [SerializeField] float m_AngularSpeed = 2.4f;
 
         float m_Angle;
 
```

```diff
         void Update()
         {
-            m_Angle += 2.4f * Time.deltaTime;
-            m_Electron.localPosition = new Vector3(Mathf.Cos(m_Angle), Mathf.Sin(m_Angle), 0f) * 4f;
+            m_Angle += m_AngularSpeed * Time.deltaTime;
+            m_Electron.localPosition = new Vector3(Mathf.Cos(m_Angle), Mathf.Sin(m_Angle), 0f) * m_Radius;
         }
```

### 4.4. Ishga tushirib ko'ring

- `Atom`ni tanlang: Electron Orbit komponentida uchta maydon: **Electron**, **Radius** (4), **Angular Speed** (2.4).
  Unity `m_AngularSpeed` ni bo'lib, **Angular Speed** qilib ko'rsatadi.
- **Play**. Elektron avvalgidek aylanadi.
- Play paytida **Radius** ni `8` qiling: elektron darhol kattaroq doiraga o'tadi va tezroq yugurayotgandek ko'rinadi
  (4.2-bo'lim: chiziqli tezlik ikki baravar).
- **Angular Speed** ni `-2.4` qiling: soat mili bo'yicha aylanadi (burchak kamayib boradi). `0` qilsangiz, to'xtaydi.
- Play'dan chiqing: Radius va Angular Speed yana `4` va `2.4`. Play'dagi o'zgarishlar saqlanmaydi (00-dars, 3.2).

<details>
<summary>Shu qadamdan keyingi to'liq <code>ElectronOrbit.cs</code> (solishtirish uchun)</summary>

```csharp
using UnityEngine;

namespace AtomSim.Bohr
{
    public sealed class ElectronOrbit : MonoBehaviour
    {
        [SerializeField] Transform m_Electron;
        [SerializeField] float m_Radius = 4f;
        [SerializeField] float m_AngularSpeed = 2.4f;

        float m_Angle;

        void Start()
        {
            Debug.Log($"ElectronOrbit ishga tushdi. Elektron: {m_Electron.name}");
        }

        void Update()
        {
            m_Angle += m_AngularSpeed * Time.deltaTime;
            m_Electron.localPosition = new Vector3(Mathf.Cos(m_Angle), Mathf.Sin(m_Angle), 0f) * m_Radius;
        }
    }
}
```

</details>

## 5-qadam. Klaviatura: orbitani almashtiramiz

Endi elektron bitta emas, bir nechta orbitada bo'la oladi. Orbitalarni 1, 2, 3, ... deb raqamlaymiz, raqamni **`n`**
harfi bilan belgilaymiz (fizikada ham aynan shu harf ishlatiladi, 03-darsda ma'nosi chuqurlashadi). Yuqori strelka
`n` ni oshiradi, pastki strelka kamaytiradi.

### 5.1. Orbitalar orasidagi masofa

Radiusni endi `n` dan hisoblaymiz: `radius = n × 4`. Orbitalar bir-biridan bir xil masofada (4 birlik) turadi:

| `n` | radius |
|---|---|
| 1 | 4 |
| 2 | 8 |
| 3 | 12 |
| 6 | 24 |

Haqiqiy atomda bunday emas: 03-darsda ko'ramiz, orbitalar `n × n` ga proporsional, ya'ni 1, 4, 9, 16, 25, 36.
6-orbita 1-sidan 36 marta katta bo'lardi va ekranga sig'masdi. Tayyor loyiha ham chizmada bir xil oraliqni ishlatadi va
haqiqiy radiusni panelda son bilan yozadi. Biz ham shunday qilamiz.

Shuning uchun `m_Radius` maydoni `m_LevelSpacing` ("darajalar orasidagi masofa") ga aylanadi. Maydon nomi o'zgargani
uchun Unity uni eski `m_Radius` bilan bog'lay olmaydi va Inspector'da koddagi boshlang'ich qiymat (`4`) chiqadi.
Bizda son bir xil, shuning uchun hech narsa yo'qolmaydi.

**`const int MaxLevel = 6`**: `const` o'zgarmas son. U Inspector'da ko'rinmaydi va dastur davomida o'zgarmaydi. `int`
butun son turi (`float` dan farqli o'laroq, kasr qismi yo'q). Orbita raqami butun bo'lgani uchun `m_Level` ham `int`.

### 5.2. Input System: `isPressed` va `wasPressedThisFrame`

00-darsda ko'rdik: loyihada faqat yangi **Input System** yoqilgan. Klaviatura shunday o'qiladi:

```csharp
Keyboard keyboard = Keyboard.current;
if (keyboard == null) return;
```

`Keyboard.current` hozirgi klaviatura. Kompyuterga klaviatura ulanmagan bo'lsa (masalan, telefonda), u **`null`**
bo'ladi, ya'ni "hech narsa". `null` ning xususiyatini o'qishga urinish dasturni xatoga olib keladi, shuning uchun
avval tekshiramiz. `return` funksiyadan darhol chiqadi.

Har bir tugmani ikki xil so'rash mumkin:

| Xususiyat | Qachon `true` |
|---|---|
| `isPressed` | tugma bosib turilgan **har bir kadrda** |
| `wasPressedThisFrame` | faqat tugma bosilgan **o'sha bitta kadrda** |

Bizga ikkinchisi kerak. Odam tugmani odatda 0.1–0.2 soniya bosib turadi. 165 FPS da 0.2 soniya 33 kadr. `isPressed`
bilan bitta bosishda `n` 33 marta oshib, darhol 6 ga sakrab ketardi.

**Muhim:** Editor'da klaviatura faqat **Game oynasi faol bo'lganda** o'yinga boradi. Play'ni bosgach, bir marta Game
oynasining ichiga bosing, keyin strelkalarni ishlating.

`using UnityEngine.InputSystem;` qatori `Keyboard` turini to'liq nomisiz ishlatishga ruxsat beradi (2.4-bo'limdagi
`using` kabi).

### 5.3. `Mathf.Clamp`: chegaradan chiqarmaslik

`n` 1 dan kichik va 6 dan katta bo'lmasligi kerak. `Mathf.Clamp(qiymat, min, max)` sonni shu oraliqqa "qisib"
qo'yadi:

| Chaqiruv | Natija | Nega |
|---|---|---|
| `Mathf.Clamp(3, 1, 6)` | 3 | oraliq ichida, o'zgarmaydi |
| `Mathf.Clamp(7, 1, 6)` | 6 | katta, maksimumga tushadi |
| `Mathf.Clamp(0, 1, 6)` | 1 | kichik, minimumga ko'tariladi |

### 5.4. Kamera qancha ko'radi

`n = 6` da radius 24. Hozirgi kamera buni ko'rsatadimi?

Kamera piramida shaklidagi hajmni ko'radi: undan uzoqlashgan sari ko'rinadigan maydon kengayadi. Bizning kameraning
ko'rish burchagi (**Field of View**, FOV) tik yo'nalishda 60°. Shu burchak uchun ko'rinadigan maydonning yarim
balandligi masofaning **0.577** qismiga teng:

| Kameragacha masofa | Ko'rinadigan yarim balandlik | Yarim kenglik (16:9) |
|---|---|---|
| 30 | 17.3 | 30.8 |
| 55 | 31.8 | 56.5 |

Ya'ni masofa ikki baravar oshsa, ko'rinadigan maydon ham ikki baravar kattalashadi. 0.577 soni qayerdan kelishi
09-darsda (3D kamera), hozir jadval yetadi.

Kamera 30 da turganda `n = 5` (radius 20) va `n = 6` (radius 24) dagi elektron yuqori va pastda ekrandan chiqib ketadi
(17.3 dan katta), yon tomonlarda esa ko'rinadi (30.8 dan kichik). Kamerani 55 ga olib borsak, 24 radiusli orbita
butunlay sig'adi va chetida joy qoladi.

### 5.5. Kod

Fayl boshi, maydonlar:

```diff
 using UnityEngine;
+using UnityEngine.InputSystem;
 
 namespace AtomSim.Bohr
 {
+    /// <summary>
+    /// Elektronni yadro atrofida doira bo'ylab aylantiradi (01-dars).
+    /// Yuqori va pastki strelka orbita raqamini (n) o'zgartiradi.
+    /// </summary>
     public sealed class ElectronOrbit : MonoBehaviour
     {
+        const int MaxLevel = 6;
+
         [SerializeField] Transform m_Electron;
-        [SerializeField] float m_Radius = 4f;
+        [SerializeField] float m_LevelSpacing = 4f;
         [SerializeField] float m_AngularSpeed = 2.4f;
 
+        int m_Level = 1;
         float m_Angle;
 
         void Start()
```

`///` bilan boshlangan qatorlar **hujjat izohi**: Rider class nomi ustiga sichqoncha olib borganingizda shu matnni
ko'rsatadi. Kodga ta'sir qilmaydi.

`Update` va ikkita yangi funksiya (`Update`ning yopuvchi `}` qavsidan keyin, classning oxirigacha):

```diff
         void Update()
         {
+            HandleKeyboard();
+
             m_Angle += m_AngularSpeed * Time.deltaTime;
-            m_Electron.localPosition = new Vector3(Mathf.Cos(m_Angle), Mathf.Sin(m_Angle), 0f) * m_Radius;
+            float radius = m_Level * m_LevelSpacing;
+            m_Electron.localPosition = new Vector3(Mathf.Cos(m_Angle), Mathf.Sin(m_Angle), 0f) * radius;
+        }
+
+        void HandleKeyboard()
+        {
+            Keyboard keyboard = Keyboard.current;
+            if (keyboard == null) return;
+
+            if (keyboard.upArrowKey.wasPressedThisFrame) SetLevel(m_Level + 1);
+            if (keyboard.downArrowKey.wasPressedThisFrame) SetLevel(m_Level - 1);
+        }
+
+        void SetLevel(int level)
+        {
+            m_Level = Mathf.Clamp(level, 1, MaxLevel);
+            Debug.Log($"Orbita: n = {m_Level}, radius = {m_Level * m_LevelSpacing}");
         }
     }
 }
```

Qismma-qism:

**`HandleKeyboard()` va `SetLevel(...)`** alohida funksiyalar. `Update` ichida hammasini yozish mumkin edi, lekin
funksiya nomi nima qilinayotganini aytib turadi va `Update` qisqa qoladi. Keyingi darslarda `SetLevel` ni klaviatura
emas, foton chaqiradi (04-dars), shuning uchun u hozirdanoq alohida.

**`float radius = m_Level * m_LevelSpacing;`** **mahalliy o'zgaruvchi** (local variable): faqat shu funksiya ichida
yashaydi va har kadrda qaytadan hisoblanadi. `int` ni `float` ga ko'paytirsa, natija `float` bo'ladi.

**`SetLevel(m_Level + 1)`** yangi qiymatni hisoblaydi, `Clamp` uni chegarada ushlaydi. `n = 6` da yuqori strelkani
bossangiz ham, `Clamp(7, 1, 6) = 6`. Console'da baribir `Orbita: n = 6, radius = 24` chiqadi: funksiya chaqirildi,
faqat qiymat o'zgarmadi.

**Kamera.** Kodni saqlagach, Unity'da `Main Camera` Position `z` ni `-55` qiling va **Ctrl+S**.

### 5.6. Ishga tushirib ko'ring

1. **Play**, keyin Game oynasining ichiga bir marta bosing.
2. Yuqori strelkani bosing: elektron 8 radiusli orbitaga o'tadi, Console'da `Orbita: n = 2, radius = 8`.
3. Yana to'rt marta bosing: `n = 3, 4, 5, 6`, radius `12, 16, 20, 24`. Elektron har safar sakrab o'tadi, chunki
   radius bir kadrda o'zgaradi. 6-orbitada u ekranga sig'adi.
4. Yuqori strelkani yana bosing: `n = 6, radius = 24`, elektron o'sha joyda qoladi.
5. Pastki strelka bilan 1 gacha qayting. 1 da pastki strelka hech narsani o'zgartirmaydi.
6. Elektron hamma orbitada 2.6 soniyada bir aylanadi, lekin tashqi orbitada ancha tez yuguradi (4.2-bo'lim).

Klaviaturani oynasiz Editor'da virtual tugmalar bilan sinadim: yuqori strelka radiusni `4 → 8 → 12 → 16 → 20 → 24`
qildi va 24 da to'xtadi, pastki strelka `24 → ... → 4` qildi va 4 da to'xtadi.

<details>
<summary>Shu qadamdan keyingi to'liq <code>ElectronOrbit.cs</code> (solishtirish uchun)</summary>

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

namespace AtomSim.Bohr
{
    /// <summary>
    /// Elektronni yadro atrofida doira bo'ylab aylantiradi (01-dars).
    /// Yuqori va pastki strelka orbita raqamini (n) o'zgartiradi.
    /// </summary>
    public sealed class ElectronOrbit : MonoBehaviour
    {
        const int MaxLevel = 6;

        [SerializeField] Transform m_Electron;
        [SerializeField] float m_LevelSpacing = 4f;
        [SerializeField] float m_AngularSpeed = 2.4f;

        int m_Level = 1;
        float m_Angle;

        void Start()
        {
            Debug.Log($"ElectronOrbit ishga tushdi. Elektron: {m_Electron.name}");
        }

        void Update()
        {
            HandleKeyboard();

            m_Angle += m_AngularSpeed * Time.deltaTime;
            float radius = m_Level * m_LevelSpacing;
            m_Electron.localPosition = new Vector3(Mathf.Cos(m_Angle), Mathf.Sin(m_Angle), 0f) * radius;
        }

        void HandleKeyboard()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard == null) return;

            if (keyboard.upArrowKey.wasPressedThisFrame) SetLevel(m_Level + 1);
            if (keyboard.downArrowKey.wasPressedThisFrame) SetLevel(m_Level - 1);
        }

        void SetLevel(int level)
        {
            m_Level = Mathf.Clamp(level, 1, MaxLevel);
            Debug.Log($"Orbita: n = {m_Level}, radius = {m_Level * m_LevelSpacing}");
        }
    }
}
```

</details>

## Nima buzilsa nima bo'ladi

Birinchi to'rt qatorni Unity'da ishga tushirib ko'rdim, qolganlari kodning o'zidan kelib chiqadi:

| O'zgarish | Natija | Xabar bormi |
|---|---|---|
| `Electron` maydoni bo'sh qolgan | `Start`da va har kadrda `UnassignedReferenceException: The variable m_Electron of ElectronOrbit has not been assigned.` | ha, har kadrda qizil xato |
| `Keyboard.current` o'rniga eski `Input.GetKeyDown(KeyCode.UpArrow)` | har kadrda `InvalidOperationException: You are trying to read Input using the UnityEngine.Input class, but you have switched active Input handling to Input System package in Player Settings.` | ha |
| `void Update()` o'rniga `void update()` | funksiya hech qachon chaqirilmaydi, elektron `(4, 0, 0)` da qotib turadi | **yo'q** |
| skriptning `.meta` fayli Git'ga tushmagan, boshqa kompyuterda ochildi | `Atom`dagi komponent "Missing", Play'da `The referenced script (Unknown) on this Behaviour is missing!` | ogohlantirish |
| `* Time.deltaTime` yo'q | elektron har kadrda 137.5° sakraydi, tezlik FPS ga bog'liq | yo'q |
| `localPosition` o'rniga `position` | `Atom` `(0, 0, 0)` da turganda farq yo'q. `Atom`ni sursangiz, yadro suriladi, elektron esa sahna markazi atrofida aylanishda davom etadi | yo'q |
| `wasPressedThisFrame` o'rniga `isPressed` | bitta bosishda `n` darhol 6 ga (yoki 1 ga) sakraydi | yo'q |
| `Mathf.Cos(90)` deb gradus berilgan | 90 radianning kosinusi `-0.448` olinadi, nuqta noto'g'ri joyga tushadi | yo'q |

Uchinchi qator eng xavflisi: xato yo'q, ogohlantirish yo'q, shunchaki hech narsa ishlamaydi. Bunday paytda birinchi
tekshiradigan narsa: Rider'da funksiya yonida Unity belgisi bormi.

## Git

Dars tugagach:

```
git add .
git status
git commit -m "Lesson 01: nucleus, orbiting electron, ElectronOrbit script"
git push
```

`git status` da `ElectronOrbit.cs` bilan birga `ElectronOrbit.cs.meta`, `Materials` ichidagi `.mat` va `.mat.meta`
fayllar, shuningdek yangi papkalarning `.meta` fayllari (`Runtime.meta`, `Bohr.meta`, `Materials.meta`) bo'lishi kerak.

## Ixtiyoriy tajribalar

1. **Debug qilish.** Rider'da `SetLevel` ichidagi `m_Level = ...` qatorining chap chetini bosib, qizil nuqta
   (*breakpoint*) qo'ying. Rider tepasidagi ishga tushirish ro'yxatidan **Attach to Unity Editor** ni tanlab, yonidagi
   qo'ng'iz (Debug) tugmasini bosing. Unity'da Play, Game oynasiga bosing va yuqori strelkani bosing: Unity to'xtaydi,
   Rider esa shu qatorda `level` va `m_Level` qiymatlarini ko'rsatadi. Pastdagi Debug oynasidagi **Step Over**
   tugmasi bilan keyingi qatorga o'ting, **Resume** bilan davom ettiring. Keyin breakpoint'ni olib tashlab, qizil
   kvadrat (Stop) bilan ulanishni uzing.
2. `Atom`ning Rotation `z` ni `30` qiling va Play. Elektron o'sha doirada aylanadi, faqat boshlanish nuqtasi 30° ga
   burilgan: `localPosition` otaning burilishini ham hisobga oladi. Keyin Rotation `x` ni `60` qiling: doira
   kameradan qaraganda ellipsga aylanadi, chunki endi uni qiya ko'ryapmiz. Sinab bo'lgach, `(0, 0, 0)` ga qaytaring.
3. `Directional Light` ni o'chirib (Inspector'ning eng tepasidagi belgi), Play'ni bosing: Lit materiallar yorug'liksiz
   deyarli ko'rinmaydi. 02-darsda yorug'likka bog'liq bo'lmagan, o'zi porlaydigan shader yozamiz.

## Lug'at

| So'z | Ma'nosi |
|---|---|
| atom | modda tuzilgan eng kichik "g'isht": yadro va elektron(lar) |
| yadro (nucleus) | atomning og'ir markazi, vodorodda bitta proton |
| proton | yadrodagi musbat zaryadli zarra, elektrondan 1836 marta og'ir |
| elektron | yadro atrofidagi yengil, manfiy zaryadli zarra |
| zaryad | zarraning xususiyati: har xili tortishadi, bir xili itarishadi |
| sayyoraviy model | elektron yadro atrofida sayyora kabi aylanadi degan (noto'g'ri) rasm |
| koordinata | nuqtaning joyini aytuvchi sonlar `(x, y, z)` |
| vektor | birga yuradigan sonlar guruhi, strelka deb tasavvur qilinadi; Unity'da `Vector3` |
| GameObject | sahnadagi har bir narsa, komponentlar uchun "quti" |
| komponent (component) | GameObject'ga taqiladigan, bitta ish qiladigan qism |
| Transform | o'rin, burilish va o'lcham; har bir GameObject'da bor |
| ota / bola (parent / child) | bola otasi bilan birga harakatlanadi |
| local / world | otaga nisbatan / butun sahnaga nisbatan koordinata |
| material, shader | sirt qanday chizilishi: GPU dasturi va uning sozlamalari |
| class | ma'lumot va funksiyalar qolipi |
| `MonoBehaviour` | classni Unity komponentiga aylantiruvchi asosiy class |
| `Start`, `Update` | Unity chaqiradigan funksiyalar: bir marta / har kadrda |
| kadr (frame), FPS | ekranga chizilgan bitta rasm / soniyadagi kadrlar soni |
| `Time.deltaTime` | oldingi kadrdan beri o'tgan vaqt, soniyada |
| `[SerializeField]` | maydonni Inspector'da ko'rsatish va sahnada saqlash |
| havola (reference) | obyektning o'zi emas, unga yo'l |
| `null` | "hech narsa": havola hech qayerga ko'rsatmayapti |
| gradus, radian | burchak o'lchovlari: to'liq aylana 360° = 6.2832 radian |
| π (pi) | aylana uzunligining diametrga nisbati, 3.1416 |
| `cos`, `sin` | burchakni radiusi 1 doira ustidagi nuqtaning `x` va `y` siga aylantiradi |
| Pifagor teoremasi | `a × a + b × b = c × c`; nuqtagacha masofa `sqrt(x × x + y × y)` |
| kvadrat, `sqrt` | sonni o'ziga ko'paytirish / kvadratdan sonni qaytarish |
| burchak tezligi | soniyada burchak qanchaga o'zgarishi, radian/soniya |
| aylanish davri | bitta to'liq aylanish vaqti: `6.2832 / burchak tezligi` |
| chiziqli tezlik | soniyada bosib o'tilgan yo'l: `burchak tezligi × radius` |
| `Mathf.Clamp` | sonni berilgan oraliq ichida ushlab turish |
| `wasPressedThisFrame` | tugma aynan shu kadrda bosildimi |
| FOV (Field of View) | kameraning ko'rish burchagi |

## Keyingi dars

02-darsda atomni "yoritamiz": orbitalarni `LineRenderer` bilan chiziq qilib chizamiz, birinchi shader'imizni
(HLSL tilida) yozamiz va yadro bilan elektronni Lit sharlar o'rniga o'zi porlaydigan nurga aylantiramiz. Oxirida
URP'ning **Bloom** effekti nurni atrofga yoyadi. Matematika tomonda: doirani nuqtalar bilan chizish va rang
qanday qo'shilishi.

Shu dars tugagach yozing. Xohlasangiz, ishingizni ko'rib chiqaman.
