using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;

namespace Michsky.UI.Reach
{
    [ExecuteInEditMode]
    [AddComponentMenu("Reach UI/Input/Hotkey Event")]
    public class HotkeyEvent : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        // Content
        public HotkeyType hotkeyType = HotkeyType.Custom;
        public ControllerPreset controllerPreset;
        public InputAction hotkey;
        public string keyID = "Escape";
        public string hotkeyLabel = "Exit";

        // Resources
        [SerializeField] private GameObject iconParent;
        [SerializeField] private GameObject textParent;
        [SerializeField] private Image iconObj;
        [SerializeField] private TextMeshProUGUI labelObj;
        [SerializeField] private TextMeshProUGUI textObj;
        [SerializeField] private CanvasGroup normalCG;
        [SerializeField] private CanvasGroup highlightCG;

        // Settings
        public bool useSounds = false;
        public bool useLocalization = true;
        [Range(0, 1)] public float normalOpacity = 0.5f;
        [Range(1, 15)] public float fadingMultiplier = 8;

        // Events
        public UnityEvent onHotkeyPress;

        // Helpers
        bool isInitialized = false;
        public bool useCooldown = false;
        public static bool isInCooldown = false;

#if UNITY_EDITOR
        public bool showOutputOnEditor = true;
#endif

        public enum HotkeyType { Dynamic, Custom }

        void OnEnable()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying && controllerPreset != null && hotkeyType == HotkeyType.Dynamic) { UpdateVisual(); }
            if (!Application.isPlaying) { return; }
#endif
            if (!isInitialized) { Initialize(); }

            UpdateUI();
        }

        void Update()
        {
            if (Application.isPlaying && hotkey.triggered && !useCooldown) { onHotkeyPress.Invoke(); }
            else if (Application.isPlaying && hotkey.triggered && useCooldown && !isInCooldown)
            {
                onHotkeyPress.Invoke();
                StopCoroutine("StartCooldown");
                StartCoroutine("StartCooldown");
            }
        }

        void Initialize()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) { return; }
#endif
            hotkey.Enable();

            if (hotkeyType == HotkeyType.Dynamic && gameObject.GetComponent<Image>() == null)
            {
                Image raycastImg = gameObject.AddComponent<Image>();
                raycastImg.color = new Color(0, 0, 0, 0);
                raycastImg.raycastTarget = true;
            }

            if (ControllerManager.instance != null && hotkeyType == HotkeyType.Dynamic)
            {
                ControllerManager.instance.hotkeyObjects.Add(this);
                controllerPreset = ControllerManager.instance.currentControllerPreset;
            }

            if (useLocalization)
            {
                LocalizedObject localizedObject = gameObject.GetComponent<LocalizedObject>();

                if (localizedObject == null || !localizedObject.CheckLocalizationStatus()) { useLocalization = false; }
                else if (localizedObject != null && !string.IsNullOrEmpty(localizedObject.localizationKey))
                {
                    // Forcing object to take the localized output on awake
                    hotkeyLabel = localizedObject.GetKeyOutput(localizedObject.localizationKey);

                    // Change label text on language change
                    localizedObject.onLanguageChanged.AddListener(delegate
                    {
                        hotkeyLabel = localizedObject.GetKeyOutput(localizedObject.localizationKey);
                        SetLabel(hotkeyLabel);
                    });
                }
            }

            if (UIManagerAudio.instance == null) { useSounds = false; }
            if (useSounds) { onHotkeyPress.AddListener(delegate { UIManagerAudio.instance.audioSource.PlayOneShot(UIManagerAudio.instance.UIManagerAsset.clickSound); }); }

            isInitialized = true;
        }

        public void UpdateUI()
        {
            if (hotkeyType == HotkeyType.Custom)
                return;

            if (hotkeyType == HotkeyType.Dynamic)
            {
                if (controllerPreset == null) { return; }
                if (normalCG != null) { normalCG.alpha = normalOpacity; }
                if (highlightCG != null) { highlightCG.alpha = 0; }

                bool keyFound = false;

                for (int i = 0; i < controllerPreset.items.Count; i++)
                {
                    if (controllerPreset.items[i].itemID == keyID)
                    {
                        keyFound = true;
                        gameObject.SetActive(true);
                        if (labelObj != null) { labelObj.text = hotkeyLabel; }

                        if (controllerPreset.items[i].itemType == ControllerPreset.ItemType.Icon)
                        {
                            if (iconParent != null) { iconParent.SetActive(true); }
                            if (textParent != null) { textParent.SetActive(false); }
                            if (iconObj != null) { iconObj.sprite = controllerPreset.items[i].itemIcon; }
                        }

                        else if (controllerPreset.items[i].itemType == ControllerPreset.ItemType.Text)
                        {
                            if (iconParent != null) { iconParent.SetActive(false); }
                            if (textParent != null) { textParent.SetActive(true); }
                            if (textObj != null) { textObj.text = controllerPreset.items[i].itemText; }
                        }

                        break;
                    }
                }

                if (!keyFound) 
                {
                    // gameObject.SetActive(false);
                    return;
                }
            }

            if (gameObject.activeInHierarchy && Application.isPlaying) { StartCoroutine("LayoutFix"); }
            else
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
                if (normalCG != null) { LayoutRebuilder.ForceRebuildLayoutImmediate(normalCG.GetComponent<RectTransform>()); }
            }
        }

#if UNITY_EDITOR
        public void UpdateVisual()
        {
            if (controllerPreset == null) { return; }
            if (normalCG != null) { normalCG.alpha = normalOpacity; }
            if (highlightCG != null) { highlightCG.alpha = 0; }
            for (int i = 0; i < controllerPreset.items.Count; i++)
            {
                if (controllerPreset.items[i].itemID == keyID)
                {
                    if (controllerPreset.items[i].itemType == ControllerPreset.ItemType.Icon)
                    {
                        if (iconParent != null) { iconParent.SetActive(true); }
                        if (textParent != null) { textParent.SetActive(false); }
                        if (iconObj != null) { iconObj.sprite = controllerPreset.items[i].itemIcon; }
                    }

                    else if (controllerPreset.items[i].itemType == ControllerPreset.ItemType.Text)
                    {
                        if (iconParent != null) { iconParent.SetActive(false); }
                        if (textParent != null) { textParent.SetActive(true); }
                        if (textObj != null) { textObj.text = controllerPreset.items[i].itemText; }
                    }

                    if (labelObj != null) { labelObj.text = hotkeyLabel; }
                    break;
                }
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
            if (normalCG != null) { LayoutRebuilder.ForceRebuildLayoutImmediate(normalCG.GetComponent<RectTransform>()); }
        }
#endif

        public void SetLabel(string value)
        {
            if (labelObj == null)
                return;

            labelObj.text = value;

            if (gameObject.activeInHierarchy == true) { StartCoroutine("LayoutFix"); }
            else
            {
                if (normalCG != null) { LayoutRebuilder.ForceRebuildLayoutImmediate(normalCG.GetComponent<RectTransform>()); }
                LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
                LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onHotkeyPress.Invoke();

            if (useSounds) { UIManagerAudio.instance.audioSource.PlayOneShot(UIManagerAudio.instance.UIManagerAsset.clickSound); }
            if (normalCG == null || highlightCG == null || !gameObject.activeInHierarchy) { return; }

            StopCoroutine("SetHighlight");
            StartCoroutine("SetNormal");
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (useSounds) { UIManagerAudio.instance.audioSource.PlayOneShot(UIManagerAudio.instance.UIManagerAsset.hoverSound); }
            if (normalCG == null || highlightCG == null) { return; }

            StopCoroutine("SetNormal");
            StartCoroutine("SetHighlight");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (normalCG == null || highlightCG == null)
                return;

            StopCoroutine("SetHighlight");
            StartCoroutine("SetNormal");
        }

        IEnumerator LayoutFix()
        {
            yield return new WaitForSecondsRealtime(0.025f);
            if (normalCG != null) { LayoutRebuilder.ForceRebuildLayoutImmediate(normalCG.GetComponent<RectTransform>()); }
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
        }

        IEnumerator SetNormal()
        {
            while (normalCG.alpha > normalOpacity)
            {
                normalCG.alpha -= Time.unscaledDeltaTime * fadingMultiplier;
                highlightCG.alpha -= Time.unscaledDeltaTime * fadingMultiplier;
                yield return null;
            }

            normalCG.alpha = normalOpacity;
            highlightCG.alpha = 0;
        }

        IEnumerator SetHighlight()
        {
            while (highlightCG.alpha < 0.99f)
            {
                normalCG.alpha += Time.unscaledDeltaTime * fadingMultiplier;
                highlightCG.alpha += Time.unscaledDeltaTime * fadingMultiplier;
                yield return null;
            }

            normalCG.alpha = 1;
            highlightCG.alpha = 1;
        }

        IEnumerator StartCooldown()
        {
            isInCooldown = true;
            yield return new WaitForSecondsRealtime(0.05f);
            isInCooldown = false;
        }
    }
}