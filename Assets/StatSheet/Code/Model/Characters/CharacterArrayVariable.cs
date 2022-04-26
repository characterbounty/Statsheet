using UnityEngine;
using UnityUtils.Variables;

namespace StatSheet.Model.Characters
{
    [CreateAssetMenu(menuName = "Data/CharacterArrayVariable", fileName = "CharacterArrayVariable", order = 0)]
    public class CharacterArrayVariable : XArrayVariable<CharacterData>
    {
        public void ForceSave() => OnDataChanged();
    }
}