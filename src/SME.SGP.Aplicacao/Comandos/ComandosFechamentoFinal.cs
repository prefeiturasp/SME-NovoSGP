using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes;

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
        private readonly IRepositorioCache repositorioCache;

        public ComandosFechamentoFinal(
            IServicoFechamentoFinal servicoFechamentoFinal,
            IRepositorioTurmaConsulta repositorioTurmaConsulta,
            IRepositorioFechamentoAlunoConsulta repositorioFechamentoAluno,
            IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurma,            
            IRepositorioFechamentoTurmaDisciplinaConsulta repositorioFechamentoTurmaDisciplina,
            IMediator mediator,
            IRepositorioCache repositorioCache)
        {
            this.servicoFechamentoFinal = servicoFechamentoFinal ?? throw new ArgumentNullException(nameof(servicoFechamentoFinal));
            this.repositorioTurmaConsulta = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
            this.repositorioFechamentoTurma = repositorioFechamentoTurma ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurma));
            this.repositorioFechamentoAluno = repositorioFechamentoAluno ?? throw new ArgumentNullException(nameof(repositorioFechamentoAluno));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<AuditoriaPersistenciaDto> SalvarAsync(FechamentoFinalSalvarDto fechamentoFinalSalvarDto)
        {
            var usuarioLogado = await ObterUsuarioLogado();
            var turma = await ObterTurma(fechamentoFinalSalvarDto.TurmaCodigo);
            var emAprovacao = await ExigeAprovacao(turma, usuarioLogado);

            var fechamentoTurmaDisciplina = await TransformarDtoSalvarEmEntidade(fechamentoFinalSalvarDto, turma);

            var auditoria = await servicoFechamentoFinal.SalvarAsync(fechamentoTurmaDisciplina, turma, usuarioLogado,
                fechamentoFinalSalvarDto.Itens, emAprovacao);

            await InserirOuAtualizarCache(fechamentoFinalSalvarDto, emAprovacao);

            if (!auditoria.EmAprovacao)
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFechamento.ConsolidarTurmaFechamentoSync,
                    new ConsolidacaoTurmaDto(turma.Id, 0),
                    Guid.NewGuid()));
            }

            return auditoria;
        }

        private async Task InserirOuAtualizarCache(FechamentoFinalSalvarDto fechamentoFinalSalvar, bool emAprovacao)
        {
            var disciplinaId = fechamentoFinalSalvar.EhRegencia ? long.Parse(fechamentoFinalSalvar.DisciplinaId) :
                fechamentoFinalSalvar.Itens.First().ComponenteCurricularCodigo;            
            
            var nomeChaveCache = string.Format(NomeChaveCache.CHAVE_FECHAMENTO_NOTA_FINAL_COMPONENTE_TURMA,
                disciplinaId, fechamentoFinalSalvar.TurmaCodigo);

            var retornoCacheMapeado = await repositorioCache.ObterObjetoAsync<List<FechamentoNotaAlunoAprovacaoDto>>(nomeChaveCache, "Obter fechamento nota final");

            if (retornoCacheMapeado == null)
                return;

            foreach (var fechamentoFinal in fechamentoFinalSalvar.Itens)
            {
                var cacheAluno = retornoCacheMapeado.FirstOrDefault(c => c.AlunoCodigo == fechamentoFinal.AlunoRf &&
                                                                         c.ComponenteCurricularId == fechamentoFinal.ComponenteCurricularCodigo);

                if (cacheAluno == null)
                {
                    retornoCacheMapeado.Add(new FechamentoNotaAlunoAprovacaoDto
                    {
                        Bimestre = 0,
                        Nota = fechamentoFinal.Nota,
                        AlunoCodigo = fechamentoFinal.AlunoRf,
                        ConceitoId = fechamentoFinal.ConceitoId,
                        EmAprovacao = emAprovacao,
                        ComponenteCurricularId = disciplinaId
                    });

                    continue;
                }

                cacheAluno.Nota = fechamentoFinal.Nota;
                cacheAluno.ConceitoId = fechamentoFinal.ConceitoId;
                cacheAluno.EmAprovacao = emAprovacao;
            }

            await repositorioCache.SalvarAsync(nomeChaveCache, retornoCacheMapeado);
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

        private async Task<FechamentoTurmaDisciplina> TransformarDtoSalvarEmEntidade(FechamentoFinalSalvarDto fechamentoFinalSalvarDto, Turma turma)
        {
            var disciplinaId = fechamentoFinalSalvarDto.EhRegencia ? long.Parse(fechamentoFinalSalvarDto.DisciplinaId) :
                fechamentoFinalSalvarDto.Itens.First().ComponenteCurricularCodigo;

            FechamentoTurmaDisciplina fechamentoTurmaDisciplina = null;

            var fechamentoFinalTurma = await repositorioFechamentoTurma.ObterPorTurmaPeriodo(turma.Id);

            if (fechamentoFinalTurma == null)
                fechamentoFinalTurma = new FechamentoTurma(0, turma.Id);
            else
                fechamentoTurmaDisciplina = await repositorioFechamentoTurmaDisciplina.ObterFechamentoTurmaDisciplina(fechamentoFinalSalvarDto.TurmaCodigo, disciplinaId);

            fechamentoTurmaDisciplina ??= new FechamentoTurmaDisciplina
            {
                DisciplinaId = disciplinaId,
                Situacao = SituacaoFechamento.ProcessadoComSucesso
            };

            fechamentoTurmaDisciplina.FechamentoTurma = fechamentoFinalTurma;

            foreach (var agrupamentoAluno in fechamentoFinalSalvarDto.Itens.GroupBy(a => a.AlunoRf))
            {
                var fechamentoAluno = await repositorioFechamentoAluno.ObterFechamentoAlunoENotas(fechamentoTurmaDisciplina.Id, agrupamentoAluno.Key) ??
                    new FechamentoAluno { AlunoCodigo = agrupamentoAluno.Key };

                fechamentoTurmaDisciplina.FechamentoAlunos.Add(fechamentoAluno);
            }

            return fechamentoTurmaDisciplina;
        }
    }
}