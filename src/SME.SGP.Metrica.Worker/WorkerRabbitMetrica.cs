using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;
using SME.SGP.Infra.Worker;
using SME.SGP.Metrica.Worker.Rotas;
using SME.SGP.Metrica.Worker.UseCases.Interfaces;

namespace SME.SGP.Metrica.Worker
{
    public class WorkerRabbitMetrica : WorkerRabbitSGP
    {
        public WorkerRabbitMetrica(IServiceScopeFactory serviceScopeFactory,
            IServicoTelemetria servicoTelemetria,
            IServicoMensageriaSGP servicoMensageria,
            IServicoMensageriaMetricas servicoMensageriaMetricas,
            IOptions<TelemetriaOptions> telemetriaOptions,
            IOptions<ConsumoFilasOptions> consumoFilasOptions,
            IConnectionFactory factory)
            : base(serviceScopeFactory, servicoTelemetria, servicoMensageria, servicoMensageriaMetricas, telemetriaOptions, consumoFilasOptions, factory, "WorkerRabbitMetrica", typeof(RotasRabbitMetrica), false)
        {
        }

        protected override void RegistrarUseCases()
        {
            Comandos.Add(RotasRabbitMetrica.AcessosSGP, new ComandoRabbit("Quantidade de Acessos Diario no SGP", typeof(IAcessosDiarioSGPUseCase)));
            Comandos.Add(RotasRabbitMetrica.DuplicacaoConselhoClasse, new ComandoRabbit("Registros de conselho de classe para o mesmo fechamento", typeof(IConselhoClasseDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.LimpezaConselhoClasseDuplicado, new ComandoRabbit("Limpeza de registros de conselho de classe para o mesmo fechamento", typeof(ILimpezaConselhoClasseDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.DuplicacaoConselhoClasseAluno, new ComandoRabbit("Registros de conselho de classe aluno para o mesmo conselho de classe", typeof(IConselhoClasseAlunoDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.DuplicacaoConselhoClasseAlunoUe, new ComandoRabbit("Registros de conselho de classe aluno para o mesmo conselho de classe na UE", typeof(IConselhoClasseAlunoUeDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.LimpezaConselhoClasseAlunoDuplicado, new ComandoRabbit("Limpeza de registros de conselho de classe aluno para o mesmo conselho de classe", typeof(ILimpezaConselhoClasseAlunoDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.DuplicacaoConselhoClasseNota, new ComandoRabbit("Registros de notas de conselho de classe para o mesmo aluno", typeof(IConselhoClasseNotaDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.LimpezaConselhoClasseNotaDuplicado, new ComandoRabbit("Limpeza de registros de notas de conselho de classe para o mesmo aluno", typeof(ILimpezaConselhoClasseNotaDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.DuplicacaoFechamentoTurma, new ComandoRabbit("Registros de fechamento para a mesma turma e periodo escolar", typeof(IFechamentoTurmaDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.LimpezaFechamentoTurmaDuplicado, new ComandoRabbit("Limpeza de registros de fechamento para a mesma turma e periodo escolar", typeof(ILimpezaFechamentoTurmaDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.DuplicacaoFechamentoTurmaDisciplina, new ComandoRabbit("Registros de fechamento disciplina para o mesmo fechamento", typeof(IFechamentoTurmaDisciplinaDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.LimpezaFechamentoTurmaDisciplinaDuplicado, new ComandoRabbit("Limpeza de registros de fechamento disciplina para o mesmo fechamento", typeof(ILimpezaFechamentoTurmaDisciplinaDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.DuplicacaoFechamentoAluno, new ComandoRabbit("Registros de fechamento aluno para o mesmo componente", typeof(IFechamentoAlunoDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.DuplicacaoFechamentoAlunoUE, new ComandoRabbit("Registros de fechamento aluno para o mesmo componente por UE", typeof(IFechamentoAlunoDuplicadoUEUseCase)));
            Comandos.Add(RotasRabbitMetrica.LimpezaFechamentoAlunoDuplicado, new ComandoRabbit("Limpeza de registros de fechamento aluno para o mesmo componente", typeof(ILimpezaFechamentoAlunoDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.DuplicacaoFechamentoNota, new ComandoRabbit("Registros de fechamento nota para o mesmo aluno", typeof(IFechamentoNotaDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.DuplicacaoFechamentoNotaTurma, new ComandoRabbit("Registros de fechamento nota para o mesmo aluno por turma", typeof(IFechamentoNotaDuplicadoTurmaUseCase)));
            Comandos.Add(RotasRabbitMetrica.LimpezaFechamentoNotaDuplicado, new ComandoRabbit("Limpeza de registros de fechamento nota para o mesmo aluno", typeof(ILimpezaFechamentoNotaDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.ConsolidacaoCCNotaNulo, new ComandoRabbit("Registros de consolidação de CC com nota e conceito nulos", typeof(IConsolidacaoConselhoClasseNotaNuloUseCase)));
            Comandos.Add(RotasRabbitMetrica.DuplicacaoConsolidacaoCCAlunoTurma, new ComandoRabbit("Registros de consolidação de aluno/turma duplicados", typeof(IConsolidacaoConselhoClasseAlunoTurmaDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.DuplicacaoConsolidacaoCCAlunoTurmaUE, new ComandoRabbit("Registros de consolidação de aluno/turma duplicados por UE", typeof(IConsolidacaoConselhoClasseAlunoTurmaDuplicadoUEUseCase)));
            Comandos.Add(RotasRabbitMetrica.LimpezaConsolidacaoCCAlunoTurmaDuplicado, new ComandoRabbit("Limpeza de registros de consolidação de aluno/turma duplicados por UE", typeof(ILimpezaConsolidacaoConselhoClasseAlunoTurmaDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.DuplicacaoConselhoClasseNota, new ComandoRabbit("Registros de consolidação de CC nota duplicados", typeof(IConsolidacaoCCNotaDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.LimpezaConsolidacaoCCNotaDuplicado, new ComandoRabbit("Limpeza de registros de consolidação de CC nota duplicados", typeof(ILimpezaConsolidacaoCCNotaDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.ConselhoClasseNaoConsolidado, new ComandoRabbit("Fechamento ou Conselho de Classe que não gerou consolidação", typeof(IConselhoClasseNaoConsolidadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.ConselhoClasseNaoConsolidadoUE, new ComandoRabbit("Fechamento ou Conselho de Classe que não gerou consolidação por UE", typeof(IConselhoClasseNaoConsolidadoUEUseCase)));
            Comandos.Add(RotasRabbitMetrica.FrequenciaAlunoInconsistente, new ComandoRabbit("Registro frequencia_aluno com relação ao numero de aulas e presenças dos alunos", typeof(IFrequenciaAlunoInconsistenteUseCase)));
            Comandos.Add(RotasRabbitMetrica.FrequenciaAlunoInconsistenteUE, new ComandoRabbit("Registro frequencia_aluno com relação ao numero de aulas e presenças dos alunos por UE", typeof(IFrequenciaAlunoInconsistenteUEUseCase)));
            Comandos.Add(RotasRabbitMetrica.FrequenciaAlunoInconsistenteTurma, new ComandoRabbit("Registro frequencia_aluno com relação ao numero de aulas e presenças dos alunos por Turma", typeof(IFrequenciaAlunoInconsistenteTurmaUseCase)));
            Comandos.Add(RotasRabbitMetrica.DuplicacaoFrequenciaAluno, new ComandoRabbit("Registro frequencia_aluno duplicados para o mesmo aluno, turma e bimestre", typeof(IFrequenciaAlunoDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.DuplicacaoFrequenciaAlunoUE, new ComandoRabbit("Registro frequencia_aluno duplicados para o mesmo aluno, turma e bimestre por UE", typeof(IFrequenciaAlunoDuplicadoUEUseCase)));
            Comandos.Add(RotasRabbitMetrica.LimpezaFrequenciaAlunoDuplicado, new ComandoRabbit("Limpeza de registro frequencia_aluno duplicados para o mesmo aluno, turma e bimestre", typeof(ILimpezaFrequenciaAlunoDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.DuplicacaoRegistroFrequencia, new ComandoRabbit("Registro de registro_frequencia duplicados para mesma aula", typeof(IRegistroFrequenciaDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.DuplicacaoRegistroFrequenciaUE, new ComandoRabbit("Registro de registro_frequencia duplicados para mesma aula por UE", typeof(IRegistroFrequenciaDuplicadoUEUseCase)));
            Comandos.Add(RotasRabbitMetrica.LimpezaRegistroFrequenciaDuplicado, new ComandoRabbit("Limpeza de registro_frequencia duplicados para mesma aula", typeof(ILimpezaRegistroFrequenciaDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.DuplicacaoRegistroFrequenciaAluno, new ComandoRabbit("Registro de registro_frequencia_aluno duplicados para mesmo registro_frequencia", typeof(IRegistroFrequenciaAlunoDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.DuplicacaoRegistroFrequenciaAlunoUE, new ComandoRabbit("Registro de registro_frequencia_aluno duplicados para mesmo registro_frequencia por UE", typeof(IRegistroFrequenciaAlunoDuplicadoUEUseCase)));
            Comandos.Add(RotasRabbitMetrica.DuplicacaoRegistroFrequenciaAlunoTurma, new ComandoRabbit("Registro de registro_frequencia_aluno duplicados para mesmo registro_frequencia por Turma", typeof(IRegistroFrequenciaAlunoDuplicadoTurmaUseCase)));
            Comandos.Add(RotasRabbitMetrica.LimpezaRegistroFrequenciaAlunoDuplicado, new ComandoRabbit("Limpeza de registro_frequencia_aluno duplicados para mesmo registro_frequencia", typeof(ILimpezaRegistroFrequenciaAlunoDuplicadoUseCase)));
            Comandos.Add(RotasRabbitMetrica.ConsolidacaoFrequenciaAlunoMensalInconsistente, new ComandoRabbit("Registro de inconsistencia em calculo de consolidação mensal de frequencia com relação aos numeros de aula e ausencias existentes", typeof(IConsolidacaoFrequenciaAlunoMensalInconsistenteUseCase)));
            Comandos.Add(RotasRabbitMetrica.ConsolidacaoFrequenciaAlunoMensalInconsistenteUE, new ComandoRabbit("Registro de inconsistencia em calculo de consolidação mensal de frequencia com relação aos numeros de aula e ausencias existentes", typeof(IConsolidacaoFrequenciaAlunoMensalInconsistenteUEUseCase)));
            Comandos.Add(RotasRabbitMetrica.ConsolidacaoFrequenciaAlunoMensalInconsistenteTurma, new ComandoRabbit("Registro de inconsistencia em calculo de consolidação mensal de frequencia com relação aos numeros de aula e ausencias existentes", typeof(IConsolidacaoFrequenciaAlunoMensalInconsistenteTurmaUseCase)));
        }
    }
}
