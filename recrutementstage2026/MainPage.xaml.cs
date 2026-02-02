// =============================================================================
// ATELIER SELENIUM - CODE-BEHIND DE L'INTERFACE PRINCIPALE
// =============================================================================
// Ce fichier contient la logique de l'interface utilisateur.
// Il fait le lien entre les boutons de l'interface et le service Selenium.
// =============================================================================

using recrutementstage2026.Services;

namespace recrutementstage2026;

/// <summary>
/// Page principale de l'application d'automatisation Selenium.
/// Contient les gestionnaires d'événements pour les 3 activités.
/// </summary>
public partial class MainPage : ContentPage
{
    // Service Selenium pour l'automatisation du navigateur
    private readonly SeleniumService _seleniumService;

    // Couleurs pour les onglets
    private readonly Color _couleurActive = Color.FromArgb("#3498db");
    private readonly Color _couleurInactive = Color.FromArgb("#7f8c8d");

    /// <summary>
    /// Constructeur de la page principale.
    /// Initialise le service Selenium et configure les événements.
    /// </summary>
    public MainPage()
    {
        InitializeComponent();

        // Création du service Selenium
        _seleniumService = new SeleniumService();

        // Abonnement aux logs du service
        _seleniumService.OnLog += message =>
        {
            // Mise à jour de l'interface sur le thread principal
            MainThread.BeginInvokeOnMainThread(() =>
            {
                LabelLogs.Text += "\n" + message;
                // Scroll automatique vers le bas
                LogScrollView.ScrollToAsync(0, double.MaxValue, false);
            });
        };

        // Initialiser les champs avec les valeurs par défaut du service
        EntryRecherche.Text = _seleniumService.RechercheParDefaut;
        EntryVille.Text = _seleniumService.VilleParDefaut;
    }

    // =========================================================================
    // GESTION DES ONGLETS
    // =========================================================================

    /// <summary>
    /// Affiche l'onglet de l'activité 1.
    /// </summary>
    private void OnOnglet1Clicked(object? sender, EventArgs e)
    {
        AfficherOnglet(1);
    }

    /// <summary>
    /// Affiche l'onglet de l'activité 2.
    /// </summary>
    private void OnOnglet2Clicked(object? sender, EventArgs e)
    {
        AfficherOnglet(2);
    }

    /// <summary>
    /// Affiche l'onglet de l'activité 3.
    /// </summary>
    private void OnOnglet3Clicked(object? sender, EventArgs e)
    {
        AfficherOnglet(3);
    }

    /// <summary>
    /// Affiche l'onglet spécifié et met à jour les couleurs des boutons.
    /// </summary>
    /// <param name="numero">Numéro de l'onglet (1, 2 ou 3)</param>
    private void AfficherOnglet(int numero)
    {
        // Masquer tous les onglets
        Onglet1.IsVisible = false;
        Onglet2.IsVisible = false;
        Onglet3.IsVisible = false;

        // Réinitialiser les couleurs des boutons
        BtnOnglet1.BackgroundColor = _couleurInactive;
        BtnOnglet2.BackgroundColor = _couleurInactive;
        BtnOnglet3.BackgroundColor = _couleurInactive;

        // Afficher l'onglet sélectionné et activer son bouton
        switch (numero)
        {
            case 1:
                Onglet1.IsVisible = true;
                BtnOnglet1.BackgroundColor = _couleurActive;
                break;
            case 2:
                Onglet2.IsVisible = true;
                BtnOnglet2.BackgroundColor = _couleurActive;
                break;
            case 3:
                Onglet3.IsVisible = true;
                BtnOnglet3.BackgroundColor = _couleurActive;
                break;
        }
    }

    // =========================================================================
    // ACTIVITÉ 1 : PERSONNALISATION
    // Gestionnaires pour la recherche Google et la météo
    // =========================================================================

    /// <summary>
    /// Lance une recherche Google avec le texte entré.
    /// </summary>
    private async void OnRechercheClicked(object? sender, EventArgs e)
    {
        var texte = EntryRecherche.Text?.Trim();
        if (string.IsNullOrEmpty(texte))
        {
            await DisplayAlert("Attention", "Veuillez entrer un texte à rechercher.", "OK");
            return;
        }

        await _seleniumService.RechercheGoogle(texte);
    }

    /// <summary>
    /// Affiche la météo pour la ville entrée.
    /// </summary>
    private async void OnMeteoClicked(object? sender, EventArgs e)
    {
        var ville = EntryVille.Text?.Trim();
        if (string.IsNullOrEmpty(ville))
        {
            await DisplayAlert("Attention", "Veuillez entrer une ville.", "OK");
            return;
        }

        await _seleniumService.VoirMeteo(ville);
    }

    // =========================================================================
    // ACTIVITÉ 2 : CINÉMA ET RESTAURANTS
    // Gestionnaires pour la recherche de lieux
    // =========================================================================

    /// <summary>
    /// Recherche les cinémas dans la ville.
    /// </summary>
    private async void OnCinemaClicked(object? sender, EventArgs e)
    {
        var ville = EntryVilleLocale.Text?.Trim();
        if (string.IsNullOrEmpty(ville))
        {
            await DisplayAlert("Attention", "Veuillez entrer une ville.", "OK");
            return;
        }

        await _seleniumService.RechercheCinema(ville);
    }

    /// <summary>
    /// Recherche les restaurants dans la ville.
    /// </summary>
    private async void OnRestaurantsClicked(object? sender, EventArgs e)
    {
        var ville = EntryVilleLocale.Text?.Trim();
        if (string.IsNullOrEmpty(ville))
        {
            await DisplayAlert("Attention", "Veuillez entrer une ville.", "OK");
            return;
        }

        await _seleniumService.RechercheRestaurants(ville);
    }

    /// <summary>
    /// Recherche un type de lieu personnalisé.
    /// </summary>
    private async void OnRecherchePersonnaliseeClicked(object? sender, EventArgs e)
    {
        var ville = EntryVilleLocale.Text?.Trim();
        var typeLieu = EntryTypeLieu.Text?.Trim();

        if (string.IsNullOrEmpty(ville) || string.IsNullOrEmpty(typeLieu))
        {
            await DisplayAlert("Attention", "Veuillez remplir tous les champs.", "OK");
            return;
        }

        await _seleniumService.RecherchePersonnalisee(typeLieu, ville);
    }

    // =========================================================================
    // ACTIVITÉ 3 : CHALLENGES
    // Gestionnaires pour les fonctionnalités avancées
    // =========================================================================

    /// <summary>
    /// Récupère les tendances YouTube.
    /// </summary>
    private async void OnYouTubeClicked(object? sender, EventArgs e)
    {
        await _seleniumService.TendancesYouTube();
    }

    /// <summary>
    /// Vérifie un produit sur Amazon.
    /// </summary>
    private async void OnProduitClicked(object? sender, EventArgs e)
    {
        var produit = EntryProduit.Text?.Trim();
        if (string.IsNullOrEmpty(produit))
        {
            await DisplayAlert("Attention", "Veuillez entrer un nom de produit.", "OK");
            return;
        }

        await _seleniumService.VerifierProduit(produit);
    }

    /// <summary>
    /// Recherche les actualités sur un sujet.
    /// </summary>
    private async void OnActuClicked(object? sender, EventArgs e)
    {
        var sujet = EntryActu.Text?.Trim();
        if (string.IsNullOrEmpty(sujet))
        {
            await DisplayAlert("Attention", "Veuillez entrer un sujet.", "OK");
            return;
        }

        await _seleniumService.RechercheActualites(sujet);
    }

    /// <summary>
    /// Recherche un article sur Wikipedia.
    /// </summary>
    private async void OnWikiClicked(object? sender, EventArgs e)
    {
        var article = EntryWiki.Text?.Trim();
        if (string.IsNullOrEmpty(article))
        {
            await DisplayAlert("Attention", "Veuillez entrer un article.", "OK");
            return;
        }

        await _seleniumService.RechercheWikipedia(article);
    }

    // =========================================================================
    // GESTION DU NAVIGATEUR
    // =========================================================================

    /// <summary>
    /// Ferme le navigateur Chrome.
    /// </summary>
    private void OnFermerClicked(object? sender, EventArgs e)
    {
        _seleniumService.FermerNavigateur();
    }

    /// <summary>
    /// Appelé lors de la fermeture de la page.
    /// Libère les ressources du service Selenium.
    /// </summary>
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _seleniumService.Dispose();
    }
}
