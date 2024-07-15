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

app_path = os.getenv('APP_PATH')
newversion = os.getenv("NEW_VERSION")
changelogs = os.getenv("CHANGELOG").split()
#get folder of APP_PATH
app_folder = os.path.dirname(app_path)
readme_file = os.path.join(app_folder, "README.md")


description = f"\n\n## Change Log"
for change in changelogs:
    description += f"\n- {change}"
description += "\n\n"

if os.path.exists(readme_file):
    with open(readme_file, 'r') as file:
        description += file.read()

tree = ET.parse(app_path)
root = tree.getroot()
property_group = root.find('PropertyGroup')

# update or add Version and Description elements
update_or_add_element(property_group ,"Version", newversion)
update_or_add_element(property_group ,"Description", description)

with open("description.txt", "w") as file:
    file.write(description)

indent(root)
tree.write(app_path, encoding="utf-8", xml_declaration=True)

# show the updated csproj file
ET.dump(root)
