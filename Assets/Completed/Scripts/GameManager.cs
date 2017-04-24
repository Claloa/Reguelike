using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager _instance;

	public static GameManager Instance{
		get{ 
			return _instance;
		}
	}

	public int level = 1;
	public int food = 100;
	public AudioClip dieAudio;

	[HideInInspector]public bool isEnd = false; //是否到达终点
	[HideInInspector]public List<Enemy> EnemyList = new List<Enemy>();
	public bool sleepStep = true;
	private Text foodText;
	private Text failText;
	private Image dayImage;
	private Text dayText;

	private Player player;
	private MapManager mapManager;



	void Awake(){
		_instance = this;
		DontDestroyOnLoad (gameObject);
		InitGame ();
	}

	void InitGame(){
		//初始化地图
		mapManager = GetComponent<MapManager> ();
		mapManager.InitMap ();
		//初始化ui
		foodText = GameObject.Find ("FoodText").GetComponent<Text> ();
		UpdateFoodText (0);
		failText = GameObject.Find ("FailText").GetComponent<Text> ();
		failText.enabled = false;
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
		dayImage = GameObject.Find ("DayImage").GetComponent<Image> ();
		dayText = GameObject.Find ("DayText").GetComponent<Text> ();
		dayText.text = "Day " + level;
		Invoke ("HideBlack", 1);
		//初始化参数
		isEnd = false;
		EnemyList.Clear ();
	}

	void UpdateFoodText(int foodChange){
		if (foodChange == 0) {
			foodText.text = "Food:" + food;
		} 
		else {
			string str = "";
			if (foodChange < 0) {
				str = foodChange.ToString ();
			} 
			else {
				str = "+" + foodChange;
			}
			foodText.text = str + " Food:" + food; 
		}
	}

	public void RedurcelFood(int count){
		food -= count;
		UpdateFoodText (-count);

		if (food <= 0) {
			failText.enabled = true;
			AudioManager.Instance.StopBgMusic ();
			AudioManager.Instance.RandomPlay (dieAudio);
		}
	}

	public void AddFood(int count){
		food += count;
		UpdateFoodText (count);
	}

	public void OnPlayMove(){
		if (sleepStep == true) {
			sleepStep = false;
		} 
		else {
			foreach (var enemy in EnemyList) {
				enemy.move ();
			}
			sleepStep = true;
		}
		//检测有没有到达终点
		if (player.targetPos.x == mapManager.cols - 2 && player.targetPos.y == mapManager.rows - 2) {
			isEnd = true;
			//加载下一个关卡
			Application.LoadLevel(Application.loadedLevel);
		}
	}

	void OnLevelWasLoaded(int scenceLeavel){
		level++;
		InitGame ();//初始化游戏
	}

	void HideBlack(){
		dayImage.gameObject.SetActive (false);
	}
}
