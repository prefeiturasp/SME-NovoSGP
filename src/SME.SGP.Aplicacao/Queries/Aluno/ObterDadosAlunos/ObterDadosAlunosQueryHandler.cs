using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosAlunosQueryHandler : IRequestHandler<ObterDadosAlunosQuery, IEnumerable<AlunoDadosBasicosDto>>
    {
        private readonly IMediator mediator;

        public ObterDadosAlunosQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AlunoDadosBasicosDto>> Handle(ObterDadosAlunosQuery request, CancellationToken cancellationToken)
        {
            var dadosAlunos = await mediator.Send(new ObterAlunosPorTurmaQuery(request.TurmaCodigo));
            if (dadosAlunos == null || !dadosAlunos.Any())
                throw new NegocioException($"Não foram localizados dados dos alunos para turma {request.TurmaCodigo} no EOL para o ano letivo {request.AnoLetivo}");

            var dadosAlunosDto = new List<AlunoDadosBasicosDto>();

            foreach (var dadoAluno in dadosAlunos)
            {
                var dadosBasicos = (AlunoDadosBasicosDto)dadoAluno;

                dadosBasicos.TipoResponsavel = ObterTipoResponsavel(dadoAluno.TipoResponsavel);
                // se informado periodo escolar carrega marcadores no periodo
                if (request.PeriodoEscolar != null)
                    dadosBasicos.Marcador = ObterMarcadorAluno(dadoAluno, request.PeriodoEscolar);

                dadosAlunosDto.Add(dadosBasicos);
            }

            return dadosAlunosDto;
        }

        private string ObterTipoResponsavel(string tipoResponsavel)
        {
            switch (tipoResponsavel)
            {
                case "1":
                    {
                        return TipoResponsavel.Filicacao1.Name();
                    }
                case "2":
                    {
                        return TipoResponsavel.Filiacao2.Name();
                    }
                case "3":
                    {
                        return TipoResponsavel.ResponsavelLegal.Name();
                    }
                case "4":
                    {
                        return TipoResponsavel.ProprioEstudante.Name();
                    }
            }
            return TipoResponsavel.Filicacao1.ToString();
        }

        public MarcadorFrequenciaDto ObterMarcadorAluno(AlunoPorTurmaResposta aluno, PeriodoEscolar bimestre, bool ehInfantil = false)
        {
            MarcadorFrequenciaDto marcador = null;

            string dataSituacao = $"{aluno.DataSituacao.Day}/{aluno.DataSituacao.Month}/{aluno.DataSituacao.Year}";

            switch (aluno.CodigoSituacaoMatricula)
            {
                case SituacaoMatriculaAluno.Ativo:
                    // Macador "Novo" durante 15 dias se iniciou depois do inicio do bimestre
                    if ((aluno.DataSituacao > bimestre.PeriodoInicio) && (aluno.DataSituacao.AddDays(15) >= DateTime.Now.Date))
                        marcador = new MarcadorFrequenciaDto()
                        {
                            Tipo = TipoMarcadorFrequencia.Novo,
                            Descricao = $"{(ehInfantil ? "Criança Nova" : "Estudante Novo")}: Data da matrícula {dataSituacao}"
                        };
                    break;

                case SituacaoMatriculaAluno.Transferido:
                    var detalheEscola = aluno.Transferencia_Interna ?
                                        $"para escola {aluno.EscolaTransferencia} e turma {aluno.TurmaTransferencia}" :
                                        "para outras redes";

                    marcador = new MarcadorFrequenciaDto()
                    {
                        Tipo = TipoMarcadorFrequencia.Transferido,
                        Descricao = $"{(ehInfantil ? "Criança Transferida" : "Estudante Transferido")}: {detalheEscola} em {dataSituacao}"
                    };

                    break;

                case SituacaoMatriculaAluno.RemanejadoSaida:
                    marcador = new MarcadorFrequenciaDto()
                    {
                        Tipo = TipoMarcadorFrequencia.Remanejado,
                        Descricao = $"{(ehInfantil ? "Criança Remanejada" : "Estudante Remanejado")}: turma {aluno.TurmaRemanejamento} em {dataSituacao}"
                    };

                    break;

                case SituacaoMatriculaAluno.Desistente:
                case SituacaoMatriculaAluno.VinculoIndevido:
                case SituacaoMatriculaAluno.Falecido:
                case SituacaoMatriculaAluno.NaoCompareceu:
                case SituacaoMatriculaAluno.Deslocamento:
                case SituacaoMatriculaAluno.Cessado:
                case SituacaoMatriculaAluno.ReclassificadoSaida:
                    marcador = new MarcadorFrequenciaDto()
                    {
                        Tipo = TipoMarcadorFrequencia.Inativo,
                        Descricao = $"{(ehInfantil ? "Criança Inativa" : "Estudante Inativo")} em {dataSituacao}"
                    };

                    break;

                default:
                    break;
            }

            return marcador;
        }
    }
}
