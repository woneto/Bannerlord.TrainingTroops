using Bannerlord.TrainingTroops.calculators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bannerlord.TrainingTroops.Trainer
{
    interface ITrainerStrategy
    {

        public void onTrainingStarted();

        public void onTrainingStopped();

        public void onTrainingConcluded();
    }
}
