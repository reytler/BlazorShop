namespace BlazorShop.Api.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nomeusuario { get; set; } = string.Empty;

        public Carrinho? Carrinho { get; set; }
    }
}
