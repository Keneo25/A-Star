using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class GridVisualizer : MonoBehaviour
{
    public GameObject emptyTilePrefab;
    public GameObject obstacleTilePrefab;
    public GameObject pathTilePrefab;
    public GameObject characterPrefab;
    public GameObject errorPanel;
    public Text errorText;
    public Button closeErrorButton;

    private GameObject character;

    void Awake()
    {
        if (errorPanel == null)
        {
            CreateErrorUI();
        }
        
        if (closeErrorButton != null)
        {
            closeErrorButton.onClick.AddListener(HideErrorPanel);
        }
        
        if (errorPanel != null)
        {
            errorPanel.SetActive(false);
        }
        
        GenerateRandomPath();
    }

    void ShowError(string message)
    {
        Debug.LogError(message);
        if (errorPanel != null && errorText != null)
        {
            errorText.text = message;
            errorPanel.SetActive(true);
        }
    }

    void HideErrorPanel()
    {
        if (errorPanel != null)
        {
            errorPanel.SetActive(false);
        }
    }

    void CreateErrorUI()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("ErrorCanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        errorPanel = new GameObject("ErrorPanel");
        errorPanel.transform.SetParent(canvas.transform, false);
        
        Image panelImage = errorPanel.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.9f);
        
        RectTransform panelRect = errorPanel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.3f, 0.4f);
        panelRect.anchorMax = new Vector2(0.7f, 0.6f);
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        GameObject textObj = new GameObject("ErrorText");
        textObj.transform.SetParent(errorPanel.transform, false);
        errorText = textObj.AddComponent<Text>();
        errorText.font = Font.CreateDynamicFontFromOSFont("Arial", 24);
        errorText.fontSize = 24;
        errorText.color = Color.white;
        errorText.alignment = TextAnchor.MiddleCenter;
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0.1f, 0.2f);
        textRect.anchorMax = new Vector2(0.9f, 0.8f);
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        GameObject buttonObj = new GameObject("CloseButton");
        buttonObj.transform.SetParent(errorPanel.transform, false);
        closeErrorButton = buttonObj.AddComponent<Button>();
        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = new Color(0.7f, 0.7f, 0.7f, 1);
        
        GameObject buttonTextObj = new GameObject("ButtonText");
        buttonTextObj.transform.SetParent(buttonObj.transform, false);
        Text buttonText = buttonTextObj.AddComponent<Text>();
        buttonText.text = "OK";
        buttonText.font = Font.CreateDynamicFontFromOSFont("Arial", 20);
        buttonText.fontSize = 20;
        buttonText.color = Color.black;
        buttonText.alignment = TextAnchor.MiddleCenter;
        
        RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0.35f, 0.1f);
        buttonRect.anchorMax = new Vector2(0.65f, 0.2f);
        buttonRect.offsetMin = Vector2.zero;
        buttonRect.offsetMax = Vector2.zero;

        RectTransform buttonTextRect = buttonTextObj.GetComponent<RectTransform>();
        buttonTextRect.anchorMin = Vector2.zero;
        buttonTextRect.anchorMax = Vector2.one;
        buttonTextRect.offsetMin = Vector2.zero;
        buttonTextRect.offsetMax = Vector2.zero;
    }

    public void VisualizeGrid(string filePath)
    {
        if (!File.Exists(filePath))
        {
            ShowError("Plik nie został znaleziony: " + filePath);
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        
        if (lines.Length == 0)
        {
            ShowError("Nie znaleziono drogi - plik jest pusty.");
            return;
        }

        int rows = lines.Length;
        int cols = lines[0].Split(' ').Length;

        bool pathExists = false;
        foreach (string line in lines)
        {
            if (line.Contains("3"))
            {
                pathExists = true;
                break;
            }
        }

        if (!pathExists)
        {
            ShowError("Nie znaleziono drogi do celu!");
            return;
        }

        for (int i = 0; i < rows; i++)
        {
            string[] lineData = lines[i].Split(' ');
            for (int j = 0; j < cols; j++)
            {
                int tileType = int.Parse(lineData[j]);
                Vector3 position = new Vector3(j, rows - i - 1, 0);

                if (tileType == 0)
                    Instantiate(emptyTilePrefab, position, Quaternion.identity, transform);
                else if (tileType == 5)
                    Instantiate(obstacleTilePrefab, position, Quaternion.identity, transform);
                else if (tileType == 3)
                    Instantiate(pathTilePrefab, position, Quaternion.identity, transform);
            }
        }
    }

    private void GenerateRandomPath()
    {
        string mapGeneratorPath = Path.Combine(Application.dataPath, "map_generator.exe");
        if (!File.Exists(mapGeneratorPath))
        {
            Debug.LogError("Cannot find map_generator.exe");
            return;
        }

        var mapGeneratorProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = mapGeneratorPath,
                WorkingDirectory = Application.dataPath
            }
        };
        mapGeneratorProcess.Start();
        
        System.Threading.Thread.Sleep(2000);
        if (!mapGeneratorProcess.HasExited)
        {
            mapGeneratorProcess.Kill();
        }

        string pathResolverPath = Path.Combine(Application.dataPath, "PathResolver.exe");
        if (!File.Exists(pathResolverPath))
        {
            Debug.LogError("Cannot find PathResolver.exe");
            return;
        }

        var pathResolverProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = pathResolverPath,
                WorkingDirectory = Application.dataPath
            }
        };
        pathResolverProcess.Start();
        pathResolverProcess.WaitForExit();
    }
    
    private void Start()
    {
        VisualizeGrid(@"Assets/output.txt");
    }
}
