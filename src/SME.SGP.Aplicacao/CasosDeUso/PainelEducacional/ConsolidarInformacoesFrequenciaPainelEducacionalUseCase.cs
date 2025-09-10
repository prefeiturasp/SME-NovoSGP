using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirAgrupamentoEscola;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirAgrupamentoGlobal;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirAgrupamentoMensal;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarAgrupamentoEscola;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarAgrupamentoGlobal;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarAgrupamentoMensal;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarInformacoesFrequenciaPainelEducacionalUseCase : AbstractUseCase, IConsolidarInformacoesFrequenciaPainelEducacionalUseCase
    {
        private readonly IRepositorioFrequenciaConsulta repositorioFrequencia;

        public ConsolidarInformacoesFrequenciaPainelEducacionalUseCase(IMediator mediator, IRepositorioFrequenciaConsulta repositorioFrequencia) : base(mediator)
        {
            this.repositorioFrequencia = repositorioFrequencia;
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var registrosFrequencia = await repositorioFrequencia.ObterInformacoesFrequenciaPainelEducacional(DateTime.Now.Year);

            registrosFrequencia = MapearModalidades(registrosFrequencia);

            await SalvarAgrupamentoMensal(registrosFrequencia);
            await SalvarAgrupamentoGlobal(registrosFrequencia);
            await SalvarAgrupamentoGlobalEscola(registrosFrequencia);

            return true;
        }

        private async Task SalvarAgrupamentoMensal(IEnumerable<RegistroFrequenciaPainelEducacionalDto> registrosFrequencia)
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
                        PercentualFrequencia = g.Average(x => x.Percentual)
                    })
                    .OrderBy(x => x.Mes).ToList();

            await mediator.Send(new PainelEducacionalExcluirAgrupamentoMensalCommand());
            await mediator.Send(new PainelEducacionalSalvarAgrupamentoMensalCommand(registroFrequenciaMensal));
        }

        private async Task SalvarAgrupamentoGlobal(IEnumerable<RegistroFrequenciaPainelEducacionalDto> registrosFrequencia)
        {
            var registroFrequenciaGlobal = registrosFrequencia
                    .GroupBy(r => new { r.Modalidade, r.CodigoDre })
                    .Select(g => new PainelEducacionalRegistroFrequenciaAgrupamentoGlobal
                    {
                        Modalidade = g.Key.Modalidade,
                        CodigoUe = g.Select(x => x.CodigoUe).FirstOrDefault(),
                        CodigoDre = g.Key.CodigoDre,
                        TotalAulas = g.Sum(x => x.QuantidadeAulas),
                        TotalAusencias = g.Sum(x => x.QuantidadeAusencias),
                        TotalCompensacoes = g.Sum(x => x.QuantidadeCompensacoes),
                        PercentualFrequencia = g.Average(x => x.Percentual),
                        TotalAlunos = g.Select(x => x.CodigoAluno).Distinct().ToList().Count()
                    })
                    .OrderBy(x => x.Modalidade).ToList();

            await mediator.Send(new PainelEducacionalExcluirAgrupamentoGlobalCommand());
            await mediator.Send(new PainelEducacionalSalvarAgrupamentoGlobalCommand(registroFrequenciaGlobal));
        }

        private async Task SalvarAgrupamentoGlobalEscola(IEnumerable<RegistroFrequenciaPainelEducacionalDto> registrosFrequencia)
        {
            var registroFrequenciaEscola = registrosFrequencia
                    .GroupBy(r => new { r.CodigoUe, r.Mes })
                    .Select(g => new PainelEducacionalRegistroFrequenciaAgrupamentoEscola
                    {
                        CodigoUe = g.Key.CodigoUe,
                        Mes = g.Key.Mes,
                        TotalAulas = g.Sum(x => x.QuantidadeAulas),
                        TotalAusencias = g.Sum(x => x.QuantidadeAusencias),
                        TotalCompensacoes = g.Sum(x => x.QuantidadeCompensacoes),
                        PercentualFrequencia = g.Average(x => x.Percentual),
                        UE = g.Select(x => x.Ue).FirstOrDefault(),
                        CodigoDre = g.Select(x => x.CodigoDre).FirstOrDefault(),
                        TotalAlunos = g.Select(x => x.CodigoAluno).Distinct().ToList().Count()
                    })
                    .OrderBy(x => x.CodigoUe).ToList();

            await mediator.Send(new PainelEducacionalExcluirAgrupamentoGlobalEscolaCommand());
            await mediator.Send(new PainelEducacionalSalvarAgrupamentoGlobalEscolaCommand(registroFrequenciaEscola));
        }

        private IEnumerable<RegistroFrequenciaPainelEducacionalDto> MapearModalidades(IEnumerable<RegistroFrequenciaPainelEducacionalDto> registrosFrequencia)
        {
            return registrosFrequencia.Select(r => new RegistroFrequenciaPainelEducacionalDto
            {
                Id = r.Id,
                CodigoDre = r.CodigoDre,
                CodigoUe = r.CodigoUe,
                Ue = r.Ue,
                CodigoAluno = r.CodigoAluno,
                Mes = r.Mes,
                Percentual = r.Percentual,
                QuantidadeAulas = r.QuantidadeAulas,
                QuantidadeAusencias = r.QuantidadeAusencias,
                QuantidadeCompensacoes = r.QuantidadeCompensacoes,
                ModalidadeCodigo = r.ModalidadeCodigo,
                Modalidade = ObterNomeModalidade(r.ModalidadeCodigo, r.AnoTurma),
                AnoLetivo = r.AnoLetivo,
                AnoTurma = r.AnoTurma
            });
        }

        private static string ObterNomeModalidade(int modalidadeCodigo, string anoTurma)
        {
            if (modalidadeCodigo == (int)Modalidade.EducacaoInfantil)
            {
                if (!string.IsNullOrWhiteSpace(anoTurma))
                {
                    var ano = Convert.ToInt32(anoTurma);
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
