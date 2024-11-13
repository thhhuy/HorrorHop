using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField] private TMP_Text BulletText;
    [SerializeField] private Transform FirePoint;
    [SerializeField] private GameObject Fire;
    [SerializeField] private GameObject HitPoint;
    [SerializeField] private Transform FireEffect;
    [SerializeField] private int Damage;
    [SerializeField] private int BulletLeft, MagSize;
    [SerializeField] private float CoolDown;
    [SerializeField] private bool CanShoot;

    public AudioClip gunshotSound; // Âm thanh bắn súng
    public static event Action<Vector3> OnGunShot; // Sự kiện phát ra khi nổ súng

    private AudioSource audioSource;

    private void Awake()
    {
        // Tìm các thành phần bằng tag
        GameObject bulletTextObject = GameObject.FindWithTag("BulletText");
        if (bulletTextObject != null)
        {
            BulletText = bulletTextObject.GetComponent<TMP_Text>();
        }
        else
        {
            Debug.LogError("BulletText với tag 'BulletTextTag' không được tìm thấy.");
        }

        GameObject firePointObject = GameObject.FindWithTag("FirePoint");
        if (firePointObject != null)
        {
            FirePoint = firePointObject.transform;
        }
        else
        {
            Debug.LogError("FirePoint với tag 'FirePointTag' không được tìm thấy.");
        }

        UpdateBulletText(); // Cập nhật số lượng đạn ban đầu lên UI
        CanShoot = true;

        audioSource = GetComponent<AudioSource>(); // Lấy AudioSource từ cùng GameObject
    }

    void Update()
    {
        // Nhấn phím E để xóa đối tượng "bullet" khi raycast vào
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(FirePoint.position, transform.TransformDirection(Vector3.forward), out hit, 100))
            {
                if (hit.collider.CompareTag("ShotgunBullet"))
                {
                    BulletLeft++;
                    UpdateBulletText(); // Cập nhật số lượng đạn sau khi nhặt đạn
                    Destroy(hit.collider.gameObject);
                    Debug.Log("Bullet removed.");
                }
            }
        }

        // Nhấn chuột trái để bắn
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (CanShoot == false || BulletLeft <= 0)
                return;
            StartCoroutine(CoolDownTime());
        }
    }

    IEnumerator CoolDownTime()
    {
        Shooting();
        CanShoot = false;
        yield return new WaitForSeconds(CoolDown);
        CanShoot = true;
    }

    void Shooting()
    {
        RaycastHit hit;
        BulletLeft--;
        UpdateBulletText(); // Cập nhật số lượng đạn sau khi bắn
        BulletText.text = BulletLeft + "/" + MagSize.ToString();

        // Phát âm thanh bắn súng
        if (audioSource != null && gunshotSound != null)
        {
            audioSource.PlayOneShot(gunshotSound);
        }

        // Phát sự kiện nổ súng với vị trí FirePoint
        if (OnGunShot != null)
        {
            OnGunShot.Invoke(FirePoint.position);
        }

        if (Physics.Raycast(FirePoint.position, transform.TransformDirection(Vector3.forward), out hit, 100))
        {
            Debug.DrawRay(FirePoint.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("We shoot");

            // Tạo hiệu ứng bắn
            GameObject a = Instantiate(Fire, FireEffect.position, Quaternion.identity);

            // Add AudioSource to fire effect clone if not present
            AudioSource cloneAudioSource = a.GetComponent<AudioSource>();
            if (cloneAudioSource == null)
            {
                cloneAudioSource = a.AddComponent<AudioSource>();
            }

            // Assign the gunshot sound to the clone's AudioSource and play it
            cloneAudioSource.clip = gunshotSound;
            cloneAudioSource.Play();

            GameObject b = Instantiate(HitPoint, hit.point, Quaternion.identity);
            Destroy(a, 2f);
            Destroy(b, 1f);

            // Xử lý sát thương cho Enemy nếu trúng mục tiêu là Enemy
            EnemyManager enemyManager = hit.transform.GetComponent<EnemyManager>();
            if (enemyManager != null)
            {
                enemyManager.TakeDamage(Damage);
            }
        }
    }

    void UpdateBulletText()
    {
        BulletText.text = BulletLeft + "/" + MagSize.ToString();
    }
}
