﻿using System.Collections.Generic;
using ATMTECH.Entities;
using ATMTECH.Expeditn.Entities;
using ATMTECH.Expeditn.Entities.DTO;

namespace ATMTECH.Expeditn.Services.Interface
{
    public interface IExpediaService
    {
        RechercheForfaitExpedia ObtenirRechercheForfaitExpedia(int id);
        void ObtenirPrixRechercheForfaitExpedia();
        IList<RechercheForfaitExpedia> ObtenirRechercheForfaitExpedia(User utilisateur);
        IList<HistoriqueForfaitExpedia> ObtenirHistoriqueForfaitExpedia(RechercheForfaitExpedia rechercheForfaitExpedia);
        int EnregistrerRechercheForfaitExpedia(RechercheForfaitExpedia rechercheForfaitExpedia);
        IList<AffichageHistoriqueForfaitExpedia> ObtenirAffichageHistoriqueForfaitExpedia(int id);
        void SupprimerSuiviPrix(int idRechercheForfaitExpedia);
    }
}
