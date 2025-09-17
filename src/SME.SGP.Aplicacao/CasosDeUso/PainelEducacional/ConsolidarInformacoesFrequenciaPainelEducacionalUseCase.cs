using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarInformacoesFrequenciaPainelEducacionalUseCase : AbstractUseCase, IConsolidarInformacoesFrequenciaPainelEducacionalUseCase
    {
        private readonly IRepositorioFrequenciaConsulta repositorioFrequencia;
        private readonly IRepositorioDreConsulta repositorioDreConsulta;
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;
        private readonly IRepositorioUeConsulta repositorioUeConsulta;
        private readonly IRepositorioTipoEscola repositorioTipoEscola;

        public ConsolidarInformacoesFrequenciaPainelEducacionalUseCase(IMediator mediator, IRepositorioFrequenciaConsulta repositorioFrequencia, IRepositorioDreConsulta repositorioDreConsulta, IRepositorioTurmaConsulta repositorioTurmaConsulta, IRepositorioUeConsulta repositorioUeConsulta, IRepositorioTipoEscola repositorioTipoEscola) : base(mediator)
        {
            this.repositorioFrequencia = repositorioFrequencia;
            this.repositorioDreConsulta = repositorioDreConsulta;
            this.repositorioTurmaConsulta = repositorioTurmaConsulta;
            this.repositorioUeConsulta = repositorioUeConsulta;
            this.repositorioTipoEscola = repositorioTipoEscola;
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var dres = await repositorioDreConsulta.ObterTodas();
            var ues = repositorioUeConsulta.ObterTodas();
            var turmas = await repositorioTurmaConsulta.ObterTodasTurmasPainelEducacionalFrequenciaAsync();
            var tipoEscolas = await repositorioTipoEscola.ObterTodasAsync();

            int anoUtilizado = DateTime.Now.Year;
            int anoMinimoConsulta = 2019;

            while (anoUtilizado >= anoMinimoConsulta)
            {
                var turmaIds = turmas.Where(t => t.AnoLetivo == $"{anoUtilizado}").Select(t => t.TurmaId).Distinct().ToList();

                var registrosFrequencia = await repositorioFrequencia.ObterInformacoesFrequenciaPainelEducacional(turmaIds);

                registrosFrequencia = MapearRegistros(registrosFrequencia, dres, ues, turmas, tipoEscolas);

                await SalvarAgrupamentoMensal(registrosFrequencia, anoUtilizado);
                await SalvarAgrupamentoGlobal(registrosFrequencia, anoUtilizado);
                await SalvarAgrupamentoGlobalEscola(registrosFrequencia, anoUtilizado);

                if (anoUtilizado == anoMinimoConsulta)
                    break;

                anoUtilizado--;
            }

            return true;
        }

        private static IEnumerable<RegistroFrequenciaPainelEducacionalDto> MapearRegistros(
                                                                                        IEnumerable<RegistroFrequenciaPainelEducacionalDto> registrosFrequencia,
                                                                                        IEnumerable<Dre> dres,
                                                                                        IEnumerable<Ue> ues,
                                                                                        IEnumerable<TurmaPainelEducacionalFrequenciaDto> turmas,
                                                                                        IEnumerable<TipoEscolaEol> tipoEscolas
                                                                                    )
        {
            return (from r in registrosFrequencia
                    join t in turmas on (long)r.TurmaId equals t.TurmaId
                    join u in ues on t.UeId equals u.Id
                    join d in dres on u.DreId equals d.Id
                    join te in tipoEscolas on (int)u.TipoEscola equals te.CodEol
                    select new RegistroFrequenciaPainelEducacionalDto
                    {
                        Id = r.Id,
                        TurmaId = r.TurmaId,
                        CodigoAluno = r.CodigoAluno,
                        Mes = r.Mes,
                        Percentual = r.Percentual,
                        QuantidadeAulas = r.QuantidadeAulas,
                        QuantidadeAusencias = r.QuantidadeAusencias,

                        CodigoDre = d.CodigoDre,
                        Dre = d.Nome,
                        CodigoUe = u.CodigoUe,
                        Ue = $"{(string.IsNullOrEmpty(te.Descricao) ? "" : te.Descricao + " ")}{u.Nome}",

                        ModalidadeCodigo = t.ModalidadeCodigo,
                        Modalidade = ObterNomeModalidade(t.ModalidadeCodigo, t.Ano),
                        AnoLetivo = int.Parse(t.AnoLetivo),
                        AnoTurma = t.Ano
                    }).ToList();
        }

        private async Task SalvarAgrupamentoMensal(IEnumerable<RegistroFrequenciaPainelEducacionalDto> registrosFrequencia, int anoUtilizado)
        {
            var registroFrequenciaMensal = registrosFrequencia
                    .GroupBy(r => new { r.Mes, r.Modalidade, r.CodigoUe, r.CodigoDre })
                    .Select(g => new PainelEducacionalRegistroFrequenciaAgrupamentoMensal
                    {
                        Modalidade = g.Key.Modalidade,
                        AnoLetivo = g.Select(x => x.AnoLetivo).FirstOrDefault(),
                        Mes = g.Key.Mes,
                        TotalAulas = g.Sum(x => x.QuantidadeAulas),
                        TotalFaltas = g.Sum(x => x.QuantidadeAusencias),
                        CodigoUe = g.Key.CodigoUe,
                        CodigoDre = g.Key.CodigoDre,
                        PercentualFrequencia = g.Average(x => x.Percentual),
                    })
                    .OrderBy(x => x.Mes).ToList();

            await mediator.Send(new PainelEducacionalExcluirAgrupamentoMensalCommand(anoUtilizado));
            await mediator.Send(new PainelEducacionalSalvarAgrupamentoMensalCommand(registroFrequenciaMensal));
        }

        private async Task SalvarAgrupamentoGlobal(IEnumerable<RegistroFrequenciaPainelEducacionalDto> registrosFrequencia, int anoUtilizado)
        {
            var registroFrequenciaGlobal = registrosFrequencia
                    .GroupBy(r => new { r.Modalidade, r.CodigoDre })
                    .Select(g => new PainelEducacionalRegistroFrequenciaAgrupamentoGlobal
                    {
                        AnoLetivo = g.Select(x => x.AnoLetivo).FirstOrDefault(),
                        Modalidade = g.Key.Modalidade,
                        CodigoUe = g.Select(x => x.CodigoUe).FirstOrDefault(),
                        CodigoDre = g.Key.CodigoDre,
                        TotalAulas = g.Sum(x => x.QuantidadeAulas),
                        TotalAusencias = g.Sum(x => x.QuantidadeAusencias),
                        PercentualFrequencia = g.Average(x => x.Percentual),
                        TotalAlunos = g.Select(x => x.CodigoAluno).Distinct().ToList().Count()
                    })
                    .OrderBy(x => x.Modalidade).ToList();

            await mediator.Send(new PainelEducacionalExcluirAgrupamentoGlobalCommand(anoUtilizado));
            await mediator.Send(new PainelEducacionalSalvarAgrupamentoGlobalCommand(registroFrequenciaGlobal));
        }

        private async Task SalvarAgrupamentoGlobalEscola(IEnumerable<RegistroFrequenciaPainelEducacionalDto> registrosFrequencia, int anoUtilizado)
        {
            var registroFrequenciaEscola = registrosFrequencia
                    .GroupBy(r => new { r.CodigoUe, r.Mes })
                    .Select(g => new PainelEducacionalRegistroFrequenciaAgrupamentoEscola
                    {
                        AnoLetivo = g.Select(x => x.AnoLetivo).FirstOrDefault(),
                        CodigoUe = g.Key.CodigoUe,
                        Mes = g.Key.Mes,
                        TotalAulas = g.Sum(x => x.QuantidadeAulas),
                        TotalAusencias = g.Sum(x => x.QuantidadeAusencias),
                        PercentualFrequencia = g.Average(x => x.Percentual),
                        UE = g.Select(x => x.Ue).FirstOrDefault(),
                        CodigoDre = g.Select(x => x.CodigoDre).FirstOrDefault(),
                        TotalAlunos = g.Select(x => x.CodigoAluno).Distinct().ToList().Count()
                    })
                    .OrderBy(x => x.CodigoUe).ToList();

            await mediator.Send(new PainelEducacionalExcluirAgrupamentoGlobalEscolaCommand(anoUtilizado));
            await mediator.Send(new PainelEducacionalSalvarAgrupamentoGlobalEscolaCommand(registroFrequenciaEscola));
        }

        private static string ObterNomeModalidade(int modalidadeCodigo, string anoTurma)
        {
            if (modalidadeCodigo == (int)Modalidade.EducacaoInfantil)
            {
                if (!string.IsNullOrWhiteSpace(anoTurma) && int.TryParse(anoTurma, out var ano))
                {
                    if (ano >= 1 && ano <= 4)
                        return "Creche";

                    if (ano >= 5 && ano <= 7)
                        return "Pré-Escola";
                }
            }

            return ((Modalidade)modalidadeCodigo).ObterDisplayName();
        }

    }
}
