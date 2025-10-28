using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotas.VisaoUe;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasModalidadesNotasVisaoUeUseCase : IConsultasModalidadesNotasVisaoUeUseCase
    {
        private readonly IMediator mediator;

        public ConsultasModalidadesNotasVisaoUeUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<IdentificacaoInfo>> ObterModalidadesNotasVisaoUe(int anoLetivo, string codigoUe, int bimestre)
        {
            var dados = await mediator.Send(new ObterModalidadesNotasVisaoUeQuery(anoLetivo, codigoUe, bimestre));

            var resultado = dados
                .Select(m => new IdentificacaoInfo
                {
                    Id = m.Id,
                    Nome = ((Modalidade)m.Id).ObterAtributo<DisplayAttribute>().Name
                });

            return resultado;
        }
    }
}
