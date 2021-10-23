﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public StateManager stateManager;
    public UI uiManager;

    public Text guyDialogue;
    public Text girlDialogue;

    public Animator girlAnimator;

    public Text option1;
    public Text option2;
    public Text option3;
    public Text option4;
    public Text option5;
    public Text option6;
    public Text option7;
    public Text option8;

    private List<Text> optionTextBoxes = null;
    private List<bool> optionBoxFinished;

    private bool populatingOptions = false;

    void Start() {
        optionTextBoxes = new List<Text>();
        optionTextBoxes.Add(option1);
        optionTextBoxes.Add(option2);
        optionTextBoxes.Add(option3);
        optionTextBoxes.Add(option4);
        optionTextBoxes.Add(option5);
        optionTextBoxes.Add(option6);
        optionTextBoxes.Add(option7);
        optionTextBoxes.Add(option8);

        optionBoxFinished = new List<bool>();

        guyDialogue.text = "";
        girlDialogue.text = "";
        EmptyOptions();
    }

    void Update() {
        if (populatingOptions) {
            if (!optionBoxFinished.Any(x => x == false)) {
                populatingOptions = false;
                stateManager.setIdle(true);
            }
        }
    }

    public void endGame(string guySentence, string girlSentence, string endReason) {
        StartCoroutine(badEnd(guySentence, girlSentence, endReason));
    }

    private IEnumerator badEnd(string guySentence, string girlSentence, string endReason) {
        guyDialogue.text = "";
        foreach (char letter in guySentence.ToCharArray()) {
            guyDialogue.text += letter;
            yield return new WaitForSecondsRealtime(0.05f);
        }

        yield return new WaitForSecondsRealtime(1.0f);
        girlDialogue.text = "";
        girlAnimator.SetBool("upset", true);
        foreach (char letter in girlSentence.ToCharArray()) {
            girlDialogue.text += letter;
            yield return new WaitForSecondsRealtime(0.05f);
        }

        girlAnimator.SetBool("upset", false);
        yield return new WaitForSecondsRealtime(1.0f);
        stateManager.endGame(endReason);
    }

    public void populateOptions(List<OptionsManager.Option> options) {
        uiManager.disableTalk();
        uiManager.disableLeave();
        uiManager.enableBoxes(options.Count);
        optionBoxFinished = Enumerable.Repeat(false, options.Count).ToList();

        for (int i = 0; i < options.Count; ++i) {
            Text box = optionTextBoxes[i];
            StartCoroutine(FillOption(box, options[i].option, i));
        }

        populatingOptions = true;
    }

    public void Converse(string guySentence, string girlSentence) {
        uiManager.disableBoxes(8);
        StartCoroutine(StartConversation(guySentence, girlSentence));
    }

    private void Reset() {
        stateManager.setIdle(true);
        guyDialogue.text = "";
        girlDialogue.text = "";
        EmptyOptions();
        uiManager.enableTalk();
        uiManager.enableLeave();
        uiManager.disableBoxes(8);
    }

    public void EmptyOptions() {
        foreach (Text box in optionTextBoxes) {
            box.text = "";
        }
    }

    IEnumerator StartConversation(string guySentence, string girlSentence) {
        guyDialogue.text = "";
        foreach (char letter in guySentence.ToCharArray()) {
            guyDialogue.text += letter;
            yield return new WaitForSecondsRealtime(0.05f);
        }

        yield return new WaitForSecondsRealtime(1.0f);

        girlDialogue.text = "";
        girlAnimator.SetBool("talking", true);
        foreach (char letter in girlSentence.ToCharArray()) {
            girlDialogue.text += letter;
            yield return new WaitForSecondsRealtime(0.05f);
        }

        girlAnimator.SetBool("talking", false);
        yield return new WaitForSecondsRealtime(1.0f);
        Reset();
    }

    IEnumerator FillOption(Text optionBox, string optionText, int optionNum) {
        optionBox.text = "";
        foreach (char letter in optionText.ToCharArray()) {
            optionBox.text += letter;
            yield return new WaitForSecondsRealtime(0.05f);
        }

        optionBoxFinished[optionNum] = true;
    }

    public void Leave() {
        uiManager.disableTalk();
        uiManager.disableLeave();
        StartCoroutine(LeaveConvo());
    }

    private IEnumerator LeaveConvo() {
        string yourSentence = "Alright, I think I'm going to leave.";
        string girlSentence = "Oh... okay.";

        guyDialogue.text = "";
        foreach (char letter in yourSentence.ToCharArray()) {
            guyDialogue.text += letter;
            yield return new WaitForSecondsRealtime(0.05f);
        }

        yield return new WaitForSecondsRealtime(1.0f);

        girlDialogue.text = "";
        girlAnimator.SetBool("talking", true);
        foreach (char letter in girlSentence.ToCharArray()) {
            girlDialogue.text += letter;
            yield return new WaitForSecondsRealtime(0.05f);
        }

        girlAnimator.SetBool("talking", false);
        yield return new WaitForSecondsRealtime(2.0f);
        Application.Quit();
    }

}
