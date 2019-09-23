import React, { useState, useLayoutEffect } from 'react';
import { useSelector } from 'react-redux';
import DataTable from '~/componentes/table/dataTable';
import { Colors } from '~/componentes/colors';
import Button from '~/componentes/button';
import history from '~/servicos/history';

const ListaNotificacoes = () => {
  const notificacoes = useSelector(state => state.notificacoes);
  const [selectedRowKeys, setSelectedRowKeys] = useState([]);

  const categoriaLista = ['', 'Alerta', 'Ação', 'Aviso'];
  const statusLista = ['', 'Não lida', 'Lida', 'Aceita', 'Recusada'];

  const colunas = [
    {
      title: 'ID',
      dataIndex: 'codigo',
      key: 'codigo',
      className: 'px-4',
    },
    {
      title: 'Tipo',
      dataIndex: 'categoria',
      key: 'categoria',
      render: categoria => categoriaLista[categoria],
    },
    {
      title: 'Título',
      dataIndex: 'titulo',
      key: 'titulo',
    },
    {
      title: 'Situação',
      dataIndex: 'status',
      key: 'status',
      className: 'text-uppercase',
      render: status => (
        <span className={`${status === 1 && 'cor-vermelho font-weight-bold'}`}>
          {statusLista[status]}
        </span>
      ),
    },
    {
      title: 'Data/Hora',
      dataIndex: 'data',
      key: 'data',
      className: 'text-right',
      width: '100px',
      textWrap: 'word-break',
      ellipsis: true,
      render: data => <span style={{ width: 50 }}>{data}</span>,
    },
  ];

  const onSelectRow = row => {
    setSelectedRowKeys(row);
  };

  useLayoutEffect(() => {
    if (selectedRowKeys[0]) {
      history.push(`/notificacoes/${selectedRowKeys[0]}`);
    }
  }, [selectedRowKeys]);

  const onClickVerTudo = () => {
    history.push(`/notificacoes`);
  };

  return (
    <>
      <DataTable
        columns={colunas}
        dataSource={notificacoes.notificacoes}
        pageSize={0}
        onSelectRow={onSelectRow}
        selectedRowKeys={selectedRowKeys}
      />
      <Button
        label="Ver tudo"
        className="btn-lg btn-block"
        color={Colors.Roxo}
        fontSize="14px"
        height="48px"
        customRadius="border-top-right-radius: 0 !important; border-top-left-radius: 0 !important;"
        border
        bold
        onClick={onClickVerTudo}
      />
    </>
  );
};

export default ListaNotificacoes;
