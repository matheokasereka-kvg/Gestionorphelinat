#!/usr/bin/env php
<?php

$exitCode = 0;
$projectRoot = dirname(__DIR__);
$iniFiles = array_values(array_filter(array_merge(
    [php_ini_loaded_file()],
    php_ini_scanned_files() ? array_map('trim', explode(',', php_ini_scanned_files())) : []
)));

function line(string $message = ''): void
{
    echo $message.PHP_EOL;
}

function ok(string $message): void
{
    line('✅ '.$message);
}

function warn(string $message): void
{
    line('⚠️  '.$message);
}

function fail_check(string $message): void
{
    global $exitCode;

    $exitCode = 1;
    line('❌ '.$message);
}

function relative_path(string $path, string $projectRoot): string
{
    $normalizedRoot = rtrim(str_replace('\\', '/', $projectRoot), '/').'/';
    $normalizedPath = str_replace('\\', '/', $path);

    if (str_starts_with($normalizedPath, $normalizedRoot)) {
        return substr($normalizedPath, strlen($normalizedRoot));
    }

    return $path;
}

function parse_extension_directives(array $iniFiles): array
{
    $directives = [];

    foreach ($iniFiles as $iniFile) {
        if (! is_file($iniFile) || ! is_readable($iniFile)) {
            continue;
        }

        foreach (file($iniFile, FILE_IGNORE_NEW_LINES) ?: [] as $lineNumber => $content) {
            $trimmed = trim($content);

            if ($trimmed === '' || str_starts_with($trimmed, ';') || str_starts_with($trimmed, '#')) {
                continue;
            }

            if (! preg_match('/^(zend_extension|extension)\s*=\s*(.+)$/i', $trimmed, $matches)) {
                continue;
            }

            $value = trim($matches[2], " \t\n\r\0\x0B\"'");
            $extension = strtolower(pathinfo($value, PATHINFO_FILENAME) ?: $value);

            if (str_starts_with($extension, 'php_')) {
                $extension = substr($extension, 4);
            }

            $directives[$extension][] = [
                'file' => $iniFile,
                'line' => $lineNumber + 1,
                'value' => $value,
                'type' => strtolower($matches[1]),
            ];
        }
    }

    return $directives;
}

line('Diagnostic de démarrage Gestionorphelinat');
line('PHP '.PHP_VERSION.' ('.PHP_BINARY.')');
line();

if (version_compare(PHP_VERSION, '8.2.0', '>=')) {
    ok('Version PHP compatible avec Laravel 12 (>= 8.2).');
} else {
    fail_check('PHP 8.2 ou plus récent est requis pour Laravel 12.');
}

$requiredExtensions = ['ctype', 'curl', 'dom', 'fileinfo', 'filter', 'hash', 'mbstring', 'openssl', 'pdo', 'session', 'tokenizer', 'xml'];
foreach ($requiredExtensions as $extension) {
    if (extension_loaded($extension)) {
        ok("Extension PHP {$extension} chargée.");
    } else {
        fail_check("Extension PHP {$extension} manquante.");
    }
}

line();
line('Fichiers php.ini analysés :');
if ($iniFiles === []) {
    warn('Aucun fichier php.ini chargé.');
} else {
    foreach ($iniFiles as $iniFile) {
        line(' - '.$iniFile);
    }
}

$extensionDirectives = parse_extension_directives($iniFiles);
$duplicates = array_filter($extensionDirectives, fn (array $entries): bool => count($entries) > 1);

line();
if ($duplicates === []) {
    ok('Aucune extension PHP déclarée plusieurs fois dans les fichiers ini chargés.');
} else {
    warn('Des extensions PHP sont déclarées plusieurs fois. Cela peut produire "Module ... is already loaded".');

    foreach ($duplicates as $extension => $entries) {
        line(" - {$extension} :");
        foreach ($entries as $entry) {
            line("   • {$entry['file']}:{$entry['line']} ({$entry['type']}={$entry['value']})");
        }
    }

    if (isset($duplicates['openssl'])) {
        warn('Pour corriger l’avertissement openssl sous Windows, gardez une seule ligne extension=openssl active dans le php.ini chargé et ses fichiers conf.d, puis redémarrez le terminal.');
    }
}

line();
if (is_file($projectRoot.'/.env')) {
    ok('Fichier .env présent.');
} else {
    warn('Fichier .env absent. Exécutez : copy .env.example .env (Windows) ou cp .env.example .env (macOS/Linux), puis php artisan key:generate.');
}

if (is_file($projectRoot.'/vendor/autoload.php')) {
    ok('Dépendances Composer installées.');
} else {
    fail_check('Dépendances Composer absentes. Exécutez : composer install.');
}

$envDatabase = getenv('DB_DATABASE') ?: null;
$defaultSqlite = $projectRoot.'/database/database.sqlite';
if (($envDatabase === null || $envDatabase === '' || str_ends_with($envDatabase, '.sqlite')) && ! is_file($defaultSqlite)) {
    warn('Base SQLite locale absente. Elle sera créée par composer setup, ou manuellement avec : type nul > database\\database.sqlite (Windows) / touch database/database.sqlite (macOS/Linux).');
} elseif (is_file($defaultSqlite)) {
    ok('Base SQLite locale présente ('.relative_path($defaultSqlite, $projectRoot).').');
}

line();
if ($exitCode === 0) {
    ok('Diagnostic terminé : aucun blocage détecté pour lancer Laravel.');
} else {
    warn('Diagnostic terminé : corrigez les points ci-dessus avant de relancer le serveur.');
}

exit($exitCode);
