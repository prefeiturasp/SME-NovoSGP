using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoDeCalendarioDaTurmaQuery : IRequest<TipoCalendario>
    {
        public Turma Turma { get; set; }

    }
}
