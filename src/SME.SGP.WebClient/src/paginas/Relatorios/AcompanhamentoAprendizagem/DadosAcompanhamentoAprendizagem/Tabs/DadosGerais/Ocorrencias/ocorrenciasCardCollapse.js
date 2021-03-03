import * as moment from 'moment';
import React, { useState } from 'react';
import { ListaPaginada } from '~/componentes';
import CardCollapse from '~/componentes/cardCollapse';

const OcorrenciasCardCollapse = () => {
  const [exibir, setExibir] = useState(true);

  const [filtro, setFiltro] = useState({});

  const colunas = [
    {
      title: 'Data',
      dataIndex: 'data',
      render: data => {
        let dataFormatada = '';
        if (data) {
          dataFormatada = moment(data).format('DD/MM/YYYY');
        }
        return <span> {dataFormatada}</span>;
      },
    },
    {
      title: 'Registrado por',
      dataIndex: 'registradoPor',
    },
    {
      title: 'Título da ocorrência',
      dataIndex: 'tituloOcorrência',
    },
  ];

  const onClickExpandir = () => setExibir(!exibir);

  return (
    <div className="col-md-12 mb-2">
      <CardCollapse
        key="ocorrencias-acompanhamento-aprendizagem-collapse"
        onClick={onClickExpandir}
        titulo="Ocorrências"
        indice="ocorrencias-acompanhamento-aprendizagem"
        show={exibir}
        alt="ocorrencias-acompanhamento-aprendizagem"
      >
        <ListaPaginada
          url=""
          id="lista-ocorrencias-acompanhamento-aprendizagem"
          colunas={colunas}
          filtro={filtro}
        />
      </CardCollapse>
    </div>
  );
};

export default OcorrenciasCardCollapse;
