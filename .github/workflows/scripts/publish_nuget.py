import subprocess
import sys
import os

def run_command(command):
    result = subprocess.run(command, check=True, capture_output=True, text=True)
    print(result.stdout.strip())
    print(result.stderr.strip())

def read_file_to_list(filename):
    with open(filename, 'r') as file:
        return [line.strip() for line in file.readlines()]

def main(gh_token):
    src_path = os.getenv('SRC_PATH')
    nuget_registry = os.getenv('NUGET_REGISTRY')
    gh_token = os.getenv('GITHUB_TOKEN')

    projects = read_file_to_list('projects.txt')
    newversions = read_file_to_list('newversions.txt')

    if len(projects) == 0:
        print("No projects to publish")
        sys.exit(1)

    for i, project in enumerate(projects):
        newversion = newversions[i]
        csproj = f"{src_path}/{project}/{project}.csproj"

        print(f"Publishing {csproj} as version {newversion}")

        run_command(['dotnet', 'pack', csproj, '-o', './output'])
        run_command(['dotnet', 'nuget', 'push', f"./output/{project}.{newversion}.nupkg", 
                     '-k', gh_token, '-s', nuget_registry])
