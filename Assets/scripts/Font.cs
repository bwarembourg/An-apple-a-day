using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Font : MonoBehaviour
{

    [SerializeField] private Sprite[] fontSprites;
    [SerializeField] private Sprite[] numeroSprites;
    [SerializeField] private Sprite[] specialCharsSprites;
    [SerializeField] private Sprite[] mininumeroSprites;
    [SerializeField] private SpriteRenderer fontItemPrefab;

    public static Font current;

    private float spacing = 6f/16f;
    private float interline = 9f/16f;


    private void Awake()
    {
        current = this;
    }

    public List<SpriteRenderer> Write(string text, float x, float y, Transform parent, int sortingOrder = 2, bool visible = true,
        bool isKey = false, float maxX = 0f, bool centered = false, string layerName = "ui", bool isMin = false)
    {
        if (isKey)
        {
            //text = Translation.current.GetString(text);
        }
        List<SpriteRenderer> fontItems = new List<SpriteRenderer>();
        if (maxX != 0f)
        {
            float lineSize;
            if (maxX > x)
                lineSize = Mathf.Abs(maxX - x);
            else
                lineSize = Mathf.Abs(x - maxX);
            int nbCharsPerLines = Mathf.CeilToInt((float) lineSize / (float) spacing);
            int nbLines = Mathf.CeilToInt((float) text.Length / (float) nbCharsPerLines);
            if (nbLines > 1)
            {
                int cursor = 0;
                int lineNo = 0;
                while (cursor < text.Length)
                {
                    int newCursor = cursor;
                    if (cursor + nbCharsPerLines >= text.Length)
                    {
                        newCursor = text.Length;
                    }
                    else if (text[cursor + nbCharsPerLines] != ' ' || (text.Length - 1 > cursor + nbCharsPerLines + 1 && text[cursor + nbCharsPerLines + 1] != ' '))
                    {
                        newCursor += text.Substring(cursor, nbCharsPerLines).LastIndexOf(' ') + 1;
                    }
                    else
                    {
                        newCursor += nbCharsPerLines;
                    }
                    string line = text.Substring(cursor, newCursor - cursor);
                    fontItems.AddRange(WriteLine(line, x, y - interline * lineNo, parent, sortingOrder, visible,
                        centered, maxX, layerName, isMin));
                    cursor = newCursor;
                    lineNo++;
                }
            } else
            {
                fontItems = WriteLine(text, x, y, parent, sortingOrder, visible, centered, maxX, layerName, isMin);
            }
            return fontItems;
        }
        fontItems = WriteLine(text, x, y, parent, sortingOrder, visible, centered, maxX, layerName, isMin);
        return fontItems;
    }

    public List<SpriteRenderer> WriteLine(string text, float x, float y, Transform parent, int sortingOrder = 2,
        bool visible = true, bool centered = false, float maxX = 0, string layerName = "ui", bool isMin = false)
    {
        if (centered)
        {
            float center = x + ((maxX - x) / 2f);
            float spacingTotal = 0f;
            foreach(char c in text)
            {
                if (isMin)
                {
                    spacingTotal += 0.25f;
                }
                else if (IsSpacing4(c))
                    spacingTotal += 0.25f;
                else if (IsSpacing2(c))
                {
                    spacingTotal += 0.125f;
                }
                else if (IsSpacing3(c))
                {
                    spacingTotal += 3f / 16f;
                }
                else if (IsSpacing9(c))
                {
                    spacingTotal += 9f / 16f;
                }
                else
                {
                    spacingTotal += spacing;
                }
            }
            x = center - spacingTotal / 2f;
        }

        List<SpriteRenderer> fontItems = new List<SpriteRenderer>();
        float padding = 0f;
        foreach (char character in text)
        {
            SpriteRenderer fontItem = CreateFontItem(character, x + padding, y, parent, sortingOrder, layerName, visible, isMin);
            fontItems.Add(fontItem);
            if (isMin)
            {
                padding += 0.25f;
            } else if (IsSpacing4(character))
            {
                padding += 0.25f;
            } else if (IsSpacing2(character))
            {
                padding += 0.125f;
            } else if (IsSpacing3(character))
            {
                padding += 3f / 16f;
            } else if (IsSpacing9(character))
            {
                padding += 9f / 16f;
            }
            else
            {
                padding += spacing;
            }
        }
        return fontItems;
    }


    private SpriteRenderer CreateFontItem(char character, float x, float y, Transform parent, int sortingOrder, string layerName,
        bool visible, bool isMin)
    {
        SpriteRenderer fontItem = Instantiate(fontItemPrefab, new Vector3(parent.position.x + x, parent.position.y + y, 0), Quaternion.identity, parent);
        fontItem.gameObject.SetActive(visible);
        fontItem.sprite = GetFontSprite(character, isMin);
        fontItem.sortingLayerName = layerName;
        fontItem.sortingOrder = sortingOrder;
        return fontItem;
    }

    private Sprite GetFontSprite(char character, bool isMin)
    {
        int numero;
        bool isNum = int.TryParse(character.ToString(), out numero);
        if (isNum)
            return GetNumero(numero, isMin);

        switch (character)
        {
            case 'A': return fontSprites[0];
            case 'B': return fontSprites[1];
            case 'C': return fontSprites[2];
            case 'D': return fontSprites[3];
            case 'E': return fontSprites[4];
            case 'F': return fontSprites[5];
            case 'G': return fontSprites[6];
            case 'H': return fontSprites[7];
            case 'I': return fontSprites[8];
            case 'J': return fontSprites[9];
            case 'K': return fontSprites[10];
            case 'L': return fontSprites[11];
            case 'M': return fontSprites[12];
            case 'N': return fontSprites[13];
            case 'O': return fontSprites[14];
            case 'P': return fontSprites[15];
            case 'Q': return fontSprites[16];
            case 'R': return fontSprites[17];
            case 'S': return fontSprites[18];
            case 'T': return fontSprites[19];
            case 'U': return fontSprites[20];
            case 'V': return fontSprites[21];
            case 'W': return fontSprites[22];
            case 'X': return fontSprites[23];
            case 'Y': return fontSprites[24];
            case 'Z': return fontSprites[25];

            case 'a': return fontSprites[26];
            case 'b': return fontSprites[27];
            case 'c': return fontSprites[28];
            case 'd': return fontSprites[29];
            case 'e': return fontSprites[30];
            case 'f': return fontSprites[31];
            case 'g': return fontSprites[32];
            case 'h': return fontSprites[33];
            case 'i': return fontSprites[34];
            case 'j': return fontSprites[35];
            case 'k': return fontSprites[36];
            case 'l': return fontSprites[37];
            case 'm': return fontSprites[38];
            case 'n': return fontSprites[39];
            case 'o': return fontSprites[40];
            case 'p': return fontSprites[41];
            case 'q': return fontSprites[42];
            case 'r': return fontSprites[43];
            case 's': return fontSprites[44];
            case 't': return fontSprites[45];
            case 'u': return fontSprites[46];
            case 'v': return fontSprites[47];
            case 'w': return fontSprites[48];
            case 'x': return fontSprites[49];
            case 'y': return fontSprites[50];
            case 'z': return fontSprites[51];
            case '\'': return specialCharsSprites[0];
            case '!': return specialCharsSprites[1];
            case '?': return specialCharsSprites[2];
            case '.': return specialCharsSprites[3];
            case ':': return specialCharsSprites[4];
            case '/': return specialCharsSprites[5];
            case '+': return specialCharsSprites[6];
            case '-': return specialCharsSprites[7];
            case ',': return specialCharsSprites[8];
            case ';': return specialCharsSprites[9];
            case 'à': return specialCharsSprites[10];
            case 'â': return specialCharsSprites[11];
            case 'è': return specialCharsSprites[12];
            case 'ë': return specialCharsSprites[13];
            case 'ê': return specialCharsSprites[14];
            case 'é': return specialCharsSprites[15];
            case 'ï': return specialCharsSprites[16];
            case 'ç': return specialCharsSprites[17];
            case 'î': return specialCharsSprites[18];
            case 'ù': return specialCharsSprites[19];
            case '(': return specialCharsSprites[20];
            case ')': return specialCharsSprites[21];
            case 'ô': return specialCharsSprites[22];
            case 'û': return specialCharsSprites[23];
            case '§': return specialCharsSprites[24];
            default: return null;
        }
    }

    private Sprite GetNumero(int num, bool isMin)
    {
        return isMin ? mininumeroSprites[num] : numeroSprites[num];
    }

    private bool IsSpacing2(char character)
    {
        return character == 'i' || character == ',' || character == '.' || character == ':' ||
            character == ';' || character == '\'' || character == '!';
    }

    private bool IsSpacing3(char character)
    {
        return character == 'l';
    }

    private bool IsSpacing4(char character)
    {
        return character == 'I' || character == 't' || character == 'î' || character == 'ï';
    }

    private bool IsSpacing9(char character)
    {
        return character == '§';
    }
}
