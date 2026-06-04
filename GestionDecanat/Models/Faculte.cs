namespace GestionDecanat.Models
{
    public class Faculte
    {
        private int idFaculte; private string nomFaculte;
        public int IdFaculte { get { return idFaculte; } set { idFaculte = value; } }
        public string NomFaculte { get { return nomFaculte; } set { nomFaculte = value; } }
        public Faculte() { }
        public Faculte(string nomFaculte) { NomFaculte = nomFaculte; }
    }
}
