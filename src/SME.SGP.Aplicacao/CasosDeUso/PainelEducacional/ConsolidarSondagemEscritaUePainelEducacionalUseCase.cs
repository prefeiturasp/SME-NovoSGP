using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoSondagemEscrita;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.TaxaAlfabetizacao.ObterSondagemEscrita;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarSondagemEscritaUePainelEducacionalUseCase : AbstractUseCase, IConsolidarSondagemEscritaUePainelEducacionalUseCase
    {
        public ConsolidarSondagemEscritaUePainelEducacionalUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var sondagemEscrita = await mediator.Send(new ObterSondagemEscritaUePorAnoLetivoPeriodoQuery());

            if (sondagemEscrita?.Any() != true)
                return false;

            var registroConsolidado = sondagemEscrita
                .GroupBy(r => new { r.AnoLetivo, r.CodigoDre, r.CodigoUe, r.SerieAno, r.Bimestre })
                .Select(g => new PainelEducacionalConsolidacaoSondagemEscritaUe
                {
                    CodigoDre = g.Key.CodigoDre,
                    CodigoUe = g.Key.CodigoUe,
                    PreSilabico = g.Sum(x => x.PreSilabico),
                    SilabicoSemValor = g.Sum(x => x.SilabicoSemValor),
                    SilabicoComValor = g.Sum(x => x.SilabicoComValor),
                    SilabicoAlfabetico = g.Sum(x => x.SilabicoAlfabetico),
                    Alfabetico = g.Sum(x => x.Alfabetico),
                    SemPreenchimento = g.Sum(x => x.SemPreenchimento),
                    QuantidadeAluno = g.Sum(x => x.QuantidadeAluno), 
                    Bimestre = g.Key.Bimestre,
                    AnoLetivo = g.Key.AnoLetivo,
                    SerieAno = g.Key.SerieAno
                })
                .OrderBy(x => x.SerieAno)
                .ThenBy(x => x.CodigoDre)
                .ThenBy(x => x.Bimestre) 
                .ToList();

            await mediator.Send(new SalvarPainelEducacionalConsolidacaoSondagemEscritaUeCommand(registroConsolidado));

            return true;
        }
    }
}
