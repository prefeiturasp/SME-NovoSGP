import * as moment from 'moment';
import React, { useCallback, useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { CampoData, ListaPaginada, Loader } from '~/componentes';
import AlertaPermiteSomenteTurmaInfantil from '~/componentes-sgp/AlertaPermiteSomenteTurmaInfantil/alertaPermiteSomenteTurmaInfantil';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Alert from '~/componentes/alert';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import SelectComponent from '~/componentes/select';
import { URL_HOME } from '~/constantes/url';
import { erros } from '~/servicos/alertas';
import history from '~/servicos/history';
import ServicoDisciplina from '~/servicos/Paginas/ServicoDisciplina';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';
import RotasDto from '~/dtos/rotasDto';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import ServicoPeriodoEscolar from '~/servicos/Paginas/Calendario/ServicoPeriodoEscolar';

const DevolutivasLista = () => {
  const usuario = useSelector(state => state.usuario);
  const { turmaSelecionada } = usuario;

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );
  const turmaCodigo = turmaSelecionada ? turmaSelecionada.turma : 0;
  const [
    listaComponenteCurriculare,
    setListaComponenteCurriculare,
  ] = useState();
  const [
    componenteCurricularSelecionado,
    setComponenteCurricularSelecionado,
  ] = useState();
  const [dataSelecionada, setDataSelecionada] = useState();
  const [carregandoGeral, setCarregandoGeral] = useState(false);
  const [turmaInfantil, setTurmaInfantil] = useState(false);
  const [filtro, setFiltro] = useState({});
  const permissoesTela = usuario.permissoes[RotasDto.DEVOLUTIVAS];
  const [somenteConsulta, setSomenteConsulta] = useState(false);
  const [periodoHabilitado, setPeriodoHabilitado] = useState();

  useEffect(() => {
    const naoSetarSomenteConsultaNoStore = !ehTurmaInfantil(
      modalidadesFiltroPrincipal,
      turmaSelecionada
    );
    setSomenteConsulta(
      verificaSomenteConsulta(permissoesTela, naoSetarSomenteConsultaNoStore)
    );
    obterPeriodoLetivoTurma();
  }, [turmaSelecionada, permissoesTela, modalidadesFiltroPrincipal]);

  const obterPeriodoLetivoTurma = async() => {
    if (turmaSelecionada && turmaSelecionada.turma) {
      const periodoLetivoTurmaResponse = await ServicoPeriodoEscolar
        .obterPeriodoLetivoTurma(turmaSelecionada.turma).catch(e => erros(e));
      if (periodoLetivoTurmaResponse?.data) {
        var datas = [moment(periodoLetivoTurmaResponse.data.periodoInicio).format('YYYY-MM-DD')];
        var qtdDias = moment(periodoLetivoTurmaResponse.data.periodoFim).diff(periodoLetivoTurmaResponse.data.periodoInicio, 'days');
        for (let indice = 1; indice <= qtdDias; indice++) {
          var novaData = moment(periodoLetivoTurmaResponse.data.periodoInicio).add(indice, 'days');
          datas.push(novaData.format('YYYY-MM-DD'));
        }
        setPeriodoHabilitado(datas);
      }      
    }
  }
  
  const colunas = [
    {
      title: 'Intervalo de Datas',
      dataIndex: 'intervaloDatas',
      render: (valor, dados) => {
        const periodoInicio = dados
          ? moment(dados.periodoInicio).format('DD/MM/YYYY')
          : '';
        const periodoFim = dados
          ? moment(dados.periodoFim).format('DD/MM/YYYY')
          : '';
        return `${periodoInicio} - ${periodoFim}`;
      },
    },
    {
      title: 'Data de inclusão',
      dataIndex: 'criadoEm',
      render: valor => {
        return valor ? moment(valor).format('DD/MM/YYYY') : '';
      },
    },
    {
      title: 'Inserido por',
      dataIndex: 'criadoPor',
    },
  ];

  const resetarTela = useCallback(() => {
    setComponenteCurricularSelecionado(undefined);
    setDataSelecionada();
  }, []);

  useEffect(() => {
    const infantil = ehTurmaInfantil(
      modalidadesFiltroPrincipal,
      turmaSelecionada
    );
    setTurmaInfantil(infantil);

    if (!turmaInfantil) {
      resetarTela();
    }
  }, [
    turmaSelecionada,
    modalidadesFiltroPrincipal,
    turmaInfantil,
    resetarTela,
  ]);

  useEffect(() => {
    resetarTela();
  }, [turmaSelecionada.turma, resetarTela]);

  const obterComponentesCurriculares = useCallback(async () => {
    setCarregandoGeral(true);
    const componentes = await ServicoDisciplina.obterDisciplinasPorTurma(
      turmaCodigo
    ).catch(e => erros(e));

    if (componentes.data && componentes.data.length) {
      setListaComponenteCurriculare(componentes.data);

      if (componentes.data.length === 1) {
        const componente = componentes.data[0];
        setComponenteCurricularSelecionado(
          String(componente.codigoComponenteCurricular)
        );
      }
    }

    setCarregandoGeral(false);
  }, [turmaCodigo]);

  useEffect(() => {
    if (turmaCodigo && turmaInfantil) {
      obterComponentesCurriculares();
    } else {
      setListaComponenteCurriculare([]);
      resetarTela();
    }
  }, [turmaCodigo, obterComponentesCurriculares, turmaInfantil, resetarTela]);

  useEffect(() => {
    const paramsFiltrar = {
      componenteCurricularCodigo: componenteCurricularSelecionado,
    };
    setFiltro({ ...paramsFiltrar });
  }, [componenteCurricularSelecionado]);

  const filtrar = (data, componenteCurricularCodigo) => {
    const paramsFiltrar = {
      dataReferencia: data ? data.format('YYYY-MM-DD') : '',
      componenteCurricularCodigo,
    };
    setFiltro({ ...paramsFiltrar });
  };

  const onChangeComponenteCurricular = valor => {
    setDataSelecionada('');
    setComponenteCurricularSelecionado(valor);
    filtrar(dataSelecionada, valor);
  };

  const onChangeData = async data => {
    setDataSelecionada(data);
    filtrar(data, componenteCurricularSelecionado);
  };

  const onClickVoltar = () => {
    history.push(URL_HOME);
  };

  const onClickNovo = () => {
    history.push(`${RotasDto.DEVOLUTIVAS}/novo`);
  };
  const onClickEditar = item => {    
      history.push(`${RotasDto.DEVOLUTIVAS}/editar/${item.id}`);
  };

  return (
    <Loader loading={carregandoGeral} className="w-100 my-2">
      {!turmaSelecionada.turma ? (
        <Alert
          alerta={{
            tipo: 'warning',
            id: 'diario-devolutivas-selecione-turma',
            mensagem: 'Você precisa escolher uma turma',
          }}
          className="mb-2"
        />
      ) : (
        ''
      )}
      {turmaSelecionada.turma ? <AlertaPermiteSomenteTurmaInfantil /> : ''}
      <Cabecalho pagina="Devolutivas" />
      <Card>
        <div className="col-md-12">
          <div className="row">
            <div className="col-md-12 d-flex justify-content-end pb-4">
              <Button
                id="btn-voltar-devolutivas"
                label="Voltar"
                icon="arrow-left"
                color={Colors.Azul}
                border
                className="mr-3"
                onClick={onClickVoltar}
              />
              <Button
                id="btn-nova-devolutivas"
                label="Nova"
                color={Colors.Roxo}
                border
                bold
                onClick={onClickNovo}
                disabled={
                  !turmaInfantil ||
                  somenteConsulta ||
                  !permissoesTela.podeIncluir ||
                  !turmaSelecionada.turma
                }
              />
            </div>
            <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 mb-2">
              <SelectComponent
                id="disciplina"
                lista={listaComponenteCurriculare || []}
                valueOption="codigoComponenteCurricular"
                valueText="nome"
                valueSelect={componenteCurricularSelecionado}
                onChange={onChangeComponenteCurricular}
                placeholder="Selecione um componente curricular"
                disabled={
                  !turmaInfantil ||
                  !turmaSelecionada.turma ||
                  (listaComponenteCurriculare &&
                    listaComponenteCurriculare.length === 1)
                }
              />
            </div>
            <div className="col-sm-12 col-md-4 col-lg-3 col-xl-3 mb-3">
              <CampoData
                valor={dataSelecionada}
                onChange={onChangeData}
                placeholder="DD/MM/AAAA"
                formatoData="DD/MM/YYYY"
                desabilitado={
                  !turmaInfantil ||
                  !listaComponenteCurriculare ||
                  !componenteCurricularSelecionado
                }
                diasParaHabilitar={periodoHabilitado}
              />
            </div>
          </div>
        </div>
        {componenteCurricularSelecionado &&
        filtro &&
        filtro.componenteCurricularCodigo ? (
          <div className="col-md-12 pt-2">
            <ListaPaginada
              url={`v1/devolutivas/turmas/${turmaCodigo}`}
              id="lista-devolutivas"
              colunas={colunas}
              filtro={filtro}
              onClick={onClickEditar}
            />
          </div>
        ) : (
          ''
        )}
      </Card>
    </Loader>
  );
};

export default DevolutivasLista;
