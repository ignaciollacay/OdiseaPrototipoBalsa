using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Item Database", menuName = "Inventory/Items/Database")]
public class ItemDatabaseSO : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemSO[] Items;

    [ContextMenu("Update ID's")]
    private void UpdateIDs()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i].data.Id != i)
                Items[i].data.Id = i;
        }
    }

    public void OnAfterDeserialize()
    {
        UpdateIDs();
    }

    public void OnBeforeSerialize()
    {

    }
}
