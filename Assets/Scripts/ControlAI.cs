using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlAI : MonoBehaviour
{

    public ParticleSystem system;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            foreach (SniperAI sai in SniperAI.sniperList)
            {
                sai.state = SNIPER_STATE.ESCORT;
            }
            system.Stop();
            system.Play();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            foreach (SniperAI sai in SniperAI.sniperList)
            {
                sai.state = SNIPER_STATE.COMBAT;
            }
            system.Stop();
            system.Play();

        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            foreach (SniperAI sai in SniperAI.sniperList)
            {
                sai.state = SNIPER_STATE.RETREAT;
            }
            system.Stop();
            system.Play();

        }
    }
}
