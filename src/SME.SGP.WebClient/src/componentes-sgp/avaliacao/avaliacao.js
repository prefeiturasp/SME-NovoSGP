import { Tooltip } from 'antd';
import PropTypes from 'prop-types';
import React, { createRef, useRef } from 'react';
import ReactDOM from 'react-dom';
import { useDispatch, useSelector } from 'react-redux';
import shortid from 'shortid';
import { LabelSemDados } from '~/componentes';
import notasConceitos from '~/dtos/notasConceitos';
import {
  setModoEdicaoGeral,
  setModoEdicaoGeralNotaFinal,
  setExpandirLinha,
} from '~/redux/modulos/notasConceitos/actions';
import NomeEstudanteLista from '../NomeEstudanteLista/nomeEstudanteLista';
import {
  acharItem,
  converterAcaoTecla,
  esperarMiliSegundos,
  moverCursor,
  tratarString,
} from '~/utils';

import Ordenacao from '../Ordenacao/ordenacao';
import SinalizacaoAEE from '../SinalizacaoAEE/sinalizacaoAEE';
import {
  CaixaMarcadores,
  InfoMarcador,
  TabelaColunasFixas,
} from './avaliacao.css';
import CampoConceito from './campoConceito';
import CampoConceitoFinal from './campoConceitoFinal';
import CampoNota from './campoNota';
import CampoNotaFinal from './campoNotaFinal';
import ColunaNotaFinalRegencia from './colunaNotaFinalRegencia';
import LinhaConceitoFinal from './linhaConceitoFinal';

const Avaliacao = props => {
  const dispatch = useDispatch();

  const {
    dados,
    notaTipo,
    onChangeOrdenacao,
    desabilitarCampos,
    ehProfessorCj,
    ehRegencia,
    disciplinaSelecionada,
  } = props;

  const expandirLinha = useSelector(
    store => store.notasConceitos.expandirLinha
  );

  const onChangeNotaConceito = (nota, valorNovo) => {
    if (!desabilitarCampos && nota.podeEditar && valorNovo !== null) {
      nota.notaConceito = valorNovo;
      nota.modoEdicao = true;
      dados.modoEdicao = true;
      dispatch(setModoEdicaoGeral(true));
    }
  };

  const onChangeNotaConceitoFinal = (notaBimestre, valorNovo) => {
    if (!desabilitarCampos && valorNovo !== null) {
      notaBimestre.notaConceito = valorNovo;
      notaBimestre.modoEdicao = true;
      dados.modoEdicao = true;
      dispatch(setModoEdicaoGeralNotaFinal(true));
    }
  };

  const descricaoAlunoAusente = 'Aluno ausente na data da avaliação';

  const montarCabecalhoAvaliacoes = () => {
    return dados.avaliacoes && dados.avaliacoes.length > 0
      ? dados.avaliacoes.map(avaliacao => {
          return (
            <th key={shortid.generate()} className="width-150">
              <div className="texto-header-avaliacao">
                <Tooltip title={avaliacao.nome}>{avaliacao.nome}</Tooltip>
              </div>
              <div className="texto-header-avaliacao">
                {window.moment(avaliacao.data).format('DD/MM/YYYY')}
              </div>
            </th>
          );
        })
      : '';
  };

  const montarToolTipDisciplinas = disciplinas => {
    let nomes = '';
    disciplinas.forEach(nomeDisciplina => {
      nomes += nomes.length > 0 ? `, ${nomeDisciplina}` : nomeDisciplina;
    });
    return nomes;
  };

  const montarCabecalhoInterdisciplinar = () => {
    return dados.avaliacoes && dados.avaliacoes.length > 0
      ? dados.avaliacoes.map(avaliacao => {
          return avaliacao.ehInterdisciplinar ? (
            <th key={shortid.generate()}>
              <Tooltip
                title={montarToolTipDisciplinas(avaliacao.disciplinas)}
                placement="bottom"
                overlayStyle={{ fontSize: '12px' }}
              >
                <CaixaMarcadores>Interdisciplinar</CaixaMarcadores>
              </Tooltip>
            </th>
          ) : (
            <th key={shortid.generate()} />
          );
        })
      : '';
  };

  const acharElemento = (e, elemento) => {
    return e.nativeEvent.path.find(item => item.localName === elemento);
  };

  const clicarSetas = (e, aluno, label = '', index = 0, regencia = false) => {
    const direcao = converterAcaoTecla(e.keyCode);
    const disciplina = label.toLowerCase();

    if (direcao && regencia) {
      let novaLinha = [];
      const novoIndex = index + direcao;
      if (expandirLinha[novoIndex]) {
        expandirLinha[novoIndex] = false;
        novaLinha = expandirLinha;
      } else {
        novaLinha[novoIndex] = true;
      }
      dispatch(setExpandirLinha([...novaLinha]));
    }
    const elementoTD = acharElemento(e, 'td');
    const indexElemento = elementoTD?.cellIndex - 2;
    const alunoEscolhido =
      direcao && acharItem(dados?.alunos, aluno, direcao, 'id');
    if (alunoEscolhido.length) {
      const disciplinaTratada = tratarString(disciplina);
      const item = regencia ? disciplinaTratada : 'aluno';
      const itemEscolhido = `${item}${alunoEscolhido[0].id}`;
      moverCursor(itemEscolhido, indexElemento, regencia);
    }
  };

  const montarCampoNotaConceito = (nota, aluno) => {
    const avaliacao = dados.avaliacoes.find(
      item => item.id === nota.atividadeAvaliativaId
    );
    const desabilitarNota = ehProfessorCj ? !avaliacao.ehCJ : avaliacao.ehCJ;

    switch (Number(notaTipo)) {
      case Number(notasConceitos.Notas):
        return (
          <CampoNota
            esconderSetas
            name={`aluno${aluno.id}`}
            clicarSetas={e => clicarSetas(e, aluno)}
            step={0}
            nota={nota}
            onChangeNotaConceito={valorNovo =>
              onChangeNotaConceito(nota, valorNovo)
            }
            desabilitarCampo={desabilitarCampos || desabilitarNota}
            mediaAprovacaoBimestre={dados.mediaAprovacaoBimestre}
          />
        );
      case Number(notasConceitos.Conceitos):
        return (
          <CampoConceito
            nota={nota}
            onChangeNotaConceito={valorNovo =>
              onChangeNotaConceito(nota, valorNovo)
            }
            desabilitarCampo={desabilitarCampos}
            listaTiposConceitos={dados.listaTiposConceitos}
          />
        );
      default:
        return '';
    }
  };

  const montaNotaFinal = (aluno, index) => {
    if (aluno && aluno.notasBimestre && aluno.notasBimestre.length) {
      if (ehRegencia) {
        return aluno.notasBimestre[index];
      }
      return aluno.notasBimestre[0];
    }

    aluno.notasBimestre = [{ notaConceito: '' }];
    return aluno.notasBimestre[0];
  };

  const montarCampoNotaConceitoFinal = (
    aluno,
    label,
    index,
    regencia,
    indexLinha
  ) => {
    if (Number(notaTipo) === Number(notasConceitos.Notas)) {
      return (
        <CampoNotaFinal
          esconderSetas
          name={`aluno${aluno.id}`}
          clicarSetas={e => clicarSetas(e, aluno, label, indexLinha, regencia)}
          step={0}
          montaNotaFinal={() => montaNotaFinal(aluno, index)}
          onChangeNotaConceitoFinal={(nota, valor) =>
            onChangeNotaConceitoFinal(nota, valor)
          }
          desabilitarCampo={ehProfessorCj || desabilitarCampos}
          podeEditar={aluno.podeEditar}
          periodoFim={dados.periodoFim}
          notaFinal={aluno.notasBimestre.find(
            x => String(x.disciplinaId) === String(disciplinaSelecionada)
          )}
          disciplinaSelecionada={disciplinaSelecionada}
          mediaAprovacaoBimestre={dados.mediaAprovacaoBimestre}
          label={label}
          podeLancarNotaFinal={dados.podeLancarNotaFinal}
        />
      );
    }
    if (Number(notaTipo) === Number(notasConceitos.Conceitos)) {
      return (
        <CampoConceitoFinal
          montaNotaConceitoFinal={() => montaNotaFinal(aluno, index)}
          onChangeNotaConceitoFinal={(nota, valor) =>
            onChangeNotaConceitoFinal(nota, valor)
          }
          desabilitarCampo={ehProfessorCj || desabilitarCampos}
          podeEditar={aluno.podeEditar}
          listaTiposConceitos={dados.listaTiposConceitos}
          label={label}
          podeLancarNotaFinal={dados.podeLancarNotaFinal}
        />
      );
    }
    return '';
  };

  return (
    <>
      {dados && dados.alunos && dados.alunos.length ? (
        <TabelaColunasFixas>
          <div className="botao-ordenacao-avaliacao">
            <Ordenacao
              conteudoParaOrdenar={dados.alunos}
              ordenarColunaNumero="numeroChamada"
              ordenarColunaTexto="nome"
              retornoOrdenado={retorno => {
                dados.alunos = retorno;
                onChangeOrdenacao(dados);
              }}
            />
          </div>
          <div className="wrapper">
            <div className="header-fixo">
              <table className="table">
                <thead className="tabela-avaliacao-thead">
                  <tr className="coluna-ordenacao-tr">
                    <th
                      className="sticky-col col-numero-chamada"
                      style={{ borderRight: 'none', borderLeft: 'none' }}
                    />
                    <th
                      className="sticky-col col-nome-aluno"
                      style={{ borderTop: 'none' }}
                    />

                    {montarCabecalhoAvaliacoes()}
                    <th
                      className="sticky-col col-nota-final cabecalho-nota-conceito-final "
                      rowSpan="2"
                    >
                      {Number(notasConceitos.Notas) === notaTipo
                        ? 'NOTA FINAL'
                        : 'CONCEITO FINAL'}
                    </th>
                    <th
                      className="sticky-col col-frequencia cabecalho-frequencia"
                      rowSpan="2"
                    >
                      %Freq.
                    </th>
                  </tr>
                  {dados.avaliacoes && dados.avaliacoes.length > 0 ? 
                  <tr>
                    <th className="sticky-col col-numero-chamada cinza-fundo" style={{ borderRight: 'none' }}/>
                    <th className="sticky-col col-nome-aluno cinza-fundo" />
                    {montarCabecalhoInterdisciplinar()}
                  </tr> : ''}
                </thead>
              </table>
            </div>

            <div>
              <table className="table">
                <tbody className="tabela-avaliacao-tbody">
                  {dados.alunos.map((aluno, i) => {
                    return (
                      <>
                        <tr>
                          <td className="sticky-col col-numero-chamada">
                            {aluno.numeroChamada}
                          </td>
                          <td className="sticky-col col-nome-aluno">
                            {aluno.marcador && (
                              <Tooltip
                                title={aluno.marcador.descricao}
                                placement="top"
                              >
                                <InfoMarcador className="fas fa-circle" />
                              </Tooltip>
                            )}
                            <div style={{ marginLeft: '30px' }}>
                              <NomeEstudanteLista
                                nome={aluno?.nome}
                                exibirSinalizacao={aluno?.ehAtendidoAEE}
                              />
                            </div>
                          </td>
                          {aluno.notasAvaliacoes.length
                            ? aluno.notasAvaliacoes.map(nota => {
                                return (
                                  <td
                                    key={shortid.generate()}
                                    className="width-150"
                                  >
                                    {montarCampoNotaConceito(nota, aluno)}
                                    {nota.ausente ? (
                                      <Tooltip title={descricaoAlunoAusente}>
                                        <i className="fas fa-user-times icon-aluno-ausente" />
                                      </Tooltip>
                                    ) : (
                                      ''
                                    )}
                                  </td>
                                );
                              })
                            : ''}
                          <td className="sticky-col col-nota-final linha-nota-conceito-final">
                            {ehRegencia ? (
                              <ColunaNotaFinalRegencia indexLinha={i} />
                            ) : (
                              montarCampoNotaConceitoFinal(aluno)
                            )}
                          </td>

                          <td className="sticky-col col-frequencia linha-frequencia ">
                            {aluno.percentualFrequencia}%
                          </td>
                        </tr>
                        <LinhaConceitoFinal
                          indexLinha={i}
                          dados={dados}
                          aluno={aluno}
                          montarCampoNotaConceitoFinal={(label, index) =>
                            montarCampoNotaConceitoFinal(
                              aluno,
                              label,
                              index,
                              true,
                              i
                            )
                          }
                        />
                      </>
                    );
                  })}
                </tbody>
              </table>
            </div>
          </div>
        </TabelaColunasFixas>
      ) : (
        <LabelSemDados
          text="Bimestre selecionado não possui atividade avaliativa cadastrada"
          center
        />
      )}
    </>
  );
};

Avaliacao.propTypes = {
  notaTipo: PropTypes.number,
  onChangeOrdenacao: () => {},
};

Avaliacao.defaultProps = {
  notaTipo: 0,
  onChangeOrdenacao: () => {},
};

export default Avaliacao;
