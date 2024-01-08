using System;
using System.Collections;
using UnityEngine;
using Gameplay.Items;

namespace Gameplay
{
	public class TwisterManager : MonoBehaviour {
		public static event Action OnLevelClear;
		public static event Action OnGameClear;
		public static event Action<TwisterButton> OnCorrectGuess;
		public static event Action<TwisterButton> OnWrongGuess;

        internal enum Players {
			player1,
			player2,
		}

		[SerializeField]
		internal TwisterSolutionDisplayer player1Displayer;
		[SerializeField]
		internal TwisterSolutionDisplayer player2Displayer;

		[SerializeField]
		internal TwisterLevel[] levels;

		internal static TwisterManager instance;

		int level;
		bool successFlag;

		private void Start() {
			StartGame();
		}

		public void StartGame() {
			instance = this;
			StartCoroutine(nameof(MainLoop));
		}

		IEnumerator MainLoop() {
			for (level = 0; level < levels.Length; level++)
			{
				levels[level].Spawn(this.transform.position);
				
				if(levels[level].guesser ==Players.player2) {
					player1Displayer.DisplaySolution(levels[level]);
				}
				else {
					player2Displayer.DisplaySolution(levels[level]);
				}
				while(!successFlag) {
					yield return new WaitForEndOfFrame();
				}
				successFlag = false;
				OnLevelClear?.Invoke();
			}
			OnGameClear?.Invoke();
		}

		internal bool TryGuess(TwisterButton button) {
			if(button == levels[level].goal) {
				successFlag = true;
				OnCorrectGuess?.Invoke(button);
				return true;
			}
			OnWrongGuess?.Invoke(button);
			return false;
		}

		// Should listen to OnCorrectGuess and OnWrongGuess
		internal void ActivateButton(TwisterButton button, bool isGoal)
		{
			button.ReactToClick(isGoal);
		}

	}
}