# 🛠️ Contributing Guidelines

### Tools

To contribute to the Lionk project effectively, make sure you have the following tools and configurations set up:

1. **Visual Studio 2022**: The main development environment for this project is Visual Studio 2022. Ensure you have it installed on your machine. You can download it from the [official Visual Studio website](https://visualstudio.microsoft.com/).

   - Install the **.NET Desktop Development** workload to have all necessary tools and libraries for C# development.
   - Configure Visual Studio to use the latest **.NET SDK** compatible with the project.

2. **.editorconfig**: The project includes an `.editorconfig` file to define coding styles and conventions across the team. Make sure your Visual Studio is configured to respect `.editorconfig` settings.
   - Visual Studio should automatically pick up these settings, but you can check this by going to **Tools > Options > Text Editor > Code Style > General** and ensuring "Use EditorConfig settings" is checked.

3. **StyleCop Analyzers**: We use **StyleCop** to enforce coding standards and ensure consistency across the codebase.
   - The project includes a `stylecop.json` file that configures StyleCop rules.
   - Make sure StyleCop is enabled in Visual Studio. You can check this in **Tools > Options > Text Editor > C# > Code Style > Enforce code style on build**.
   - To manually run StyleCop analysis, right-click on your project in **Solution Explorer** and select **Analyze and Code Cleanup > Run Code Analysis**.

4. **NuGet Packages**: Ensure all required NuGet packages are restored when you open the solution in Visual Studio. If they are not restored automatically, you can run:
   ```sh
   dotnet restore
   ```
   This will download and install all the necessary dependencies.

5. **Directory.Build.Props**: The `Directory.Build.Props` file is used to define build properties and settings that apply to all projects within the solution.
   - Do not modify this file unless necessary. If changes are needed, ensure they do not conflict with existing settings or create build issues.

By ensuring you have these tools and configurations in place, you can contribute efficiently and maintain the high-quality standards of the Lionk project.

---

### Workflow

0. **Open an Issue**: Before making any changes, open a new issue in the repository. Clearly describe the problem or feature you want to address, including any relevant context, use cases, or screenshots. This step helps us track work and discuss possible solutions before development begins.

1. **Fork the Repository**: Start by forking the project repository to your GitHub account. This will create a copy of the repository under your control.

2. **Clone Locally**: Clone the forked repository to your local machine using a Git client.
   ```sh
   git clone https://github.com/your-username/Lionk
   ```

3. **Create a New Branch**: Always work on a new branch. Make sure to name your branch in a way that clearly reflects the issue number and the

 nature of your changes. For example:
   ```sh
   git checkout -b issue-#123-add-new-feature-x
   ```
   Ensure that your branch name includes the issue number (e.g., `issue-#123`) to maintain traceability.

4. **Develop Your Changes**: Make your changes in the new branch. Ensure that your code is clean, follows the project's coding standards, and is well-documented. Make small, incremental changes and commit often.

5. **Write Unit Tests**: Ensure that any new functionality is thoroughly covered by unit tests. Also, make sure to run the existing test suite to confirm that no existing functionality is broken.
   - To run tests, you can use the following command:
     ```sh
     dotnet test
     ```

6. **Commit Your Changes**: Once you are satisfied with your changes and tests, commit your updates with a clear and descriptive commit message. Reference the issue number in your commit message for better traceability.
   ```sh
   git commit -m 'Fixes #123: Implemented new feature x.'
   ```

7. **Push to GitHub**: Push your branch to your forked repository.
   ```sh
   git push origin issue-#123-add-new-feature-x
   ```

8. **Submit a Pull Request (PR)**: Go to the original repository and create a Pull Request (PR) from your forked repository. Make sure to:
   - Reference the issue number in the PR description (e.g., "Fixes #123") to automatically link the PR to the issue.
   - Provide a detailed description of the changes, why they are necessary, and any potential impacts.
   - Ensure your PR adheres to the project's contribution guidelines and passes all continuous integration (CI) checks.

9. **Request a Review**: Tag project maintainers or relevant team members for a review. Be open to feedback and be prepared to make changes if requested.

10. **Ensure Tests Pass**: Make sure all CI checks and tests pass before the PR is merged. This includes running unit tests, linting, and any other checks that are part of the CI process.

11. **Merge and Celebrate**: Once your PR is reviewed, approved, and all tests are passing, it will be merged into the main branch. Congratulations on your contribution!

12. **Close the Issue**: After the PR is merged, ensure that the associated issue is closed automatically by including "Fixes #123" in the PR description, or manually close it if needed.