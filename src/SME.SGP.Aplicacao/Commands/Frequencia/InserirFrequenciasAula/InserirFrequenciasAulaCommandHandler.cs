using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Usuario = SME.SGP.Dominio.Usuario;

namespace SME.SGP.Aplicacao
{
    public class InserirFrequenciasAulaCommandHandler : IRequestHandler<InserirFrequenciasAulaCommand, FrequenciaAuditoriaAulaDto>
    {
        private readonly IMediator mediator;
        private InserirFrequenciasAulaCommand requestParams;

        public InserirFrequenciasAulaCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<FrequenciaAuditoriaAulaDto> Handle(InserirFrequenciasAulaCommand request, CancellationToken cancellationToken)
        {
            requestParams = request ?? throw new ArgumentNullException(nameof(request));
            (Usuario usuario, Aula aula, Turma turma, IEnumerable<string> CodigosAlunos) = await ValidarParametros(cancellationToken);

            if (usuario.EhProfessorCj())
                await ValidarPermissaoProfessorCJ(usuario, aula, turma, cancellationToken);

            if (!aula.PermiteRegistroFrequencia(turma))
                throw new NegocioException(MensagensNegocioFrequencia.Nao_e_permitido_registro_de_frequencia_para_este_componente);

            if (!usuario.EhGestorEscolar())
            {
                ValidaSeUsuarioPodeCriarAula(aula, usuario);
                await ValidaProfessorPodePersistirTurmaDisciplina(aula.TurmaId, usuario, aula.DisciplinaId, aula.DataAula, false);
            }

            await ValidarPeriodoEscolarAberto(aula, turma, cancellationToken);

            var registroFrequencia = await mediator.Send(new ObterRegistroFrequenciaPorAulaIdQuery(aula.Id), cancellationToken);
            var alteracaoRegistro = registroFrequencia.NaoEhNulo();
            
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
                    await mediator.Send(new IncluirFilaCalcularFrequenciaPorTurmaCommand(CodigosAlunos, aula.DataAula, aula.TurmaId, aula.DisciplinaId), cancellationToken);
                
                await mediator.Send(new ExcluirPendenciaAulaCommand(aula.Id, TipoPendencia.Frequencia), cancellationToken);
                await mediator.Send(new IncluirFilaConsolidacaoDiariaDashBoardFrequenciaCommand(turma.Id, aula.DataAula), cancellationToken);              
                await mediator.Send(new IncluirFilaConsolidacaoSemanalMensalDashBoardFrequenciaCommand(turma.Id, turma.CodigoTurma, turma.ModalidadeCodigo == Modalidade.EducacaoInfantil, turma.AnoLetivo, aula.DataAula), cancellationToken);

                return new FrequenciaAuditoriaAulaDto() { Auditoria = (AuditoriaDto)registroFrequencia, DataAula = aula.DataAula, TurmaId = aula.TurmaId, DisciplinaId = aula.DisciplinaId };
            }

            return new FrequenciaAuditoriaAulaDto() { AulaIdComErro = aula.Id, DataAulaComErro = aula.DataAula };
        }

        private async Task ValidarPeriodoEscolarAberto(Aula aula, Turma turma, CancellationToken cancellationToken)
        {
            var mesmoAnoLetivo = DateTimeExtension.HorarioBrasilia().Year == aula.DataAula.Year;
            var bimestreAula = await mediator.Send(new ObterBimestreAtualQuery(aula.DataAula, turma), cancellationToken);

            var temPeriodoAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTimeExtension.HorarioBrasilia(),
                bimestreAula, mesmoAnoLetivo), cancellationToken);
            if (!temPeriodoAberto)
                throw new NegocioException(MensagemNegocioComuns.APENAS_EH_POSSIVEL_CONSULTAR_ESTE_REGISTRO_POIS_O_PERIODO_NAO_ESTA_EM_ABERTO);
        }

        private async Task ValidarPermissaoProfessorCJ(Usuario usuario, Aula aula, Turma turma, CancellationToken cancellationToken)
        {
            var possuiAtribuicaoCJ = await mediator.Send(new PossuiAtribuicaoCJPorDreUeETurmaQuery(turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe, turma.CodigoTurma, usuario.CodigoRf), cancellationToken);
            var atribuicoesEsporadica = (await mediator.Send(new ObterAtribuicoesPorRFEAnoQuery(usuario.CodigoRf, false, aula.DataAula.Year, turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe), cancellationToken))
                .ToList();

            if (possuiAtribuicaoCJ &&
                atribuicoesEsporadica.Any() &&
                !atribuicoesEsporadica.Any(a => a.DataInicio <= aula.DataAula.Date && a.DataFim >= aula.DataAula.Date && a.DreId == turma.Ue.Dre.CodigoDre && a.UeId == turma.Ue.CodigoUe))
                throw new NegocioException(MensagensNegocioFrequencia.Nao_possui_permissão_para_inserir_neste_periodo);
        }

        private async Task<(Usuario Usuario, Aula Aula, Turma Turma, IEnumerable<string> CodigosAlunos)> ValidarParametros(CancellationToken cancellationToken)
        {
            var alunos = requestParams.Frequencia.ListaFrequencia.Select(a => a.CodigoAluno).ToList();
            if (alunos.NaoPossuiRegistros())
                throw new NegocioException(MensagensNegocioFrequencia.Lista_de_alunos_e_o_componente_devem_ser_informados);
            var usuario = await ObterUsuario(cancellationToken);
            if (usuario.EhNulo())
                throw new NegocioException(string.Format(MensagensNegocioFrequencia.Nao_foi_localizado_usuario_pelo_login, requestParams.UsuarioLogin));

            var aula = await mediator.Send(new ObterAulaPorIdQuery(requestParams.Frequencia.AulaId), cancellationToken);
            if (aula.EhNulo())
                throw new NegocioException(MensagensNegocioFrequencia.A_aula_informada_nao_foi_encontrada);

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(aula.TurmaId), cancellationToken);
            if (turma.EhNulo())
                throw new NegocioException(MensagensNegocioFrequencia.Turma_informada_nao_foi_encontrada);

            return (usuario, aula, turma, alunos);
        }

        private async Task<Usuario> ObterUsuario(CancellationToken cancellationToken)
            => !string.IsNullOrWhiteSpace(requestParams.UsuarioLogin) ? 
                await mediator.Send(new ObterUsuarioPorCodigoRfLoginQuery(null, requestParams.UsuarioLogin), cancellationToken) : 
                await mediator.Send(ObterUsuarioLogadoQuery.Instance, cancellationToken);
        private static void ValidaSeUsuarioPodeCriarAula(Aula aula, Usuario usuario)
        {
            if (!usuario.PodeRegistrarFrequencia(aula))
                throw new NegocioException(MensagensNegocioFrequencia.Nao_e_possível_registrar_a_frequência_o_componente_nao_permite_substituicao);
        }

        private async Task ValidaProfessorPodePersistirTurmaDisciplina(string turmaId, Usuario usuario, string disciplinaId, DateTime dataAula, bool historico)
        {
            var podePersistirTurma = await mediator.Send(new VerificaPodePersistirTurmaDisciplinaQuery(usuario, turmaId, disciplinaId, dataAula.Local(), historico));

            if (!usuario.EhProfessorCj() && !podePersistirTurma)
                throw new NegocioException(MensagemNegocioComuns.Voce_nao_pode_fazer_alteracoes_ou_inclusoes_nesta_turma_componente_e_data);
        }
    }
}
