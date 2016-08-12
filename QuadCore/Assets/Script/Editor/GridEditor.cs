using UnityEngine;
using UnityEditor;

using System.Collections;
using System.IO;

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor 
{
    Grid grid;

    private int oldIndex = 0;

    private Vector3 mouseBeginPos;
    private Vector3 mouseEndPos;

    void OnEnable()
    {
        grid = (Grid)target;
    }

    [MenuItem("Assets/TileSet")]
    static void CreateTileSet()
    {
        var asset = ScriptableObject.CreateInstance<TileSet>();
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);

        if(string.IsNullOrEmpty(path))
        {
            path = "Assets";
        }
        else if(Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(path),"");
        }
        else
        {
            path += "/";
        }

        var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "TileSet.asset");
        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
        asset.hideFlags = HideFlags.DontSave;
    }

    public override void OnInspectorGUI()
    {
        grid.width = createSlider("Width", grid.width);
        grid.height = createSlider("Height", grid.height);

        if(GUILayout.Button("Open Grid Window"))
        {
            GridWindow window = (GridWindow)EditorWindow.GetWindow(typeof(GridWindow));
            window.Init();
        }

        //Tile Prefab
        EditorGUI.BeginChangeCheck();
        var newTilePrefab = (Transform)EditorGUILayout.ObjectField("Tile Prefab", grid.tilePrefab, typeof(Transform), false);
        if(EditorGUI.EndChangeCheck())
        {
            grid.tilePrefab = newTilePrefab;
            Undo.RecordObject(target, "Grid Changed");        
        }

        //Tile Map
        EditorGUI.BeginChangeCheck();
        var newTileSet = (TileSet)EditorGUILayout.ObjectField("TileSet", grid.tileSet, typeof(TileSet), false);
        if(EditorGUI.EndChangeCheck())
        {
            grid.tileSet = newTileSet;
            Undo.RecordObject(target, "Grid Changed");
        }

        // TileSet을 쉽게 이용 하기 위한 장치
        if(grid.tileSet != null)
        {
            EditorGUI.BeginChangeCheck();
            var names = new string[grid.tileSet.prefabs.Length];
            var values = new int[names.Length];

            for(int i = 0; i < names.Length; i++)
            {
                names[i] = grid.tileSet.prefabs[i] != null ? grid.tileSet.prefabs[i].name : "";
                values[i] = i;
            }

            var index = EditorGUILayout.IntPopup("Select Tile", oldIndex, names, values);

            if(EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Grid Changed");
                if(oldIndex != index)
                {
                    oldIndex = index;
                    grid.tilePrefab = grid.tileSet.prefabs[index];

                    float width = grid.tilePrefab.GetComponent<Renderer>().bounds.size.x;
                    float height = grid.tilePrefab.GetComponent<Renderer>().bounds.size.y;

                    grid.width = width;
                    grid.height = height;
                }
            }
        }

        EditorGUI.BeginChangeCheck();

        bool draggable = EditorGUILayout.Toggle("Toggle Dragging: ", grid.draggable);
        if(EditorGUI.EndChangeCheck())
        {
            grid.draggable = draggable;
        }
    }

    private float createSlider(string labelName,float sliderPosition)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Grid " + labelName);
        sliderPosition = EditorGUILayout.Slider(sliderPosition, 0.1f, 100f, null);
        GUILayout.EndHorizontal();

        return sliderPosition;
    }

    void OnSceneGUI()
    {
        int controlId = GUIUtility.GetControlID(FocusType.Passive);
        Event e = Event.current;
        Ray ray = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
        Vector3 mousePos = ray.origin;

        //create tile
        if(e.isMouse && e.type == EventType.MouseDown && e.button == 0)
        {
            GUIUtility.hotControl = controlId;
            e.Use();

            if(grid.draggable)
            {
                mouseBeginPos = mousePos;
            }
            else
            {
                GameObject gameObject;
                Transform prefab = grid.tilePrefab;

                if(prefab)
                {
                    Undo.IncrementCurrentGroup();
                    Vector3 aligned = new Vector3(Mathf.Floor(mousePos.x / grid.width) * grid.width + grid.width / 2.0f, Mathf.Floor(mousePos.y / grid.height) * grid.height + grid.height / 2.0f, 0.0f);

                    if (GetTransformFromPosition(aligned) != null) return;

                    gameObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab.gameObject);
                    gameObject.transform.position = aligned;
                    gameObject.transform.parent = grid.transform;

                    Undo.RegisterCreatedObjectUndo(gameObject, " Create " + gameObject.name);
                }
            }
        }

        if ((e.isMouse & e.type == EventType.MouseDown && (e.button == 0 || e.button == 1))) // 눌렀을 때 이벤트
        {
            GUIUtility.hotControl = controlId;
            e.Use();
            Vector3 aligned = new Vector3(Mathf.Floor(mousePos.x / grid.width) * grid.width + grid.width / 2.0f, Mathf.Floor(mousePos.y / grid.height) * grid.height + grid.height / 2.0f, 0.0f);
            Transform transform = GetTransformFromPosition(aligned);

            if (transform != null) // 삭제
            {
                DestroyImmediate(transform.gameObject);
            }
        }

        if ((e.isMouse && e.type == EventType.MouseUp && (e.button == 0 || e.button == 1)))  // 손 땠을 때 이벤트
        {
            if (grid.draggable && e.button == 0)
            {
                mouseEndPos = mousePos;
                FillArea(mouseBeginPos, mouseEndPos);

                mouseEndPos = Vector3.zero;
                mouseBeginPos = Vector3.zero;
            }

            GUIUtility.hotControl = 0;
        }
    }

    Transform GetTransformFromPosition(Vector3 aligned)
    {

        int i = 0;
        while (i < grid.transform.childCount)
        {
            Transform transform = grid.transform.GetChild(i);
            if (transform.position == aligned)
            {
                return transform;
            }

            i++;
        }
        return null;
    }

    void FillArea(Vector3 _StartPosition, Vector3 _EndPosition) // 드래그 모드 시 타일 채우는 기능
    {

        Transform prefab = grid.tilePrefab;
        if (prefab == null)
        {
            Debug.LogError("No prefab attached to grid.");
        }

        Vector2 numberOfTilesToStartPosition = new Vector2();
        Vector2 numberOfTilesToEndPosition = new Vector2();

        Vector2 tilesToFill = new Vector2();
        _StartPosition.x = Mathf.Floor(_StartPosition.x / grid.width) * grid.width;
        _StartPosition.y = Mathf.Floor(_StartPosition.y / grid.height) * grid.height;

        _EndPosition.x = Mathf.Floor(_EndPosition.x / grid.width) * grid.width;
        _EndPosition.y = Mathf.Floor(_EndPosition.y / grid.height) * grid.height;

        Vector2 numberOfTilesToFill = new Vector2();

        numberOfTilesToFill.x = Mathf.Abs(_StartPosition.x - _EndPosition.x);
        numberOfTilesToFill.y = Mathf.Abs(_StartPosition.y - _EndPosition.y);

        // swap to fill from left to right
        if (_StartPosition.x > _EndPosition.x)
        {
            Vector3 tmp = new Vector3();
            tmp = _EndPosition;
            _StartPosition.x = _EndPosition.x;
            _EndPosition = tmp;
        }
        // swap to fill from up to down
        if (_StartPosition.y > _EndPosition.y)
        {
            Vector3 tmp = new Vector3();
            tmp = _EndPosition;
            _StartPosition.y = _EndPosition.y;
            _EndPosition = tmp;
        }
       


        numberOfTilesToFill.x = numberOfTilesToFill.x / grid.width + 1.0f;
        numberOfTilesToFill.y = numberOfTilesToFill.y / grid.height + 1.0f;
        
        int currentXTileNumber = 0;
        int currentYTileNumber = 0;


        do
        {
            currentYTileNumber = 0;
            do
            {

                Vector3 realWorldPosition = new Vector3();
                GameObject gameObject;

                realWorldPosition.x = _StartPosition.x + (currentXTileNumber * grid.width) + grid.width / 2.0f;
                realWorldPosition.y = _StartPosition.y + (currentYTileNumber * grid.height) + grid.height / 2.0f;              
               
                realWorldPosition.z = 0.0f;

                gameObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab.gameObject);
                gameObject.transform.position = realWorldPosition;
                gameObject.transform.parent = grid.transform;


                ++currentYTileNumber;
            } while (currentYTileNumber < numberOfTilesToFill.y);

            ++currentXTileNumber;
        } while (currentXTileNumber < numberOfTilesToFill.x);
    }
}

//      맵 제작을 쉽게 하기 위해 맵 에디터를 인터넷에 찾아보면서 만들어 봤습니다.
//      사용법은 Grid 스크립트를 Map이라는 빈오브젝트에 넣어 TileSet을 넣으신 후 프리팹을 골라서 마우스로 눌러서 
//      맵을 만들 수 있습니다 참고로 TileSet은 TileSet 폴더에서 오른쪽 클릭해서 TileSet을 눌러 만들 수 있습니다 
//      프리팹으로 세트를 채워 넣으시면 한개에 사용할 수 있는 세트를 만들 수 있습니다. 잘 모르시거나 맵 에디터에 추가 사항은 메시지로 문의 해주세요 by sounghoo
//      ps. 그리드 칸의 크기는 최상위 부모의 이미지의 크기에 맟춰집니다.
//      pss. 간단하게 Map 이라는 프리팹 쓰세요
//      psss. 특정칸에 드래그가 이상하게 되는 버그가 있습니다 이유를 찾아내시는 분은 감사합니다.

//      개선 해야할 사항 1. 만들기 전에 색깔 칸으로 표시로 이용 편하게
//      개선 해야할 사항 2. 드래그 모드에선 타일 겹치기가 가능한 문제
//      개선 해야할 사항 3. 위에서 언급한 특정칸에서 드래그 모드 버그