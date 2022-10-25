public class Product{

public int Id {get; set;}

public string Descricao {get; set;}

public string Code {get; set;}

public string Name {get; set;}

public int categoriaId { get; set; }

public Category categoria { get; set; }

public List<Tags> tags { get; set; }

}
