using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PuzzleManager : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public Texture2D sourceImage;        // drag your image (Texture2D, Read/Write enabled)
    public GameObject piecePrefab;       // drag the UI Image prefab (with PuzzlePiece script)
    public RectTransform puzzleRoot;     // drag an UI empty RectTransform (the grid area)

    [Header("Grid")]
    public int rows = 3;
    public int cols = 3;

    [Header("Behavior")]
    public float snapThreshold = 80f;    // distance in UI units for swap
    // lists
    [HideInInspector] public List<RectTransform> slots = new List<RectTransform>();
    [HideInInspector] public List<PuzzlePiece> pieces = new List<PuzzlePiece>();

    void Start()
    {
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

        int texW = sourceImage.width;
        int texH = sourceImage.height;
        int cellW = texW / cols;
        int cellH = texH / rows;

        // Create a shuffled list of indices so pieces appear in random order in slots
        List<int> indices = new List<int>();
        for (int i = 0; i < rows * cols; i++) indices.Add(i);
        Shuffle(indices);

        for (int i = 0; i < rows * cols; i++)
        {
            int r = i / cols;
            int c = i % cols;

            int randomIndex = indices[i]; // which slice will be placed into slot i

            int sx = (randomIndex % cols) * cellW;
            int sy = (rows - 1 - (randomIndex / cols)) * cellH; // invert y

            Rect rect = new Rect(sx, sy, cellW, cellH);
            Sprite sp = Sprite.Create(sourceImage, rect, new Vector2(0.5f, 0.5f), 100f);

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

            pieces.Add(pp);

            // Place piece into the slot position (but using shuffled order)
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
}
