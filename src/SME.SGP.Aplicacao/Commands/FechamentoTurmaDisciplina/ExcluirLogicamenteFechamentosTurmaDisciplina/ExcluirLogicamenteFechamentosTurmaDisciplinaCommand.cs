using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirLogicamenteFechamentosTurmaDisciplinaCommand : IRequest<bool>
    {
        public ExcluirLogicamenteFechamentosTurmaDisciplinaCommand(long[] idsFechamentoTurmaDisciplina)
        {
            IdsFechamentoTurmaDisciplina = idsFechamentoTurmaDisciplina;
        }

        public long[] IdsFechamentoTurmaDisciplina { get; private set; }
    }
}
