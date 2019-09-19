import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import DataTable from '~/componentes/table/dataTable';
import { Base, Colors } from '~/componentes/colors';
import Button from '~/componentes/button';

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
      className: 'text-uppercase text-center',
      render: status => (
        <span
          className={`${status === 1 && 'font-weight-bold'}`}
          style={{ color: status === 1 && Base.Vermelho }}
        >
          {statusLista[status]}
        </span>
      ),
    },
    {
      title: 'Data/Hora',
      dataIndex: 'data',
      key: 'data',
      className: 'text-right',
    },
  ];

  const onSelectRow = row => {
    setSelectedRowKeys(row);
  };

  const onSelecionaNotificacao = row => {
    console.log(row);
  };

  return (
    <>
      <DataTable
        columns={colunas}
        dataSource={notificacoes.notificacoes}
        pageSize={0}
        onSelectRow={onSelectRow}
        selectedRowKeys={selectedRowKeys}
        onRowClick={onSelecionaNotificacao}
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
      />
    </>
  );
};

export default ListaNotificacoes;
