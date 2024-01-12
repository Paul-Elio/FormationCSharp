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
                    return true;
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
            foreach (uint cli in Clients.Keys)
            {
                if (Clients[cli].Comptes.Any(x => x == num_cpt))
                {
                    tr_max = Clients[cli].tr_max;
                }
            }
            decimal sum = montant;
            Compte cpt = Banque[num_cpt];
            int nb_tr = cpt.Historique.Count;
            if (nb_tr >= tr_max )
            {
                for (int i = 1; i <= tr_max ; i++)
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
            int i = cpt.Histo_transac.Count - 1;
            TimeSpan week = TimeSpan.FromDays(7);
            while (i >= 0 && date - cpt.Histo_transac[i] < week)
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
                    List<uint> list_client = new List<uint>();
                    foreach (uint id in Clients.Keys)
                    {
                        list_client.Add(id);
                    }
                    foreach (uint client_id in list_client)
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
                    return Depot(dest, montant - frai_banquaire);
                }
            }
            return false;
        }
        public void Traitement_transaction(ref Transaction tr)
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
                tr.Statut = Retrait(tr.Expediteur, tr.Montant, tr.Date);
            }
            else
            {
                tr.Statut = Virement(tr.Montant, tr.Expediteur, tr.Destinataire, tr.Date);
            }
            Historique_Banque.Add(tr);
            return;
        }
        public void Traitement_operation(ref Operation ope)
        {
            if (ope.Sortie == 0)
            {
                ope.Statut = Add_compte_client(ope.Entree, ope.Num_cpt, ope.Date_ope, ope.Solde_init);
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
                    if (elem[1] == "Particulier" || elem[1] == "Entreprise")
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
        public bool Valide_line_operation(string operat, ref Operation ope)
        {
            if (operat == null)
            {
                return false;
            }
            string[] operation = operat.Split(';');
            if (operation.Length != 5)
            {
                return false;
            }
            uint num_cpt;
            DateTime date_ope = new DateTime();
            decimal solde_init = 0;
            uint entree = 0;
            uint sortie = 0;
            if (!uint.TryParse(operation[0], out num_cpt) || !DateTime.TryParse(operation[1], out date_ope))
            {
                return false;
            }
            if (operation[2] == "")
            {
                solde_init = 0;
            }
            else if (!decimal.TryParse(operation[2], out solde_init))
            {
                return false;
            }
            if (!uint.TryParse(operation[3], out entree) & !uint.TryParse(operation[4], out sortie))
            {
                return false;
            }
            ope.Num_cpt = num_cpt;
            ope.Date_ope = date_ope;
            ope.Entree = entree;
            ope.Sortie = sortie;
            ope.Statut = false;
            ope.Solde_init = solde_init;
            return true;
        }
        public bool Valide_line_transaction(string tra, ref Transaction tr)
        {
            if (tra == null)
            {
                return false;
            }
            string[] transaction = tra.Split(';');
            if (transaction.Length != 5)
            {
                return false;
            }
            uint id;
            DateTime date = new DateTime();
            decimal montant;
            uint expe;
            uint dest = 0;
            if (!uint.TryParse(transaction[0], out id) | 
                !DateTime.TryParse(transaction[1], out date) |
                !decimal.TryParse(transaction[2], out montant))
            {
                return false;
            }
            if (!uint.TryParse(transaction[3], out expe) & !uint.TryParse(transaction[4], out dest))
            {
                return false;
            }
            tr.Date = date;
            tr.Destinataire = dest;
            tr.Expediteur = expe;
            tr.Montant = montant;
            tr.Transaction_ID = id;
            tr.Statut = false;
            return true;
        }
        public void Gestion(string gestionnaire, string transactions, string operations, string stat_ope, string stat_tr, string cr)
        {
            Chargement_Gestionnaire(gestionnaire);
            StreamReader transaction = new StreamReader(transactions);
            StreamReader operation = new StreamReader(operations);
            string ope = operation.ReadLine();
            string tr = transaction.ReadLine();
            do
            {
                Operation opera = new Operation();
                Transaction transac = new Transaction();
                while (!Valide_line_operation(ope, ref opera) && ope != null)
                {
                    Console.WriteLine($"Operation : {ope} invalide !");
                    ope = operation.ReadLine();
                }
                while (!Valide_line_transaction(tr, ref transac) && tr != null)
                {
                    Console.WriteLine($"Transaction : {tr} invalide !");
                    tr = transaction.ReadLine();
                }
                opera.Date_ope = ope == null ? DateTime.MaxValue : opera.Date_ope;
                transac.Date = tr == null ? DateTime.MaxValue : transac.Date;
                if (ope == null && tr == null) break;
                if (opera.Date_ope <= transac.Date)
                {
                    Traitement_operation(ref opera);
                    Display_ope(opera);
                    ope = operation.ReadLine();
                }
                else
                {
                    Traitement_transaction(ref transac);
                    Display_transac(transac);
                    tr = transaction.ReadLine();
                }
            } while (!(ope == null && tr == null));
            Compte_rendu_ope(stat_ope);
            Compte_rendu_transac(stat_tr);
            Compte_rendu(cr);
            Display_comptes();
            Display_gestionnaires();
            return;
        }
        public void Compte_rendu(string cr)
        {
            StreamWriter compte_rendu = new StreamWriter(cr);
            compte_rendu.WriteLine("Statistique :");
            compte_rendu.WriteLine("Nombre de comptes : {0}", Banque.Count);
            compte_rendu.WriteLine("Nombre de transactions : {0}", Historique_Banque.Count);
            compte_rendu.WriteLine("Nombre de réussites : {0}", Historique_Banque.Where(x => x.Statut).Count());
            compte_rendu.WriteLine("Nombre d'échecs : {0}", Historique_Banque.Where(x => !x.Statut).Count());
            decimal total = 0;
            foreach (Transaction tr in Historique_Banque.Where(x => x.Statut))
            {
                total += tr.Montant;
            }
            compte_rendu.WriteLine("Montant total des réussites : {0} euros", total.ToString("C2"));
            compte_rendu.WriteLine("\nFrais de gestions :");
            foreach (uint client in Clients.Keys)
            {
                compte_rendu.WriteLine($"{client} : {Clients[client].Frai_banquaires.ToString("C2")} euros");
            }
            compte_rendu.Close();
            return;
        }
        public void Compte_rendu_ope(string stat_ope)
        {
            StreamWriter operation = new StreamWriter(stat_ope);
            foreach (Operation ope in Historique_ope)
            {
                string statut = ope.Statut ? "OK" : "KO";
                operation.WriteLine($"{ope.Num_cpt};{statut}");
            }
            operation.Close();
            return;
        }
        public void Compte_rendu_transac(string stat_tr)
        {
            using (StreamWriter transaction = new StreamWriter(stat_tr))
            {
                foreach (Transaction tr in Historique_Banque)
                {
                    string statut = tr.Statut ? "OK" : "KO";
                    transaction.WriteLine($"{tr.Transaction_ID};{statut}");
                }
            }
            return;
        }
        public void Display_comptes()
        {
            string statut;
            foreach (KeyValuePair<uint, Compte> compte in Banque)
            {
                statut = compte.Value.Actif ? "Actif" : "Closed";
                Console.WriteLine($"Compte n° {compte.Key} :   Solde = {compte.Value.Solde} {statut}");
            }
            return;
        }
        public void Display_gestionnaires()
        {
            foreach (KeyValuePair<uint, Gestionnaire> client in Clients)
            {
                string type = client.Value.Type ? "Particulier" : "Entreprise";
                Console.Write($"{type} n° {client.Key}  Nombre max de transaction {client.Value.tr_max} comptes : ");
                foreach (uint compte in client.Value.Comptes)
                {
                    Console.Write($" {compte}");
                }
                Console.WriteLine("");
            }
            return;
        }
        public void Display_ope(Operation ope)
        {
            string statut = ope.Statut ? "OK" : "KO";
            Console.WriteLine($"Operation sur compte : {ope.Num_cpt}  Date : {ope.Date_ope}  Solde : {ope.Solde_init} Entree : {ope.Entree} Sortie : {ope.Sortie} Statut : {statut}");
        }
        public void Display_transac(Transaction tr)
        {
            string statut = tr.Statut ? "OK" : "KO";
            Console.WriteLine($"Transaction {tr.Transaction_ID}  date : {tr.Date} Montant : {tr.Montant} Expediteur : {tr.Expediteur} Destinatiare : {tr.Destinataire} Statut : {statut}");
        }
    }
}
