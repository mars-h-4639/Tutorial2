using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    private int scoreValue = 0;
   
    public Text winText;
    public Text loseText;

    public Text lives;
    private int livesValue = 3;
    private int levelValue = 1;

    public AudioClip musicBg;
    public AudioClip winSound;
    public AudioSource musicSource;

    Animator anim;

    private bool facingRight = true;
    
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;

    public float jumpForce;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        winText.text = " ";
        lives.text = "Lives: " + livesValue.ToString();
        loseText.text = " ";
        musicSource.clip = musicBg;
        musicSource.Play();
        musicSource.loop = true;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

         if (Input.GetKey("escape"))
         {
            Application.Quit();
         }

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        if (vertMovement > 0 && isOnGround == false)
        {
            anim.SetInteger("State", 2);
        }
        else if (vertMovement == 0 && isOnGround == true)
        {
            anim.SetInteger("State", 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
        }
        if (scoreValue == 4 & levelValue == 1)
        {
            transform.position = new Vector2(100.0f, 2.0f);
            levelValue = 2;
        }

        if (scoreValue == 8 & levelValue == 2)
        {
            musicSource.Stop();
            musicSource.clip = winSound;
            musicSource.loop = false;
            musicSource.Play();
            winText.text = "You win! Game created by Mars Hutchins!";
            Destroy(this);
            
        }

        if (collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            lives.text = "Lives: " + livesValue.ToString();
            Destroy(collision.collider.gameObject);
        }

        if (livesValue == 0)
        {
            loseText.text = "You Lose! Sorry!";
            Destroy(this);
        }

    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }

        
    }
    

    void Flip()
   {
     facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
   }

}


