using System;
using System.Linq;
using MyBox;
using StatSheet.Model.Characters;
using StatSheet.Model.Tables;
using StatSheet.View.CharacterSelection;
using UnityEngine;
using UnityUtils.Attributes;

namespace StatSheet.Control.Tables.Managers
{
    public class TableDataManager : MonoBehaviour
    {
        public static event Action OnActiveCharChange;
        
        [SerializeField] private CharacterArrayVariable charData;
        [SerializeField] private CharacterArrayVariable defaultCharData;
        [NamedArray(typeof(TableType))]
        [SerializeField] private Table[] tables;

        private int _currentChar;
        public CharacterData CurrentCharacterData => charData[_currentChar];
        private CharacterData DefaultCharData => defaultCharData[0].Clone();
        public ulong CurrentCharUid => CurrentCharacterData.uid;

        private ulong _nextCharUid;

        private void OnValidate()
        {
            CheckTableCount();
            CheckData();
        }

        private void CheckTableCount()
        {
            tables = GetComponentsInChildren<Table>();
            if (tables.Length > Enum.GetValues(typeof(TableType)).Length)
            {
                Debug.LogWarning($"There are more tables than there are table types");
            }
        }

        [ButtonMethod]
        private void CheckData()
        {
            if (charData.Value == null || charData.Value.Length < 1)
            {
                charData.Value = new[]{DefaultCharData};
            }
            for (int i = 0; i < charData.Value.Length; i++)
            {
                var nullTable = charData.Value[i].tables == null; 
                if (nullTable || charData.Value[i].tables.Length < tables.Length)
                {
                    var newTableData = new TableData[tables.Length];
                    if(!nullTable) charData.Value[i].tables.CopyTo(newTableData, 0);
                    charData.Value[i].tables = newTableData;
                }
            }
        }

        private void Awake()
        {
            CharacterSelectionView.OnNewCharRequest += OnNewCharRequest;
            CharacterSelectionView.OnOpenCharRequest += OnOpenCharRequest;
            CharacterSelectionView.OnRemoveCharRequest += OnRemoveCharRequest;
        }

        private void OnDestroy()
        {
            CharacterSelectionView.OnNewCharRequest -= OnNewCharRequest;
            CharacterSelectionView.OnOpenCharRequest -= OnOpenCharRequest;
            CharacterSelectionView.OnRemoveCharRequest -= OnRemoveCharRequest;
        }

        private void Start()
        {
            SafelyFillTables();
        }

        private void SafelyFillTables()
        {
            CheckData();
            InitNextCharUid();
            InitTables();
        }

        private void InitNextCharUid() 
            => _nextCharUid = charData.Value.Max(character => character.uid) + 1;

        private void InitTables()
        {
            var tablesData = CurrentCharacterData.tables;
            for (int i = 0; i < tables.Length; i++)
            {
                tables[i].Manager = this;
                tables[i].FillTable(tablesData[i]);
            }
            OnActiveCharChange?.Invoke();
        }

        public void SaveTableData(TableData tableData)
        {
            charData.Value[_currentChar].tables[(int) tableData.tableType] = tableData;
            charData.ForceSave();
        }

        public string GetImagePath(uint id)
        {
            var paths = CurrentCharacterData.imagePaths; 
            if (paths == null || !(id < paths.Length))
            {
                return "";
            }
            return paths[id];
        }

        public void SetImagePath(uint id, string newImagePath)
        {
            var paths = CurrentCharacterData.imagePaths;
            if (paths == null) paths = Array.Empty<string>();
            
            if (!(id < paths.Length))
            {
                var newPaths = new string[id + 1];
                Array.Copy(paths, newPaths, paths.Length);
                paths = newPaths;
            }

            paths[id] = newImagePath;
            charData.Value[_currentChar].imagePaths = paths;
            charData.ForceSave();
        }

        public void OnCharNameUpdate(string newCharName)
        {
            charData.Value[_currentChar].name = newCharName;
            charData.ForceSave();
        }

        private void OnNewCharRequest()
        {
            var newChar = DefaultCharData;
            newChar.uid = _nextCharUid++; 
            charData.Value = charData.Value.Append(newChar).ToArray();
            CheckData();
        }

        private void OnOpenCharRequest(int charIndex)
        {
            _currentChar = charIndex;
            InitTables();
        }

        private void OnRemoveCharRequest(int charToRemove)
        {
            var removingCurrentChar = charToRemove == _currentChar;
            var newCharIndex = 0;
            if (!removingCurrentChar)
            {
                newCharIndex = charToRemove > _currentChar ? _currentChar : _currentChar - 1;
            }
            _currentChar = newCharIndex;
            
            var data = charData.Value.ToList();
            data.RemoveAt(charToRemove);
            if (data.Count == 0)
            {
                data.Add(DefaultCharData);
            }
            charData.Value = data.ToArray();
            
            if (removingCurrentChar)
            {
                OnOpenCharRequest(_currentChar);
            }
        }
    }
}