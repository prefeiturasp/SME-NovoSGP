import { Switch } from 'antd';
import PropTypes from 'prop-types';
import React, { useState } from 'react';

import { Lista } from './listaFrequencia.css';

const ListaFrequencia = props => {
  const { dados } = props;

  const [dataSource, setDataSource] = useState(dados);

  const renderSwitch = (i, aula) => {
    return (
      <div  id={`switch-${i}`} className={aula.compareceu ? 'presenca' : 'falta'} >
        <Switch
          checkedChildren="C"
          unCheckedChildren="F"
          onChange={faltou => {
            aula.compareceu = !faltou;
            setDataSource([...dataSource]);
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
    aluno.aulas.forEach(aula => aula.compareceu = marcarPresenca);
    setDataSource([...dataSource]);
  }

  const marcarPresencaFaltaTodosAlunos = marcarPresenca => {
    dataSource.forEach(aluno => {
      aluno.aulas.forEach(aula => aula.compareceu = marcarPresenca);
    });
    setDataSource([...dataSource]);
  }

  return (
    <>
    { dataSource && dataSource.length > 0 ?
        <Lista className="mt-4 table-responsive">
          <table className="table mb-0">
            <thead className="tabela-frequencia-thead">
              <tr>
                <th className="width-60"></th>
                <th className="text-left">Lista de estudantes</th>
                <th className="width-40 cursor-pointer" onClick={()=> marcarPresencaFaltaTodosAlunos(true)}>C</th>
                <th className="width-40 cursor-pointer" onClick={()=> marcarPresencaFaltaTodosAlunos(false)}>F</th>
                {
                  dataSource[0].aulas.map(( aula, i) => {
                    return (
                      <th key={i} className={dataSource[0].aulas.length -1 == i ? 'width-70' : 'border-right-none'}>{aula.numeroAula}</th>
                    )
                  })
                }
                {/* <th className="width-70">
                  <i className="fas fa-exclamation-triangle"></i>
                </th> */}
              </tr>
            </thead>
            <tbody className="tabela-frequencia-tbody">
              {
                dataSource.map((aluno, i) => {
                  return (
                  <tr key={i}>
                    <td className="text-center font-weight-bold">{aluno.numeroAlunoChamada}</td>
                    <td className="text-left">{aluno.nomeAluno}</td>
                    <td>
                      <button onClick={()=> marcaPresencaFaltaTodasAulas(aluno, true)} type="button" className={`ant-btn ant-btn-circle ant-btn-sm btn-falta-presenca ${validaSeCompareceuTodasAulas(aluno) ? 'btn-compareceu' : ''} `} >
                        <i className="fas fa-check fa-sm"></i>
                      </button>
                    </td>
                    <td>
                    <button onClick={()=> marcaPresencaFaltaTodasAulas(aluno, false)} type="button" className={`ant-btn ant-btn-circle ant-btn-sm btn-falta-presenca ${validaSeFaltouTodasAulas(aluno) ? 'btn-falta' : ''} `}>
                      <i className="fas fa-times fa-sm"></i>
                    </button>
                    </td>

                    {
                      aluno.aulas.map((aula, i) => {
                        return (
                          <td key={i} className={dataSource[0].aulas.length -1 == i ? 'width-70' : 'border-right-none width-70'}>{renderSwitch(i, aula)}</td>
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
        </Lista>
        : ''
    }
    </>
  );
};

ListaFrequencia.propTypes = {
  dados: PropTypes.array
};

ListaFrequencia.defaultProps = {
  dados: []
};

export default ListaFrequencia;
