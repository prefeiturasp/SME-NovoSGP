using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoInformacoesEducacionais;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.UE.ObterTodasUes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Dtos.PainelEducacional.InformacoesEducacionais;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarInformacoesEducacionaisPainelEducacionalUseCase :
        ConsolidacaoBaseUseCase,
        IConsolidarInformacoesEducacionaisPainelEducacionalUseCase
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;
        private readonly IRepositorioTipoEscola repositorioTipoEscola;
        private readonly IRepositorioPainelEducacionalConsolidacaoInformacoesEducacionais repositorio;
        public ConsolidarInformacoesEducacionaisPainelEducacionalUseCase(IMediator mediator,
            IRepositorioTurmaConsulta repositorioTurmaConsulta,
            IRepositorioTipoEscola repositorioTipoEscola,
            IRepositorioPainelEducacionalConsolidacaoInformacoesEducacionais repositorio)
            : base(mediator) 
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta;
            this.repositorioTipoEscola = repositorioTipoEscola;
            this.repositorio = repositorio;
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            try
            {
                var listagensDres = await mediator.Send(new ObterTodasDresQuery());
                var listagemUe = (await mediator.Send(new ObterTodasUesQuery()))?.ToList();
                var tipoEscolas = await repositorioTipoEscola.ObterTodasAsync();

                return await ExecutarConsolidacao(listagensDres, listagemUe, tipoEscolas);
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand("Painel Educacional - Consolidação de Informações Educacionais", LogNivel.Critico, LogContexto.WorkerPainelEducacional, ex.Message));
            }

            return false;
        }

        private async Task<bool> ExecutarConsolidacao(IEnumerable<Dre> dres, List<Ue> ues, IEnumerable<TipoEscolaEol> tipoEscolas)
        {
            int anoUtilizado = DateTime.Now.Year;
            int anoMinimoConsulta = 2019;

            while (anoUtilizado >= anoMinimoConsulta)
            {
                await SalvarConsolidacaoInformacoesEducacionaisPorAno(anoUtilizado, dres, ues, tipoEscolas);

                if (anoUtilizado == anoMinimoConsulta)
                    break;

                anoUtilizado--;
            }

            return true;
        }

        private static IEnumerable<DadosParaConsolidarInformacoesEducacionaisDto> MapearRegistros(
                                                                                IEnumerable<DadosParaConsolidarInformacoesEducacionaisDto> registrosInformacoesEducacionais,
                                                                                IEnumerable<Dre> dres,
                                                                                IEnumerable<Ue> ues,
                                                                                IEnumerable<TipoEscolaEol> tipoEscolas)
        {
            return (from r in registrosInformacoesEducacionais
                    join u in ues on r.CodigoUe equals u.CodigoUe
                    join d in dres on u.DreId equals d.Id
                    join te in tipoEscolas on (int)u.TipoEscola equals te.CodEol
                    select new DadosParaConsolidarInformacoesEducacionaisDto
                    {
                        AnoLetivo = r.AnoLetivo,
                        CodigoDre = r.CodigoDre,
                        CodigoUe = r.CodigoUe,
                        Ue = $"{(string.IsNullOrEmpty(te.Descricao) ? "" : te.Descricao + " ")}{u.Nome}",
                        IdepAnosIniciais = r.IdepAnosIniciais,
                        IdepAnosFinais = r.IdepAnosFinais,                        
                        IdebAnosIniciais = r.IdebAnosIniciais,
                        IdebAnosFinais = r.IdebAnosFinais,
                        IdebEnsinoMedio = r.IdebEnsinoMedio,                        
                        PercentualFrequenciaGlobal = r.PercentualFrequenciaGlobal,
                        QuantidadeTurmasPap = r.QuantidadeTurmasPap,
                        PercentualFrequenciaAlunosPap = r.PercentualFrequenciaAlunosPap,                        
                        QuantidadeAlunosDesistentesAbandono = r.QuantidadeAlunosDesistentesAbandono,
                        QuantidadePromocoes = r.QuantidadePromocoes,
                        QuantidadeRetencoesFrequencia = r.QuantidadeRetencoesFrequencia,
                        QuantidadeRetencoesNota = r.QuantidadeRetencoesNota,                        
                        QuantidadeNotasAbaixoMedia = r.QuantidadeNotasAbaixoMedia,
                        QuantidadeNotasAcimaMedia = r.QuantidadeNotasAcimaMedia
                    }).ToList();
        }

        private async Task SalvarConsolidacaoInformacoesEducacionaisPorAno(int anoLetivo, IEnumerable<Dre> dres, IEnumerable<Ue> ues, IEnumerable<TipoEscolaEol> tipoEscolas)
        {
            var turmas = await repositorioTurmaConsulta.ObterTurmasComModalidadePorModalidadeAnoUe(anoLetivo, ues.Select(u => u.Id).ToArray(), ModalidadesTurmasInformacoesEducacionais.Select(m => (int)m).ToArray());

            var codigosUe = turmas.Where(t => !string.IsNullOrEmpty(t.SerieAno) && t.SerieAno.All(char.IsDigit)).Select(t => t.CodigoUe).Distinct().ToArray();

            var registrosInformacoesEducacionais = await repositorio.ObterDadosParaConsolidarInformacoesEducacionais(anoLetivo, codigosUe);

            registrosInformacoesEducacionais = MapearRegistros(registrosInformacoesEducacionais, dres, ues, tipoEscolas);

            var indicadores = registrosInformacoesEducacionais
                .GroupBy(r => new
                {
                    r.AnoLetivo,
                    r.CodigoDre,
                    r.CodigoUe,
                    r.Ue
                })
                .Select(g => new DadosParaConsolidarInformacoesEducacionaisDto
                {
                    AnoLetivo = g.Key.AnoLetivo,
                    CodigoDre = g.Key.CodigoDre,
                    CodigoUe = g.Key.CodigoUe,
                    Ue = g.Key.Ue,
                    // IDEP
                    IdepAnosIniciais = g.Sum(x => x.IdepAnosIniciais),
                    IdepAnosFinais = g.Sum(x => x.IdepAnosFinais),
                    // IDEB
                    IdebAnosIniciais = g.Sum(x => x.IdebAnosIniciais),
                    IdebAnosFinais = g.Sum(x => x.IdebAnosFinais),
                    IdebEnsinoMedio = g.Sum(x => x.IdebEnsinoMedio),
                    // Frequência Global
                    PercentualFrequenciaGlobal = g.Sum(x => x.PercentualFrequenciaGlobal),
                    // PAP
                    QuantidadeTurmasPap = g.Sum(x => x.QuantidadeTurmasPap),
                    PercentualFrequenciaAlunosPap = g.Sum(x => x.PercentualFrequenciaAlunosPap),
                    // Abandono
                    QuantidadeAlunosDesistentesAbandono = g.Sum(x => x.QuantidadeAlunosDesistentesAbandono),
                    // Aprovações
                    QuantidadePromocoes = g.Sum(x => x.QuantidadePromocoes),
                    QuantidadeRetencoesFrequencia = g.Sum(x => x.QuantidadeRetencoesFrequencia),
                    QuantidadeRetencoesNota = g.Sum(x => x.QuantidadeRetencoesNota),
                    // Notas
                    QuantidadeNotasAbaixoMedia = g.Sum(x => x.QuantidadeNotasAbaixoMedia),
                    QuantidadeNotasAcimaMedia = g.Sum(x => x.QuantidadeNotasAcimaMedia),
                })
                .OrderBy(u => u.Ue)
                .ToList();

            await mediator.Send(new SalvarPainelEducacionalConsolidacaoInformacoesEducacionaisCommand(indicadores));
        }
    }
}
