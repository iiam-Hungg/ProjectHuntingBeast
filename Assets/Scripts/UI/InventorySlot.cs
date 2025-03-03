using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Weaponinfo weaponInfo;
    public Weaponinfo GetWeaponinfo()
    {
        return weaponInfo;
    }
}
