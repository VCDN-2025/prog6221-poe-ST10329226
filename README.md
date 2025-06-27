# Cybersecurity Awareness Chatbot

## Project Overview

The Cybersecurity Awareness Chatbot is an interactive console-based application (implemented as a Windows Forms Application) designed to educate users about various cybersecurity topics, help them manage related tasks, and test their knowledge through a quiz. It aims to provide a personalized and engaging experience to enhance digital safety awareness.

## Features

* **Personalized Greetings:** The chatbot remembers and uses your name for a more personalized conversation (e.g., "Hello, [Your Name]!"). Names are automatically capitalized with only the first letter (e.g., "sheldon" becomes "Sheldon").
* **Cybersecurity Information Lookup:** Get quick information on essential cybersecurity topics.
* **Task Management:** Add, list, complete, and delete cybersecurity-related tasks.
* **Interactive Quiz:** Test your cybersecurity knowledge with a multi-choice and true/false quiz.
* **Activity Logging:** All interactions and key actions are recorded in an internal activity log, which can be reviewed at any time.
* **Conversation Reset:** Easily reset the conversation to the initial state.
* **Sentiment Recognition (Basic):** The chatbot can detect basic expressions of worry related to scams.

## How to Use

The chatbot responds to various commands and natural language inputs. You can interact with it by typing in the input box and clicking "Send", or using the dedicated buttons.

### General Interaction

* **Start Conversation:** The chatbot will greet you and ask for your name when you open the application or click "Main Menu".
* **Entering Name:**
    * When prompted "What's your name?", simply type your name (e.g., `Sheldon`).
    * You can also explicitly state your name later: `My name is Sheldon` or `Call me Bob`.
* **Greetings:** Type `Hello`, `Hi`, `Good morning`, etc.
* **Farewells:** Type `Goodbye`, `Bye`, `See ya`.
* **Thank You:** Type `Thank you`, `Thanks`.
* **Reset Conversation:** Type `reset` in the input box or click the **"Main Menu"** button. This will clear the chat and reset the chatbot's state.

### Cybersecurity Information

Ask about specific topics. The chatbot recognizes keywords for common cybersecurity subjects.

* **Passwords:** `What is a strong password?`, `Tell me about passwords`, `Password best practices`.
* **Phishing:** `What is phishing?`, `Phishing tips`, `How to spot phishing`.
* **Malware:** `Explain malware`, `What is a virus?`, `Types of malware`.
* **2FA (Two-Factor Authentication):** `What is 2FA?`, `Info on 2FA`, `How does 2FA work?`.
* **Privacy:** `How can I protect my privacy?`, `What is data privacy?`.
* **Scams:** `How to avoid scams?`, `I'm worried about scams`.

### Task Management

Keep track of your cybersecurity-related to-dos.

* **Add a Task:** `Add task: Change password`, `Task: Backup data regularly`.
* **Show Tasks:** `Show tasks`, `List my tasks`.
* **Complete a Task:** `Complete task 1` (where 1 is the task number shown in `Show tasks`), `I finished task "Backup data"`.
* **Delete a Task:** `Delete task 2`, `Remove task "Change password"`.

### Interactive Quiz

Test your knowledge with a quiz related to cybersecurity.

* **Start Quiz:** Type `Quiz` or click the **"Quiz"** button.
* The quiz will present multiple-choice and true/false questions.
* Follow the instructions to answer (e.g., type A, B, C, D or True/False).

### Activity Log

View a chronological record of your interactions with the chatbot.

* **Show Log:** Type `Show activity log`, `Display log`, `What's in the log?`, or click the **"Activity Log"** button (once added and implemented).

## Setup and Installation

To get this project up and running on your local machine, follow these steps:

### Prerequisites

* **Visual Studio 2022:** This project is developed using Visual Studio 2022. You can download the Community Edition for free.
* **.NET Desktop Development Workload:** Ensure you have the ".NET desktop development" workload installed with Visual Studio.
* **.NET Framework 4.7.2 or higher:** The project targets .NET Framework.

### Cloning the Repository

1.  Open your Git Bash or Command Prompt.
2.  Navigate to the directory where you want to store the project.
3.  Clone the repository:
    ```bash
    git clone [https://github.com/VCDN-2025/prog6221-poe-ST10329226.git](https://github.com/VCDN-2025/prog6221-poe-ST10329226.git)
    ```
4.  Navigate into the cloned directory:
    ```bash
    cd prog6221-poe-ST10329226
    ```

### Building the Project

1.  **Open in Visual Studio:** Double-click the `prog6221-poe-ST10329226.sln` file to open the solution in Visual Studio 2022.
2.  **Build Solution:**
    * In Visual Studio, go to `Build` > `Build Solution` (or press `Ctrl + Shift + B`).
    * This will compile the project and create the executable file.

### Running the Application

1.  **From Visual Studio:**
    * After building, simply click the `Start` button (green play icon) in Visual Studio (or press `F5`).
2.  **From Executable:**
    * Navigate to the build output directory: `prog6221-poe-ST10329226\AwarenessChatbot\bin\Debug\` (or `Release\` if you built in Release mode).
    * Double-click `AwarenessChatbot.exe` to run the application.

## GitHub Versioning & Releases

This project utilizes Git for version control. Key milestones and releases are marked with Git tags and described in GitHub Releases.

* **Commit Messages:** Each commit includes a clear, concise message summarizing the changes.
* **Tags & Releases:** Significant versions of the application are marked with [Git tags](https://git-scm.com/book/en/v2/Git-Basics-Tagging) and documented via [GitHub Releases](https://docs.github.com/en/repositories/releasing-projects/managing-releases-in-a-repository). You can find release notes under the "Releases" tab on the GitHub repository page.

---
