import os
import xml.etree.ElementTree as ET
import json

# Méthode pour écrire avec un formatage indente
def indent(elem, level=0):
    i = "\n" + level * "  "
    if len(elem):
        if not elem.text or not elem.text.strip():
            elem.text = i + "  "
        if not elem.tail or not elem.tail.strip():
            elem.tail = i
        for subelem in elem:
            indent(subelem, level + 1)
        if not subelem.tail or not subelem.tail.strip():
            subelem.tail = i
    else:
        if level and (not elem.tail or not elem.tail.strip()):
            elem.tail = i

def update_or_add_element(container, name, value=None, attributes=None):
    element = container.find(name)
    if element is not None:
        # Modifier l'élément existant
        element.text = value
    elif value is not None:
        # Ajouter un nouvel élément avec une valeur de texte
        new_element = ET.SubElement(container, name)
        new_element.text = value
    else:
        # Ajouter un nouvel élément avec des attributs
        new_element = ET.SubElement(container, name)
        if attributes:
            for attr_name, attr_value in attributes.items():
                new_element.set(attr_name, attr_value)
        # Si aucune valeur de texte n'est spécifiée, laisser l'élément vide

# Récupérer les variables d'environnement
LIB_PATH = os.getenv("LIB_PATH")
projects = os.getenv("PROJECTS").split()
newversions = os.getenv("NEW_VERSION").split()
changelogs = json.loads(os.getenv("CHANGELOG"))

# Parcourir les projets et mettre à jour les fichiers .csproj
for project in projects:
    project_path = os.path.join(LIB_PATH, project)
    csproj_file = os.path.join(project_path, f"{project}.csproj")
    readme_file = os.path.join(project_path, "README.md")
    new_version = newversions[projects.index(project)]   

    if project in changelogs:
        changes = ""
        for change in changelogs[project]:
            changes += f"- {change}\n"
    
    tree = ET.parse(csproj_file)
    root = tree.getroot()

    # Trouver ou créer PropertyGroup
    property_group = root.find('PropertyGroup')
    if property_group is None:
        property_group = ET.SubElement(root, 'PropertyGroup')

    # Trouver ou créer ItemGroup
    item_group = root.find('ItemGroup')
    if item_group is None:
        item_group = ET.SubElement(root, 'ItemGroup')

    # Mettre à jour ou ajouter les éléments Version, PackageReleaseNotes et PackageReadmeFile
    update_or_add_element(property_group, "Version", value=new_version)
    update_or_add_element(property_group, "PackageReleaseNotes", value=changes)
    if os.path.exists(readme_file):
        update_or_add_element(property_group, "PackageReadmeFile", "README.md")
        update_or_add_element(item_group, 'None', attributes={'Include': 'README.md', 'Pack': 'true', 'PackagePath': ''})

    indent(root)
    tree.write(csproj_file, encoding="utf-8", xml_declaration=True)

    # Afficher le fichier .csproj mis à jour
    ET.dump(root)
