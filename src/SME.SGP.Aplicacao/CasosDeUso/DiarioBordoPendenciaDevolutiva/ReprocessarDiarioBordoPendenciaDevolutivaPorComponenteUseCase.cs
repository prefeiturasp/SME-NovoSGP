using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ReprocessarDiarioBordoPendenciaDevolutivaPorComponenteUseCase : AbstractUseCase, IReprocessarDiarioBordoPendenciaDevolutivaPorComponenteUseCase
    {
        private readonly IConsultasTurma repositorioTurma;
        private readonly IRepositorioPendenciaDevolutiva repositorioPendenciaDevolutiva;
        public ReprocessarDiarioBordoPendenciaDevolutivaPorComponenteUseCase(IMediator mediator, IConsultasTurma repositorioTurma, IRepositorioPendenciaDevolutiva repositorioPendenciaDevolutiva) : base(mediator)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioPendenciaDevolutiva = repositorioPendenciaDevolutiva ?? throw new ArgumentNullException(nameof(repositorioPendenciaDevolutiva));
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            try
            {
                var filtro = param.ObterObjetoMensagem<FiltroDiarioBordoPendenciaDevolutivaDto>();
                var componentes = await repositorioPendenciaDevolutiva.ObterCodigoComponenteComDiarioBordoSemDevolutiva(filtro.TurmaId, filtro.UeCodigo);

                foreach (var componente in componentes)
                {
                    var pendenciaDevolutivaCriada = await repositorioPendenciaDevolutiva.ObterPendenciasDevolutivaPorTurmaComponente(filtro.TurmaId, long.Parse(componente));
                    if (!pendenciaDevolutivaCriada.Any())
                    {
                        var existeDiarioSemDevolutiva = await mediator.Send(new ExistePendenciaDiarioBordoQuery(filtro.TurmaId, componente));
                        if (existeDiarioSemDevolutiva)
                        {
                            var dadosTurma = await ObterDadosTurma(filtro.TurmaId);

                            string descricaoComponente = await ObterDescricaoComponente(componente);

                            long pendenciaId = await GerarPedencia(dadosTurma.Ue.Id, dadosTurma, descricaoComponente);

                            await SalvarPendenciaDevolutiva(pendenciaId, componente, dadosTurma.Id);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível executar a verificação de pendencias de devolutivas por Componente", LogNivel.Critico, LogContexto.Devolutivas, ex.Message));
                return false;
            }
        }

        private async Task<string> ObterDescricaoComponente(string componenteCodigo)
              => await mediator.Send(new ObterDescricaoComponenteCurricularPorIdQuery(long.Parse(componenteCodigo)));

        private async Task<long> GerarPedencia(long ueId, Turma dadosTurma, string descricaoComponente)
        {
            var tituloPendencia = ObterTituloPendencia(dadosTurma, descricaoComponente);
            var descricaoPendencia = ObterDescricaoPendencia(dadosTurma, descricaoComponente);
            long pendenciaId = await mediator.Send(new SalvarPendenciaCommand(TipoPendencia.Devolutiva, ueId, dadosTurma.Id, descricao: descricaoPendencia, titulo: tituloPendencia, instrucao: ObterInstrucaoPendencia()));
            await mediator.Send(new SalvarPendenciaPerfilCommand(pendenciaId, ObterCodigoPerfis()));
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpPendencias.RotaTratarAtribuicaoPendenciaUsuarios, new FiltroTratamentoAtribuicaoPendenciaDto(pendenciaId, ueId), Guid.NewGuid()));

            return pendenciaId;
        }

        private static string ObterInstrucaoPendencia()
            => "Esta pendência será resolvida automaticamente quando o registro da devolutiva for regularizado.";

        private static string ObterTituloPendencia(Turma dadosTurma, string descricaoComponente)
            => $"Devolutiva - {dadosTurma.ObterEscola()} - {descricaoComponente}";

        private static string ObterDescricaoPendencia(Turma dadosTurma, string descricaoComponente)
            => $"O componente {descricaoComponente} da turma {dadosTurma.NomeComModalidade()} da {dadosTurma.ObterEscola()} está há mais de 25 dias sem registro de devolutiva para os diários de bordo.";

        private async Task SalvarPendenciaDevolutiva(long pendenciaId, string componenteCodigo, long turmaId)
        {
            var pendenciaDevolutiva = new PendenciaDevolutiva
            {
                PedenciaId = pendenciaId,
                TurmaId = turmaId,
                ComponenteCurricularId = long.Parse(componenteCodigo)
            };
            await repositorioPendenciaDevolutiva.Salvar(pendenciaDevolutiva);
        }
        private async Task<Turma> ObterDadosTurma(long turmaId)
            => await repositorioTurma.ObterComUeDrePorId(turmaId);

        private static List<PerfilUsuario> ObterCodigoPerfis()
         => new() { PerfilUsuario.CP };
    }
}
