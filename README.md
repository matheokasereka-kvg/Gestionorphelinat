# Gestionorphelinat

Application Laravel 12 pour la gestion d’un orphelinat.

## Prérequis

- PHP 8.2 ou plus récent avec les extensions usuelles de Laravel (`openssl`, `pdo`, `mbstring`, `xml`, `curl`, etc.).
- Composer.
- Node.js et npm pour Vite.

## Installation locale

```bash
composer setup
```

La commande installe les dépendances PHP, crée `.env` depuis `.env.example` si nécessaire, crée la base SQLite locale `database/database.sqlite`, génère la clé Laravel, lance les migrations, puis installe et compile les assets front-end.

Si vous préférez faire les étapes manuellement :

```bash
composer install
cp .env.example .env
php artisan key:generate
touch database/database.sqlite
php artisan migrate
npm install
npm run build
```

Sous Windows PowerShell, remplacez `cp` et `touch` par :

```powershell
Copy-Item .env.example .env
New-Item -ItemType File database/database.sqlite -Force
```

## Lancer l’application

Pour lancer uniquement le serveur Laravel avec le diagnostic préalable :

```bash
composer serve
```

Pour lancer l’environnement complet de développement (serveur Laravel, queue, logs et Vite) :

```bash
composer dev
```

Vous pouvez aussi utiliser directement :

```bash
php artisan serve
npm run dev
```

## Diagnostic PHP

Si `php artisan serve` affiche :

```text
PHP Warning:  Module "openssl" is already loaded in Unknown on line 0
```

le problème vient de la configuration PHP globale de la machine, pas du code Laravel : l’extension `openssl` est chargée deux fois dans `php.ini` ou dans un fichier additionnel `conf.d`.

Le projet contient une commande de diagnostic pour repérer les fichiers concernés :

```bash
composer doctor
```

La sortie liste les fichiers `php.ini` chargés et signale les extensions déclarées plusieurs fois. Pour corriger l’avertissement, gardez une seule ligne active `extension=openssl` dans le fichier `php.ini` chargé ou dans ses fichiers additionnels, commentez les doublons avec `;`, puis redémarrez le terminal.

Vous pouvez afficher les fichiers PHP réellement chargés avec :

```bash
php --ini
```

## Tests

```bash
composer test
```
