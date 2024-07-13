import os
import subprocess

# get environment variables
bot_name = os.getenv("BOT_NAME")
bot_email = os.getenv("BOT_MAIL")
src_path = os.getenv("SRC_PATH")
github_head_ref = os.getenv("GITHUB_HEAD_REF")

#config git
subprocess.run(["git", "config", "--global", "user.name", bot_name])
subprocess.run(["git", "config", "--global", "user.email", bot_email])

# get project names from projects.txt
with open("projects.txt", "r") as file:
    projects = file.read().strip().split()

# Add each project file
for project in projects:
    project_file = os.path.join(src_path, project, f"{project}.csproj")
    subprocess.run(["git", "add", project_file])

# Commit and push changes
subprocess.run(["git", "commit", "-m", "Update project versions"])
subprocess.run(["git", "push", "origin", f"HEAD:{github_head_ref}"])