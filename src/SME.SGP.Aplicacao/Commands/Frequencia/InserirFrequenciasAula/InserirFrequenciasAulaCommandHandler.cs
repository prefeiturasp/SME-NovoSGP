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

namespace SME.SGP.Aplicacao
{
    public class InserirFrequenciasAulaCommandHandler : IRequestHandler<InserirFrequenciasAulaCommand, AuditoriaDto>
    {
        private readonly IMediator mediator;

        public InserirFrequenciasAulaCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<AuditoriaDto> Handle(InserirFrequenciasAulaCommand request, CancellationToken cancellationToken)
        {
            var alunos = request.Frequencia.ListaFrequencia.Select(a => a.CodigoAluno).ToList();
            if (alunos == null || !alunos.Any())
                throw new NegocioException(MensagensNegocioFrequencia.Lista_de_alunos_e_o_componente_devem_ser_informados);

            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());
            var aula = await mediator.Send(new ObterAulaPorIdQuery(request.Frequencia.AulaId));

            if (aula == null)
                throw new NegocioException(MensagensNegocioFrequencia.A_aula_informada_nao_foi_encontrada);

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(aula.TurmaId));
            if (turma == null)
                throw new NegocioException(MensagensNegocioFrequencia.Turma_informada_nao_foi_encontrada);


            if (usuario.EhProfessorCj())
            {
                var possuiAtribuicaoCJ = await mediator.Send(new PossuiAtribuicaoCJPorDreUeETurmaQuery(turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe, turma.CodigoTurma, usuario.CodigoRf));

                var atribuicoesEsporadica = await mediator.Send(new ObterAtribuicoesPorRFEAnoQuery(usuario.CodigoRf, false, aula.DataAula.Year, turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe));

                if (possuiAtribuicaoCJ && atribuicoesEsporadica.Any())
                {
                    if (!atribuicoesEsporadica.Where(a => a.DataInicio <= aula.DataAula.Date && a.DataFim >= aula.DataAula.Date && a.DreId == turma.Ue.Dre.CodigoDre && a.UeId == turma.Ue.CodigoUe).Any())
                        throw new NegocioException(MensagensNegocioFrequencia.Nao_possui_permissão_para_inserir_neste_periodo);
                }
            }


            if (!aula.PermiteRegistroFrequencia(turma))
                throw new NegocioException(MensagensNegocioFrequencia.Nao_e_permitido_registro_de_frequencia_para_este_componente);


            if (!usuario.EhGestorEscolar())
            {
                ValidaSeUsuarioPodeCriarAula(aula, usuario);
                await ValidaProfessorPodePersistirTurmaDisciplina(aula.TurmaId, usuario, aula.DisciplinaId, aula.DataAula);
            }

            var registroFrequencia = await mediator.Send(new ObterRegistroFrequenciaPorAulaIdQuery(aula.Id));

            var alteracaoRegistro = registroFrequencia != null;
            if (registroFrequencia == null)
                registroFrequencia = new RegistroFrequencia(aula);

            registroFrequencia.Id = await mediator.Send(new PersistirRegistroFrequenciaCommand(registroFrequencia));
            await mediator.Send(new InserirRegistrosFrequenciasAlunosCommand(request.Frequencia.ListaFrequencia, registroFrequencia.Id, turma.Id, long.Parse(aula.DisciplinaId),aula.Id));

            // Quando for alteração de registro de frequencia chama o servico para verificar se atingiu o limite de dias para alteração e notificar
            if (alteracaoRegistro)
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.NotificacaoFrequencia, registroFrequencia, Guid.NewGuid(), usuario));
            }

            await mediator.Send(new IncluirFilaCalcularFrequenciaPorTurmaCommand(alunos, aula.DataAula, aula.TurmaId, aula.DisciplinaId));

            await mediator.Send(new ExcluirPendenciaAulaCommand(aula.Id, TipoPendencia.Frequencia));

            foreach (var tipo in Enum.GetValues(typeof(TipoPeriodoDashboardFrequencia)))
                await mediator.Send(new IncluirFilaConsolidarDashBoardFrequenciaCommand(turma.Id, aula.DataAula, (TipoPeriodoDashboardFrequencia)tipo));

            return await ObterAuditoriaParaRetorno(aula.Id);
        }

        private void ValidaSeUsuarioPodeCriarAula(Aula aula, Usuario usuario)
        {
            if (!usuario.PodeRegistrarFrequencia(aula))
            {
                throw new NegocioException(MensagensNegocioFrequencia.Nao_e_possível_registrar_a_frequência_o_componente_nao_permite_substituicao);
            }
        }

        private async Task<AuditoriaDto> ObterAuditoriaParaRetorno(long  aulaId)
        {
            var registroFrequencia = await mediator.Send(new ObterRegistroFrequenciaPorAulaIdQuery(aulaId));
            return (AuditoriaDto)registroFrequencia;
        }
        private async Task ValidaProfessorPodePersistirTurmaDisciplina(string turmaId, Usuario usuario, string disciplinaId, DateTime dataAula)
        {
            var podePersistirTurma = await mediator.Send(new VerificaPodePersistirTurmaDisciplinaQuery(usuario, turmaId, disciplinaId, dataAula.Local()));

            if (!usuario.EhProfessorCj() && !podePersistirTurma)
                throw new NegocioException("Você não pode fazer alterações ou inclusões nesta turma, componente curricular e data.");
        }
    }
}
