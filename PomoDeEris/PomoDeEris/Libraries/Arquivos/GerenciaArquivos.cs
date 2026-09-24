namespace PomoDeEris.Libraries.Arquivos
{
    public class GerenciaArquivos
    {
            public static string CadastrarImagemProduto(IFormFile file)
            {
                var NomeArquivo = Path.GetFileName(file.FileName);
                var Caminho = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", NomeArquivo);

                using (var stream = new FileStream(Caminho, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return Path.Combine("/Imagens", NomeArquivo).Replace("\\", "/");
            }
    }
}

