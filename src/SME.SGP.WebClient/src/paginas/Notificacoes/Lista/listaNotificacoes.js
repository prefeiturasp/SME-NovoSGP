import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import * as moment from 'moment';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Button from '~/componentes/button';
import CampoTexto from '~/componentes/campoTexto';
import { Colors } from '~/componentes/colors';
import SelectComponent from '~/componentes/select';
import ListaPaginada from '~/componentes/listaPaginada/listaPaginada';
import { confirmar, erro, sucesso } from '~/servicos/alertas';
import api from '~/servicos/api';
import history from '~/servicos/history';
import servicoNotificacao from '~/servicos/Paginas/ServicoNotificacao';

import { EstiloLista } from './estiloLista';
import notificacaoStatus from '~/dtos/notificacaoStatus';
import CampoTextoBusca from '~/componentes/campoTextoBusca';
import { URL_HOME } from '~/constantes/url';
import RotasDto from '~/dtos/rotasDto';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import { validaSeObjetoEhNuloOuVazio } from '~/utils/funcoes/gerais';

import { Loader } from '~/componentes';

export default function NotificacoesLista() {
  const [idNotificacoesSelecionadas, setIdNotificacoesSelecionadas] = useState(
    []
  );

  const colunas = [
    {
      title: 'Código',
      dataIndex: 'codigo',
      render: (text, row) => montarLinhasTabela(text, row),
      align: 'center',
    },
    {
      title: 'Tipo',
      dataIndex: 'tipo',
      render: (text, row) => montarLinhasTabela(text, row),
    },
    {
      title: 'Categoria',
      dataIndex: 'descricaoCategoria',
      render: (text, row) => montarLinhasTabela(text, row),
    },
    {
      title: 'Título',
      dataIndex: 'titulo',
      render: (text, row) => montarLinhasTabela(text, row),
    },
    {
      title: 'Situação',
      dataIndex: 'descricaoStatus',
      render: (text, row) => montarLinhasTabela(text, row, true),
    },
    {
      title: 'Data/Hora',
      dataIndex: 'data',
      width: 200,
      align: 'center',
      render: (text, row) => {
        const dataFormatada = moment(text).format('DD/MM/YYYY HH:mm:ss');
        return montarLinhasTabela(dataFormatada, row);
      },
    },
  ];

  const usuario = useSelector(store => store.usuario);
  const turmaSelecionada = useSelector(store => store.usuario.turmaSelecionada);

  const [listaCategorias, setListaCategorias] = useState([]);
  const [listaStatus, setListaStatus] = useState([]);
  const [listaTipos, setTipos] = useState([]);
  const [filtro, setFiltro] = useState({
    usuarioRf: usuario.rf,
  });

  const [dropdownTurmaSelecionada, setTurmaSelecionada] = useState();
  const [statusSelecionado, setStatusSelecionado] = useState();
  const [categoriaSelecionada, setCategoriaSelecionada] = useState();
  const [tipoSelecionado, setTipoSelecionado] = useState();
  const [tituloSelecionado, setTituloSelecionado] = useState();
  const [codigoSelecionado, setCodigoSelecionado] = useState();
  const [desabilitarBotaoExcluir, setDesabilitarBotaoExcluir] = useState(true);
  const [desabilitarBotaoMarcarLido, setDesabilitarBotaoMarcarLido] = useState(
    true
  );
  const [carregandoTela, setCarregandoTela] = useState(false);
  const [desabilitarTurma, setDesabilitarTurma] = useState(true);
  const [colunasTabela, setColunasTabela] = useState([]);

  const permissoesTela = usuario.permissoes[RotasDto.NOTIFICACOES];

  async function carregarListas() {
    const status = await api.get('v1/notificacoes/status');
    setListaStatus(status.data);

    const categorias = await api.get('v1/notificacoes/categorias');
    setListaCategorias(categorias.data);

    const tipos = await api.get('v1/notificacoes/tipos');
    setTipos(tipos.data);
  }

  useEffect(() => {
    setColunasTabela(colunas);
    verificaSomenteConsulta(permissoesTela);
    setDesabilitarBotaoExcluir(permissoesTela.podeExcluir);
    carregarListas();
  }, []);

  useEffect(() => {
    if (usuario && turmaSelecionada) {
      setDesabilitarTurma(false);
    } else {
      setDesabilitarTurma(true);
      setTurmaSelecionada('');
    }
    onClickFiltrar();
  }, [turmaSelecionada]);

  useEffect(() => {
    onClickFiltrar();
  }, [
    statusSelecionado,
    dropdownTurmaSelecionada,
    categoriaSelecionada,
    tipoSelecionado,
    tituloSelecionado,
  ]);

  const listaSelectTurma = [
    { id: 1, descricao: 'Todas as turmas' },
    { id: 2, descricao: 'Turma selecionada' },
  ];

  const statusLista = ['', 'Não lida', 'Lida', 'Aceita', 'Recusada'];

  function montarLinhasTabela(text, row, colunaSituacao) {
    return colunaSituacao ? (
      <span
        className={
          row.status === notificacaoStatus.Pendente
            ? 'cor-vermelho font-weight-bold text-uppercase'
            : 'cor-novo-registro-lista'
        }
      >
        {statusLista[row.status]}
      </span>
    ) : (
      text
    );
  }
  const onSelecionarItems = items => {
    if (items && items.length > 0) {
      const naoPodeRemover = items.find(item => !item.podeRemover);
      if (naoPodeRemover) {
        setDesabilitarBotaoExcluir(true);
      } else {
        setDesabilitarBotaoExcluir(false);
      }

      const naoPodeMarcarLido = items.find(item => !item.podeMarcarComoLida);
      if (naoPodeMarcarLido) {
        setDesabilitarBotaoMarcarLido(true);
      } else {
        setDesabilitarBotaoMarcarLido(false);
      }
    } else {
      setDesabilitarBotaoExcluir(true);
      setDesabilitarBotaoMarcarLido(true);
    }

    setIdNotificacoesSelecionadas(items.map(c => c.id));
  };

  function onChangeTurma(turma) {
    setTurmaSelecionada(turma);
  }

  function onChangeStatus(status) {
    setStatusSelecionado(status);
  }

  function onChangeCategoria(categoria) {
    setCategoriaSelecionada(categoria);
  }

  function onChangeTitulo(titulo) {
    setTituloSelecionado(titulo.target.value);
  }

  function onSearchCodigo() {
    onClickFiltrar();
  }

  function onChangeCodigo(codigo) {
    setCodigoSelecionado(codigo.target.value);
  }

  function onChangeTipo(tipo) {
    setTipoSelecionado(tipo);
  }

  function onClickEditar(notificacao) {
    if (!permissoesTela.podeAlterar) return;

    history.push(`/notificacoes/${notificacao.id}`);
  }

  const filtrarNotificacoes = async () => {
    const paramsQuery = {
      categoria: categoriaSelecionada,
      codigo: codigoSelecionado || null,
      status: statusSelecionado,
      tipo: tipoSelecionado,
      titulo: tituloSelecionado || null,
      usuarioRf: usuario.rf || null,
      anoLetivo: usuario.filtroAtual.anoLetivo,
    };
    if (dropdownTurmaSelecionada && dropdownTurmaSelecionada == '2') {
      if (turmaSelecionada) {
        paramsQuery.ano = turmaSelecionada.ano;
        paramsQuery.dreId = turmaSelecionada.dre;
        paramsQuery.ueId = turmaSelecionada.unidadeEscolar;
      }
      if (turmaSelecionada && !desabilitarTurma) {
        paramsQuery.turmaId = turmaSelecionada.unidadeEscolar;
      }
    }
    setFiltro(paramsQuery);
  };

  async function onClickFiltrar() {
    filtrarNotificacoes();
  }

  const validarFiltro = () => {
    return !validaSeObjetoEhNuloOuVazio(filtro);
  };

  async function marcarComoLida() {
    if (!permissoesTela.podeAlterar) return;

    try {
      setCarregandoTela(true);
      const { data, status } = await servicoNotificacao.marcarComoLidaNot(
        idNotificacoesSelecionadas
      );
      if (data && status === 200) {
        data.forEach(resultado => {
          if (resultado.sucesso) {
            sucesso(resultado.mensagem);
          } else {
            erro(resultado.mensagem);
          }
        });
        onClickFiltrar();
        setCarregandoTela(false);
      }
    } catch (error) {
      setCarregandoTela(false);
      erro('Não foi possível marcar como lida.');
    }
  }

  async function excluir() {
    if (!permissoesTela.podeExcluir) return;

    const confirmado = await confirmar(
      'Atenção',
      'Você tem certeza que deseja excluir estas notificações?'
    );
    if (confirmado) {
      try {
        setCarregandoTela(true);
        const { data, status } = await servicoNotificacao.excluirNot(
          idNotificacoesSelecionadas
        );
        if ((data !== null || data !== undefined) && status === 200) {
          data.forEach(resultado => {
            if (resultado.sucesso) {
              sucesso(resultado.mensagem);
            } else {
              erro(resultado.mensagem);
            }
          });
          onClickFiltrar();
          setIdNotificacoesSelecionadas([]);
          setDesabilitarBotaoExcluir(true);
          setCarregandoTela(false);
        }
      } catch (error) {
        setCarregandoTela(false);
        erro('Não foi possível excluir notificação.');
      }
    }
  }

  function quandoTeclaParaBaixoPesquisaCodigo(e) {
    if (e.key === 'e' || e.key === '-') e.preventDefault();
  }

  function quandoClicarVoltar() {
    history.push(URL_HOME);
  }

  return (
    <>
      <Cabecalho pagina="Notificações" />
      <Loader loading={carregandoTela} tip="Carregando...">
        <EstiloLista>
          <div className="col-md-6 pb-3">
            <CampoTexto
              placeholder="Título"
              onChange={onChangeTitulo}
              value={tituloSelecionado}
              desabilitado={!permissoesTela.podeConsultar}
            />
          </div>
          <div className="col-md-6 pb-3">
            <CampoTextoBusca
              placeholder="Código"
              onSearch={onSearchCodigo}
              onChange={onChangeCodigo}
              value={codigoSelecionado}
              desabilitado={!permissoesTela.podeConsultar}
              onKeyDown={quandoTeclaParaBaixoPesquisaCodigo}
              type="number"
              minValue="0"
            />
          </div>
          <div className="col-md-3 pb-3">
            <SelectComponent
              name="turma-noti"
              id="turma-noti"
              lista={listaSelectTurma}
              valueOption="id"
              valueText="descricao"
              onChange={onChangeTurma}
              valueSelect={dropdownTurmaSelecionada || []}
              placeholder="Turma"
              disabled={desabilitarTurma || !permissoesTela.podeConsultar}
            />
          </div>
          <div className="col-md-3 pb-3">
            <SelectComponent
              name="status-noti"
              id="status-noti"
              lista={listaStatus}
              valueOption="id"
              disabled={!permissoesTela.podeConsultar}
              valueText="descricao"
              onChange={onChangeStatus}
              valueSelect={statusSelecionado || []}
              placeholder="Filtrar por"
            />
          </div>
          <div className="col-md-3 pb-3">
            <SelectComponent
              name="categoria-noti"
              id="categoria-noti"
              lista={listaCategorias}
              valueOption="id"
              disabled={!permissoesTela.podeConsultar}
              valueText="descricao"
              onChange={onChangeCategoria}
              valueSelect={categoriaSelecionada || []}
              placeholder="Categoria"
            />
          </div>
          <div className="col-md-3 pb-3">
            <SelectComponent
              name="tipo-noti"
              id="tipo-noti"
              lista={listaTipos}
              valueOption="id"
              valueText="descricao"
              disabled={!permissoesTela.podeConsultar}
              onChange={onChangeTipo}
              valueSelect={tipoSelecionado || []}
              placeholder="Tipo"
            />
          </div>
          <div className="col-md-12 ">
            <Button
              label="Excluir"
              color={Colors.Vermelho}
              border
              className="mb-2 ml-2 float-right"
              onClick={excluir}
              disabled={
                (idNotificacoesSelecionadas &&
                  idNotificacoesSelecionadas.length < 1) ||
                !permissoesTela.podeExcluir ||
                desabilitarBotaoExcluir
              }
            />
            <Button
              label="Marcar como lida"
              color={Colors.Azul}
              border
              className="mb-2 ml-2 float-right"
              onClick={marcarComoLida}
              disabled={
                desabilitarBotaoMarcarLido || !permissoesTela.podeAlterar
              }
            />
            <Button
              label="Voltar"
              color={Colors.Azul}
              border
              className="mb-2 float-right"
              onClick={quandoClicarVoltar}
            />
          </div>
          <div className="col-md-12 pt-2">
            <ListaPaginada
              url="v1/notificacoes/"
              id="lista-notificacoes"
              colunas={colunasTabela}
              filtro={filtro}
              onClick={permissoesTela.podeAlterar && onClickEditar}
              multiSelecao
              selecionarItems={onSelecionarItems}
              filtroEhValido={validarFiltro()}
            />
          </div>
        </EstiloLista>
      </Loader>
    </>
  );
}
