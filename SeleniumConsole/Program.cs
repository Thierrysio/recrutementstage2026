// =============================================================================
// ATELIER SELENIUM - APPLICATION CONSOLE
// =============================================================================
// Application d'automatisation de navigateur pour les lycéens.
// Version Console (contourne les restrictions de sécurité MAUI)
// =============================================================================

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumConsole;

class Program
{
    // =========================================================================
    // ACTIVITÉ 1 : PERSONNALISATION
    // Modifiez ces valeurs par défaut !
    // =========================================================================
    static string RechercheParDefaut = "Programmation C#";  // CHANGEZ-MOI !
    static string VilleParDefaut = "Paris";                  // METTEZ VOTRE VILLE !

    static IWebDriver? driver;

    static void Main(string[] args)
    {
        Console.Title = "Atelier Selenium - Automatisation Web";
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(@"
╔═══════════════════════════════════════════════════════════════╗
║         ATELIER SELENIUM - AUTOMATISATION WEB                 ║
║                   Version Console                             ║
╚═══════════════════════════════════════════════════════════════╝
");
        Console.ResetColor();

        bool continuer = true;
        while (continuer)
        {
            AfficherMenu();
            var choix = Console.ReadLine()?.Trim();

            switch (choix)
            {
                // Activité 1
                case "1":
                    RechercheGoogle();
                    break;
                case "2":
                    VoirMeteo();
                    break;

                // Activité 2
                case "3":
                    RechercheCinema();
                    break;
                case "4":
                    RechercheRestaurants();
                    break;
                case "5":
                    RecherchePersonnalisee();
                    break;

                // Activité 3
                case "6":
                    TendancesYouTube();
                    break;
                case "7":
                    VerifierProduit();
                    break;
                case "8":
                    RechercheActualites();
                    break;
                case "9":
                    RechercheWikipedia();
                    break;

                // Utilitaires
                case "0":
                    FermerNavigateur();
                    continuer = false;
                    break;
                case "f":
                case "F":
                    FermerNavigateur();
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Choix invalide. Réessayez.");
                    Console.ResetColor();
                    break;
            }

            if (continuer)
            {
                Console.WriteLine("\nAppuyez sur Entrée pour continuer...");
                Console.ReadLine();
                Console.Clear();
            }
        }

        Console.WriteLine("Au revoir !");
    }

    static void AfficherMenu()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine("                         MENU PRINCIPAL");
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n  ACTIVITÉ 1 : Personnalisation");
        Console.ResetColor();
        Console.WriteLine("    [1] Recherche Google");
        Console.WriteLine("    [2] Voir la météo");

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\n  ACTIVITÉ 2 : Cinéma & Restaurants");
        Console.ResetColor();
        Console.WriteLine("    [3] Horaires cinéma");
        Console.WriteLine("    [4] Restaurants");
        Console.WriteLine("    [5] Recherche personnalisée");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n  ACTIVITÉ 3 : Challenge");
        Console.ResetColor();
        Console.WriteLine("    [6] Tendances YouTube");
        Console.WriteLine("    [7] Vérifier produit (Amazon)");
        Console.WriteLine("    [8] Actualités");
        Console.WriteLine("    [9] Wikipedia");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\n  [F] Fermer le navigateur    [0] Quitter");
        Console.ResetColor();

        Console.Write("\n  Votre choix : ");
    }

    // =========================================================================
    // INITIALISATION DU NAVIGATEUR
    // =========================================================================
    static bool InitialiserNavigateur()
    {
        if (driver != null)
            return true;

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[INFO] Initialisation du navigateur Chrome...");
        Console.ResetColor();

        try
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--disable-popup-blocking");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddExcludedArgument("enable-logging");

            // Chercher ChromeDriver
            string? cheminDriver = TrouverChromeDriver();

            if (cheminDriver != null)
            {
                Console.WriteLine($"[INFO] ChromeDriver trouvé : {cheminDriver}");
                var service = ChromeDriverService.CreateDefaultService(cheminDriver);
                service.HideCommandPromptWindow = true;
                driver = new ChromeDriver(service, options);
            }
            else
            {
                Console.WriteLine("[INFO] Recherche dans le PATH système...");
                var service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;
                driver = new ChromeDriver(service, options);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[OK] Navigateur initialisé avec succès !");
            Console.ResetColor();
            return true;
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[ERREUR] {ex.Message}");
            Console.WriteLine("\n═══════════════════════════════════════════════════════════════");
            Console.WriteLine("  SOLUTION : Téléchargez ChromeDriver");
            Console.WriteLine("═══════════════════════════════════════════════════════════════");
            Console.ResetColor();
            Console.WriteLine("1. Allez sur : https://googlechromelabs.github.io/chrome-for-testing/");
            Console.WriteLine("2. Téléchargez la version correspondant à votre Chrome");
            Console.WriteLine("3. Extrayez chromedriver.exe dans le même dossier que ce programme");
            Console.WriteLine("4. Relancez le programme");
            return false;
        }
    }

    static string? TrouverChromeDriver()
    {
        var emplacements = new[]
        {
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "chromedriver.exe"),
            "chromedriver.exe",
            @"C:\Selenium\chromedriver.exe",
            @"C:\Tools\chromedriver.exe",
            @"C:\chromedriver\chromedriver.exe",
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "chromedriver.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "chromedriver-win64", "chromedriver.exe"),
        };

        foreach (var chemin in emplacements)
        {
            if (File.Exists(chemin))
                return Path.GetDirectoryName(Path.GetFullPath(chemin));
        }

        return null;
    }

    static void FermerNavigateur()
    {
        if (driver != null)
        {
            try { driver.Quit(); } catch { }
            try { driver.Dispose(); } catch { }
            driver = null;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[INFO] Navigateur fermé.");
            Console.ResetColor();
        }
    }

    // =========================================================================
    // ACTIVITÉ 1 : RECHERCHE GOOGLE ET MÉTÉO
    // =========================================================================
    static void RechercheGoogle()
    {
        Console.Write($"\nTexte à rechercher [{RechercheParDefaut}] : ");
        var texte = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(texte))
            texte = RechercheParDefaut;

        if (!InitialiserNavigateur() || driver == null) return;

        try
        {
            Console.WriteLine($"\n[ACTION] Recherche Google : {texte}");

            driver.Navigate().GoToUrl("https://www.google.com");
            Thread.Sleep(1000);

            // Accepter les cookies
            try
            {
                driver.FindElement(By.Id("L2AGLb")).Click();
                Thread.Sleep(500);
            }
            catch { }

            // Rechercher
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var barre = wait.Until(d => d.FindElement(By.Name("q")));
            barre.Clear();
            barre.SendKeys(texte);
            barre.SendKeys(Keys.Return);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[OK] Recherche '{texte}' effectuée !");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERREUR] {ex.Message}");
            Console.ResetColor();
        }
    }

    static void VoirMeteo()
    {
        Console.Write($"\nVille [{VilleParDefaut}] : ");
        var ville = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(ville))
            ville = VilleParDefaut;

        if (!InitialiserNavigateur() || driver == null) return;

        try
        {
            Console.WriteLine($"\n[ACTION] Météo pour : {ville}");
            driver.Navigate().GoToUrl($"https://www.google.com/search?q=météo+{Uri.EscapeDataString(ville)}");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[OK] Météo de {ville} affichée !");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERREUR] {ex.Message}");
            Console.ResetColor();
        }
    }

    // =========================================================================
    // ACTIVITÉ 2 : CINÉMA ET RESTAURANTS
    // =========================================================================
    static void RechercheCinema()
    {
        Console.Write($"\nVille [{VilleParDefaut}] : ");
        var ville = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(ville))
            ville = VilleParDefaut;

        if (!InitialiserNavigateur() || driver == null) return;

        try
        {
            Console.WriteLine($"\n[ACTION] Recherche cinémas à : {ville}");
            driver.Navigate().GoToUrl($"https://www.google.com/search?q=cinéma+horaires+{Uri.EscapeDataString(ville)}");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[OK] Cinémas à {ville} trouvés !");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERREUR] {ex.Message}");
            Console.ResetColor();
        }
    }

    static void RechercheRestaurants()
    {
        Console.Write($"\nVille [{VilleParDefaut}] : ");
        var ville = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(ville))
            ville = VilleParDefaut;

        if (!InitialiserNavigateur() || driver == null) return;

        try
        {
            Console.WriteLine($"\n[ACTION] Recherche restaurants à : {ville}");
            driver.Navigate().GoToUrl($"https://www.google.com/search?q=restaurants+{Uri.EscapeDataString(ville)}");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[OK] Restaurants à {ville} trouvés !");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERREUR] {ex.Message}");
            Console.ResetColor();
        }
    }

    static void RecherchePersonnalisee()
    {
        Console.Write("\nType de lieu (pizzeria, bowling, escape game...) : ");
        var typeLieu = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(typeLieu))
            typeLieu = "pizzeria";

        Console.Write($"Ville [{VilleParDefaut}] : ");
        var ville = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(ville))
            ville = VilleParDefaut;

        if (!InitialiserNavigateur() || driver == null) return;

        try
        {
            Console.WriteLine($"\n[ACTION] Recherche {typeLieu} à : {ville}");
            driver.Navigate().GoToUrl($"https://www.google.com/search?q={Uri.EscapeDataString(typeLieu)}+{Uri.EscapeDataString(ville)}");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[OK] {typeLieu} à {ville} trouvés !");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERREUR] {ex.Message}");
            Console.ResetColor();
        }
    }

    // =========================================================================
    // ACTIVITÉ 3 : CHALLENGES
    // =========================================================================
    static void TendancesYouTube()
    {
        if (!InitialiserNavigateur() || driver == null) return;

        try
        {
            Console.WriteLine("\n[ACTION] Accès aux tendances YouTube...");
            driver.Navigate().GoToUrl("https://www.youtube.com/feed/trending");
            Thread.Sleep(3000);

            try
            {
                var videos = driver.FindElements(By.CssSelector("#video-title"));
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n═══════════════════════════════════════════════════════════════");
                Console.WriteLine("                  TOP 5 TENDANCES YOUTUBE");
                Console.WriteLine("═══════════════════════════════════════════════════════════════");
                Console.ResetColor();

                int count = 0;
                foreach (var video in videos)
                {
                    if (count >= 5) break;
                    var titre = video.GetAttribute("title");
                    if (!string.IsNullOrEmpty(titre))
                    {
                        Console.WriteLine($"  {count + 1}. {(titre.Length > 60 ? titre[..60] + "..." : titre)}");
                        count++;
                    }
                }
            }
            catch
            {
                Console.WriteLine("[INFO] Page tendances YouTube ouverte !");
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERREUR] {ex.Message}");
            Console.ResetColor();
        }
    }

    static void VerifierProduit()
    {
        Console.Write("\nNom du produit : ");
        var produit = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(produit))
            produit = "iPhone 15";

        if (!InitialiserNavigateur() || driver == null) return;

        try
        {
            Console.WriteLine($"\n[ACTION] Recherche du produit : {produit}");
            driver.Navigate().GoToUrl($"https://www.amazon.fr/s?k={Uri.EscapeDataString(produit)}");
            Thread.Sleep(2000);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[OK] Résultats pour '{produit}' sur Amazon !");
            Console.ResetColor();

            try
            {
                var elements = driver.FindElements(By.CssSelector("[data-component-type='s-search-result'] h2 a span"));
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n═══════════════════════════════════════════════════════════════");
                Console.WriteLine("                    PREMIERS RÉSULTATS");
                Console.WriteLine("═══════════════════════════════════════════════════════════════");
                Console.ResetColor();

                int count = 0;
                foreach (var elem in elements)
                {
                    if (count >= 3) break;
                    var texte = elem.Text;
                    if (!string.IsNullOrEmpty(texte))
                    {
                        Console.WriteLine($"  {count + 1}. {(texte.Length > 50 ? texte[..50] + "..." : texte)}");
                        count++;
                    }
                }
            }
            catch { }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERREUR] {ex.Message}");
            Console.ResetColor();
        }
    }

    static void RechercheActualites()
    {
        Console.Write("\nSujet d'actualité : ");
        var sujet = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(sujet))
            sujet = "technologie";

        if (!InitialiserNavigateur() || driver == null) return;

        try
        {
            Console.WriteLine($"\n[ACTION] Recherche actualités : {sujet}");
            driver.Navigate().GoToUrl($"https://news.google.com/search?q={Uri.EscapeDataString(sujet)}&hl=fr&gl=FR");
            Thread.Sleep(2000);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[OK] Actualités sur '{sujet}' !");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERREUR] {ex.Message}");
            Console.ResetColor();
        }
    }

    static void RechercheWikipedia()
    {
        Console.Write("\nArticle Wikipedia : ");
        var article = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(article))
            article = "Intelligence artificielle";

        if (!InitialiserNavigateur() || driver == null) return;

        try
        {
            Console.WriteLine($"\n[ACTION] Recherche Wikipedia : {article}");
            driver.Navigate().GoToUrl($"https://fr.wikipedia.org/wiki/{Uri.EscapeDataString(article.Replace(" ", "_"))}");
            Thread.Sleep(2000);

            try
            {
                var paragraphe = driver.FindElement(By.CssSelector("#mw-content-text .mw-parser-output > p:not(.mw-empty-elt)"));
                var extrait = paragraphe.Text;
                if (extrait.Length > 300)
                    extrait = extrait[..300] + "...";

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n═══════════════════════════════════════════════════════════════");
                Console.WriteLine("                    EXTRAIT WIKIPEDIA");
                Console.WriteLine("═══════════════════════════════════════════════════════════════");
                Console.ResetColor();
                Console.WriteLine(extrait);
            }
            catch
            {
                Console.WriteLine($"[INFO] Article '{article}' ouvert sur Wikipedia !");
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERREUR] {ex.Message}");
            Console.ResetColor();
        }
    }
}
