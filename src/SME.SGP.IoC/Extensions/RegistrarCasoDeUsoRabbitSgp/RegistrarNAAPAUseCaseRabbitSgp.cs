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
            services.TryAddScoped<IAtualizarInformacoesDoEncaminhamentoNAAPAUseCase, AtualizarInformacoesDoEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IAtualizarTurmaDoEncaminhamentoNAAPAUseCase, AtualizarTurmaDoEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IAtualizarEnderecoDoEncaminhamentoNAAPAUseCase, AtualizarEnderecoDoEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<INotificarSobreInativacaoAlunoTurmaDoEncaminhamentoNAAPAUseCase, NotificarSobreInativacaoAlunoTurmaDoEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<INotificarSobreTransferenciaUeDreAlunoTurmaDoEncaminhamentoNAAPAUseCase, NotificarSobreTransferenciaUeDreAlunoTurmaDoEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IAtualizarTurmasProgramaDoEncaminhamentoNAAPAUseCase, AtualizarTurmasProgramaDoEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IExecutarCargaConsolidadoEncaminhamentoNAAPAUseCase, ExecutarCargaConsolidadoEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IExecutarBuscarUesConsolidadoEncaminhamentoNAAPAUseCase, ExecutarBuscarUesConsolidadoEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IExecutarInserirConsolidadoEncaminhamentoNAAPAUseCase, ExecutarInserirConsolidadoEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IExecutarBuscarConsolidadoAtendimentosProfissionalEncaminhamentoNAAPAUseCase, ExecutarBuscarConsolidadoAtendimentosProfissionalEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IExecutarInserirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase, ExecutarInserirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IExecutarExcluirConsolidadoEncaminhamentoNAAPAUseCase, ExecutarExcluirConsolidadoEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IExecutarExcluirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase, ExecutarExcluirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<INotificarInatividadeDoAtendimentoNAAPAUseCase, NotificarInatividadeDoAtendimentoNAAPAUseCase>();
            services.TryAddScoped<INotificarInatividadeDoAtendimentoNAAPAPorUeUseCase, NotificarInatividadeDoAtendimentoNAAPAPorUeUseCase>();
            services.TryAddScoped<INotificarInatividadeDoAtendimentoNAAPAInformacaoUseCase, NotificarInatividadeDoAtendimentoNAAPAInformacaoUseCase>();
            services.TryAddScoped<IExecutarExclusaoNotificacaoInatividadeAtendimentoNAAPAUseCase, ExecutarExclusaoNotificacaoInatividadeAtendimentoNAAPAUseCase>();
        }
    }
}
