import React, { useEffect, useState, useCallback } from 'react';
import * as moment from 'moment';

// Redux
import { useSelector } from 'react-redux';

// Componentes SGP
import { Cabecalho } from '~/componentes-sgp';

// Componentes
import { Card, ListaPaginada, ButtonGroup } from '~/componentes';

// Serviços
import { confirmar, erros, sucesso } from '~/servicos/alertas';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import ServicoEvento from '~/servicos/Paginas/Calendario/ServicoEvento';

import { URL_HOME } from '~/constantes/url';
import RotasDto from '~/dtos/rotasDto';
import history from '~/servicos/history';

// Componentes Locais
import Filtro from './components/Filtro';
import AlertaSelecionarTipo from './components/AlertaSelecionarTipo';

// Funcões
import { valorNuloOuVazio } from '~/utils/funcoes/gerais';

const EventosLista = () => {
  const usuario = useSelector(store => store.usuario);
  const permissoesTela = usuario.permissoes[RotasDto.EVENTOS];

  const [somenteConsulta, setSomenteConsulta] = useState(false);

  const [eventosSelecionados, setEventosSelecionados] = useState([]);
  const [filtro, setFiltro] = useState({});
  const [selecionouCalendario, setSelecionouCalendario] = useState(false);

  const [filtroValido, setFiltroValido] = useState({ valido: false });

  const [estaCarregando, setEstaCarregando] = useState(false);

  const formatarCampoDataGrid = data => {
    let dataFormatada = '';
    if (data) {
      dataFormatada = moment(data).format('DD/MM/YYYY');
    }
    return <span> {dataFormatada}</span>;
  };

  const colunas = [
    {
      title: 'Nome do evento',
      dataIndex: 'nome',
      width: '45%',
    },
    {
      title: 'Tipo de evento',
      dataIndex: 'tipo',
      width: '20%',
      render: (text, row) => <span> {row.tipoEvento.descricao}</span>,
    },
    {
      title: 'Data início',
      dataIndex: 'dataInicio',
      width: '15%',
      render: data => formatarCampoDataGrid(data),
    },
    {
      title: 'Data fim',
      dataIndex: 'dataFim',
      width: '15%',
      render: data => formatarCampoDataGrid(data),
    },
  ];

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  const onClickExcluir = async () => {
    if (eventosSelecionados && eventosSelecionados.length > 0) {
      const listaNomeExcluir = eventosSelecionados.map(item => item.nome);
      const confirmado = await confirmar(
        'Excluir evento',
        listaNomeExcluir,
        `Deseja realmente excluir ${
          eventosSelecionados.length > 1 ? 'estes eventos' : 'este evento'
        }?`,
        'Excluir',
        'Cancelar'
      );
      if (confirmado) {
        const idsDeletar = eventosSelecionados.map(c => c.id);
        const excluir = await ServicoEvento.deletar(idsDeletar).catch(e =>
          erros(e)
        );
        if (excluir && excluir.status === 200) {
          const mensagemSucesso = `${
            eventosSelecionados.length > 1
              ? 'Eventos excluídos'
              : 'Evento excluído'
          } com sucesso.`;
          sucesso(mensagemSucesso);
          setEventosSelecionados([]);
          setFiltro(atual => ({
            ...atual,
            atualizar: !atual.atualizar || true,
          }));
        }
      }
    }
  };

  const onClickNovo = () => {
    const calendarioId = filtro.tipoCalendarioId;
    history.push(`/calendario-escolar/eventos/novo/${calendarioId}`);
  };

  const onClickEditar = evento => {
    history.push(
      `/calendario-escolar/eventos/editar/${evento.id}/${filtro.tipoCalendarioId}`
    );
  };

  const onSelecionarItems = items => {
    setEventosSelecionados(items);
  };

  const onFiltrar = useCallback(
    valores => {
      if (!valorNuloOuVazio(valores.tipoCalendarioId) && !estaCarregando) {
        setSelecionouCalendario(true);
        setFiltroValido({ valido: true });
        setFiltro({
          ...valores,
          dataInicio: valores.dataInicio && valores.dataInicio.toDate(),
          dataFim: valores.dataFim && valores.dataFim.toDate(),
          ehTodasDres: valores.dreId === '0' && true,
          ehTodasUes: valores.dreId === '0' && true,
        });
      } else {
        setSelecionouCalendario(false);
        setFiltroValido({ valido: false });
      }
    },
    [estaCarregando]
  );

  useEffect(() => {
    setSomenteConsulta(verificaSomenteConsulta(permissoesTela));
  }, [permissoesTela]);

  return (
    <>
      <AlertaSelecionarTipo filtro={filtro} />
      <Cabecalho pagina="Eventos do calendário escolar" />
      <Card>
        <ButtonGroup
          somenteConsulta={somenteConsulta}
          permissoesTela={permissoesTela}
          temItemSelecionado={
            eventosSelecionados && eventosSelecionados.length > 0
          }
          onClickVoltar={onClickVoltar}
          onClickExcluir={onClickExcluir}
          onClickBotaoPrincipal={onClickNovo}
          labelBotaoPrincipal="Novo"
          desabilitarBotaoPrincipal={!filtro.tipoCalendarioId}
        />
        <Filtro onFiltrar={ftr => onFiltrar(ftr)} />
        <div className="col-md-12 pt-2">
          {selecionouCalendario ? (
            <ListaPaginada
              url="v1/calendarios/eventos"
              id="lista-eventos"
              colunaChave="id"
              colunas={colunas}
              filtro={filtro}
              onClick={onClickEditar}
              multiSelecao
              selecionarItems={onSelecionarItems}
              filtroEhValido={filtroValido.valido}
              onCarregando={valor => setEstaCarregando(valor)}
            />
          ) : (
            ''
          )}
        </div>
      </Card>
    </>
  );
};

export default EventosLista;
