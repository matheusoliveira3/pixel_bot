﻿using MiniBot.Domain;
using System.Drawing;
using System.Windows.Forms;

namespace MiniBot.Core
{
    public class Health
    {
        private static Color ColorHealthBarEmpty = ColorTranslator.FromHtml(Colors.HexHealthBarEmpty);

        public static void UsePotionOrWait()
        {
            var tibiaWindowFocused = TibiaClient.IsFocused();

            if (!tibiaWindowFocused)
                return;

            if (MustUsePotion())
                SendKeys.Send("{" + Configuration.Settings.LifeHotKey + "}");
        }

        private static bool MustUsePotion()
        {
            var healthColor = Pixel.GetColor(700, 28);

            if (ColorHealthBarEmpty != healthColor)
                return false;

            return true;
        }


    }
}
