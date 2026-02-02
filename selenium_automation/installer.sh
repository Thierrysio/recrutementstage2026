#!/bin/bash
echo "==========================================="
echo "  INSTALLATION - Atelier Selenium"
echo "==========================================="
echo

echo "[1/2] Installation des dépendances Python..."
pip3 install -r requirements.txt

echo
echo "[2/2] Vérification de l'installation..."
python3 -c "import selenium; print('Selenium installé avec succès!')"

echo
echo "==========================================="
echo "  Installation terminée !"
echo "  Lancez l'application avec: python3 navigateur_auto.py"
echo "==========================================="
