using System;
using System.Collections.Generic;
using Bannerlord.TrainingTroops;
using Bannerlord.TrainingTroops.calculators;
using Bannerlord.TrainingTroops.handlers.interfaces;
using Bannerlord.TrainingTroops.Handlers;
using Bannerlord.TrainingTroops.managers;
using Bannerlord.TrainingTroops.strategies;
using Bannerlord.TrainingTroops.tickers;
using Bannerlord.TrainingTroops.Trainer;
using StoryMode;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TrainingField
{
    public class TrainingTroopsCampaignBehavior : CampaignBehaviorBase
    {
        private IMessageHandler messageHandler;

        private TrainerContext trainerContext;

        private TrainingCalculator trainingCalculator;

        private HourAdvanceTicker hourAdvanceTicker;

        private TrainerManager trainerManager;

        private IMenuHandler menuHandler;

        public TrainingTroopsCampaignBehavior(ref IMessageHandler messageHandler, ref TrainerContext trainerContext,ref TrainerManager trainerManager, ref TrainingCalculator trainingCalculator, ref HourAdvanceTicker hourAdvanceTicker, ref IMenuHandler menuHandler)
        {
            this.trainerContext = trainerContext;
            this.trainingCalculator = trainingCalculator;
            this.messageHandler = messageHandler;
            this.trainerManager = trainerManager;
            this.hourAdvanceTicker = hourAdvanceTicker;
            this.menuHandler = menuHandler;
        }


        public override void RegisterEvents()
        {
            CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(hourAdvanceTicker.onHourAdvanced));
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
        }

        private void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
        {
            this.AddArenaTrainingMenus(campaignGameStarter);
        }

        private void AddArenaTrainingMenus(CampaignGameStarter campaignGameStarter)
        {
            TextObject townArenaTrainTroopsTextObject = new TextObject("{=Sk1PUgaq}Train your troops");
            TextObject townArenaTrainTroopsWaitingTextObject = new TextObject("{= X7FQwZph }Your troops train tirelessly throughout the day, increasing their combat effectiveness.");
            campaignGameStarter.AddGameMenuOption("town_arena", "town_arena_train_troops", "{SWORD_ICON}" + townArenaTrainTroopsTextObject.ToString() + " ({COST} {GOLD_ICON})", delegate (MenuCallbackArgs menuCallbackArgs)
            {
                MBTextManager.SetTextVariable("COST", trainingCalculator.calculateCostToTrainTroops(Settlement.CurrentSettlement, MobileParty.MainParty));
                menuCallbackArgs.optionLeaveType = GameMenuOption.LeaveType.Wait;
                return trainerManager.HasTroopsToTrain && !Settlement.CurrentSettlement.Town.HasTournament;
            }, delegate (MenuCallbackArgs menuCallbackArgs)
            {
                InformationManager.DisplayMessage(new InformationMessage(new TextObject("executar estrategia").ToString(), Colors.Red));
                trainerContext.Strategy = new BasicTrainerStrategyImpl(ref trainingCalculator, ref trainerManager, ref messageHandler, ref menuHandler);
                trainerContext.executeStrategy();
                
            }, false, 2, false);
            campaignGameStarter.AddWaitGameMenu("town_arena_train_troops_wait", townArenaTrainTroopsWaitingTextObject.ToString(), null, delegate (MenuCallbackArgs args)
            {
                args.MenuContext.GameMenu.SetTargetedWaitingTimeAndInitialProgress((float)trainerManager.MaximumNumberOfHoursToTrain, 0f);
                args.MenuContext.GameMenu.StartWait();
                return true;
            }, null, delegate (MenuCallbackArgs args, CampaignTime dt)
            {
                args.MenuContext.GameMenu.SetProgressOfWaitingInMenu(trainerManager.TrainingProgress);
            }, GameMenu.MenuAndOptionType.WaitMenuShowOnlyProgressOption, 0, 0f, 0, null);
        }

        

        public override void SyncData(IDataStore dataStore)
        {
            try
            {
                dataStore.SyncData<int>("_trainingCooldownHoursRemaining", ref trainerManager.getTrainingCooldownHoursRemainingReference());
            }
            catch
            {
            }
        }
    }
}
