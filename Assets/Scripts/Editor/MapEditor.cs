using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;


[CustomEditor(typeof(MapManager))]
public class MapEditor : Editor
{
    private MapManager m_CurrentMapManager;

    private bool m_IsInSquareEditMode = false;

    private SquareState m_SquareStateEditMode = SquareState.Normal;

    private EdgeState m_EdgeStateEditMode = EdgeState.Hori;   

    private BrushState m_BrushStateEditMode = BrushState.One;   

    private bool m_IsInEdgeEditMode = false;
   
    private bool m_IsInBrushEditMode = false;

    private bool m_IsInEraseMode = false;
    public override void OnInspectorGUI()
    {
        GUILayout.Label("========== MAP EDITOR ==========");
        //Le bouton pour créer une map random
        if (GUILayout.Button("Initialize Map Randomly"))
        {

        m_CurrentMapManager.InitializeMapRandomly();
            
        }
        //celui pour une map vide
        if (GUILayout.Button("Initialize Map Empty"))
        {

        m_CurrentMapManager.InitializeEmptyMap();

        }
        //passer en mode EDIT
        m_IsInSquareEditMode = GUILayout.Toggle(m_IsInSquareEditMode, "Edit Square Mode");

        if (m_IsInSquareEditMode)
        {
            m_IsInEdgeEditMode = false;
            m_IsInEraseMode = false;
            m_IsInBrushEditMode = false;
            m_SquareStateEditMode = (SquareState)EditorGUILayout.EnumPopup(m_SquareStateEditMode);
            m_BrushStateEditMode = (BrushState)EditorGUILayout.EnumPopup(m_BrushStateEditMode);
        }

        //Passer en mode BRUSH

        //m_IsInBrushEditMode = GUILayout.Toggle(m_IsInBrushEditMode, "Brush Mode");


        if (m_IsInBrushEditMode)
        {
            m_IsInEdgeEditMode = false;
            m_IsInEraseMode = false;
            m_IsInSquareEditMode = false;
            //m_SquareStateEditMode = (SquareState)EditorGUILayout.EnumPopup(m_SquareStateEditMode);
            m_BrushStateEditMode = (BrushState)EditorGUILayout.EnumPopup(m_BrushStateEditMode);
        }

        //Passer en mode EDIT pour les edges
        m_IsInEdgeEditMode = GUILayout.Toggle(m_IsInEdgeEditMode, "Edit Edge Mode");

        if (m_IsInEdgeEditMode)
        {
            m_IsInBrushEditMode = false;
            m_IsInSquareEditMode = false;
            m_EdgeStateEditMode = (EdgeState)EditorGUILayout.EnumPopup(m_EdgeStateEditMode);
            //Passer en mode ERASE pour clear ce qu'on veut pas pour les EDGES
            m_IsInEraseMode = GUILayout.Toggle(m_IsInEraseMode, "Erase Mode");

        if (m_IsInEraseMode)
        {
            m_IsInSquareEditMode = false;
        }
        }


        GUILayout.Label("========== MAP EDITOR ==========");
        base.OnInspectorGUI();
    }
    public void OnEnable()
    {
        m_CurrentMapManager = (MapManager)target;

        LoadEditorState();
    }

    public void OnDisable()
    {
        SaveEditorState();
    }
    private void LoadEditorState()
    {
        m_IsInSquareEditMode = EditorPrefs.GetBool(nameof(m_IsInSquareEditMode));
        m_SquareStateEditMode = (SquareState)EditorPrefs.GetInt(nameof(m_SquareStateEditMode));
        //Je récupère les infos sauvgardés pour l'édition des Edges
        m_IsInEdgeEditMode = EditorPrefs.GetBool(nameof(m_IsInEdgeEditMode));
        m_EdgeStateEditMode = (EdgeState)EditorPrefs.GetInt(nameof(m_EdgeStateEditMode));
    }

    private void SaveEditorState()
    {
        EditorPrefs.SetBool(nameof(m_IsInSquareEditMode), m_IsInSquareEditMode);
        EditorPrefs.SetInt(nameof(m_SquareStateEditMode), (int)m_SquareStateEditMode);
        //Je sauve les infos pour l'édition des edges
        EditorPrefs.SetBool(nameof(m_IsInEdgeEditMode), m_IsInEdgeEditMode);
        EditorPrefs.SetInt(nameof(m_EdgeStateEditMode), (int)m_EdgeStateEditMode);


    }

    public void OnSceneGUI()
    {
        if(m_IsInSquareEditMode)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            Tools.current = Tool.None;

            Vector3 posIntersection = CalculateInteractPositionPlan();

            Vector3 posInt = new Vector3((int)posIntersection.x, 0, (int)posIntersection.z);

            if (posIntersection.x >= 0 && posIntersection.x < m_CurrentMapManager.mapData.width
               && posIntersection.z >= 0 && posIntersection.z < m_CurrentMapManager.mapData.height)
            {
                DisplayGizmoSquareEdit(posInt);
                EditSquareState(posInt);
            }
            SceneView.RepaintAll();
            m_CurrentMapManager.CreateMapView();
        }
        //Tout ça pour dire qu'on est dans l'EDIT mode des EDGES
        if (m_IsInEdgeEditMode)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            Tools.current = Tool.None;

            Vector3 posIntersection = CalculateInteractPositionPlan();

            Vector3 posInt = new Vector3((int)posIntersection.x, 0, (int)posIntersection.z);

            if (posIntersection.x >= 0 && posIntersection.x < m_CurrentMapManager.mapData.width
               && posIntersection.z >= 0 && posIntersection.z < m_CurrentMapManager.mapData.height
               && m_IsInSquareEditMode)
            {
                DisplayGizmoSquareEdit(posInt);
                EditSquareState(posInt);
            }
            if (posIntersection.x >= 0 && posIntersection.x < m_CurrentMapManager.mapData.width
                && posIntersection.z >= 0 && posIntersection.z < m_CurrentMapManager.mapData.height
                && m_IsInEdgeEditMode || m_IsInEraseMode || m_IsInBrushEditMode)
            {
                //On montre les EDGES horizontaux quand on est en mode HORI
                if(m_EdgeStateEditMode == EdgeState.Hori)
                {

                    DisplayGizmoEdgeHoriEdit(posInt);
                    EditEdgeState(posInt);

                }
                //La même pour le mode VERT
                if(m_EdgeStateEditMode == EdgeState.Vert)
                {

                    DisplayGizmoEdgeVertEdit(posInt);
                    EditEdgeState(posInt);

                }
                //EditSquareState(posInt);
            }
            SceneView.RepaintAll();
            m_CurrentMapManager.CreateMapView();
        }

    }

    private void EditSquareState(Vector3 posInt)
    {
        if (Event.current.button == 0)
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                case EventType.MouseDrag:    
                    int index = m_CurrentMapManager.GetIndexSquareFromPos(posInt);
                    m_CurrentMapManager.mapData.grid[index].state = m_SquareStateEditMode;
                    
                    break;



            }
        }

    }
    //On edit les data quand on clique ou drag clique
    //ça marche pas pour l'instant... 
    private void EditEdgeState(Vector3 posInt)
    {
        if (Event.current.button == 0)
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                case EventType.MouseDrag:
                    //int index = m_CurrentMapManager.GetIndexSquareFromPos(posInt);
                    //m_CurrentMapManager.mapData.grid[index] = m_EdgeStateEditMode;

                    break;



            }
        }

    }

    private void DisplayGizmoSquareEdit(Vector3 posInt)
    {
        Handles.color = m_CurrentMapManager.GetColorFromState(m_SquareStateEditMode);

        posInt.x += 0.5f;
        posInt.z += 0.5f;

        Vector3 sizeWireSquare = Vector3.one;
        sizeWireSquare.y = 0.2f;
        Handles.DrawWireCube(posInt, sizeWireSquare);

    }

    //Les GIZMOS des EDGES HORI
    private void DisplayGizmoEdgeHoriEdit(Vector3 posInt)
    {
        //Handles.color = m_CurrentMapManager.GetColorFromState(m_SquareStateEditMode);

        posInt.x += 0.5f;
        posInt.z += 1.0f;

        Vector3 sizeWireSquare = Vector3.one;
        sizeWireSquare.y = 0.5f;
        sizeWireSquare.z = 0.2f;
        Handles.DrawWireCube(posInt, sizeWireSquare);
    }

    //Les GIZMOS des EDGES VERT
    private void DisplayGizmoEdgeVertEdit(Vector3 posInt)
    {
        //Handles.color = m_CurrentMapManager.GetColorFromState(m_SquareStateEditMode);

        //posInt.x += 0.5f;
        posInt.z += 0.5f;

        Vector3 sizeWireSquare = Vector3.one;
        sizeWireSquare.y = 0.5f;
        sizeWireSquare.x = 0.2f;
        Handles.DrawWireCube(posInt, sizeWireSquare);
    }

    private Vector3 CalculateInteractPositionPlan()
    {
        Vector2 pos = Event.current.mousePosition;

        Ray ray = HandleUtility.GUIPointToWorldRay(pos);

        Plane plan = new Plane(Vector3.up, Vector3.zero);

        float distance;
        if(plan.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.zero;
    }
}
