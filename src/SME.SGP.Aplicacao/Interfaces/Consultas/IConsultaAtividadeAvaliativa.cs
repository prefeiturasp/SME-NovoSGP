using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IConsultaAtividadeAvaliativa
    {
        AtividadeAvaliativaCompletaDto ObterPorId(long id);
    }
}