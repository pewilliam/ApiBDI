namespace Aplicacao.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public DateTime DtNascimento { get; set; }
        public string Cpf { get; set; }

        public Cliente(int id, int idUser, string nome, string email, DateTime dtNascimento, string cpf)
        {
            Id = id;
            IdUser = idUser;
            Nome = nome;
            Email = email;
            DtNascimento = dtNascimento;
            Cpf = cpf;
        }
    }
}
