namespace GestionDecanat.Models
{
    public class AttributionCours
    {
        private int idAttribution; private int idCours; private int idEnseignant;
        public int IdAttribution { get { return idAttribution; } set { idAttribution = value; } }
        public int IdCours { get { return idCours; } set { idCours = value; } }
        public int IdEnseignant { get { return idEnseignant; } set { idEnseignant = value; } }
        public AttributionCours() { }
        public AttributionCours(int idCours, int idEnseignant) { IdCours = idCours; IdEnseignant = idEnseignant; }
    }
}
