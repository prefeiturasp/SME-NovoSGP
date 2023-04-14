﻿using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterAtividadesNotasAlunoPorTurmaPeriodoUseCase : AbstractUseCase, IObterAtividadesNotasAlunoPorTurmaPeriodoUseCase
    {
        private readonly IMediator mediator;

        public ObterAtividadesNotasAlunoPorTurmaPeriodoUseCase(IMediator mediator) : base(mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AvaliacaoNotaAlunoDto>> Executar(FiltroTurmaAlunoPeriodoEscolarDto param)
        {
            var retorno =  (await mediator.Send(new ObterAtividadesNotasAlunoPorTurmaPeriodoQuery(param.TurmaId, param.PeriodoEscolarId, param.AlunoCodigo, param.ComponenteCurricular))).ToList();
            
            await CarregarDisciplinasDeRegencia(retorno, long.Parse(param.ComponenteCurricular), param.TurmaId);
            retorno = await ObterAusencia(param, retorno);

            return retorno;
        }

        private async Task CarregarDisciplinasDeRegencia(
                                    IEnumerable<AvaliacaoNotaAlunoDto> avaliacoesNotas, 
                                    long codigoComponenteCurricular,
                                    long turmaId)
        {
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(turmaId));
            var componentesCurricularesCompletos = await mediator.Send(new ObterComponentesCurricularesPorIdsUsuarioLogadoQuery(new long[] { codigoComponenteCurricular }, codigoTurma: turma.CodigoTurma));
            if (componentesCurricularesCompletos == null || !componentesCurricularesCompletos.Any())
                throw new NegocioException(MensagemNegocioEOL.COMPONENTE_CURRICULAR_NAO_LOCALIZADO_INFORMACOES_EOL);

            var componenteReferencia = componentesCurricularesCompletos.FirstOrDefault(a => a.CodigoComponenteCurricular == codigoComponenteCurricular);

            if (componenteReferencia.Regencia)
            {
                var turmaCompleta = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(turmaId));

                if (turmaCompleta == null)
                    throw new NegocioException(MensagemNegocioTurma.NAO_FOI_POSSIVEL_OBTER_TURMA);

                foreach (var avaliacao in avaliacoesNotas)
                {
                    var atividadeDisciplinas = await ObterDisciplinasAtividadeAvaliativa(avaliacao.Id, avaliacao.Regencia);
                    var idsDisciplinas = atividadeDisciplinas?.Select(a => long.Parse(a.DisciplinaId)).ToArray();
                    IEnumerable<DisciplinaDto> disciplinas;
                    if (idsDisciplinas != null && idsDisciplinas.Any())
                        disciplinas = await ObterDisciplinasPorIds(idsDisciplinas);
                    else
                    {
                        disciplinas = await mediator.Send(new ObterComponentesCurricularesPorProfessorETurmaParaPlanejamentoQuery(
                                                                                            componenteReferencia.CodigoComponenteCurricular,
                                                                                            turmaCompleta.CodigoTurma,
                                                                                            turmaCompleta.TipoTurma == TipoTurma.Programa,
                                                                                            componenteReferencia.Regencia));
                    }
                    var nomesDisciplinas = disciplinas?.Select(d => d.Nome).ToArray();
                    avaliacao.Disciplinas = nomesDisciplinas;
                }
            }
        }

        public async Task<IEnumerable<AtividadeAvaliativaDisciplina>> ObterDisciplinasAtividadeAvaliativa(long avaliacao_id, bool ehRegencia)
        {
            return await mediator.Send(new ObterDisciplinasAtividadeAvaliativaQuery(avaliacao_id, ehRegencia));
        }
        public async Task<IEnumerable<DisciplinaDto>> ObterDisciplinasPorIds(long[] ids)
        {
            return await mediator.Send(new ObterDisciplinasPorIdsQuery(ids));
        }

        private async Task<List<AvaliacaoNotaAlunoDto>> ObterAusencia(FiltroTurmaAlunoPeriodoEscolarDto request, List<AvaliacaoNotaAlunoDto> listAtividades)
        {
            var retorno = new List<AvaliacaoNotaAlunoDto>();
            var datasDasAtividadesAvaliativas = listAtividades.Select(x => x.Data).ToArray();
            var turma = await mediator.Send(new ObterTurmaPorIdQuery(request.TurmaId));
            
            if(turma == null)
                throw new NegocioException("Turma não encontrada");

            var ausenciasDasAtividadesAvaliativas = (await mediator.Send(new ObterAusenciasDaAtividadesAvaliativasPorAlunoQuery(turma.CodigoTurma, datasDasAtividadesAvaliativas, request.ComponenteCurricular, request.AlunoCodigo))).ToList();

            foreach (var atividade in listAtividades)
            {
                var ausente = ausenciasDasAtividadesAvaliativas
                    .Any(a => a.AlunoCodigo == request.AlunoCodigo && a.AulaData.Date == atividade.Data.Date);

                atividade.Ausente = ausente;
                retorno.Add(atividade);
            }

            return retorno;
        }
    }
}
