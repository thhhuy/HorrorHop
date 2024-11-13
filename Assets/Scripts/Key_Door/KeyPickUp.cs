using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickUp : MonoBehaviour
{
    public GameObject keyimage;
    public GameObject keyistrue;
    public bool isPlayer;
    public GameObject hand;

    // Biến tĩnh để theo dõi trạng thái của chìa khóa
    public static bool keyPickedUp = false;

    // Start is called before the first frame update
    void Start()
    {
        isPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayer)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                keyimage.SetActive(true);
                keyistrue.SetActive(true);
                hand.SetActive(false);

                // Đánh dấu chìa khóa đã được nhặt
                keyPickedUp = true;

                // Ẩn đối tượng thay vì phá hủy
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isPlayer = true;
            hand.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isPlayer = false;
            hand.SetActive(false);
        }
    }
}
