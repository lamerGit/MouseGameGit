using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private GameObject Player = null; //플레이어 미리 찾아놓기위한 변수
    private GameObject Mouse = null; // 마우스 미리 찾아놓기위한 변수
    private Canvas canvas; // 캔버스 찾아놓기 위한 변수

    //미리 만들어 놓기위한 변수와 큐ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    public GameObject Enemy = null; 
    private Queue<GameObject> EnemyQueue= new Queue<GameObject>();

    public GameObject DamageText = null;
    private Queue<GameObject> DamageTextQueue= new Queue<GameObject>();

    public GameObject BoardWeapon = null;
    private Queue<GameObject> BoardWeaponQueue = new Queue<GameObject>();

    public GameObject TargetWeapon = null;
    private Queue<GameObject> TargetWeaponQueue = new Queue<GameObject>();

    public GameObject ChaseWeapon = null;
    private Queue<GameObject> ChaseWeaponQueue = new Queue<GameObject>();

    public GameObject ThunderWeapon = null;
    private Queue<GameObject> ThunderWeaponQueue=new Queue<GameObject>();

    public GameObject Exp = null;
    private Queue<GameObject> ExpQueue = new Queue<GameObject>();

    private GameObject[] RotateWeapon = new GameObject[2];

    public GameObject Enemy2;
    private Queue<GameObject> Enemy2Queue = new Queue<GameObject>();
    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    private bool GamePause = false; //게임 정지상태를 확인할 변수

    private GameObject Map = null; //맵 오브젝트를 미리찾아둘 변수


    //세이브데이터들ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    private int healthLevel = 0;
    private int attackLevel = 0;
    private int expLevel = 0;
    private int Token = 0;
    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    private Dictionary<WeaponEnum, int> WeaponDamages= new Dictionary<WeaponEnum, int>(); // 데미지별로 저장하기위한 변수

    private int EnemyExtraHp = 0; // 시간이 지날수록 적의 체력을 증가시켜줄 변수

    

    TimeScript timeScript; // TimeScript를 미리찾아두기 위한 변수

    //프로퍼티ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    public TimeScript TIMESCRIPT
    {
        get { return timeScript; }
    }

    public Queue<GameObject> ENEMY2QUEUE
    {
        get { return Enemy2Queue; }
    }

    public int ENEMYEXTRAHP
    {
        get { return EnemyExtraHp; }
        set { EnemyExtraHp = value; }
    }
    
    public static GameManager INSTANCE
    {
        get
        {
            return instance;
        }
    }

    public Dictionary<WeaponEnum,int> WEAPONDAMAGES
    {
        get { return WeaponDamages; }
        set { WeaponDamages = value; }
    }

    public int HEALTHLEVEL
    {
        get { return healthLevel; }
    }

    public int ATTACKLEVEL
    {
        get { return attackLevel; }
    }

    public int EXPLEVEL
    {
        get { return expLevel; }
    }
    public GameObject MAP
    {
        get { return Map; }
    }

    public GameObject[] ROTATEWEAPON
    {
        get { return RotateWeapon; }
        set { RotateWeapon = value; }
    }

    public bool GAMEPAUSE
    {
        get { return GamePause; }
        set { GamePause = value; }

    }

    public GameObject PLAYER
    {
        get { return Player; }
    }

    public Queue<GameObject> ENEMYQUEUE
    {
        get { return EnemyQueue; }
    }

    public Queue<GameObject> DAMAGETEXTQUEUE
    {
        get { return DamageTextQueue; }
    }

    public Queue<GameObject> BOARDWEAPONQUEUE
    {
        get { return BoardWeaponQueue; }
    }

    public Queue<GameObject> TARGETWEAPONQUEUE
    {
        get { return TargetWeaponQueue; }
    }

    public Queue<GameObject> CHASEWEAPONQUEUE
    {
        get { return ChaseWeaponQueue; }
    }

    public Queue<GameObject> THUNDERWEAPONQUEUE
    {
        get { return ThunderWeaponQueue; }
    }

    public Queue<GameObject> EXPQUEUE
    {
        get { return ExpQueue; }
    }
    public Canvas CANBAS
    {
        get { return canvas; }
    }

    public GameObject MOUSE
    {
        get { return Mouse; }
    }
    //프로퍼티ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    private void Awake()
    {
        //싱글톤 작업
        if(instance==null)
        {
            instance = this;

            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(instance!=this)
            {
                Destroy(gameObject);
            }
        }
        // 오브젝트 미리찾아두기
        Player = GameObject.FindGameObjectWithTag("Player");
        canvas=GameObject.FindGameObjectWithTag("Canvas").gameObject.GetComponent<Canvas>();
        Mouse = GameObject.FindGameObjectWithTag("Mouse");
        RotateWeapon = GameObject.FindGameObjectsWithTag("RotateWeapon");
        Map = GameObject.FindGameObjectWithTag("Map");
        timeScript=FindObjectOfType<TimeScript>();

        
        LoadLevelData(); // 데이터불러오기

        //오브젝트 미리 만들어서 queue에 할당
        for(int i=0; i<1000; i++)
        {
            GameObject E=Instantiate(Enemy);
            E.SetActive(false);
            EnemyQueue.Enqueue(E);
        }

        for (int i = 0; i < 100; i++)
        {
            GameObject T = Instantiate(DamageText);
            T.SetActive(false);
            T.transform.SetParent(CANBAS.transform);
            DamageTextQueue.Enqueue(T);
        }

        for(int i=0; i<1000; i++)
        {
            GameObject B = Instantiate(BoardWeapon);
            B.SetActive(false);
            BoardWeaponQueue.Enqueue(B);
        }

        for (int i = 0; i < 100; i++)
        {
            GameObject TW = Instantiate(TargetWeapon);
            TW.SetActive(false);
            TargetWeaponQueue.Enqueue(TW);
        }

        for(int i=0; i<3; i++)
        {
            GameObject CW = Instantiate(ChaseWeapon);
            CW.SetActive(false);
            ChaseWeaponQueue.Enqueue(CW);
        }

        for(int i=0; i<10; i++)
        {
            GameObject THW = Instantiate(ThunderWeapon);
            THW.SetActive(false);
            ThunderWeaponQueue.Enqueue(THW);
        }

        for(int i=0; i<100; i++)
        {
            GameObject E = Instantiate(Exp);
            E.SetActive(false);
            ExpQueue.Enqueue(E);
        }

        foreach(WeaponEnum i in System.Enum.GetValues(typeof(WeaponEnum)))
        {
            WeaponDamages.Add(i, 0);
        }

        for(int i=0; i<400; i++)
        {
            GameObject E2 = Instantiate(Enemy2);
            E2.SetActive(false);
            Enemy2Queue.Enqueue(E2);
        }

        Cursor.visible = false;
       
    }

    //데이터를 불러오는 함수
    void LoadLevelData()
    {
     
        string path = $"{Application.dataPath}/Save/";
        string fullPath = $"{path}Save.json";
        if (Directory.Exists(path) && File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            healthLevel = saveData.HealthLevel;
            attackLevel = saveData.AttackLevel;
            expLevel = saveData.ExpLevel;
            Token = saveData.Token;
            
        }

        
    }
    //저장되었던 데미지총합을 순서대로 정렬하여 저장하는 함수
    public void SaveDamges()
    {
        //foreach(var i in WeaponDamages)
        //{
        //    Debug.Log($"Weapon = {i.Key} , Damages = {i.Value}");
        //}

        SaveData saveData = new();
        int[] weaponName = new int[System.Enum.GetValues(typeof(WeaponEnum)).Length];
        int[] weaponDamage = new int[System.Enum.GetValues(typeof(WeaponEnum)).Length];
        int count = 0;
        var order = WeaponDamages.OrderByDescending(x => x.Value);

        foreach(var i in order)
        {
            weaponName[count] =(int) i.Key;
            weaponDamage[count] =i.Value;
            count++;
        }
        saveData.HealthLevel = healthLevel;
        saveData.AttackLevel = attackLevel;
        saveData.ExpLevel = expLevel;
        saveData.Token = Token;
        saveData.WeaponName = weaponName;
        saveData.WeaponDamage = weaponDamage;

        string json = JsonUtility.ToJson(saveData);     

        string path = $"{Application.dataPath}/Save/";  
        if (!Directory.Exists(path))           
        {
            Directory.CreateDirectory(path);    
        }

        string fullPath = $"{path}Save.json";   
        File.WriteAllText(fullPath, json);

    }



}
