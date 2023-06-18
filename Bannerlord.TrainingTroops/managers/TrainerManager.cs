using System;
using System.Collections.Generic;
using System.Text;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.TrainingTroops.managers
{
    public class TrainerManager
    {

        public TrainerManager() { }

        private int _trainingCooldownHoursRemaining = 0;

        private bool _isTraining = false;

        private int _trainingHoursRemaining = 0;

        private int _totalUnitsWoundedInTraining = 0;

        private bool _startedTraining = false;

        public bool CanTrain
        {
            get
            {
                return this.TrainingCooldownHoursRemaining == 0;
            }
        }

        public float TrainingProgress
        {
            get
            {
                return 1f - (float)this.TrainingHoursRemaining / (float)1;
            }
        }

        public int MaximumNumberOfHoursToTrain
        {
            get
            {
                return 24;
            }
        }

        public int TotalUnitsWoundedInTraining
        {
            get
            {
                return this._totalUnitsWoundedInTraining;
            }
        }

        public bool HasTroopsToTrain
        {
            get
            {
                return MobileParty.MainParty.MemberRoster.TotalRegulars > 0;
            }
        }

        public int TrainingCooldownHoursRemaining { get => _trainingCooldownHoursRemaining; internal set => _trainingCooldownHoursRemaining = value; }
        public bool IsTraining { get => _isTraining; set => _isTraining = value; }
        public int TrainingHoursRemaining { get => _trainingHoursRemaining; set => _trainingHoursRemaining = value; }
        public bool StartedTraining { get => _startedTraining; set => _startedTraining = value; }

        public ref int getTrainingCooldownHoursRemainingReference()
        {
            return ref _trainingCooldownHoursRemaining;
        }

        public ref bool getIsTraining()
        {
            return ref _isTraining;
        }

        public ref bool getStartedTraining()
        {
            return ref _startedTraining;
        }

        public void trainingStarted()
        {
            IsTraining = true;
            _totalUnitsWoundedInTraining = 0;
            TrainingHoursRemaining = 24;
            TrainingCooldownHoursRemaining = 72;
            StartedTraining = true;
        }

        public void trainingConcluded()
        {
            IsTraining = false;
            StartedTraining = false;
            InformationManager.DisplayMessage(new InformationMessage(new TextObject("Treinamento concluido (TrainerManager)").ToString(), Colors.Red));
        }
    }
}
