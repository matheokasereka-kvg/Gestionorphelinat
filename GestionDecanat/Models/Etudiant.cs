using System;
namespace GestionDecanat.Models
{
    public class Etudiant
    {
        private int idEtudiant; private string matricule; private string nom; private string postnom; private string prenom; private string sexe; private DateTime dateNaissance; private string adresse; private string telephone;
        public int IdEtudiant { get { return idEtudiant; } set { idEtudiant = value; } }
        public string Matricule { get { return matricule; } set { matricule = value; } }
        public string Nom { get { return nom; } set { nom = value; } }
        public string Postnom { get { return postnom; } set { postnom = value; } }
        public string Prenom { get { return prenom; } set { prenom = value; } }
        public string Sexe { get { return sexe; } set { sexe = value; } }
        public DateTime DateNaissance { get { return dateNaissance; } set { dateNaissance = value; } }
        public string Adresse { get { return adresse; } set { adresse = value; } }
        public string Telephone { get { return telephone; } set { telephone = value; } }
        public Etudiant() { DateNaissance = DateTime.Today; }
    }
}
