using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirAgrupamentoEscola;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirAgrupamentoGlobal;
using SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirAgrupamentoMensal;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarAgrupamentoEscola;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarAgrupamentoGlobal;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarAgrupamentoMensal;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
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

            await SalvarAgrupamentoMensal(registrosFrequencia);
            await SalvarAgrupamentoGlobal(registrosFrequencia);
            await SalvarAgrupamentoGlobalEscola(registrosFrequencia);

            return true;
        }

        private async Task SalvarAgrupamentoMensal(IEnumerable<RegistroFrequenciaPainelEducacionalDto> registrosFrequencia)
        {
            var registroFrequenciaMensal = registrosFrequencia
                    .GroupBy(r => new { r.AnoLetivo, r.Mes, r.ModalidadeCodigo })
                    .Select(g => new PainelEducacionalRegistroFrequenciaAgrupamentoMensal
                    {
                        Modalidade = g.Key.ModalidadeCodigo,
                        AnoLetivo = g.Key.AnoLetivo,
                        Mes = g.Key.Mes,
                        TotalAulas = g.Sum(x => x.QuantidadeAulas),
                        TotalFaltas = g.Sum(x => x.QuantidadeAusencias),
                        PercentualFrequencia = g.Average(x => x.Percentual)
                    })
                    .OrderBy(x => x.Mes);

            await mediator.Send(new PainelEducacionalExcluirAgrupamentoMensalCommand());
            await mediator.Send(new PainelEducacionalSalvarAgrupamentoMensalCommand(registroFrequenciaMensal));
        }

        private async Task SalvarAgrupamentoGlobal(IEnumerable<RegistroFrequenciaPainelEducacionalDto> registrosFrequencia)
        {
            var registroFrequenciaGlobal = registrosFrequencia
                    .GroupBy(r => new { r.ModalidadeCodigo })
                    .Select(g => new PainelEducacionalRegistroFrequenciaAgrupamentoGlobal
                    {
                        Modalidade = g.Key.ModalidadeCodigo,
                        TotalAulas = g.Sum(x => x.QuantidadeAulas),
                        TotalAusencias = g.Sum(x => x.QuantidadeAusencias),
                        TotalCompensacoes = g.Sum(x => x.QuantidadeCompensacoes),
                        PercentualFrequencia = g.Average(x => x.Percentual),
                        TotalAlunos = g.Select(x => x.CodigoAluno).Distinct().ToList().Count()
                    })
                    .OrderBy(x => x.Modalidade);

            await mediator.Send(new PainelEducacionalExcluirAgrupamentoGlobalCommand());
            await mediator.Send(new PainelEducacionalSalvarAgrupamentoGlobalCommand(registroFrequenciaGlobal));
        }

        private async Task SalvarAgrupamentoGlobalEscola(IEnumerable<RegistroFrequenciaPainelEducacionalDto> registrosFrequencia)
        {
            var registroFrequenciaEscola = registrosFrequencia
                    .GroupBy(r => new { r.CodigoUe })
                    .Select(g => new PainelEducacionalRegistroFrequenciaAgrupamentoEscola
                    {
                        CodigoUe = g.Key.CodigoUe,
                        TotalAulas = g.Sum(x => x.QuantidadeAulas),
                        TotalAusencias = g.Sum(x => x.QuantidadeAusencias),
                        TotalCompensacoes = g.Sum(x => x.QuantidadeCompensacoes),
                        PercentualFrequencia = g.Average(x => x.Percentual),
                        UE = g.Select(x => x.Ue).FirstOrDefault(),
                        CodigoDre = g.Select(x => x.CodigoDre).FirstOrDefault(),
                        TotalAlunos = g.Select(x => x.CodigoAluno).Distinct().ToList().Count()
                    })
                    .OrderBy(x => x.CodigoUe);

            await mediator.Send(new PainelEducacionalExcluirAgrupamentoGlobalEscolaCommand());
            await mediator.Send(new PainelEducacionalSalvarAgrupamentoGlobalEscolaCommand(registroFrequenciaEscola));
        }
    }
}
