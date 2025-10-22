using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.Reclassificacao.ExcluirReclassificacaoAnual;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarReclassificacaoAnual;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.SituacaoAluno;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarReclassificacaoPainelEducacionalUseCase : AbstractUseCase, IConsolidarReclassificacaoPainelEducacionalUseCase
    {
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;

        public ConsolidarReclassificacaoPainelEducacionalUseCase(IMediator mediator, IRepositorioTurmaConsulta repositorioTurmaConsulta) : base(mediator)
        {
            this.repositorioTurmaConsulta = repositorioTurmaConsulta;
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            int anoUtilizado = DateTime.Now.Year;
            int anoMinimoConsulta = 2019;

            while (anoUtilizado >= anoMinimoConsulta)
            {
                var registrosReclassificacao = await ObterDadosReclassificacaoPorAno(anoUtilizado);
                await SalvarAgrupamentoAnual(registrosReclassificacao, anoUtilizado);

                if (anoUtilizado == anoMinimoConsulta)
                    break;

                anoUtilizado--;
            }

            return true;
        }

        private async Task<IEnumerable<PainelEducacionalReclassificacao>> ObterDadosReclassificacaoPorAno(int anoLetivo)
        {
            var alunosReclassificados = new List<AlunosSituacaoTurmas>();

            var turmasPainelEducacional = (await repositorioTurmaConsulta.ObterTurmasPorAnoLetivo(anoLetivo))
                                          .Where(t => (t.ModalidadeCodigo == Modalidade.Fundamental || t.ModalidadeCodigo == Modalidade.Medio || t.ModalidadeCodigo == Modalidade.EJA)
                                                 && int.TryParse(t.Ano, out _))
                                          .ToList();

            if (!turmasPainelEducacional.Any())
                return new List<PainelEducacionalReclassificacao>();

            var todasDres = await mediator.Send(new ObterTodasDresQuery());

            if (!todasDres.Any())
                return new List<PainelEducacionalReclassificacao>();

            foreach (var dre in todasDres)
            {
                var alunosSituacaoTurmas = await mediator.Send(new ObterAlunosSituacaoTurmasQuery(anoLetivo, (int)SituacaoMatriculaAluno.ReclassificadoSaida, dre.CodigoDre));
                alunosReclassificados.AddRange(alunosSituacaoTurmas);
            }

            if (!alunosReclassificados.Any())
                return new List<PainelEducacionalReclassificacao>();

            var registrosReclassificacao = alunosReclassificados
                .Where(aluno => turmasPainelEducacional.Any(turma => turma.CodigoTurma == aluno.CodigoTurma))
                .Select(aluno => new
                {
                    Aluno = aluno,
                    Turma = turmasPainelEducacional.First(turma => turma.CodigoTurma == aluno.CodigoTurma)
                })
               .GroupBy(item => new
               {
                   DreId = item.Turma.Ue.Dre.CodigoDre,
                   UeId = item.Turma.Ue.CodigoUe,
                   AnoLetivo = anoLetivo,
                   AnoTurma = item.Turma.Ano,
                   ModalidadeTurma = (Modalidade)item.Turma.ModalidadeCodigo
               })
               .Select(g => new PainelEducacionalReclassificacao
               {
                   CodigoDre = g.Key.DreId,
                   CodigoUe = g.Key.UeId,
                   AnoLetivo = g.Key.AnoLetivo,
                   AnoTurma = Convert.ToInt16(g.Key.AnoTurma),
                   ModalidadeTurma = g.Key.ModalidadeTurma,
                   QuantidadeAlunosReclassificados = g.Sum(x => x.Aluno.QuantidadeAlunos)
               })
                .ToList();

            return registrosReclassificacao;
        }

        private async Task SalvarAgrupamentoAnual(IEnumerable<PainelEducacionalReclassificacao> registrosReclassificacao, int anoUtilizado)
        {
            if (!registrosReclassificacao.Any())
                return;

            var registroAnual = registrosReclassificacao
                    .GroupBy(r => new { r.CodigoDre, r.CodigoUe, r.AnoLetivo, r.AnoTurma, r.ModalidadeTurma })
                    .Select(g => new PainelEducacionalReclassificacao
                    {
                        CodigoDre = g.Key.CodigoDre,
                        CodigoUe = g.Key.CodigoUe,
                        AnoLetivo = g.Key.AnoLetivo,
                        ModalidadeTurma = g.Key.ModalidadeTurma,
                        AnoTurma = g.Key.AnoTurma,
                        QuantidadeAlunosReclassificados = g.Sum(x => x.QuantidadeAlunosReclassificados)
                    })
                    .OrderByDescending(x => x.AnoLetivo).ToList();

            await mediator.Send(new PainelEducacionalExcluirReclassificacaoAnualCommand(anoUtilizado));
            await mediator.Send(new PainelEducacionalSalvarReclassificacaoAnualCommand(registroAnual));
        }
    }
}
