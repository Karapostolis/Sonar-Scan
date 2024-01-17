using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

// Player class, representing a player character
public class Player : MonoBehaviour
{

    
    public static Player Instance;

    public string playerName;

    public int Level;
    public int ExperiencePoints;
    public int MaxHealthPoints;
    public int HealthPoints;
    public int ManaPoints;
    public int damage;
    public int armor ;
    public int Gold;
    public Inventory Inventory;

    public float moveSpeed;
    private bool isMoving;
    private Vector2 input;

    private Animator animator;

    public LayerMask solidObjectsLayer;
    public LayerMask interactableLayer;

     public Skill SelectedSkill { get; set; }


    
    private void Awake()
    {
        Instance = this;
        //DontDestroyOnLoad(this.gameObject);

        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator component not found on Player GameObject.");
            }
        }
    }


    void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            if(!isMoving)
            {
                input.x = Input.GetAxisRaw("Horizontal");
                input.y = Input.GetAxisRaw("Vertical");
                
                //Debug.Log("This is input.x: " + input.x);
                //Debug.Log("This is input.y: " + input.y);

                

                if(input.x != 0) input.y = 0;

                if (input != Vector2.zero)
                {
                    
                    animator.SetFloat("moveX", input.x);
                    animator.SetFloat("moveY", input.y);

                    var targetPos = transform.position;
                    targetPos.x += input.x;
                    targetPos.y += input.y;

                    if (IsWalkable(targetPos))
                        StartCoroutine(Move(targetPos));
                }
            }
            if (SceneManager.GetActiveScene().name == "MainScene")
            {
                animator.SetBool("isMoving",isMoving);
            }
            if(Input.GetKeyDown(KeyCode.Z))
                Interact();

            }
    }

    void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir ;

        Debug.DrawLine(transform.position, interactPos,Color.red,1f);

        var collider = Physics2D.OverlapCircle(interactPos,0.2f,interactableLayer);
        if(collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ( (targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer | interactableLayer) != null)
        {
            return false;
        }
        return true;
    }

    public void LevelUp()
    {
        Debug.Log($"{playerName} leveled up to level {Level}!");
    }

    public void GainExperience(int amount)
    {
        ExperiencePoints += amount;
        Debug.Log($"{playerName} gained {amount} experience points.");

        if (ExperiencePoints >= 100) // Adjust threshold as needed
        {
            Level++;
            ExperiencePoints = 0;
            LevelUp();
        }
    }

    // Override TakeDamage to customize behavior if needed
    public bool TakeDamage(int dmg)
    {
        int damageAfterArmor = Mathf.Max(0, dmg - armor);
        HealthPoints -=damageAfterArmor;
        HealthPoints -= dmg;
        if (HealthPoints <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Heal(int healing)
    {
        if(HealthPoints <MaxHealthPoints && (HealthPoints+healing) <= MaxHealthPoints)
        {
            HealthPoints += healing;
        }
        else if(HealthPoints <MaxHealthPoints && (HealthPoints+healing) > MaxHealthPoints)
        {
            HealthPoints = MaxHealthPoints;
        }
        else
        {
            HealthPoints = MaxHealthPoints;
        }
        
    }

    
}