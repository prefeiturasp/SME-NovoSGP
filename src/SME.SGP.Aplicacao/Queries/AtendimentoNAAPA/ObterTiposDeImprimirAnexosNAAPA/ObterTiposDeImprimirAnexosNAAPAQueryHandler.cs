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
    public class ObterTiposDeImprimirAnexosNAAPAQueryHandler : IRequestHandler<ObterTiposDeImprimirAnexosNAAPAQuery, IEnumerable<ImprimirAnexoDto>>
    {
        private const string SECAO_INFORMACOES = "INFORMACOES_ESTUDANTE";
        private readonly IRepositorioRespostaEncaminhamentoNAAPA repositorio;

        public ObterTiposDeImprimirAnexosNAAPAQueryHandler(IRepositorioRespostaEncaminhamentoNAAPA repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<ImprimirAnexoDto>> Handle(ObterTiposDeImprimirAnexosNAAPAQuery request, CancellationToken cancellationToken)
        {
            var dicionario = new Dictionary<string, EnumImprimirAnexosNAAPA>()
            {
                { SECAO_INFORMACOES, EnumImprimirAnexosNAAPA.ApenasEncaminhamento },
                { AtendimentoNAAPAConstants.SECAO_ITINERANCIA, EnumImprimirAnexosNAAPA.ApenasAtendimentos }
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
