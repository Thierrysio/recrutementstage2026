// =============================================================================
// ATELIER SELENIUM - SERVICE D'AUTOMATISATION DE NAVIGATEUR
// =============================================================================
// Ce service gère toutes les interactions avec Selenium WebDriver.
// Les lycéens peuvent modifier les méthodes pour personnaliser les actions.
// =============================================================================

#if WINDOWS
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
#endif

namespace recrutementstage2026.Services;

/// <summary>
/// Service d'automatisation de navigateur avec Selenium.
/// Contient toutes les fonctionnalités pour les 3 activités de l'atelier.
/// </summary>
public class SeleniumService : IDisposable
{
#if WINDOWS
    private IWebDriver? _driver;
    private readonly List<string> _logs = new();

    /// <summary>
    /// Événement déclenché quand un nouveau log est ajouté.
    /// </summary>
    public event Action<string>? OnLog;

    // =========================================================================
    // ACTIVITÉ 1 : PERSONNALISATION
    // Modifiez ces valeurs par défaut !
    // =========================================================================

    /// <summary>
    /// Texte de recherche par défaut - CHANGEZ-LE !
    /// </summary>
    public string RechercheParDefaut { get; set; } = "Programmation C#";

    /// <summary>
    /// Ville par défaut pour la météo - METTEZ VOTRE VILLE !
    /// </summary>
    public string VilleParDefaut { get; set; } = "Paris";

    /// <summary>
    /// Ajoute un message au journal.
    /// </summary>
    private void Log(string message)
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss");
        var logMessage = $"[{timestamp}] {message}";
        _logs.Add(logMessage);
        OnLog?.Invoke(logMessage);
    }

    /// <summary>
    /// Initialise le navigateur Chrome.
    /// </summary>
    public async Task<bool> InitialiserNavigateur()
    {
        if (_driver != null)
            return true;

        return await Task.Run(() =>
        {
            try
            {
                Log("Initialisation du navigateur Chrome...");

                // Installation automatique du driver Chrome
                new DriverManager().SetUpDriver(new ChromeConfig());

                // Configuration du navigateur
                var options = new ChromeOptions();
                options.AddArgument("--start-maximized");
                options.AddArgument("--disable-notifications");
                options.AddArgument("--disable-popup-blocking");
                options.AddExcludedArgument("enable-logging");

                _driver = new ChromeDriver(options);
                Log("Navigateur initialisé avec succès !");
                return true;
            }
            catch (Exception ex)
            {
                Log($"Erreur d'initialisation : {ex.Message}");
                return false;
            }
        });
    }

    /// <summary>
    /// Ferme le navigateur proprement.
    /// </summary>
    public void FermerNavigateur()
    {
        if (_driver != null)
        {
            _driver.Quit();
            _driver.Dispose();
            _driver = null;
            Log("Navigateur fermé.");
        }
    }

    // =========================================================================
    // ACTIVITÉ 1 : RECHERCHE GOOGLE ET MÉTÉO
    // =========================================================================

    /// <summary>
    /// Effectue une recherche sur Google.
    /// </summary>
    /// <param name="texte">Le texte à rechercher</param>
    public async Task RechercheGoogle(string texte)
    {
        await Task.Run(async () =>
        {
            if (!await InitialiserNavigateur() || _driver == null)
                return;

            try
            {
                Log($"Recherche Google : {texte}");

                // Aller sur Google
                _driver.Navigate().GoToUrl("https://www.google.com");
                await Task.Delay(1000);

                // Accepter les cookies si nécessaire
                try
                {
                    var btnCookies = _driver.FindElement(By.Id("L2AGLb"));
                    btnCookies.Click();
                    await Task.Delay(500);
                }
                catch { /* Pas de popup cookies */ }

                // Trouver la barre de recherche
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                var barreRecherche = wait.Until(d => d.FindElement(By.Name("q")));

                // Taper le texte et valider
                barreRecherche.Clear();
                barreRecherche.SendKeys(texte);
                barreRecherche.SendKeys(Keys.Return);

                Log($"Recherche '{texte}' effectuée !");
            }
            catch (Exception ex)
            {
                Log($"Erreur lors de la recherche : {ex.Message}");
            }
        });
    }

    /// <summary>
    /// Affiche la météo pour une ville.
    /// </summary>
    /// <param name="ville">La ville pour la météo</param>
    public async Task VoirMeteo(string ville)
    {
        await Task.Run(async () =>
        {
            if (!await InitialiserNavigateur() || _driver == null)
                return;

            try
            {
                Log($"Recherche météo pour : {ville}");

                // Recherche météo directe sur Google
                _driver.Navigate().GoToUrl($"https://www.google.com/search?q=météo+{Uri.EscapeDataString(ville)}");

                Log($"Météo de {ville} affichée !");
            }
            catch (Exception ex)
            {
                Log($"Erreur météo : {ex.Message}");
            }
        });
    }

    // =========================================================================
    // ACTIVITÉ 2 : CINÉMA ET RESTAURANTS
    // =========================================================================

    /// <summary>
    /// Recherche les horaires de cinéma dans une ville.
    /// </summary>
    public async Task RechercheCinema(string ville)
    {
        await Task.Run(async () =>
        {
            if (!await InitialiserNavigateur() || _driver == null)
                return;

            try
            {
                Log($"Recherche cinémas à : {ville}");
                _driver.Navigate().GoToUrl($"https://www.google.com/search?q=cinéma+horaires+{Uri.EscapeDataString(ville)}");
                Log($"Cinémas à {ville} trouvés !");
            }
            catch (Exception ex)
            {
                Log($"Erreur : {ex.Message}");
            }
        });
    }

    /// <summary>
    /// Recherche les restaurants dans une ville.
    /// </summary>
    public async Task RechercheRestaurants(string ville)
    {
        await Task.Run(async () =>
        {
            if (!await InitialiserNavigateur() || _driver == null)
                return;

            try
            {
                Log($"Recherche restaurants à : {ville}");
                _driver.Navigate().GoToUrl($"https://www.google.com/search?q=restaurants+{Uri.EscapeDataString(ville)}");
                Log($"Restaurants à {ville} trouvés !");
            }
            catch (Exception ex)
            {
                Log($"Erreur : {ex.Message}");
            }
        });
    }

    /// <summary>
    /// Recherche un type de lieu personnalisé.
    /// </summary>
    public async Task RecherchePersonnalisee(string typeLieu, string ville)
    {
        await Task.Run(async () =>
        {
            if (!await InitialiserNavigateur() || _driver == null)
                return;

            try
            {
                Log($"Recherche {typeLieu} à : {ville}");
                _driver.Navigate().GoToUrl($"https://www.google.com/search?q={Uri.EscapeDataString(typeLieu)}+{Uri.EscapeDataString(ville)}");
                Log($"{typeLieu} à {ville} trouvés !");
            }
            catch (Exception ex)
            {
                Log($"Erreur : {ex.Message}");
            }
        });
    }

    // =========================================================================
    // ACTIVITÉ 3 : CHALLENGES AVANCÉS
    // =========================================================================

    /// <summary>
    /// Récupère les tendances YouTube.
    /// </summary>
    public async Task<List<string>> TendancesYouTube()
    {
        var resultats = new List<string>();

        await Task.Run(async () =>
        {
            if (!await InitialiserNavigateur() || _driver == null)
                return;

            try
            {
                Log("Accès aux tendances YouTube...");
                _driver.Navigate().GoToUrl("https://www.youtube.com/feed/trending");
                await Task.Delay(3000);

                // Récupérer les titres des vidéos
                try
                {
                    var videos = _driver.FindElements(By.CssSelector("#video-title"));
                    Log("=== TOP 5 TENDANCES YOUTUBE ===");

                    int count = 0;
                    foreach (var video in videos)
                    {
                        if (count >= 5) break;

                        var titre = video.GetAttribute("title");
                        if (!string.IsNullOrEmpty(titre))
                        {
                            var texte = titre.Length > 50 ? titre[..50] + "..." : titre;
                            resultats.Add(texte);
                            Log($"{count + 1}. {texte}");
                            count++;
                        }
                    }
                }
                catch
                {
                    Log("Page tendances YouTube ouverte !");
                }
            }
            catch (Exception ex)
            {
                Log($"Erreur YouTube : {ex.Message}");
            }
        });

        return resultats;
    }

    /// <summary>
    /// Recherche un produit sur Amazon.
    /// </summary>
    public async Task<List<string>> VerifierProduit(string produit)
    {
        var resultats = new List<string>();

        await Task.Run(async () =>
        {
            if (!await InitialiserNavigateur() || _driver == null)
                return;

            try
            {
                Log($"Recherche du produit : {produit}");
                _driver.Navigate().GoToUrl($"https://www.amazon.fr/s?k={Uri.EscapeDataString(produit)}");
                await Task.Delay(2000);

                Log($"Résultats pour '{produit}' sur Amazon !");

                // Extraire les premiers résultats
                try
                {
                    var elements = _driver.FindElements(
                        By.CssSelector("[data-component-type='s-search-result'] h2 a span"));

                    Log("=== PREMIERS RÉSULTATS ===");
                    int count = 0;
                    foreach (var elem in elements)
                    {
                        if (count >= 3) break;
                        var texte = elem.Text;
                        if (!string.IsNullOrEmpty(texte))
                        {
                            var affichage = texte.Length > 40 ? texte[..40] + "..." : texte;
                            resultats.Add(affichage);
                            Log($"{count + 1}. {affichage}");
                            count++;
                        }
                    }
                }
                catch { /* Pas de résultats */ }
            }
            catch (Exception ex)
            {
                Log($"Erreur Amazon : {ex.Message}");
            }
        });

        return resultats;
    }

    /// <summary>
    /// Recherche les actualités sur un sujet.
    /// </summary>
    public async Task<List<string>> RechercheActualites(string sujet)
    {
        var resultats = new List<string>();

        await Task.Run(async () =>
        {
            if (!await InitialiserNavigateur() || _driver == null)
                return;

            try
            {
                Log($"Recherche actualités : {sujet}");
                _driver.Navigate().GoToUrl($"https://news.google.com/search?q={Uri.EscapeDataString(sujet)}&hl=fr&gl=FR");
                await Task.Delay(2000);

                Log($"Actualités sur '{sujet}' !");

                // Extraire les titres
                try
                {
                    var articles = _driver.FindElements(By.CssSelector("article h3, article h4"));
                    Log("=== DERNIÈRES ACTUALITÉS ===");

                    int count = 0;
                    foreach (var article in articles)
                    {
                        if (count >= 5) break;
                        var texte = article.Text;
                        if (!string.IsNullOrEmpty(texte))
                        {
                            var affichage = texte.Length > 50 ? texte[..50] + "..." : texte;
                            resultats.Add(affichage);
                            Log($"{count + 1}. {affichage}");
                            count++;
                        }
                    }
                }
                catch { /* Pas d'articles */ }
            }
            catch (Exception ex)
            {
                Log($"Erreur actualités : {ex.Message}");
            }
        });

        return resultats;
    }

    /// <summary>
    /// Recherche un article sur Wikipedia.
    /// </summary>
    public async Task<string> RechercheWikipedia(string article)
    {
        string extrait = "";

        await Task.Run(async () =>
        {
            if (!await InitialiserNavigateur() || _driver == null)
                return;

            try
            {
                Log($"Recherche Wikipedia : {article}");
                _driver.Navigate().GoToUrl($"https://fr.wikipedia.org/wiki/{Uri.EscapeDataString(article.Replace(" ", "_"))}");
                await Task.Delay(2000);

                // Extraire le premier paragraphe
                try
                {
                    var paragraphe = _driver.FindElement(
                        By.CssSelector("#mw-content-text .mw-parser-output > p:not(.mw-empty-elt)"));

                    extrait = paragraphe.Text;
                    if (extrait.Length > 200)
                        extrait = extrait[..200] + "...";

                    Log("=== EXTRAIT WIKIPEDIA ===");
                    Log(extrait);
                }
                catch
                {
                    Log($"Article '{article}' ouvert sur Wikipedia !");
                }
            }
            catch (Exception ex)
            {
                Log($"Erreur Wikipedia : {ex.Message}");
            }
        });

        return extrait;
    }

    /// <summary>
    /// Libère les ressources.
    /// </summary>
    public void Dispose()
    {
        FermerNavigateur();
        GC.SuppressFinalize(this);
    }

#else
    // Version stub pour les plateformes non-Windows
    public event Action<string>? OnLog;
    public string RechercheParDefaut { get; set; } = "Programmation C#";
    public string VilleParDefaut { get; set; } = "Paris";

    private void Log(string message) => OnLog?.Invoke($"[{DateTime.Now:HH:mm:ss}] {message}");

    public Task<bool> InitialiserNavigateur()
    {
        Log("Selenium n'est disponible que sur Windows.");
        return Task.FromResult(false);
    }

    public void FermerNavigateur() => Log("Navigateur non disponible.");
    public Task RechercheGoogle(string texte) => Task.Run(() => Log("Disponible uniquement sur Windows."));
    public Task VoirMeteo(string ville) => Task.Run(() => Log("Disponible uniquement sur Windows."));
    public Task RechercheCinema(string ville) => Task.Run(() => Log("Disponible uniquement sur Windows."));
    public Task RechercheRestaurants(string ville) => Task.Run(() => Log("Disponible uniquement sur Windows."));
    public Task RecherchePersonnalisee(string typeLieu, string ville) => Task.Run(() => Log("Disponible uniquement sur Windows."));
    public Task<List<string>> TendancesYouTube() => Task.FromResult(new List<string>());
    public Task<List<string>> VerifierProduit(string produit) => Task.FromResult(new List<string>());
    public Task<List<string>> RechercheActualites(string sujet) => Task.FromResult(new List<string>());
    public Task<string> RechercheWikipedia(string article) => Task.FromResult("");
    public void Dispose() { }
#endif
}
