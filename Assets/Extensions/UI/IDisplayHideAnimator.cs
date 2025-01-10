using UnityEngine;

namespace PierreMizzi.Useful.UI
{
	public interface IDisplayHideAnimator
	{
		public Animator animator { get; set; }

		public const string k_isDisplayed = "IsDisplayed";
		public const string k_quickTrigger = "QuickTrigger";


		public void Display()
		{
			if (animator == null)
			{
				return;
			}
			animator.SetBool(k_isDisplayed, true);
		}

		public void QuickDisplay()
		{
			if (animator == null)
			{
				return;
			}
			animator.SetTrigger(k_quickTrigger);
			animator.SetBool(k_isDisplayed, true);
		}

		public void Hide()
		{
			if (animator == null)
			{
				return;
			}
			animator.SetBool(k_isDisplayed, false);
		}

		public void QuickHide()
		{
			if (animator == null)
			{
				return;
			}
			animator.SetTrigger(k_quickTrigger);
			animator.SetBool(k_isDisplayed, false);
		}
	}
}