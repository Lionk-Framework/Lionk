# Stylecop and editor config installation

The following steps will guide you through the installation of Stylecop and editor config in Visual Studio. This will help you to enforce coding standards and conventions in your C# projects.

## Installation

To use Stylecop and editor config in Visual Studio, you only need to copy the configuration files to your project. All the files must be placed near you .sln file.

Three files are required:

- .editorconfig - used to define coding styles and formatting rules for Visual Studio.
- stylecop.json - used to define the rules for Stylecop.
- Directory.Build.props - used to set some information for the build process and to include the Stylecop analyzers.

These files can be found in this folder. Copy them to your project folder near the .sln file and that's all.
