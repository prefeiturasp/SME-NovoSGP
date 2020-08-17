import * as moment from 'moment';
import PropTypes from 'prop-types';
import React from 'react';
import shortid from 'shortid';
import Editor from '~/componentes/editor/editor';
import {
  ListaPlanejamentos,
  Tabela,
  EditorPlanejamento,
} from './cardPlanejamento.css';

const CardPlanejamento = props => {
  const { dados } = props;

  return (
    <ListaPlanejamentos className="row">
      {dados && dados.length
        ? dados.map(item => {
            return (
              <div className="col-md-6">
                <Tabela
                  className="table-responsive mb-3"
                  key={`planejamento-diario-bordo-${shortid.generate()}`}
                >
                  <table className="table">
                    <thead>
                      <tr>
                        <th>
                          <span className="titulo">Planejamento</span> (somente
                          leitura)
                        </th>
                        {item.cj ? <th className="cj">CJ</th> : ''}
                        <th className="data">
                          {item.data ? moment(item.data).format('L') : ''}
                        </th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr>
                        <td colSpan="4">
                          <EditorPlanejamento>
                            <Editor
                              id="planejamento-diario-bordo"
                              inicial={item.planejamento}
                              removerToolbar
                              desabilitar
                            />
                          </EditorPlanejamento>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </Tabela>
              </div>
            );
          })
        : 'Sem dados'}
    </ListaPlanejamentos>
  );
};

CardPlanejamento.propTypes = {
  dados: PropTypes.oneOfType([PropTypes.array]),
};

CardPlanejamento.defaultProps = {
  dados: [],
};

export default CardPlanejamento;
