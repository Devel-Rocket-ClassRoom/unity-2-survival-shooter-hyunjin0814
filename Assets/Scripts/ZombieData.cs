using UnityEngine;

[CreateAssetMenu(fileName = "ZombieData", menuName = "Scriptable Objects/ZombieData")]
public class ZombieData : ScriptableObject
{
    public AudioClip deathClip;
    public AudioClip hurtClip;

    public float speed = 4f;
    public float hp = 100f;
}
