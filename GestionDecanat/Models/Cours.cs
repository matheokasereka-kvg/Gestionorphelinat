namespace GestionDecanat.Models
{
    public class Cours
    {
        private int idCours; private string nomCours; private decimal coefficient; private int idFaculte; private int idPromotion;
        public int IdCours { get { return idCours; } set { idCours = value; } }
        public string NomCours { get { return nomCours; } set { nomCours = value; } }
        public decimal Coefficient { get { return coefficient; } set { coefficient = value; } }
        public int IdFaculte { get { return idFaculte; } set { idFaculte = value; } }
        public int IdPromotion { get { return idPromotion; } set { idPromotion = value; } }
        public Cours() { Coefficient = 1; }
    }
}
