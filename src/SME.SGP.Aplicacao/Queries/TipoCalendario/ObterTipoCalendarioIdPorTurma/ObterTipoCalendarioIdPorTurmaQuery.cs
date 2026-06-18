using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoCalendarioIdPorTurmaQuery: IRequest<long>
    {
        public ObterTipoCalendarioIdPorTurmaQuery(Turma turma)
        {
            Turma = turma;
        }

        public Turma Turma { get; set; }
    }
}
