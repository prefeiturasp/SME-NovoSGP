using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class SalvarCompensacaoAusenciaDiciplinaRegenciaCommand : IRequest<long>
    {
        public SalvarCompensacaoAusenciaDiciplinaRegenciaCommand(CompensacaoAusenciaDisciplinaRegencia compensacaoAusenciaDisciplinaRegencia)
        {
            CompensacaoAusenciaDisciplinaRegencia = compensacaoAusenciaDisciplinaRegencia;
        }

        public Dominio.CompensacaoAusenciaDisciplinaRegencia CompensacaoAusenciaDisciplinaRegencia { get; }
    }
}
