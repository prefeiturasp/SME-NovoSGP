using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TrataSincronizacaoInstitucionalUeCommandHandler : IRequestHandler<TrataSincronizacaoInstitucionalUeCommand, bool>
    {
        private readonly IRepositorioUe repositorioUe;
        private readonly IRepositorioDreConsulta repositorioDre;
        private readonly IRepositorioCache repositorioCache;
        private readonly IMediator mediator;

        public TrataSincronizacaoInstitucionalUeCommandHandler(IRepositorioUe repositorioUe, IRepositorioDreConsulta repositorioDre, IRepositorioCache repositorioCache, IMediator mediator)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Handle(TrataSincronizacaoInstitucionalUeCommand request, CancellationToken cancellationToken)
        {
            if (request.UeSGP == null)
            {
                var dreDaUe = await ObterDadosDre(request) ?? 
                    throw new NegocioException($"Não foi possível localizar a Dre da Ue {request.UeEOL.UeCodigo}");
                
                var ueParaIncluir = new Ue()
                {
                    CodigoUe = request.UeEOL.UeCodigo,
                    Nome = request.UeEOL.UeNome.Trim(),
                    TipoEscola = (TipoEscola)request.UeEOL.TipoEscolaCodigo
                };

                ueParaIncluir.AdicionarDre(dreDaUe);

                var idNovaUe = await repositorioUe.IncluirAsync(ueParaIncluir);
                if (idNovaUe <= 0)
                    throw new NegocioException($"Erro ao incluir nova Ue {request.UeEOL.UeCodigo}");
            }
            else
            {
                if (VerificaSeTemAlteracao(request.UeEOL, request.UeSGP))
                {
                    var dreId = await mediator
                        .Send(new ObterDREIdPorCodigoQuery(request.UeEOL.DreCodigo.ToString()), cancellationToken);

                    var ueParaAtualizar = new Ue()
                    {
                        CodigoUe = request.UeEOL.UeCodigo,
                        Nome = request.UeEOL.UeNome.Trim(),
                        TipoEscola = (TipoEscola)request.UeEOL.TipoEscolaCodigo,
                        Id = request.UeSGP.Id,
                        DreId = dreId
                    };

                    await repositorioUe.AtualizarAsync(ueParaAtualizar);
                }
            }

            return true;
        }
        private async Task<Dre> ObterDadosDre(TrataSincronizacaoInstitucionalUeCommand request)
        {
            return await repositorioCache.ObterAsync(ObterChaveCodigoDre(request.UeEOL.DreCodigo.ToString()),
                async () => await repositorioDre.ObterPorCodigo(request.UeEOL.DreCodigo.ToString()), "Obter DRE por código");
        }
        private bool VerificaSeTemAlteracao(UeDetalhesParaSincronizacaoInstituicionalDto ueEOL, Ue ueSGP)
        {
            if (ueEOL.UeNome.Trim() != ueSGP.Nome.Trim() ||
                ueEOL.TipoEscolaCodigo != (int)ueSGP.TipoEscola ||
                ueEOL.DreCodigo != long.Parse(ueSGP.Dre.CodigoDre))
                return true;

            return false;
        }
        private string ObterChaveCodigoDre(string codigoDre)
        {
            return string.Format(NomeChaveCache.DRE_CODIGO, codigoDre);
        }
    }
}
