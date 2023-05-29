using System.Text;

string diretorio = @"C:\Users\ewerton.guimaraes\Desktop\Etiquetas";
string diretorioImpressos = Path.Combine(diretorio, "Impressos");

try
{
    // Criar a pasta "Impressos" se ainda não existir
    if (!Directory.Exists(diretorioImpressos))
    {
        Directory.CreateDirectory(diretorioImpressos);
    }

    // Obter todos os arquivos com extensão .txt no diretório
    string[] arquivosZPL = Directory.GetFiles(diretorio, "*.txt");

    foreach (string arquivoZPL in arquivosZPL)
    {
        // Ler o conteúdo do arquivo ZPL
        string conteudoZPL = File.ReadAllText(arquivoZPL, Encoding.UTF8);

        // Enviar o conteúdo para a impressora
        RawPrinterHelper.SendStringToPrinter("ELGIN L42Pro", conteudoZPL);

        // Obter o nome do arquivo apenas
        string nomeArquivo = Path.GetFileName(arquivoZPL);

        // Mover o arquivo para a pasta "Impressos"
        string destino = Path.Combine(diretorioImpressos, nomeArquivo);
        File.Move(arquivoZPL, destino);

        Console.WriteLine($"Impressão do arquivo {arquivoZPL} concluída e movido para a pasta Impressos.");
    }
}
catch (Exception ex)
{
    Console.WriteLine("Ocorreu um erro durante a impressão: " + ex.Message);
}
