import React, { useState, useLayoutEffect } from 'react';
import { useSelector } from 'react-redux';
import styled from 'styled-components';
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
      title: 'Código',
      dataIndex: 'codigo',
      key: 'codigo',
      className:
        'text-left px-4 d-sm-none d-md-none d-lg-table-cell d-xl-table-cell',
    },
    {
      title: 'Tipo',
      dataIndex: 'categoria',
      key: 'categoria',
      className: 'text-left px-4',
      render: categoria => categoriaLista[categoria],
    },
    {
      title: 'Título',
      dataIndex: 'titulo',
      key: 'titulo',
      className:
        'text-left px-4 d-sm-none d-md-none d-lg-table-cell d-xl-table-cell',
    },
    {
      title: 'Situação',
      dataIndex: 'status',
      key: 'status',
      className: 'text-left text-uppercase px-4',
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
      className: 'text-left px-4 py-0 data-hora',
      width: 100,
      render: data => <span>{data}</span>,
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

  const Container = styled.span`
    .data-hora {
      line-height: 1rem;
      white-space: normal !important;
    }
  `;

  return (
    <Container>
      <DataTable
        columns={colunas}
        dataSource={notificacoes.notificacoes}
        pagination={false}
        onSelectRow={onSelectRow}
        selectedRowKeys={selectedRowKeys}
        locale={{ emptyText: 'Você não tem nenhuma notificação!' }}
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
    </Container>
  );
};

export default ListaNotificacoes;
