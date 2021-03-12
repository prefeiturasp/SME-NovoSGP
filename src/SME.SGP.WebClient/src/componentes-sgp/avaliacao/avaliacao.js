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

import Ordenacao from '../Ordenacao/ordenacao';
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
    if (!desabilitarCampos && nota.podeEditar) {
      nota.notaConceito = valorNovo;
      nota.modoEdicao = true;
      dados.modoEdicao = true;
      dispatch(setModoEdicaoGeral(true));
    }
  };

  const onChangeNotaConceitoFinal = (notaBimestre, valorNovo) => {
    notaBimestre.notaConceito = valorNovo;
    notaBimestre.modoEdicao = true;
    dados.modoEdicao = true;
    dispatch(setModoEdicaoGeralNotaFinal(true));
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

  const acharAluno = (aluno, numero) => {
    return (
      dados &&
      dados?.alunos
        .map((valor, index, elementos) => {
          if (valor.id === aluno?.id) {
            return elementos[index + numero];
          }
          return '';
        })
        .filter(item => item?.id)
    );
  };
  const moverCursor = async (
    alunoEscolhido,
    indexElemento,
    item,
    regencia = false
  ) => {
    const elemento = document.getElementsByName(`${item}${alunoEscolhido}`);
    let elementoCursor = elemento[indexElemento];
    if (regencia) {
      await new Promise(resolve => setTimeout(resolve, 600));
      // eslint-disable-next-line prefer-destructuring
      elementoCursor = elemento[0].getElementsByTagName('input')[0];
    }
    if (elementoCursor) {
      elementoCursor.focus();
      elementoCursor.select();
    }
  };

  const escolherDirecao = keyCode => {
    switch (keyCode) {
      case 38:
        return -1;
      case 40:
        return 1;
      default:
        return 0;
    }
  };

  const acharTd = e => {
    return e.nativeEvent.path.find(item => item.localName === 'td');
  };

  const clicarSetas = (
    e,
    aluno,
    label = '',
    indexNotaConceito = 0,
    regencia = false
  ) => {
    const direcao = escolherDirecao(e.keyCode);
    const disciplina = label.toLowerCase();

    if (regencia) {
      let novaLinha = [];
      const novoIndex = indexNotaConceito + direcao;
      if (expandirLinha[novoIndex]) {
        expandirLinha[novoIndex] = false;
        novaLinha = expandirLinha;
      } else {
        novaLinha[novoIndex] = true;
      }
      dispatch(setExpandirLinha([...novaLinha]));
    }
    const elementoTD = acharTd(e);
    const indexElemento = elementoTD?.cellIndex - 2;

    // console.log('elementoTD', elementoTD);
    // console.log('indexElemento', indexElemento);
    // console.log('e', e);

    const alunoEscolhido = direcao && acharAluno(aluno, direcao);
    if (alunoEscolhido.length) {
      const item = regencia ? disciplina : 'aluno';
      moverCursor(alunoEscolhido[0].id, indexElemento, item, regencia);
    }
  };

  const montarCampoNotaConceito = (nota, aluno) => {
    switch (Number(notaTipo)) {
      case Number(notasConceitos.Notas):
        return (
          <CampoNota
            esconderSetas
            name={`aluno${aluno.id}`}
            clicarSetas={e => clicarSetas(e, aluno)}
            nota={nota}
            onChangeNotaConceito={valorNovo =>
              onChangeNotaConceito(nota, valorNovo)
            }
            desabilitarCampo={desabilitarCampos}
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

  const montaNotaFinal = (aluno, indexNotaConceito) => {
    if (aluno && aluno.notasBimestre && aluno.notasBimestre.length) {
      if (ehRegencia) {
        return aluno.notasBimestre[indexNotaConceito];
      }
      return aluno.notasBimestre[0];
    }

    aluno.notasBimestre = [{ notaConceito: '' }];
    return aluno.notasBimestre[0];
  };

  const montarCampoNotaConceitoFinal = (
    aluno,
    label,
    indexNotaConceito,
    regencia
  ) => {
    if (Number(notaTipo) === Number(notasConceitos.Notas)) {
      return (
        <CampoNotaFinal
          name={`aluno${aluno.id}`}
          clicarSetas={e =>
            clicarSetas(e, aluno, label, indexNotaConceito, regencia)
          }
          montaNotaFinal={() => montaNotaFinal(aluno, indexNotaConceito)}
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
          montaNotaConceitoFinal={() =>
            montaNotaFinal(aluno, indexNotaConceito)
          }
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
                  <tr>
                    <th
                      className="sticky-col col-numero-chamada cinza-fundo"
                      style={{ borderRight: 'none' }}
                    />
                    <th className="sticky-col col-nome-aluno cinza-fundo" />
                    {montarCabecalhoInterdisciplinar()}
                  </tr>
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
                            {aluno.marcador ? (
                              <>
                                <Tooltip
                                  title={aluno.marcador.descricao}
                                  placement="top"
                                >
                                  <InfoMarcador className="fas fa-circle" />
                                </Tooltip>
                                <div style={{ marginLeft: '30px' }}>
                                  {aluno.nome}
                                </div>
                              </>
                            ) : (
                              <div style={{ marginLeft: '30px' }}>
                                {aluno.nome}
                              </div>
                            )}
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
                          montarCampoNotaConceitoFinal={(
                            label,
                            indexNotaConceito
                          ) =>
                            montarCampoNotaConceitoFinal(
                              aluno,
                              label,
                              indexNotaConceito,
                              true
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
