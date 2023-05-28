# Mythic Ranchers

## Par Matei Pelletier et Christophe Auclair

### Présentation générale:
Notre livrable est un jeu multijoueur de style RPG dungeon crawler. On commence tout d'abord par se créer un compte et se créer un ou plusieurs personnages d'une des 3 classes suivantes: mage, nécromancien et berzerker. Chaque classe possède ses habiletés et son équipement propre. Une fois son personnage choisi on crée un lobby dans lequel il peut y avoir jusqu'à 4 joueurs simultanément. Une fois le lobby créé ce dernier sera visible pour tous les autres joueurs connectés au même moment, ils pourront alors rejoindre le lobby de leur choix. Une fois que tous les joueurs sont prêts l’hôte pourra ainsi lancer la partie. Lors de la création de le de la partie un niveau sera alors généré aléatoirement et les joueurs apparaîtront au début de celui-ci. Le but du jeu et donc de finir le donjon à l'intérieur du temps limite qui est attribué selon la taille et le nombre d'ennemis que le donjon contient. Si les joueurs réussissent ce défi, qui consiste à tuer tous les ennemis à l'intérieur du chronomètre, ils seront récompensés par des points d'expérience, du meilleur équipement et la possibilité de faire un donjon plus difficile.

### Installation:
Afin d'installer notre projet dans un contexte de développement, vous devez d'abord installer Unity avec la version 2021.3.24f1 de l'éditeur. Vous devez ensuite ouvrir le projet dans le Unity Hub, en sélectionnant le dossier du projet, qui est le dossier Mythic Ranchers dans le dossier dev de ce dépôt git. A partir de là, une fois que vous avez ouvert le projet avec la bonne version de l'éditeur, vous avez accès à tous les éléments du projet. Tous les éléments de codage du projet se trouvent dans le dossier Assets/Scripts. Nous recommandons d'utiliser Visual Studio avec le package Unity Game Development pour les ouvrir, et d'aller dans Edit>Preferences>External Tools et de s'assurer que toutes les cases sous "generate .csproj files for :" sont cochées afin d'avoir l'intellisense dans l'IDE.

### Utilisation:
Pour jouer à notre jeu, il vous suffit d'ouvrir le dossier Current Build sous Dev et de lancer MythicRanchers.exe. Dans le jeu, vous serez accueilli par un écran de connexion, avec la possibilité de vous enregistrer si vous n'avez pas de compte. Sachez que la police que nous avons choisie ne contient que des lettres majuscules, mais que le nom d'utilisateur et le mot de passe sont case-sensitive. Votre mot de passe est encrypté avant d'être enregistré dans notre base de données, ainsi que dans le registre si le bouton "Se souvenir de moi" est coché. Si vous n'avez pas encore de personnage créé, vous serez immédiatement dirigé vers la création de personnage. Vous pourrez y créer un personnage avec la classe de votre choix. Une fois que vous avez terminé, ou si vous avez déjà au moins un personnage, vous pouvez alors sélectionner le personnage que vous voulez utiliser, dont le nom apparaîtra en vert lors de la sélection. L'étape suivante consiste à revenir au menu principal et à appuyer sur Play, ce qui vous amènera au menu du lobby. Là, vous pouvez appuyer sur le bouton + pour créer un nouveau lobby, ou attendre/appuyer sur Rafraîchir pour voir s'il y a d'autres lobbies publics disponibles à rejoindre. Une fois le lobby créé, vous pourrez voir les autres joueurs connectés. Il est très important que l'hôte attende quelques secondes avant de commencer une partie, à la fois après avoir créé le lobby et après que quelqu'un l'ait rejoint, afin de laisser le temps à tous les joueurs d'envoyer et de recevoir leurs données à l'hôte. Une fois la partie lancée, les joueurs ont tout intérêt à appuyer rapidement sur la touche Echap et à équiper leurs sorts dans la case gauche de l'interface utilisateur dans leur barre d'action, car les ennemis ont tendance à se promener dans la salle de départ. Des infobulles expliquent les effets de chaque sort. Pour utiliser vos capacités, vous devez sélectionner l'emplacement correspondant dans la barre d'action (l'emplacement sélectionné par défaut est le 1), qui sera d'une couleur un peu plus foncée que les autres emplacements, puis utiliser le bouton droit de la souris pour utiliser le sort. N'oubliez pas qu'il y a des temps de recharge pour chaque sort, vous ne pouvez donc pas les spammer. De plus, il n'y a actuellement aucun moyen de quitter le jeu en dehors du menu principal, donc la seule façon de sortir est de faire alt + f4.

### Références:
Documentation Unity:
https://docs-multiplayer.unity3d.com/netcode/current/about/
https://docs.unity.com/relay/en/manual/introduction
https://docs.unity.com/lobby/en/manual/unity-lobby-service
Chaîne Youtube avec des tutoriels utiles pour nous:
https://www.youtube.com/@CodeMonkeyUnity

### Contact:
Blasayy#6359 sur discord

Whutz#8376 sur discord
