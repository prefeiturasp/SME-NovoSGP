using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Commands.Aulas.InserirAula;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirFrequenciaUseCase : AbstractUseCase, IInserirFrequenciaUseCase
    {
        public InserirFrequenciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(FrequenciaDto param)
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

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(aula.TurmaId));
            if (turma == null)
                throw new NegocioException("Turma informada não foi encontrada");

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
            {
                registroFrequencia = new RegistroFrequencia(aula);
                registroFrequencia.Id = await mediator.Send(new InserirRegistroFrequenciaCommand(registroFrequencia));
            }

            await mediator.Send(new InserirRegistrosFrequenciasAlunosCommand(param.ListaFrequencia, registroFrequencia.Id));
            // Quando for alteração de registro de frequencia chama o servico para verificar se atingiu o limite de dias para alteração e notificar
            if (alteracaoRegistro)
                Background.Core.Cliente.Executar<IServicoNotificacaoFrequencia>(e => e.VerificaRegraAlteracaoFrequencia(registroFrequencia.Id, registroFrequencia.CriadoEm, DateTime.Now, usuario.Id));

            var bimestre = await mediator.Send(new ObterBimestrePorTurmaCodigoQuery(aula.TurmaId, aula.DataAula));

            //await mediator.Send(new IncluirFilaCalcularFrequenciaPorTurmaCommand(alunos, aula.DataAula, aula.TurmaId, aula.DisciplinaId, bimestre));

            await mediator.Send(new ExcluirPendenciaAulaCommand(aula.Id, TipoPendencia.Frequencia));

            return true;


        }

        private static List<RegistroAusenciaAluno> ObtemListaDeAusencias(FrequenciaDto frequenciaDto)
        {
            var registrosAusenciaAlunos = new List<RegistroAusenciaAluno>();

            foreach (var frequencia in frequenciaDto.ListaFrequencia.Where(c => c.Aulas.Any(a => !a.Compareceu)))
            {
                foreach (var ausenciaNaAula in frequencia.Aulas.Where(c => !c.Compareceu))
                {
                    registrosAusenciaAlunos.Add(new RegistroAusenciaAluno(frequencia.CodigoAluno, ausenciaNaAula.NumeroAula));
                }
            }

            return registrosAusenciaAlunos;
        }

        private void ValidaSeUsuarioPodeCriarAula(Aula aula, Usuario usuario)
        {
            if (!usuario.PodeRegistrarFrequencia(aula))
            {
                throw new NegocioException("Não é possível registrar a frequência pois esse componente curricular não permite substituição.");
            }
        }

        private async Task ValidaProfessorPodePersistirTurmaDisciplina(string turmaId, Usuario usuario, string disciplinaId, DateTime dataAula)
        {
            var podePersistirTurma = await mediator.Send(new VerificaPodePersistirTurmaDisciplinaQuery(usuario, turmaId, disciplinaId, dataAula.Local()));

            if (!usuario.EhProfessorCj() && !podePersistirTurma)
                throw new NegocioException("Você não pode fazer alterações ou inclusões nesta turma, componente curricular e data.");
        }
    }
}
