using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterItensDeImprimirAnexosNAAPAQueryHandler : IRequestHandler<ObterItensDeImprimirAnexosNAAPAQuery, IEnumerable<ImprimirAnexoDto>>
    {
        private const string SECAO_ATENDIMENTO = "QUESTOES_ITINERACIA";
        private const string SECAO_INFORMACOES = "INFORMACOES_ESTUDANTE";
        private readonly IRepositorioRespostaEncaminhamentoNAAPA repositorio;

        public ObterItensDeImprimirAnexosNAAPAQueryHandler(IRepositorioRespostaEncaminhamentoNAAPA repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<ImprimirAnexoDto>> Handle(ObterItensDeImprimirAnexosNAAPAQuery request, CancellationToken cancellationToken)
        {
            var dicionario = new Dictionary<string, EnumImprimirAnexosNAAPA>()
            {
                { SECAO_INFORMACOES, EnumImprimirAnexosNAAPA.ApenasEncaminhamento },
                { SECAO_ATENDIMENTO, EnumImprimirAnexosNAAPA.ApenasAtendimentos }
            };
            var itensImprimerAnexo = new List<ImprimirAnexoDto>() { ObterDto(EnumImprimirAnexosNAAPA.Nao) };
            var nomesComponentes = await repositorio.ObterNomesComponenteSecaoComAnexosEmPdf(request.EncaminhamentoId);

            foreach (var nomeComponente in nomesComponentes)
                itensImprimerAnexo.Add(ObterDto(dicionario[nomeComponente]));

            if (itensImprimerAnexo.Count > 2)
                itensImprimerAnexo.Add(ObterDto(EnumImprimirAnexosNAAPA.EncaminhamentoAtendimentos));

            return itensImprimerAnexo;
        }

        private ImprimirAnexoDto ObterDto(EnumImprimirAnexosNAAPA enumImprimirAnexos)
        {
            return new ImprimirAnexoDto() { Id = (int)enumImprimirAnexos, Descricao = enumImprimirAnexos.GetAttribute<DisplayAttribute>().Name };
        }
    }
}
