using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.PendenciasGerais.SalvarPendencia
{
    public class SalvarPendenciaCommandHandler : IRequestHandler<SalvarPendenciaCommand, long>
    {
        private readonly IRepositorioPendencia repositorioPendencia;

        public SalvarPendenciaCommandHandler(IRepositorioPendencia repositorioPendencia)
        {
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
        }

        public async Task<long> Handle(SalvarPendenciaCommand request, CancellationToken cancellationToken)
        {
            var pendencia = new Pendencia(request.TipoPendencia)
            {
                Titulo = ObterTitulo(request),
                Descricao = ObterDescricao(request),
                Instrucao = request.Instrucao,
                DescricaoHtml = request.DescricaoHtml,
                UeId = request.UeId,
                TurmaId = request.TurmaId
            };

            return await repositorioPendencia.SalvarAsync(pendencia);
        }

        private string ObterDescricao(SalvarPendenciaCommand request)
        {
            return string.IsNullOrEmpty(request.Descricao) ? ObterDescricaoPorTipo(request) : request.Descricao;
        }

        private string ObterTitulo(SalvarPendenciaCommand request)
        {
            return !string.IsNullOrEmpty(request.Titulo) ? request.Titulo : request.TipoPendencia.Name();
        }

        private string ObterDescricaoPorTipo(SalvarPendenciaCommand request)
        {
            switch (request.TipoPendencia)
            {
                case TipoPendencia.AvaliacaoSemNotaParaNenhumAluno:
                    return "";
                case TipoPendencia.AulasReposicaoPendenteAprovacao:
                    return "";
                case TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento:
                    return "";
                case TipoPendencia.AulasSemFrequenciaNaDataDoFechamento:
                    return "";
                case TipoPendencia.ResultadosFinaisAbaixoDaMedia:
                    return "";
                case TipoPendencia.AlteracaoNotaFechamento:
                    return "";
                case TipoPendencia.Frequencia:
                    return PendenciaConstants.ObterDescricaoPendenciaFrequencia(request.DescricaoComponenteCurricular, request.TurmaAnoComModalidade, request.DescricaoUeDre);                    
                case TipoPendencia.PlanoAula:
                    return PendenciaConstants.ObterDescricaoPendenciaPlanoAula(request.DescricaoComponenteCurricular, request.TurmaAnoComModalidade, request.DescricaoUeDre);
                case TipoPendencia.DiarioBordo:
                    return PendenciaConstants.ObterDescricaoPendenciaDiarioBordo(request.DescricaoComponenteCurricular, request.TurmaAnoComModalidade, request.DescricaoUeDre);
                case TipoPendencia.Avaliacao:
                    return PendenciaConstants.ObterDescricaoPendenciaAvaliacao(request.DescricaoComponenteCurricular, request.TurmaAnoComModalidade, request.DescricaoUeDre);
                case TipoPendencia.AulaNaoLetivo:
                    return "";
                case TipoPendencia.CalendarioLetivoInsuficiente:
                    return "";
                case TipoPendencia.CadastroEventoPendente:
                    return "";
                default:
                    return "";
            }
        }
    }
}
