import React, { useState } from 'react';
import styled from 'styled-components';

import { Cabecalho } from '~/componentes-sgp';
import { Loader, Card, ButtonGroup, ListaPaginada } from '~/componentes';
import history from '~/servicos/history';
import RotasDto from '~/dtos/rotasDto';

const ComunicadosLista = () => {
  const Badge = styled.button``;

  const [loaderSecao] = useState(false);

  const onClickVoltar = () => {
    history.push('/');
  };

  const onClickBotaoPrincipal = () => {
    history.push(`${RotasDto.ACOMPANHAMENTO_COMUNICADOS}/novo`);
  };

  const onChangeFiltro = () => {};

  const onClickEditar = mensagem => {
    history.push(`${RotasDto.ACOMPANHAMENTO_COMUNICADOS}/novo/${mensagem.Id}`);
  };

  const renderizarGrupos = grupos => {
    const objeto = { children: '' };

    if (grupos && grupos.length) {
      grupos.forEach(grupo => {
        objeto.children = (
          <Badge
            alt={grupo.Nome_Grupo}
            className="badge badge-pill border text-dark font-weight-light px-2 py-1 mr-2"
          >
            {grupo.Nome_Grupo}
          </Badge>
        );
      });
    }

    return objeto;
  };

  const colunas = [
    {
      title: 'Grupo',
      dataIndex: 'Mensagem_Grupo',
      render: grupos => {
        if (grupos && grupos.length) {
          grupos.forEach(grupo => {
            return (
              <Badge
                alt={grupo.Nome_Grupo}
                className="badge badge-pill border text-dark font-weight-light px-2 py-1 mr-2"
              >
                {grupo.Nome_Grupo}
              </Badge>
            );
          });
        }
      },
    },
    {
      title: 'Título',
      dataIndex: 'Titulo',
    },
    {
      title: 'Data de envio',
      dataIndex: 'Data_Envio',
    },
    {
      title: 'Data de expiração',
      dataIndex: 'Data_Expiracao',
    },
  ];

  return (
    <>
      <Cabecalho pagina="Comunicados" />
      <Loader loading={loaderSecao}>
        <Card mx="mx-0">
          <ButtonGroup
            // somenteConsulta={somenteConsulta}
            // permissoesTela={permissoesTela[RotasDto.REGISTRO_POA]}
            // temItemSelecionado={
            //   itensSelecionados && itensSelecionados.length >= 1
            // }
            // onClickExcluir={onClickExcluir}
            onClickVoltar={onClickVoltar}
            onClickBotaoPrincipal={onClickBotaoPrincipal}
            labelBotaoPrincipal="Novo"
            // desabilitarBotaoPrincipal={
            //   !!filtro.dreId === false && !!filtro.ueId === false
            // }
          />
          {/* <Filtro onFiltrar={onChangeFiltro} /> */}
          <div className="col-md-12 pt-2 py-0 px-0">
            <ListaPaginada
              id="lista-atribuicoes-cj"
              url="https://demo4860311.mockable.io/mensagens"
              idLinha="Id"
              colunaChave="Id"
              colunas={colunas}
              onClick={onClickEditar}
              // multiSelecao
              // filtro={filtro}
              // selecionarItems={onSelecionarItems}
              filtroEhValido
              // onErro={err => erro(JSON.stringify(err))}
            />
          </div>
        </Card>
      </Loader>
    </>
  );
};

export default ComunicadosLista;
