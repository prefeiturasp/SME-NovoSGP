import { Switch } from 'antd';
import PropTypes from 'prop-types';
import React, { useState } from 'react';

import { Button } from 'antd';

import { Lista } from './listaFrequencia.css';

const ListaFrequencia = props => {
  const { dataSource } = props;

  const renderSwitch = (i, linha, aula) => {
    return (
      <div className={faltas[i][aula]} id={`switch-${i}`}>
        <Switch
          checkedChildren="C"
          unCheckedChildren="F"
          onChange={valor => {
            linha.aulaFalta[aula] = valor;
            setFaltas(montaClasseFaltaPresenca());
          }}
          checked={linha.aulaFalta[aula]}
          />
      </div>
    );
  };

  const montaClasseFaltaPresenca = () => {
    return dataSource.map(aluno => {
      return {
        primeira: aluno.aulaFalta.primeira ? 'falta' : 'presenca',
        segunda: aluno.aulaFalta.segunda ? 'falta' : 'presenca',
        terceira: aluno.aulaFalta.terceira ? 'falta' : 'presenca',
        quarta: aluno.aulaFalta.quarta ? 'falta' : 'presenca',
        quinta: aluno.aulaFalta.quinta ? 'falta' : 'presenca',
        faltouTodasAulas: validaSeFaltouTodasAulas(aluno) ? 'btn-falta' : '',
        compareceuTodasAulas: validaSeCompareceuTodasAulas(aluno) ? 'btn-compareceu' : '',
      };
    })
  };

  const validaSeFaltouTodasAulas = aluno => {
    if (aluno.aulaFalta.primeira && aluno.aulaFalta.segunda && aluno.aulaFalta.terceira && aluno.aulaFalta.quarta && aluno.aulaFalta.quinta) {
      return true
    } else {
      return false;
    }
  }

  const validaSeCompareceuTodasAulas = aluno => {
    if (!aluno.aulaFalta.primeira && !aluno.aulaFalta.segunda && !aluno.aulaFalta.terceira && !aluno.aulaFalta.quarta && !aluno.aulaFalta.quinta) {
      return true;
    } else {
      return false
    }
  }

  const marcaPresencaFaltaTodasAulas = (aluno, marcarFalta, i) => {
    marcarTodos(aluno, marcarFalta, i);
    setFaltas(montaClasseFaltaPresenca());
  }

  const marcarTodos = (aluno, marcarFalta, i)=> {
    if (marcarFalta) {
      aluno.presencaEmTodasAulas = false;
      aluno.faltaEmTodasAulas = true;
    } else {
      aluno.presencaEmTodasAulas = true;
      aluno.faltaEmTodasAulas = false;
    }

    aluno.aulaFalta.primeira = aluno.faltaEmTodasAulas;
    aluno.aulaFalta.segunda = aluno.faltaEmTodasAulas;
    aluno.aulaFalta.terceira = aluno.faltaEmTodasAulas;
    aluno.aulaFalta.quarta = aluno.faltaEmTodasAulas;
    aluno.aulaFalta.quinta = aluno.faltaEmTodasAulas;

    dataSource[i] = aluno;
  }

  const [faltas, setFaltas] = useState(montaClasseFaltaPresenca());

  const marcarPresencaFalta = falta => {
    dataSource.forEach((aluno,i) => {
      marcarTodos(aluno, falta, i);
    });
    setFaltas(montaClasseFaltaPresenca());
  }

  return (
    <>
      <Lista className="mt-4 table-responsive">
        <table className="table mb-0">
          <thead className="tabela-frequencia-thead">
            <tr>
              <th className="width-60"></th>
              <th className="text-left">Lista de estudantes</th>
              <th className="width-40 cursor-pointer" onClick={()=> marcarPresencaFalta(false)}>C</th>
              <th className="width-40 cursor-pointer" onClick={()=> marcarPresencaFalta(true)}>F</th>
              <th className="border-right-none width-70">1</th>
              <th className="border-right-none width-70">2</th>
              <th className="border-right-none width-70">3</th>
              <th className="border-right-none width-70">4</th>
              <th className="width-70">5</th>
              <th className="width-70">
                <i className="fas fa-exclamation-triangle"></i>
              </th>
            </tr>
          </thead>
          <tbody className="tabela-frequencia-tbody">
            {
              dataSource.map((item, i) => {
                return (
                <tr key={i}>
                  <td className="text-center font-weight-bold"> {item.codigoAluno }</td>
                  <td className="text-left">{item.nomeEstudante}</td>
                  <td>
                    <button onClick={()=> marcaPresencaFaltaTodasAulas(item, false, i)} type="button" className={`ant-btn ant-btn-circle ant-btn-sm btn-falta-presenca ${faltas[i].compareceuTodasAulas} `} >
                      <i className="fas fa-check fa-sm"></i>
                    </button>
                  </td>
                  <td>
                  <button onClick={()=> marcaPresencaFaltaTodasAulas(item, true, i)} type="button" className={`ant-btn ant-btn-circle ant-btn-sm btn-falta-presenca ${faltas[i].faltouTodasAulas} `}>
                    <i className="fas fa-times fa-sm"></i>
                  </button>
                  </td>
                  <td className="border-right-none">{renderSwitch(i, item, 'primeira')}</td>
                  <td className="border-right-none">{renderSwitch(i, item, 'segunda')}</td>
                  <td className="border-right-none">{renderSwitch(i, item, 'terceira')}</td>
                  <td className="border-right-none">{renderSwitch(i, item, 'quarta')}</td>
                  <td>{renderSwitch(i, item, 'quinta')}</td>
                  <td>80%</td>
                </tr>
                )
              })
            }
          </tbody>
        </table>
      </Lista>
    </>
  );
};

ListaFrequencia.propTypes = {
  dataSource: PropTypes.array
};

ListaFrequencia.defaultProps = {
  dataSource: []
};

export default ListaFrequencia;
