using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoSituacaoUseCase : AbstractUseCase, IObterFechamentoSituacaoUseCase
    {
        public ObterFechamentoSituacaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<GraficoBaseDto>> Executar(FiltroDashboardFechamentoDto param)
        {
            var fechamentosRetorno = await mediator.Send(new ObterFechamentoSituacaoQuery(param.UeId, param.AnoLetivo,
                param.DreId,
                param.Modalidade,
                param.Semestre,
                param.Bimestre));

            List<GraficoBaseDto> fechamentos = new List<GraficoBaseDto>();

            bool filtrouTodos = param.DreId == 0 || param.UeId == 0;

            foreach (var fechamento in fechamentosRetorno)
            {
                var grupo = filtrouTodos ? $"{fechamento.Ano}" : $"{fechamento.AnoTurma}";

                if(fechamento.Quantidade > 0)
                {
                    GraficoBaseDto fechamentoExistente = fechamentos?.FirstOrDefault(f => f.Grupo == grupo && f.Descricao == fechamento.Situacao.Name());

                    if (fechamentoExistente.NaoEhNulo())
                    {
                        int quantidadeAtual = fechamentos.FirstOrDefault(f => f.Grupo == grupo && f.Descricao == fechamento.Situacao.Name()).Quantidade;

                        var novoValorConsolidado = new GraficoBaseDto()
                        {
                            Grupo = grupo,
                            Quantidade = quantidadeAtual + fechamento.Quantidade,
                            Descricao = fechamento.Situacao.Name()
                        };

                        fechamentos.RemoveAt(fechamentos.IndexOf(fechamentoExistente));

                        fechamentos.Add(novoValorConsolidado);
                    }
                    else
                        fechamentos.Add(new GraficoBaseDto(grupo, fechamento.Quantidade, fechamento.Situacao.Name()));
                }
                    
            }

            return fechamentos.OrderBy(a => a.Grupo).ToList();
        }
    }
}