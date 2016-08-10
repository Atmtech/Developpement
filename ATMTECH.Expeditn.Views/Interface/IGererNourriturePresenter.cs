﻿using System;
using System.Collections.Generic;
using ATMTECH.Expeditn.Entities;
using ATMTECH.Views.Interface;

namespace ATMTECH.Expeditn.Views.Interface
{
    public interface IGererNourriturePresenter : IViewBase
    {
        Expedition Expedition { set; }
        string IdExpedition { get;}
        string IdParticipantCuisinier { get; }
        string Nom { get; set; }
        string Menu { get; set; }
        DateTime Date { get; set; }
        IList<Participant> ListeParticipant { set; }
        IList<Nourriture> ListeNourriture { set; }
    }
}
