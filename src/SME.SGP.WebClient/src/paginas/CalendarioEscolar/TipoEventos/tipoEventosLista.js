import React, { useState, useRef, useEffect } from 'react';
import { useSelector } from 'react-redux';
import styled from 'styled-components';
import RotasDto from '~/dtos/rotasDto';
import Card from '~/componentes/card';
import Grid from '~/componentes/grid';
import Button from '~/componentes/button';
import { Base, Colors } from '~/componentes/colors';
import SelectComponent from '~/componentes/select';
import history from '~/servicos/history';
import { confirmar, sucesso, erros } from '~/servicos/alertas';
import ListaPaginada from '~/componentes/listaPaginada/listaPaginada';
import api from '~/servicos/api';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';

const TipoEventosLista = () => {
  const Div = styled.div`
    .select-local {
    }
  `;

  const Titulo = styled(Div)`
    color: ${Base.CinzaMako};
    font-size: 24px;
  `;

  const Icone = styled.i`
    color: ${Base.CinzaMako};
    cursor: pointer;
  `;

  const Busca = styled(Icone)`
    left: 10px;
    max-height: 25px;
    max-width: 15px;
    padding: 1rem;
    right: 0;
    top: -2px;
  `;

  const CampoTexto = styled.input`
    color: ${Base.CinzaBotao};
    font-size: 14px;
    padding-left: 40px;
    &[type='radio'] {
      background: ${Base.Branco};
      border: 1px solid ${Base.CinzaDesabilitado};
    }
  `;

  const usuario = useSelector(store => store.usuario);
  const permissoesTela = usuario.permissoes[RotasDto.TIPO_EVENTOS];

  const [desabilitarBotaoExcluir, setDesabilitarBotaoExcluir] = useState(true);
  const [tipoEventoSelecionados, setTipoEventoSelecionados] = useState([]);

  const clicouBotaoVoltar = () => {
    history.push('/');
  };

  const clicouBotaoExcluir = async () => {
    if (tipoEventoSelecionados && tipoEventoSelecionados.length > 0) {
      const listaNomesExcluir = tipoEventoSelecionados.map(
        tipo => tipo.descricao
      );

      const confirmado = await confirmar(
        'Atenção',
        listaNomesExcluir,
        'Você tem certeza que deseja excluir estes itens?',
        'Excluir',
        'Cancelar'
      );
      if (confirmado) {
        const idsDeletar = tipoEventoSelecionados.map(tipo => tipo.id);

        api
          .delete('v1/calendarios/eventos/tipos', {
            data: idsDeletar,
          })
          .then(resposta => {
            if (resposta) sucesso('Tipos de evento deletados com sucesso!');
          })
          .catch(e => erros(e));
      }
    }
  };

  useEffect(() => {
    verificaSomenteConsulta(permissoesTela);
  }, []);

  useEffect(() => {
    if (
      tipoEventoSelecionados &&
      tipoEventoSelecionados.length > 0 &&
      permissoesTela.podeExcluir
    )
      setDesabilitarBotaoExcluir(false);
    else setDesabilitarBotaoExcluir(true);
  }, [tipoEventoSelecionados]);

  const clicouBotaoNovo = () => {
    if (permissoesTela.podeIncluir)
      history.push('/calendario-escolar/tipo-eventos/novo');
  };

  const clicouBotaoEditar = tipoEvento => {
    if (permissoesTela.podeAlterar)
      history.push(`/calendario-escolar/tipo-eventos/editar/${tipoEvento.id}`);
  };

  const listaLetivo = [
    { valor: 1, descricao: 'Sim' },
    { valor: 2, descricao: 'Não' },
    { valor: 3, descricao: 'Opcional' },
  ];

  const listaLocalOcorrencia = [
    { valor: 1, descricao: 'UE' },
    { valor: 2, descricao: 'DRE' },
    { valor: 3, descricao: 'SME' },
    { valor: 4, descricao: 'SME/UE' },
    { valor: 5, descricao: 'Todos' },
  ];

  const colunas = [
    {
      title: 'Tipo de Evento',
      dataIndex: 'descricao',
      className: 'text-left px-4',
    },
    {
      title: 'Local de ocorrência',
      dataIndex: 'localOcorrencia',
      className: 'text-left px-4',
      render: localOcorrencia =>
        listaLocalOcorrencia.filter(l => l.valor === localOcorrencia)[0]
          .descricao,
    },
    {
      title: 'Letivo',
      dataIndex: 'letivo',
      className: 'text-left px-4',
      render: letivo =>
        listaLetivo.filter(l => l.valor === letivo)[0].descricao,
    },
  ];

  const campoNomeTipoEventoRef = useRef();

  const [filtro, setFiltro] = useState({});
  const [
    localOcorrenciaSelecionado,
    setLocalOcorrenciaSelecionado,
  ] = useState();

  const aoSelecionarLocalOcorrencia = local => {
    setLocalOcorrenciaSelecionado(local);
    setFiltro({ ...filtro, localOcorrencia: local });
  };

  const [letivoSelecionado, setLetivoSelecionado] = useState();

  const aoSelecionarLetivo = letivo => {
    setLetivoSelecionado(letivo);
    setFiltro({ ...filtro, letivo });
  };

  const [nomeTipoEvento, setNomeTipoEvento] = useState('');

  const aoDigitarNomeTipoEvento = () => {
    setNomeTipoEvento(campoNomeTipoEventoRef.current.value);
    setFiltro({ ...filtro, descricao: campoNomeTipoEventoRef.current.value });
  };

  useEffect(() => {
    setTipoEventoSelecionados([]);
  }, [filtro]);

  useEffect(() => {
    campoNomeTipoEventoRef.current.focus();
  }, [nomeTipoEvento]);

  const aoSelecionarItems = items => {
    setTipoEventoSelecionados(items);
  };

  return (
    <Div className="col-12">
      <Grid cols={12} className="mb-1 p-0">
        <Titulo className="font-weight-bold">Tipo de eventos</Titulo>
      </Grid>
      <Card className="rounded" mx="mx-auto">
        <Div className="row w-100 mx-auto mb-5">
          <Div className="col-12 d-flex justify-content-end">
            <Button
              label="Voltar"
              Icone="arrow-left"
              color={Colors.Azul}
              onClick={clicouBotaoVoltar}
              border
              className="mr-3"
            />
            <Button
              label="Excluir"
              color={Colors.Roxo}
              onClick={clicouBotaoExcluir}
              disabled={desabilitarBotaoExcluir}
              border
              bold
              className="mr-3"
            />
            <Button
              label="Novo"
              color={Colors.Roxo}
              onClick={clicouBotaoNovo}
              disabled={!permissoesTela.podeIncluir}
              bold
            />
          </Div>
        </Div>
        <Div className="row mb-3 w-100 mx-auto">
          <Div className="col-4">
            <SelectComponent
              placeholder="Local de ocorrência"
              valueOption="valor"
              valueText="descricao"
              lista={listaLocalOcorrencia}
              valueSelect={localOcorrenciaSelecionado}
              onChange={aoSelecionarLocalOcorrencia}
              className="select-local"
            />
          </Div>
          <Div className="col-3">
            <SelectComponent
              placeholder="Letivo"
              valueOption="valor"
              valueText="descricao"
              lista={listaLetivo}
              valueSelect={letivoSelecionado}
              onChange={aoSelecionarLetivo}
            />
          </Div>
          <Div className="col-5 position-relative">
            <Busca className="fa fa-search fa-lg bg-transparent position-absolute text-center" />
            <CampoTexto
              className="form-control form-control-lg"
              placeholder="Digite o nome do tipo de evento"
              onChange={aoDigitarNomeTipoEvento}
              value={nomeTipoEvento}
              ref={campoNomeTipoEventoRef}
            />
          </Div>
        </Div>
        <Grid cols={12} className="mb-4">
          <ListaPaginada
            url="v1/calendarios/eventos/tipos/listar"
            id="lista-tipo-eventos"
            colunaChave="id"
            colunas={colunas}
            filtro={filtro}
            onClick={clicouBotaoEditar}
            multiSelecao
            selecionarItems={aoSelecionarItems}
          />
        </Grid>
      </Card>
    </Div>
  );
};

export default TipoEventosLista;
