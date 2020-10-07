import PropTypes from 'prop-types';
import React from 'react';
import shortid from 'shortid';
import { DataTable, Label } from '~/componentes';
import {
  CardLista,
  ColunaBotaoLista,
  BotaoLista,
} from './transferenciaLista.css';

const TransferenciaLista = props => {
  const {
    listaEsquerda,
    listaDireita,
    onClickAdicionar,
    onClickRemover,
  } = props;

  const propPadrao = {
    id: shortid.generate(),
    onSelectRow: () => {},
    columns: [],
    dataSource: [],
    selectedRowKeys: [],
    selectMultipleRows: false,
    pagination: false,
    scroll: { y: 400 },
    title: '',
  };

  return (
    <>
      <div className="mt-2" style={{ flexGrow: 1, display: 'flex' }}>
        <div>
          <div style={{ height: '50px' }}>
            <Label text={listaEsquerda.title} />
          </div>
          <CardLista>
            <DataTable
              scroll={listaEsquerda.scroll || propPadrao.scroll}
              id={listaEsquerda.id || shortid.generate()}
              onSelectRow={listaEsquerda.onSelectRow || propPadrao.onSelectRow}
              columns={listaEsquerda.columns || propPadrao.columns}
              dataSource={listaEsquerda.dataSource || propPadrao.dataSource}
              pagination={listaEsquerda.pagination || propPadrao.pagination}
              selectedRowKeys={
                listaEsquerda.selectedRowKeys || propPadrao.selectedRowKeys
              }
              selectMultipleRows={
                listaEsquerda.selectMultipleRows ||
                propPadrao.selectMultipleRows
              }
            />
          </CardLista>
        </div>
        <ColunaBotaoLista style={{ margin: '15px' }}>
          <BotaoLista className="mb-2" onClick={onClickAdicionar}>
            <i className="fas fa-chevron-right" />
          </BotaoLista>
          <BotaoLista onClick={onClickRemover}>
            <i className="fas fa-chevron-left" />
          </BotaoLista>
        </ColunaBotaoLista>
        <div>
          <div style={{ height: '50px' }}>
            <Label text={listaDireita.title} />
          </div>
          <CardLista>
            <DataTable
              scroll={listaDireita.scroll || propPadrao.scroll}
              id={listaDireita.id || shortid.generate()}
              onSelectRow={listaDireita.onSelectRow || propPadrao.onSelectRow}
              columns={listaDireita.columns || propPadrao.columns}
              dataSource={listaDireita.dataSource || propPadrao.dataSource}
              pagination={listaDireita.pagination || propPadrao.pagination}
              selectedRowKeys={
                listaDireita.selectedRowKeys || propPadrao.selectedRowKeys
              }
              selectMultipleRows={
                listaDireita.selectMultipleRows || propPadrao.selectMultipleRows
              }
            />
          </CardLista>
        </div>
      </div>
    </>
  );
};

TransferenciaLista.propTypes = {
  listaEsquerda: PropTypes.oneOfType(PropTypes.object),
  listaDireita: PropTypes.oneOfType(PropTypes.object),
  onClickAdicionar: PropTypes.func,
  onClickRemover: PropTypes.func,
};

TransferenciaLista.defaultProps = {
  listaEsquerda: {},
  listaDireita: {},
  onClickAdicionar: () => {},
  onClickRemover: () => {},
};

export default TransferenciaLista;
