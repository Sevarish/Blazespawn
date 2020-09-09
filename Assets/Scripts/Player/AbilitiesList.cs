using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesList : MonoBehaviour
{
    
    public delegate void Ability(KeyCode Input);
    public List<Ability> Rotation1 = new List<Ability>(),
                         Rotation2 = new List<Ability>(),
                         Rotation3 = new List<Ability>();
    private KeyCode[] input = new KeyCode[3];
    private int CurrentRotation = 1;
    [SerializeField] private Text CurrentRotationText;
    [SerializeField] GameObject EmitPoint,
                                basicAttackEmitter, basicFireCollider,

                                FirestreamObject,

                                BlazeburstEmitter, BlazeburstCollider,

                                StargazeObj,

                                FreezeblowObj, FreezeblowExpl,
                                
                                RecoveryObj,
                                
                                BloodsurgePtcl,
                                    
                                ShockthriveParticle,
                            
                                AcidicRushPtcl,
                                
                                IncineratePtcl,
                                
                                MindArtPtcl;

    Camera cam;
    PlayerSoundManager soundManager;
    bool basicAttackActive = false,
         blazeBurstActive = false,
         CurrentRotationSwitched = false,
         freezeBlowAnimStop = false,
         animationPlaying = false;
    private GameObject basicAttackObj,
                       blazeBurstObj;
    public string team, targetTeam;
    float animationTimer,
          animationDuration,
          StargazeAnimDuration = 0.5f;

    float basicAttackTimer = 0.5f,
          basicAttackDuration = 3,
          basicAttackActiveTime = 0,
          basicCollisionTimer = 0,
          basicCollisionRate = 0.05f,

          FirestreamTimer = 0.15f,
          FirestreamCooldown = 0.15f,

          blazeBurstCooldown = 5,
          blazeBurstTimer = 5,
          blazeBurstEffectTimer,

          StargazeTimer = 8,
          StargazeCooldown = 1,

          FreezeblowTimer = 0.6f,
          FreezeblowCooldown = 0.6f,

          RecoveryTimer = 1,
          RecoveryCooldown = 1,

          BloodsurgeTimer = 1.3f,
          BloodsurgeCooldown = 1.3f,
          amountOfBloodSurgePtcl = 12,

          ShockthriveTimer = 8,
          ShockthriveCooldown = 8,

          AcidicRushTimer = 12,
          AcidicRushCooldown = 12,
        
          IncinerateTimer = 15,
          IncinerateCooldown = 15,
        
          MindArtTimer = 0.1f,
          MindArtCooldown = 0.1f; 

    TargetSystem TgSystem;

    GameObject Hands;
    Animator anim;
    float animTimer = 0;

    private void Start()
    {
        SetAbilities();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        EmitPoint = cam.transform.GetChild(0).gameObject;
        soundManager = GetComponent<PlayerSoundManager>();
        team = this.gameObject.tag;
        TgSystem = GetComponent<TargetSystem>();

        Hands = GameObject.Find("Hands");
        anim = Hands.GetComponent<Animator>();

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

    public void SetAbilities()
    {
        input[0] = KeyCode.Joystick1Button2;
        input[1] = KeyCode.Joystick1Button3;
        input[2] = KeyCode.Joystick1Button1;

        Rotation1.Add(FirestreamVersion2);
        Rotation1.Add(BlazeBurst);
        Rotation1.Add(StargazeBomb);
        Rotation1.Add(Recovery);

        Rotation2.Add(Freezeblow);
        Rotation2.Add(Shockthrive);
        Rotation2.Add(AcidicRush);
        Rotation2.Add(Incinerate);

        Rotation3.Add(MindArtilery);

        SetAbilityList(Rotation1, Rotation2, Rotation3, input);
    }

    public void SetAbilityList(List<Ability> list1, List<Ability> list2, List<Ability> list3, KeyCode[] inputs)
    {
        Rotation1 = list1;
        Rotation2 = list2;
        Rotation3 = list3;

        input = inputs;
    }

    private void AnimationHandling()
    {
        if (animationPlaying)
        {
            animationTimer += Time.deltaTime;
            if (animationTimer > animationDuration)
            {
                anim.Play("Static");
                animationTimer = 0;
                animationPlaying = false;
            }
        }
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
            Rotation1[0](KeyCode.A);
            Rotation1[1](input[0]);
            Rotation1[2](input[1]);
            Rotation1[3](input[2]);
        }
        if (CurrentRotation == 2)
        {
            Rotation2[0](KeyCode.A);
            Rotation2[1](input[0]);
            Rotation2[2](input[1]);
            Rotation2[3](input[2]);
        }
        if (CurrentRotation == 3)
        {
            Rotation3[0](KeyCode.A);
        }
        AnimationHandling();
    }

    //FIRE TYPE-----------------------------------------------

    public void FirestreamVersion2(KeyCode key)
    {
        FirestreamTimer += Time.deltaTime;
        if ((Input.GetMouseButtonDown(0) || Input.GetAxis("R2") != 0) && FirestreamTimer > FirestreamCooldown)
        {
            var FirestreamObj = Instantiate(FirestreamObject, new Vector3(EmitPoint.transform.position.x, EmitPoint.transform.position.y - 0.2f, EmitPoint.transform.position.z), cam.transform.rotation);
            FirestreamObj.GetComponent<FirestreamBehaviour>().damage = 6;
            FirestreamTimer = 0;
        }

        if (Input.GetAxis("R2") != 0)
        {
            anim.Play("Fire");
        }
        else
        {
            anim.Play("FireEnd");
        }
    }

    public void Firestream()
    {
        //BASIC ATTACK
        basicAttackTimer += Time.deltaTime;
        if ((Input.GetMouseButtonDown(0) || Input.GetAxis("R2") != 0) && !basicAttackActive && basicAttackTimer > 0.6f)
        {
            basicAttackActive = true;
            basicAttackObj = Instantiate(basicAttackEmitter, new Vector3(EmitPoint.transform.position.x, EmitPoint.transform.position.y - 0.2f, EmitPoint.transform.position.z), cam.transform.rotation);
            basicAttackObj.transform.parent = cam.transform;
            soundManager.PlayBasicAttack();
            basicCollisionTimer = 0.5f;
        }

        if (basicAttackActive)
        {
            anim.Play("Fire");
            basicAttackActiveTime += Time.deltaTime;
            if (basicAttackActiveTime > basicAttackDuration)
            {
                anim.Play("FireEnd");
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
            basicAttackActiveTime = 0;
            soundManager.StopBasicAttack();
            anim.Play("FireEnd");
        }
        //END OF BASIC ATTACK
    }

    public void BlazeBurst(KeyCode inputKey)
    {
        //BLAZEBURST
        blazeBurstTimer += Time.deltaTime;
        if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(inputKey)) && blazeBurstTimer > blazeBurstCooldown)
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

    public void StargazeBomb(KeyCode inputKey)
    {
        StargazeTimer += Time.deltaTime;
        if (Input.GetKeyDown(inputKey) && StargazeTimer >= StargazeCooldown)
        {
            anim.Play("StargazeStart");
            animationPlaying = true;
            animationDuration = StargazeAnimDuration;
            Instantiate(StargazeObj, new Vector3(EmitPoint.transform.position.x, EmitPoint.transform.position.y, EmitPoint.transform.position.z), cam.transform.rotation);
            StargazeObj.GetComponent<StargazeBombBehaviour>().targetTeam = targetTeam;
            StargazeTimer = 0;
        }
    }

    public void Incinerate(KeyCode inputKey)
    {
        IncinerateTimer += Time.deltaTime;
        if (Input.GetKeyDown(inputKey) && IncinerateTimer > IncinerateCooldown && TgSystem.GetTarget() != null)
        {
            TgSystem.GetTarget().AddComponent<DotFire>();
            TgSystem.GetTarget().GetComponent<DotFire>().InitiatalSetup(2, 3, 0.2f); //DURATION | DAMAGE PER INTERVAL | INTERVAL
            var obj = Instantiate(IncineratePtcl, TgSystem.GetTarget().transform.position, Quaternion.identity);
            Destroy(obj, 0.8f);
            IncinerateTimer = 0;
        }
    }

    //ICE TYPE-----------------------------------------------

    public void Freezeblow(KeyCode key)
    {
        FreezeblowTimer += Time.deltaTime;
        if (Input.GetAxis("R2") != 0 && FreezeblowTimer > FreezeblowCooldown)
        {
            freezeBlowAnimStop = true;
            anim.Play("Freezeblow");
            var obj = Instantiate(FreezeblowObj, new Vector3(EmitPoint.transform.position.x, EmitPoint.transform.position.y, EmitPoint.transform.position.z), cam.transform.rotation);
            obj.GetComponent<FreezeblowBehaviour>().damage = 25;
            var expl = Instantiate(FreezeblowExpl, new Vector3(EmitPoint.transform.position.x, EmitPoint.transform.position.y, EmitPoint.transform.position.z), cam.transform.rotation);
            expl.transform.parent = this.gameObject.transform;
            Destroy(expl, 0.8f);
            FreezeblowTimer = 0;
        }
        else if (Input.GetAxis("R2") == 0 && freezeBlowAnimStop)
        {
            anim.Play("Static");
            freezeBlowAnimStop = false;
        }
    }

    //NATURE TYPE-----------------------------------------------

    public void Recovery(KeyCode inputKey)
    {
        RecoveryTimer += Time.deltaTime;
        if (Input.GetKeyDown(inputKey) && RecoveryTimer > RecoveryCooldown)
        {
            this.gameObject.GetComponent<PlayerHealthSystem>().Heal(20);
            var obj = Instantiate(RecoveryObj, transform.position, transform.rotation);
            obj.transform.parent = this.gameObject.transform;
            Destroy(obj, 0.8f);
            RecoveryTimer = 0;
        }
    }

    //DARK TYPE-----------------------------------------------

    public void Bloodsurge(KeyCode key)
    {
        BloodsurgeTimer += Time.deltaTime;
        if (Input.GetAxis("R2") != 0 && BloodsurgeTimer > BloodsurgeCooldown)
        {
            if (TgSystem.GetTarget() != null)
            {
                TgSystem.GetTarget().GetComponent<HealthSystem>().TakeDamage(23, "Dark");
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

    //PSYCHIC TYPE------------------------------------------------

    public void Shockthrive(KeyCode inputKey)
    {
        ShockthriveTimer += Time.deltaTime;
        if (Input.GetKeyDown(inputKey) && ShockthriveTimer > ShockthriveCooldown && TgSystem.GetTarget() != null)
        {
            TgSystem.GetTarget().GetComponent<HealthSystem>().TakeDamage(10, "Psychic");
            var obj = Instantiate(ShockthriveParticle, TgSystem.GetTarget().transform.position, Quaternion.identity);
            obj.GetComponent<ShockthriveBehaviour>().damage = 20;
            Destroy(obj, 0.5f);
            ShockthriveTimer = 0;
        }
    }

    public void MindArtilery(KeyCode key)
    {
        MindArtTimer += Time.deltaTime;
        if (Input.GetAxis("R2") != 0 && MindArtTimer > MindArtCooldown && TgSystem.GetTarget() != null)
        {
            var obj = Instantiate(MindArtPtcl, EmitPoint.transform.position, transform.rotation);
            obj.GetComponent<MindArtileryBehaviour>().target = TgSystem.GetTarget();
            obj.GetComponent<MindArtileryBehaviour>().damage = 5;
            MindArtTimer = 0;
        }
    }

    //POISON TYPE-----------------------------------------------------

    public void AcidicRush(KeyCode inputKey)
    {
        AcidicRushTimer += Time.deltaTime;
        if (Input.GetKeyDown(inputKey) && AcidicRushTimer > AcidicRushCooldown && TgSystem.GetTarget() != null)
        {
            TgSystem.GetTarget().AddComponent<DotPoison>();
            TgSystem.GetTarget().GetComponent<DotPoison>().InitiatalSetup(5, 6, 0.5f); //DURATION | DAMAGE PER INTERVAL | INTERVAL
            var obj = Instantiate(AcidicRushPtcl, TgSystem.GetTarget().transform.position, Quaternion.identity);
            Destroy(obj, 0.8f);
            AcidicRushTimer = 0;
        }
    }
}
