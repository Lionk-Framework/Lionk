import os

# Lire les noms des projets à partir du fichier projects.txt
with open("projects.txt", "r") as file:
    projects = file.read().strip().split()

# Copier chaque fichier de projet
for project in projects:
    src_path = os.getenv("SRC_PATH")
    src_file = os.path.join(src_path, f"{project}.csproj")
    backup_file = os.path.join(src_path, f"{project}.csproj.bkp")

    # Lire le contenu du fichier source
    with open(src_file, "rb") as src:
        content = src.read()

    # Écrire le contenu dans le fichier de sauvegarde
    with open(backup_file, "wb") as bkp:
        bkp.write(content)
