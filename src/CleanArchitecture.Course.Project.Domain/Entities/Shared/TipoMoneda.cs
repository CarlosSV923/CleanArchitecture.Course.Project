namespace CleanArchitecture.Course.Project.Domain.Entities.Shared
{
    public record TipoMoneda    
    {

        public static readonly TipoMoneda USD = new("USD");

        public static readonly TipoMoneda EUR = new("EUR");

        public static readonly TipoMoneda None = new("");

        private TipoMoneda(string codigo) {
            Codigo = codigo;
         }
        public string? Codigo { get; init; }

        public static TipoMoneda FromCodigo(string codigo) {
            return All.FirstOrDefault(x => x.Codigo == codigo) ?? None;
        }

        public static readonly IReadOnlyCollection<TipoMoneda> All = [USD, EUR];
    }
}