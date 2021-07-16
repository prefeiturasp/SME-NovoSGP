using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Queries.Abrangencia.ObterTurmaPorAnoLetivoCodigoUeModalidade;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.Abrangencia
{
    public class ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreUseCase : AbstractUseCase, IObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreUseCase
    {
        public ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<IEnumerable<OpcaoDropdownDto>> Executar(string codigoUe, int anoLetivo, Modalidade? modalidade, int semestre, bool consideraNovosAnosInfantil = false)
        {
            var anosInfantilDesconsiderar = modalidade == Modalidade.EducacaoInfantil && !consideraNovosAnosInfantil ?
                await mediator.Send(new ObterParametroTurmaFiltroPorAnoLetivoEModalidadeQuery(anoLetivo, Modalidade.EducacaoInfantil)) : null;
            return await mediator.Send(new ObterTurmaPorAnoLetivoCodigoUeModalidadeSemestreQuery(anoLetivo, codigoUe, modalidade, semestre, anosInfantilDesconsiderar));
        }
    }
}
