import os
import re
import json

def main():

    PR_TITLE = os.getenv('PR_TITLE')
    PR_BODY = os.getenv('PR_BODY')


    print(f'Pull Request title: {PR_TITLE}')
    print(f'Pull Request body: {PR_BODY}')

    #remove nuget: from PR_TITLE
    app_path = os.getenv('APP_PATH')

    type = PR_TITLE.replace('app:', '').lower().strip()
    print(f'Type: {type}')


    if type not in ['major', 'minor', 'patch']:
        print(f'Invalid version type: {type}')
        exit(1)

    # Extract changelogs from PR body and convert to JSON
    changelogs = []
    for line in PR_BODY.splitlines():
        line = line.strip()
        if not line or line.startswith('//'):
            continue
        
        elif line.startswith('-'):
            # It's a changelog item
            changelog = line.lstrip('-').strip()
            changelogs.append(changelog)


    print(f'Changelogs: {changelogs}')

    #populate oldversions and newversions arrays

    csproj = app_path
    with open(csproj, 'r') as f:
        content = f.read()
        # if content contains <Version> tag, extract the version
        if '<Version>' in content:
            oldversion = re.search(r'<Version>(.*?)</Version>', content).group(1)
        else:
            oldversion = "0.0.0"
 

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


    print(f'old version: {oldversion}, new version: {newversion}')

    # Save data to artifact

    with open('oldversion.txt', 'w') as f:
        f.write(oldversion)
    with open('newversion.txt', 'w') as f:
        f.write(newversion)
    with open('changelog.txt', 'w') as f:
        for changelog in changelogs:
            f.write(f'{changelog}\n')

if __name__ == "__main__":
    main()
