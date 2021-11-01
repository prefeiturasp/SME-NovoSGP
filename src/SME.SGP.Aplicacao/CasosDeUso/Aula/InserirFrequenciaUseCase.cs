using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirFrequenciaUseCase : AbstractUseCase, IInserirFrequenciaUseCase
    {
        public InserirFrequenciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AuditoriaDto> Executar(FrequenciaDto param)
        {
            var alunos = param.ListaFrequencia.Select(a => a.CodigoAluno).ToList();
            if (alunos == null || !alunos.Any())
            {
                throw new NegocioException("A lista de alunos da turma e o componente curricular devem ser informados para calcular a frequência.");
            }

            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());
            var aula = await mediator.Send(new ObterAulaPorIdQuery(param.AulaId));

            if (aula == null)
                throw new NegocioException("A aula informada não foi encontrada");

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(aula.TurmaId));
            if (turma == null)
                throw new NegocioException("Turma informada não foi encontrada");


            if (usuario.EhProfessorCj())
            {
                var possuiAtribuicaoCJ = await mediator.Send(new PossuiAtribuicaoCJPorDreUeETurmaQuery(turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe, turma.CodigoTurma, usuario.CodigoRf));

                var atribuicoesEsporadica = await mediator.Send(new ObterAtribuicoesPorRFEAnoQuery(usuario.CodigoRf, false, aula.DataAula.Year, turma.Ue.Dre.CodigoDre, turma.Ue.CodigoUe));

                if (possuiAtribuicaoCJ && atribuicoesEsporadica.Any())
                {
                    if (!atribuicoesEsporadica.Where(a => a.DataInicio <= aula.DataAula.Date && a.DataFim >= aula.DataAula.Date && a.DreId == turma.Ue.Dre.CodigoDre && a.UeId == turma.Ue.CodigoUe).Any())
                        throw new NegocioException($"Você não possui permissão para inserir registro de frequência neste período");
                }
            }


            if (!aula.PermiteRegistroFrequencia(turma))
            {
                throw new NegocioException("Não é permitido registro de frequência para este componente curricular.");
            }


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
            await mediator.Send(new InserirRegistrosFrequenciasAlunosCommand(param.ListaFrequencia, registroFrequencia.Id, turma.Id, long.Parse(aula.DisciplinaId)));

            // Quando for alteração de registro de frequencia chama o servico para verificar se atingiu o limite de dias para alteração e notificar
            if (alteracaoRegistro)
                Background.Core.Cliente.Executar<IServicoNotificacaoFrequencia>(e => e.VerificaRegraAlteracaoFrequencia(registroFrequencia.Id, registroFrequencia.CriadoEm, DateTime.Now, usuario.Id));

            await mediator.Send(new IncluirFilaCalcularFrequenciaPorTurmaCommand(alunos, aula.DataAula, aula.TurmaId, aula.DisciplinaId));

            await mediator.Send(new ExcluirPendenciaAulaCommand(aula.Id, TipoPendencia.Frequencia));

            return (AuditoriaDto)registroFrequencia;


        }

        private void ValidaSeUsuarioPodeCriarAula(Aula aula, Usuario usuario)
        {
            if (!usuario.PodeRegistrarFrequencia(aula))
            {
                //throw new NegocioException("Não é possível registrar a frequência pois esse componente curricular não permite substituição.");
            }
        }

        private async Task ValidaProfessorPodePersistirTurmaDisciplina(string turmaId, Usuario usuario, string disciplinaId, DateTime dataAula)
        {
            var podePersistirTurma = await mediator.Send(new VerificaPodePersistirTurmaDisciplinaQuery(usuario, turmaId, disciplinaId, dataAula.Local()));

            //if (!usuario.EhProfessorCj() && !podePersistirTurma)
            //    throw new NegocioException("Você não pode fazer alterações ou inclusões nesta turma, componente curricular e data.");
        }
    }
}
