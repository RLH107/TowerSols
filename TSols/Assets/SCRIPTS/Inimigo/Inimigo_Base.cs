using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo_Base : MonoBehaviour
{
    [SerializeField] private string EnemyTipe;
    [SerializeField] private Rigidbody rb;

    [HideInInspector] public bool ActivationState;

    private float Health;
    private float MaxHealth; 
    private float HealthCheck;
    private float Attack;
    private float Speed;
    private float SpeedVariance;
    private float Force;

    private float MaxSpeed;
    private float MinSpeed;

    private Vector3 VForce;
    private Quaternion RotateTo;
    public float RSpeed;

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    
    private enum EState
    {
        IDLE,
        MOVE,
        TURN,
    }
    private EState enemyState;


    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// <param name="Start"></param>


    void Start()
    {
        if (Health <= 0f)
        {
            Health = 50f;
        }

        if (MaxHealth <= 0f)
        {
            MaxHealth = 55f;
        }

        if (Attack <= 0f)
        {
            Attack = 5f;
        }

        if (Speed <= 0f)
        {
            Speed = 2f;
        }

        if (SpeedVariance <= 0f)
        {
            SpeedVariance = 0.5f;
        }

        if (Force <= 0f)
        {
            Force = 1f;
        }

        RSpeed = 1f;

        MaxSpeed = Speed + SpeedVariance;
        MinSpeed = Speed - SpeedVariance;

        //Debug.Log(MinSpeed +"MIN MAX"+  MaxSpeed);

        ActivationState = false;
        VForce = new Vector3(Force, 0, 0);
        RotateTo = transform.rotation;
        EnemyStateSwitch(EState.IDLE);
    }


    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// <param name="DemegeTaken"></param>


    private void TakeDamege(float DemegeTaken)
    {
        Debug.Log("TakeDamge_CALLED");
        Health -= DemegeTaken;
        if (Health <= 0)
        {
            Dead();
        }
    }

    private void AddHealth(float HealthToAdd)
    {
        Debug.Log("addHealth_CALLED");
        HealthCheck = Health;
        HealthCheck += HealthToAdd;
        if(HealthCheck < MaxHealth)
        {
            Debug.Log("If_CALLED");
            Health += HealthToAdd;
        }
        else
        {
            Debug.Log("Else_CALLED");
            Health = MaxHealth;
        }
    }

    

    private void Dead()
    {
        Debug.Log(gameObject + "DEAD");
        ActivationState = false;
        EnemyStateSwitch(EState.IDLE);
    }



    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// <param name="collision"></param>




    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "TurnColider")
        {
            Debug.Log("Colision Detected");
            RotateTo = collision.gameObject.transform.rotation;
            
            EnemyStateSwitch(EState.TURN);
        }
    }


    /// <summary>
    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// <param name="StateSwitch"></param>


    private void EnemyStateSwitch(EState E_State)
    {
        enemyState = E_State;
        switch (enemyState)
        {
            case EState.IDLE:
                StartCoroutine(IDLE());
                break;
            case EState.MOVE:
                StartCoroutine(MOVE());
                break;
            case EState.TURN:
                StartCoroutine(TURN());
                break;
        }
    }

    /// <summary>
    /// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// <param name="IEnumerators"></param>

    private IEnumerator IDLE()
    {
        //Debug.Log("IDLE Called");
        yield return null;
        if (ActivationState == true)
        {
            //Debug.Log("ActivationState" + ActivationState);
            EnemyStateSwitch(EState.MOVE);
        }
        else
        {
            //Debug.Log("ActivationState" + ActivationState);
            EnemyStateSwitch(EState.IDLE);
        }
    }


    private IEnumerator MOVE()
    {
        //Debug.Log("MOVE Called");
        yield return null;
        if(ActivationState == false)
        {
            EnemyStateSwitch(EState.IDLE);
        }
        else
        {
            if(transform.rotation == RotateTo)
            {
                ///////////////////////////////////////////////////////////////////
                //EnemyVelocity = rb.velocity;
                if(rb.velocity.x < Speed)
                {
                    Debug.Log("Velocity < Speed " + rb.velocity);
                    rb.AddForce( VForce, ForceMode.Acceleration);
                }
                if(rb.velocity.x > Speed)
                {
                    Debug.Log("Velocity > Speed " + rb.velocity);
                    rb.AddForce(-VForce, ForceMode.Acceleration);
                }
                else
                {
                    Debug.Log("Velo = Speed");
                }
                ///////////////////////////////////////////////////////////////////

                EnemyStateSwitch(EState.MOVE);
            }
            else
            {
                EnemyStateSwitch(EState.TURN);
            }
        }
    }


    private IEnumerator TURN()
    {
        //Debug.Log("TURN Called");
        yield return null;
        ////////////////////////////////////////////////////////////////////
        transform.rotation = Quaternion.RotateTowards(transform.rotation, RotateTo, RSpeed * Time.deltaTime);
        ////////////////////////////////////////////////////////////////////
        EnemyStateSwitch(EState.TURN);
    }
    
    //private void TurnEnemy(float HowMutshToRotate, float HowFast)
    //{
    //    ToRotate = Quaternion.Euler(0, HowMutshToRotate, 0);
    //    transform.rotation = Quaternion.Slerp(transform.rotation, ToRotate, Time.deltaTime * HowFast);
    //}
}
