import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import styled from 'styled-components';
import * as moment from 'moment';
import shortid from 'shortid';
import DataTable from '~/componentes/table/dataTable';
import { Colors } from '~/componentes/colors';
import Button from '~/componentes/button';
import history from '~/servicos/history';
import { Loader } from '~/componentes';

const ListaNotificacoes = () => {
  const [carregando, setCarregando] = useState(true);

  const notificacoes = useSelector(state => state.notificacoes);

  useEffect(() => {
    setCarregando(false);
  }, [notificacoes]);

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
      className: 'text-center px-4 py-0 data-hora',
      width: 300,
      render: data => {
        const dataFormatada = moment(data).format('DD/MM/YYYY HH:mm:ss');
        return <span>{dataFormatada}</span>;
      },
    },
  ];

  const aoClicarNaLinha = row => {
    history.push(`/notificacoes/${row.id}`);
  };

  const onClickVerTudo = () => {
    history.push(`/notificacoes`);
  };

  return (
    <Loader loading={carregando}>
      <DataTable
        columns={colunas}
        dataSource={notificacoes.notificacoes}
        pagination={false}
        onClickRow={aoClicarNaLinha}
        locale={{ emptyText: 'Você não tem nenhuma notificação!' }}
      />
      <Button
        id={shortid.generate()}
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
    </Loader>
  );
};

export default ListaNotificacoes;
