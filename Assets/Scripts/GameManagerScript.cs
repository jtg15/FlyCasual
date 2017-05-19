﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour {

    //Move to board consts
    public readonly float PLAYMAT_SIZE = 10;

    public PrefabsList PrefabList;

    public UIManagerScript UI;
    public ShipSelectionManagerScript Selection;
    public ShipMovementScript Movement;
    public ShipPositionManager Position;

    void Start() {
        SetApplicationParameters();
        InitializeScripts();

        Phases.StartPhases();
    }

    private void SetApplicationParameters()
    {
        Application.targetFrameRate = 60;
    }

    private void InitializeScripts()
    {
        PrefabList = this.GetComponent<PrefabsList>();

        UI = this.GetComponent<UIManagerScript>();
            UI.ErrorManager = this.GetComponent<MessageManagerScript>();
            UI.DiceResults = this.GetComponent<DiceResultsScript>();
            UI.ActionsPanel = this.GetComponent<ActionsPanelScript>();
        
        Selection = this.GetComponent<ShipSelectionManagerScript>();
        Movement = this.GetComponent<ShipMovementScript>();
        
        Position = this.GetComponent<ShipPositionManager>();
    }

}
