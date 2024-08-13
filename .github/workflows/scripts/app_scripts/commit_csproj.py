import os
import subprocess


def run_command(command):
    result = subprocess.run(command, check=True, capture_output=True, text=True)
    print(result.stdout.strip())
    print(result.stderr.strip())


# Get environment variables
app_path = os.getenv("APP_PATH")
bot_name = os.getenv("BOT_NAME")
bot_email = os.getenv("BOT_MAIL")
github_head_ref = os.getenv("GITHUB_HEAD_REF")

# Config git
run_command(["git", "config", "--global", "user.name", bot_name])
run_command(["git", "config", "--global", "user.email", bot_email])

# Fetch the latest changes
run_command(["git", "fetch", "origin", github_head_ref])

run_command(["git", "add", app_path])

# Commit changes
run_command(["git", "commit", "-m", "Update project versions"])

# Push changes
run_command(["git", "push", "--force"])
