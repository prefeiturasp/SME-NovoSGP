import * as moment from 'moment';
import React, { useState } from 'react';
import { ListaPaginada } from '~/componentes';
import CardCollapse from '~/componentes/cardCollapse';

const RegistroComunicacaoEscolaAquiCardCollapse = () => {
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
      title: 'Categoria',
      dataIndex: 'categoria',
    },
    {
      title: 'Título',
      dataIndex: 'titulo',
    },
    {
      title: 'Leitura',
      dataIndex: 'leitura',
    },
  ];

  const onClickExpandir = () => setExibir(!exibir);

  return (
    <div className="col-md-12 mb-2">
      <CardCollapse
        key="registro-comunicacao-escola-aqui-acompanhamento-aprendizagem-collapse"
        onClick={onClickExpandir}
        titulo="Registro de comunicação (Escola aqui)"
        indice="registro-comunicacao-escola-aqui-acompanhamento-aprendizagem"
        show={exibir}
        alt="registro-comunicacao-escola-aqui-acompanhamento-aprendizagem"
      >
        <div className="col-md-12 mb-2">
          <ListaPaginada
            url=""
            id="lista-registro-comunicacao-escola-aqui"
            colunas={colunas}
            filtro={filtro}
          />
        </div>
      </CardCollapse>
    </div>
  );
};

export default RegistroComunicacaoEscolaAquiCardCollapse;
