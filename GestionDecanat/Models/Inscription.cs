namespace GestionDecanat.Models
{
    public class Inscription
    {
        private int idInscription; private int idEtudiant; private int idPromotion; private int idAnnee;
        public int IdInscription { get { return idInscription; } set { idInscription = value; } }
        public int IdEtudiant { get { return idEtudiant; } set { idEtudiant = value; } }
        public int IdPromotion { get { return idPromotion; } set { idPromotion = value; } }
        public int IdAnnee { get { return idAnnee; } set { idAnnee = value; } }
        public Inscription() { }
        public Inscription(int idEtudiant, int idPromotion, int idAnnee) { IdEtudiant = idEtudiant; IdPromotion = idPromotion; IdAnnee = idAnnee; }
    }
}
