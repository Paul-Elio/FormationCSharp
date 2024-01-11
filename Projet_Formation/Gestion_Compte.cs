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
        public List<Operation> Historique_ope = new List<Operation>();
        public Dictionary<uint, Compte> Banque = new Dictionary<uint, Compte>();
        public List<Transaction> Historique_Banque = new List<Transaction>();
        public Dictionary<uint, Gestionnaire> Clients = new Dictionary<uint, Gestionnaire>();
        private const int Max_Retrait = 1000;
        private const int Max_week = 2000;
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
        public bool Create_client(uint num_cli, bool type, int tr_max)
        {
            if (!Existe_client(num_cli))
            {
                Gestionnaire cli = new Gestionnaire();
                cli.Type = type;
                cli.Comptes = new List<uint>();
                cli.Frai_banquaires = 0;
                cli.tr_max = tr_max;
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
                cpt.Histo_transac = new List<DateTime>();
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
        public bool Retrait(uint num_cpt, decimal montant, DateTime date)
        {
            if (montant > 0)
            {
                if (Existe_compte(num_cpt) && Banque[num_cpt].Actif)
                {
                    if (Banque[num_cpt].Solde >= montant && !Plafond1_atteint(num_cpt, montant) && !Plafond2_atteint(num_cpt, montant, date))
                    {
                        Compte cpt = Banque[num_cpt];
                        cpt.Solde -= montant;
                        cpt.Historique.Add(montant);
                        cpt.Histo_transac.Add(date);
                        Banque[num_cpt] = cpt;
                        return true;
                    }
                }
            }
            return false;
        }
        public bool Plafond1_atteint(uint num_cpt, decimal montant)
        {
            int tr_max = 0;
            foreach(uint cli in Clients.Keys)
            {
                if (Clients[cli].Comptes.Any(x => x == num_cpt))
                {
                    tr_max = Clients[cli].tr_max;
                }
            }
            decimal sum = montant;
            Compte cpt = Banque[num_cpt];
            int nb_tr = cpt.Historique.Count;
            if (nb_tr >= tr_max-1)
            {
                for (int i = 1; i <= tr_max; i++)
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
        public bool Plafond2_atteint(uint num_cpt, decimal montant, DateTime date)
        {
            decimal sum = montant;
            Compte cpt = Banque[num_cpt];
            int i= cpt.Histo_transac.Count-1;
            TimeSpan week = TimeSpan.FromDays(7);
            while (date - cpt.Histo_transac[i] <= week)
            {
                sum += cpt.Historique[i];
                i--;
            }
            return (sum > Max_week);
        }
        public bool Virement(decimal montant, uint expe, uint dest, DateTime date)
        {
            if (Existe_compte(expe) && Existe_compte(dest))
            {
                if (Retrait(expe, montant, date))
                {
                    decimal frai_banquaire = 0;
                    foreach (uint client_id in Clients.Keys)
                    {
                        if (Clients[client_id].Comptes.Any(x => x == expe))
                        {
                            // si le compte destinataire est chez le même client les frai banquaires sont de 0;
                            // sinon on regarde son type est si c'est true c'est un Particulier et sinon c'est une entreprise;
                            frai_banquaire = Clients[client_id].Comptes.Any(x => x == dest) ? 0 : Clients[client_id].Type ? (decimal)0.01 * montant : 10;
                            Gestionnaire cli = Clients[client_id];
                            cli.Frai_banquaires += frai_banquaire;
                            Clients[client_id] = cli;
                        }
                    }
                    return Depot(dest, montant-frai_banquaire); 
                }
            }
            return false;
        }
        public void Traitement_transaction(Transaction tr)
        {
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
                tr.Statut = Retrait(tr.Expediteur, tr.Montant, tr.Date );
            }
            else
            {
                tr.Statut = Virement(tr.Montant, tr.Expediteur, tr.Destinataire, tr.Date);
            }
            Historique_Banque.Add(tr);
            return;
        }
        public void Traitement_operation(Operation ope)
        {
            if (ope.Sortie == 0)
            {
                ope.Statut =Add_compte_client(ope.Entree, ope.Num_cpt, ope.Date_ope, ope.Solde_init);
            }
            else if (ope.Entree == 0)
            {
                ope.Statut = Remove_compte_client(ope.Sortie, ope.Num_cpt);
            }
            else
            {
                ope.Statut = Swap_compte_clients(ope.Entree, ope.Sortie, ope.Num_cpt);
            }
            Historique_ope.Add(ope);
            return;
        }
        public void Chargement_Gestionnaire(string gestionnaire)
        {
            string[] gestion = File.ReadAllLines(gestionnaire);
            foreach (string line in gestion)
            {
                try
                {
                    string[] elem = line.Split(';');
                    uint gest_id;
                    uint.TryParse(elem[0], out gest_id);
                    int tr_max;
                    int.TryParse(elem[2], out tr_max);
                    bool type = true;
                    if (elem[1]== "Particulier" || elem[1] == "Entreprise")
                    {
                        type = elem[1] == "Particulier";
                    }
                   else
                    {
                        Console.WriteLine($"Compte : {line} invalide !");
                        continue;
                    }

                    Create_client(gest_id, type, tr_max);
                }
                catch (Exception)
                {
                    Console.WriteLine($"Compte : {line} invalide !");
                }
            }
            return;
        }
        public bool Valide_line_operation(string[] operation, ref Operation ope)
        {
            return false;
        }
        public bool Valide_line_transaction(string[] transaction, ref Transaction tr)
        {
            return false;
        }
        public void Gestion(string gestionnaire, string transactions, string operations, string stat_ope, string stat_tr, string cr)
        {
            Chargement_Gestionnaire(gestionnaire);
            StreamReader transaction = new StreamReader(transactions);
            StreamReader operation = new StreamReader(operations);
            string[] ope = operation.ReadLine().Split(';');
            string[] tr = transaction.ReadLine().Split(';');
            do
            {
                Operation opera = new Operation();
                Transaction transac = new Transaction();
                while (!Valide_line_operation(ope, ref opera) && ope != null) ope = operation.ReadLine().Split(';');
                while (!Valide_line_transaction(tr, ref transac) && tr != null) tr = transaction.ReadLine().Split(';');
                if (opera.Date_ope <= transac.Date)
                {
                    Traitement_operation(opera);
                    ope = operation.ReadLine().Split(';');
                }
                else
                {
                    Traitement_transaction(transac);
                    tr = transaction.ReadLine().Split(';');
                }
            } while (ope != null && tr != null );
            Compte_rendu_ope(stat_ope);
            Compte_rendu_transac(stat_tr);
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
        public void Compte_rendu_ope(string stat_ope)
        {

        }
        public void Compte_rendu_transac(string stat_tr)
        {

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
