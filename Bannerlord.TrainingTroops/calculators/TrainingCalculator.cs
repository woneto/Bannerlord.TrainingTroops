using Bannerlord.TrainingTroops.handlers.interfaces;
using Bannerlord.TrainingTroops.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.TrainingTroops.calculators
{
    public class TrainingCalculator
    {

        private IMessageHandler messageHandler;

        public TrainingCalculator(ref IMessageHandler messageHandler)
        {
            this.messageHandler = messageHandler;
        }

        public int calculateCostToTrainTroops(Settlement currentSettlement, MobileParty mobileParty)
        {
            int totalUnits = mobileParty.MemberRoster.TotalRegulars;
            int basePrice = totalUnits * 15;
            float additionalFee = 0.05f * (currentSettlement.Prosperity + currentSettlement.Militia);
            return (int)((float)basePrice + additionalFee);
        }

        private int getBaseHourlyExperience()
        {
            Settlement settlement = Settlement.CurrentSettlement;
            float settlementMilitiaExperienceBoost = settlement.Militia * 0.01f;
            return (int)(50 + settlementMilitiaExperienceBoost);
        }


        public void applyTrainingCosts(int trainingCost, Settlement currentSettlement, Hero giverHero)
        {
            GiveGoldAction.ApplyForCharacterToSettlement(giverHero, currentSettlement, trainingCost);
        }

        public void distributeTrainingExperienceToParty(MobileParty mobileParty)
        {
            int totalUnitsWoundedInPartyThisHour = 0;
            int totalExperienceGainedThisHour = 0;
            int totalTroopsTrainedThisHour = 0;
            int baseHourlyExperience = getBaseHourlyExperience();

            FlattenedTroopRoster flattenedMemberRoster = mobileParty.MemberRoster.ToFlattenedRoster();
            foreach (FlattenedTroopRosterElement member in flattenedMemberRoster)
            {
                bool isMemberWoundedKilledOrHero = member.Troop.IsHero || member.IsWounded || member.IsKilled;
                if (!isMemberWoundedKilledOrHero)
                {
                    totalTroopsTrainedThisHour++;
                    float experienceGainMultiplier = 1f;
                    int experienceGained = (int)((float)baseHourlyExperience * experienceGainMultiplier);
                    totalExperienceGainedThisHour += experienceGained;
                    mobileParty.MemberRoster.AddXpToTroop(experienceGained, member.Troop);
                    mobileParty.MemberRoster.AddToCounts(member.Troop, 0, false, 0, experienceGained, true, -1);
                }
            }
            if (totalUnitsWoundedInPartyThisHour > 0)
            {

                string singular = "{=sHYVaCy4}units were";
                string plural = "{=14hRl8yQ}unit was";
                Dictionary<string, object> dictionary = new Dictionary<string, object>
                {
                    { "TOTAL_UNITS_WOUNDED_HOUR", new TextObject(totalUnitsWoundedInPartyThisHour) },
                    { "UNIT_SINGULAR_OR_PLURAL", new TextObject(totalUnitsWoundedInPartyThisHour > 1 ? plural : singular) }
                };
                TextObject mainTextObject = new TextObject("{=r7WmdUKm}{TOTAL_UNITS_WOUNDED_HOUR} {UNIT_SINGULAR_OR_PLURAL} wounded during training.", dictionary);

                messageHandler.displayMessage(mainTextObject, MessageTypeEnum.INFORMATION);
            }
            if (totalExperienceGainedThisHour > 0)
            {

                string singular = "{=gYsdkvl1}troop";
                string plural = "{=BiYrTho7}troops";
                Dictionary<string, object> dictionary = new Dictionary<string, object>
                {
                    { "TOTAL_UNITS_TRAINED_HOUR", new TextObject(totalTroopsTrainedThisHour) },
                    { "TROOP_SINGULAR_OR_PLURAL", new TextObject(totalTroopsTrainedThisHour == 1 ? singular : plural) },
                    { "EXPERIENCE_GAINED_THIS_HOUR", new TextObject(totalExperienceGainedThisHour) }
                };
                TextObject mainTextObject = new TextObject("{=OBGlbT6C}{TOTAL_UNITS_TRAINED_HOUR} {TROOP_SINGULAR_OR_PLURAL} gained {EXPERIENCE_GAINED_THIS_HOUR} experience.", dictionary);

                messageHandler.displayMessage(mainTextObject, MessageTypeEnum.INFORMATION);
            }
        }
    }
}
