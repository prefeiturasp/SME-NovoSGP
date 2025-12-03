using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;

namespace SME.SGP.IoC
{
    internal static partial class RegistrarCasoDeUsoRabbitSgp
    {
        internal static void RegistrarNAAPAUseCaseRabbitSgp(this IServiceCollection services)
        {
            services.TryAddScoped<IAtualizarInformacoesDoAtendimentoNAAPAUseCase, AtualizarInformacoesDoAtendimentoNAAPAUseCase>();
            services.TryAddScoped<IAtualizarTurmaDoAtendimentoNAAPAUseCase, AtualizarTurmaDoAtendimentoNAAPAUseCase>();
            services.TryAddScoped<IAtualizarEnderecoDoAtendimentoNAAPAUseCase, AtualizarEnderecoDoAtendimentoNAAPAUseCase>();
            services.TryAddScoped<INotificarSobreInativacaoAlunoTurmaDoAtendimentoNAAPAUseCase, NotificarSobreInativacaoAlunoTurmaDoAtendimentoNAAPAUseCase>();
            services.TryAddScoped<INotificarSobreTransferenciaUeDreAlunoTurmaDoAtendimentoNAAPAUseCase, NotificarSobreTransferenciaUeDreAlunoTurmaDoAtendimentoNAAPAUseCase>();
            services.TryAddScoped<IAtualizarTurmasProgramaDoAtendimentoNAAPAUseCase, AtualizarTurmasProgramaDoAtendimentoNAAPAUseCase>();
            services.TryAddScoped<IExecutarCargaConsolidadoAtendimentoNAAPAUseCase, ExecutarCargaConsolidadoAtendimentoNAAPAUseCase>();
            services.TryAddScoped<IExecutarBuscarUesConsolidadoAtendimentoNAAPAUseCase, ExecutarBuscarUesConsolidadoAtendimentoNAAPAUseCase>();
            services.TryAddScoped<IExecutarInserirConsolidadoAtendimentoNAAPAUseCase, ExecutarInserirConsolidadoAtendimentoNAAPAUseCase>();
            services.TryAddScoped<IExecutarBuscarConsolidadoAtendimentosProfissionalAtendimentoNAAPAUseCase, ExecutarBuscarConsolidadoAtendimentosProfissionalAtendimentoNAAPAUseCase>();
            services.TryAddScoped<IExecutarInserirConsolidadoAtendimentoProfissionalAtendimentoNAAPAUseCase, ExecutarInserirConsolidadoAtendimentoProfissionalAtendimentoNAAPAUseCase>();
            services.TryAddScoped<IExecutarExcluirConsolidadoAtendimentoNAAPAUseCase, ExecutarExcluirConsolidadoAtendimentoNAAPAUseCase>();
            services.TryAddScoped<IExecutarExcluirConsolidadoAtendimentoProfissionalAtendimentoNAAPAUseCase, ExecutarExcluirConsolidadoAtendimentoProfissionalAtendimentoNAAPAUseCase>();
            services.TryAddScoped<INotificarInatividadeDoAtendimentoNAAPAUseCase, NotificarInatividadeDoAtendimentoNAAPAUseCase>();
            services.TryAddScoped<INotificarInatividadeDoAtendimentoNAAPAPorUeUseCase, NotificarInatividadeDoAtendimentoNAAPAPorUeUseCase>();
            services.TryAddScoped<INotificarInatividadeDoAtendimentoNAAPAInformacaoUseCase, NotificarInatividadeDoAtendimentoNAAPAInformacaoUseCase>();
            services.TryAddScoped<IExecutarExclusaoNotificacaoInatividadeAtendimentoNAAPAUseCase, ExecutarExclusaoNotificacaoInatividadeAtendimentoNAAPAUseCase>();
        }
    }
}
