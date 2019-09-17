import React from 'react';
import { useSelector } from 'react-redux';
import DataTable from '~/componentes/table/dataTable';

const ListaNotificacoes = () => {
  const notificacoes = useSelector(state => state.notificacoes);

  const colunas = [
    {
      title: 'ID',
      dataIndex: 'codigo',
      key: 'codigo',
    },
    {
      title: 'Tipo',
      dataIndex: 'categoria',
      key: 'categoria',
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
    },
    {
      title: 'Data/Hora',
      dataIndex: 'data',
      key: 'data',
    },
  ];

  return (
    <DataTable
      columns={colunas}
      dataSource={notificacoes.notificacoes}
      pageSize={0}
    />
  );
};

export default ListaNotificacoes;
