import os
import subprocess


def run_command(command):
    result = subprocess.run(command, check=True, capture_output=True, text=True)
    return result.stdout.strip()


# Get environment variables
bot_name = os.getenv("BOT_NAME")
bot_email = os.getenv("BOT_MAIL")
src_path = os.getenv("SRC_PATH")
github_head_ref = os.getenv("GITHUB_HEAD_REF")

# Config git
run_command(["git", "config", "--global", "user.name", bot_name])
run_command(["git", "config", "--global", "user.email", bot_email])

# Fetch the latest changes
run_command(["git", "fetch", "origin", github_head_ref])

# Get project names from projects.txt
with open("projects.txt", "r") as file:
    projects = file.read().strip().split()

# Add each project file
for project in projects:
    project_file = os.path.join(src_path, project, f"{project}.csproj")
    run_command(["git", "add", project_file])

# Commit changes
run_command(["git", "commit", "-m", "Update project versions"])

# Push changes
run_command(["git", "push"])
