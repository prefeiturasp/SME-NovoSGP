using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Relatorios;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RelatorioListagemOcorrenciasUseCase : AbstractUseCase, IRelatorioListagemOcorrenciasUseCase
    {
        public RelatorioListagemOcorrenciasUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(FiltroRelatorioListagemOcorrenciasDto param)
        {
            ValidarPreenchimento(param);

            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance) ?? throw new NegocioException("Não foi possível localizar o usuário.");
            param.NomeUsuario = usuario.Nome;
            param.CodigoRf = usuario.CodigoRf;

            return await mediator.Send(new GerarRelatorioCommand(TipoRelatorio.ListagemOcorrencias, param, usuario, rotaRelatorio: RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosListagemOcorrencias));
        }

        private static void ValidarPreenchimento(FiltroRelatorioListagemOcorrenciasDto param)
        {
            if (param.DataInicio.HasValue && !param.DataFim.HasValue)
                throw new NegocioException("Data final deve ser preenchida.");

            if (!param.DataInicio.HasValue && param.DataFim.HasValue)
                throw new NegocioException("Data inicio deve ser preenchida.");

            if (param.DataInicio.HasValue && param.DataFim.HasValue)
            {
                if (param.DataInicio.GetValueOrDefault().Year != param.AnoLetivo && param.DataFim.GetValueOrDefault().Year != param.AnoLetivo)
                    throw new NegocioException("As datas devem estar dentro do ano letivo selecionado.");

                if (param.DataInicio.Value > param.DataFim.Value)
                    throw new NegocioException("A data de início não pode ser maior que a data de fim.");

                if (param.DataFim.Value < param.DataFim.Value)
                    throw new NegocioException("A data de fim não pode ser menor que a data de início.");
            }
        }
    }
}
