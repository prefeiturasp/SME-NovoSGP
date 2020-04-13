import React from 'react';

import PropTypes from 'prop-types';
import { ListaAnotacao, Tabela } from './anotacaoAluno.css';
import Editor from '~/componentes/editor/editor';

const AnotacoesAlunoLista = props => {
  const { anotacoes } = props;

  // TODO VER COM UX!
  const cores = ['#20aa73', '#b40c02', '#086397'];

  return (
    <ListaAnotacao>
      {anotacoes.map((item, index) => {
        return (
          <Tabela
            className="table-responsive mb-3"
            style={{ borderLeftColor: `${cores[index]}` }}
          >
            <table className="table">
              <thead>
                <tr>
                  <th>{item.componente}</th>
                  <th>{item.professor}</th>
                  <th>RF={item.professorRf}</th>
                  <th>{item.data}</th>
                </tr>
              </thead>
              <tbody>
                <td colSpan="4">
                  <Editor
                    id="anotacao-aluno"
                    inicial={item.anotacao}
                    removerToolbar
                    desabilitar
                  />
                </td>
              </tbody>
            </table>
          </Tabela>
        );
      })}
    </ListaAnotacao>
  );
};

AnotacoesAlunoLista.propTypes = {
  anotacoes: PropTypes.array,
};

AnotacoesAlunoLista.defaultProps = {
  anotacoes: [],
};

export default AnotacoesAlunoLista;
