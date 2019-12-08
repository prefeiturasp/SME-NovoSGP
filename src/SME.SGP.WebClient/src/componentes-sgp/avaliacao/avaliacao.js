import { Tooltip } from 'antd';
import PropTypes from 'prop-types';
import React, { useState } from 'react';
import CampoNumero from '~/componentes/campoNumero';
import SelectComponent from '~/componentes/select';
import notasConceitos from '~/dtos/notasConceitos';

import Ordenacao from '../Ordenacao/ordenacao';
import { Lista, Container } from './avaliacao.css';

const Avaliacao = props => {
  const { dados, onChangeAvaliacao, notaTipo} = props;

  const [dataSource, setDataSource] = useState(dados);
  const [alunos, setAlunos] = useState(dados.alunos);

  const listaConceitos = [
    { valor: 'P', descricao: 'P'},
    { valor: 'S', descricao: 'S'},
    { valor: 'NS', descricao: 'NS'},
  ];

  const onChangeNotaConceito = (aluno, notaConceito, index) => {
    aluno.notasAvaliacoes[index].notaConceito = notaConceito;
    setAlunos([...alunos])
  }

  const descricaoAlunoAusente = 'Aluno ausente na data da avaliação';

  return (
    <Container>
    { dataSource ?
      <>
        <Lista className="mt-4 table-responsive">
          <div className="scroll-tabela-avaliacao-thead">
            <table className="table mb-0 ">
              <thead className="tabela-avaliacao-thead">
                <tr className="coluna-ordenacao-tr">
                  <th colSpan="2" className="coluna-ordenacao-th">
                    <Ordenacao
                      className="botao-ordenacao-avaliacao"
                      conteudoParaOrdenar={dataSource.alunos}
                      ordenarColunaNumero="numeroChamada"
                      ordenarColunaTexto="nome"
                      retornoOrdenado={retorno =>  {
                        setAlunos([...retorno])
                      }}
                    ></Ordenacao>
                  </th>
                  { dataSource.avaliacoes && dataSource.avaliacoes.length > 0 ?
                      dataSource.avaliacoes.map((avaliacao, i) => {
                        return (
                        <th key={i} className="width-150">
                          <div className="texto-header-avaliacao">
                            {avaliacao.nome}
                          </div>
                          <div className="texto-header-avaliacao">
                          <Tooltip title={avaliacao.descricao}>
                            {avaliacao.descricao}
                          </Tooltip>
                          </div>
                          <div className="texto-header-avaliacao">
                            {window.moment(avaliacao.data).format('DD/MM/YYYY')}
                          </div>
                        </th>
                        )
                      })
                    : ''
                  }
                </tr>
                <tr>
                <th colSpan="2"></th>
                 { dataSource.avaliacoes && dataSource.avaliacoes.length > 0 ?
                      dataSource.avaliacoes.map((item, i) => {
                        return (
                        <th key={i} className="width-150">
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
                    alunos.map((aluno, i) => {
                      return (
                      <tr key={i}>
                        <td className="width-60 text-center font-weight-bold">{aluno.numeroChamada}</td>
                        <td className="text-left">{aluno.nome}</td>
                        {
                          aluno.notasAvaliacoes.length ?
                            aluno.notasAvaliacoes.map((nota, i) => {
                              return (
                                <td key={i}  className="width-150" style={{padding: "3px"}}>
                                    {
                                      notasConceitos.Notas == notaTipo ?
                                      <CampoNumero
                                        onChange={valorNovo=> onChangeNotaConceito(aluno, valorNovo, i)}
                                        value={nota.notaConceito}
                                        min={0}
                                        max={10}
                                        step={0.1}
                                        placeholder="Nota"
                                        classNameCampo={`${nota.ausente ? 'aluno-ausente-notas' : 'aluno-notas'}`}
                                      />
                                      :
                                      <SelectComponent
                                        valueOption="valor"
                                        valueText="descricao"
                                        lista={listaConceitos}
                                        valueSelect={nota.notaConceito}
                                        onChange={valorNovo => onChangeNotaConceito(aluno, valorNovo, i)}
                                        showSearch
                                        placeholder="Conceito"
                                        className="select-conceitos"
                                        classNameContainer={nota.ausente ? 'aluno-ausente-conceitos' : 'aluno-conceitos'}
                                      />
                                    }
                                    {
                                      nota.ausente ?
                                      <Tooltip title={descricaoAlunoAusente}>
                                        <i className="fas fa-user-times icon-aluno-ausente"></i>
                                      </Tooltip>
                                      : ''
                                    }
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
        </>
        : ''
      }
    </Container>
  );
};

Avaliacao.propTypes = {
  dados: PropTypes.object,
  onChangeAvaliacao: PropTypes.func
};

Avaliacao.defaultProps = {
  dados: [],
  onChangeAvaliacao: ()=>{}
};

export default Avaliacao;
