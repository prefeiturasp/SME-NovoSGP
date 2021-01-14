import * as moment from 'moment';
import React, { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import shortid from 'shortid';
import { SelectComponent } from '~/componentes';
import JoditEditor from '~/componentes/jodit-editor/joditEditor';
import { setPaginaAtiva } from '~/redux/modulos/devolutivas/actions';
import {
  EditorPlanejamento,
  ListaPlanejamentos,
  Tabela,
} from './cardPlanejamento.css';

const CardPlanejamento = React.memo(() => {
  const dadosPlanejamentos = useSelector(
    store => store.devolutivas.dadosPlanejamentos
  );

  const [totalRegistrosSelecionado, setTotalRegistrosSelecionado] = useState(
    '4'
  );

  const dispatch = useDispatch();

  const listaRegistrosPorPaginas = [
    {
      valor: 1,
      descricao: '1 registro por página',
    },
    {
      valor: 2,
      descricao: '2 registros por página',
    },
    {
      valor: 3,
      descricao: '3 registros por página',
    },
    {
      valor: 4,
      descricao: '4 registros por página',
    },
  ];

  const onChangeTotalPaginas = valor => {
    setTotalRegistrosSelecionado(valor);
    dispatch(setPaginaAtiva(valor));
  };

  return (
    <div style={{ border: '1px solid #DADADA' }}>
      <div className="col-md-3 col-sm-12 col-xl-3 col-lg-3 mt-3">
        <SelectComponent
          id="registrosPorPagina"
          lista={listaRegistrosPorPaginas}
          valueOption="valor"
          valueText="descricao"
          onChange={onChangeTotalPaginas}
          valueSelect={totalRegistrosSelecionado}
        />
      </div>
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
                        {item.aulaCj ? <th className="cj">CJ</th> : null}
                        <th className="data">
                          {item.data ? moment(item.data).format('L') : ''}
                        </th>
                      </tr>
                    </thead>
                  </table>
                  <EditorPlanejamento>
                    <JoditEditor
                      id="planejamento-diario-bordo"
                      value={item.planejamento}
                      removerToolbar
                      readonly
                    />
                  </EditorPlanejamento>
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
