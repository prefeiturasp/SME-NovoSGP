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
            var pendencia = new Pendencia(request.TipoPendencia);
            pendencia.Titulo = string.IsNullOrEmpty(request.Titulo) ? ObterTitulo(request.TipoPendencia) : request.Titulo;
            pendencia.Descricao = string.IsNullOrEmpty(request.Descricao) ? ObterDescricao(request.TipoPendencia) : request.Descricao;
            pendencia.Instrucao = request.Instrucao;
            pendencia.DescricaoHtml = request.DescricaoHtml;
            pendencia.UeId = request.UeId > 0 ? request.UeId.Value : null;

            return await repositorioPendencia.SalvarAsync(pendencia);
        }

        private string ObterTitulo(TipoPendencia tipoPendencia) => tipoPendencia.Name();

        private string ObterDescricao(TipoPendencia tipoPendencia)
        {
            switch (tipoPendencia)
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
                    return "As seguintes aulas estão sem Frequência registradas";
                case TipoPendencia.PlanoAula:
                    return "As seguintes aulas estão sem Plano de Aula registrados:";
                case TipoPendencia.DiarioBordo:
                    return "As seguintes aulas estão sem Diario de Bordo registrados:";
                case TipoPendencia.Avaliacao:
                    return "As seguintes aulas estão sem Avaliação registradas:";
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
