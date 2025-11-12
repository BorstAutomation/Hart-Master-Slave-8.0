# Hart Communication Slave and Master

## C++ Source Code for Real Time Systems

In this repository you will find the c++ source code for a Hart Communication Protocol master and a slave that is designed to be used in an embedded real time systems system. The Hart protocol itself and the interfaces to the application are written in C++. I placed great importance on storing all functions and declarations that are independent of a slave or master in common directories. About **50 % of the source codes for slave and master** are shared by both applications.

## HART (4..20) and HART IP(Internet Protocol)

Both classic **Hart (4..20 mA)** and the **Hart Ip** version are supported.

## Interfaces

The slave or the master each have three interfaces (**OSAL**, **UI** and **HAL**). One connects the communication driver with the application (UI), the OSAL connects the platform-specific implementation of the system environment (Windows, FreeRTOS) and finally a HAL is creating the bridge to the hardware (if required).

## Hart Field Devices in a Nutshell

As a universal option for testing (and also for development), I used a **Windows DLL** for the platform frame of the master and the slave, each of which is visualized with a **C# app**. 

Both implementations include the ability to create a **Windows simulation of field devices**. This means that around 95% of field device firmware can be conveniently developed in Visual Studio on a PC. This is much easier and more efficient than working on the final target hardware.

## Detailed Information

Further details about the two software/firmware packages can be found in a data sheet.

Data Sheet Master: https://walter-borst.de/documents/DataSheet-Hart-Master-C++-8.0.pdf

Data Sheet Slave: https://walter-borst.de/documents/DataSheet-Hart-Slave-C++-8.0.pdf

## Getting Started

### Opening a Solution

1. Open the solution **.\03-Slave\CppHartSlave-8.0.sln** or **.\03-Slave\CppHartMaster-8.0.sln** with Visual Studio 2022.

   It has to be 2022. Older versions are not supported.
   Unless you have 2022 not installed on your computer. You can download it from Microsoft:
   https://visualstudio.microsoft.com/de/downloads/
   The community version is sufficient enough and free of charge. :-)

2. Perform a **'Build All'**.

3. Start debugging ..
   .. and investigate the source code.

### Starting Master and Slave Simulation

Execute the batch file: **.\04-Test\Windows\StartMasterAndSlave.bat**.

### Testing with the Windows HART-IP Client from FieldComm Group

#### Building the Test Client

The Hart Ip client I used is available on Github:

https://github.com/FieldCommGroup/WindowsHartIpClient.

Once you have downloaded the content, you can open the solution **HartIPClient.sln** by Visual Studio 2022.

**Remove** the project **Install** and do a '**Build all**'. Then you will find the executable in the following subdirectory:

**.\HartIPClient\bin\Debug**.

Please note that our c++ Slave only implements the **TCP** variant.

#### Starting the Slave

Start the C++ Slave Test executable:

**.\\03-Slave\03-Test\01-Windows\03-DebugBench\BaTestHartSlave.exe**.

Start the Hart Ip Client in the other directory: **.\HartIPClient\bin\Debug\HartIpClient.exe**.

Click on the large icon in the upper left corner of the test client window to establish a connection to the slave.

In the new window that appears, enter an address of the slave. The default address is: **127.0.0.2**.

Click **OK**.

If everything worked, the test client is now connected to the slave and you can try everything out.

## Directory Structure

### Overview

| Directory             | Description                                                  |
| --------------------- | ------------------------------------------------------------ |
| .\01-Master&Slave     | Items which are common to the master and the slave implementation. |
| .\01-Master&Slave\C++ | C++ modules common to master and slave.                      |
| .\01-Master&Slave\C#  | C# modules common to master and slave.                       |
| .\02-Master           | Implementation of the Hart master.                           |
| .\03-Slave            | Implementation of the Hart slave.                            |
| .\04-Test\Windows     | Contains a batch script for starting the master and the slave at once. |

### .\02-Master

 This path contains the C++ sources for the master and a test client implementation in C# code.

| Directory                                           | Description                                                  |
| --------------------------------------------------- | ------------------------------------------------------------ |
| .\02_Master                                         | Contains the VS 2022 solution with the C++ master sources for a device simulation (DLL) and the C# sources for a test client. |
| .\02_Master\01_Docu                                 | Documentation                                                |
| .\02_Master\02_Code                                 | Source codes for the device                                  |
| .\02_Master\02_Code\01_Common                       | Hardware independent C++ sources.                            |
| .\02_Master\02_Code\02_Specific                     | Hardware specific C++ sources                                |
| .\02_Master\02_Code\02_Specific\01_WinDLL           | Sources for a Windows simulation DLL and the project file for VS 2022 |
| .\02_Master\02_Code\02_Specific\01_WinDLL\01_Shell  | User (Test Client) interface of the DLL                      |
| .\02_Master\02_Code\02_Specific\01_WinDLL\\02_OSAL  | Operating System Abstraction Layer                           |
| .\02_Master\02_Code\02_Specific\01_WinDLL\\03_Build | Build output for the compiler                                |
| .\02_Master\02_Code\02_Specific\02_Nrf52832         | Space for a specific embedded project                        |
| .\02_Master\03_Test                                 | Space for test clients                                       |
| .\02_Master\03_Test\01_Windows                      | Space for Windows test packets                               |
| .\02_Master\03_Test\01_Windows\01_Docu              | Documentation                                                |
| .\02_Master\03_Test\01_Windows\02_Apps              | Executable test clients                                      |
| +\01_TestClientMaster                               | Test client for the master (VS 2022 project for a C# application) |
| +\01_TestClientMaster\01_Main                       | Main form of the test client                                 |
| +\01_TestClientMaster\02_Modules                    | Further C# submodules                                        |
| +\01_TestClientMaster\02_Modules\01_Helpers         | Helper routines                                              |
| +\01_TestClientMaster\02_Modules\02_Forms           | Additional Windows forms                                     |
| +\01_TestClientMaster\02_Modules\03_TestClient      | Special test routines                                        |
| +\01_TestClientMaster\02_Modules\04_HartMasterIntf  | C# interface to the C++ DLL                                  |
| +\01_TestClientMaster\03_DebugBench                 | All run time libraries and required executables are copied into this directory. The exe file can be executed in here. |
| +\01_TestClientMaster\04_Results                    | Not yet used.                                                |

### .\03-Slave

The subdirectory for the slave is structured exactly the same as the one for the master. Therefore, it makes no sense to list it all again here.

## Visual Studio 2022 Solutions

This package contains two solutions for Visual Studio 2022, one for a Hart Master and one for a Hart Slave.

### Hart Master

The solution can be found in the following path: **.\02-Master\CppHartMaster-8.0.sln**.

#### Windows DLL (C++)

The solution has two projects. The project with the source code for the Hart Master is located at the following path: **.\02-Master\02-Code\02-Specific\01-WinDLL\HartMasterDLL.vcxproj**.

The project is specific because a Windows library (DLL) is created from the sources of the master in which the 'device' works. To speak of a simulation here is actually wrong, because the core of the master runs in a Windows thread at millisecond intervals and behaves exactly as the implementation on an embedded system would.

Regarding the source code paths, it should be noted that the modules that are included by master and slave can be found in a special area: **.\01-Master&Slave\01-C++**.

#### Windows EXE (C#)

A Windows DLL is of course not able to run on its own. To use it, you need an executable application. The C# sources and the project for this test client can be found at the following location:

**.\02-Master\03-Test\01-Windows\02-Apps\01-TestClientMaster\BaTestHartMaster.csproj**.

The sources for the test clients also contain a whole series of modules that are shared between master and slave. These are located in the folder: **.\01-Master&Slave\02-C#**.

To make things less complicated, it is best if the DLL and the executable are in the same path. I called this special directory DebugBench: **.\02-Master\03-Test\01-Windows\03-DebugBench**.

### Hart Slave

The structure of the slave is the same as that of the master, with the only difference being that the term 'Master' is replaced by the term 'Slave'.

## Coding Conventions

### General

Indent is 4 spaces (tabs are avoided).

Allman style is preferred

```
while (x == y)
{
    foo();
    bar();
}
```

### Variables Naming

#### Snake Case

| Format Example   | Variable Type                                |
| ---------------- | -------------------------------------------- |
| local_variable   | Variable with local scope.                   |
| function_param_  | A function parameter has tailing underscore. |
| m_member_var     | Basic type private member variable.          |
| mo_member_object | Complex object (class) member.               |
| s_member_var     | Basic type static private member variable.   |
| so_member_object | Complex static object member.                |

#### Pascal Case

| Format Example | Variable Type                             |
| -------------- | ----------------------------------------- |
| PublicVariable | Variable with public or internal scope.   |
| PublicObject   | Object with public or internal scope.     |
| AnyMethod      | No difference between public and private. |

## Abbreviations

| Abbreviation | Description                                                  |
| ------------ | ------------------------------------------------------------ |
| OSAL         | <u>O</u>perating <u>S</u>ystem <u>A</u>bstraction <u>L</u>ayer |
| UI           | <u>U</u>ser <u>I</u>nterface                                 |
| HAL          | <u>H</u>ardware <u>A</u>bstraction <u>L</u>ayer              |
| FreeRTOS     | Free <u>R</u>eal <u>T</u>ime <u>O</u>perating <u>S</u>ystem  |
| VS           | <u>V</u>isual <u>S</u>tudio                                  |

## Contact

**Walter Borst**

Kapitaen-Alexander-Strasse 39

27472 Cuxhaven, Germany

Fon: +49 4721 6985100

E-Mail: Hart@walter-borst.de

Walter Borst, Cuxhaven, 12.11.2025
