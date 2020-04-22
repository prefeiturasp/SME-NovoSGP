using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaPorCodigoQuery : IRequest<Turma>
    {
        public string TurmaCodigo { get; set; }
    }
}
