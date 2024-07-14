import subprocess
import json
import sys
import os

def get_projects(file_path):
    with open(file_path, 'r') as file:
        return file.read().split()

def get_versions(file_path):
    with open(file_path, 'r') as file:
        return file.read().split()

def delete_package_version(api_token, api_url, package_type, package_name, package_version):
    # Command to list versions
    list_versions_cmd = [
        "curl",
        "-H", f"Authorization: token {api_token}",
        "-H", "Accept: application/vnd.github.v3+json",
        f"{api_url}/packages/{package_type}/{package_name}/versions"
    ]

    # Execute the command to list versions
    result = subprocess.run(list_versions_cmd, capture_output=True, text=True)

    # Check for errors
    if result.returncode != 0:
        print(f"Error listing versions for {package_name}:", result.stderr)
        return

    # Parse the JSON response
    versions = json.loads(result.stdout)

    # Find the version ID
    version_id = None
    for version in versions:
        if version['name'] == package_version:
            version_id = version['id']
            break

    if version_id is None:
        print(f"Version {package_version} not found for package {package_name}.")
        return

    # Command to delete the version
    delete_version_cmd = [
        "curl",
        "-X", "DELETE",
        "-H", f"Authorization: token {api_token}",
        "-H", "Accept: application/vnd.github.v3+json",
        f"{api_url}/packages/{package_type}/{package_name}/versions/{version_id}"
    ]

    # Execute the command to delete the version
    delete_result = subprocess.run(delete_version_cmd, capture_output=True, text=True)

    # Check for errors
    if delete_result.returncode != 0 or delete_result.stdout.strip() != '':
        print(f"Error deleting version {package_version} for package {package_name}:", delete_result.stderr)
    else:
        print(f"Version {package_version} (ID: {version_id}) successfully deleted for package {package_name}.")

def main():
    api_token = os.getenv("GITHUB_TOKEN")
    api_url = os.getenv("GH_API_URL")
    package_type = "nuget"  # Adjust if necessary

    if not api_token or not api_url:
        print("Environment variables GITHUB_TOKEN and GH_API_URL must be set.")
        exit(1)

    projects = get_projects('projects.txt')
    versions = get_versions('newversions.txt')

    for project in projects:
        for version in versions:
            delete_package_version(api_token, api_url, package_type, project, version)

if __name__ == "__main__":
    main()
