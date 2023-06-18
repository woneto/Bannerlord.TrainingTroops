using Bannerlord.TrainingTroops.handlers.interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.TrainingTroops.Handlers
{
    public class MessageHandlerImpl : IMessageHandler
    {
        public MessageHandlerImpl()
        {
        }

        public void displayMessage(TextObject message, MessageTypeEnum messageType)
        {

            switch(messageType)
            {
                case MessageTypeEnum.INFORMATION:
                    displayInformationMessage(message);
                    break;
                case MessageTypeEnum.WARNING:
                    displayWarningMessage(message);
                    break;
            }
            
        }

        public void displayWarningMessage(TextObject message)
        {
            InformationManager.DisplayMessage(new InformationMessage(message.ToString(), Colors.Red));
        }

        public void displayInformationMessage(TextObject message)
        {
            InformationManager.DisplayMessage(new InformationMessage(message.ToString()));
        }

    }
}
