import React, { useState, useEffect } from 'react';
import { useSelector } from 'react-redux';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Card from '~/componentes/card';
import { ListaPaginada, ButtonGroup } from '~/componentes';
import history from '~/servicos/history';
import RotasDto from '~/dtos/rotasDto';
import Filtro from './componentes/Filtro';
import servicoTipoAvaliaco from '~/servicos/Paginas/TipoAvaliacao';
import { sucesso, confirmar, erro, erros } from '~/servicos/alertas';

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
      render: item => {
        return item.replace(/<[^>]*>?/gm, '');
      },
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
    if (itensSelecionados && itensSelecionados.length > 0) {
      const confirmado = await confirmar(
        '',
        'Deseja realmente excluir estes itens?'
      );
      if (confirmado) {
        const idsDeletar = itensSelecionados.map(c => c.id);
        const excluir = await servicoTipoAvaliaco
          .deletarTipoAvaliacao(idsDeletar)
          .catch(e => erros(e));
        if (excluir && excluir.status == 200) {
          sucesso('Tipos de avaliação excluídos com sucesso.');
        } else {
          erro('Erro ao excluir tipos de avaliação.');
        }
      }
    }
  };

  // const onClickExcluir = async () => {
  //   if (eventosSelecionados && eventosSelecionados.length > 0) {
  //     const listaNomeExcluir = eventosSelecionados.map(item => item.nome);
  //     const confirmado = await confirmar(
  //       'Excluir evento',
  //       listaNomeExcluir,
  //       `Deseja realmente excluir ${
  //         eventosSelecionados.length > 1 ? 'estes eventos' : 'este evento'
  //       }?`,
  //       'Excluir',
  //       'Cancelar'
  //     );
  //     if (confirmado) {
  //       const idsDeletar = eventosSelecionados.map(c => c.id);
  //       const excluir = await servicoEvento
  //         .deletar(idsDeletar)
  //         .catch(e => erros(e));
  //       if (excluir && excluir.status == 200) {
  //         const mensagemSucesso = `${
  //           eventosSelecionados.length > 1
  //             ? 'Eventos excluídos'
  //             : 'Evento excluído'
  //         } com sucesso.`;
  //         sucesso(mensagemSucesso);
  //         validaFiltrar();
  //       }
  //     }
  //   }
  // };

  const onClickEditar = item => {
    history.push(`/configuracoes/tipo-avaliacao/editar/${item.id}`);
  };

  const onChangeFiltro = valoresFiltro => {
    if (valoresFiltro.nome === '') {
      delete valoresFiltro.nome;
    }
    if (valoresFiltro.descricao === '') {
      delete valoresFiltro.descricao;
    }
    if (valoresFiltro.situacao === '') {
      delete valoresFiltro.situacao;
    }

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
          selecionarItems={onSelecionarItems}
          filtroEhValido
          multiSelecao
        />
      </Card>
    </>
  );
};

export default TipoAvaliacaoLista;
