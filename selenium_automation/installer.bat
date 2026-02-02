@echo off
echo ===========================================
echo   INSTALLATION - Atelier Selenium
echo ===========================================
echo.

echo [1/2] Installation des dependances Python...
pip install -r requirements.txt

echo.
echo [2/2] Verification de l'installation...
python -c "import selenium; print('Selenium installe avec succes!')"

echo.
echo ===========================================
echo   Installation terminee !
echo   Lancez l'application avec: python navigateur_auto.py
echo ===========================================
pause
