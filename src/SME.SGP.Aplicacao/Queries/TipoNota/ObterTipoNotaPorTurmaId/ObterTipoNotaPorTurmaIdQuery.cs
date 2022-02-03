using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoNotaPorTurmaIdQuery : IRequest<NotaTipoValor>
    {
        public ObterTipoNotaPorTurmaIdQuery(long turmaId, TipoTurma tipoturma)
        {
            TurmaId = turmaId;
            TipoTurma = tipoturma;
        }

        public long TurmaId { get; set; }
        public TipoTurma TipoTurma { get; set; }
    }
}
