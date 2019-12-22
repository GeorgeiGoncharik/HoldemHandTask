# HoldemHand
Application that implements an algorithm for comparing the strength of Texas Hold'em Hands. 
## How to execute...
### on Windows:
Running a program on Windows 10 is quite easy. Everything you need to do:
1. Clone or download the repository and unzip it if necessary
2. In the repo folder, run run this command from the command line: 
3.  ```
    dotnet run < input.txt (optionally: > output.txt)
    ```
![Screenshot of the Windows application](https://github.com/GeorgeiGoncharik/bootcamp-home-task/blob/master/images/runWindows.png)
### on Linux (on the example of Ubuntu):
1. The first thing to do is install the necessary repository. To do this, open a terminal window and issue the following commands: 
    ```
    wget -q https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb
    sudo dpkg -i packages-microsoft-prod.deb
    ```
2. Once the repository has been added, there's a single dependency that must be installed. Do this with the following commands:
    ```
    sudo add-apt-repository universe
    sudo apt-get install apt-transport-https
    ```
3. Install DotNet Core with these commands:
    ```
    sudo apt-get update
    sudo apt-get install dotnet-sdk-3.1
    ```
4. (optional) You can check if everything is set by executing the following command:
    ```
    dotnet --info
    ```
5. Clone or download the repository and unzip it if necessary
6. Open a terminal in the repository folder and issue the following command specifying where to read input:
    ```
    dotnet run < input.txt
    ```
7. The program is launched!
![Screenshot of the Linux application](https://github.com/GeorgeiGoncharik/bootcamp-home-task/blob/master/images/runLinux.png)
