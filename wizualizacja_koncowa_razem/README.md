# Wizualizacja Algorytmu A* w Unity

## Opis projektu

Projekt składa się z dwóch głównych komponentów:
1. **Algorytm A*** - implementacja w C# (aplikacja konsolowa)
2. **Wizualizacja Unity** - graficzna reprezentacja działania algorytmu

System umożliwia generowanie losowych map z przeszkodami, znajdowanie optymalnej ścieżki algorytmem A* oraz wizualizację tej ścieżki w środowisku Unity z animowaną postacią.


## Funkcjonalności

### Algorytm A*
- Implementacja klasycznego algorytmu A* do znajdowania najkrótszej ścieżki
- Obsługa map 2D z przeszkodami
- Heurystyka euklidesowa
- Eksport wyników do plików tekstowych

### Wizualizacja Unity
- Automatyczne ładowanie i wizualizacja map
- Graficzna reprezentacja:
  - Wolne pola (białe)
  - Przeszkody (czerwone/czarne)
  - Ścieżka (niebieska/zielona)
- Animowana postać poruszająca się po znalezionej ścieżce
- Obsługa błędów z interfejsem użytkownika
- Generator losowych map

## Wymagania

### Dla algorytmu A*
- .NET 8.0 lub nowszy
- Visual Studio 2022 lub JetBrains Rider

### Dla wizualizacji Unity
- Unity 2022.3 LTS lub nowszy
- Pakiety Unity:
  - Universal Render Pipeline (URP)
  - Input System

## Instalacja i uruchomienie

### 1. Algorytm A*

```bash
cd Algorytm_koncowy_razem
dotnet build
dotnet run
```

Algorytm:
- Wczytuje mapę z pliku `grid.txt`
- Znajduje ścieżkę z punktu (0,0) do (19,19)
- Zapisuje wyniki do plików `output.txt` i `path.txt`

### 2. Wizualizacja Unity

1. Otwórz projekt Unity w folderze `Algorytm_wizualizacja_unity_razem`
2. Otwórz scenę główną
3. Uruchom projekt (Play button)

System automatycznie:
- Wygeneruje nową losową mapę
- Uruchomi algorytm A*
- Zwizualizuje wyniki
- Uruchomi animację postaci

## Format plików

### grid.txt
```
0 0 0 5 0 0 0
0 5 0 0 0 5 0
0 0 0 5 0 0 0
```
- `0` = wolne pole
- `5` = przeszkoda

### path.txt
```
0,0
1,0
1,1
2,1
```
Format: `x,y` - współrzędne kolejnych punktów ścieżki

## Konfiguracja

### Parametry algorytmu (Program.cs)
```csharp
// Zmiana punktu startowego i końcowego
var path = algorithm.FindPath(new Node(0, 0), new Node(19, 19));
```

### Parametry wizualizacji (CharacterMovement.cs)
```csharp
public float moveSpeed = 3f;        // Prędkość ruchu postaci
public float rotationSpeed = 10f;   // Prędkość rotacji
```