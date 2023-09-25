namespace Aplicacao.Models
{
    public class Aluguel
    {
        public int Id { get; set; }
        public int IdFuncionario { get; set; }
        public int IdCliente { get; set; }
        public int IdModelo { get; set; }
        public int IdFormaPagto { get; set; }
        public DateTime DhAluguel { get; set; }
        public int PeriodoAluguel { get; set; }
        public double ValorAluguel { get; set; }
        public DateTime? DhRenovacao { get; set; }
        public int? PeriodoRenovacao { get; set; }
        public double ValorTotal { get; set; }
        public double? ValorRenovacao { get; set; }

        public Aluguel(int id, int idFuncionario, int idCliente, int idModelo, int idFormaPagto, DateTime dhAluguel, int periodoAluguel, double valorAluguel, DateTime? dhRenovacao, int? periodoRenovacao, double valorTotal, double? valorRenovacao)
        {
            Id = id;
            IdFuncionario = idFuncionario;
            IdCliente = idCliente;
            IdModelo = idModelo;
            IdFormaPagto = idFormaPagto;
            DhAluguel = dhAluguel;
            PeriodoAluguel = periodoAluguel;
            ValorAluguel = valorAluguel;
            DhRenovacao = dhRenovacao;
            PeriodoRenovacao = periodoRenovacao;
            ValorTotal = valorTotal;
            ValorRenovacao = valorRenovacao;
        }
    }
}
