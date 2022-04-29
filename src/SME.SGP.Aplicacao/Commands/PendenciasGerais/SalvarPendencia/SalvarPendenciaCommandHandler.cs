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
                UeId = request.UeId
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
                    return $"O registro de frequência do componente {request.DescricaoComponenteCurricular} da turma {request.TurmaAnoComModalidade} da {request.DescricaoUeDre} das aulas abaixo está pendente:";
                case TipoPendencia.PlanoAula:
                    return $"As aulas abaixo do componente {request.DescricaoComponenteCurricular} da turma {request.TurmaAnoComModalidade} da {request.DescricaoUeDre} estão sem plano de aula registrado:";
                case TipoPendencia.DiarioBordo:
                    return $"O registro do Diário de Bordo do componente {request.DescricaoComponenteCurricular} da turma {request.TurmaAnoComModalidade} da {request.DescricaoUeDre} das aulas abaixo está pendente:";
                case TipoPendencia.Avaliacao:
                    return $"As avaliações abaixo do componente {request.DescricaoComponenteCurricular} da turma {request.TurmaAnoComModalidade} da {request.DescricaoUeDre} estão sem notas lançadas:";
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
