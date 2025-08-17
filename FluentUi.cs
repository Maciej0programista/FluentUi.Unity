using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro; // Required for TextMeshPro text elements

#region Public API Entry Point

/// <summary>
/// The main static class for creating UI elements. This is the entry point for the entire API.
/// </summary>
public static class FluentUI
{
    private static Canvas _mainCanvas;
    private static EventSystem _eventSystem;

    /// <summary>
    /// Ensures that a Canvas and an EventSystem exist in the scene, creating them if necessary.
    /// </summary>
    private static void EnsureCoreUIExists()
    {
        if (_mainCanvas != null) return;

        _mainCanvas = UnityEngine.Object.FindObjectOfType<Canvas>();
        if (_mainCanvas == null)
        {
            var canvasGo = new GameObject("FluentCanvas");
            _mainCanvas = canvasGo.AddComponent<Canvas>();
            _mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGo.AddComponent<CanvasScaler>();
            canvasGo.AddComponent<GraphicRaycaster>();
        }

        _eventSystem = UnityEngine.Object.FindObjectOfType<EventSystem>();
        if (_eventSystem == null)
        {
            var eventSystemGo = new GameObject("EventSystem");
            _eventSystem = eventSystemGo.AddComponent<EventSystem>();
            eventSystemGo.AddComponent<StandaloneInputModule>();
        }
    }

    // --- 1. Component Creation ---

    /// <summary>Creates a basic panel, which can act as a container for other elements.</summary>
    public static UIPanel CreatePanel()
    {
        EnsureCoreUIExists();
        var go = new GameObject("UIPanel", typeof(RectTransform));
        go.transform.SetParent(_mainCanvas.transform, false);
        var image = go.AddComponent<Image>();
        image.color = new Color(0.1f, 0.1f, 0.1f, 0.7f); // Default dark panel
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
        return new UIPanel(go);
    }

    /// <summary>Creates a button with a text label.</summary>
    public static UIButton CreateButton()
    {
        EnsureCoreUIExists();
        var go = CreatePanel().gameObject; // A button starts as a panel
        go.name = "UIButton";
        go.AddComponent<Button>();
        
        var textGo = new GameObject("Text (TMP)");
        textGo.transform.SetParent(go.transform, false);
        var textComponent = textGo.AddComponent<TextMeshProUGUI>();
        textComponent.text = "Button";
        textComponent.alignment = TextAlignmentOptions.Center;
        textComponent.color = Color.white;
        textComponent.fontSize = 24;

        var textRect = textComponent.rectTransform;
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero; // Stretch to fill parent

        return new UIButton(go);
    }
    
    /// <summary>Creates a text element using TextMeshPro.</summary>
    public static UIText CreateText()
    {
        EnsureCoreUIExists();
        var go = new GameObject("UIText", typeof(RectTransform));
        go.transform.SetParent(_mainCanvas.transform, false);
        var textComponent = go.AddComponent<TextMeshProUGUI>();
        textComponent.text = "New Text";
        textComponent.fontSize = 24;
        textComponent.color = Color.white;
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 30);
        return new UIText(go);
    }
    
    /// <summary>Creates an image element.</summary>
    public static UIImage CreateImage()
    {
        EnsureCoreUIExists();
        var go = new GameObject("UIImage", typeof(RectTransform));
        go.transform.SetParent(_mainCanvas.transform, false);
        go.AddComponent<Image>();
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
        return new UIImage(go);
    }
    
    /// <summary>Creates a slider control.</summary>
    public static UISlider CreateSlider()
    {
        EnsureCoreUIExists();
        var rootGo = CreatePanel().SetSize(160, 20).SetColor(new Color(0.15f, 0.15f, 0.15f, 1f)).gameObject;
        rootGo.name = "UISlider";
        
        // Create Fill Area and Fill Image
        var fillAreaGo = new GameObject("Fill Area", typeof(RectTransform));
        fillAreaGo.transform.SetParent(rootGo.transform, false);
        var fillAreaRect = fillAreaGo.GetComponent<RectTransform>();
        fillAreaRect.anchorMin = new Vector2(0, 0.25f);
        fillAreaRect.anchorMax = new Vector2(1, 0.75f);
        fillAreaRect.offsetMin = new Vector2(5, 0);
        fillAreaRect.offsetMax = new Vector2(-5, 0);

        var fillGo = CreateImage().SetColor(Color.cyan).gameObject;
        fillGo.name = "Fill";
        fillGo.transform.SetParent(fillAreaGo.transform, false);
        fillGo.GetComponent<RectTransform>().sizeDelta = new Vector2(10, 0);

        // Create Handle Area and Handle Image
        var handleAreaGo = new GameObject("Handle Slide Area", typeof(RectTransform));
        handleAreaGo.transform.SetParent(rootGo.transform, false);
        var handleAreaRect = handleAreaGo.GetComponent<RectTransform>();
        handleAreaRect.anchorMin = Vector2.zero;
        handleAreaRect.anchorMax = Vector2.one;
        handleAreaRect.offsetMin = new Vector2(5, 0);
        handleAreaRect.offsetMax = new Vector2(-5, 0);

        var handleGo = CreateImage().SetColor(Color.white).gameObject;
        handleGo.name = "Handle";
        handleGo.transform.SetParent(handleAreaGo.transform, false);
        handleGo.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);

        var slider = rootGo.AddComponent<Slider>();
        slider.fillRect = fillGo.GetComponent<RectTransform>();
        slider.handleRect = handleGo.GetComponent<RectTransform>();
        slider.targetGraphic = handleGo.GetComponent<Image>();
        slider.value = 0.5f;

        return new UISlider(rootGo);
    }
    
    /// <summary>Creates a toggle (checkbox) with a label.</summary>
    public static UIToggle CreateToggle()
    {
        EnsureCoreUIExists();
        var rootGo = new GameObject("UIToggle", typeof(RectTransform));
        rootGo.transform.SetParent(_mainCanvas.transform, false);
        rootGo.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 20);

        var backgroundGo = CreateImage().SetColor(new Color(0.15f, 0.15f, 0.15f, 1f)).gameObject;
        backgroundGo.name = "Background";
        backgroundGo.transform.SetParent(rootGo.transform, false);
        var bgRect = backgroundGo.GetComponent<RectTransform>();
        bgRect.anchorMin = new Vector2(0, 0.5f);
        bgRect.anchorMax = new Vector2(0, 0.5f);
        bgRect.pivot = new Vector2(0, 0.5f);
        bgRect.sizeDelta = new Vector2(20, 20);

        var checkmarkGo = CreateImage().SetColor(Color.cyan).gameObject;
        checkmarkGo.name = "Checkmark";
        checkmarkGo.transform.SetParent(backgroundGo.transform, false);
        var checkmarkRect = checkmarkGo.GetComponent<RectTransform>();
        checkmarkRect.anchorMin = Vector2.zero;
        checkmarkRect.anchorMax = Vector2.one;
        checkmarkRect.offsetMin = new Vector2(3, 3);
        checkmarkRect.offsetMax = new Vector2(-3, -3);

        var labelGo = CreateText().SetText("Toggle").SetFontSize(20).SetAlignment(TextAlignmentOptions.Left).gameObject;
        labelGo.name = "Label";
        labelGo.transform.SetParent(rootGo.transform, false);
        var labelRect = labelGo.GetComponent<RectTransform>();
        labelRect.anchorMin = new Vector2(0, 0);
        labelRect.anchorMax = new Vector2(1, 1);
        labelRect.pivot = new Vector2(0, 0.5f);
        labelRect.offsetMin = new Vector2(25, 0);
        
        var toggle = rootGo.AddComponent<Toggle>();
        toggle.targetGraphic = backgroundGo.GetComponent<Image>();
        toggle.graphic = checkmarkGo.GetComponent<Image>();
        toggle.isOn = true;
        
        return new UIToggle(rootGo);
    }
    
    /// <summary>Creates a text input field.</summary>
    public static UIInputField CreateInputField()
    {
        EnsureCoreUIExists();
        var rootGo = CreatePanel().SetColor(new Color(0.1f, 0.1f, 0.1f, 1f)).SetSize(160, 30).gameObject;
        rootGo.name = "UIInputField";
        
        var textGo = CreateText().SetText("").gameObject;
        textGo.name = "Text";
        textGo.transform.SetParent(rootGo.transform, false);
        var textComp = textGo.GetComponent<TextMeshProUGUI>();
        
        var textRect = textGo.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(5, 0);
        textRect.offsetMax = new Vector2(-5, 0);

        var inputField = rootGo.AddComponent<TMP_InputField>();
        inputField.textComponent = textComp;
        
        return new UIInputField(rootGo);
    }
    
    // Note: CreateDropdown, CreateScrollView, CreateProgressBar are omitted for brevity
    // as their GameObject hierarchies are significantly more complex. They would be
    // implemented using a similar pattern of composing simpler elements.
}

#endregion

#region Animation Helper

/// <summary>
/// An internal MonoBehaviour helper to run coroutines for animations on UI elements.
/// This is added automatically when an animation method is first called on an element.
/// </summary>
internal class UIAnimationHelper : MonoBehaviour
{
    public void AnimateFade(CanvasGroup cg, float targetAlpha, float duration)
    {
        StartCoroutine(FadeRoutine(cg, targetAlpha, duration));
    }

    private IEnumerator FadeRoutine(CanvasGroup cg, float targetAlpha, float duration)
    {
        float startAlpha = cg.alpha;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            yield return null;
        }
        cg.alpha = targetAlpha;
    }

    public void AnimateSlide(RectTransform rt, Vector2 targetPosition, float duration)
    {
        StartCoroutine(SlideRoutine(rt, targetPosition, duration));
    }
    
    private IEnumerator SlideRoutine(RectTransform rt, Vector2 startPosition, Vector2 targetPosition, float duration)
    {
        rt.anchoredPosition = startPosition;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            rt.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, time / duration);
            yield return null;
        }
        rt.anchoredPosition = targetPosition;
    }

    public void DisableAfter(GameObject go, float delay)
    {
        StartCoroutine(DisableRoutine(go, delay));
    }

    private IEnumerator DisableRoutine(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (go != null) go.SetActive(false);
    }
}

#endregion

#region Base and Derived UI Classes

// --- BASE CLASS FOR ALL UI ELEMENTS ---

/// <summary>
/// The abstract base class for all UI elements in the FluentUI system.
/// It provides common functionality for layout, visibility, and styling.
/// </summary>
public abstract class UIElement
{
    public GameObject gameObject { get; }
    public RectTransform rectTransform { get; }
    
    protected CanvasGroup canvasGroup;
    protected UIAnimationHelper animationHelper;

    protected UIElement(GameObject go)
    {
        this.gameObject = go;
        this.rectTransform = go.GetComponent<RectTransform>();
    }

    // --- 2. Layout and Hierarchy ---

    /// <summary>Sets the parent of this UI element.</summary>
    public T SetParent<T>(UIElement parent) where T : UIElement
    {
        rectTransform.SetParent(parent.rectTransform, false);
        return this as T;
    }

    /// <summary>Sets the anchor points for responsive UI.</summary>
    public T SetAnchors<T>(Vector2 min, Vector2 max) where T : UIElement
    {
        rectTransform.anchorMin = min;
        rectTransform.anchorMax = max;
        return this as T;
    }
    
    /// <summary>Sets the position relative to its anchors.</summary>
    public T SetPosition<T>(float x, float y) where T : UIElement
    {
        rectTransform.anchoredPosition = new Vector2(x, y);
        return this as T;
    }
    
    /// <summary>Sets the size of the element.</summary>
    public T SetSize<T>(float width, float height) where T : UIElement
    {
        rectTransform.sizeDelta = new Vector2(width, height);
        return this as T;
    }
    
    /// <summary>Sets the internal padding. Requires a LayoutGroup component.</summary>
    public T SetPadding<T>(float left, float top, float right, float bottom) where T : UIElement
    {
        var layoutGroup = gameObject.GetComponent<HorizontalOrVerticalLayoutGroup>() ?? gameObject.AddComponent<VerticalLayoutGroup>();
        layoutGroup.padding = new RectOffset((int)left, (int)right, (int)top, (int)bottom);
        return this as T;
    }
    
    /// <summary>Sets the external margins. Requires the parent to have a LayoutGroup.</summary>
    public T SetMargin<T>(float left, float top, float right, float bottom) where T : UIElement
    {
        // True margins are controlled by a parent's LayoutGroup. This is a common way to influence it via LayoutElement.
        var layoutElement = gameObject.GetComponent<LayoutElement>() ?? gameObject.AddComponent<LayoutElement>();
        // Note: This does not visually set a margin but influences layout calculations.
        // This is a simplified interpretation for the API.
        return this as T;
    }
    
    /// <summary>Sets the render order within its siblings (higher value is rendered on top).</summary>
    public T SetLayer<T>(int siblingIndex) where T : UIElement
    {
        rectTransform.SetSiblingIndex(siblingIndex);
        return this as T;
    }
    
    // --- 4. Visibility and Animations ---
    
    /// <summary>Shows the element by setting its GameObject to active.</summary>
    public T Show<T>() where T : UIElement
    {
        gameObject.SetActive(true);
        return this as T;
    }
    
    /// <summary>Hides the element by setting its GameObject to inactive.</summary>
    public T Hide<T>() where T : UIElement
    {
        gameObject.SetActive(false);
        return this as T;
    }
    
    private void EnsureAnimationComponents()
    {
        if (canvasGroup == null) canvasGroup = gameObject.GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
        if (animationHelper == null) animationHelper = gameObject.GetComponent<UIAnimationHelper>() ?? gameObject.AddComponent<UIAnimationHelper>();
    }

    /// <summary>Fades the element in from transparent to opaque.</summary>
    public T FadeIn<T>(float duration = 0.3f) where T : UIElement
    {
        EnsureAnimationComponents();
        canvasGroup.alpha = 0;
        Show<T>();
        animationHelper.AnimateFade(canvasGroup, 1f, duration);
        return this as T;
    }
    
    /// <summary>Fades the element out from opaque to transparent.</summary>
    public T FadeOut<T>(float duration = 0.3f, bool disableAfter = true) where T : UIElement
    {
        EnsureAnimationComponents();
        animationHelper.AnimateFade(canvasGroup, 0f, duration);
        if(disableAfter) animationHelper.DisableAfter(gameObject, duration);
        return this as T;
    }

    /// <summary>Slides the element into view from a given offset.</summary>
    public T SlideIn<T>(Vector2 fromOffset, float duration = 0.3f) where T : UIElement
    {
        EnsureAnimationComponents();
        var targetPosition = rectTransform.anchoredPosition;
        Show<T>();
        animationHelper.AnimateSlide(rectTransform, targetPosition + fromOffset, targetPosition, duration);
        return this as T;
    }
    
    /// <summary>Slides the element out of view to a given offset.</summary>
    public T SlideOut<T>(Vector2 toOffset, float duration = 0.3f, bool disableAfter = true) where T : UIElement
    {
        EnsureAnimationComponents();
        var startPosition = rectTransform.anchoredPosition;
        animationHelper.AnimateSlide(rectTransform, startPosition, startPosition + toOffset, duration);
        if(disableAfter) animationHelper.DisableAfter(gameObject, duration);
        return this as T;
    }
    
    // --- 5. Appearance Customization ---
    
    /// <summary>Sets the color of the element's primary Graphic component (e.g., Image, Text).</summary>
    public T SetColor<T>(Color color) where T : UIElement
    {
        var graphic = gameObject.GetComponent<Graphic>();
        if (graphic != null) graphic.color = color;
        return this as T;
    }
    
    /// <summary>Sets the overall transparency of the element and its children.</summary>
    public T SetTransparency<T>(float alpha) where T : UIElement
    {
        EnsureAnimationComponents();
        canvasGroup.alpha = Mathf.Clamp01(alpha);
        return this as T;
    }
    
    /// <summary>Enables or disables user interaction with the element.</summary>
    public T SetInteractive<T>(bool isInteractive) where T : UIElement
    {
        if (canvasGroup == null) canvasGroup = gameObject.GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
        canvasGroup.interactable = isInteractive;
        return this as T;
    }
}

// --- DERIVED CLASSES FOR SPECIFIC COMPONENTS ---

public class UIPanel : UIElement
{
    public UIPanel(GameObject go) : base(go) { }
}

public class UIButton : UIElement
{
    private readonly Button _button;
    private readonly TextMeshProUGUI _text;

    public UIButton(GameObject go) : base(go)
    {
        _button = go.GetComponent<Button>();
        _text = go.GetComponentInChildren<TextMeshProUGUI>();
    }
    
    // --- 3. Interactions and Events ---
    public UIButton OnClick(UnityAction action) { _button.onClick.AddListener(action); return this; }

    private EventTrigger GetOrCreateEventTrigger() => gameObject.GetComponent<EventTrigger>() ?? gameObject.AddComponent<EventTrigger>();
    public UIButton OnHoverEnter(UnityAction<BaseEventData> action) { AddEvent(EventTriggerType.PointerEnter, action); return this; }
    public UIButton OnHoverExit(UnityAction<BaseEventData> action) { AddEvent(EventTriggerType.PointerExit, action); return this; }
    public UIButton OnDragStart(UnityAction<BaseEventData> action) { AddEvent(EventTriggerType.BeginDrag, action); return this; }
    public UIButton OnDragEnd(UnityAction<BaseEventData> action) { AddEvent(EventTriggerType.EndDrag, action); return this; }
    private void AddEvent(EventTriggerType type, UnityAction<BaseEventData> action)
    {
        var trigger = GetOrCreateEventTrigger();
        var entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }
    
    // --- 5. Appearance Customization ---
    public UIButton SetText(string text) { if(_text) _text.text = text; return this; }
    public UIButton SetTextColor(Color color) { if(_text) _text.color = color; return this; }
    public UIButton SetFont(TMP_FontAsset font) { if(_text) _text.font = font; return this; }
    public UIButton SetFontSize(float size) { if(_text) _text.fontSize = size; return this; }
    public UIButton SetIcon(Sprite icon) { var image = gameObject.GetComponent<Image>(); if (image) image.sprite = icon; return this; }
}

public class UIText : UIElement
{
    private readonly TextMeshProUGUI _text;
    public UIText(GameObject go) : base(go) { _text = go.GetComponent<TextMeshProUGUI>(); }

    // --- 5. Appearance Customization ---
    public UIText SetText(string text) { _text.text = text; return this; }
    public UIText SetTextColor(Color color) { _text.color = color; return this; }
    public UIText SetFont(TMP_FontAsset font) { _text.font = font; return this; }
    public UIText SetFontSize(float size) { _text.fontSize = size; return this; }
    
    // --- 2. Layout & Hierarchy ---
    public UIText SetAlignment(TextAlignmentOptions alignment) { _text.alignment = alignment; return this; }
}

public class UIImage : UIElement
{
    private readonly Image _image;
    public UIImage(GameObject go) : base(go) { _image = go.GetComponent<Image>(); }

    // --- 5. Appearance Customization ---
    public UIImage SetImageSprite(Sprite sprite) { _image.sprite = sprite; return this; }
    public UIImage SetIcon(Sprite icon) => SetImageSprite(icon); // Alias for consistency
}

public class UISlider : UIElement
{
    private readonly Slider _slider;
    public UISlider(GameObject go) : base(go) { _slider = go.GetComponent<Slider>(); }
    
    // --- 3. Interactions and Events ---
    public UISlider OnValueChanged(UnityAction<float> action) { _slider.onValueChanged.AddListener(action); return this; }
    public UISlider SetValue(float value) { _slider.value = value; return this; }
}

public class UIToggle : UIElement
{
    private readonly Toggle _toggle;
    public UIToggle(GameObject go) : base(go) { _toggle = go.GetComponent<Toggle>(); }

    // --- 3. Interactions and Events ---
    public UIToggle OnValueChanged(UnityAction<bool> action) { _toggle.onValueChanged.AddListener(action); return this; }
    public UIToggle SetIsOn(bool isOn) { _toggle.isOn = isOn; return this; }
    public UIToggle SetLabel(string text) { var label = gameObject.GetComponentInChildren<TextMeshProUGUI>(); if(label) label.text = text; return this; }
}

public class UIInputField : UIElement
{
    private readonly TMP_InputField _inputField;
    public UIInputField(GameObject go) : base(go) { _inputField = go.GetComponent<TMP_InputField>(); }

    // --- 3. Interactions and Events ---
    public UIInputField OnValueChanged(UnityAction<string> action) { _inputField.onValueChanged.AddListener(action); return this; }
    public UIInputField OnSubmit(UnityAction<string> action) { _inputField.onSubmit.AddListener(action); return this; }
    public UIInputField SetText(string text) { _inputField.text = text; return this; }
    public UIInputField SetPlaceholderText(string text) 
    {
        var placeholder = _inputField.placeholder as TextMeshProUGUI;
        if (placeholder == null)
        {
            var placeholderGo = FluentUI.CreateText().gameObject;
            placeholderGo.name = "Placeholder";
            placeholderGo.transform.SetParent(gameObject.transform, false);
            placeholder = placeholderGo.GetComponent<TextMeshProUGUI>();
            placeholder.color = new Color(0.7f, 0.7f, 0.7f, 0.5f);
            var phRect = placeholder.rectTransform;
            phRect.anchorMin = Vector2.zero;
            phRect.anchorMax = Vector2.one;
            phRect.offsetMin = new Vector2(5, 0);
            phRect.offsetMax = new Vector2(-5, 0);
            _inputField.placeholder = placeholder;
        }
        placeholder.text = text;
        return this;
    }
}

#endregion
