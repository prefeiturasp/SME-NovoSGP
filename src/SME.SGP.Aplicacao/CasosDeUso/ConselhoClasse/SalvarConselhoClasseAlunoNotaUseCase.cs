using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarConselhoClasseAlunoNotaUseCase : ISalvarConselhoClasseAlunoNotaUseCase
    {
        private readonly IMediator mediator;
        private const int BIMESTRE_2 = 2;
        private const int BIMESTRE_4 = 4;

        public SalvarConselhoClasseAlunoNotaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ConselhoClasseNotaRetornoDto> Executar(SalvarConselhoClasseAlunoNotaDto dto)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(dto.CodigoTurma));

            if (turma == null)
                throw new NegocioException("Turma não encontrada");

            var ehAnoAnterior = turma.AnoLetivo != DateTime.Now.Year;

            var fechamentoTurma = await mediator.Send(new ObterFechamentoTurmaPorIdAlunoCodigoQuery(dto.FechamentoTurmaId,
                dto.CodigoAluno, ehAnoAnterior));

            var fechamentoTurmaDisciplina = new FechamentoTurmaDisciplina();
            var periodoEscolar = new PeriodoEscolar();

            if (fechamentoTurma == null)
            {
                if (!ehAnoAnterior)
                    throw new NegocioException("Não existe fechamento de turma para o conselho de classe");

                periodoEscolar =
                    await mediator.Send(
                        new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, dto.Bimestre));

                if (periodoEscolar == null && dto.Bimestre > 0)
                    throw new NegocioException("Período escolar não encontrado");

                fechamentoTurma = new FechamentoTurma()
                {
                    TurmaId = turma.Id,
                    Migrado = false,
                    PeriodoEscolarId = periodoEscolar?.Id,
                    Turma = turma,
                    PeriodoEscolar = periodoEscolar
                };

                fechamentoTurmaDisciplina = new FechamentoTurmaDisciplina()
                {
                    DisciplinaId = dto.ConselhoClasseNotaDto.CodigoComponenteCurricular
                };
            }
            else
            {
                if (fechamentoTurma.PeriodoEscolarId != null)
                    periodoEscolar =
                        await mediator.Send(new ObterPeriodoEscolarePorIdQuery(fechamentoTurma.PeriodoEscolarId.Value));

                fechamentoTurmaDisciplina = new FechamentoTurmaDisciplina()
                {
                    DisciplinaId = dto.ConselhoClasseNotaDto.CodigoComponenteCurricular
                };
            }

            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());

            await ValidarAtribuicaoUsuario(dto.ConselhoClasseNotaDto.CodigoComponenteCurricular, turma, periodoEscolar.PeriodoFim, usuario);

            var periodoReaberturaCorrespondente = await mediator.Send(new ObterFechamentoReaberturaPorDataTurmaQuery() { DataParaVerificar = DateTime.Now, TipoCalendarioId = periodoEscolar.TipoCalendarioId, UeId = turma.UeId });
            var alunos = await mediator.Send(new ObterAlunosPorTurmaEAnoLetivoQuery(fechamentoTurma.Turma.CodigoTurma));
            var alunoConselho = alunos.FirstOrDefault(x => x.CodigoAluno == dto.CodigoAluno);

            if (alunoConselho.CodigoSituacaoMatricula != SituacaoMatriculaAluno.Ativo)
            {
                if (alunoConselho.DataSituacao < periodoReaberturaCorrespondente.Inicio || alunoConselho.DataSituacao > periodoReaberturaCorrespondente.Fim)
                    throw new NegocioException(MensagemNegocioFechamentoNota.ALUNO_INATIVO_ANTES_PERIODO_REABERTURA);
            }

            await mediator.Send(new GravarFechamentoTurmaConselhoClasseCommand(
                fechamentoTurma, fechamentoTurmaDisciplina, periodoEscolar?.Bimestre));

            return await mediator.Send(new GravarConselhoClasseCommad(fechamentoTurma, dto.ConselhoClasseId, dto.CodigoAluno,
                dto.ConselhoClasseNotaDto, periodoEscolar?.Bimestre, usuario));
        }

        private async Task ValidarAtribuicaoUsuario(long componenteCurricularId, Turma turma, DateTime dataAula, Usuario usuarioLogado)
        {
            if (dataAula == DateTime.MinValue)
            {
                var periodoEscolar4Bimestre =  await mediator.Send(new ObterPeriodoEscolarPorTurmaBimestreQuery(turma, turma.ModalidadeTipoCalendario == ModalidadeTipoCalendario.EJA ? BIMESTRE_2 : BIMESTRE_4));
                dataAula = periodoEscolar4Bimestre.PeriodoFim;
            }
            
            var usuarioPossuiAtribuicaoNaTurmaNaData = await mediator.Send(new ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery(componenteCurricularId, turma.Id.ToString(), dataAula, usuarioLogado));
            if (!usuarioPossuiAtribuicaoNaTurmaNaData)
                throw new NegocioException(MensagensNegocioFrequencia.Nao_pode_fazer_alteracoes_anotacao_nesta_turma_componente_e_data);
        }
    }
}