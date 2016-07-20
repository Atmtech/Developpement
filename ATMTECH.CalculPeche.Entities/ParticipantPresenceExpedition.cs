﻿using System;
using ATMTECH.Entities;

namespace ATMTECH.CalculPeche.Entities
{
    public class ParticipantPresenceExpedition : BaseEntity 
    {
       public Participant Participant { get; set; }
       public Expedition Expedition { get; set; }
       public DateTime Date { get; set; }
       public bool EstPresence { get; set; }
    }
}
