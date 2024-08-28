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

sln_path = os.getenv('SLN_PATH')
app_name = os.getenv('APP_NAME')
docker_registry = os.getenv('DOCKER_REGISTRY')
gh_token = os.getenv('GH_TOKEN')
print(f"Publishing {app_name} from {sln_path} to {docker_registry}")

with open('newversion.txt', 'r') as file:
    newversion = file.read().strip()

print(f"Publishing {app_name} as version {newversion}")

run_command(['ls' , 'src'])

# Créer un builder multiplateforme s'il n'existe pas déjà
run_command(['docker', 'buildx', 'create', '--name', 'mybuilder', '--use'])

# Construire les images Docker pour x86_64 et arm64
run_command([
    'docker', 'buildx', 'build', sln_path,
    '--platform', 'linux/amd64,linux/arm64,windows/amd64',
    '--tag', f"{docker_registry}/{app_name.lower()}:{newversion}",
    '--tag', f"{docker_registry}/{app_name.lower()}:latest",
    '--push'
])

# Vous pouvez supprimer le builder après si vous n'en avez plus besoin
run_command(['docker', 'buildx', 'rm', 'mybuilder'])
