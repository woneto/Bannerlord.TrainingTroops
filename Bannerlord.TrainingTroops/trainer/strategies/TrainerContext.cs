using Bannerlord.TrainingTroops.calculators;
using Bannerlord.TrainingTroops.Trainer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bannerlord.TrainingTroops.strategies
{
    public class TrainerContext
    {

        public TrainerContext() { }
        private ITrainerStrategy? strategy;

        internal ITrainerStrategy? Strategy { get => strategy; set => strategy = value; }

        public void executeStrategy()
        {
            if(Strategy != null)
            {
                Strategy.onTrainingStarted();
            }
            
        }

        public void stopStrategyExecution()
        {
            if (Strategy != null)
            {

                Strategy.onTrainingConcluded();
            }
             
        }
    }
}
