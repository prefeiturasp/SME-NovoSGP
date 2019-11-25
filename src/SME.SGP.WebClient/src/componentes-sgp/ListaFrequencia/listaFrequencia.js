import { Switch } from 'antd';
import PropTypes from 'prop-types';
import React, { useState } from 'react';

import { Lista } from './listaFrequencia.css';

const ListaFrequencia = props => {
  const { dados, onChangeFrequencia } = props;

  const [dataSource, setDataSource] = useState(dados);

  const renderSwitch = (i, aula, aluno) => {
    return (
      <div  id={`switch-${i}`} className={aula.compareceu ? 'presenca' : 'falta'} >
        <Switch
          disabled={aluno.desabilitado}
          checkedChildren="C"
          unCheckedChildren="F"
          onChange={faltou => {
            if (!aluno.desabilitado) {
              aula.compareceu = !faltou;
              setDataSource([...dataSource]);
              onChangeFrequencia(true);
            }
          }}
          checked={!aula.compareceu}
          />
      </div>
    );
  };

  const validaSeFaltouTodasAulas = aluno => {
    const totalAulas = aluno.aulas.length;
    const totalAulasFaltou = aluno.aulas.filter(aula=> !aula.compareceu);
    return totalAulas === totalAulasFaltou.length;
  }

  const validaSeCompareceuTodasAulas = aluno => {
    const totalAulas = aluno.aulas.length;
    const totalAulasCompareceu = aluno.aulas.filter(aula => aula.compareceu);
    return totalAulas === totalAulasCompareceu.length;
  }

  const marcaPresencaFaltaTodasAulas = (aluno, marcarPresenca) => {
    if (!aluno.desabilitado) {
      aluno.aulas.forEach(aula => aula.compareceu = marcarPresenca);
      setDataSource([...dataSource]);
      onChangeFrequencia(true);
    }
  }

  const marcarPresencaFaltaTodosAlunos = marcarPresenca => {
    dataSource.forEach(aluno => {
      if (!aluno.desabilitado) {
        aluno.aulas.forEach(aula => aula.compareceu = marcarPresenca);
      }
    });
    setDataSource([...dataSource]);
    onChangeFrequencia(true);
  }

  return (
    <>
    { dataSource && dataSource.length > 0 ?
        <Lista className="mt-4 table-responsive">
          <div className="scroll-tabela-frequencia-thead">
            <table className="table mb-0 ">
              <thead className="tabela-frequencia-thead">
                <tr>
                  <th className="width-60"></th>
                  <th className="text-left">Lista de estudantes</th>
                  { dataSource[0].aulas.length > 1 ?
                    <>

                      <th className="width-50 cursor-pointer" onClick={()=> marcarPresencaFaltaTodosAlunos(true)}>
                        <div className="marcar-todas-frequencia">
                          Marcar todas
                        </div>
                        <div className="margin-marcar-todos">
                          C
                        </div>
                      </th>
                      <th className="width-50 cursor-pointer" onClick={()=> marcarPresencaFaltaTodosAlunos(false)}>
                       <div className="margin-marcar-todos">
                          F
                        </div>
                      </th>
                    </>
                    : ''
                  }
                  {
                    dataSource[0].aulas.map(( aula, i) => {
                      return (
                        <th key={i} className={dataSource[0].aulas.length -1 == i ? 'width-70' : 'border-right-none width-70'}>{aula.numeroAula}</th>
                      )
                    })
                  }
                  {/* <th className="width-70">
                    <i className="fas fa-exclamation-triangle"></i>
                  </th> */}
                </tr>
              </thead>
              </table>
            </div>
            <div className="scroll-tabela-frequencia-tbody">
              <table className="table mb-0">
                <tbody className="tabela-frequencia-tbody">
                  {
                    dataSource.map((aluno, i) => {
                      return (
                      <tr key={i} className={aluno.desabilitado ? 'desabilitar-aluno' : ''} >
                        <td className="width-60 text-center font-weight-bold">{aluno.numeroAlunoChamada}</td>
                        <td className="text-left">{aluno.nomeAluno}</td>
                        {
                          dataSource[0].aulas.length > 1 ?
                          <>
                            <td className="width-50">
                              <button type="button"
                                      onClick={()=> marcaPresencaFaltaTodasAulas(aluno, true)}
                                      className={`ant-btn ant-btn-circle ant-btn-sm btn-falta-presenca ${validaSeCompareceuTodasAulas(aluno) ? 'btn-compareceu' : ''} `}
                                      disabled={aluno.desabilitado}>
                                <i className="fas fa-check fa-sm"></i>
                              </button>
                            </td>
                            <td className="width-50">
                            <button type="button"
                                    onClick={()=> marcaPresencaFaltaTodasAulas(aluno, false)}
                                    className={`ant-btn ant-btn-circle ant-btn-sm btn-falta-presenca ${validaSeFaltouTodasAulas(aluno) ? 'btn-falta' : ''} `}
                                    disabled={aluno.desabilitado}>
                              <i className="fas fa-times fa-sm"></i>
                            </button>
                            </td>
                          </>
                          : ''
                        }

                        {
                          aluno.aulas.map((aula, i) => {
                            return (
                              <td key={i} className={dataSource[0].aulas.length -1 == i ? 'width-70' : 'border-right-none width-70'}>{renderSwitch(i, aula, aluno)}</td>
                            )
                          })
                        }
                        {/* <td>80%</td> */}
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

ListaFrequencia.propTypes = {
  dados: PropTypes.array,
  onChangeFrequencia: PropTypes.func
};

ListaFrequencia.defaultProps = {
  dados: [],
  onChangeFrequencia: ()=>{}
};

export default ListaFrequencia;
