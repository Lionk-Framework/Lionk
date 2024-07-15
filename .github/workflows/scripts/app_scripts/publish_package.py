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

app_path = os.getenv('APP_PATH')
app_name = os.getenv('APP_NAME')
docker_registry = os.getenv('DOCKER_REGISTRY')
gh_token = os.getenv('GH_TOKEN')


context = os.path.dirname(app_path)

with open('newversion.txt', 'r') as file:
    newversion = file.read().strip()

with open('description.txt', 'r') as file:
    description = file.read().strip()

print(f"Publishing {app_name} as version {newversion}")

# Construire l'image Docker avec une étiquette de description
run_command(['docker', 'build', '-t', f"{docker_registry}/{app_name.lower()}:{newversion}", '--label', f"description={description}", context])

# Se connecter au registre Docker
run_command(['echo', gh_token, '|', 'docker', 'login', docker_registry, '-u', 'USERNAME', '--password-stdin'])

# Pousser l'image Docker au registre
run_command(['docker', 'push', f"{docker_registry}/{app_name.lower()}:{newversion}"])
run_command(['docker', 'push', f"{docker_registry}/{app_name.lower()}:latest"])
exit(1)
