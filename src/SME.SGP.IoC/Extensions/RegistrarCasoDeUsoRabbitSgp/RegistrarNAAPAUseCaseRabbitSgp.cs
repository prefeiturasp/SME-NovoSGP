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
            services.TryAddScoped<IAtualizarInformacoesDoAtendimentoNAAPAUseCase, AtualizarInformacoesDoEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IAtualizarTurmaDoAtendimentoNAAPAUseCase, AtualizarTurmaDoEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IAtualizarEnderecoDoAtendimentoNAAPAUseCase, AtualizarEnderecoDoEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<INotificarSobreInativacaoAlunoTurmaDoAtendimentoNAAPAUseCase, NotificarSobreInativacaoAlunoTurmaDoEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<INotificarSobreTransferenciaUeDreAlunoTurmaDoAtendimentoNAAPAUseCase, NotificarSobreTransferenciaUeDreAlunoTurmaDoEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IAtualizarTurmasProgramaDoAtendimentoNAAPAUseCase, AtualizarTurmasProgramaDoEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IExecutarCargaConsolidadoAtendimentoNAAPAUseCase, ExecutarCargaConsolidadoEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IExecutarBuscarUesConsolidadoAtendimentoNAAPAUseCase, ExecutarBuscarUesConsolidadoEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IExecutarInserirConsolidadoAtendimentoNAAPAUseCase, ExecutarInserirConsolidadoEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IExecutarBuscarConsolidadoAtendimentosProfissionalAtendimentoNAAPAUseCase, ExecutarBuscarConsolidadoAtendimentosProfissionalEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IExecutarInserirConsolidadoAtendimentoProfissionalAtendimentoNAAPAUseCase, ExecutarInserirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IExecutarExcluirConsolidadoAtendimentoNAAPAUseCase, ExecutarExcluirConsolidadoEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<IExecutarExcluirConsolidadoAtendimentoProfissionalAtendimentoNAAPAUseCase, ExecutarExcluirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase>();
            services.TryAddScoped<INotificarInatividadeDoAtendimentoNAAPAUseCase, NotificarInatividadeDoAtendimentoNAAPAUseCase>();
            services.TryAddScoped<INotificarInatividadeDoAtendimentoNAAPAPorUeUseCase, NotificarInatividadeDoAtendimentoNAAPAPorUeUseCase>();
            services.TryAddScoped<INotificarInatividadeDoAtendimentoNAAPAInformacaoUseCase, NotificarInatividadeDoAtendimentoNAAPAInformacaoUseCase>();
            services.TryAddScoped<IExecutarExclusaoNotificacaoInatividadeAtendimentoNAAPAUseCase, ExecutarExclusaoNotificacaoInatividadeAtendimentoNAAPAUseCase>();
        }
    }
}
