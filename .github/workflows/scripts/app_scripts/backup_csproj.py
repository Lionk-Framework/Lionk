import os
# get environment variables
app_path = os.getenv("APP_PATH")

backup_file = app_path + ".bkp"

with open(app_path, "rb") as src:
    content = src.read()

with open(backup_file, "wb") as bkp:
    bkp.write(content)

print(f"Backup of {app_path} created in {backup_file}")

print("Files in the directory:")

for file in os.listdir(os.path.dirname(app_path)):
    print(file)