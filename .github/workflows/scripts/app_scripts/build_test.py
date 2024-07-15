import subprocess
import os

def build_dotnet_project(project_path):
    try:
        # Exécuter la commande de build .NET
        result = subprocess.run(['dotnet', 'build', project_path], check=True, stdout=subprocess.PIPE, stderr=subprocess.PIPE, text=True)
        print("Build réussi:")
        print(result.stdout)
    except subprocess.CalledProcessError as e:
        print("Erreur lors du build:")
        print(e.stderr)

def test_dotnet_project(project_path):
    try:
        # Exécuter la commande de test .NET
        result = subprocess.run(['dotnet', 'test', project_path], check=True, stdout=subprocess.PIPE, stderr=subprocess.PIPE, text=True)
        print("Tests réussis:")
        print(result.stdout)
    except subprocess.CalledProcessError as e:
        print("Erreur lors des tests:")
        print(e.stderr)

if __name__ == "__main__":
    # Chemin du projet .NET
    app_path = os.environ['APP_PATH']

    # Construire le projet
    build_dotnet_project(app_path)

    # Tester le projet
    test_dotnet_project(app_path)
