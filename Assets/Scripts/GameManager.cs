using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {
    public GameObject preDisk;
    public GameObject preTower;
    public Text txtTurns, txtSpeed, txtDisks;
    public double diskMaxWidth, diskMinWidth, diskMaxHeight, diskMinHeight;
    public static int diskNumber = 7;
    public Material mWhite;//
    public Material mBlack;//
    public static float speed = 15;

    public static float diskHeight;
    int add = 0;
    bool running = false;
    bool done = false;
    int movTower = 1;
    int posSmall = 0;
    bool isSmall = true;
    int moveCount = 0;
    bool moving = false;
    Tower[] towers = new Tower[3];
    
    //
    //
    //
    // KeyHandler

    // Activates/Deactivates Looping (deactivation occurs after current turn ends)
    public void StartStop()
    {
        running = !running;
    }

    // Decreases the speed variable affecting the move speed of the disks in increments of 2.5 (Limit: 5)
    public void Slower()
    {
        if (Input.GetKey(KeyCode.LeftShift)) speed /= 2;
        if (speed % 5 > 0) speed -= 2.5f;
        else speed -= 5;
        if (speed < 5) speed = 5;
        txtSpeed.text = "Speed: " + speed / 5;
    }

    // Increases the speed variable affecting the move speed of the disks in increments of 2.5 (no Limit)
    public void Faster()
    {
        if (Input.GetKey(KeyCode.LeftShift)) speed *= 2;
        else speed += 5;
        txtSpeed.text = "Speed: " + speed / 5;
    }

    // 
    public void AddDisk()
    {
        add += 1;
        done = true;
        running = true;
    }
    public void RmvDisk()
    {
        add -= 1;
        done = true;
        running = true;
    }
    // Use this for initialization
    void Start()
    {
        int off = -5;
        //setup towers once at x = -5, x = 0 and x = 5
        for (int i = 0; i < 3; i++)
        {
            txtSpeed.text = "Speed: " + speed/5;
            txtDisks.text = "Disks: " + diskNumber;
            txtTurns.text = "Turns: " + 0;
            GameObject tower = Instantiate(preTower);
            Transform t = tower.GetComponent<Transform>();
            tower.transform.localPosition = new Vector3(off, tower.transform.localPosition.y, tower.transform.localPosition.z);
            towers[i] = new Tower(tower);
            towers[i].mBlack = mBlack;
            towers[i].mWhite = mWhite;
            off += 5;
        }
        resetDisks();
    }

    // Update is called once per frame
    void Update() {
        if (moving)
        {
            moving = towers[movTower].moveDisk();
        }
        else if (running)
        {
            if (done)
            {
                
                done = false;
                running = false;
                diskNumber += add;
                add = 0;
                if (diskNumber < 3) diskNumber = 3; 
                txtDisks.text = "Disks: " + diskNumber;
                resetDisks();
            }
            else if (towers[2].isFull())
            {
                done = true;
                running = false;
            }
            else
            {
                if (isSmall)
                {
                    int nposSmall;
                    if (diskNumber % 2 == 0) //if even number of disks move smallest right
                    {
                        if (posSmall < 2) nposSmall = posSmall + 1;
                        else nposSmall = 0;
                    }
                    else
                    {
                        if (posSmall > 0) nposSmall = posSmall - 1; //if odd number of disks move smallest left
                        else nposSmall = 2;
                    }
                    towers[nposSmall].addDisk(towers[posSmall].rmvDisk());
                    posSmall = nposSmall;
                    isSmall = false;
                    movTower = nposSmall;
                }
                else
                {
                    if (posSmall == 0)
                    {
                        if (towers[1].getDiskTr() != null && (towers[2].getDiskTr() == null || towers[1].getDiskTr().localScale.x < towers[2].getDiskTr().localScale.x))
                        {
                            towers[2].addDisk(towers[1].rmvDisk());
                            movTower = 2;
                        }
                        else
                        {
                            towers[1].addDisk(towers[2].rmvDisk());
                            movTower = 1;
                        }
                    }
                    if (posSmall == 1)
                    {
                        if (towers[0].getDiskTr() != null && (towers[2].getDiskTr() == null || towers[0].getDiskTr().localScale.x < towers[2].getDiskTr().localScale.x))
                        {
                            towers[2].addDisk(towers[0].rmvDisk());
                            movTower = 2;
                        }
                        else
                        {
                            towers[0].addDisk(towers[2].rmvDisk());
                            movTower = 0;
                        }
                    }
                    if (posSmall == 2)
                    {
                        if (towers[0].getDiskTr() != null && (towers[1].getDiskTr() == null || towers[0].getDiskTr().localScale.x < towers[1].getDiskTr().localScale.x))
                        {
                            towers[1].addDisk(towers[0].rmvDisk());
                            movTower = 1;
                        }
                        else
                        {
                            towers[0].addDisk(towers[1].rmvDisk());
                            movTower = 0;
                        }
                    }

                    isSmall = true;
                }
                moving = true;
                moveCount++;
                txtTurns.text = "Turns: " + moveCount;
            }
        }
    }
    void resetDisks()
    {
        /*if (disks != null)
        {
            for (int i = 0; i < disks.Length; i++)
            {
                Destroy(disks[i]);
            }
        }
        disks = new GameObject[diskNumber];
        float off = 0;
        float increment = (float)(diskMaxHeight * preDisk.transform.localScale.y / towers[0].transform.localScale.y);
        
        for (int i = 0; i < diskNumber; i++)
        {
            disks[i] = Instantiate(preDisk, towers[0].transform);
            Transform t = disks[i].GetComponent<Transform>();
            t.localScale = new Vector3(t.localScale.x / towers[0].transform.localScale.x, t.localScale.y / towers[0].transform.localScale.y, t.localScale.z / towers[0].transform.localScale.z);
            t.localPosition = new Vector3(0, -0.875f + off, 0);
            off += increment;
            //Transform t = disk.GetComponent<Transform>();
            //Vector3 pos= t.localPosition;
        }*/
        towers[0].reset();
        towers[1].reset();
        towers[2].reset();

        towers[0].createDisks(diskNumber,preDisk);
        isSmall = true;
        posSmall = 0;
        moveCount = 0;
    }


    class Tower
    {
        public GameObject tower;
        GameObject[] disks;
        float yTowerBottom;
        Vector3 pos;
        public Material mBlack, mWhite;
        public Tower(GameObject tower)
        {
            this.tower = tower;
            Transform t = tower.GetComponent<Transform>();
            yTowerBottom = t.position.y - t.lossyScale.y;
        }

        public void reset()
        {
            if (disks != null) {
            foreach (GameObject d in disks) Destroy(d);
            disks = null;
            }
        }

        public void createDisks(int nr, GameObject preDisk)
        {
            disks = new GameObject[nr];
            bool bBlack = true;
            for (int i = 0; i < nr; i++)
            {
                GameObject disk = Instantiate(preDisk);
                float sizeOffset = ((float)i / (float)nr) * 4f;
                //(tower.transform.localScale.y/nr) * disk.transform.localScale.y * disk.transform.localScale.y

                //+ 2 * (i - 1.5f) * disk.transform.lossyScale.y
                GameManager.diskHeight = (tower.transform.localScale.y / nr);
                disk.transform.localScale = new Vector3(disk.transform.localScale.x - sizeOffset, GameManager.diskHeight, disk.transform.localScale.z - sizeOffset);
                disk.transform.localPosition = new Vector3(tower.transform.localPosition.x, yTowerBottom + (1 + 2*i) * GameManager.diskHeight, disk.transform.localPosition.z);

                MeshRenderer mr = disk.GetComponent<MeshRenderer>();//
                if (bBlack) mr.material = mBlack;//
                else mr.material = mWhite;//

                disks[i] = disk;
                bBlack = !bBlack;
            }   
        }

        public void addDisk(GameObject ndisk)
        {
            if (disks == null) {
                disks = new GameObject[1];
                disks[0] = ndisk;
            }
            else
            {
                GameObject[] help = new GameObject[disks.Length + 1];
                for (int i = 0; i < disks.Length; i++) help[i] = disks[i];
                help[disks.Length] = ndisk;
                disks = help;
            }
            pos = new Vector3(tower.transform.position.x, yTowerBottom + (1 + 2 * (disks.Length-1)) * GameManager.diskHeight, 0);
        }

        public bool moveDisk()
        {

            GameObject lDisk = disks[disks.Length - 1];
                if (!(lDisk.transform.position.x == tower.transform.position.x))
            {
                float step = GameManager.speed * Time.deltaTime;
                lDisk.transform.localPosition = Vector3.MoveTowards(lDisk.transform.localPosition, pos, step);
                return true;
            }
            else return false;
        }

        public GameObject rmvDisk()
        {
            if (disks != null && disks.Length > 0)
            { 
                GameObject d = disks[disks.Length - 1];
                GameObject[] help = new GameObject[disks.Length - 1];
                for (int i = 0; i < disks.Length - 1; i++)
                {
                    help[i] = disks[i];
                }
                disks = help;
                return d;
            }
            return null;
        }

        public Transform getDiskTr()
        {
            if (disks != null && disks.Length>0) return disks[disks.Length-1].transform;
            return null;
        }
        public bool isFull()
        {
            if (disks == null || disks.Length == 0) return false;
            return (disks.Length == GameManager.diskNumber);
        }
        
    }
}
