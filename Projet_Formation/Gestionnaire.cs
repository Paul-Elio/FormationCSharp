using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Formation
{
    public struct Operation
    {
        public uint Num_cpt { get; set; }
        public DateTime Date_ope { get; set; }
        public decimal Solde_init { get; set; }
        public uint Entree { get; set; }
        public uint Sortie { get; set; }
        public bool Statut { get; set; }
    }
    public struct Gestionnaire
    {
        public bool Type { get; set; } // true = Particulier, False = Entreprise
        public List<uint> Comptes { get; set; }
        public decimal Frai_banquaires { get; set; }
        public int tr_max { get; set; }
    }
    public struct Transaction
    {
        public uint Transaction_ID { get; set; }
        public decimal Montant { get; set; }
        public bool Statut { get; set; }
        public uint Expediteur { get; set; }
        public uint Destinataire { get; set; }
        public DateTime Date { get; set; }
    }

    public struct Compte
    {
        public decimal Solde { get; set; }
        public List<decimal> Historique { get; set; }
        public List<DateTime> Histo_transac { get; set; }
        public DateTime Date_ouv { get; set; }
        public bool Actif { get; set; }
    }
}
