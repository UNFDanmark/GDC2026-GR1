using UnityEngine;

public static class SoundPlayer
{
    public static void PlayRandomSound(AudioSource sigma, AudioClip[] phi)
    {
        if (phi.Length == 0) return;
        sigma.clip = phi[Random.Range(0, phi.Length)];
        sigma.Play();
    }
}