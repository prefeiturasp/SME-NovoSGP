using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosFechamentoFinal : IComandosFechamentoFinal
    {
        private readonly IRepositorioFechamentoAlunoConsulta repositorioFechamentoAluno;
        private readonly IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma;        
        private readonly IRepositorioTurmaConsulta repositorioTurmaConsulta;        
        private readonly IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina;
        private readonly IServicoFechamentoFinal servicoFechamentoFinal;
        private readonly IMediator mediator;

        public ComandosFechamentoFinal(
            IServicoFechamentoFinal servicoFechamentoFinal,
            IRepositorioTurmaConsulta repositorioTurmaConsulta,
            IRepositorioFechamentoAlunoConsulta repositorioFechamentoAluno,
            IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma,            
            IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina,
            IMediator mediator)
        {
            this.servicoFechamentoFinal = servicoFechamentoFinal ?? throw new System.ArgumentNullException(nameof(servicoFechamentoFinal));
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new System.ArgumentNullException(nameof(repositorioTurmaConsulta));
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.repositorioFechamentoAluno = repositorioFechamentoAluno ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoAluno));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<AuditoriaPersistenciaDto> SalvarAsync(FechamentoFinalSalvarDto fechamentoFinalSalvarDto)
        {
            var usuarioLogado = await ObterUsuarioLogado();
            var turma = await ObterTurma(fechamentoFinalSalvarDto.TurmaCodigo);
            var emAprovacao = await ExigeAprovacao(turma, usuarioLogado);

            var fechamentoTurmaDisciplina = await TransformarDtoSalvarEmEntidade(fechamentoFinalSalvarDto, turma, emAprovacao);

            var auditoria = await servicoFechamentoFinal.SalvarAsync(fechamentoTurmaDisciplina, turma, usuarioLogado, fechamentoFinalSalvarDto.Itens, emAprovacao);

            if (!auditoria.EmAprovacao)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarTurmaFechamentoSync,
                                                               new ConsolidacaoTurmaDto(turma.Id, 0),
                                                               Guid.NewGuid(),
                                                               null));

            return auditoria;
        }

        private Task<Usuario> ObterUsuarioLogado()
            => mediator.Send(new ObterUsuarioLogadoQuery());

        private async Task<Turma> ObterTurma(string turmaCodigo)
        {
            var turma = await repositorioTurmaConsulta.ObterTurmaComUeEDrePorCodigo(turmaCodigo);
            if (turma == null)
                throw new NegocioException("Não foi possível localizar a turma.");
            return turma;
        }

        private async Task<bool> ExigeAprovacao(Turma turma, Usuario usuarioLogado)
        {
            return turma.AnoLetivo < DateTime.Today.Year
                && !usuarioLogado.EhGestorEscolar()
                && await ParametroAprovacaoAtivo(turma.AnoLetivo);
        }

        private async Task<bool> ParametroAprovacaoAtivo(int anoLetivo)
        {
            var parametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.AprovacaoAlteracaoNotaConselho, anoLetivo));
            if (parametro == null)
                throw new NegocioException($"Não foi possível localizar o parametro 'AprovacaoAlteracaoNotaConselho' para o ano {anoLetivo}");

            return parametro.Ativo;
        }

        private async Task<FechamentoTurmaDisciplina> TransformarDtoSalvarEmEntidade(FechamentoFinalSalvarDto fechamentoFinalSalvarDto, Turma turma, bool emAprovacao)
        {
            var disciplinaId = fechamentoFinalSalvarDto.EhRegencia ? long.Parse(fechamentoFinalSalvarDto.DisciplinaId) : fechamentoFinalSalvarDto.Itens.First().ComponenteCurricularCodigo;

            FechamentoTurmaDisciplina fechamentoTurmaDisciplina = null;
            var fechamentoFinalTurma = await repositorioFechamentoTurma.ObterPorTurmaPeriodo(turma.Id);
            if (fechamentoFinalTurma == null)
                fechamentoFinalTurma = new FechamentoTurma(0, turma.Id);
            else
                fechamentoTurmaDisciplina = await repositorioFechamentoTurmaDisciplina.ObterFechamentoTurmaDisciplina(fechamentoFinalSalvarDto.TurmaCodigo, disciplinaId);

            if (fechamentoTurmaDisciplina == null)
                fechamentoTurmaDisciplina = new FechamentoTurmaDisciplina() { DisciplinaId = disciplinaId, Situacao = SituacaoFechamento.ProcessadoComSucesso };

            fechamentoTurmaDisciplina.FechamentoTurma = fechamentoFinalTurma;

            foreach (var agrupamentoAluno in fechamentoFinalSalvarDto.Itens.GroupBy(a => a.AlunoRf))
            {
                var fechamentoAluno = await repositorioFechamentoAluno.ObterFechamentoAlunoENotas(fechamentoTurmaDisciplina.Id, agrupamentoAluno.Key);
                if (fechamentoAluno == null)
                    fechamentoAluno = new FechamentoAluno() { AlunoCodigo = agrupamentoAluno.Key };

                fechamentoTurmaDisciplina.FechamentoAlunos.Add(fechamentoAluno);
            }

            return fechamentoTurmaDisciplina;
        }
    }
}