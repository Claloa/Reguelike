using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public float smoothing = 1;
	public float restTime = 1;
	public AudioClip chop1Audio;
	public AudioClip chop2Audio;
	public AudioClip step1Audio;
	public AudioClip step2Audio;
	public AudioClip soda1Audio;
	public AudioClip soda2Audio;
	public AudioClip food1Audio;
	public AudioClip food2Audio;

	public float restTimer;


	[HideInInspector]public Vector2 targetPos = new Vector2(1,1);
	private Rigidbody2D arigidbody;
	private BoxCollider2D acollider;
	private Animator animator;
	// Use this for initialization
	void Start () {
		arigidbody = GetComponent<Rigidbody2D> ();
		acollider = GetComponent<BoxCollider2D> ();
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		arigidbody.MovePosition (Vector2.Lerp (transform.position, targetPos, smoothing * Time.deltaTime));

		if (GameManager.Instance.food <= 0 || GameManager.Instance.isEnd == true)
			return;
		
		restTimer += Time.deltaTime;
		if (restTimer < restTime) return;

		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");

		if (h > 0) {
			v = 0;
		}

		if (h != 0 || v != 0) {
			GameManager.Instance.RedurcelFood (1);
			acollider.enabled = false;
			RaycastHit2D hit = Physics2D.Linecast (targetPos,targetPos + new Vector2(h,v));
			acollider.enabled = true;
			if (hit.transform == null) {
				targetPos += new Vector2 (h, v);
				AudioManager.Instance.RandomPlay (step1Audio, step2Audio);
			} 
			else {
				switch (hit.collider.tag) {
					case"OutWall":
						break;
					case"Wall":
						animator.SetTrigger ("Attack");
						AudioManager.Instance.RandomPlay (chop1Audio, chop2Audio);
						hit.transform.SendMessage ("TakeDamage");
						break;
					case"Food":
						GameManager._instance.AddFood (10);
						targetPos += new Vector2 (h, v);
						Destroy (hit.transform.gameObject);
						AudioManager.Instance.RandomPlay (food1Audio, food2Audio);
						break;
					case"Soda":
						GameManager._instance.AddFood (20);
						targetPos += new Vector2 (h, v);
						Destroy (hit.transform.gameObject);
						AudioManager.Instance.RandomPlay (soda1Audio, soda2Audio);
						break;
					case"Enemy":
						break;
				}

			}
			GameManager.Instance.OnPlayMove ();
			restTimer = 0;
		}
	}

	public void TakeDamage(int lossFood){
		GameManager.Instance.RedurcelFood (lossFood);
		animator.SetTrigger ("Damage");
	}
}
