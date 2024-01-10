using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
            return Banque.ContainsKey(num_cpt);
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
            if (Existe_compte(expe) && Existe_compte(dest))
            {
                if (Retrait(expe, montant))
                {
                    return Depot(dest, montant); ;
                }
            }
            return false;
        }

        public void Gestion(string compte, string transac, string cr)
        {
            string[] comptes = File.ReadAllLines(compte);
            string[] transacs = File.ReadAllLines(transac);
            foreach (string line in comptes)
            {
                try
                {
                    uint num_cpt;
                    uint.TryParse(line.Split(';')[0], out num_cpt);
                    decimal solde;
                    if (!decimal.TryParse(line.Split(';')[1], out solde))
                    {
                        solde = 0;
                    }
                    Create_compte(num_cpt, solde);
                }
                catch (Exception)
                {
                    Console.WriteLine($"Compte : {line} invalide !");
                }
            }
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("Etat des comptes avant traitement :");
            Display_comptes();
            foreach (string line in transacs)
            {
                uint trid;
                decimal montant;
                uint expe;
                uint dest;
                Transaction tr = new Transaction();
                if (uint.TryParse(line.Split(';')[0], out trid) && decimal.TryParse(line.Split(';')[1], out montant) && uint.TryParse(line.Split(';')[2], out expe) && uint.TryParse(line.Split(';')[3], out dest))
                {
                    tr.Transaction_ID = trid;
                    tr.Montant = montant;
                    tr.Expediteur = expe;
                    tr.Destinataire = dest;
                    tr.Statut = false;
                }
                else
                {
                    Console.WriteLine($"Transaction : {line} invalide !");
                    continue;
                }
                if (tr.Expediteur == 0)
                {
                    tr.Statut = Depot(tr.Destinataire, tr.Montant);
                }
                else if (tr.Destinataire == 0)
                {
                    tr.Statut = Retrait(tr.Expediteur, tr.Montant);
                }
                else
                {
                    tr.Statut = Virement(tr.Montant, tr.Expediteur, tr.Destinataire);
                }
                Historique_Banque.Add(tr);
            }
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("Etat des comptes après traitement :");
            Display_comptes();
            Console.WriteLine("----------------------------------------------");
            Compte_rendu(cr);
            return;
        }

        public void Compte_rendu(string cr)
        {
            List<string> compte_rendu = new List<string>(); 
            foreach (Transaction tr in Historique_Banque)
            {
                string statut = tr.Statut ? "OK" : "KO";
                compte_rendu.Add($"{tr.Transaction_ID};{statut}");
            }
            File.WriteAllLines(cr, compte_rendu.ToArray());
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
