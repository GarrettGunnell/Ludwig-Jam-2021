﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {

    public OptionsManager optionsManager;

    private bool idle, talking;

    private int actionNumber = 0;

    void Start() {
        idle = true;
        talking = false;
    }

    void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool collided = Physics.Raycast(ray, out hit);

        if (Input.GetMouseButtonDown(0)) {
            if (idle) {
                if (collided && !talking) {
                    if (hit.transform.name == "Talk") {
                        setIdle(false);
                        talking = true;
                        actionNumber++;
                        optionsManager.Talk(actionNumber);
                    } else if (hit.transform.name == "Leave") {
                        optionsManager.Leave();
                    }
                } else if (collided && talking) {
                    if (hit.transform.name.Contains("Option")) {
                        int chosenOption = int.Parse(hit.transform.name.Split(' ')[1]);
                        optionsManager.Choose(chosenOption - 1);
                        setIdle(false);
                        talking = false;
                    }
                }
            }
        }
    }

    public void setIdle(bool state) {
        //Debug.Log("Setting idle to: " + state);
        idle = state;
    }

    public void endGame(string reason) {
        Debug.Log("Game Over!");
        Debug.Log("Reason: " + reason);
    }
}
