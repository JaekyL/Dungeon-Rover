using System;
using System.Collections;
using System.Collections.Generic;
using Components;
using Config;
using Helper;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;


public class TileDataEditor : OdinMenuEditorWindow
{
    [MenuItem("Configs/Tile Data")]
    private static void OpenWindow()
    {
        GetWindow<TileDataEditor>().Show();
    }

    private CreateNewTileData _createNewTileData;

    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        if(_createNewTileData != null) DestroyImmediate(_createNewTileData.config);
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();

        _createNewTileData = new CreateNewTileData();
        
        tree.Add("Create New", new CreateNewTileData());
        tree.AddAllAssetsAtPath("Tile Data", "Assets/Configs/Tiles", typeof(TileConfig));
        
        return tree;
    }


    protected override void OnBeginDrawEditors()
    {
        OdinMenuTreeSelection selected = this.MenuTree.Selection;

        SirenixEditorGUI.BeginHorizontalToolbar();
        {
            GUILayout.FlexibleSpace();

            if (SirenixEditorGUI.ToolbarButton("Delete Current"))
            {
                TileConfig config = selected.SelectedValue as TileConfig;
                string path = AssetDatabase.GetAssetPath(config);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }

    private class CreateNewTileData
    {
        public CreateNewTileData()
        {
            config = ScriptableObject.CreateInstance<TileConfig>();
        }
        
        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public TileConfig config;

        [Button("Add New Enemy SO")]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(config, "Assets/Configs/Tiles/" + Enum.GetName(typeof(TileType),config.ToBlobAssetReference().Value.Type) + ".asset");
            AssetDatabase.SaveAssets();
            
            config = ScriptableObject.CreateInstance<TileConfig>();
        }
        
        
    }
}
