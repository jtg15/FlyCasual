﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Todo: Move to different scripts by menu names

public delegate void DiceModification();

public class DiceResultsScript: MonoBehaviour {

    private GameManagerScript Game;

    public GameObject panelDiceResultsMenu;
    public GameObject prefabDiceModificationButton;

    void Start()
    {
        Game = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
    }

    public void ShowDiceResultMenu()
    {
        Game.UI.DiceResults.panelDiceResultsMenu.SetActive(true);        
    }

    public void ShowDiceModificationButtons()
    {
        //todo: rework
        Game.Selection.ActiveShip.GenerateDiceModificationButtons();

        //Debug.Log(Roster.GetPlayer(Game.Selection.ActiveShip.Owner.PlayerNo).Type);
        if (Roster.GetPlayer(Game.Selection.ActiveShip.Owner.PlayerNo).Type == Players.PlayerType.Human)
        {
            float offset = 0;
            Vector3 defaultPosition = panelDiceResultsMenu.transform.position + new Vector3(5, 195, 0);

            foreach (var actionEffect in Game.Selection.ActiveShip.AvailableActionEffects)
            {
                GameObject newButton = Instantiate(prefabDiceModificationButton, panelDiceResultsMenu.transform);
                newButton.name = "Button" + actionEffect.EffectName;
                newButton.transform.GetComponentInChildren<Text>().text = actionEffect.EffectName;
                newButton.GetComponent<RectTransform>().position = defaultPosition + new Vector3(0, -offset, 0);
                offset += 40;
                newButton.GetComponent<Button>().onClick.AddListener(delegate
                {
                    actionEffect.ActionEffect();
                    newButton.GetComponent<Button>().interactable = false;
                });
                newButton.GetComponent<Button>().interactable = true;
                newButton.SetActive(true);
            }

            panelDiceResultsMenu.transform.Find("Confirm").gameObject.SetActive(true);
        }

        Roster.GetPlayer(Game.Selection.ActiveShip.Owner.PlayerNo).UseDiceModifications();
    }

    public void HideDiceModificationButtons()
    {
        foreach (Transform button in panelDiceResultsMenu.transform)
        {
            if (button.name.StartsWith("Button"))
            {
                MonoBehaviour.Destroy(button.gameObject);
            }
        }
        panelDiceResultsMenu.transform.Find("Confirm").gameObject.SetActive(false);
    }

    public void ConfirmDiceResult()
    {
        HideDiceResultMenu();
        
        if (Combat.AttackStep == CombatStep.Attack)
        {
            Combat.PerformDefence(Game.Selection.ThisShip, Game.Selection.AnotherShip);
        }
        else if ((Combat.AttackStep == CombatStep.Defence))
        {
            //TODO: Show compare results dialog
            Combat.CalculateAttackResults(Game.Selection.ThisShip, Game.Selection.AnotherShip);

            MovementTemplates.ReturnRangeRuler();

            if (Roster.NoSamePlayerAndPilotSkillNotAttacked(Game.Selection.ThisShip))
            {
                Phases.CurrentSubPhase.NextSubPhase();
            }

        }
    }

    private void HideDiceResultMenu()
    {
        panelDiceResultsMenu.SetActive(false);
        HideDiceModificationButtons();
        Combat.CurentDiceRoll.RemoveDiceModels();
    }

}
