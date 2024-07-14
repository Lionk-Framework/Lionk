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
            return [line.strip() for line in file.readlines()]
    except FileNotFoundError:
        print(f"File not found: {filename}")
        sys.exit(1)


src_path = os.getenv('SRC_PATH')
nuget_registry = os.getenv('NUGET_REGISTRY')
gh_token = os.getenv('GITHUB_TOKEN')

print(f"SRC_PATH: {src_path}")
print(f"NUGET_REGISTRY: {nuget_registry}")
print(f"GITHUB_TOKEN: {'******' if gh_token else None}")

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


