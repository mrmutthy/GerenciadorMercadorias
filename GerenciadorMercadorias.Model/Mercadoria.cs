namespace GerenciadorMercadorias.Model
{
    public class Mercadoria
    {
        public int Id { get; set; }              
        public string Nome { get; set; } = "";   
        public string? Descricao { get; set; }   
        public decimal Preco { get; set; }       
        public int Estoque { get; set; }         
    }
}