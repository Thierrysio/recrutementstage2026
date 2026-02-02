# Atelier Selenium - Automatisation de Navigateur Web

Bienvenue dans cet atelier de programmation ! Vous allez apprendre à automatiser un navigateur web avec Python et Selenium.

## Installation

### 1. Installer Python
Si ce n'est pas déjà fait, téléchargez Python sur [python.org](https://www.python.org/downloads/)

### 2. Installer les dépendances
Ouvrez un terminal et tapez :
```bash
cd selenium_automation
pip install -r requirements.txt
```

### 3. Lancer l'application
```bash
python navigateur_auto.py
```

---

## Activité 1 : Personnalisation (20 min)

### Objectif
Modifier le texte de recherche par défaut et ajouter votre ville pour la météo.

### Instructions
1. Ouvrez le fichier `navigateur_auto.py` dans votre éditeur
2. Trouvez les lignes 52-53 :
```python
self.recherche_defaut = "Python programming"  # Changez ce texte !
self.ville_meteo = "Paris"  # Mettez votre ville ici !
```
3. Remplacez par vos propres valeurs !
4. Relancez l'application et testez

### Exemple
```python
self.recherche_defaut = "jeux vidéo 2026"
self.ville_meteo = "Lyon"
```

---

## Activité 2 : Nouveau Bouton (25 min)

### Objectif
Créer un bouton qui recherche automatiquement un nouveau type de lieu.

### Instructions
1. Trouvez la fonction `recherche_restaurants` (ligne ~390)
2. Copiez-la et créez une nouvelle fonction, par exemple :

```python
def recherche_bowling(self):
    """Recherche les bowlings locaux."""
    def tache():
        if not self.initialiser_navigateur():
            return

        ville = self.entry_ville_locale.get()
        self.log(f"Recherche bowling à : {ville}")

        try:
            self.driver.get(f"https://www.google.com/search?q=bowling+{ville}")
            self.log(f"Bowlings à {ville} trouvés !")

        except Exception as e:
            self.log(f"Erreur : {str(e)}")

    threading.Thread(target=tache, daemon=True).start()
```

3. Ajoutez un bouton dans l'interface (dans `creer_onglet_activite2`) :
```python
btn_bowling = ttk.Button(
    frame_boutons,
    text="Bowling",
    command=self.recherche_bowling
)
btn_bowling.pack(side="left", padx=10)
```

### Idées de boutons
- Escape game
- Piscine
- Laser game
- Karaoké
- Musée

---

## Activité 3 : Challenge (20 min)

### Objectif
Par groupes, choisissez et améliorez une fonctionnalité avancée.

### Challenge 1 : Tendances YouTube améliorées
Récupérez plus d'informations sur les vidéos tendances :
- Nombre de vues
- Nom de la chaîne
- Durée de la vidéo

```python
def tendances_youtube_avance(self):
    """Version améliorée des tendances YouTube."""
    def tache():
        if not self.initialiser_navigateur():
            return

        self.driver.get("https://www.youtube.com/feed/trending")
        time.sleep(3)

        # Récupérer les conteneurs de vidéos
        videos = self.driver.find_elements(By.CSS_SELECTOR, "ytd-video-renderer")[:5]

        for i, video in enumerate(videos, 1):
            try:
                titre = video.find_element(By.CSS_SELECTOR, "#video-title").text
                chaine = video.find_element(By.CSS_SELECTOR, "#channel-name").text
                vues = video.find_element(By.CSS_SELECTOR, "#metadata-line span").text
                self.log(f"{i}. {titre}")
                self.log(f"   Chaîne: {chaine} | {vues}")
            except:
                pass

    threading.Thread(target=tache, daemon=True).start()
```

### Challenge 2 : Alerte de prix
Créez un système qui vérifie si un produit est en dessous d'un certain prix :

```python
def alerte_prix(self, produit, prix_max):
    """Vérifie si un produit est disponible sous un certain prix."""
    # Votre code ici...
    # 1. Rechercher le produit sur Amazon
    # 2. Récupérer les prix
    # 3. Comparer avec prix_max
    # 4. Afficher une alerte si le prix est bon
```

### Challenge 3 : Surveillance d'actualités
Créez un système qui recherche des mots-clés dans les actualités :

```python
def surveillance_actu(self, mots_cles):
    """Surveille les actualités pour des mots-clés."""
    # Votre code ici...
    # 1. Aller sur Google Actualités
    # 2. Rechercher chaque mot-clé
    # 3. Récupérer les titres qui correspondent
    # 4. Sauvegarder dans un fichier
```

---

## Aide-mémoire Selenium

### Trouver des éléments
```python
# Par ID
element = driver.find_element(By.ID, "mon-id")

# Par nom de classe
element = driver.find_element(By.CLASS_NAME, "ma-classe")

# Par sélecteur CSS
element = driver.find_element(By.CSS_SELECTOR, "div.classe > p")

# Par texte du lien
element = driver.find_element(By.LINK_TEXT, "Cliquez ici")

# Plusieurs éléments
elements = driver.find_elements(By.CSS_SELECTOR, ".item")
```

### Actions courantes
```python
# Cliquer
element.click()

# Écrire du texte
element.send_keys("Mon texte")

# Appuyer sur Entrée
element.send_keys(Keys.RETURN)

# Récupérer le texte
texte = element.text

# Récupérer un attribut
href = element.get_attribute("href")

# Attendre un élément
element = WebDriverWait(driver, 10).until(
    EC.presence_of_element_located((By.ID, "mon-id"))
)
```

### Navigation
```python
# Aller sur une page
driver.get("https://www.google.com")

# Page précédente
driver.back()

# Rafraîchir
driver.refresh()

# URL actuelle
url = driver.current_url
```

---

## Dépannage

### Le navigateur ne s'ouvre pas
- Vérifiez que Chrome est installé
- Réinstallez webdriver-manager : `pip install --upgrade webdriver-manager`

### Erreur "element not found"
- L'élément n'existe peut-être pas sur la page
- Ajoutez un `time.sleep(2)` avant de chercher l'élément
- Utilisez `WebDriverWait` pour attendre l'élément

### L'application freeze
- Les tâches longues sont exécutées en arrière-plan (threading)
- Attendez que le message apparaisse dans les logs

---

## Pour aller plus loin

- [Documentation Selenium Python](https://selenium-python.readthedocs.io/)
- [Sélecteurs CSS](https://www.w3schools.com/cssref/css_selectors.asp)
- [XPath Tutorial](https://www.w3schools.com/xml/xpath_intro.asp)

Bon atelier !
