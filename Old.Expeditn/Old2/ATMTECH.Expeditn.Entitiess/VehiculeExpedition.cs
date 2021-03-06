﻿using System.CodeDom;
using ATMTECH.Entities;

namespace ATMTECH.Expeditn.Entities
{
    public class VehiculeExpedition : BaseEntity
    {
        public const string EXPEDITION = "Expedition";
        public const string UTILISATEUR = "Utilisateur";

        public Expedition Expedition { get; set; }
        public User Utilisateur { get; set; }
        public Vehicule Vehicule { get; set; }

        public string ComboboxDescriptionUpdate
        {
            get { return string.Format("{0} {2} ({1})", Expedition.Nom, Utilisateur.FirstNameLastName, Vehicule.Nom); }
        }

    }
}