using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager instance;
    [Header("Assign in Inspector")]
    public Texture2D[] sourceImage;
    public GameObject piecePrefab;
        public RectTransform puzzleRoot;

    [Header("Grid")]
    private int rows;
    private int cols;

    [Header("Behavior")]
    public float snapThreshold = 80f;
    
    [HideInInspector] public List<RectTransform> slots = new List<RectTransform>();
    [HideInInspector] public List<PuzzlePiece> pieces = new List<PuzzlePiece>();

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        int range = PlayerPrefs.GetInt("CurrentLevelNumber");
        if(0 < range && range <= 5)
        {
            rows = Random.Range(3,4);
            cols = Random.Range(3,4);
        }
        else if(5 < range && range <= 15)
        {
            rows = Random.Range(5,7);
            cols = Random.Range(3,4);
        }
        else if(15 < range && range <= 20)
        {
            rows = Random.Range(6,7);
            cols = Random.Range(4,4);
        }
        else
        {
            Debug.Log("Max Level Reached");
        }


        if (sourceImage == null || piecePrefab == null || puzzleRoot == null)
        {
            Debug.LogError("PuzzleManager: Assign sourceImage, piecePrefab and puzzleRoot in Inspector.");
            return;
        }

        CreateSlots();
        CreatePieces();
    }

    void CreateSlots()
    {
        // clear old
        foreach (Transform t in puzzleRoot) Destroy(t.gameObject);
        slots.Clear();

        float pieceWidth = puzzleRoot.rect.width / cols;
        float pieceHeight = puzzleRoot.rect.height / rows;

        Vector2 startPos = new Vector2(
            -puzzleRoot.rect.width / 2 + pieceWidth / 2,
             puzzleRoot.rect.height / 2 - pieceHeight / 2
        );

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                GameObject slotGO = new GameObject("Slot_" + (r * cols + c), typeof(RectTransform));
                slotGO.transform.SetParent(puzzleRoot, false);
                RectTransform rt = slotGO.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(pieceWidth, pieceHeight);
                float x = startPos.x + c * pieceWidth;
                float y = startPos.y - r * pieceHeight;
                rt.anchoredPosition = new Vector2(x, y);
                slots.Add(rt);
            }
        }
    }

    void CreatePieces()
    {
        pieces.Clear();

        int texW = sourceImage[PlayerPrefs.GetInt("currentImageNumber")].width;
        int texH = sourceImage[PlayerPrefs.GetInt("currentImageNumber")].height;
        int cellW = texW / cols;
        int cellH = texH / rows;

        
        List<int> indices = new List<int>();
        for (int i = 0; i < rows * cols; i++) indices.Add(i);
        Shuffle(indices);

        for (int i = 0; i < rows * cols; i++)
        {
            int r = i / cols;
            int c = i % cols;

            int randomIndex = indices[i];

            int sx = (randomIndex % cols) * cellW;
            int sy = (rows - 1 - (randomIndex / cols)) * cellH;

            Rect rect = new Rect(sx, sy, cellW, cellH);
            Sprite sp = Sprite.Create(sourceImage[PlayerPrefs.GetInt("currentImageNumber")], rect, new Vector2(0.5f, 0.5f), 100f);

            GameObject go = Instantiate(piecePrefab, puzzleRoot);
            go.name = "Piece_" + randomIndex;

            Image img = go.GetComponent<Image>();
            if (img == null) img = go.AddComponent<Image>();
            img.sprite = sp;
            img.preserveAspect = false;

            RectTransform rt = go.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(puzzleRoot.rect.width / cols, puzzleRoot.rect.height / rows);

            PuzzlePiece pp = go.GetComponent<PuzzlePiece>();
            if (pp == null) pp = go.AddComponent<PuzzlePiece>();
            pp.manager = this;
            pp.slotIndex = randomIndex;
            pp.currentSlot = i;

            pieces.Add(pp);

            rt.anchoredPosition = slots[i].anchoredPosition;
        }
    }

    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int r = Random.Range(i, list.Count);
            int tmp = list[i];
            list[i] = list[r];
            list[r] = tmp;
        }
    }

public void CheckPuzzleComplete()
{
    foreach (var piece in pieces)
    {
        if (piece.currentSlot != piece.slotIndex)
            return;
    }

    if (GamePlay.instance == null)
    {
        return;
    }
    GamePlay.instance.YouWin();
    PuzzlePiece.instance.isLocked = true;
}

public Vector2 GetSlotPosition(int index)
{
    if (index < 0 || index >= slots.Count)
    {
        return Vector2.zero;
    }
    return slots[index].anchoredPosition;
}


}