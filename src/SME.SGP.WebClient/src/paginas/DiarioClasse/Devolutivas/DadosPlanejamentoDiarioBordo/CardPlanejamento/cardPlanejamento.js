import * as moment from 'moment';
import React from 'react';
import { useSelector } from 'react-redux';
import shortid from 'shortid';
import Editor from '~/componentes/editor/editor';
import {
  EditorPlanejamento,
  ListaPlanejamentos,
  Tabela,
} from './cardPlanejamento.css';

const CardPlanejamento = React.memo(() => {
  const dadosPlanejamentos = useSelector(
    store => store.devolutivas.dadosPlanejamentos
  );

  return (
    <div style={{ border: '1px solid #DADADA' }}>
      {dadosPlanejamentos &&
      dadosPlanejamentos.items &&
      dadosPlanejamentos.items.length ? (
        <ListaPlanejamentos className="row mt-3 p-3">
          {dadosPlanejamentos.items.map(item => {
            return (
              <div className="col-md-6" key={shortid.generate()}>
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
                        {item.cj ? <th className="cj">CJ</th> : null}
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
          })}
        </ListaPlanejamentos>
      ) : (
        <div className="text-center p-2"> Sem dados</div>
      )}
    </div>
  );
});

export default CardPlanejamento;
