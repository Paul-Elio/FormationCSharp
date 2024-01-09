using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Formation
{
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
    }

    public class Gestion_Compte
    {
        public Dictionary<uint, Compte> Banque = new Dictionary<uint, Compte>();
        public List<Transaction> Historique_Banque = new List<Transaction>();
        private const int Max_Retrait = 1000;

        public bool Existe_compte(uint num_cpt)
        {
            try
            {
                Compte cpt = Banque[num_cpt];
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public void Create_compte(uint num_cpt, decimal solde = 0)
        {
            if (!Existe_compte(num_cpt))
            {
                Compte cpt = new Compte();
                cpt.Solde = solde;
                cpt.Historique = new List<decimal>();
                Banque[num_cpt] = cpt;
            }
            return;
        }

        public bool Depot(uint num_cpt, decimal montant)
        {
            if (montant >= 0)
            {
                if (Existe_compte(num_cpt))
                {
                    Compte cpt = Banque[num_cpt];
                    cpt.Solde += montant;
                    Banque[num_cpt] = cpt;
                    return true;
                }
            }
            return false;
        }

        public bool Retrait(uint num_cpt, decimal montant)
        {
            if (montant >= 0)
            {
                if (Existe_compte(num_cpt))
                {
                    if (Banque[num_cpt].Solde >= montant && !Plafond_atteint(num_cpt, montant))
                    {
                        Compte cpt = Banque[num_cpt];
                        cpt.Solde -= montant;
                        cpt.Historique.Add(montant);
                        Banque[num_cpt] = cpt;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool Plafond_atteint(uint num_cpt, decimal montant)
        {
            decimal sum = montant;
            Compte cpt = Banque[num_cpt];
            int nb_tr = cpt.Historique.Count;
            if (nb_tr >= 9)
            {
                for (int i = 0; i < 9; i++)
                {
                    sum += Banque[num_cpt].Historique[nb_tr - i];
                }
            }
            else
            {
                foreach (decimal mont in Banque[num_cpt].Historique)
                {
                    sum += mont;
                }
            }
            return (sum > Max_Retrait);
        }

        public bool Virement(decimal montant, uint expe, uint dest)
        {
            return false;
        }
        public bool Prelevement(decimal montant, uint expe, uint dest)
        {
            return false;
        }
        public void Gestion(string compte, string transac, string cr)
        {
            return;
        }
        public void Display_comptes()
        {
            foreach (KeyValuePair<uint, Compte> compte in Banque)
            {
                Console.WriteLine($"Compte n° {compte.Key} :   Solde = {compte.Value.Solde}");
            }
            return;
        }
        
    }
}
