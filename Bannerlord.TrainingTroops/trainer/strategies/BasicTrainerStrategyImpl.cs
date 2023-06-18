using Bannerlord.TrainingTroops.calculators;
using Bannerlord.TrainingTroops.handlers.interfaces;
using Bannerlord.TrainingTroops.Handlers;
using Bannerlord.TrainingTroops.managers;
using Bannerlord.TrainingTroops.Trainer;
using System;
using System.Collections.Generic;
using System.Text;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.TrainingTroops.strategies
{
    class BasicTrainerStrategyImpl : ITrainerStrategy
    {
        private TrainingCalculator trainingCalculator;
        private TrainerManager trainerManager;
        private IMessageHandler messageHandler;
        private IMenuHandler menuHandler;
        public BasicTrainerStrategyImpl(ref TrainingCalculator trainingCalculator, ref TrainerManager trainerManager, ref IMessageHandler messageHandler, ref IMenuHandler menuHandler) {
            this.trainingCalculator = trainingCalculator;
            this.trainerManager = trainerManager;
            this.messageHandler = messageHandler;
            this.menuHandler = menuHandler;
        }
        public void onTrainingConcluded()
        {
            menuHandler.returnToPreviousMenu();
        }

        public void onTrainingStarted()
        {
            if (!trainerManager.CanTrain)
            {
                string singular = "{=TZEdLZli}hour";
                string plural = "{=ITp7VuBw}hours";
                Dictionary<string, object> dictionary = new Dictionary<string, object>
                {
                    { "TOTAL_HOURS_REMAINING", new TextObject(trainerManager.TrainingCooldownHoursRemaining) },
                    { "HOUR_SINGULAR_PLURAL", new TextObject(trainerManager.TrainingCooldownHoursRemaining > 1 ? plural : singular) }
                };
                TextObject mainTextObject = new TextObject("{=VPHuUQV9}You have already conducted training recently. Your troops need {TOTAL_HOURS_REMAINING} {HOUR_SINGULAR_PLURAL} to recover.", dictionary);

                messageHandler.displayMessage(mainTextObject, MessageTypeEnum.INFORMATION);
            }
            else
            {
                int trainingCost = trainingCalculator.calculateCostToTrainTroops(Settlement.CurrentSettlement, MobileParty.MainParty);
                if (MobileParty.MainParty.LeaderHero.Gold < trainingCost)
                {
                    TextObject mainTextObject = new TextObject("{=UtI8RoyG}You cannot afford to train your troops here.");

                    messageHandler.displayMessage(mainTextObject, MessageTypeEnum.INFORMATION);
                }
                else
                {
                    trainingCalculator.applyTrainingCosts(trainingCost, Settlement.CurrentSettlement, PartyBase.MainParty.LeaderHero);
                    menuHandler.switchToActiveTrainingMenu();
                    trainerManager.trainingStarted();

                }
            }
        }

        public void onTrainingStopped()
        {
            trainerManager.trainingConcluded();
            menuHandler.returnToPreviousMenu();
        }
    }
}
