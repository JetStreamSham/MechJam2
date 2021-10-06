using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlAI : MonoBehaviour
{

    public ParticleSystem system;
    public AudioSource radarSource;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            radarSource.Play();
            foreach (SniperAI sai in SniperAI.sniperList)
            {
                sai.agent.ResetPath();
                sai.state = SNIPER_STATE.ESCORT;
                sai.target = null;
            }
            system.Stop();
            system.Play();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            radarSource.Play();
            foreach (SniperAI sai in SniperAI.sniperList)
            {
                sai.agent.ResetPath();
                sai.state = SNIPER_STATE.COMBAT;
                sai.target = null;
            }
            system.Stop();
            system.Play();

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            radarSource.Play();
            foreach (SniperAI sai in SniperAI.sniperList)
            {
                sai.agent.ResetPath();
                sai.state = SNIPER_STATE.RETREAT;
                sai.target = null;
            }
            system.Stop();
            system.Play();

        }
    }
}
