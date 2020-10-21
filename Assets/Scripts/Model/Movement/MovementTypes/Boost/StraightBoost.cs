﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Movement
{

    public class StraightBoost : StraightMovement
    {
        public StraightBoost(int speed, ManeuverDirection direction, ManeuverBearing bearing, MovementComplexity color) : base(speed, direction, bearing, color)
        {

        }

        public override IEnumerator Perform()
        {
            ProgressTarget = SetProgressTarget();
            AnimationSpeed = Options.ManeuverSpeed * SetAnimationSpeed();

            Initialize();

            //Temporary
            GameManagerScript Game = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
            Game.Movement.FuncsToUpdate.Add(UpdateBoost);

            yield return true;
        }

        private bool UpdateBoost()
        {
            UpdateMovementExecution();
            return false;
        }

        protected override void FinishMovement()
        {
            //TEMP
            GameManagerScript Game = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
            Game.Movement.FuncsToUpdate.Remove(UpdateBoost);

            MovementTemplates.HideLastMovementRuler();
            TheShip.ResetRotationHelpers();

            // Important! Fixes final position according to prediction - otherwise animation can cause another final position
            TheShip.SetPositionInfo(FinalPositionInfo);

            (Phases.CurrentSubPhase as SubPhases.BoostExecutionSubPhase).FinishBoost();
        }

    }

}

