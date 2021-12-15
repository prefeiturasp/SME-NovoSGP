using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirFrequenciaListaoUseCase : AbstractUseCase, IInserirFrequenciaListaoUseCase
    {
        public InserirFrequenciaListaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<RetornoBaseDto> Executar(IEnumerable<FrequenciaSalvarDto> param)
        {
            RetornoBaseDto retorno = new RetornoBaseDto();
            var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());
            var aula = param.FirstOrDefault().Aulas.Any()
                ? await mediator.Send(new ObterAulaPorIdQuery(param.FirstOrDefault().Aulas.FirstOrDefault().AulaId))
                : await mediator.Send(new ObterAulaPorIdQuery(param.FirstOrDefault().AulaId[0]));

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(aula.TurmaId));
            string disciplinaCodigo = aula.DisciplinaId;
            var listaDatasAulas = new List<DateTime>();

            if (param == null)
                throw new NegocioException("A lista de alunos do componente e turma devem ser informados para calcular a frequência.");

            try
            {
                foreach (var aluno in param)
                {
                    if (aluno.Aulas.Count() > 0)
                        await SalvaRegistroListaAulas(aluno.Aulas, aluno, aula, turma, usuario);                      
                    else
                        await SalvaRegistroParaTodasAulasDoDia(aluno, usuario);

                    if (!listaDatasAulas.Contains(aluno.DataAula))
                        listaDatasAulas.Add(aluno.DataAula);
                }

                foreach (var data in listaDatasAulas)
                    await mediator.Send(new IncluirFilaCalcularFrequenciaPorTurmaCommand(null, data, turma.CodigoTurma, disciplinaCodigo));

                retorno.Mensagens.Add("Frequências cadastradas com sucesso!");
            }
            catch (Exception)
            {
                retorno.Mensagens.Add("Ocorreu um erro ao cadastrar as frequências solicitadas, tente novamente.");
            }
 
            return retorno;
        }

        public async Task SalvaRegistroListaAulas(List<FrequenciaAulasDoDiaDto> aulas, FrequenciaSalvarDto aluno, Aula aula, Turma turma, Usuario usuario)
        {
            foreach (var alunoAula in aulas)
            {
                var valor = alunoAula.TipoFrequencia;
                aula = await mediator.Send(new ObterAulaPorIdQuery(alunoAula.AulaId));

                if (turma == null)
                    throw new NegocioException("Turma que a aula está registrada, não foi encontrada");

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

                if (!usuario.EhGestorEscolar())
                {
                    ValidaSeUsuarioPodeCriarAula(aula, usuario);
                    await ValidaProfessorPodePersistirTurmaDisciplina(aula.TurmaId, usuario, aula.DisciplinaId, aula.DataAula);
                }

                if (!aula.PermiteRegistroFrequencia(turma))
                    throw new NegocioException("Não é permitido registro de frequência para este componente curricular.");

                var registroFrequencia = await mediator.Send(new ObterRegistroFrequenciaPorAulaIdQuery(aula.Id));

                var alteracaoRegistro = registroFrequencia != null;
                if (registroFrequencia == null)
                    registroFrequencia = new RegistroFrequencia(aula);

                registroFrequencia.Id = await mediator.Send(new PersistirRegistroFrequenciaCommand(registroFrequencia));

                int tipoFrequencia = (int)Enum.GetValues(typeof(TipoFrequencia))
                        .Cast<TipoFrequencia>()
                        .Where(tf => tf.ShortName() == aluno.TipoFrequencia)
                        .FirstOrDefault();       

                var registroFrequenciaAluno = new RegistroFrequenciaAluno
                {
                    CodigoAluno = aluno.AlunoCodigo,
                    NumeroAula = alunoAula.NumeroAula,
                    Valor = tipoFrequencia,
                    RegistroFrequencia = registroFrequencia,
                    RegistroFrequenciaId = registroFrequencia.Id
                };

                await mediator.Send(new SalvarRegistroFrequenciaAlunoCommand(registroFrequenciaAluno));

                if (alteracaoRegistro)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.NotificacaoFrequencia, registroFrequencia, Guid.NewGuid(), usuario));
            }
        }
        public async Task SalvaRegistroParaTodasAulasDoDia(FrequenciaSalvarDto aluno, Usuario usuario)
        {
            int contadorAula = 0;
            foreach(var aulaId in aluno.AulaId)
            {
                var aula = await mediator.Send(new ObterAulaPorIdQuery(aulaId));
                var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(aula.TurmaId));
                if (turma == null)
                    throw new NegocioException("Turma que a aula está registrada, não foi encontrada");

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

                if (!usuario.EhGestorEscolar())
                {
                    ValidaSeUsuarioPodeCriarAula(aula, usuario);
                    await ValidaProfessorPodePersistirTurmaDisciplina(aula.TurmaId, usuario, aula.DisciplinaId, aula.DataAula);
                }

                if (!aula.PermiteRegistroFrequencia(turma))
                    throw new NegocioException("Não é permitido registro de frequência para este componente curricular.");

                var registroFrequencia = await mediator.Send(new ObterRegistroFrequenciaPorAulaIdQuery(aula.Id));

                var alteracaoRegistro = registroFrequencia != null;
                if (registroFrequencia == null)
                    registroFrequencia = new RegistroFrequencia(aula);

                registroFrequencia.Id = await mediator.Send(new PersistirRegistroFrequenciaCommand(registroFrequencia));

                int tipoFrequencia = (int)Enum.GetValues(typeof(TipoFrequencia))
                    .Cast<TipoFrequencia>()
                    .Where(tf => tf.ShortName() == aluno.TipoFrequencia)
                    .FirstOrDefault();

                var registroFrequenciaAluno = new RegistroFrequenciaAluno
                {
                    CodigoAluno = aluno.AlunoCodigo,
                    NumeroAula = aluno.NumeroAula[contadorAula],
                    Valor = tipoFrequencia,
                    RegistroFrequencia = registroFrequencia,
                    RegistroFrequenciaId = registroFrequencia.Id
                };

                await mediator.Send(new SalvarRegistroFrequenciaAlunoCommand(registroFrequenciaAluno));

                if (alteracaoRegistro)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.NotificacaoFrequencia, registroFrequencia, Guid.NewGuid(), usuario));

                contadorAula++;
            }
            
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
