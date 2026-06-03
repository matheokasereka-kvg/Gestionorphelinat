namespace GestionDecanat.Models
{
    public class Enseignant
    {
        private int idEnseignant; private string nom; private string postnom; private string prenom; private string telephone; private string specialite;
        public int IdEnseignant { get { return idEnseignant; } set { idEnseignant = value; } }
        public string Nom { get { return nom; } set { nom = value; } }
        public string Postnom { get { return postnom; } set { postnom = value; } }
        public string Prenom { get { return prenom; } set { prenom = value; } }
        public string Telephone { get { return telephone; } set { telephone = value; } }
        public string Specialite { get { return specialite; } set { specialite = value; } }
        public Enseignant() { }
    }
}
