import os
import sys
import subprocess
import json

def get_changed_files(token, repo_name, pr_number):
    url = f"https://api.github.com/repos/{repo_name}/pulls/{pr_number}/files"
    headers = [
        f'Authorization: token {token}',
        'Accept: application/vnd.github.v3+json'
    ]

    # Construire la commande curl
    curl_command = ['curl', '-s']
    for header in headers:
        curl_command += ['-H', header]
    curl_command.append(url)

    # Exécuter la commande curl et capturer la sortie
    result = subprocess.run(curl_command, capture_output=True, text=True)
    
    if result.returncode != 0:
        print(f"Error: Failed to fetch changed files. {result.stderr}")
        sys.exit(1)

    files = json.loads(result.stdout)
    changed_files = [file['filename'] for file in files]
    return changed_files

def get_accepted_extensions(pr_body):
    # Extraire les extensions de fichiers acceptées du corps de la PR
    extensions = []
    lines = pr_body.splitlines()
    for line in lines:
        if line.startswith("-"):
            extensions.append(line.replace("-", "").strip())
    return extensions

def check_files(changed_files, accepted_extensions):
    # Vérifier si tous les fichiers modifiés ont des extensions acceptées
    for file in changed_files:
        if not any(file.endswith(ext) for ext in accepted_extensions):
            return False
    return True

def main():
    token = os.getenv('GITHUB_TOKEN')
    repo_name = os.getenv('REPO_NAME')
    pr_number = os.getenv('PR_NUMBER')
    pr_body = os.getenv('PR_BODY')

    if not token or not repo_name or not pr_number:
        print("Error: Missing required environment variables.")
        sys.exit(1)

    default_accepted_extensions = ["yml", "py", "sh"]
    
    changed_files = get_changed_files(token, repo_name, pr_number)
    print(f"changed files : {changed_files}")

    if not changed_files:
        print("Error: No changed files detected.")
        sys.exit(1)

    accepted_extensions = get_accepted_extensions(pr_body)
    for ext in default_accepted_extensions:
        if ext not in accepted_extensions:
            accepted_extensions.append(ext)

    if check_files(changed_files, accepted_extensions):
        print("All changed files have accepted extensions.")
        sys.exit(0)
    else:
        print("Some changed files do not have accepted extensions.")
        sys.exit(1)

if __name__ == "__main__":
    main()
