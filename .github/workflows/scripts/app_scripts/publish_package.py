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

dockerfile_context = os.getenv('DOCKERFILE_CONTEXT')
app_name = os.getenv('APP_NAME')
docker_registry = os.getenv('DOCKER_REGISTRY')
gh_token = os.getenv('GH_TOKEN')
print(f"Publishing {app_name} from {sln_path} to {docker_registry}")

with open('newversion.txt', 'r') as file:
    newversion = file.read().strip()

print(f"Publishing {app_name} as version {newversion}")

run_command(['ls' , 'src'])

# Create a new builder instance
run_command(['docker', 'buildx', 'create', '--name', 'mybuilder', '--use'])

# Create image and push it to the registry
run_command([
    'docker', 'buildx', 'build', dockerfile_context,
    '--platform', 'linux/amd64,linux/arm64',
    '--tag', f"{docker_registry}/{app_name.lower()}:{newversion}",
    '--tag', f"{docker_registry}/{app_name.lower()}:latest",
    '--push'
])

# Clean up the builder instance
run_command(['docker', 'buildx', 'rm', 'mybuilder'])
