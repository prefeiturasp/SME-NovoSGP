import { Switch } from 'antd';
import PropTypes from 'prop-types';
import React, { useState, useEffect } from 'react';

import { Lista } from './avaliacao.css';
import Ordenacao from '../Ordenacao/ordenacao';
import CampoNumero from '~/componentes/campoNumero';


const Avaliacao = props => {
  const { dados, onChangeAvaliacao} = props;

  const [dataSource, setDataSource] = useState(dados);
  const [alunos, setAlunos] = useState(dados.alunos);

  const onChangeNota = (aluno, nota, index) => {
    aluno.notas[index].nota = nota;
    setAlunos([...alunos])
  }

  return (
    <>
    { dataSource ?
        <Lista className="mt-4 table-responsive">
          <div className="scroll-tabela-avaliacao-thead">
            <table className="table mb-0 ">
              <thead className="tabela-avaliacao-thead">
                <tr className="coluna-ordenacao-tr">
                  <th colspan="2" className="coluna-ordenacao-th">
                    <Ordenacao
                      className="botao-ordenacao-avaliacao"
                      conteudoParaOrdenar={dataSource.alunos}
                      ordenarColunaNumero="codigo"
                      ordenarColunaTexto="nome"
                      retornoOrdenado={retorno =>  {
                        setAlunos([...retorno])
                      }}
                    ></Ordenacao>
                  </th>
                  { dataSource.avaliacoes && dataSource.avaliacoes.length > 0 ?
                      dataSource.avaliacoes.map((avaliacao, i) => {
                        return (
                        <th key={i} className="width-170">
                          {avaliacao.nome}
                        </th>
                        )
                      })
                    : ''
                  }
                </tr>
                <tr>
                <th colspan="2"></th>
                 { dataSource.avaliacoes && dataSource.avaliacoes.length > 0 ?
                      dataSource.avaliacoes.map(() => {
                        return (
                        <th className="width-170">
                          {/* TODO - INTERDISCIPLINAR */}
                        </th>
                        )
                      })
                    : ''
                  }
                </tr>
              </thead>
              </table>
            </div>
            <div className="scroll-tabela-avaliacao-tbody">
              <table className="table mb-0">
                <tbody className="tabela-avaliacao-tbody">
                  {
                    dataSource.alunos.map((aluno, i) => {
                      return (
                      <tr key={i} >
                        <td className="width-60 text-center font-weight-bold">{aluno.codigo}</td>
                        <td className="text-left">{aluno.nome}</td>
                        {
                          aluno.notas.length ?
                            aluno.notas.map((nota, i) => {
                              return (
                                <td key={i}  className="width-170">
                                  {/* {nota.nota} */}
                                  <CampoNumero
                                    onChange={(valorNovo)=> onChangeNota(aluno, valorNovo, i)}
                                    value={nota.nota}
                                    min={0}
                                    max={10}
                                    step={0.1}
                                    placeholder="Nota"
                                    >
                                  </CampoNumero>
                                </td>
                                )
                              })
                          : ''
                        }
                      </tr>
                      )
                    })
                  }
                </tbody>
              </table>
           </div>
        </Lista>
        : ''
    }
    </>
  );
};

Avaliacao.propTypes = {
  dados: PropTypes.array,
  onChangeAvaliacao: PropTypes.func
};

Avaliacao.defaultProps = {
  dados: [],
  onChangeAvaliacao: ()=>{}
};

export default Avaliacao;
