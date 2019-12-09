import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Card from '~/componentes/card';
import { ListaPaginada, ButtonGroup } from '~/componentes';
import history from '~/servicos/history';
import RotasDto from '~/dtos/rotasDto';
import Filtro from './componentes/Filtro';
import servicoTipoAvaliaco from '~/servicos/Paginas/TipoAvaliacao';
import { sucesso, confirmar, erro } from '~/servicos/alertas';

const TipoAvaliacaoLista = () => {
  const [itensSelecionados, setItensSelecionados] = useState([]);
  const [filtro, setFiltro] = useState({});
  const [somenteConsulta] = useState(false);
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

  const onClickVoltar = () => history.push('/');

  const onClickBotaoPrincipal = () => {
    history.push(`tipo-avaliacao/novo`);
  };

  const onSelecionarItems = items => {
    setItensSelecionados(items);
  };

  const onClickExcluir = async () => {
    const confirmado = await confirmar(
      '',
      'Deseja realmente excluir estes itens?'
    );
    if (confirmado) {
      const excluir = await servicoTipoAvaliaco.deletarTipoAvaliacao(
        itensSelecionados
      );
      if (excluir) {
        sucesso('Tipos de avaliação excluídos com sucesso.');
      } else {
        erro('Erro ao excluir tipos de avaliação.');
      }
    }
  };

  const onClickEditar = item => {
    history.push(`/configuracoes/tipo-avaliacao/editar/${item.id}`);
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
          temItemSelecionado={itensSelecionados && itensSelecionados.length}
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
        />
      </Card>
    </>
  );
};

export default TipoAvaliacaoLista;
