# 00b. MacBook'ga ko'chish

Bu dars emas, qisqa qo'llanma: loyihani Windows kompyuterdan MacBook'ga olib o'tish va u yerda davom ettirish.
Kurs kodi ikkala tizimda bir xil, o'zgaradigan narsa faqat atrofdagi muhit.

> **Halol ogohlantirish.** Windows'dagi qadamlarni va oxiridagi papka nomini o'zgartirish retseptini shu
> kompyuterda sinab ko'rdim. Mac'dagi qadamlarni sinay olmadim, chunki menda Mac yo'q. Ular Unity'ning odatdagi
> tartibi bo'yicha yozilgan. Biror joyi ekrandagidan farq qilsa, menga ayting, shu sahifani tuzataman.

## Nima ko'chadi, nima ko'chmaydi

00-darsning 2-qadamini eslang: loyihaning "o'zi" uchta papka, qolganini Unity qayta yaratadi.

| Narsa | Mac'ga qanday boradi |
|---|---|
| `Assets` (`.meta` fayllari bilan), `Packages`, `ProjectSettings` | GitHub orqali |
| `docs`, `CLAUDE.md`, `.gitignore` | GitHub orqali |
| `Library` (986 MB) | bormaydi. Mac'dagi Unity birinchi ochilishda o'zi quradi |
| `UserSettings`, `.idea`, `.csproj`, `.slnx` | bormaydi, har bir kompyuterda o'ziniki |
| "External Script Editor = Rider" tanlovi | bormaydi. Bu loyihaning emas, kompyuterning sozlamasi (4-qadamda qayta qilasiz) |
| Unity Editor va Rider dasturlari | Mac'da alohida o'rnatiladi |
| Yonidagi tayyor `AtomSimulation` loyihasi | bormaydi, u Git'da yo'q (oxirgi bo'limga qarang) |

## 1-qadam (Windows). Hamma ishni GitHub'ga yuboring

Hozir 01-darsning 4-qadamigacha qilgan ishingiz (materiallar, `ElectronOrbit.cs`, sahnadagi `Atom`) faqat shu
kompyuterda turibdi, GitHub'da faqat 00-dars bor. Dars tugamagan bo'lsa ham commit qilish mumkin, bunday commit
"WIP" (work in progress, "ish davom etmoqda") deb belgilanadi.

1. Unity ochiq bo'lsa: **Ctrl+S**, keyin Unity'ni yoping. Saqlanmagan sahna diskda yo'q, demak Git ham uni ko'rmaydi.
2. Rider terminalida:

```
git add .
git status
git commit -m "Lesson 01 (WIP): steps 1-4"
git push
```

`git status` ro'yxatida bo'lishi kerak: `ElectronOrbit.cs` va `ElectronOrbit.cs.meta`, `Electron.mat`, `Nucleus.mat`
va ularning `.meta`lari, `Runtime.meta`, `Bohr.meta`, `Materials.meta`, sahna fayli, `ProjectSettings` ichidagi
bir nechta fayl, shu qo'llanma va `CLAUDE.md`.

**Tekshirib ko'ring:**

- `git status` "nothing to commit, working tree clean" deydi.
- `git status -sb` ning birinchi qatorida `[ahead 1]` degan yozuv yo'q (bo'lsa, push ketmagan).
- GitHub sahifasida `Assets/AtomSIm/Runtime/Bohr` ichida ikkala fayl ko'rinadi: `.cs` va `.cs.meta`.

`.meta` nega muhimligini 00-darsda (2.2) ko'rdik: usiz Mac'da `Atom`dagi komponent "Missing" bo'lib qoladi.

## 2-qadam (Mac). Dasturlar

1. **Git.** Terminal'da `git --version`. OpenGL kursidan beri o'rnatilgan bo'lishi kerak.
2. **Unity Hub.** [unity.com/download](https://unity.com/download) dan o'rnating va Windows'dagi Unity akkauntingiz
   bilan kiring. Hub litsenziya so'rasa, bepul **Personal** litsenziyani tanlang.
3. **Unity Editor 6000.6.9f1.** Aynan shu versiya kerak (00-dars, 1.3: boshqa versiya loyihani "ko'chirib" yuboradi,
   keyin Windows'da ochilmay qolishi mumkin). Ikki yo'li bor:
   - 3-qadamda loyihani Hub'ga qo'shganingizda Hub versiya yo'qligini ko'rsatadi va o'rnatishni taklif qiladi;
   - yoki Safari manzil qatoriga `unityhub://6000.6.9f1/8c45277fb9c5` deb yozing, Hub aynan shu versiyani ochadi.

   Mac'ingiz M1 yoki undan yangi bo'lsa, **Apple silicon** variantini tanlang. Qo'shimcha modul hozircha kerak emas.
4. **Rider.** [jetbrains.com/rider](https://www.jetbrains.com/rider/) dan, Windows'dagi JetBrains akkauntingiz bilan.

## 3-qadam (Mac). Loyihani olish va ochish

Terminal'da:

```
mkdir -p ~/dev/UNITY
cd ~/dev/UNITY
git clone https://github.com/sardor-c/AtomSimulation_Unity.git AtomSimulation_Scratch
```

Oxirgi so'z papka nomi. GitHub'dagi repo `AtomSimulation_Unity` deb atalgan, lekin papkani Windows'dagidek
`AtomSimulation_Scratch` deb nomlasak, darslardagi nomlar (masalan `AtomSimulation_Scratch.slnx`) mos keladi.

Keyin:

1. Unity Hub → **Projects** → **Add** yonidagi strelka → **Add project from disk** → `~/dev/UNITY/AtomSimulation_Scratch`.
2. Loyihani bosing. **Birinchi ochilish bir necha daqiqa oladi**: Unity `Library`ni noldan quryapti (hamma
   paketlarni import qiladi, shaderlarni kompilyatsiya qiladi). Keyingi ochilishlar tez bo'ladi.
3. Editor bo'sh `Untitled` sahna bilan ochilishi mumkin: oxirgi qaysi sahna ochiq turgani ham `Library`da
   saqlanadi. Project oynasida `Assets/AtomSIm/Scenes/AtomSimulation` ni ikki marta bosing.

## 4-qadam (Mac). Rider'ni ulash

00-darsning 4.2-bo'limi, faqat menyu boshqa joyda: Mac'da sozlamalar **Edit** menyusida emas, eng chapdagi
**Unity** menyusida.

1. **Unity → Settings...** (**Cmd+,**) → **External Tools**.
2. **External Script Editor** ro'yxatidan Rider'ni tanlang.
3. **Regenerate project files**.
4. **Assets → Open C# Project**.

## 5-qadam (Mac). Tekshirib ko'ring

- Console'da qizil xato yo'q.
- Hierarchy'da `Atom`ni tanlang: Inspector'da **Electron Orbit** komponenti bor ("Missing" emas), **Electron**
  maydonida `Electron (Transform)`, Radius `4`, Angular Speed `2.4`.
- Yadro va elektron o'z rangida. Pushti (magenta) bo'lsa, material yoki shader kelmagan.
- **Play** (**Cmd+P**): elektron yadro atrofida aylanadi, xuddi Windows'dagidek.
- Unity'ni yopib, Terminal'da `git status`. Toza bo'lishi kerak. Agar siz hech narsa qilmagan holda
  `ProjectSettings` yoki `Packages/packages-lock.json` o'zgargan bo'lsa, bu Unity'ning ishi: `git diff` natijasini
  menga ko'rsating, birga qaraymiz.

Hammasi joyida bo'lsa, [01-dars](01-birinchi-skript.md)ning 5-qadamidan davom etasiz.

## Windows va Mac: nimasi boshqacha

| Windows | Mac |
|---|---|
| **Ctrl** (Ctrl+S, Ctrl+P, Ctrl+Z) | **Cmd** (Cmd+S, Cmd+P, Cmd+Z) |
| Edit → Preferences... | Unity → Settings... |
| Hierarchy'da o'chirish: Delete | Cmd+Delete |
| Nomini o'zgartirish: F2 | Return (Enter) |
| `C:\dev\UNITY\AtomSimulation_Scratch` | `~/dev/UNITY/AtomSimulation_Scratch` |
| Grafika: Direct3D | Grafika: Metal (Editor sarlavhasida `<Metal>`) |
| 165 Hz monitor | MacBook ekrani 60 yoki 120 Hz |

Darslarda "Ctrl+S" deyilgan joyni "Cmd+S" deb o'qing. Kelajakdagi darslarda Mac'dagi farqni o'zim yozib boraman.

- **Grafika.** Unity bizning shader va compute shader kodimizni har bir tizimning o'z tiliga o'zi tarjima qiladi
  (Windows'da Direct3D, Mac'da Metal). Shuning uchun kurs kodi o'zgarmaydi.
- **Kadr tezligi.** Darslardagi `dt` va FPS sonlari Windows kompyuterda o'lchangan. Mac'da boshqa son chiqadi, bu
  xato emas. Elektronning aylanish tezligi baribir bir xil: `Time.deltaTime` aynan shuning uchun (01-dars, 2.3).
- **Sichqoncha.** Scene oynasida aylanish, 04-darsdagi bosish va 09-darsdagi kamera sichqoncha bilan ancha qulay.
  Trackpad bilan ham bo'ladi (aylantirish: Option bosib sudrash, yaqinlashtirish: ikki barmoq), lekin imkon bo'lsa
  sichqoncha ulang.

## Ikki kompyuterda ishlasangiz

- Ishni boshlashdan oldin `git pull`, tugatgach `git add .`, `git commit`, `git push`. Har doim.
- Bitta sahnani ikki kompyuterda parallel o'zgartirmang. `.unity` fayli matn, lekin Git ikki xil o'zgarishni
  o'zi qo'sha olmaydi va "conflict" chiqadi. Bir kompyuterda tugatib push qiling, keyin ikkinchisida pull.

## Claude va tayyor namuna loyiha

Mac'da Claude Code'ni shu papkada ishga tushirsangiz, u `CLAUDE.md` ni o'qib, kursni to'xtagan joyidan davom
ettiradi. Lekin bitta narsa yetishmaydi: yonidagi tayyor `AtomSimulation` loyihasi (men darslarni yozganda
sonlarni va fizikani shunga solishtiraman) faqat Windows kompyuterda, Git'da yo'q.

Ikki yo'l bor:

1. **Namunani ham Mac'ga ko'chirish (tavsiya).** `C:\dev\UNITY\AtomSimulation` ichidan faqat uchta papka kerak:
   `Assets`, `Packages`, `ProjectSettings` (jami 5 MB atrofida, `Library` kerak emas). Ularni zip qilib, AirDrop,
   fleshka yoki bulut orqali Mac'ga o'tkazing va `~/dev/UNITY/AtomSimulation` ga oching, ya'ni kurs papkasining
   yoniga. Yoki GitHub'da alohida private repo qiling (shu loyihaning `.gitignore`idan nusxa olib).
2. **Darslarni Windows'da yozdirish.** Yangi dars kerak bo'lganda shu kompyuterda Claude'ni ochasiz, dars yozilib
   push qilinadi, Mac'da `git pull` qilib bajarasiz.

## Ixtiyoriy: papka nomidagi xato

Darslarda papka `Assets/AtomSim` deb yozilgan, sizda esa `Assets/AtomSIm` (ikkinchi `I` katta). Hech narsa
buzilmaydi: Windows ham, Mac ham fayl nomida katta-kichik harfni farqlamaydi, kod ichidagi `namespace AtomSim`
esa papka nomiga bog'liq emas. Xohlasangiz shunday qoldiring.

Tuzatmoqchi bo'lsangiz, **Mac'ga ko'chishdan oldin, Windows'da** qiling (keyin fayllar ko'payadi, ikki kompyuterda
nom har xil bo'lib qoladi). Git ham bunday nom o'zgarishini to'g'ridan-to'g'ri qila olmaydi: `git mv Assets/AtomSIm
Assets/AtomSim` buyrug'i `Invalid argument` xatosini berdi (sinab ko'rdim). Vaqtinchalik nom orqali ikki qadamda
bo'ladi. Unity yopiq bo'lsin:

```
git mv Assets/AtomSIm Assets/AtomSim_tmp
git mv Assets/AtomSim_tmp Assets/AtomSim
git mv Assets/AtomSIm.meta Assets/AtomSim.meta
git add .
git status
```

`git status` da fayllar `renamed: Assets/AtomSIm/... -> Assets/AtomSim/...` bo'lib ko'rinadi. `.meta` fayl ham
ko'chgani uchun GUID o'zgarmaydi va sahnadagi havolalar buzilmaydi. Keyin 1-qadamdagi commit va push.
Agar `Permission denied` chiqsa, Rider'ni ham yopib, buyruqlarni oddiy PowerShell'da bajaring.
