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

def update_or_add_element(container, name, value):
    element = container.find(name)
    if element is not None:
        # Modifier l'élément existant
        element.text = value
    else:
        # Ajouter un nouvel élément
        new_element = ET.SubElement(container, name)
        new_element.text = value



# get environment variables
LIB_PATH = os.getenv("LIB_PATH")
projects = os.getenv("PROJECTS").split()
newversions = os.getenv("NEW_VERSION").split()
changelogs = json.loads(os.getenv("CHANGELOG"))

# Parcourir les projets et mettre à jour les fichiers .csproj
for project in projects:
    project_path = os.path.join(LIB_PATH, project)
    csproj_file = os.path.join(project_path, f"{project}.csproj")
    new_version = newversions[projects.index(project)]
    readme_file = os.path.join(project_path, "README.md")
   

    if project in changelogs:
        description = f"\n\n## Change Log"
        for change in changelogs[project]:
            description += f"\n- {change}"
        description += "\n\n"
    
    if not os.path.exists(readme_file):
            #create empty README.md
            with open(readme_file, "w") as file:
                file.write("")


     # read description from README.md and add changelog
    with open(readme_file, "r") as file:
        description += file.read()

    tree = ET.parse(csproj_file)
    root = tree.getroot()
    property_group = root.find('PropertyGroup')

    # update or add Version and Description elements
    update_or_add_element(property_group ,"Version", new_version)
    update_or_add_element(property_group ,"Description", description)

    with open("description.txt", "w") as file:
        file.write(description)

    indent(root)
    tree.write(csproj_file, encoding="utf-8", xml_declaration=True)

    # show the updated csproj file
    ET.dump(root)
