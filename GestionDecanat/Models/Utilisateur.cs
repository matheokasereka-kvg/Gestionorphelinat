namespace GestionDecanat.Models
{
    public class Utilisateur
    {
        private int idUser; private string username; private string password; private string role;
        public int IdUser { get { return idUser; } set { idUser = value; } }
        public string Username { get { return username; } set { username = value; } }
        public string Password { get { return password; } set { password = value; } }
        public string Role { get { return role; } set { role = value; } }
        public Utilisateur() { }
        public Utilisateur(string username, string password, string role) { Username = username; Password = password; Role = role; }
    }
}
