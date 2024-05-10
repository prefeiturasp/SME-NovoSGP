using MediatR;
using Microsoft.Extensions.Options;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarAcompanhamentoAlunoUseCase : AbstractUseCase, ISalvarAcompanhamentoAlunoUseCase
    {
        private readonly IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions;

        public SalvarAcompanhamentoAlunoUseCase(IMediator mediator, IOptions<ConfiguracaoArmazenamentoOptions> configuracaoArmazenamentoOptions) : base(mediator)
        {
            this.configuracaoArmazenamentoOptions = configuracaoArmazenamentoOptions ?? throw new ArgumentNullException(nameof(configuracaoArmazenamentoOptions));
        }

        public async Task<AcompanhamentoAlunoSemestreAuditoriaDto>


            Executar(AcompanhamentoAlunoDto acompanhamentoAlunoDto)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(acompanhamentoAlunoDto.TurmaId));
            var parametroQuantidadeImagens = await ObterQuantidadeLimiteImagens(turma.AnoLetivo);

            if (acompanhamentoAlunoDto.PercursoIndividual.ExcedeuQuantidadeImagensPermitidas(parametroQuantidadeImagens))
                throw new NegocioException(String.Format(MensagemAcompanhamentoTurma.QUANTIDADE_DE_IMAGENS_PERMITIDAS_EXCEDIDA, parametroQuantidadeImagens));

            if (turma.EhNulo())
                throw new NegocioException(MensagensNegocioFrequencia.Turma_informada_nao_foi_encontrada);

            var bimestre = acompanhamentoAlunoDto.Semestre == 1 ? 2 : 4;

            var dataAtual = DateTimeExtension.HorarioBrasilia().Date;
            var temPeriodoAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, dataAtual, bimestre, turma.AnoLetivo == dataAtual.Year));

            if (!temPeriodoAberto)
                throw new NegocioException(MensagemNegocioComuns.APENAS_EH_POSSIVEL_CONSULTAR_ESTE_REGISTRO_POIS_O_PERIODO_NAO_ESTA_EM_ABERTO);

            var aluno = await mediator.Send(new ObterAlunoPorCodigoEolQuery(acompanhamentoAlunoDto.AlunoCodigo, turma.AnoLetivo, turma.Historica, false, turma.CodigoTurma));
            if (aluno.EhNulo())
                throw new NegocioException(MensagemNegocioAluno.ESTUDANTE_NAO_ENCONTRADO);

            if (aluno.EstaInativo(dataAtual))
                throw new NegocioException(MensagemNegocioAluno.ESTUDANTE_INATIVO);

            if (acompanhamentoAlunoDto.TextoSugerido)
                await CopiarArquivo(acompanhamentoAlunoDto);

            var acompanhamentoSemestre = await MapearAcompanhamentoSemestre(acompanhamentoAlunoDto);

            return (AcompanhamentoAlunoSemestreAuditoriaDto)acompanhamentoSemestre;
        }

        private async Task<int> ObterQuantidadeLimiteImagens(int ano)
        {
            var parametroQuantidade = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.QuantidadeImagensPercursoIndividualCrianca, ano));
            return parametroQuantidade.EhNulo() ?
                0 : int.Parse(parametroQuantidade.Valor);
        }

        private async Task CopiarArquivo(AcompanhamentoAlunoDto acompanhamentoAluno)
        {
            var imagens = UtilRegex.RegexTagsIMG.Matches(acompanhamentoAluno.PercursoIndividual);
            if (imagens.NaoEhNulo())
                foreach (var imagem in imagens)
                {
                    var nomeArquivo = UtilRegex.RegexNomesArquivosUUID.Match(imagem.ToString());
                    if (!ImagemJaExistente(imagem.ToString()) && ImagemExisteTemp(imagem.ToString()))
                    {
                        var novoCaminho = nomeArquivo.Success ? await mediator.Send(new MoverArquivoCommand(nomeArquivo.ToString(), TipoArquivo.AcompanhamentoAluno)) : string.Empty;
                        if (!string.IsNullOrEmpty(novoCaminho))
                        {
                            var str = acompanhamentoAluno.PercursoIndividual.Replace(configuracaoArmazenamentoOptions.Value.BucketTemp, configuracaoArmazenamentoOptions.Value.BucketArquivos);
                            acompanhamentoAluno.PercursoIndividual = str;
                        }
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
            else
                entidade.PercursoIndividual = percursoIndividual;

            if (!string.IsNullOrEmpty(observacoes))
            {
                var moverArquivoObservacoes = await mediator.Send(new MoverArquivosTemporariosCommand(TipoArquivo.AcompanhamentoAluno, entidade.Observacoes, observacoes));
                entidade.Observacoes = moverArquivoObservacoes;
            }
            else
                entidade.Observacoes = observacoes;

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

        private bool ImagemJaExistente(string imagem)
        => imagem.Contains($@"/{configuracaoArmazenamentoOptions.Value.BucketArquivos}/");
        private bool ImagemExisteTemp(string imagem)
        => imagem.Contains($@"/{configuracaoArmazenamentoOptions.Value.BucketTemp}");
    }
}
