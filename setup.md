### Setup of an empty stand-alone project
1. Create a Console App (.Net Framework) in Visual Studio
1. Install the Nuget package [OpenTK](hhttps://www.nuget.org/packages/OpenTK/3.0.0-pre). An [OpenTK Manual](https://github.com/mono/opentk/blob/master/Documentation/Manual.pdf).
1. Install either Nuget package [Zenseless](https://www.nuget.org/packages/Zenseless/) or [Zenseless.sources](https://www.nuget.org/packages/Zenseless.sources/)
	+ [Zenseless](https://www.nuget.org/packages/Zenseless/) is a .Net Framework 4.6 assembly package.
	+ [Zenseless.sources](https://www.nuget.org/packages/Zenseless.sources/) is a source package. The sources of Zenseless will be compiled along-side your project.

### Setup of full framework
1. download framework
	1. create empty dir
	1. change into empty dir
	+ repository clone [TortoiseGit](https://tortoisegit.org/)
		1. right click `git clone...`
		1. URL: https://github.com/danielscherzer/Framework
	+ repository clone (shell)
		1. open `git cmd` or `git bash`
		1. `git clone https://github.com/danielscherzer/Framework`
1. compile and run
	1. open solution file (`all.sln`)
	1. build solution

### Understanding versioning with git
+ [What is version control?](https://de.atlassian.com/git/tutorials/what-is-version-control)
+ [Quick introduction](https://rogerdudler.github.io/git-guide/index.de.html)
+ [Detailed tutorials](https://de.atlassian.com/git/tutorials/)

### GLSL syntax highlighting
+ [notepad++ GLSL syntax highlighting and intelisense](/glslExtensions/notepadpp)
+ [visual studio (incl. 2015) shader highlighting](http://www.horsedrawngames.com/shader-syntax-highlighting-in-visual-studio-2013/)
+ [Visual Studio 2015 extension by me (alpha state)](/glslExtensions/GLSLintegration.vsix)
