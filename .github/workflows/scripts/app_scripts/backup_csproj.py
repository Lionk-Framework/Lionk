import os
# get environment variables
app_path = os.getenv("APP_PATH")

backup_file = os.path.join(app_path,".bkp")

with open(app_path, "rb") as src:
    content = src.read()

with open(backup_file, "wb") as bkp:
    bkp.write(content)
