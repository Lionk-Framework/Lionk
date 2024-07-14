import subprocess
import sys
import os

def read_file_to_list(filename):
    with open(filename, 'r') as file:
        return [line.strip() for line in file.readlines()]

def main(github_token):
    src_path = os.getenv('SRC_PATH')

    projects = read_file_to_list('projects.txt')
    newversions = read_file_to_list('newversions.txt')

    print(f"Projects: {projects}")
    print(f"New versions: {newversions}")

    if len(projects) == 0:
        print("No projects to process")
        sys.exit(1)

    for i, project in enumerate(projects):
        newversion = newversions[i]
        tag = f"{project}_{newversion}"

        print(f"Deleting tag {tag}")

        # Delete the tag locally
        subprocess.run(['git', 'tag', '-d', tag], check=True)

        # Delete the tag remotely
        subprocess.run(['git', 'push', '--delete', 'origin', tag], check=True)

        # Delete the release
        subprocess.run(['gh', 'release', 'delete', tag, '--yes'], check=True)

if __name__ == "__main__":
    if len(sys.argv) != 2:
        print("Usage: python delete_releases.py <GITHUB_TOKEN>")
        sys.exit(1)

    github_token = sys.argv[1]
    main(github_token)