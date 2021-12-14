using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarAcompanhamentoAlunoUseCase : AbstractUseCase, ISalvarAcompanhamentoAlunoUseCase
    {
        public SalvarAcompanhamentoAlunoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<AcompanhamentoAlunoSemestreAuditoriaDto> Executar(AcompanhamentoAlunoDto dto)
        {
            if (dto.TextoSugerido)
                await CopiarArquivo(dto);

            var acompanhamentoSemestre = await MapearAcompanhamentoSemestre(dto);            

            return (AcompanhamentoAlunoSemestreAuditoriaDto)acompanhamentoSemestre;
        }

        private async Task CopiarArquivo(AcompanhamentoAlunoDto acompanhamentoAluno)
        {
            var imagens = Regex.Matches(acompanhamentoAluno.PercursoIndividual, "<img[^>]*>");
            if (imagens != null)
                foreach (var imagem in imagens)
                {
                    var nomeArquivo = Regex.Match(imagem.ToString(), @"[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}.[A-Za-z0-4]+");
                    var novoCaminho = await mediator.Send(new CopiarArquivoCommand(nomeArquivo.ToString(), TipoArquivo.RegistroIndividual, TipoArquivo.AcompanhamentoAluno));
                    if (!string.IsNullOrEmpty(novoCaminho))
                    {
                        var caminhoBase = UtilArquivo.ObterDiretorioBase();
                        var caminhoArquivoDestino = Path.Combine(caminhoBase, novoCaminho, nomeArquivo.ToString());                        
                        Regex.Replace(acompanhamentoAluno.PercursoIndividual, @$"https.+?{nomeArquivo.ToString()}", caminhoArquivoDestino);
                    }
                }
        }

        private async Task MoverRemoverExcluidosAlterar(string observacoes, string percursoIndividual, AcompanhamentoAlunoSemestre entidade)
        {
            string percursoIndividualAtual = entidade.PercursoIndividual;
            string observacoesAtual = entidade.Observacoes;
            if (!string.IsNullOrEmpty(percursoIndividual))
            {
                var moverArquivoPercursoIndividual = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.AcompanhamentoAluno, entidade.PercursoIndividual, percursoIndividual));
                entidade.PercursoIndividual = moverArquivoPercursoIndividual;
            }
            if (!string.IsNullOrEmpty(observacoes))
            {
                var moverArquivoObservacoes = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.AcompanhamentoAluno, entidade.Observacoes, observacoes));
                entidade.Observacoes = moverArquivoObservacoes;
            }

            if (!string.IsNullOrEmpty(percursoIndividualAtual))
            {
                await mediator.Send(new RemoverArquivosExcluidosCommand(percursoIndividualAtual, percursoIndividual, TipoArquivo.AcompanhamentoAluno.Name()));
            }
            if (!string.IsNullOrEmpty(observacoesAtual))
            {
                await mediator.Send(new RemoverArquivosExcluidosCommand(observacoesAtual, observacoes, TipoArquivo.AcompanhamentoAluno.Name()));
            }
        }
        private async Task MoverArquivosIncluir(AcompanhamentoAlunoDto dto)
        {
            if (!string.IsNullOrEmpty(dto.PercursoIndividual))
            {
                var percursoIndividual = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.AcompanhamentoAluno, string.Empty, dto.PercursoIndividual));
                dto.PercursoIndividual = percursoIndividual;
            }
            if (!string.IsNullOrEmpty(dto.Observacoes))
            {
                var observacoes = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.AcompanhamentoAluno, string.Empty, dto.Observacoes));
                dto.Observacoes = observacoes;
            }
        }
        private async Task<AcompanhamentoAlunoSemestre> MapearAcompanhamentoSemestre(AcompanhamentoAlunoDto dto)
        {
            var acompanhamentoSemestre = dto.AcompanhamentoAlunoSemestreId > 0 ?
                await AtualizaObservacoesAcompanhamento(dto.AcompanhamentoAlunoSemestreId, dto.Observacoes, dto.PercursoIndividual) :
                await GerarAcompanhamentoSemestre(dto);

            return acompanhamentoSemestre;
        }

        private async Task<AcompanhamentoAlunoSemestre> AtualizaObservacoesAcompanhamento(long acompanhamentoAlunoSemestreId, string observacoes, string percursoIndividual)
        {
            var acompanhamento = await ObterAcompanhamentoSemestrePorId(acompanhamentoAlunoSemestreId);
            await MoverRemoverExcluidosAlterar(observacoes, percursoIndividual, acompanhamento);
            return await mediator.Send(new SalvarAcompanhamentoAlunoSemestreCommand(acompanhamento));
        }

        private async Task<AcompanhamentoAlunoSemestre> ObterAcompanhamentoSemestrePorId(long acompanhamentoAlunoSemestreId)
            => await mediator.Send(new ObterAcompanhamentoAlunoSemestrePorIdQuery(acompanhamentoAlunoSemestreId));

        private async Task<AcompanhamentoAlunoSemestre> GerarAcompanhamentoSemestre(AcompanhamentoAlunoDto dto)
        {
            await MoverArquivosIncluir(dto);
            var acompanhamentoAlunoId = dto.AcompanhamentoAlunoId > 0 ?
                dto.AcompanhamentoAlunoId :
                await mediator.Send(new GerarAcompanhamentoAlunoCommand(dto.TurmaId, dto.AlunoCodigo));

            return await mediator.Send(new GerarAcompanhamentoAlunoSemestreCommand(acompanhamentoAlunoId, dto.Semestre, dto.Observacoes, dto.PercursoIndividual));
        }
    }
}
