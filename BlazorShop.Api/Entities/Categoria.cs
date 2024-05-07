using System.Collections.ObjectModel;

namespace BlazorShop.Api.Entities
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string IconCSS { get; set;} = string.Empty;

        public Collection<Produto> produtos { get; set; } = new Collection<Produto>();
    }
}
