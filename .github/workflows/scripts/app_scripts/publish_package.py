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
dockerfile = os.path.join('src', 'Dockerfile')

# Construire l'image Docker avec une étiquette de description
run_command(['docker', 'build', dockerfile , '-t', f"{docker_registry}/{app_name.lower()}:{newversion}", '-t',f"{docker_registry}/{app_name.lower()}:latest"])	


# Pousser l'image Docker au registre
run_command(['docker', 'push', f"{docker_registry}/{app_name.lower()}:{newversion}"])
run_command(['docker', 'push', f"{docker_registry}/{app_name.lower()}:latest"])
