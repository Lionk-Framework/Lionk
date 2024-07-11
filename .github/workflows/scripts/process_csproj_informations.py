import os
import json
import re

# Lire le contenu de projects.txt
with open('projects.txt', 'r') as file:
    projects = file.read().strip()

# Lire le contenu de newversions.txt
with open('newversions.txt', 'r') as file:
    versions = file.read().strip()

projects = projects.split()
versions = versions.split()

print(projects)
print(versions)

# Récupérer le chemin src de la variable d'environnement ou utiliser une valeur par défaut
SRC_PATH = os.getenv('SRC_PATH', './src_test/')

# Fonction pour extraire la valeur d'une balise XML
def extract_value(tag, content):
    match = re.search(f'<{tag}>(.*?)</{tag}>', content)
    return match.group(1) if match else None

# Initialiser une liste pour stocker les données de projet
projects_data = []

# Parcourir les projets et extraire les données nécessaires
for project in projects:
    project_path = os.path.join(SRC_PATH, project)
    csproj_file = os.path.join(project_path, f"{project}.csproj")
    
    try:
        with open(csproj_file, 'r') as file:
            csproj_content = file.read()
        
        target_framework = extract_value('TargetFramework', csproj_content)
        package_id = extract_value('PackageId', csproj_content)
        authors = extract_value('Authors', csproj_content)
        company = extract_value('Company', csproj_content)
        product = extract_value('Product', csproj_content)
        description = extract_value('Description', csproj_content)
        
        project_data = {
            "Project": project.strip(),
            "TargetFramework": target_framework,
            "PackageId": package_id,
            "Authors": authors,
            "Company": company,
            "Product": product,
            "Description": description
        }
        projects_data.append(project_data)
    except Exception as e:
        print(f"Erreur lors du traitement du projet {project.strip()}: {e}")

# Convertir les données en JSON et les afficher
json_data = json.dumps(projects_data, indent=4)
print(json_data)
