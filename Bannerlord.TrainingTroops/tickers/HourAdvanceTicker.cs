using Bannerlord.TrainingTroops.calculators;
using Bannerlord.TrainingTroops.managers;
using Bannerlord.TrainingTroops.strategies;
using System;
using System.Collections.Generic;
using System.Text;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.TrainingTroops.tickers
{
    public class HourAdvanceTicker
    {
        private TrainerContext trainerContext;
        private TrainingCalculator trainingCalculator;
        private TrainerManager trainingManager;

        public HourAdvanceTicker(ref TrainerContext trainerContext, ref TrainingCalculator trainingCalculator, ref TrainerManager trainingManager) {
            this.trainerContext = trainerContext;
            this.trainingCalculator = trainingCalculator;
            this.trainingManager = trainingManager;
            }

        public void onHourAdvanced()
        {
            if (!trainingManager.IsTraining && trainingManager.TrainingCooldownHoursRemaining == 0) return;

            if (!trainingManager.IsTraining)
            {
                trainingManager.TrainingCooldownHoursRemaining = Math.Max(trainingManager.TrainingCooldownHoursRemaining - 1, 0);
            }
            else if (trainingManager.StartedTraining)
            {
                trainingCalculator.distributeTrainingExperienceToParty(MobileParty.MainParty);
                trainingManager.TrainingHoursRemaining = Math.Max(trainingManager.TrainingHoursRemaining - 1, 0);
                if (trainingManager.TrainingHoursRemaining == 0)
                {
                    trainingManager.IsTraining = false;
                    trainingManager.StartedTraining = false;
                    trainerContext.stopStrategyExecution();
                }
            }
        }
    }
}
