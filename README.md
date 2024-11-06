# Mouse Cursor Speeder

Curspeeder is a Windows utility application that allows users to dynamically adjust the speed of their mouse cursor. This application features a tray icon where users can manually set the cursor speed from 1 to 20, making it easy to control the sensitivity of the mouse. The application also automatically adjusts the cursor speed based on the mouse device detected.

## Features

- **Dynamic Mouse Speed Adjustment:** Automatically adjusts the mouse speed depending on the connected device.
- **Manual Speed Control:** Provides a tray icon that allows users to manually select a speed from 1 to 20.
- **Tray Icon Notifications:** Displays notifications when the mouse speed is changed.
- **Runs on Startup:** Automatically adds itself to Windows startup to ensure it is always available when the system boots.
- **DPI Awareness:** Properly handles high-DPI displays, ensuring correct scaling.

## Installation

1. **Clone the repository**
   ```sh
   git clone https://github.com/sologub/Curspeeder.git
   ```
2. **Navigate to the project directory**
   ```sh
   cd Curspeeder
   ```
3. **Build the application**
   - Open the solution file (`Curspeeder.sln`) in Visual Studio.
   - Build the solution to generate the executable.

4. **Run the application**
   - Once built, run the executable from the output directory.
   - The application will add itself to startup, display a tray icon, and begin listening for mouse device changes.

## Usage

- **Tray Icon Options:** Right-click the tray icon to access the context menu.
  - **Set Speed [1-20]:** Manually set the speed of the mouse cursor.
  - **Exit:** Closes the application.

- **Automatic Device Detection:** When a USB mouse is connected, the application detects it and sets the speed accordingly.

## Requirements

- **Operating System:** Windows 10 or later.
- **.NET Framework:** .NET Framework 4.8 or higher.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contributions

Contributions are welcome! If you find a bug or have a feature request, feel free to open an issue or submit a pull request.

## Acknowledgments

- This project was developed as a utility to enhance productivity by allowing users to dynamically adjust mouse sensitivity with ease.

## Contact

For any issues, questions, or suggestions, please feel free to reach out:
- **GitHub Issues:** [https://github.com/sologub/Curspeeder/issues](https://github.com/sologub/Curspeeder/issues)
