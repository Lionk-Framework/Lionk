import os
import sys


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

pr_body = os.getenv('PR_BODY')
changed_files = os.getenv('CHANGED_FILES', '').split()


if not pr_body:
    print("Error: PR body is missing.")
    sys.exit(1)

default_accepted_extensions = ["md", "txt", "drawio", "png", "jpg", "jpeg", "gif", "svg"]



if not changed_files:
    print("Error: No changed files detected.")
    sys.exit(1)

accepted_extensions = get_accepted_extensions(pr_body)

for ext in default_accepted_extensions:
    if ext not in accepted_extensions:
        accepted_extensions.append(ext)

if not accepted_extensions:
    print("Error: No accepted extensions found in PR body.")
    sys.exit(1)

if check_files(changed_files, accepted_extensions):
    print("All changed files have accepted extensions.")
    sys.exit(0)
else:
    print("Some changed files do not have accepted extensions.")
    sys.exit(1)
