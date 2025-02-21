# Expense Tracker App

## Overview
The **Expense Tracker App** is a mobile and desktop application developed using .NET MAUI that helps users manage their daily expenses efficiently. It allows users to record, categorize, and analyze their financial transactions with a user-friendly interface.

## Features
- **Cross-platform support** (Android, iOS, Windows, macOS)
- **User authentication** (Sign up, login, and password reset)
- **Expense tracking** (Add, edit, and delete expenses)
- **Categories management** (Custom expense categories)
- **Expense reports** (View spending trends via charts and summaries)
- **Dark mode support**
- **Cloud sync** (Optional integration with cloud storage for data backup)

## Technologies Used
- **.NET MAUI** (Multi-platform UI framework)
- **SQLite** (Local database for offline data storage)
- **Entity Framework Core** (For database operations)
- **MVVM Architecture** (Ensuring clean and maintainable code)
- **Syncfusion / Microcharts** (For data visualization, if used)

## Installation Guide
### Prerequisites
- Install **.NET SDK 7.0 or later**
- Install **Visual Studio 2022** with MAUI workload
- Ensure **Android/iOS emulators or physical devices** are available for testing

### Steps to Run
1. Clone the repository:
   ```sh
   git clone https://github.com/yourusername/ExpenseTrackerApp.git
   cd ExpenseTrackerApp
   ```
2. Open the project in **Visual Studio 2022**.
3. Restore dependencies:
   ```sh
   dotnet restore
   ```
4. Build and run the project:
   ```sh
   dotnet build
   dotnet maui run android  # For Android
   dotnet maui run windows  # For Windows
   ```

## Usage Guide
1. **Sign up/Login** to access your expense dashboard.
2. **Add new expenses** by specifying the category, amount, and date.
3. **View reports** to analyze your spending habits.
4. **Edit or delete** transactions as needed.
5. **Enable sync** to back up your data online (if cloud sync is enabled).

## Contribution
If you’d like to contribute:
1. Fork the repository.
2. Create a new branch (`feature-name`).
3. Commit and push your changes.
4. Submit a pull request.

## License
This project is licensed under the **MIT License**.

## Contact
For questions or feedback, reach out via:
- Email: your-email@example.com
- GitHub Issues: [https://github.com/yourusername/ExpenseTrackerApp/issues](https://github.com/yourusername/ExpenseTrackerApp/issues)

