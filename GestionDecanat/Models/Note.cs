using System;
namespace GestionDecanat.Models
{
    public class Note
    {
        private int idNote; private int idEtudiant; private int idCours; private decimal valeurNote; private DateTime dateAjout;
        public int IdNote { get { return idNote; } set { idNote = value; } }
        public int IdEtudiant { get { return idEtudiant; } set { idEtudiant = value; } }
        public int IdCours { get { return idCours; } set { idCours = value; } }
        public decimal ValeurNote { get { return valeurNote; } set { valeurNote = value; } }
        public DateTime DateAjout { get { return dateAjout; } set { dateAjout = value; } }
        public Note() { DateAjout = DateTime.Now; }
    }
}
