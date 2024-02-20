#Prérequis
Avant de commencer, assurez-vous d'avoir les éléments suivants installés sur votre système :

Installer ASP.NET Core https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-aspnetcore-8.0.2-windows-hosting-bundle-installer
Installer Microsoft SQL Server pour la base de données.
Installer Internet Information Services (IIS) pour l'hébergement du site web.

Étapes d'installation
1. Clonage du projet
Clonez le projet ASP.NET Core depuis github ou copiez-le localement.

2. Configuration de la base de données
Ouvrez SQL Server Management Studio.
Connectez-vous à votre serveur SQL.
Restaurer la base données nommée "Gamsesoft.bak" situé à la racine de GitHub.

3. Configuration du fichier appsettings.json
Dans le projet ASP.NET Core, ouvrez le fichier appsettings.json et configurez la chaîne de connexion à la base de données.

4. Configuration de l'hébergement IIS
Ouvrez Internet Information Services (IIS) Manager.
Ajoutez un nouveau site en spécifiant le chemin du répertoire racine.
Assurez-vous que le pool d'applications associé utilise la version correcte de .NET CLR.
Assurez-vous que les autorisations d'accès au répertoire sont correctement configurées.

5. Déploiement de l'application
Copiez les fichiers situés dans le répertoire "SitePublie" sur le serveur IIS.

6. Configuration de l'application dans IIS
Dans IIS Manager, sélectionnez le site que vous avez créé.
Cliquez sur "Explorateur des liaisons" et ajoutez une nouvelle liaison si nécessaire.
Redémarrez le site web pour appliquer les modifications.
Le site ASP.NET Core est désormais déployé et accessible via le navigateur en utilisant l'URL que vous avez configurée dans IIS.
