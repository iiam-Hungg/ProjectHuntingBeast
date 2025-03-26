using System.Collections;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon { get; private set; }

    private PlayerControls playerControls;
    private float timeBetweenAttacks;
    private bool attackButtonDown, isAttacking = false;

    private AudioSource audioSource;
    private AudioClip attackSound;
    [SerializeField] private AudioClip swordSound;
    [SerializeField] private AudioClip bowSound;

    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();
        AttackCooldown();
    }

    private void Update()
    {
        Attack();
    }

    public void NewWeapon(MonoBehaviour newWeapon)
    {
        CurrentActiveWeapon = newWeapon;
        AttackCooldown();
        timeBetweenAttacks = (CurrentActiveWeapon as IWeapon).GetWeaponinfo().weaponCooldown;

        if (CurrentActiveWeapon is Sword)
        {
            attackSound = swordSound;
        }
        else if (CurrentActiveWeapon is Bow)
        {
            attackSound = bowSound;
        }

        Debug.Log("Assigned Sound: " + (attackSound != null ? attackSound.name : "NULL"));
    }

    public void WeaponNull()
    {
        CurrentActiveWeapon = null;
    }

    private void AttackCooldown()
    {
        isAttacking = true;
        StopAllCoroutines();
        StartCoroutine(TimeBetweenAttacksRoutine());
    }

    private IEnumerator TimeBetweenAttacksRoutine()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }

    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }

    private void Attack()
    {
        if (attackButtonDown && !isAttacking && CurrentActiveWeapon)
        {
            AttackCooldown();
            (CurrentActiveWeapon as IWeapon).Attack();

            if (audioSource != null && attackSound != null)
            {
                Debug.Log("Playing attack sound: " + attackSound.name);
                audioSource.PlayOneShot(attackSound);
            }
            else
            {
                Debug.LogWarning("AudioSource or Attack Sound is missing!");
            }
        }
    }
}