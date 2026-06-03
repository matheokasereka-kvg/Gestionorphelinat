namespace GestionDecanat.Models
{
    public class Promotion
    {
        private int idPromotion; private string nomPromotion; private int idFaculte;
        public int IdPromotion { get { return idPromotion; } set { idPromotion = value; } }
        public string NomPromotion { get { return nomPromotion; } set { nomPromotion = value; } }
        public int IdFaculte { get { return idFaculte; } set { idFaculte = value; } }
        public Promotion() { }
        public Promotion(string nomPromotion, int idFaculte) { NomPromotion = nomPromotion; IdFaculte = idFaculte; }
    }
}
