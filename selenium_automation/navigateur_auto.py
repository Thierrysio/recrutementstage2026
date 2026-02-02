#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
=============================================================================
ATELIER SELENIUM - AUTOMATISATION DE NAVIGATEUR WEB
=============================================================================
Application d'automatisation de navigateur pour les lycéens.

Activités :
- Activité 1 : Personnalisation (recherche + météo)
- Activité 2 : Nouveau bouton (cinéma/restaurants)
- Activité 3 : Challenge (YouTube, produits, actualités)

Auteur : Atelier Stage 2026
=============================================================================
"""

import tkinter as tk
from tkinter import ttk, messagebox, scrolledtext
import threading
import time

# Selenium imports
from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.chrome.service import Service
from selenium.webdriver.chrome.options import Options
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.common.exceptions import TimeoutException, NoSuchElementException
from webdriver_manager.chrome import ChromeDriverManager


class NavigateurAutomatique:
    """
    Classe principale pour l'automatisation du navigateur.
    Cette classe contient toutes les fonctionnalités de l'atelier.
    """

    def __init__(self, root):
        """Initialise l'application et l'interface graphique."""
        self.root = root
        self.root.title("Atelier Selenium - Automatisation Web")
        self.root.geometry("900x700")
        self.root.configure(bg="#2c3e50")

        # Variable pour le navigateur
        self.driver = None

        # =================================================================
        # ACTIVITÉ 1 : PERSONNALISATION
        # Modifiez ces valeurs par défaut !
        # =================================================================
        self.recherche_defaut = "Python programming"  # Changez ce texte !
        self.ville_meteo = "Paris"  # Mettez votre ville ici !

        # Configuration de l'interface
        self.creer_interface()

    def creer_interface(self):
        """Crée l'interface graphique complète."""

        # Style
        style = ttk.Style()
        style.configure("TButton", font=("Arial", 11), padding=10)
        style.configure("TLabel", font=("Arial", 11), background="#2c3e50", foreground="white")
        style.configure("TEntry", font=("Arial", 11))
        style.configure("Header.TLabel", font=("Arial", 16, "bold"), background="#2c3e50", foreground="#3498db")

        # Titre principal
        titre = ttk.Label(
            self.root,
            text="Atelier Selenium - Automatisation de Navigateur",
            style="Header.TLabel"
        )
        titre.pack(pady=20)

        # Frame principal avec onglets
        self.notebook = ttk.Notebook(self.root)
        self.notebook.pack(fill="both", expand=True, padx=20, pady=10)

        # Création des onglets
        self.creer_onglet_activite1()
        self.creer_onglet_activite2()
        self.creer_onglet_activite3()

        # Zone de logs
        self.creer_zone_logs()

        # Bouton fermer navigateur
        btn_fermer = ttk.Button(
            self.root,
            text="Fermer le navigateur",
            command=self.fermer_navigateur
        )
        btn_fermer.pack(pady=10)

    def creer_onglet_activite1(self):
        """
        =================================================================
        ACTIVITÉ 1 : PERSONNALISATION (20 min)
        =================================================================
        Les lycéens modifient le texte de recherche et la ville météo.
        """
        frame = ttk.Frame(self.notebook, padding=20)
        self.notebook.add(frame, text="Activité 1 : Personnalisation")

        # Section Recherche Google
        ttk.Label(frame, text="RECHERCHE GOOGLE", font=("Arial", 14, "bold")).pack(anchor="w", pady=(0, 10))

        frame_recherche = ttk.Frame(frame)
        frame_recherche.pack(fill="x", pady=5)

        ttk.Label(frame_recherche, text="Texte à rechercher :").pack(side="left")
        self.entry_recherche = ttk.Entry(frame_recherche, width=40)
        self.entry_recherche.insert(0, self.recherche_defaut)
        self.entry_recherche.pack(side="left", padx=10)

        btn_recherche = ttk.Button(
            frame_recherche,
            text="Lancer la recherche",
            command=self.recherche_google
        )
        btn_recherche.pack(side="left")

        # Séparateur
        ttk.Separator(frame, orient="horizontal").pack(fill="x", pady=20)

        # Section Météo
        ttk.Label(frame, text="MÉTÉO", font=("Arial", 14, "bold")).pack(anchor="w", pady=(0, 10))

        frame_meteo = ttk.Frame(frame)
        frame_meteo.pack(fill="x", pady=5)

        ttk.Label(frame_meteo, text="Votre ville :").pack(side="left")
        self.entry_ville = ttk.Entry(frame_meteo, width=30)
        self.entry_ville.insert(0, self.ville_meteo)
        self.entry_ville.pack(side="left", padx=10)

        btn_meteo = ttk.Button(
            frame_meteo,
            text="Voir la météo",
            command=self.voir_meteo
        )
        btn_meteo.pack(side="left")

        # Instructions pour les élèves
        instructions = """
        INSTRUCTIONS POUR L'ACTIVITÉ 1 :

        1. Modifiez le texte de recherche par défaut dans le code (ligne 52)
        2. Remplacez "Paris" par votre ville (ligne 53)
        3. Testez les boutons pour voir le résultat !

        Astuce : Vous pouvez aussi modifier directement les champs ci-dessus.
        """
        ttk.Label(frame, text=instructions, font=("Arial", 10), foreground="#7f8c8d").pack(pady=20)

    def creer_onglet_activite2(self):
        """
        =================================================================
        ACTIVITÉ 2 : NOUVEAU BOUTON (25 min)
        =================================================================
        Créer un bouton pour rechercher cinéma ou restaurants.
        """
        frame = ttk.Frame(self.notebook, padding=20)
        self.notebook.add(frame, text="Activité 2 : Cinéma & Restaurants")

        ttk.Label(frame, text="RECHERCHE LOCALE", font=("Arial", 14, "bold")).pack(anchor="w", pady=(0, 10))

        # Ville pour la recherche locale
        frame_ville = ttk.Frame(frame)
        frame_ville.pack(fill="x", pady=5)

        ttk.Label(frame_ville, text="Ville :").pack(side="left")
        self.entry_ville_locale = ttk.Entry(frame_ville, width=30)
        self.entry_ville_locale.insert(0, "Paris")
        self.entry_ville_locale.pack(side="left", padx=10)

        # Boutons cinéma et restaurants
        frame_boutons = ttk.Frame(frame)
        frame_boutons.pack(fill="x", pady=20)

        btn_cinema = ttk.Button(
            frame_boutons,
            text="Horaires Cinéma",
            command=self.recherche_cinema
        )
        btn_cinema.pack(side="left", padx=10)

        btn_restaurants = ttk.Button(
            frame_boutons,
            text="Restaurants",
            command=self.recherche_restaurants
        )
        btn_restaurants.pack(side="left", padx=10)

        # Séparateur
        ttk.Separator(frame, orient="horizontal").pack(fill="x", pady=20)

        # Section recherche personnalisée
        ttk.Label(frame, text="RECHERCHE PERSONNALISÉE", font=("Arial", 14, "bold")).pack(anchor="w", pady=(0, 10))

        frame_perso = ttk.Frame(frame)
        frame_perso.pack(fill="x", pady=5)

        ttk.Label(frame_perso, text="Type de lieu :").pack(side="left")
        self.entry_type_lieu = ttk.Entry(frame_perso, width=30)
        self.entry_type_lieu.insert(0, "pizzeria")
        self.entry_type_lieu.pack(side="left", padx=10)

        btn_perso = ttk.Button(
            frame_perso,
            text="Rechercher",
            command=self.recherche_personnalisee
        )
        btn_perso.pack(side="left")

        # Instructions
        instructions = """
        INSTRUCTIONS POUR L'ACTIVITÉ 2 :

        1. Entrez votre ville dans le champ
        2. Cliquez sur "Horaires Cinéma" ou "Restaurants"
        3. Créez votre propre bouton en modifiant le code !

        DÉFI : Ajoutez un bouton pour rechercher autre chose
        (bowling, escape game, piscine, etc.)
        """
        ttk.Label(frame, text=instructions, font=("Arial", 10), foreground="#7f8c8d").pack(pady=20)

    def creer_onglet_activite3(self):
        """
        =================================================================
        ACTIVITÉ 3 : CHALLENGE (20 min)
        =================================================================
        Fonctionnalités avancées pour les groupes.
        """
        frame = ttk.Frame(self.notebook, padding=20)
        self.notebook.add(frame, text="Activité 3 : Challenge")

        ttk.Label(frame, text="FONCTIONNALITÉS AVANCÉES", font=("Arial", 14, "bold")).pack(anchor="w", pady=(0, 10))

        # Challenge 1 : YouTube Tendances
        frame_youtube = ttk.LabelFrame(frame, text="YouTube - Tendances", padding=10)
        frame_youtube.pack(fill="x", pady=10)

        btn_youtube = ttk.Button(
            frame_youtube,
            text="Voir les tendances YouTube",
            command=self.tendances_youtube
        )
        btn_youtube.pack(side="left", padx=10)

        # Challenge 2 : Vérifier disponibilité produit
        frame_produit = ttk.LabelFrame(frame, text="Vérification Produit", padding=10)
        frame_produit.pack(fill="x", pady=10)

        ttk.Label(frame_produit, text="Produit :").pack(side="left")
        self.entry_produit = ttk.Entry(frame_produit, width=30)
        self.entry_produit.insert(0, "iPhone 15")
        self.entry_produit.pack(side="left", padx=10)

        btn_produit = ttk.Button(
            frame_produit,
            text="Vérifier sur Amazon",
            command=self.verifier_produit
        )
        btn_produit.pack(side="left")

        # Challenge 3 : Actualités
        frame_actu = ttk.LabelFrame(frame, text="Actualités", padding=10)
        frame_actu.pack(fill="x", pady=10)

        ttk.Label(frame_actu, text="Sujet :").pack(side="left")
        self.entry_actu = ttk.Entry(frame_actu, width=30)
        self.entry_actu.insert(0, "technologie")
        self.entry_actu.pack(side="left", padx=10)

        btn_actu = ttk.Button(
            frame_actu,
            text="Rechercher actualités",
            command=self.recherche_actualites
        )
        btn_actu.pack(side="left")

        # Challenge 4 : Wikipedia
        frame_wiki = ttk.LabelFrame(frame, text="Wikipedia", padding=10)
        frame_wiki.pack(fill="x", pady=10)

        ttk.Label(frame_wiki, text="Article :").pack(side="left")
        self.entry_wiki = ttk.Entry(frame_wiki, width=30)
        self.entry_wiki.insert(0, "Intelligence artificielle")
        self.entry_wiki.pack(side="left", padx=10)

        btn_wiki = ttk.Button(
            frame_wiki,
            text="Rechercher sur Wikipedia",
            command=self.recherche_wikipedia
        )
        btn_wiki.pack(side="left")

        # Instructions
        instructions = """
        CHALLENGE POUR LES GROUPES :

        Choisissez une fonctionnalité et améliorez-la !
        - Récupérer les titres des vidéos tendances
        - Afficher le prix des produits trouvés
        - Créer une alerte pour un mot-clé dans les actualités
        - Extraire le résumé d'un article Wikipedia
        """
        ttk.Label(frame, text=instructions, font=("Arial", 10), foreground="#7f8c8d").pack(pady=10)

    def creer_zone_logs(self):
        """Crée la zone d'affichage des logs."""
        frame_logs = ttk.LabelFrame(self.root, text="Journal d'activité", padding=10)
        frame_logs.pack(fill="x", padx=20, pady=10)

        self.text_logs = scrolledtext.ScrolledText(
            frame_logs,
            height=6,
            font=("Consolas", 9),
            bg="#1e272e",
            fg="#00ff00"
        )
        self.text_logs.pack(fill="x")

    def log(self, message):
        """Ajoute un message dans les logs."""
        timestamp = time.strftime("%H:%M:%S")
        self.text_logs.insert(tk.END, f"[{timestamp}] {message}\n")
        self.text_logs.see(tk.END)

    def initialiser_navigateur(self):
        """Initialise le navigateur Chrome avec Selenium."""
        if self.driver is None:
            self.log("Initialisation du navigateur Chrome...")
            try:
                options = Options()
                # Options pour un meilleur fonctionnement
                options.add_argument("--start-maximized")
                options.add_argument("--disable-notifications")
                options.add_experimental_option("excludeSwitches", ["enable-logging"])

                # Initialisation automatique du driver
                service = Service(ChromeDriverManager().install())
                self.driver = webdriver.Chrome(service=service, options=options)
                self.log("Navigateur initialisé avec succès !")
                return True
            except Exception as e:
                self.log(f"Erreur : {str(e)}")
                messagebox.showerror("Erreur", f"Impossible d'initialiser le navigateur:\n{str(e)}")
                return False
        return True

    def fermer_navigateur(self):
        """Ferme le navigateur."""
        if self.driver:
            self.driver.quit()
            self.driver = None
            self.log("Navigateur fermé.")

    # =====================================================================
    # ACTIVITÉ 1 : FONCTIONS DE RECHERCHE ET MÉTÉO
    # =====================================================================

    def recherche_google(self):
        """Effectue une recherche sur Google."""
        def tache():
            if not self.initialiser_navigateur():
                return

            texte = self.entry_recherche.get()
            self.log(f"Recherche Google : {texte}")

            try:
                # Aller sur Google
                self.driver.get("https://www.google.com")
                time.sleep(1)

                # Accepter les cookies si nécessaire
                try:
                    btn_cookies = self.driver.find_element(By.ID, "L2AGLb")
                    btn_cookies.click()
                    time.sleep(0.5)
                except:
                    pass

                # Trouver la barre de recherche et taper le texte
                barre_recherche = WebDriverWait(self.driver, 10).until(
                    EC.presence_of_element_located((By.NAME, "q"))
                )
                barre_recherche.clear()
                barre_recherche.send_keys(texte)
                barre_recherche.send_keys(Keys.RETURN)

                self.log(f"Recherche '{texte}' effectuée !")

            except Exception as e:
                self.log(f"Erreur lors de la recherche : {str(e)}")

        threading.Thread(target=tache, daemon=True).start()

    def voir_meteo(self):
        """Affiche la météo pour une ville."""
        def tache():
            if not self.initialiser_navigateur():
                return

            ville = self.entry_ville.get()
            self.log(f"Recherche météo pour : {ville}")

            try:
                # Recherche météo sur Google
                self.driver.get(f"https://www.google.com/search?q=météo+{ville}")
                self.log(f"Météo de {ville} affichée !")

            except Exception as e:
                self.log(f"Erreur : {str(e)}")

        threading.Thread(target=tache, daemon=True).start()

    # =====================================================================
    # ACTIVITÉ 2 : FONCTIONS CINÉMA ET RESTAURANTS
    # =====================================================================

    def recherche_cinema(self):
        """Recherche les horaires de cinéma."""
        def tache():
            if not self.initialiser_navigateur():
                return

            ville = self.entry_ville_locale.get()
            self.log(f"Recherche cinémas à : {ville}")

            try:
                self.driver.get(f"https://www.google.com/search?q=cinéma+horaires+{ville}")
                self.log(f"Cinémas à {ville} trouvés !")

            except Exception as e:
                self.log(f"Erreur : {str(e)}")

        threading.Thread(target=tache, daemon=True).start()

    def recherche_restaurants(self):
        """Recherche les restaurants locaux."""
        def tache():
            if not self.initialiser_navigateur():
                return

            ville = self.entry_ville_locale.get()
            self.log(f"Recherche restaurants à : {ville}")

            try:
                self.driver.get(f"https://www.google.com/search?q=restaurants+{ville}")
                self.log(f"Restaurants à {ville} trouvés !")

            except Exception as e:
                self.log(f"Erreur : {str(e)}")

        threading.Thread(target=tache, daemon=True).start()

    def recherche_personnalisee(self):
        """Recherche un type de lieu personnalisé."""
        def tache():
            if not self.initialiser_navigateur():
                return

            ville = self.entry_ville_locale.get()
            type_lieu = self.entry_type_lieu.get()
            self.log(f"Recherche {type_lieu} à : {ville}")

            try:
                self.driver.get(f"https://www.google.com/search?q={type_lieu}+{ville}")
                self.log(f"{type_lieu} à {ville} trouvés !")

            except Exception as e:
                self.log(f"Erreur : {str(e)}")

        threading.Thread(target=tache, daemon=True).start()

    # =====================================================================
    # ACTIVITÉ 3 : FONCTIONS CHALLENGE
    # =====================================================================

    def tendances_youtube(self):
        """Récupère les tendances YouTube."""
        def tache():
            if not self.initialiser_navigateur():
                return

            self.log("Accès aux tendances YouTube...")

            try:
                self.driver.get("https://www.youtube.com/feed/trending")
                time.sleep(2)

                # Essayer de récupérer les titres des vidéos tendances
                try:
                    videos = self.driver.find_elements(By.CSS_SELECTOR, "#video-title")[:5]
                    self.log("=== TOP 5 TENDANCES YOUTUBE ===")
                    for i, video in enumerate(videos, 1):
                        titre = video.get_attribute("title") or video.text
                        if titre:
                            self.log(f"{i}. {titre[:50]}...")
                except:
                    self.log("Page tendances YouTube ouverte !")

            except Exception as e:
                self.log(f"Erreur : {str(e)}")

        threading.Thread(target=tache, daemon=True).start()

    def verifier_produit(self):
        """Vérifie la disponibilité d'un produit sur Amazon."""
        def tache():
            if not self.initialiser_navigateur():
                return

            produit = self.entry_produit.get()
            self.log(f"Recherche du produit : {produit}")

            try:
                # Recherche sur Amazon France
                self.driver.get(f"https://www.amazon.fr/s?k={produit.replace(' ', '+')}")
                time.sleep(2)

                self.log(f"Résultats pour '{produit}' affichés sur Amazon !")

                # Essayer d'extraire les premiers résultats
                try:
                    resultats = self.driver.find_elements(
                        By.CSS_SELECTOR,
                        "[data-component-type='s-search-result'] h2 a span"
                    )[:3]

                    if resultats:
                        self.log("=== PREMIERS RÉSULTATS ===")
                        for i, r in enumerate(resultats, 1):
                            self.log(f"{i}. {r.text[:40]}...")
                except:
                    pass

            except Exception as e:
                self.log(f"Erreur : {str(e)}")

        threading.Thread(target=tache, daemon=True).start()

    def recherche_actualites(self):
        """Recherche les actualités sur un sujet."""
        def tache():
            if not self.initialiser_navigateur():
                return

            sujet = self.entry_actu.get()
            self.log(f"Recherche actualités : {sujet}")

            try:
                # Recherche sur Google Actualités
                self.driver.get(f"https://news.google.com/search?q={sujet}&hl=fr&gl=FR")
                time.sleep(2)

                self.log(f"Actualités sur '{sujet}' affichées !")

                # Essayer d'extraire les titres
                try:
                    articles = self.driver.find_elements(By.CSS_SELECTOR, "article h3")[:5]
                    if articles:
                        self.log("=== DERNIÈRES ACTUALITÉS ===")
                        for i, article in enumerate(articles, 1):
                            self.log(f"{i}. {article.text[:50]}...")
                except:
                    pass

            except Exception as e:
                self.log(f"Erreur : {str(e)}")

        threading.Thread(target=tache, daemon=True).start()

    def recherche_wikipedia(self):
        """Recherche un article sur Wikipedia."""
        def tache():
            if not self.initialiser_navigateur():
                return

            article = self.entry_wiki.get()
            self.log(f"Recherche Wikipedia : {article}")

            try:
                # Accès direct à Wikipedia français
                self.driver.get(f"https://fr.wikipedia.org/wiki/{article.replace(' ', '_')}")
                time.sleep(2)

                # Essayer d'extraire le premier paragraphe
                try:
                    premier_paragraphe = self.driver.find_element(
                        By.CSS_SELECTOR,
                        "#mw-content-text .mw-parser-output > p:not(.mw-empty-elt)"
                    )
                    texte = premier_paragraphe.text[:200]
                    self.log(f"=== EXTRAIT ===")
                    self.log(texte + "...")
                except:
                    self.log(f"Article '{article}' ouvert sur Wikipedia !")

            except Exception as e:
                self.log(f"Erreur : {str(e)}")

        threading.Thread(target=tache, daemon=True).start()


# =========================================================================
# POINT D'ENTRÉE DE L'APPLICATION
# =========================================================================

def main():
    """Fonction principale pour lancer l'application."""
    root = tk.Tk()
    app = NavigateurAutomatique(root)

    # Fermer proprement le navigateur à la fermeture de l'app
    def on_closing():
        app.fermer_navigateur()
        root.destroy()

    root.protocol("WM_DELETE_WINDOW", on_closing)
    root.mainloop()


if __name__ == "__main__":
    main()
