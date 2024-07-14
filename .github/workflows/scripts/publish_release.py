import subprocess
import sys
import os

def read_file_to_list(filename):
    with open(filename, 'r') as file:
        return [line.strip() for line in file.readlines()]

def main(github_token):
    src_path = os.getenv('SRC_PATH')
    bot_name = os.getenv('BOT_NAME')
    bot_mail = os.getenv('BOT_MAIL')

    projects = read_file_to_list('projects.txt')
    newversions = read_file_to_list('newversions.txt')

    print(f"Projects: {projects}")
    print(f"New versions: {newversions}")

    if len(projects) == 0:
        print("No projects to process")
        sys.exit(1)

    subprocess.run(['git', 'config', '--global', 'user.name', bot_name], check=True)
    subprocess.run(['git', 'config', '--global', 'user.email', bot_mail], check=True)

    for i, project in enumerate(projects):
        newversion = newversions[i]
        tag = f"{project}_{newversion}"

        print(f"Creating tag {tag}")

        # Create the tag
        subprocess.run(['git', 'tag', '-a', tag, '-m', f"Release {tag}"], check=True)
        subprocess.run(['git', 'push', 'origin', tag], check=True)

        # Extract description from .csproj file
        csproj_path = f"{src_path}/{project}/{project}.csproj"
        description = ""
        with open(csproj_path, 'r') as file:
            in_description = False
            for line in file:
                if '<Description>' in line:
                    in_description = True
                    description += line.replace('<Description>', '').strip() + " "
                elif '</Description>' in line:
                    in_description = False
                    description += line.replace('</Description>', '').strip()
                elif in_description:
                    description += line.strip() + " "

        description = description.strip()
        print(f"Description for {project}: {description}")

        # Create the release with the description
        subprocess.run(['gh', 'release', 'create', tag, '--title', tag, '--notes', description], check=True)

if __name__ == "__main__":
    if len(sys.argv) != 2:
        print("Usage: python create_releases.py <GITHUB_TOKEN>")
        sys.exit(1)

    github_token = sys.argv[1]
    main(github_token)
