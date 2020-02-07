import React, { useEffect } from 'react';
import axios from 'axios';
import styled from 'styled-components';

// Ant
import { Table } from 'antd';

const Tabela = styled(Table)``;

const servico = axios.create({
  baseURL: 'http://demo7314211.mockable.io/api',
});

const TabelaResultados = () => {
  const buscarDadosApi = () => {
    servico.get('v1/recuperacao-paralela/resumos/resultado');
  };

  useEffect(() => {
    buscarDadosApi();
  }, []);

  return (
    <Tabela
      pagination={false}
      columns={[]}
      dataSource={[]}
      rowKey="key"
      size="middle"
      className="my-2"
      bordered
    />
  );
};

export default TabelaResultados;
