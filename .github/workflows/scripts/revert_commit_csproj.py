import os
import subprocess

def revert_last_commit():
    try:
        # Revert the last commit
        subprocess.run(["git", "reset", "--hard", "HEAD~1"], check=True)
        print("Last commit has been reverted.")
    except subprocess.CalledProcessError as e:
        print(f"Failed to revert the last commit: {e}")
        raise

if __name__ == "__main__":
    revert_last_commit()
