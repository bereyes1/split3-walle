using UnityEngine;

public class TrashItem : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    public ItemData Data => itemData;
}
