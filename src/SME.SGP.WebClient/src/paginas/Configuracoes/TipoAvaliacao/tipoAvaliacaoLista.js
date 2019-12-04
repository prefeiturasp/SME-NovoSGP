import React, { useEffect, useState } from 'react';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Card from '~/componentes/card';
import { ListaPaginada, ButtonGroup } from '~/componentes';
import history from '~/servicos/history';
import RotasDto from '~/dtos/rotasDto';
import Filtro from './componentes/Filtro';

// Redux
import { useSelector } from 'react-redux';

const TipoAvaliacaoLista = () => {
  const [itensSelecionados, setItensSelecionados] = useState([]);
  const [filtro, setFiltro] = useState({});
  const [somenteConsulta, setSomenteConsulta] = useState(false);
  const permissoesTela = useSelector(store => store.usuario.permissoes);

  const colunas = [
    {
      title: 'Nome',
      dataIndex: 'nome',
    },
    {
      title: 'Descrição',
      dataIndex: 'descricao',
    },
    {
      title: 'Situação',
      dataIndex: 'situacao',
      render: item => {
        return item ? 'Ativo' : 'Inativo';
      },
    },
  ];

  const cliclouBotaoNovo = () => {
    history.push('/configuracoes/tipo-avaliacao/novo');
  };

  const onClickVoltar = () => history.push('/');

  const onClickBotaoPrincipal = () => {
    history.push(`atribuicao-esporadica/novo`);
  };

  const onSelecionarItems = items => {
    setItensSelecionados(items);
  };

  const onClickExcluir = async () => {};

  const onClickEditar = item => {
    history.push(`/gestao/atribuicao-esporadica/editar/${item.id}`);
  };
  const onChangeFiltro = valoresFiltro => {
    setFiltro(valoresFiltro);
  };
  return (
    <>
      <Cabecalho pagina="Tipos de Avaliações" />
      <Card>
        <ButtonGroup
          somenteConsulta={somenteConsulta}
          permissoesTela={permissoesTela[RotasDto.TIPO_AVALIACAO]}
          temItemSelecionado={
            itensSelecionados && itensSelecionados.length >= 1
          }
          onClickVoltar={onClickVoltar}
          onClickExcluir={onClickExcluir}
          onClickBotaoPrincipal={onClickBotaoPrincipal}
          labelBotaoPrincipal="Novo"
          desabilitarBotaoPrincipal={false}
        />
        <Filtro onFiltrar={onChangeFiltro} />
        <ListaPaginada
          url="/v1/atividade-avaliativa/tipos/listar"
          id="lista-tipo-avaliacao"
          colunaChave="id"
          colunas={colunas}
          filtro={filtro}
          onClick={onClickEditar}
          multiSelecao
          selecionarItems={onSelecionarItems}
          //   filtroEhValido={validarFiltro()}
        />
      </Card>
    </>
  );
};

export default TipoAvaliacaoLista;
