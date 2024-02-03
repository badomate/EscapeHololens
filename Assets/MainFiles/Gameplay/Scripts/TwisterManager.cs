using System;
using System.Collections;
using UnityEngine;
using Gameplay.Items;

namespace Gameplay
{
	public class TwisterManager : MonoBehaviour {
		public static event Action OnLevelClear;
		public static event Action OnGameClear;

		public static event Action<TwisterButton, bool> OnCorrectGuess;
		public static event Action<TwisterButton, bool> OnWrongGuess;
		public static event Action<TwisterLevel> OnP1Display;
		public static event Action<TwisterLevel> OnP2Display;

        internal enum Players {
			player1,
			player2,
		}

		[SerializeField]
		internal TwisterSolutionDisplayer player1Displayer;
		[SerializeField]
		internal TwisterSolutionDisplayer player2Displayer;

		[SerializeReference]
		internal TwisterLevel[] levels;

		[SerializeField]
		internal GameObject InstanceTarget;

		internal static TwisterManager instance;

		int level;
		bool successFlag;

		private void Start() {
			StartGame();
		}

		public void StartGame() {
			instance = this;

			OnCorrectGuess += ActivateButton;
			OnWrongGuess += ActivateButton;
            
			StartCoroutine(nameof(MainLoop));
		}

		IEnumerator MainLoop() {
			for (level = 0; level < levels.Length; level++)
			{
                levels[level].Spawn(InstanceTarget.transform.position);
					
                if (levels[level].guesser ==Players.player2) {
					player1Displayer.DisplaySolution(levels[level]);
					OnP1Display.Invoke(levels[level]);
				}
				else {
					player2Displayer.DisplaySolution(levels[level]);
					OnP2Display.Invoke(levels[level]);
				}
				while(!successFlag) {
					yield return new WaitForEndOfFrame();
				}
				successFlag = false;
				OnLevelClear?.Invoke();
			}
			OnGameClear?.Invoke();
		}

		public bool TryGuess(TwisterButton button) {
            if (level < levels.Length && button.EqualsId(levels[level].goal.GetId())) {
                successFlag = true;
				OnCorrectGuess?.Invoke(button, true);
				return true;
			}
            OnWrongGuess?.Invoke(button, false);
			return false;
        }

		// Should listen to OnCorrectGuess and OnWrongGuess
		internal void ActivateButton(TwisterButton button, bool isGoal)
		{
			button.ReactToClick(isGoal);
		}

	}
}