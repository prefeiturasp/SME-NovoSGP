using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ExigeAprovacaoDeNotaQuery : IRequest<bool>
    {
        public Turma Turma { get; set; }

        public ExigeAprovacaoDeNotaQuery(Turma turma)
        {
            Turma = turma;
        }
    }
}
