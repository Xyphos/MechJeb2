﻿using UnityEngine;
using KSP.Localization;
namespace MuMech
{
    public class OperationResonantOrbit : Operation
    {
        public override string getName() { return Localizer.Format("#MechJeb_resonant_title");}//resonant orbit

        [Persistent(pass = (int)Pass.Global)]
        public EditableInt resonanceNumerator = 2;
        [Persistent(pass = (int)Pass.Global)]
        public EditableInt resonanceDenominator = 3;
        private TimeSelector timeSelector;

        public OperationResonantOrbit ()
        {
            timeSelector = new TimeSelector(new TimeReference[]{ TimeReference.APOAPSIS, TimeReference.PERIAPSIS, TimeReference.X_FROM_NOW } );
        }

        public override void DoParametersGUI(Orbit o, double universalTime, MechJebModuleTargetController target)
        {
            GUILayout.Label(Localizer.Format("#MechJeb_resonant_label1_1") + resonanceNumerator.val + "/" + resonanceDenominator.val + Localizer.Format("#MechJeb_resonant_label1_2"));//"Change your orbital period to "" of your current orbital period"
            GUILayout.BeginHorizontal();
            GUILayout.Label(Localizer.Format("#MechJeb_resonant_label2"), GUILayout.ExpandWidth(true));//New orbital period ratio :
            resonanceNumerator.text = GUILayout.TextField(resonanceNumerator.text, GUILayout.Width(30));
            GUILayout.Label("/", GUILayout.ExpandWidth(false));
            resonanceDenominator.text = GUILayout.TextField(resonanceDenominator.text, GUILayout.Width(30));
            GUILayout.EndHorizontal();
            timeSelector.DoChooseTimeGUI();
        }

        public override ManeuverParameters MakeNodeImpl(Orbit o, double universalTime, MechJebModuleTargetController target)
        {
            double UT = timeSelector.ComputeManeuverTime(o, universalTime, target);
            var dV = OrbitalManeuverCalculator.DeltaVToResonantOrbit(o, UT, (double)resonanceNumerator.val / resonanceDenominator.val);

            return new ManeuverParameters(dV, UT);
        }

		public TimeSelector getTimeSelector() //Required for scripts to save configuration
		{
			return this.timeSelector;
		}
    }
}

