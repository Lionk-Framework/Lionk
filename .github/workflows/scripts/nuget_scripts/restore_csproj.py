import os
import subprocess
import sys

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

# Get environment variables
bot_name = os.getenv("BOT_NAME")
bot_email = os.getenv("BOT_MAIL")
LIB_PATH = os.getenv("LIB_PATH")
github_head_ref = os.getenv("GITHUB_HEAD_REF")

# print environment variables
print(f"bot_name: {bot_name}")
print(f"bot_email: {bot_email}")
print(f"LIB_PATH: {LIB_PATH}")
print(f"github_head_ref: {github_head_ref}")

# Config git
print("Configuring git")
run_command(["git", "config", "--global", "user.name", bot_name])
run_command(["git", "config", "--global", "user.email", bot_email])

# Fetch the latest changes
print("Fetching latest changes")
run_command(["git", "fetch", "origin", github_head_ref])

# Get project names from projects.txt
with open("projects.txt", "r") as file:
    projects = file.read().strip().split()

if len(projects) == 0:
    print("No projects to process")
    sys.exit(1)

# Add each project file
for project in projects:

# replace project.csproj with project.csproj.bkp    
    project_file = os.path.join(LIB_PATH, project, f"{project}.csproj")
    backup_file = os.path.join(LIB_PATH, project, f"{project}.csproj.bkp")
    with open(backup_file, "rb") as backup:
        content = backup.read()
    with open(project_file, "wb") as project:
        project.write(content)
    run_command(["git", "add", project_file])

# Commit changes 
print("Committing changes")
run_command(["git", "commit", "-m", "Restore project versions"])

# Push changes
print("Pushing changes")
run_command(["git", "push"])
