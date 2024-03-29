﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarDiarioBordoCommandHandler : IRequestHandler<AlterarDiarioBordoCommand, AuditoriaDto>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioDiarioBordo repositorioDiarioBordo;
        private readonly IConsultasDisciplina consultasDisciplina;

        public AlterarDiarioBordoCommandHandler(IMediator mediator,
                                                IRepositorioDiarioBordo repositorioDiarioBordo,
                                                IConsultasDisciplina consultasDisciplina)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioDiarioBordo = repositorioDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioDiarioBordo));
            this.consultasDisciplina = consultasDisciplina ??
                throw new ArgumentNullException(nameof(consultasDisciplina));
        }

        public async Task<AuditoriaDto> Handle(AlterarDiarioBordoCommand request, CancellationToken cancellationToken)
        {
            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);
            var aula = await mediator.Send(new ObterAulaPorIdQuery(request.AulaId));

            if (aula.EhNulo())
                throw new NegocioException("Aula informada não existe");

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(aula.TurmaId));
            if (turma.EhNulo())
                throw new NegocioException("Turma informada não encontrada");

            if (usuario.EhProfessorCj())
            {
                var possuiAtribuicaoCJ = await mediator.Send(new PossuiAtribuicaoCJPorDreUeETurmaQuery(turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe, turma.CodigoTurma, usuario.CodigoRf));

                var atribuicoesEsporadica = await mediator.Send(new ObterAtribuicoesPorRFEAnoQuery(usuario.CodigoRf, false, aula.DataAula.Year, turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe));

                if (possuiAtribuicaoCJ && 
                    atribuicoesEsporadica.Any() &&
                    !atribuicoesEsporadica.Any(a => a.DataInicio <= aula.DataAula.Date && a.DataFim >= aula.DataAula.Date && a.DreId == turma.Ue.Dre.CodigoDre && a.UeId == turma.Ue.CodigoUe))
                    throw new NegocioException($"Você não possui permissão para alterar o registro de diário de bordo neste período");
            }

            var componenteCurricularPrincipalProfessor = await RetornaComponenteCurricularIdPrincipalDoProfessor(turma.CodigoTurma, request.ComponenteCurricularId);
            if (componenteCurricularPrincipalProfessor == 0)
                throw new NegocioException($"Componente Curricular não encontrado");

            var diarioBordo = await repositorioDiarioBordo.ObterPorAulaId(request.AulaId, componenteCurricularPrincipalProfessor);
            if (diarioBordo.EhNulo())
                throw new NegocioException($"Diário de Bordo para a aula {request.AulaId} não encontrado!");

            await MoverRemoverExcluidos(request, diarioBordo);
            
            MapearAlteracoes(diarioBordo, request, componenteCurricularPrincipalProfessor);

            await repositorioDiarioBordo.SalvarAsync(diarioBordo);

            return (AuditoriaDto)diarioBordo;
        }
        private async Task<long> RetornaComponenteCurricularIdPrincipalDoProfessor(string turmaCodigo, long componenteCurricularId)
        {
            var disciplinas = await consultasDisciplina.ObterComponentesCurricularesPorProfessorETurma(turmaCodigo, false, false, false);
            if (disciplinas != null && disciplinas.Any())
            {
                if (disciplinas.Count > 1)
                {
                    var disciplina = disciplinas.Where(b => b.CodigoComponenteCurricular == componenteCurricularId);

                    if (disciplina == null)
                        return 0;

                    if (disciplina.Any())
                        return disciplina.FirstOrDefault().CodigoComponenteCurricular;
                    else
                        return (long)disciplinas.FirstOrDefault().CdComponenteCurricularPai;
                }

                return disciplinas.FirstOrDefault().CodigoComponenteCurricular;
            }
            return 0;
        }

        private async Task MoverRemoverExcluidos(AlterarDiarioBordoCommand diario, DiarioBordo diarioBordo)
        {
            if (!string.IsNullOrEmpty(diario.Planejamento))
            {
                var moverArquivo = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.DiarioBordo, diarioBordo.Planejamento, diario.Planejamento));
                diario.Planejamento = moverArquivo;
            }
            if (!string.IsNullOrEmpty(diarioBordo.Planejamento))
            {
                await mediator.Send(new RemoverArquivosExcluidosCommand(diarioBordo.Planejamento, diario.Planejamento, TipoArquivo.DiarioBordo.Name()));
            }
        }
        private void MapearAlteracoes(DiarioBordo entidade, AlterarDiarioBordoCommand request, long componenteCurricularPrincipalProfessor)
        {
            entidade.Planejamento = request.Planejamento;
            entidade.ComponenteCurricularId = componenteCurricularPrincipalProfessor;
        }
    }
}
