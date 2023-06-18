using Bannerlord.TrainingTroops.calculators;
using Bannerlord.TrainingTroops.handlers.interfaces;
using Bannerlord.TrainingTroops.Handlers;
using Bannerlord.TrainingTroops.managers;
using Bannerlord.TrainingTroops.strategies;
using Bannerlord.TrainingTroops.tickers;
using HarmonyLib;
using StoryMode.GameComponents.CampaignBehaviors;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TrainingField;

namespace Bannerlord.TrainingTroops
{
    public class SubModule : MBSubModuleBase

    {
        public SubModule()
        {
            this.messageHandler = new MessageHandlerImpl();
            this.trainerContext = new TrainerContext();
            this.trainingCalculator = new TrainingCalculator(ref messageHandler);
            this.trainerManager = new TrainerManager();
            this.hourAdvanceTicker = new HourAdvanceTicker(ref trainerContext, ref trainingCalculator, ref trainerManager);
            this.menuHandler = new MenuHandlerImpl();
        }
        private static readonly string Namespace = typeof(SubModule).Namespace;

        private IMessageHandler messageHandler;

        private TrainerContext trainerContext;

        private TrainingCalculator trainingCalculator;

        private HourAdvanceTicker hourAdvanceTicker;

        private TrainerManager trainerManager;

        private IMenuHandler menuHandler;

        protected override void OnSubModuleLoad()
        {
            
            new Harmony("Bannerlord.TrainingTroops").PatchAll();
        }
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);
            if (game.GameType as Campaign != null)
            {
                CampaignGameStarter campaignGameStarter = (CampaignGameStarter)gameStarterObject;
                campaignGameStarter.AddBehavior(new TrainingTroopsCampaignBehavior(ref messageHandler, ref trainerContext, ref trainerManager, ref trainingCalculator, ref hourAdvanceTicker, ref menuHandler));
            }
        }

    }
}