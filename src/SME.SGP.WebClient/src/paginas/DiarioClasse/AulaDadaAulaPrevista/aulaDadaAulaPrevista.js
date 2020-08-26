import React, { useState, useEffect } from 'react';
import { useSelector } from 'react-redux';
import { Titulo, TituloAno } from './aulaDadaAulaPrevista.css';
import Grid from '~/componentes/grid';
import Card from '~/componentes/card';
import Button from '~/componentes/button';
import SelectComponent from '~/componentes/select';
import ServicoDisciplina from '~/servicos/Paginas/ServicoDisciplina';
import { Colors, Auditoria, Loader } from '~/componentes';
import ListaAulasPorBimestre from './ListaAulasPorBimestre/ListaAulasPorBimestre';
import api from '~/servicos/api';
import Alert from '~/componentes/alert';
import RotasDto from '~/dtos/rotasDto';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import { confirmar, erros, sucesso, erro, exibirAlerta } from '~/servicos/alertas';
import { URL_HOME } from '~/constantes/url';
import history from '~/servicos/history';
import AlertaModalidadeInfantil from '~/componentes-sgp/AlertaModalidadeInfantil/alertaModalidadeInfantil';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';

const AulaDadaAulaPrevista = () => {
  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;
  const turmaId = turmaSelecionada ? turmaSelecionada.turma : 0;
  const periodo = turmaSelecionada ? turmaSelecionada.periodo : 0;
  const { modalidade } = turmaSelecionada;
  const anoLetivo = turmaSelecionada ? turmaSelecionada.anoLetivo : 0;
  const [desabilitarDisciplina, setDesabilitarDisciplina] = useState(false);
  const [listaDisciplinas, setListaDisciplinas] = useState([]);
  const [disciplinaSelecionada, setDisciplinaSelecionada] = useState(undefined);
  const [modoEdicao, setModoEdicao] = useState(false);
  const [disciplinaIdSelecionada, setDisciplinaIdSelecionada] = useState(
    undefined
  );
  const [dadoslista, setDadosLista] = useState([]);
  const [auditoria, setAuditoria] = useState(undefined);
  const permissoesTela = usuario.permissoes[RotasDto.AULA_DADA_AULA_PREVISTA];
  const [somenteConsulta, setSomenteConsulta] = useState(false);

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  useEffect(() => {
    const naoSetarSomenteConsultaNoStore = ehTurmaInfantil(
      modalidadesFiltroPrincipal,
      turmaSelecionada
    );
    setSomenteConsulta(
      verificaSomenteConsulta(permissoesTela, naoSetarSomenteConsultaNoStore)
    );
  }, [turmaSelecionada, permissoesTela, modalidadesFiltroPrincipal]);

  useEffect(() => {
    const obterDisciplinas = async () => {
      const disciplinas = await ServicoDisciplina.obterDisciplinasPorTurma(
        turmaId
      );
      setListaDisciplinas(disciplinas.data);
      if (disciplinas.data && disciplinas.data.length === 1) {
        const disciplina = disciplinas.data[0];
        setDisciplinaSelecionada(disciplina);
        onChangeDisciplinas(disciplina.codigoComponenteCurricular);
        setDesabilitarDisciplina(true);
      }
    };
    if (
      turmaId &&
      !ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada)
    ) {
      obterDisciplinas();
    } else {
      setDadosLista([]);
      setModoEdicao(false);
      setDisciplinaIdSelecionada(undefined);
      setListaDisciplinas([]);
    }
  }, [turmaSelecionada, modalidade, modalidadesFiltroPrincipal]);

  const perguntaAoSalvar = async () => {
    return confirmar(
      'Atenção',
      '',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
  };

  const [carregandoDados, setCarregandoDados] = useState(false);

  const verificarBimestreAtual = (dataInicio, dataFim) => {
    const dataAtual = window.moment(new Date());
    return (
      window.moment(dataInicio) <= dataAtual &&
      window.moment(dataFim) >= dataAtual
    );
  };

  const buscarDados = async disciplinaId => {
    setCarregandoDados(true);
    const resposta = await api.get(
      `v1/aula-prevista/modalidades/${modalidade}/turmas/${turmaId}/disciplinas/${disciplinaId}/semestres/${periodo}`
    );
    const dadosAula = resposta.data;
    let periodosFechados = '';
    if (dadosAula && dadosAula.aulasPrevistasPorBimestre) {
      const dadosBimestre = dadosAula.aulasPrevistasPorBimestre;
      let totalPrevistas = 0;
      let totalCriadasTitular = 0;
      let totalCriadasCj = 0;
      let totalDadas = 0;
      let totalRepostas = 0;
      dadosBimestre.forEach(item => {
        item.ehBimestreAtual = verificarBimestreAtual(item.inicio, item.fim);
        item.dadas = item.cumpridas;
        totalPrevistas += item.previstas.quantidade;
        totalCriadasTitular += item.criadas.quantidadeTitular;
        totalCriadasCj += item.criadas.quantidadeCJ;
        totalDadas += item.dadas;
        totalRepostas += item.reposicoes;
        if (item.previstas.mensagens && item.previstas.mensagens.length > 0) {
          item.previstas.temDivergencia = true;
        }
        periodosFechados += !item.podeEditar ?
          (periodosFechados.length > 0 ? `, ${item.bimestre}` : item.bimestre) : '';
      });
      const dados = {
        id: dadosAula.id,
        bimestres: dadosBimestre,
        totalPrevistas,
        totalCriadasTitular,
        totalCriadasCj,
        totalDadas,
        totalRepostas,
      };
      setDadosLista(dados);
      const aud = {
        alteradoRf: dados.alteradoRf,
        alteradoEm: dadosAula.alteradoEm,
        alteradoPor: dadosAula.alteradoPor,
        criadoRf: dadosAula.criadoRf,
        criadoEm: dadosAula.criadoEm,
        criadoPor: dadosAula.criadoPor,
      };
      if (periodosFechados.length > 0) {
        periodosFechados = periodosFechados.replace(/,(?=[^,]*$)/, ' e ');
        const mensagem = `Apenas é possível consultar o(s) registro(s) para o(s) bimestre(s) ${periodosFechados},
         pois seus períodos de fechamento estão encerrados.`;
        exibirAlerta('warning', mensagem);
      }
      setAuditoria(aud);
    }
    setCarregandoDados(false);
  };

  const resetarTela = () => {
    setModoEdicao(false);
    buscarDados(disciplinaIdSelecionada);
  };

  const salvar = async () => {
    const bimestresQuantidade = [];
    dadoslista.bimestres.forEach(item => {
      const dados = {
        bimestre: item.bimestre,
        quantidade: item.previstas.quantidade,
      };
      bimestresQuantidade.push(dados);
    });
    const dados = {
      bimestresQuantidade,
      disciplinaId: disciplinaIdSelecionada,
      modalidade,
      turmaId,
    };
    if (dadoslista.id) {
      await api
        .put(`v1/aula-prevista/${dadoslista.id}`, dados)
        .then(res => {
          if (res.status === 200)
            sucesso('Suas informações foram salvas com sucesso');
        })
        .catch(e => erros(e));
    } else {
      await api
        .post(`v1/aula-prevista`, dados)
        .then(res => {
          if (res.status === 200)
            sucesso('Suas informações foram salvas com sucesso');
          buscarDados();
          resetarTela();
        })
        .catch(e => erros(e));
    }
  };

  const onChangeDisciplinas = async disciplinaId => {
    if (modoEdicao) {
      const confirmarSalvar = await perguntaAoSalvar();
      if (confirmarSalvar) {
        await salvar();
      }
    }
    setDisciplinaIdSelecionada(String(disciplinaId));
    await buscarDados(disciplinaId);
  };

  const onClickVoltar = async () => {
    if (modoEdicao) {
      const confirmado = await perguntaAoSalvar();
      if (confirmado) {
        await salvar();
        history.push(URL_HOME);
      }
    } else {
      history.push(URL_HOME);
    }
  };

  const onClickCancelar = async () => {
    if (modoEdicao) {
      const confirmado = await confirmar(
        'Atenção',
        'Você não salvou as informações preenchidas.',
        'Deseja realmente cancelar as alterações?'
      );
      if (confirmado) {
        resetarTela();
      }
    }
  };

  const onClickSalvar = async () => {
    await salvar();
  };

  return (
    <>
      {!turmaSelecionada.turma &&
      !ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada) ? (
        <Grid cols={12} className="p-0">
          <Alert
            alerta={{
              tipo: 'warning',
              id: 'AlertaPrincipal',
              mensagem: 'Você precisa escolher uma turma.',
              estiloTitulo: { fontSize: '18px' },
            }}
            className="mb-2"
          />
        </Grid>
      ) : null}{' '}
      <AlertaModalidadeInfantil />
      <Grid cols={12} className="p-0">
        <Titulo>
          Aula prevista X Aula dada
          <TituloAno>
            {' '}
            {` / ${anoLetivo || new Date().getFullYear()}`}{' '}
          </TituloAno>{' '}
        </Titulo>{' '}
      </Grid>{' '}
      <Card>
        <div className="col-md-12">
          <div className="row">
            <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 mb-2">
              <SelectComponent
                id="disciplina"
                name="disciplinaId"
                lista={listaDisciplinas}
                valueOption="codigoComponenteCurricular"
                valueText="nome"
                valueSelect={disciplinaIdSelecionada}
                onChange={onChangeDisciplinas}
                placeholder="Selecione um componente curricular"
                disabled={desabilitarDisciplina || !turmaSelecionada.turma}
              />
            </div>
            <div className="col-sm-12 col-lg-8 col-md-8 d-flex justify-content-end pb-4">
              <Button
                label="Voltar"
                icon="arrow-left"
                color={Colors.Azul}
                border
                className="mr-2"
                onClick={onClickVoltar}
              />
              <Button
                label="Cancelar"
                color={Colors.Roxo}
                border
                className="mr-2"
                onClick={onClickCancelar}
                disabled={!modoEdicao || somenteConsulta}
              />
              <Button
                label="Salvar"
                color={Colors.Roxo}
                border
                bold
                className="mr-2"
                onClick={onClickSalvar}
                disabled={!modoEdicao || somenteConsulta}
              />
            </div>
            <div className="col-md-12">
              <Loader
                loading={disciplinaIdSelecionada && carregandoDados}
                className={`w-100 ${disciplinaIdSelecionada &&
                  carregandoDados &&
                  'p-3 text-center'}`}
              >
                {dadoslista && dadoslista.bimestres ? (
                  <ListaAulasPorBimestre
                    dados={dadoslista}
                    setModoEdicao={e => setModoEdicao(e)}
                    permissoesTela={permissoesTela}
                    somenteConsulta={somenteConsulta}
                  />
                ) : null}
              </Loader>
            </div>
            <div className="col-md-6 d-flex justify-content-start">
              {auditoria ? (
                <Auditoria
                  criadoEm={auditoria.criadoEm}
                  criadoPor={auditoria.criadoPor}
                  criadoRf={auditoria.criadoRf}
                  alteradoPor={auditoria.alteradoPor}
                  alteradoEm={auditoria.alteradoEm}
                  alteradoRf={auditoria.alteradoRf}
                />
              ) : (
                  ''
                )}
            </div>
          </div>
        </div>
      </Card>
    </>
  );
};

export default AulaDadaAulaPrevista;
