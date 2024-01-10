using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Formation
{
    public struct Gestionnaire
    {
        public bool Type { get; set; } // true = Particulier, False = Entreprise
        public List<uint> Comptes { get; set; }
        public decimal Frai_banquaires { get; set; }
        public int Code_ope { get; set; } // Code 1 : Création de compte
                                          // Code 2 : Cloture de compte
                                          // Code 3 : Cession d'un compte =   Réception de compte 
    }
    public struct Transaction
    {
        public uint Transaction_ID { get; set; }
        public decimal Montant { get; set; }
        public bool Statut { get; set; }
        public uint Expediteur { get; set; }
        public uint Destinataire { get; set; }
    }

    public struct Compte
    {
        public decimal Solde { get; set; }
        public List<decimal> Historique { get; set; }
        public DateTime Date_ouv { get; set; }
        public bool Actif { get; set; }
    }
}
