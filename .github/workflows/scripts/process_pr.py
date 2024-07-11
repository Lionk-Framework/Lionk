import os
import re
import json

def main():
    PR_TITLE = os.getenv('PR_TITLE')
    PR_BODY = os.getenv('PR_BODY')
    SRC_PATH = './src_test/'

    print(f'Pull Request title: {PR_TITLE}')
    print(f'Pull Request body: {PR_BODY}')

    # Initialize arrays
    projects = []
    types = []
    oldversions = []
    newversions = []

    # Split projects and types into arrays, and remove spaces from project names
    for pair in pairs:
        parts = pair.split()
        if len(parts) == 2:
            project, type = parts
            projects.append(project.replace(' ', ''))
            types.append(type.replace(' ', ''))
        else:
            print(f'Invalid pair: {pair}')
            exit(1)

    # Show projects and types
    print(f'Projects: {projects}')
    print(f'Types: {types}')

    # Validate types
    for type in types:
        if type not in ['major', 'minor', 'patch']:
            print(f'Invalid version type: {type}')
            exit(1)

    print('All version types are valid.')

    # Check if project file exists
    for project in projects:
        if not os.path.isfile(f'{SRC_PATH}/{project}/{project}.csproj'):
            print(f'Project file not found: {SRC_PATH}/{project}/{project}.csproj')
            exit(1)

    print('All project files exist.')

    # Extract changelogs from PR body and convert to JSON
    changelogs = {}
    current_project = None
    for line in PR_BODY.splitlines():
        line = line.strip()
        if not line:
            continue
        if not line.startswith('-'):
            # It's a project name, clean spaces and check if it exists in the projects array
            project_clean = line.replace(' ', '')
            if project_clean not in projects:
                print(f'Project {project_clean} not found in the PR title.')
                exit(1)
            if current_project:
                changelogs[current_project] = changelogs.get(current_project, [])
            current_project = line.strip()
        else:
            # It's a changelog item
            changelog = line.lstrip('- ').strip()
            if current_project:
                if current_project not in changelogs:
                    changelogs[current_project] = []
                changelogs[current_project].append(changelog)

    # Ensure the last project changelog is added
    if current_project:
        changelogs[current_project] = changelogs.get(current_project, [])

    changelogs_json = json.dumps(changelogs)
    print(f'Changelogs JSON: {changelogs_json}')

    # Loop through each project and type to populate oldversions and newversions arrays
    for project, type in zip(projects, types):
        csproj = f'{SRC_PATH}/{project}/{project}.csproj'
        with open(csproj, 'r') as f:
            content = f.read()
            oldversion = re.search(r'<Version>(.*?)</Version>', content).group(1)
            oldversions.append(oldversion)

        major, minor, patch = map(int, oldversion.split('.'))
        if type == 'major':
            major += 1
            minor = 0
            patch = 0
        elif type == 'minor':
            minor += 1
            patch = 0
        elif type == 'patch':
            patch += 1

        newversion = f'{major}.{minor}.{patch}'
        newversions.append(newversion)

        print(f'{project} old version: {oldversion}, new version: {newversion}')

    # Save data to artifact
    with open('projects.txt', 'w') as f:
        f.write(' '.join(projects))
    with open('types.txt', 'w') as f:
        f.write(' '.join(types))
    with open('oldversions.txt', 'w') as f:
        f.write(' '.join(oldversions))
    with open('newversions.txt', 'w') as f:
        f.write(' '.join(newversions))
    with open('changelogs.json', 'w') as f:
        f.write(changelogs_json)

if __name__ == "__main__":
    main()
