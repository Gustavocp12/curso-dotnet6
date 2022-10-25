public record ProductRequest (
    string Code, string Name, string Descricao, int categoriaId, List<string> tags
    );
