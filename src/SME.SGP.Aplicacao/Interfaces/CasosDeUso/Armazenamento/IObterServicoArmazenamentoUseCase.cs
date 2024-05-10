namespace SME.SGP.Aplicacao
{
    public interface IObterServicoArmazenamentoUseCase
    {
        string Executar(string nomeArquivo, bool ehPastaTemporaria);
    }
}
