import React from 'react';
import { useSelector } from 'react-redux';
import { Card } from '~/componentes';
import Cabecalho from '~/componentes-sgp/cabecalho';
import RotasDto from '~/dtos/rotasDto';
import { obterDescricaoNomeMenu } from '~/servicos/servico-navegacao';
import ListaObjetivos from './ListaObjetivos/listaObjetivos';
import { ContainerPlanoAnual } from './planoAnual.css';

const PlanoAnual = () => {
  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  return (
    <ContainerPlanoAnual>
      <Cabecalho
        pagina={obterDescricaoNomeMenu(
          RotasDto.PLANO_ANUAL,
          modalidadesFiltroPrincipal,
          turmaSelecionada
        )}
      />
      <Card>
        <ListaObjetivos />
      </Card>
    </ContainerPlanoAnual>
  );
};

export default PlanoAnual;
