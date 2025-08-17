# FluentUI for Unity

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![Unity Version](https://img.shields.io/badge/Unity-2020.3%2B-brightgreen)
![Status](https://img.shields.io/badge/status-stable-green)

**FluentUI** is a zero-setup, single-file, code-driven API for creating and managing modern user interfaces in Unity. It leverages a fluent interface (method chaining) to build complex, responsive, and animated UI hierarchies in a clean and readable way, directly from C# code.

This API acts as a powerful wrapper around Unity's native UI system (UGUI) and TextMeshPro, abstracting away the boilerplate of GameObject creation and component management.

## ✨ Key Features

-   **Fluent Interface:** Chain methods together for declarative and highly readable UI code.
-   **Zero Setup:** No prefabs or scene setup required. The API automatically creates a `Canvas` and `EventSystem` if they don't exist.
-   **Code-Driven:** Define the entire UI, including layout, styling, and logic, from a single C# script. Perfect for procedural UIs, tool development, and rapid prototyping.
-   **Fully Customizable:** Control every aspect of your UI elements—colors, fonts, sprites, text, and behavior—at runtime.
-   **Built-in Animations:** Simple, one-line methods for common animations like `FadeIn`, `FadeOut`, `SlideIn`, and `SlideOut`.
-   **Event Handling:** Easily subscribe to UI events like `OnClick`, `OnValueChanged`, and `OnHoverEnter` using C# actions and delegates.
-   **Extensible:** The class-based architecture makes it easy to add your own custom UI components.

## 🚀 Getting Started

### 1. Prerequisites

-   Unity 2020.3 or newer.
-   **TextMeshPro** package installed in your project. You can add it via the Unity Package Manager (`Window > Package Manager`). If prompted, import the "TMP Essentials".

### 2. Installation

1.  Download the `FluentUI.cs` file.
2.  Add the file to your Unity project's `Assets` folder (e.g., in a `Scripts/UI` subfolder).

That's it! The `FluentUI` static class is now globally available in your project.

## 🎓 Core Concepts

The API is built around three main ideas:

1.  **Static Factory (`FluentUI`):** You always start by calling a static method on the `FluentUI` class, like `FluentUI.CreateButton()` or `FluentUI.CreatePanel()`. This creates the GameObject with all necessary components and returns a wrapper object.

2.  **Wrapper Objects (`UIButton`, `UIPanel`, etc.):** Each `Create...` method returns a typed object (e.g., `UIButton`) that exposes specific methods for that element. All of these objects inherit from a base `UIElement` class, which provides common functionality.

3.  **Method Chaining:** Almost every method returns the object itself (`return this;`), allowing you to chain calls together to configure the element in a single, fluid statement.

```csharp
// The basic structure of a fluent call
FluentUI.CreateButton()         // 1. Create the element
    .SetParent(...)             // 2. Configure its properties (layout, style)
    .SetText("Click Me")        // 3. Set element-specific data
    .OnClick(() => { /*...*/ }) // 4. Add event listeners
    .FadeIn(0.5f);              // 5. Animate it
```

## 💡 Usage Examples

Here are some examples to demonstrate how to use the API. Simply create a new C# script on any GameObject in your scene and paste this code into its `Start()` method.

### Example 1: A Simple "Hello, World" Button

This creates a single button in the center of the screen that prints a message to the console when clicked.

```csharp
using UnityEngine;

public class BasicUIExample : MonoBehaviour
{
    void Start()
    {
        FluentUI.CreateButton()
            .SetAnchors(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f)) // Center anchor
            .SetPosition(0, 0)
            .SetSize(200, 50)
            .SetText("Click Me!")
            .SetColor(new Color(0.2f, 0.6f, 0.8f)) // A nice blue
            .OnClick(() => Debug.Log("Button was clicked!"));
    }
}
```

### Example 2: Building a UI Hierarchy

This example creates a main panel and places a title, an image, and a button inside it.

```csharp
using UnityEngine;

public class HierarchyUIExample : MonoBehaviour
{
    void Start()
    {
        // 1. Create the main container panel
        var mainPanel = FluentUI.CreatePanel()
            .SetAnchors(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f))
            .SetPosition(0, 0)
            .SetSize(300, 400)
            .SetColor(new Color(0.2f, 0.2f, 0.2f, 0.9f));

        // 2. Create a title and parent it to the panel
        FluentUI.CreateText()
            .SetParent(mainPanel)
            .SetAnchors(new Vector2(0.5f, 1f), new Vector2(0.5f, 1f)) // Anchor to top-center
            .SetPosition(0, -40)
            .SetSize(280, 40)
            .SetText("Main Menu")
            .SetFontSize(32)
            .SetAlignment(TMPro.TextAlignmentOptions.Center);

        // 3. Create an icon image and parent it to the panel
        FluentUI.CreateImage()
            .SetParent(mainPanel)
            .SetAnchors(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f))
            .SetPosition(0, 50)
            .SetSize(128, 128)
            .SetColor(Color.cyan); // Set a tint color
            // .SetImageSprite(yourSprite); // Assign your sprite here

        // 4. Create a quit button at the bottom
        FluentUI.CreateButton()
            .SetParent(mainPanel)
            .SetAnchors(new Vector2(0.5f, 0f), new Vector2(0.5f, 0f)) // Anchor to bottom-center
            .SetPosition(0, 50)
            .SetSize(150, 40)
            .SetText("Quit")
            .SetColor(new Color(0.8f, 0.3f, 0.3f))
            .OnClick(() => Application.Quit());
    }
}
```

### Example 3: An Animated Login Form

This demonstrates how to use `InputField`, event handling, and animations to create a simple, dynamic form.

```csharp
using UnityEngine;

public class AnimatedFormExample : MonoBehaviour
{
    void Start()
    {
        var formPanel = FluentUI.CreatePanel()
            .SetAnchors(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f))
            .SetPosition(0, 0)
            .SetSize(350, 250);

        var usernameField = FluentUI.CreateInputField()
            .SetParent(formPanel)
            .SetPosition(0, 50)
            .SetSize(300, 40)
            .SetPlaceholderText("Enter username...");

        var passwordField = FluentUI.CreateInputField()
            .SetParent(formPanel)
            .SetPosition(0, -10)
            .SetSize(300, 40)
            .SetPlaceholderText("Enter password...");

        var loginButton = FluentUI.CreateButton()
            .SetParent(formPanel)
            .SetPosition(0, -80)
            .SetSize(120, 45)
            .SetText("Login")
            .SetInteractive(false) // Initially disabled
            .SetColor(Color.gray);

        // Logic: Enable the login button only if both fields have text
        UnityAction<string> validateInput = (text) =>
        {
            bool isValid = !string.IsNullOrEmpty(usernameField.GetText()) && 
                           !string.IsNullOrEmpty(passwordField.GetText());
            
            loginButton.SetInteractive(isValid);
            loginButton.SetColor(isValid ? Color.green : Color.gray);
        };

        usernameField.OnValueChanged(validateInput);
        passwordField.OnValueChanged(validateInput);

        // Action on successful login
        loginButton.OnClick(() =>
        {
            Debug.Log($"Logging in with user: {usernameField.GetText()}");
            formPanel.FadeOut(0.5f, disableAfter: true); // Hide form after login
        });
        
        // Animate the entire form into view
        formPanel.SlideIn(new Vector2(0, -800), 0.7f);
    }
}
```

## 📚 API Reference Overview

This is a high-level overview of the available functions, grouped by category. For detailed parameter information, refer to the XML comments in `FluentUI.cs`.

#### 1. Component Creation (`FluentUI.*`)

-   `CreatePanel()`
-   `CreateButton()`
-   `CreateText()`
-   `CreateImage()`
-   `CreateSlider()`
-   `CreateToggle()`
-   `CreateInputField()`
-   *(CreateDropdown, CreateScrollView, etc. can be added)*

#### 2. Layout & Hierarchy

-   `SetParent(UIElement parent)`
-   `SetAnchors(Vector2 min, Vector2 max)`
-   `SetPosition(float x, float y)`
-   `SetSize(float width, float height)`
-   `SetPadding(float left, float top, float right, float bottom)`
-   `SetMargin(...)`
-   `SetAlignment(TextAlignmentOptions alignment)` (on `UIText`)
-   `SetLayer(int siblingIndex)`

#### 3. Interactions & Events

-   `OnClick(UnityAction action)`
-   `OnHoverEnter(UnityAction<BaseEventData> action)`
-   `OnHoverExit(UnityAction<BaseEventData> action)`
-   `OnValueChanged(UnityAction<T> action)` (on Sliders, Toggles, InputFields)
-   `OnSubmit(UnityAction<string> action)` (on InputFields)
-   `OnDragStart(UnityAction<BaseEventData> action)`
-   `OnDragEnd(UnityAction<BaseEventData> action)`

#### 4. Visibility & Animations

-   `Show()` / `Hide()`
-   `FadeIn(float duration)`
-   `FadeOut(float duration, bool disableAfter)`
-   `SlideIn(Vector2 fromOffset, float duration)`
-   `SlideOut(Vector2 toOffset, float duration)`

#### 5. Appearance Customization

-   `SetColor(Color color)`
-   `SetTextColor(Color color)`
-   `SetFont(TMP_FontAsset font)`
-   `SetFontSize(float size)`
-   `SetIcon(Sprite icon)`
-   `SetImageSprite(Sprite sprite)`
-   `SetTransparency(float alpha)`
-   `SetInteractive(bool isInteractive)`

## 🔧 Extending the API

To create your own custom component (e.g., a circular progress bar), follow these steps:

1.  **Create a factory method** in the `FluentUI` class that builds the `GameObject` hierarchy for your component.
2.  **Create a new class** that inherits from `UIElement` (e.g., `public class UICircularProgress : UIElement`).
3.  **Add custom methods** to your new class that control its unique properties (e.g., `SetProgress(float percentage)`).
4.  Have your factory method return an instance of your new class.

## ⚖️ License

This project is licensed under the MIT License. See the [LICENSE](LICENSE.md) file for details.
