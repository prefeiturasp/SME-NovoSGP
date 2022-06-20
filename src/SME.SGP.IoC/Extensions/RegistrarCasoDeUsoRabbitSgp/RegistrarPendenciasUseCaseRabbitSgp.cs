using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.IoC
{
    internal partial class RegistrarCasoDeUsoRabbitSgp
    {
        internal static void RegistrarPendenciasUseCaseRabbitSgp(this IServiceCollection services)
        {
            services.TryAddScoped<IExecutaVerificacaoPendenciasGeraisUseCase, ExecutaVerificacaoPendenciasGeraisUseCase>();
            services.TryAddScoped<IExecutaVerificacaoPendenciasGeraisCalendarioUseCase, ExecutaVerificacaoPendenciasGeraisCalendarioUseCase>();
            services.TryAddScoped<IExecutaVerificacaoPendenciasGeraisEventosUseCase, ExecutaVerificacaoPendenciasGeraisEventosUseCase>();
            services.TryAddScoped<IExecutaVerificacaoGeracaoPendenciaProfessorAvaliacaoUseCase, ExecutaVerificacaoGeracaoPendenciaProfessorAvaliacaoUseCase>();
            services.TryAddScoped<ITratarAtribuicaoPendenciasUsuariosUseCase, TratarAtribuicaoPendenciasUsuariosUseCase>();
            services.TryAddScoped<ICargaAtribuicaoPendenciasPerfilUsuarioUseCase, CargaAtribuicaoPendenciasPerfilUsuarioUseCase>();
            services.TryAddScoped<IRemoverAtribuicaoPendenciasUsuariosUseCase, RemoverAtribuicaoPendenciasUsuariosUseCase>();
            services.TryAddScoped<IRemoverAtribuicaoPendenciasUsuariosUeUseCase, RemoverAtribuicaoPendenciasUsuariosUeUseCase>();
            services.TryAddScoped<IRemoverAtribuicaoPendenciasUsuariosUeFuncionarioUseCase, RemoverAtribuicaoPendenciasUsuariosUeFuncionarioUseCase>();

            #region ATENÇÃO - Use Cases injetados em processos consumidos pelo o worker
            // TODO
            services.TryAddScoped<IObterDataCriacaoRelatorioUseCase, ObterDataCriacaoRelatorioUseCase>();
            services.TryAddScoped<IRepositorioTipoRelatorio, RepositorioTipoRelatorio>();

            #endregion
        }
    }
}
