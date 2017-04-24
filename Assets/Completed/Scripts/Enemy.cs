using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public Vector2 targetPos;
	public Transform player;
	public Rigidbody2D arigidbody;
	public int lossFood;
	public float smoothing = 3;
	public Animator animator;
	public AudioClip attackAudio;

	private BoxCollider2D acollider;


	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		arigidbody = GetComponent<Rigidbody2D> ();
		targetPos = transform.position;
		animator = GetComponent<Animator> ();
		GameManager._instance.EnemyList.Add (this);
		acollider = GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		arigidbody.MovePosition (Vector2.Lerp (transform.position, targetPos, smoothing * Time.deltaTime));
	}

	public void move(){
		Vector2 offset = player.position - transform.position;
		if (offset.magnitude < 1.1) {
			//攻击
			animator.SetTrigger("Attack");
			AudioManager.Instance.RandomPlay (attackAudio);
			player.SendMessage ("TakeDamage", lossFood);
		}
		else {
			float x = 0, y = 0;
			if (Mathf.Abs (offset.y) > Mathf.Abs (offset.x)) {
				if (offset.y < 0) {
					y = -1;
				} else {
					y = 1;
				}
			} 
			else {
				if (offset.x < 0) {
					x = -1;
				} else {
					x = 1;
				}
			}
			//设置目标位置之前先做检测
			acollider.enabled = false;
			RaycastHit2D hit = Physics2D.Linecast (targetPos,targetPos + new Vector2(x,y));
			acollider.enabled = true;
			if (hit.transform == null) {
				targetPos += new Vector2 (x, y);
			} 
			else {
				if (hit.collider.tag == "Food" || hit.collider.tag == "Soda") {
					targetPos += new Vector2 (x, y);
				}
			}

		}
	}
}
