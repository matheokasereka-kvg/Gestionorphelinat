namespace GestionDecanat.Models
{
    public class AnneeAcademique
    {
        private int idAnnee; private string libelle; private bool estActive;
        public int IdAnnee { get { return idAnnee; } set { idAnnee = value; } }
        public string Libelle { get { return libelle; } set { libelle = value; } }
        public bool EstActive { get { return estActive; } set { estActive = value; } }
        public AnneeAcademique() { }
        public AnneeAcademique(string libelle, bool estActive) { Libelle = libelle; EstActive = estActive; }
    }
}
