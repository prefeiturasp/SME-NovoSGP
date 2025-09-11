using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirVisaoGeral;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarAgrupamentoMensal;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarVisaoGeralPainelEducacionalUseCase : AbstractUseCase, IConsolidarVisaoGeralPainelEducacionalUseCase
    {
        private readonly IRepositorioPainelEducacionalVisaoGeral repositorioPainelEducacionalVisaoGeral;

        public ConsolidarVisaoGeralPainelEducacionalUseCase(IMediator mediator, IRepositorioPainelEducacionalVisaoGeral repositorioPainelEducacionalVisaoGeral) : base(mediator)
        {
            this.repositorioPainelEducacionalVisaoGeral = repositorioPainelEducacionalVisaoGeral;
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var registros = await repositorioPainelEducacionalVisaoGeral.ObterVisaoGeralPainelEducacional();

            await SalvarAgrupamentoVisaoGeral(registros);
            
            return true;
        }

        private async Task SalvarAgrupamentoVisaoGeral(IEnumerable<PainelEducacionalVisaoGeralDto> registro)
        {
            var registroVisaoGeral = registro
                    .GroupBy(r => new { r.CodigoDre, r.CodigoUe, r.Indicador, r.Serie, r.AnoLetivo })
                    .Select(g => new PainelEducacionalVisaoGeral
                    {
                        CodigoDre = g.Key.CodigoDre,
                        CodigoUe = g.Key.CodigoUe,
                        Indicador = g.Key.Indicador,
                        AnoLetivo = g.Key.AnoLetivo,
                        Serie = MapearReferencia(g.Key.Indicador, g.Key.Serie),
                        Valor = g.Average(x => x.Valor)
                    })
                    .OrderBy(x => x.AnoLetivo).ToList();

            await mediator.Send(new PainelEducacionalExcluirVisaoGeralCommand());
            await mediator.Send(new PainelEducacionalSalvarVisaoGeralCommand(registroVisaoGeral));
        }

        private string MapearReferencia(string indicador, string serie)
        {
            if (!int.TryParse(serie, out var texto))
                return null;

            if (indicador == "IDEP" && Enum.IsDefined(typeof(SerieAnoIdepEnum), texto))
                return ((SerieAnoIdepEnum)texto).Name();

            if (indicador == "IDEB" && Enum.IsDefined(typeof(SerieAnoIdebEnum), texto))
                return ((SerieAnoIdebEnum)texto).Name();

            return serie;
        }
    }
}
