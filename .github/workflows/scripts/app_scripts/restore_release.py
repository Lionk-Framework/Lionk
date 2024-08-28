import subprocess
import sys
import os

app_name = os.getenv('APP_NAME')
github_token = os.getenv('GH_TOKEN')

with open('newversion.txt', 'r') as file:
    newversion = file.read().strip()


tag = f"{app_name}_{newversion}"

print(f"Deleting tag {tag}")

# Delete the release
subprocess.run(['gh', 'release', 'delete', tag, '--yes'], check=True)

# Delete the tag remotely
subprocess.run(['git', 'push', '--delete', 'origin', tag], check=True)



