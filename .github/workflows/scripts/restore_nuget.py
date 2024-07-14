import subprocess
import sys
import os

def read_file_to_list(filename):
    with open(filename, 'r') as file:
        return [line.strip() for line in file.readlines()]

def main(gh_token):
    src_path = os.getenv('SRC_PATH')
    nuget_registry = os.getenv('NUGET_REGISTRY')

    projects = read_file_to_list('projects.txt')
    newversions = read_file_to_list('newversions.txt')

    if len(projects) == 0:
        print("No projects to delete")
        sys.exit(1)

    for i, project in enumerate(projects):
        newversion = newversions[i]
        package_id = f"{project}.{newversion}"

        print(f"Deleting {package_id} from {nuget_registry}")

        # Suppression du package NuGet
        subprocess.run(['dotnet', 'nuget', 'delete', project, newversion, 
                        '-k', gh_token, '-s', nuget_registry, '--non-interactive'], check=True)

if __name__ == "__main__":
    if len(sys.argv) != 2:
        print("Usage: python delete_packages.py <GH_TOKEN>")
        sys.exit(1)

    gh_token = sys.argv[1]
    main(gh_token)
