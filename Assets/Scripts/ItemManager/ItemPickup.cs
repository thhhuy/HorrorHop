using System.Collections.Generic;
using UnityEngine;

public class PickupThrow : MonoBehaviour
{
    private GameObject heldItem;            // Vật phẩm đang được nhặt
    private Rigidbody heldItemRb;           // Rigidbody của vật phẩm đang được nhặt
    private bool originalCCDState;          // Trạng thái ban đầu của Collision Detection Mode (CCD)

    public float throwForce = 10f;          // Lực ném
    public KeyCode pickUpKey = KeyCode.E;   // Phím để nhặt vật phẩm
    public KeyCode throwKey = KeyCode.F;    // Phím để ném vật phẩm
    public Transform handPosition;          // Vị trí để giữ vật phẩm (HandPlayer)
    public List<TagPrefabMapping> tagPrefabMappings;  // Danh sách các tag và prefab tương ứng

    [System.Serializable]
    public struct TagPrefabMapping
    {
        public string tag;       // Tag của vật phẩm
        public GameObject prefab;  // Prefab tương ứng
    }

    private void OnTriggerEnter(Collider other)
    { 
        if (heldItem == null && IsTagInList(other.gameObject.tag))
        {
            Debug.Log("Nhấn " + pickUpKey.ToString() + " để nhặt vật phẩm.");
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(pickUpKey) && heldItem == null && IsTagInList(other.gameObject.tag))
        {
            PickUpItem(other.gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(throwKey) && heldItem != null)
        {
            ThrowItem();
        }
    }

    private void PickUpItem(GameObject item)
    {
        heldItemRb = item.GetComponent<Rigidbody>();
        if (heldItemRb == null)
        {
            Debug.LogWarning("Không tìm thấy Rigidbody gắn với vật phẩm: " + item.name);
            return;
        }

        originalCCDState = heldItemRb.collisionDetectionMode == CollisionDetectionMode.Continuous;

        heldItemRb.collisionDetectionMode = CollisionDetectionMode.Discrete;

        heldItem = item;

        heldItemRb.isKinematic = true;

        heldItem.transform.position = handPosition.position;
        heldItem.transform.rotation = handPosition.rotation;
        heldItem.transform.parent = handPosition;

        GameObject prefab;
        if (TryGetPrefabForTag(heldItem.tag, out prefab))
        {
            if (prefab != null)
            {
                GameObject newObject = Instantiate(prefab, handPosition.position, handPosition.rotation);
                newObject.transform.parent = handPosition;
                Destroy(heldItem);
                heldItem = newObject;
                heldItemRb = heldItem.GetComponent<Rigidbody>();
                heldItemRb.isKinematic = true;

                Debug.Log("Đã nhặt vật phẩm: " + heldItem.name);
            }
            else
            {
                Debug.LogError("Prefab không tồn tại cho tag: " + heldItem.tag);
            }
        }
        else
        {
            Debug.LogError("Không tìm thấy prefab cho tag: " + heldItem.tag);
        }
    }


    private void ThrowItem()
    {
        heldItem.transform.parent = null;
        heldItemRb.isKinematic = false;

        heldItemRb.collisionDetectionMode = originalCCDState ? CollisionDetectionMode.Continuous : CollisionDetectionMode.Discrete;

        heldItemRb.AddForce(transform.forward * throwForce, ForceMode.Impulse);

        Debug.Log("Đã ném vật phẩm: " + heldItem.name);

        heldItem = null;
        heldItemRb = null;
    }

    private bool IsTagInList(string tag)
    {
        foreach (var mapping in tagPrefabMappings)
        {
            if (mapping.tag == tag)
                return true;
        }
        return false;
    }

    private bool TryGetPrefabForTag(string tag, out GameObject prefab)
    {
        foreach (var mapping in tagPrefabMappings)
        {
            if (mapping.tag == tag)
            {
                prefab = mapping.prefab;
                return true;
            }
        }
        prefab = null;
        return false;
    }

    public bool IsHoldingItemWithTag(string tag)
    {
        return heldItem != null && heldItem.CompareTag(tag);
    }

}
