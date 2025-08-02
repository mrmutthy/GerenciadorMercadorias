namespace GerenciadorMercadorias.Model
{
    public class Mercadoria
    {
        public int Id { get; set; }              // INTEGER
        public string Nome { get; set; } = "";   // TEXT (obrigatório)
        public string? Descricao { get; set; }   // TEXT (pode ser nulo)
        public decimal Preco { get; set; }       // REAL
        public int Estoque { get; set; }         // INTEGER
    }
}