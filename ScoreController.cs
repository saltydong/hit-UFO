using UnityEngine;
using System.Collections;

public class ScoreController : MonoBehaviour
{
    private int oneDiskScore = 5;
    private int oneDiskFail = -2;

    private SceneController scene;

    void Awake()
    {
        scene = SceneController.getInstance();
        scene.setScoreController(this);
    }


    public void hitDisk(int round)
    {
        float level = 1;
        switch (round)
        {
            case 1:
                level = 1;
                break;
            case 2:
                level = 1.2f;
                break;
            case 3:
                level = 1.5f;
                break;
            case 4:
                level = 2f;
                break;
        }
        scene.setScore(scene.getScore() + (int)(level * oneDiskScore));
    }

    public void hitGround(int input)
    {
        scene.setScore(scene.getScore() + oneDiskFail * input * input);
    }
    public void wasteBullet()
    {
        scene.setScore(scene.getScore() - 2);
    }
}
