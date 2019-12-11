import { Tooltip } from 'antd';
import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import shortid from 'shortid';
import notasConceitos from '~/dtos/notasConceitos';

import Ordenacao from '../Ordenacao/ordenacao';
import { Lista } from './avaliacao.css';
import CampoConceito from './campoConceito';
import CampoNota from './campoNota';

const Avaliacao = props => {
  const { dados, notaTipo, listaAvaliacoes, listaAlunos } = props;

  const [alunos, setAlunos] = useState(listaAlunos);
  const [avaliacoes] = useState(listaAvaliacoes);

  const onChangeNotaConceito = (nota, valorNovo) => {
    if (nota.podeEditar) {
      nota.notaConceito = valorNovo;
      dados.modoEdicao = true;
    }
  };

  const descricaoAlunoAusente = 'Aluno ausente na data da avaliação';

  const montarCabecalhoAvaliacoes = () => {
    return avaliacoes && avaliacoes.length > 0
      ? avaliacoes.map(avaliacao => {
          const descricaoSemHtml = avaliacao.descricao.replace(
            /<[^>]*>?/gm,
            ''
          );
          return (
            <th key={shortid.generate()} className="width-150">
              <div className="texto-header-avaliacao">{avaliacao.nome}</div>
              <div className="texto-header-avaliacao">
                <Tooltip title={descricaoSemHtml}>{descricaoSemHtml}</Tooltip>
              </div>
              <div className="texto-header-avaliacao">
                {window.moment(avaliacao.data).format('DD/MM/YYYY')}
              </div>
            </th>
          );
        })
      : '';
  };

  const montarCabecalhoInterdisciplinar = () => {
    return avaliacoes && avaliacoes.length > 0
      ? avaliacoes.map(() => {
          return (
            <th key={shortid.generate()} className="width-150">
              {/* TODO - INTERDISCIPLINAR */}
            </th>
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
      />
    ) : (
      <CampoConceito
        nota={nota}
        onChangeNotaConceito={valorNovo =>
          onChangeNotaConceito(nota, valorNovo)
        }
      />
    );
  };

  return (
    <>
      {dados ? (
        <Lista className="mt-4 table-responsive">
          <table className="table mb-0 ">
            <thead className="tabela-avaliacao-thead">
              <div className="scroll-tabela-avaliacao-thead">
                <tr className="coluna-ordenacao-tr">
                  <th colSpan="2" className="width-460 coluna-ordenacao-th">
                    <Ordenacao
                      className="botao-ordenacao-avaliacao"
                      conteudoParaOrdenar={alunos}
                      ordenarColunaNumero="numeroChamada"
                      ordenarColunaTexto="nome"
                      retornoOrdenado={retorno => {
                        setAlunos([...retorno]);
                      }}
                    />
                  </th>
                  {montarCabecalhoAvaliacoes()}
                </tr>
                <tr>
                  <th colSpan="2" className="width-460 " />
                  {montarCabecalhoInterdisciplinar()}
                </tr>
              </div>
            </thead>
          </table>
          <table className="table mb-0">
            <tbody className="tabela-avaliacao-tbody">
              <div className="scroll-tabela-avaliacao-tbody">
                {alunos.map(aluno => {
                  return (
                    <tr key={shortid.generate()}>
                      <td className="width-60 text-center font-weight-bold">
                        {aluno.numeroChamada}
                      </td>
                      <td className="width-400 text-left">{aluno.nome}</td>
                      {aluno.notasAvaliacoes.length
                        ? aluno.notasAvaliacoes.map(nota => {
                            return (
                              <td
                                key={shortid.generate()}
                                className={`width-150 ${
                                  nota.podeEditar ? '' : 'desabilitar-nota'
                                }`}
                                style={{ padding: '3px' }}
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
                    </tr>
                  );
                })}
              </div>
            </tbody>
          </table>
        </Lista>
      ) : (
        ''
      )}
    </>
  );
};

Avaliacao.propTypes = {
  dados: {},
  notaTipo: PropTypes.number,
  listaAvaliacoes: [],
  listaAlunos: [],
};

Avaliacao.defaultProps = {
  dados: [],
  notaTipo: 0,
  listaAvaliacoes: [],
  listaAlunos: [],
};

export default Avaliacao;
