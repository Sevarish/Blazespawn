using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesList : MonoBehaviour
{
    
    private List<System.Action> rotationList1 = new List<System.Action>(), 
                                rotationList2 = new List<System.Action>(),
                                rotationList3 = new List<System.Action>();
    private int CurrentRotation = 1;
    [SerializeField] private Text CurrentRotationText;
    [SerializeField] GameObject EmitPoint,
                                basicAttackEmitter, basicFireCollider,

                                BlazeburstEmitter, BlazeburstCollider,

                                StargazeObj,

                                FreezeblowObj, FreezeblowExpl,
                                
                                RecoveryObj,
                                
                                BloodsurgePtcl,
                                    
                                ShockthriveParticle;

    Camera cam;
    PlayerSoundManager soundManager;
    [SerializeField] Slider fireStreamCDImage;
    Slider fireStreamCDObject;
    bool basicAttackActive = false,
         blazeBurstActive = false,
         CurrentRotationSwitched = false;
    private GameObject basicAttackObj,
                       blazeBurstObj;
    public string team, targetTeam;
    float basicAttackTimer = 0.5f,
          basicAttackDuration = 3,
          basicAttackActiveTime = 0,
          basicCollisionTimer = 0,
          basicCollisionRate = 0.05f,

          blazeBurstCooldown = 5,
          blazeBurstTimer = 5,
          blazeBurstEffectTimer,

          StargazeTimer = 8,
          StargazeCooldown = 1,

          FreezeblowTimer = 0.6f,
          FreezeblowCooldown = 0.6f,
          
          RecoveryTimer = 1,
          RecoveryCooldown = 1,
        
          BloodsurgeTimer = 1.5f,
          BloodsurgeCooldown = 0.8f,
          amountOfBloodSurgePtcl = 10,
        
          ShockthriveTimer = 8,
          ShockthriveCooldown = 8;

    TargetSystem TgSystem;

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        EmitPoint = cam.transform.GetChild(0).gameObject;
        soundManager = GetComponent<PlayerSoundManager>();
        team = this.gameObject.tag;
        TgSystem = GetComponent<TargetSystem>();

        //Sets target team opposite of own team.
        if (team == "Blue")
        {
            targetTeam = "Red";
        }
        else
        {
            targetTeam = "Blue";
        }
        CurrentRotationText.text = "" + CurrentRotation;
    }

    public void SetAbilityList(List<System.Action> list1, List<System.Action> list2, List<System.Action> list3)
    {
        rotationList1 = list1;
        rotationList2 = list2;
        rotationList3 = list3;
        //rotationList3 = list2;
    }

    void Update()
    {
        SpellRotationSwitch();
        SpellRotation();
    }

    private void SpellRotationSwitch()
    {
        if (Input.GetAxis("L1") > 0.1)
        {
            if (!CurrentRotationSwitched)
            {
                if (CurrentRotation < 3)
                {
                    CurrentRotation += 1;
                }
                else
                {
                    CurrentRotation = 1;
                }
            }
            CurrentRotationText.text =  "" + CurrentRotation;
            CurrentRotationSwitched = true;
        }
        else
        {
            CurrentRotationSwitched = false;
        }
    }

    private void SpellRotation()
    {
        if (CurrentRotation == 1)
        {
            rotationList1[0]();
            rotationList1[1]();
            rotationList1[2]();
            rotationList1[3]();
        }
        if (CurrentRotation == 2)
        {
            rotationList2[0]();
            rotationList2[1]();
        }
        if (CurrentRotation == 3)
        {
            rotationList3[0]();
        }
    }

    //FIRE TYPE-----------------------------------------------

    public void Firestream()
    {
        //BASIC ATTACK
        basicAttackTimer += Time.deltaTime;
        if ((Input.GetMouseButtonDown(0) || Input.GetAxis("R2") != 0) && !basicAttackActive && basicAttackTimer > 0.6f)
        {
            basicAttackActive = true;
            basicAttackObj = Instantiate(basicAttackEmitter, new Vector3(EmitPoint.transform.position.x, EmitPoint.transform.position.y, EmitPoint.transform.position.z), cam.transform.rotation);
            basicAttackObj.transform.parent = cam.transform;
            soundManager.PlayBasicAttack();
            basicCollisionTimer = 0.5f;
        }

        if (basicAttackActive)
        {
            basicAttackActiveTime += Time.deltaTime;
            if (basicAttackActiveTime > basicAttackDuration)
            {
                basicAttackActive = false;
                if (basicAttackObj != null)
                {
                    basicAttackObj.GetComponent<ParticleSystem>().Stop();
                    Destroy(basicAttackObj, 0.5f);
                }
                basicAttackTimer = 0;
                basicAttackActiveTime = 0;
                soundManager.StopBasicAttack();
            }

            //Here every (basicCollisionRate) a empty object with a collider and a collision script is fired from the EmitPoint.
            basicCollisionTimer += Time.deltaTime;
            if (basicCollisionTimer > basicCollisionRate)
            {
                Instantiate(basicFireCollider, new Vector3(EmitPoint.transform.position.x, EmitPoint.transform.position.y, EmitPoint.transform.position.z), cam.transform.rotation);
                //basicFireCollider.GetComponent<BasicFireCollision>().teamTarget = targetTeam;
                //DAMAGE HERE CAN BE CHANGED BY FUTURE UPDATES, I.E CALCULATION OF DIFFERENT BUFFS/DEBUFFS
                basicFireCollider.GetComponent<BasicFireCollision>().damage = 2;
                basicCollisionTimer = 0;
            }
        }

        if ((Input.GetMouseButtonUp(0) || Input.GetAxis("R2") == 0) && basicAttackActive)
        {
            basicAttackActive = false;
            if (basicAttackObj != null)
            {
                basicAttackObj.GetComponent<ParticleSystem>().Stop();
                Destroy(basicAttackObj, 0.5f);
            }
            basicAttackTimer = 0;
            soundManager.StopBasicAttack();
        }
        //END OF BASIC ATTACK
    }

    public void BlazeBurst()
    {
        //BLAZEBURST
        blazeBurstTimer += Time.deltaTime;
        if ((Input.GetMouseButtonDown(1) || Input.GetAxis("Controller Button Square") != 0) && blazeBurstTimer > blazeBurstCooldown)
        {
            Instantiate(BlazeburstCollider, new Vector3(EmitPoint.transform.position.x, EmitPoint.transform.position.y, EmitPoint.transform.position.z), cam.transform.rotation);
            BlazeburstCollider.GetComponent<BlazeBurstCollision>().teamTarget = targetTeam;
            //DAMAGE HERE CAN BE CHANGED BY FUTURE UPDATES, I.E CALCULATION OF DIFFERENT BUFFS/DEBUFFS
            BlazeburstCollider.GetComponent<BlazeBurstCollision>().damage = 20;

            blazeBurstObj = Instantiate(BlazeburstEmitter, new Vector3(EmitPoint.transform.position.x, EmitPoint.transform.position.y, EmitPoint.transform.position.z), cam.transform.rotation);
            Destroy(blazeBurstObj, 0.7f);
            blazeBurstTimer = 0;
            blazeBurstActive = true;
        }

        if (blazeBurstActive)
        {
            blazeBurstEffectTimer += Time.deltaTime;
            if (blazeBurstEffectTimer > 0.15f)
            {
                blazeBurstObj.GetComponent<ParticleSystem>().Stop();
                blazeBurstEffectTimer = 0;
                blazeBurstActive = false;
            }
        }
        //END OF BLAZEBURST
    }

    public void StargazeBomb()
    {
        StargazeTimer += Time.deltaTime;
        if (Input.GetAxis("Controller Button Circle") != 0 && StargazeTimer >= StargazeCooldown)
        {
            Instantiate(StargazeObj, new Vector3(EmitPoint.transform.position.x, EmitPoint.transform.position.y, EmitPoint.transform.position.z), cam.transform.rotation);
            StargazeObj.GetComponent<StargazeBombBehaviour>().targetTeam = targetTeam;
            StargazeTimer = 0;
        }
    }

    //ICE TYPE-----------------------------------------------

    public void Freezeblow()
    {
        FreezeblowTimer += Time.deltaTime;
        if (Input.GetAxis("R2") != 0 && FreezeblowTimer > FreezeblowCooldown)
        {
            var obj = Instantiate(FreezeblowObj, new Vector3(EmitPoint.transform.position.x, EmitPoint.transform.position.y, EmitPoint.transform.position.z), cam.transform.rotation);
            obj.GetComponent<FreezeblowBehaviour>().damage = 25;
            var expl = Instantiate(FreezeblowExpl, new Vector3(EmitPoint.transform.position.x, EmitPoint.transform.position.y, EmitPoint.transform.position.z), cam.transform.rotation);
            expl.transform.parent = this.gameObject.transform;
            Destroy(expl, 0.8f);
            FreezeblowTimer = 0;
        }
    }

    //NATURE TYPE-----------------------------------------------

    public void Recovery()
    {
        RecoveryTimer += Time.deltaTime;
        if (Input.GetAxis("Controller Button Triangle") != 0 && RecoveryTimer > RecoveryCooldown)
        {
            this.gameObject.GetComponent<PlayerHealthSystem>().Heal(20);
            var obj = Instantiate(RecoveryObj, transform.position, transform.rotation);
            obj.transform.parent = this.gameObject.transform;
            Destroy(obj, 0.8f);
            RecoveryTimer = 0;
        }
    }

    //DARK TYPE-----------------------------------------------

    public void Bloodsurge()
    {
        BloodsurgeTimer += Time.deltaTime;
        if (Input.GetAxis("R2") != 0 && BloodsurgeTimer > BloodsurgeCooldown)
        {
            if (TgSystem.GetTarget() != null)
            {
                TgSystem.GetTarget().GetComponent<HealthSystem>().TakeDamage(15);
                for (int i = 0; i < amountOfBloodSurgePtcl; i++)
                {
                    Instantiate(BloodsurgePtcl, TgSystem.GetTarget().transform.position, Quaternion.Euler(90, 0, 0));
                }
                BloodsurgeTimer = 0;
            }
            else
            {
                //OUT OF RANGE TEXT
            }
        }
    }

    //PSYCHIC TYPE

    public void Shockthrive()
    {
        ShockthriveTimer += Time.deltaTime;
        if (Input.GetAxis("Controller Button Square") != 0 && ShockthriveTimer > ShockthriveCooldown)
        {
            TgSystem.GetTarget().GetComponent<HealthSystem>().TakeDamage(10);
            var obj = Instantiate(ShockthriveParticle, TgSystem.GetTarget().transform.position, Quaternion.identity);
            obj.GetComponent<ShockthriveBehaviour>().damage = 20;
            Destroy(obj, 0.5f);
            ShockthriveTimer = 0;
        }
    }
}
