using Bannerlord.TrainingTroops.handlers.interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.TrainingTroops.managers
{
	public class MenuHandlerImpl : IMenuHandler
	{

		public MenuHandlerImpl() { }

		public void switchToActiveTrainingMenu()
		{
			string trainingWaitMenuId = "town_arena_train_troops_wait";
			GameMenu.ActivateGameMenu(trainingWaitMenuId);
			GameMenu.SwitchToMenu(trainingWaitMenuId);
		}

		public void returnToPreviousMenu()
		{
			InformationManager.DisplayMessage(new InformationMessage(new TextObject("Voltando ao menu da cidade").ToString(), Colors.Red));
			GameMenu.SwitchToMenu("town_arena");
		}
	}
}
