import subprocess
import sys
import os

def run_command(command):
    try:
        result = subprocess.run(command, check=True, capture_output=True, text=True)
        print("STDOUT:", result.stdout.strip())
        print("STDERR:", result.stderr.strip())
    except subprocess.CalledProcessError as e:
        print(f"Error running command {command}: {e}")
        print("STDOUT:", e.stdout)
        print("STDERR:", e.stderr)
        sys.exit(1)

def read_file_to_list(filename):
    try:
        with open(filename, 'r') as file:
            return file.read().strip().split()
    except FileNotFoundError:
        print(f"File not found: {filename}")
        sys.exit(1)

LIB_PATH = os.getenv('LIB_PATH')
nuget_registry = os.getenv('NUGET_REGISTRY')
gh_token = os.getenv('GITHUB_TOKEN')

print(f"LIB_PATH: {LIB_PATH}")
print(f"NUGET_REGISTRY: {nuget_registry}")
print(f"GITHUB_TOKEN: {'******' if gh_token else None}")

projects = read_file_to_list('projects.txt')
newversions = read_file_to_list('newversions.txt')

if len(projects) == 0:
    print("No projects to publish")
    sys.exit(1)

if len(projects) != len(newversions):
    print("Mismatch between number of projects and versions")
    sys.exit(1)

for i, project in enumerate(projects):
    newversion = newversions[i]
    project_path = os.path.join(LIB_PATH, project)
    csproj = os.path.join(project_path, f"{project}.csproj")
    readme_path = os.path.join(project_path, "README.md")

    print(f"Publishing {csproj} as version {newversion}")

    run_command(['dotnet', 'pack', csproj, '-o', './output'])
    run_command(['dotnet', 'nuget', 'push', f"./output/{project}.{newversion}.nupkg", 
                    '-k', gh_token, '-s', nuget_registry])
