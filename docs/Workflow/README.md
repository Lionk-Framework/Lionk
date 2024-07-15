# Development and Deployment Workflow

## 0. Workflow Diagram

![workflow diagram](workflow.svg "workflow diagram").

## 1. Workflow Steps

### 1.1 Create your branch 
- When a new feature or bug fix needs to be developed, create a new branch from the `main` branch.

### 1.2 Make Changes and Create a Pull Request with specific Pull Request name and body
For each type of modification, like `app`, `nuget`, `doc` ,`wof` , use specific Pull Request names and bodies.

`<version type>` will be `patch`, `minor`, or `major`

**For code changes:**
The version is automatically updated by the CI/CD pipeline. The `.csproj` file is updated with the new version number, changlog and description.

Keep in mind that the pull request workflow creates a commit of .csproj files. Even if it fails, the commit is overwritten by another commit. In any case, you must pull the latest modifications before making a new commit, until the tests have been passed.

#### 1.2.1 If you want to merge application features or bug fixes
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

#### 1.2.2 If you want to merge nuget library features or bug fixes

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

#### 1.2.3 If you want to merge documentation

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

#### 1.2.4 If you want to merge workflow
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

### 1.3 Check failed

If the check fails, look at the error message in the action tab https://github.com/Lionk-Framework/Lionk/actions

Then, if the check fails because the PR title or body is not correct, the PR will just be closed and re-opened. You don't need to create a new PR.

If the check fails because the tests are not passed, you must correct the code and push again until the check passes.