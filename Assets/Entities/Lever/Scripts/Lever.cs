using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public GameObject m_NextLever = null;
    public bool m_IsEnabled = true;
    public bool m_ExplodePlatform;

    private void Start()
    {
        if (!m_IsEnabled)
        {
            Disable();
        }
    }

    /// <summary>
    /// Turn this lever off and activate the next one.
    /// </summary>
    public void ActivateNext()
    {
        if (!m_IsEnabled) return;

        Disable();
        RotateHandle();

        if (m_NextLever)
            m_NextLever.GetComponent<Lever>().Enable();

        if (m_ExplodePlatform)
            ExplodePlatform();
    }

    private void ExplodePlatform()
    {
        var plats = GameObject.FindGameObjectsWithTag("Explodable Platform");
        foreach (var plat in plats)
        {
            Destroy(plat);
            
        }

        GameObject.FindWithTag("Boss").GetComponent<Boss>().Die();
    }

    public void Disable()
    {
        GameObject light = gameObject.transform.Find("Light Effect").gameObject;
        GameObject hintEffect = gameObject.transform.Find("Hint Effect").gameObject;
        light.GetComponent<Light>().enabled = false;
        hintEffect.GetComponent<ParticleSystem>().Stop();
        m_IsEnabled = false;
    }

    public void Enable()
    {
        GameObject light = gameObject.transform.Find("Light Effect").gameObject;
        GameObject hintEffect = gameObject.transform.Find("Hint Effect").gameObject;
        light.GetComponent<Light>().enabled = true;
        hintEffect.GetComponent<ParticleSystem>().Play();
        m_IsEnabled = true;
    }

    public void RotateHandle()
    {
        GameObject handle = gameObject.transform.Find("Handle").gameObject;
        handle.transform.Rotate(Vector3.back * 45);
    }
}
