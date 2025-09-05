using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 3f; // Prędkość ruchu
    private List<Vector3> path = new List<Vector3>(); // Lista współrzędnych ścieżki
    private int currentNodeIndex = 0; // Indeks obecnego węzła
    public float rotationSpeed = 10f; // Prędkość rotacji postaci
    public float arrivalThreshold = 0.0001f; // Minimalna odległość do celu, po której postać uznaje dotarcie

    void Start()
    {
        LoadPathFromFile(@"Assets/path.txt");
        StartCoroutine(MoveAlongPath());
    }

    void LoadPathFromFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("Plik z ścieżką nie został znaleziony.");
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        
        if (lines.Length == 0)
        {
            Debug.LogError("Nie znaleziono drogi - plik ze ścieżką jest pusty.");
            return;
        }

        path.Clear();
        foreach (var line in lines)
        {
            string[] parts = line.Split(',');
            if (parts.Length != 2)
            {
                Debug.LogError("Nieprawidłowy format danych w pliku ze ścieżką.");
                path.Clear();
                return;
            }

            if (!int.TryParse(parts[0], out int x) || !int.TryParse(parts[1], out int y))
            {
                Debug.LogError("Nieprawidłowe dane współrzędnych w pliku.");
                path.Clear();
                return;
            }

            path.Add(new Vector3(x, y, 0));
        }

        if (path.Count == 0)
        {
            Debug.LogError("Nie znaleziono drogi do celu!");
            return;
        }

        Debug.Log("Ścieżka załadowana pomyślnie.");
    }
    IEnumerator MoveAlongPath()
    {
        if (path.Count == 0)
        {
            Debug.LogError("Nie można rozpocząć ruchu - brak ścieżki.");
            yield break;
        }

        while (currentNodeIndex < path.Count)
        {
            Vector3 targetPosition = path[currentNodeIndex];
            while (Vector3.Distance(transform.position, targetPosition) > arrivalThreshold)
            {
                
                Vector3 direction = (targetPosition - transform.position).normalized;

              
                float step = rotationSpeed * Time.deltaTime;
                Vector3 newDirection = Vector3.RotateTowards(transform.up, direction, step, 0f);

                
                transform.up = newDirection;

               
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                yield return null; 
            }

            
            currentNodeIndex++;
        }

        Debug.Log("Postać dotarła do końca ścieżki.");
    }
}
