using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Usuario = SME.SGP.Dominio.Usuario;

namespace SME.SGP.Aplicacao
{
    public class InserirFrequenciasAulaCommandHandler : IRequestHandler<InserirFrequenciasAulaCommand, FrequenciaAuditoriaAulaDto>
    {
        private readonly IMediator mediator;

        public InserirFrequenciasAulaCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<FrequenciaAuditoriaAulaDto> Handle(InserirFrequenciasAulaCommand request, CancellationToken cancellationToken)
        {
            var alunos = request.Frequencia.ListaFrequencia.Select(a => a.CodigoAluno).ToList();

            if (alunos == null || !alunos.Any())
                throw new NegocioException(MensagensNegocioFrequencia.Lista_de_alunos_e_o_componente_devem_ser_informados);

            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery(), cancellationToken);
            var aula = await mediator.Send(new ObterAulaPorIdQuery(request.Frequencia.AulaId), cancellationToken);

            if (aula == null)
                throw new NegocioException(MensagensNegocioFrequencia.A_aula_informada_nao_foi_encontrada);

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(aula.TurmaId), cancellationToken);
            
            if (turma == null)
                throw new NegocioException(MensagensNegocioFrequencia.Turma_informada_nao_foi_encontrada);

            if (usuario.EhProfessorCj())
            {
                var possuiAtribuicaoCJ = await mediator.Send(new PossuiAtribuicaoCJPorDreUeETurmaQuery(turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe, turma.CodigoTurma, usuario.CodigoRf), cancellationToken);

                var atribuicoesEsporadica = (await mediator.Send(new ObterAtribuicoesPorRFEAnoQuery(usuario.CodigoRf, false, aula.DataAula.Year, turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe), cancellationToken))
                    .ToList();

                if (possuiAtribuicaoCJ && atribuicoesEsporadica.Any())
                {
                    if (!atribuicoesEsporadica.Any(a => a.DataInicio <= aula.DataAula.Date && a.DataFim >= aula.DataAula.Date && a.DreId == turma.Ue.Dre.CodigoDre && a.UeId == turma.Ue.CodigoUe))
                        throw new NegocioException(MensagensNegocioFrequencia.Nao_possui_permissão_para_inserir_neste_periodo);
                }
            }

            if (!aula.PermiteRegistroFrequencia(turma))
                throw new NegocioException(MensagensNegocioFrequencia.Nao_e_permitido_registro_de_frequencia_para_este_componente);

            if (!usuario.EhGestorEscolar())
            {
                ValidaSeUsuarioPodeCriarAula(aula, usuario);
                await ValidaProfessorPodePersistirTurmaDisciplina(aula.TurmaId, usuario, aula.DisciplinaId, aula.DataAula, false);
            }
            
            var mesmoAnoLetivo = DateTimeExtension.HorarioBrasilia().Year == aula.DataAula.Year;
            var bimestreAula = await mediator.Send(new ObterBimestreAtualQuery(aula.DataAula, turma), cancellationToken);
            
            var temPeriodoAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTimeExtension.HorarioBrasilia(),
                bimestreAula, mesmoAnoLetivo), cancellationToken);
            
            if (!temPeriodoAberto)
                throw new NegocioException(MensagemNegocioComuns.APENAS_EH_POSSIVEL_CONSULTAR_ESTE_REGISTRO_POIS_O_PERIODO_NAO_ESTA_EM_ABERTO);

            var registroFrequencia = await mediator.Send(new ObterRegistroFrequenciaPorAulaIdQuery(aula.Id), cancellationToken);

            var alteracaoRegistro = registroFrequencia != null;
            
            registroFrequencia ??= new RegistroFrequencia(aula);
            registroFrequencia.Id = await mediator.Send(new PersistirRegistroFrequenciaCommand(registroFrequencia), cancellationToken);

            var registrouComSucesso = await mediator.Send(new InserirRegistrosFrequenciasAlunosCommand(request.Frequencia.ListaFrequencia, registroFrequencia.Id, turma.Id,
                long.Parse(aula.DisciplinaId),aula.Id, aula.DataAula), cancellationToken);

            if (registrouComSucesso)
            {
                // Quando for alteração de registro de frequencia chama o servico para verificar se atingiu o limite de dias para alteração e notificar
                if (alteracaoRegistro)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.NotificacaoFrequencia, registroFrequencia, Guid.NewGuid(), usuario), cancellationToken);

                if (request.CalcularFrequencia)
                    await mediator.Send(new IncluirFilaCalcularFrequenciaPorTurmaCommand(alunos, aula.DataAula, aula.TurmaId, aula.DisciplinaId), cancellationToken);
                
                await mediator.Send(new ExcluirPendenciaAulaCommand(aula.Id, TipoPendencia.Frequencia), cancellationToken);

                await mediator.Send(new IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommand(turma.Id, aula.DataAula), cancellationToken);
                //Fazer chamada para atualização semanal e mensal - D2

                return new FrequenciaAuditoriaAulaDto() { Auditoria = (AuditoriaDto)registroFrequencia, DataAula = aula.DataAula, TurmaId = aula.TurmaId, DisciplinaId = aula.DisciplinaId };
            }

            return new FrequenciaAuditoriaAulaDto() { AulaIdComErro = aula.Id, DataAulaComErro = aula.DataAula };
        }

        private void ValidaSeUsuarioPodeCriarAula(Aula aula, Usuario usuario)
        {
            if (!usuario.PodeRegistrarFrequencia(aula))
            {
                throw new NegocioException(MensagensNegocioFrequencia.Nao_e_possível_registrar_a_frequência_o_componente_nao_permite_substituicao);
            }
        }

        private async Task ValidaProfessorPodePersistirTurmaDisciplina(string turmaId, Usuario usuario, string disciplinaId, DateTime dataAula, bool historico)
        {
            var podePersistirTurma = await mediator.Send(new VerificaPodePersistirTurmaDisciplinaQuery(usuario, turmaId, disciplinaId, dataAula.Local(), historico));

            if (!usuario.EhProfessorCj() && !podePersistirTurma)
                throw new NegocioException(MensagemNegocioComuns.Voce_nao_pode_fazer_alteracoes_ou_inclusoes_nesta_turma_componente_e_data);
        }
    }
}
