import os
import subprocess

def get_commit_count():
    result = subprocess.run(["git", "rev-list", "--count", "HEAD"], capture_output=True, text=True)
    return int(result.stdout.strip())

def revert_last_commit():
    commit_count = get_commit_count()
    if commit_count <= 1:
        print("Not enough commits to revert. Skipping revert.")
        return

    try:
        # Revert the last commit
        subprocess.run(["git", "reset", "--hard", "HEAD~1"], check=True)
        print("Last commit has been reverted.")
    except subprocess.CalledProcessError as e:
        print(f"Failed to revert the last commit: {e}")
        raise

if __name__ == "__main__":
    revert_last_commit()
