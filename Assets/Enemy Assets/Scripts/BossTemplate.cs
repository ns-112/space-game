using UnityEngine;

public enum BossStage
{
    IDLE, //circling around center
    COOLDOWN, //frozen cooldown after attack then back to idle
    ATTACK01, //attack pattern 1
    ATTACK02, //attack pattern 2
    
};

public class BossTemplate : MonoBehaviour
{
    
   
    public float stageTimer = 8;


    //Idle props
    [SerializeField]
    private float idleSpeed;
    private float angle;
    private float centerPoint = 6;

    public BossStage stage = BossStage.IDLE;

    void Start()
    {
        
    }

    
    void Update()
    {
        //Timer between boss stages
        stageTimer -= Time.deltaTime;
        if (stageTimer <= 0 && stageTimer != -1)
        {
            //Swap stages
            switch (stage)
            {
                //Randomly pick an attack pattern to go to
                case BossStage.IDLE:   
                //timer can be set to whatever you want, or -1 if you want to specify your own end of attack
                //in your own script/function, just make sure to set the timer back to 0 after your attack is done
                if (Random.Range(0, 1) == 1)
                    {
                        stage = BossStage.ATTACK01;
                        stageTimer = 2;
                    } else
                    {
                        stage = BossStage.ATTACK02;
                        stageTimer = 4;
                    }
                break;

                //Move from any attack to cooldown
                case BossStage.ATTACK01:
                case BossStage.ATTACK02:
                stage = BossStage.COOLDOWN;
                stageTimer = 8;
                break;

                //Move from cooldown to idle
                case BossStage.COOLDOWN:
                stage = BossStage.IDLE;
                stageTimer = 8;
                break;
            }
        }

        switch (stage)
        {
            //Idle animation
            case BossStage.IDLE:
            angle += idleSpeed * Time.deltaTime;

            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);

            transform.position = new Vector3(x + centerPoint, 0, y);
            break;

            case BossStage.ATTACK01:
            //Implement Attack 1 pattern
            //May be going to a random pos and spewing bullets or whatever
            break;

            case BossStage.ATTACK02:
            //Implement Attack 2 pattern
            //Can be any attack, same as attack 1 but unique
            break;

            //Sit Still and let player attack
            case BossStage.COOLDOWN:

            break;
        }
    }
}
