using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    [SerializeField] private Weaponinfo weaponinfo;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnPoint;
    [SerializeField] private AudioClip arrowSound;

    readonly int FIRE_HASH = Animator.StringToHash("Fire");

    private Animator myAnimator;
    private AudioSource audioSource;

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void Attack()
    {
        myAnimator.SetTrigger(FIRE_HASH);

        if (arrowSound != null)
        {
            audioSource.PlayOneShot(arrowSound);
        }

        GameObject newArrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, ActiveWeapon.Instance.transform.rotation);
        newArrow.GetComponent<ProjectTile>().UpdateProjectileRange(weaponinfo.weaponRange);
    }

    public Weaponinfo GetWeaponinfo()
    {
        return weaponinfo;
    }
}
