# Development and Deployment Workflow

## 0. Workflow Diagram

![workflow diagram](workflow.svg "workflow diagram").

The workflow is divided into 3 main branches: `main`, `dev`, and `feature branches`.
The main branch is the production branch, the dev branch is the development branch, and the feature branches are the branches where the features are developed. Anybody can create new feature branches from the dev branch. When the feature is ready, a pull request is created from the feature branch to the dev branch. The CI/CD pipeline will run tests and build the project. If the tests are passed, the PR will be merged to the dev branch after reviews. Once the dev branch is ready to be released, a pull request is created from the dev branch to the main branch. The CI/CD pipeline will run tests and build the project. If the tests are passed, the PR will be merged to the main branch after reviews. 

Once the PR is merged, the CI/CD pipeline will deploy the release. The artifacts deployed depend on the modifications made in the PR. The version is automatically updated by the CI/CD pipeline. The `.csproj` file is updated with the new version number, changelog, and description. The PR title and body must be specific to the type of modification made. If the PR title or body is not correct, the PR will just be closed and re-opened. If the tests are not passed, the PR must be corrected and pushed again until the tests are passed.

When the PR is finally merged into main, new NuGet package versions are created and published to the NuGet repository if needed. If the base app is modified, a new docker image is created and published to the Docker Hub repository. If the documentation is modified, the documentation is updated and published to the GitHub Pages repository. If the workflow is modified, the workflow is updated and published to the GitHub repository.

## 1. Workflow Steps

### 1.1. Create your branch 
- When a new feature or bug fix needs to be developed, create a new branch from the `dev` branch.

### 1.2. Make your changes and merge it to dev branch
- Make your changes and push them to your branch.
- Create a pull request from your branch to the `dev` branch.
- The CI/CD pipeline will run tests and build the project.
- If the tests are passed, the PR will be merged to the `dev` branch after reviews.

### 1.3. Deploy release and Create a Pull Request with specific Pull Request name and body into main
Once develop branch is ready to be released, create a pull request from `dev` to `main` branch.

For each type of modification, like `app`, `nuget`, `doc` ,`wof` , use specific Pull Request names and bodies.

`<version type>` will be `patch`, `minor`, or `major`

**For code changes:**
The version is automatically updated by the CI/CD pipeline. The `.csproj` file is updated with the new version number, changlog and description.

Keep in mind that the pull request workflow creates a commit of .csproj files. Even if it fails, the commit is overwritten by another commit. In any case, you must pull the latest modifications before making a new commit, until the tests have been passed.

#### 1.3.1. If you want to merge application features or bug fixes
PR name: `app: <version type> `
PR body: 
```
- feature or bug fix 1
- feature or bug fix 2
- feature or bug fix 3
```

**example:**

PR name: `app: patch`
PR body: 
```
- fix mainwindows bug
- fix dependency injection bug
```

#### 1.3.2. If you want to merge nuget library features or bug fixes

PR name: `nuget: project1 <version type>, project2 <version type>, project3 <version type>, ...  `
PR body: 
```
project1
- feature or bug fix 1
- feature or bug fix 2

project2
- feature or bug fix 1

project3
- feature or bug fix 1
- feature or bug fix 2
- feature or bug fix 3
```

**example:**

PR name: `nuget: Logger patch, Authentification minor, Core major`
PR body: 
```
Logger
- fix write file bug

Authentification
- fix login bug
- Add pink logout button because it's cool

Core
- Change the way to get the user
- Replace API Urls
```

#### 1.3.3. If you want to merge documentation

To merge documentation, it is important that the code is **not** modified, which is why tests are performed on file extensions. By default, a list of extensions is defined as documentation. If you want to add other extensions, you need to list them in the body in this way:
```
- docx
- xlsx
```
(no dot)

Default extensions that do not need to be entered in the PR body:
```
- md
- txt
- drawio
- png
- jpg
- jpeg
- gif
- svg
```


PR name: `doc: <Name or text> [optional]`
PR body: 
```
- <authored file extension>
- <authored file extension>
- <authored file extension>
```

#### 1.3.4. If you want to merge workflow
to merge a workflow, the procedure is similar to the documentation merge procedure, with the exception of the default file extensions 

Default extensions that do not need to be entered in the PR body:
```
- yml
- py
- sh
```

PR name: `wof: <Name or text> [optional]`
PR body: 
```
- <authored file extension>
- <authored file extension>
- <authored file extension>
``` 

### 1.4. Check failed

If the check fails, look at the error message in the action tab https://github.com/Lionk-Framework/Lionk/actions

Then, if the check fails because the PR title or body is not correct, the PR will just be closed and re-opened. You don't need to create a new PR.

If the check fails because the tests are not passed, you must correct the code and push again until the check passes.

# 2. Workflow runnner

if you want to use your own runner, you need to modify the GitHub action variable named RUNNER_DISTRIBUTION. 
- To use your own runner: `self-hosted`.
- To use Github runners: `ubunutu-latest`

# 2.1 Self-hosted runner

When using a self-hosted runner, pull-requests from external contributors should **never** be validated, as they may result in malicious code or secret leaks.

To guarantee security, I suggest using docker-based runners to compartmentalize the application and, above all, shut it down once it's no longer needed.

To get a token for a self-hosted runner, you need to folow this instructions:
- Go here: https://github.com/organizations/Lionk-Framework/settings/actions/runners 
- Click on the button `Add runner`
- Look at the configuration section
- Copy the token that is displayed after `--token`

here a docker-compose file to run a self-hosted runner:

```yaml
version: '2.3'

services:
  runner:
    image: myoung34/github-runner:latest
    environment:
      REPO_URL: https://github.com/votre-utilisateur/votre-repo
      RUNNER_NAME: <runner name>
      RUNNER_TOKEN: <the token you copied>
      RUNNER_WORKDIR: /tmp/github-runner
      RUNNER_LABELS: <custom labels>
    volumes:
      - ./runner:/tmp/github-runner
      - /var/run/docker.sock:/var/run/docker.sock
    restart: always
```

**example:**

```yaml
version: '2.3'

services:
  runner:
    image: myoung34/github-runner:latest
    environment:
      REPO_URL: https://github.com/Lionk-Framework/Lionk
      RUNNER_NAME: Alex-lionk-runner
      RUNNER_TOKEN: <the token you copied>
      RUNNER_LABELS: linux,x64,ubuntu
    volumes:
      - ./runner:/tmp/github-runner
      - /var/run/docker.sock:/var/run/docker.sock
    restart: always
```

# 2.2 Workflow parallelization

If your workflow has parallel tasks, you can set up self-hosted runners in a simple way, by adding many other services to the docker-compose file.
You need to change the folder name of the volumes to `./runner1`, `./runner2`, `./runner3`, ... and change the `RUNNER_NAME`.
The token can be the same for all runners.

```yaml
version: '2.3'

services:
    runner1:
        image: myoung34/github-runner:latest
        environment:
            REPO_URL: https://github.com/Lionk-Framework/Lionk
            RUNNER_NAME: Alex-lionk-runner-1
            RUNNER_TOKEN: <the token you copied>
            RUNNER_LABELS: linux,x64,ubuntu
        volumes:
        - ./runner1:/tmp/github-runner
        - /var/run/docker.sock:/var/run/docker.sock
        restart: always


    runner2:
        image: myoung34/github-runner:latest
        environment:
            REPO_URL: https://github.com/Lionk-Framework/Lionk
            RUNNER_NAME: Alex-lionk-runner-2
            RUNNER_TOKEN: <the token you copied>
            RUNNER_LABELS: linux,x64,ubuntu
        volumes:
        - ./runner2:/tmp/github-runner
        - /var/run/docker.sock:/var/run/docker.sock
        restart: always    
```