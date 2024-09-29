using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : DroneSlot
{
    public Transform weaponHolder;
    public bool isPrimarySlot; 
    private GameObject currentWeaponInstance;
    private GameObject _player;
    private Weapon _currentWeapon;

    public override bool CanAcceptItem(Item item)
    {
        Debug.Log(item is Weapon);
        return item is Weapon;
    }

    public override void AddItem(Item newItem)
    {
        base.AddItem(newItem);
        Weapon newWeapon = newItem as Weapon;
        if (newWeapon != null)
        {
            _currentWeapon = newWeapon;
            weaponHolder = GameObject.FindGameObjectWithTag("WeaponHolder").transform;

            if (isPrimarySlot && newWeapon.weaponPrefab != null)
            {
                if (currentWeaponInstance != null)
                {
                    Destroy(currentWeaponInstance);
                    RemoveWeaponAttributes();

                }
          
                AddWeaponAttributes(newWeapon);
            }
            else if (!isPrimarySlot && newWeapon.weaponPrefab != null)
            {
                if (currentWeaponInstance != null)
                {
                    InstantiateWeapon();
                    RemoveWeaponAttributes();
                }
            }
        }
        
    }

    public override void ClearSlot()
    {
        ClearInstantiatedWeapon();
        base.ClearSlot();
    }
    public void ClearInstantiatedWeapon()
    {
        if (currentWeaponInstance != null)
        {
            Destroy(currentWeaponInstance);
            currentWeaponInstance = null;
            RemoveWeaponAttributes();

        }
    }
    public Weapon GetCurrentWeapon()
    {
        return _currentWeapon;
    }

    public void AddWeaponAttributes(Weapon newWeapon)
    {
    }

    public void RemoveWeaponAttributes()
    {
        if (_currentWeapon != null)
        {
        }
    }

    public void InstantiateWeapon()
    {
        if (currentWeaponInstance != null)
        {
            Destroy(currentWeaponInstance);

        }
        if (_currentWeapon != null && _currentWeapon.weaponPrefab != null)
        {
            currentWeaponInstance = Instantiate(_currentWeapon.weaponPrefab, weaponHolder.position, weaponHolder.rotation, weaponHolder);
            AddWeaponAttributes(_currentWeapon);
            currentWeaponInstance.transform.GetChild(0).GetComponent<Renderer>().material.color = Random.ColorHSV();
        }
    }
}
