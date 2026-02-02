# Atelier Selenium - Automatisation de Navigateur Web (MAUI Windows)

Bienvenue dans cet atelier de programmation ! Vous allez apprendre a automatiser un navigateur web avec C# et Selenium dans une application MAUI.

## Prerequis

- Windows 10/11
- Visual Studio 2022 avec la charge de travail ".NET MAUI"
- Google Chrome installe

## Installation

### 1. Ouvrir le projet
Double-cliquez sur `recrutementstage2026.slnx` pour ouvrir la solution dans Visual Studio.

### 2. Restaurer les packages NuGet
Visual Studio restaurera automatiquement les packages. Sinon, faites un clic droit sur la solution > "Restaurer les packages NuGet".

### 3. Lancer l'application
- Selectionnez "Windows Machine" comme cible
- Appuyez sur F5 ou cliquez sur le bouton "Demarrer"

---

## Activite 1 : Personnalisation (20 min)

### Objectif
Modifier le texte de recherche par defaut et ajouter votre ville pour la meteo.

### Instructions

1. Ouvrez le fichier `Services/SeleniumService.cs`
2. Trouvez les lignes 35-40 :

```csharp
/// <summary>
/// Texte de recherche par defaut - CHANGEZ-LE !
/// </summary>
public string RechercheParDefaut { get; set; } = "Programmation C#";

/// <summary>
/// Ville par defaut pour la meteo - METTEZ VOTRE VILLE !
/// </summary>
public string VilleParDefaut { get; set; } = "Paris";
```

3. Remplacez par vos propres valeurs :

```csharp
public string RechercheParDefaut { get; set; } = "Jeux video 2026";
public string VilleParDefaut { get; set; } = "Lyon";
```

4. Relancez l'application et testez !

---

## Activite 2 : Nouveau Bouton (25 min)

### Objectif
Creer un bouton qui recherche automatiquement un nouveau type de lieu.

### Etape 1 : Ajouter la methode dans SeleniumService.cs

Trouvez la methode `RechercheRestaurants` et ajoutez en dessous :

```csharp
/// <summary>
/// Recherche les bowlings dans une ville.
/// </summary>
public async Task RechercheBowling(string ville)
{
    await Task.Run(async () =>
    {
        if (!await InitialiserNavigateur() || _driver == null)
            return;

        try
        {
            Log($"Recherche bowling a : {ville}");
            _driver.Navigate().GoToUrl($"https://www.google.com/search?q=bowling+{Uri.EscapeDataString(ville)}");
            Log($"Bowlings a {ville} trouves !");
        }
        catch (Exception ex)
        {
            Log($"Erreur : {ex.Message}");
        }
    });
}
```

### Etape 2 : Ajouter le bouton dans MainPage.xaml

Trouvez la section avec les boutons Cinema et Restaurants, et ajoutez :

```xml
<Button Text="Bowling"
        Clicked="OnBowlingClicked"
        BackgroundColor="#16a085"
        TextColor="White"/>
```

### Etape 3 : Ajouter le gestionnaire dans MainPage.xaml.cs

Ajoutez cette methode :

```csharp
/// <summary>
/// Recherche les bowlings dans la ville.
/// </summary>
private async void OnBowlingClicked(object? sender, EventArgs e)
{
    var ville = EntryVilleLocale.Text?.Trim();
    if (string.IsNullOrEmpty(ville))
    {
        await DisplayAlert("Attention", "Veuillez entrer une ville.", "OK");
        return;
    }

    await _seleniumService.RechercheBowling(ville);
}
```

### Idees de boutons supplementaires
- Escape Game
- Piscine
- Laser Game
- Karaoke
- Musee

---

## Activite 3 : Challenge (20 min)

### Objectif
Par groupes, choisissez et ameliorez une fonctionnalite avancee.

### Challenge 1 : Tendances YouTube ameliorees

Modifiez la methode `TendancesYouTube` pour recuperer plus d'informations :

```csharp
public async Task<List<string>> TendancesYouTubeAvance()
{
    var resultats = new List<string>();

    await Task.Run(async () =>
    {
        if (!await InitialiserNavigateur() || _driver == null)
            return;

        try
        {
            Log("Acces aux tendances YouTube...");
            _driver.Navigate().GoToUrl("https://www.youtube.com/feed/trending");
            await Task.Delay(3000);

            // Recuperer les conteneurs de videos
            var videos = _driver.FindElements(By.CssSelector("ytd-video-renderer"));
            Log("=== TOP 5 TENDANCES YOUTUBE ===");

            int count = 0;
            foreach (var video in videos)
            {
                if (count >= 5) break;

                try
                {
                    var titre = video.FindElement(By.CssSelector("#video-title")).Text;
                    var chaine = video.FindElement(By.CssSelector("#channel-name")).Text;

                    resultats.Add($"{titre} - {chaine}");
                    Log($"{count + 1}. {titre}");
                    Log($"   Chaine: {chaine}");
                    count++;
                }
                catch { }
            }
        }
        catch (Exception ex)
        {
            Log($"Erreur YouTube : {ex.Message}");
        }
    });

    return resultats;
}
```

### Challenge 2 : Alerte de prix

Creez un systeme qui verifie si un produit est en dessous d'un certain prix :

```csharp
public async Task<bool> AlertePrix(string produit, decimal prixMax)
{
    // 1. Rechercher le produit sur Amazon
    // 2. Recuperer les prix avec :
    //    var prixElements = _driver.FindElements(By.CssSelector(".a-price-whole"));
    // 3. Comparer avec prixMax
    // 4. Retourner true si un prix est inferieur

    // A vous de coder !
    return false;
}
```

### Challenge 3 : Surveillance d'actualites

Creez un systeme qui enregistre les titres dans un fichier :

```csharp
public async Task SauvegarderActualites(string sujet, string fichier)
{
    var actualites = await RechercheActualites(sujet);

    // Sauvegarder dans un fichier
    var contenu = $"Actualites du {DateTime.Now:dd/MM/yyyy HH:mm}\n";
    contenu += $"Sujet : {sujet}\n\n";

    foreach (var actu in actualites)
    {
        contenu += $"- {actu}\n";
    }

    await File.WriteAllTextAsync(fichier, contenu);
    Log($"Actualites sauvegardees dans {fichier}");
}
```

---

## Aide-memoire Selenium en C#

### Trouver des elements

```csharp
// Par ID
var element = driver.FindElement(By.Id("mon-id"));

// Par nom de classe
var element = driver.FindElement(By.ClassName("ma-classe"));

// Par selecteur CSS
var element = driver.FindElement(By.CssSelector("div.classe > p"));

// Par texte du lien
var element = driver.FindElement(By.LinkText("Cliquez ici"));

// Plusieurs elements
var elements = driver.FindElements(By.CssSelector(".item"));
```

### Actions courantes

```csharp
// Cliquer
element.Click();

// Ecrire du texte
element.SendKeys("Mon texte");

// Appuyer sur Entree
element.SendKeys(Keys.Return);

// Recuperer le texte
string texte = element.Text;

// Recuperer un attribut
string href = element.GetAttribute("href");

// Attendre un element
var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
var element = wait.Until(d => d.FindElement(By.Id("mon-id")));
```

### Navigation

```csharp
// Aller sur une page
driver.Navigate().GoToUrl("https://www.google.com");

// Page precedente
driver.Navigate().Back();

// Rafraichir
driver.Navigate().Refresh();

// URL actuelle
string url = driver.Url;
```

---

## Structure du projet

```
recrutementstage2026/
├── MainPage.xaml           # Interface utilisateur (XAML)
├── MainPage.xaml.cs        # Code-behind (gestionnaires d'evenements)
├── Services/
│   └── SeleniumService.cs  # Service d'automatisation Selenium
├── App.xaml                # Configuration de l'application
└── MauiProgram.cs          # Point d'entree de l'application
```

---

## Depannage

### Le navigateur ne s'ouvre pas
- Verifiez que Chrome est installe
- Verifiez que vous executez sur "Windows Machine"

### Erreur "element not found"
- L'element n'existe peut-etre pas sur la page
- Ajoutez un `await Task.Delay(2000);` avant de chercher l'element
- Utilisez `WebDriverWait` pour attendre l'element

### L'application ne compile pas
- Verifiez que vous avez restaure les packages NuGet
- Verifiez que vous ciblez Windows

---

## Pour aller plus loin

- [Documentation Selenium C#](https://www.selenium.dev/documentation/webdriver/)
- [Documentation .NET MAUI](https://learn.microsoft.com/fr-fr/dotnet/maui/)
- [Selecteurs CSS](https://www.w3schools.com/cssref/css_selectors.asp)

Bon atelier !
