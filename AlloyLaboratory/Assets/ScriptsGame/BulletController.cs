using UnityEngine;

public class BulletController : MonoBehaviour
{
    //プレイヤーの方向に飛んでいく
    //レベルによってちょっとだけ追尾したりしても面白そう
    GameObject shootTarget;
    Vector2 targetDirection;
    public string shootTargetName;
    public float speed;

    public float chaseLevel = 0f;//EnemyGuardianControllerからいじる
    float time = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shootTarget = GameObject.FindGameObjectWithTag(shootTargetName);
        targetDirection = new Vector2(shootTarget.transform.position.x - transform.position.x,
        shootTarget.transform.position.y - transform.position.y).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        transform.position += new Vector3(targetDirection.x * speed * Time.deltaTime,
        targetDirection.y * speed * Time.deltaTime, 0f);

        if (time + chaseLevel >= 3f)
        {
            //一定時間経過後追尾するように
            targetDirection = new Vector2(shootTarget.transform.position.x - transform.position.x,
            shootTarget.transform.position.y - transform.position.y).normalized;
        }
        
        if (time >= 3f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
