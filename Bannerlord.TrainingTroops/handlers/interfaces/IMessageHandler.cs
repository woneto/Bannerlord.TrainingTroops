using System;
using System.Collections.Generic;
using System.Text;
using TaleWorlds.Localization;

namespace Bannerlord.TrainingTroops.handlers.interfaces
{
    public interface IMessageHandler
    {

        public void displayMessage(TextObject message, MessageTypeEnum messageType);

        public void displayWarningMessage(TextObject message);
        public void displayInformationMessage(TextObject message);
    }
}
