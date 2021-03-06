﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandsTrigger : MonoBehaviour
{
    // Which door is it opening
    public TheCellGameMgr.CardinalPoint cardinal;

    private Renderer m_renderer;

    // True if an index tip is inside the cube, false otherwise.
    // First item is left hand, second item is right hand
    private bool[] m_isIndexStaying;

    private float m_goingOutStartTime = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        m_renderer = GetComponent<Renderer>();
        m_isIndexStaying = new bool[2] { false, false };
        m_goingOutStartTime = 0.0f;
    }


    // Update is called once per frame
    void Update()
    {

    }


    private void FixedUpdate()
    {
        if ((m_goingOutStartTime != 0.0f) && (Time.fixedTime - m_goingOutStartTime > 2.0f))
        {
            m_goingOutStartTime = 0.0f;

            switch (cardinal)
            {
                case TheCellGameMgr.CardinalPoint.North:
                    TheCellGameMgr.instance.MovePlayerNorth();
                    break;
                case TheCellGameMgr.CardinalPoint.East:
                    TheCellGameMgr.instance.MovePlayerEast();
                    break;
                case TheCellGameMgr.CardinalPoint.South:
                    TheCellGameMgr.instance.MovePlayerSouth();
                    break;
                case TheCellGameMgr.CardinalPoint.West:
                    TheCellGameMgr.instance.MovePlayerWest();
                    break;
            }
        }
    }


    private void OnTriggerEnter(Collider collider)
    {
        //get hand associated with trigger
        int handIdx = TheCellGameMgr.instance.GetFingerHandId(collider, OVRPlugin.BoneId.Hand_Index3);
        //m_goingOutStartTime = 0.0f;

        //if there is an associated hand, it means that an index of one of two hands is entering the cube
        //change the color of the cube accordingly (blue for left hand, green for right one)
        if (handIdx != -1)
        {
            //m_renderer.material.color = handIdx == 0 ? m_renderer.material.color = Color.blue : m_renderer.material.color = Color.green;
            m_isIndexStaying[handIdx] = true;

            if ((m_isIndexStaying[0] == true) && (m_isIndexStaying[1] == true))
            {
                m_renderer.material.SetColor("_TintColor", Color.red);
                if (m_goingOutStartTime == 0.0f)
                {
                    m_goingOutStartTime = Time.fixedTime;
                }
            }
            else if (m_isIndexStaying[0] == true)
            {
                m_renderer.material.SetColor("_TintColor", Color.blue);
                m_goingOutStartTime = 0.0f;
            }
            else if (m_isIndexStaying[1] == true)
            {
                m_renderer.material.SetColor("_TintColor", Color.green);
                m_goingOutStartTime = 0.0f;
            }
            else
            {
                m_renderer.material.SetColor("_TintColor", Color.cyan);
                m_goingOutStartTime = 0.0f;
            }
        }
    }


    private void OnTriggerExit(Collider collider)
    {
        //get hand associated with trigger
        int handIdx = TheCellGameMgr.instance.GetFingerHandId(collider, OVRPlugin.BoneId.Hand_Index3);
        //m_goingOutStartTime = 0.0f;

        //if there is an associated hand, it means that an index of one of two hands is levaing the cube,
        //so set the color of the cube back to white, or to the one of the other hand, if it is in
        if (handIdx != -1)
        {
            m_isIndexStaying[handIdx] = false;
            //m_renderer.material.color = m_isIndexStaying[0] ? m_renderer.material.color = Color.blue :
            //                          (m_isIndexStaying[1] ? m_renderer.material.color = Color.green : Color.white);
            if (m_isIndexStaying[0] == true)
            {
                m_renderer.material.SetColor("_TintColor", Color.blue);
            }
            else if (m_isIndexStaying[1] == true)
            {
                m_renderer.material.SetColor("_TintColor", Color.green);
            }
            else
            {
                m_renderer.material.SetColor("_TintColor", Color.cyan);
            }
        }
    }
}
