namespace Aplicacao.Models
{
    public class Modelo
    {
        public string Ano { get; set; }
        public string Motor { get; set; }
        public int TipoCambio { get; set; }
        public int IdMarca { get; set; }
        public int IdCategoria { get; set; }
        public decimal ValorHora { get; set; }
        public int IdCarro { get; set; }
        public string Combustivel { get; set; }
        public int Capacidade { get; set; }
        public int Portas { get; set; }
        public string Placa { get; set; }

        public Modelo(string ano, string motor, int tipoCambio, int idMarca, int idCategoria, decimal valorHora, int idCarro, string combustivel, int capacidade, int portas, string placa)
        {
            Ano = ano;
            Motor = motor;
            TipoCambio = tipoCambio;
            IdMarca = idMarca;
            IdCategoria = idCategoria;
            ValorHora = valorHora;
            IdCarro = idCarro;
            Combustivel = combustivel;
            Capacidade = capacidade;
            Portas = portas;
            Placa = placa;
        }
    }
}
