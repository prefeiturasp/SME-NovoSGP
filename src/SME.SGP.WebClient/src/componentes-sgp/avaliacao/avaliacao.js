import { Icon, Tooltip } from 'antd';
import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import shortid from 'shortid';
import notasConceitos from '~/dtos/notasConceitos';
import { setModoEdicaoGeral } from '~/redux/modulos/notasConceitos/actions';

import Ordenacao from '../Ordenacao/ordenacao';
import {
  CaixaMarcadores,
  IconePlusMarcadores,
  TabelaColunasFixas,
} from './avaliacao.css';
import CampoConceito from './campoConceito';
import CampoNota from './campoNota';
import { LabelSemDados } from '~/componentes';

const Avaliacao = props => {
  const dispatch = useDispatch();

  const { dados, notaTipo, onChangeOrdenacao, desabilitarCampos } = props;

  const [expandirLinha, setExpandirLinha] = useState([]);

  const onChangeNotaConceito = (nota, valorNovo) => {
    if (!desabilitarCampos && nota.podeEditar) {
      nota.notaConceito = valorNovo;
      nota.modoEdicao = true;
      dados.modoEdicao = true;
      dispatch(setModoEdicaoGeral(true));
    }
  };

  const descricaoAlunoAusente = 'Aluno ausente na data da avaliação';

  const montarCabecalhoAvaliacoes = () => {
    return dados.avaliacoes && dados.avaliacoes.length > 0
      ? dados.avaliacoes.map(avaliacao => {
          const descricaoSemHtml = avaliacao.descricao.replace(
            /<[^>]*>?/gm,
            ''
          );
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

  const montarCampoNotaConceito = nota => {
    return Number(notasConceitos.Notas) === Number(notaTipo) ? (
      <CampoNota
        nota={nota}
        onChangeNotaConceito={valorNovo =>
          onChangeNotaConceito(nota, valorNovo)
        }
        desabilitarCampo={desabilitarCampos}
      />
    ) : (
      <CampoConceito
        nota={nota}
        onChangeNotaConceito={valorNovo =>
          onChangeNotaConceito(nota, valorNovo)
        }
        desabilitarCampo={desabilitarCampos}
      />
    );
  };

  const onClickExpandir = index => {
    expandirLinha[index] = !expandirLinha[index];
    setExpandirLinha([...expandirLinha]);
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
                // setExpandirLinha([]);
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
                      Nota final
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
                            {aluno.nome}
                            {/* TODO Padrão de marcador vai ser alterado! */}
                            {/* {aluno.marcador ? (
                              <>
                                <CaixaMarcadores>
                                  {aluno.marcador.nome}
                                </CaixaMarcadores>
                                <IconePlusMarcadores
                                  onClick={() => onClickExpandir(i)}
                                  className={
                                    expandirLinha[i]
                                      ? 'fas fa-minus fa-minus-linha-expandida '
                                      : 'fas fa-plus-circle'
                                  }
                                />
                              </>
                            ) : (
                              ''
                            )} */}
                          </td>
                          {aluno.notasAvaliacoes.length
                            ? aluno.notasAvaliacoes.map(nota => {
                                return (
                                  <td
                                    key={shortid.generate()}
                                    className={`width-150 ${
                                      nota.podeEditar ? '' : 'desabilitar-nota'
                                    }`}
                                  >
                                    {montarCampoNotaConceito(nota)}
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
                            {/* TODO Montar campo! */}
                            10,00
                          </td>
                          <td className="sticky-col col-frequencia linha-frequencia ">
                            100%
                          </td>
                        </tr>
                        {/* TODO Padrão de marcador vai ser alterado! */}
                        {/* {expandirLinha[i] ? (
                          <>
                            <tr className="linha-expandida">
                              <td colSpan="1" className="text-center">
                                <Icon type="double-right" />
                              </td>
                              <td colSpan={dados.avaliacoes.length + 2}>
                                {aluno.marcador.descricao}
                              </td>
                            </tr>
                          </>
                        ) : (
                          ''
                        )} */}
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
