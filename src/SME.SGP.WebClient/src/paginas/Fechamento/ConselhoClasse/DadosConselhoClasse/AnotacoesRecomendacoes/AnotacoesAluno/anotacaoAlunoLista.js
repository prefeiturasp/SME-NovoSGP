import * as moment from 'moment';
import PropTypes from 'prop-types';
import React from 'react';
import Editor from '~/componentes/editor/editor';
import { ListaAnotacao, Tabela } from './anotacaoAluno.css';

const AnotacoesAlunoLista = props => {
  const { anotacoes } = props;

  // TODO VER COM UX!
  const cores = [
    '#20aa73',
    '#b40c02',
    '#086397',
    '#20aa73',
    '#b40c02',
    '#086397',
  ];

  return (
    <ListaAnotacao>
      {anotacoes && anotacoes.length ? (
        anotacoes.map((item, index) => {
          return (
            <Tabela
              className="table-responsive mb-3"
              style={{ borderLeftColor: `${cores[index]}` }}
              key={`anotacao-disciplina-${item.disciplina}`}
            >
              <table className="table">
                <thead>
                  <tr>
                    <th>{item.disciplina}</th>
                    <th>{item.professor}</th>
                    <th>RF={item.professorRf}</th>
                    <th>{item.data ? moment(item.data).format('L') : ''}</th>
                  </tr>
                </thead>
                <tbody>
                  <tr>
                    <td colSpan="4">
                      <Editor
                        id="anotacao-aluno"
                        inicial={item.anotacao}
                        removerToolbar
                        desabilitar
                      />
                    </td>
                  </tr>
                </tbody>
              </table>
            </Tabela>
          );
        })
      ) : (
        <div className="col-md-12 text-center">Sem dados</div>
      )}
    </ListaAnotacao>
  );
};

AnotacoesAlunoLista.propTypes = {
  anotacoes: PropTypes.oneOfType([PropTypes.array]),
};

AnotacoesAlunoLista.defaultProps = {
  anotacoes: [],
};

export default AnotacoesAlunoLista;
