import React, { useState, useCallback } from 'react';
import { useSelector } from 'react-redux';
import styled from 'styled-components';
import shortid from 'shortid';

import { Cabecalho } from '~/componentes-sgp';
import { Loader, Card, ButtonGroup, ListaPaginada } from '~/componentes';
import Filtro from '../Filtro';

import history from '~/servicos/history';
import RotasDto from '~/dtos/rotasDto';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';

const ComunicadosLista = () => {
  const Badge = styled.button``;
  const [loaderSecao] = useState(false);

  const [somenteConsulta, setSomenteConsulta] = useState(false);
  const permissoesTela = useSelector(store => store.usuario.permissoes);
  const [itensSelecionados, setItensSelecionados] = useState([]);

  useCallback(() => {
    setSomenteConsulta(verificaSomenteConsulta(permissoesTela));
  }, [permissoesTela]);

  const onSelecionarItems = items => {
    setItensSelecionados(items);
  };

  const onClickExcluir = async () => {};

  const onClickVoltar = () => {
    history.push('/');
  };

  const onClickBotaoPrincipal = () => {
    history.push(`${RotasDto.ACOMPANHAMENTO_COMUNICADOS}/novo`);
  };

  const onClickEditar = comunicado => {
    history.push(
      `${RotasDto.ACOMPANHAMENTO_COMUNICADOS}/editar/${comunicado.id}`
    );
  };

  const [filtro, setFiltro] = useState({});

  const onChangeFiltro = valoresFiltro => {
    setFiltro({ ...valoresFiltro });
  };

  const colunas = [
    {
      title: 'Grupo',
      dataIndex: 'grupos',
      render: grupos => {
        return (
          grupos &&
          grupos.map(grupo => (
            <Badge
              key={shortid.generate()}
              alt={grupo.nome}
              className="badge badge-pill bg-white border text-dark font-weight-light px-2 py-1 mr-2"
            >
              {grupo.nome}
            </Badge>
          ))
        );
      },
    },
    {
      title: 'Título',
      dataIndex: 'titulo',
    },
    {
      title: 'Data de envio',
      dataIndex: 'dataEnvio',
      render: data => {
        return data && window.moment(data).format('DD/MM/YYYY');
      },
    },
    {
      title: 'Data de expiração',
      dataIndex: 'dataExpiracao',
      render: data => {
        return data && window.moment(data).format('DD/MM/YYYY');
      },
    },
  ];

  return (
    <>
      <Cabecalho pagina="Comunicação com pais ou responsáveis" />
      <Loader loading={loaderSecao}>
        <Card mx="mx-0">
          <ButtonGroup
            somenteConsulta={somenteConsulta}
            permissoesTela={permissoesTela[RotasDto.ACOMPANHAMENTO_COMUNICADOS]}
            temItemSelecionado={
              itensSelecionados && itensSelecionados.length >= 1
            }
            onClickExcluir={onClickExcluir}
            onClickVoltar={onClickVoltar}
            onClickBotaoPrincipal={onClickBotaoPrincipal}
            labelBotaoPrincipal="Novo"
          />
          <Filtro onFiltrar={onChangeFiltro} />
          <div className="col-md-12 pt-2 py-0 px-0">
            <ListaPaginada
              id="lista-comunicados"
              url="v1/comunicado/listar"
              idLinha="id"
              colunaChave="id"
              colunas={colunas}
              onClick={onClickEditar}
              multiSelecao
              filtro={filtro}
              selecionarItems={onSelecionarItems}
              filtroEhValido
            />
          </div>
        </Card>
      </Loader>
    </>
  );
};

export default ComunicadosLista;
