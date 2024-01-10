using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Projet_Formation
{

    public class Gestion_Compte
    {

        public Dictionary<uint, Compte> Banque = new Dictionary<uint, Compte>();
        public List<Transaction> Historique_Banque = new List<Transaction>();
        public Dictionary<uint, Gestionnaire> Clients = new Dictionary<uint, Gestionnaire>();
        private const int Max_Retrait = 1000;
        public int nb_compte = 0;
        public int nb_tr_ok = 0;
        public int nb_tr_ko = 0;

        public bool Existe_compte(uint num_cpt)
        {
            return Banque.ContainsKey(num_cpt);
        }
        public bool Existe_transaction(uint trid)
        {
            foreach (Transaction tr in Historique_Banque)
            {
                if (tr.Transaction_ID == trid)
                {
                    return true;
                }
            }
            return false;
        }
        public bool Existe_client(uint ID)
        {
            foreach (uint id in Clients.Keys)
            {
                if (id == ID)
                {
                    return true;
                }
            }
            return false;
        }
        public bool Create_client(uint num_cli, bool type)
        {
            if (!Existe_client(num_cli))
            {
                Gestionnaire cli = new Gestionnaire();
                cli.Type = type;
                cli.Comptes = new List<uint>();
                cli.Frai_banquaires = 0;
                Clients[num_cli] = cli;
                return true;
            }
            return false;
        }
        public bool Create_compte(uint num_cpt, DateTime date, decimal solde = 0)
        {
            if (!Existe_compte(num_cpt) && solde >= 0)
            {
                Compte cpt = new Compte();
                cpt.Solde = solde;
                cpt.Historique = new List<decimal>();
                cpt.Date_ouv = date;
                cpt.Actif = true;
                Banque[num_cpt] = cpt;
                return true;
            }
            return false;
        }
        public bool Add_compte_client(uint num_cli, uint num_cpt, DateTime date, decimal solde = 0)
        {
            if (Clients.ContainsKey(num_cli))
            {
                if (Create_compte(num_cpt, date, solde))
                {
                    Clients[num_cli].Comptes.Add(num_cpt);
                    return true;
                }
            }
            return false;

        }
        public bool Remove_compte_client(uint num_cli, uint num_cpt)
        {
            if (Existe_client(num_cli))
            {
                if (Clients[num_cli].Comptes.Any(x => x == num_cpt))
                {
                    Clients[num_cli].Comptes.Remove(num_cpt);
                    Compte cpt = Banque[num_cpt];
                    cpt.Actif = false;
                    Banque[num_cpt] = cpt;
                }
            }
            return false;
        }
        public bool Swap_compte_clients(uint num_expe, uint num_dest, uint num_cpt)
        {
            if (Existe_client(num_dest) && Existe_client(num_expe))
            {
                if (Clients[num_expe].Comptes.Any(x => x == num_cpt))
                {
                    Clients[num_expe].Comptes.Remove(num_cpt);
                    Clients[num_dest].Comptes.Add(num_cpt);
                    return true;
                }
            }
            return false;
        }
        public bool Depot(uint num_cpt, decimal montant)
        {
            if (montant > 0)
            {
                if (Existe_compte(num_cpt) && Banque[num_cpt].Actif)
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
            if (montant > 0)
            {
                if (Existe_compte(num_cpt) && Banque[num_cpt].Actif)
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
                for (int i = 1; i <= 9; i++)
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
        public void traitement_transaction(uint trid, decimal montant, uint expe, uint dest)
        {
            Transaction tr = new Transaction();
            tr.Transaction_ID = trid;
            tr.Montant = montant;
            tr.Expediteur = expe;
            tr.Destinataire = dest;
            tr.Statut = false;
            if (Existe_transaction(tr.Transaction_ID))
            {
                tr.Statut = false;
            }
            else if (tr.Expediteur == 0)
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
        /** Chargement_Gestionnaire
        public void Chargement_Gestionnaire(string gestionnaire)
        {
            string[] comptes = File.ReadAllLines(gestionnaire);
            foreach (string line in comptes)
            {
                try
                {
                    string[] elem = line.Replace('.', ',').Split(';');
                    uint num_cpt;
                    uint.TryParse(elem[0], out num_cpt);
                    decimal solde;
                    if (elem[1] == "")
                    {
                        solde = 0;
                    }
                    else if (!decimal.TryParse(elem[1], out solde) || solde < 0)
                    {
                        Console.WriteLine($"Compte {line} invalide !");
                        continue;
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
            return;
        }
        /**/
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
