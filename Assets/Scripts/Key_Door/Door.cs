using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject hand;
    public bool isPlayer;
    Animator animator;
    public GameObject keyImage; // Thêm tham chiếu đến hình ảnh chìa khóa

    private void Start()
    {
        isPlayer = false;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Kiểm tra xem chìa khóa đã được nhặt chưa
        if (KeyPickUp.keyPickedUp)
        {
            if (isPlayer)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Ẩn hình ảnh tay và chìa khóa khi mở cửa
                    hand.SetActive(true);
                    if (keyImage != null)
                    {
                        keyImage.SetActive(false);
                    }
                    animator.enabled = true;
                }
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
