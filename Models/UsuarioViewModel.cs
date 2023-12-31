namespace MyMvc.Models
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordString { get; set; }
        public byte[] Foto { get; set; }
        public string Perfil { get; set; }
        public string Email { get; set; }
        public string Token { get; set; } = string.Empty;
    
    }
}