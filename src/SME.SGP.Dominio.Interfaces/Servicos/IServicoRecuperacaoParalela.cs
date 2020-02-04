namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoRecuperacaoParalela
    {
        RecuperacaoParalelaStatus ObterStatusRecuperacaoParalela(int RespostasRecuperacaoParalela, int Respostas);
    }
}