﻿using System;

namespace MiniBot.Core
{
    [Serializable]
    public class ConfigurationModel
    {
        public string LifeHotKey { get; set; }

        public string ManaHotKey { get; set; }

        public short UseLifeAtPercent { get; set; }

        public short UseManaAtPercent { get; set; }
    }
}