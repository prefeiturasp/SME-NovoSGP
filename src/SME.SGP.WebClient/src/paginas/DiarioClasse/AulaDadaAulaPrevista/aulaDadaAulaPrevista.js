import React, { useState, useEffect } from 'react';
import { Titulo, TituloAno } from './aulaDadaAulaPrevista.css';
import Grid from '~/componentes/grid';
import Card from '~/componentes/card';
import Button from '~/componentes/button';
import SelectComponent from '~/componentes/select';
import { useSelector } from 'react-redux';
import ServicoDisciplina from '~/servicos/Paginas/ServicoDisciplina';
import { Colors, Auditoria } from '~/componentes';
import ListaAulasPorBimestre from './ListaAulasPorBimestre/ListaAulasPorBimestre';
import api from '~/servicos/api';
import Alert from '~/componentes/alert';
import RotasDto from '~/dtos/rotasDto';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import { confirmar, erros } from '~/servicos/alertas';
import { URL_HOME } from '~/constantes/url';
import history from '~/servicos/history';

const AulaDadaAulaPrevista = () => {
  const usuario = useSelector(store => store.usuario);
  const turmaSelecionada = usuario.turmaSelecionada;
  const turmaId = turmaSelecionada ? turmaSelecionada.turma : 0;
  const modalidade = turmaSelecionada.modalidade;
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
  const [somenteConsulta, setSomenteConsulta] = useState(
    verificaSomenteConsulta(permissoesTela)
  );

  useEffect(() => {
    const obterDisciplinas = async () => {
      const disciplinas = await ServicoDisciplina.obterDisciplinasPorTurma(
        turmaId
      );
      setListaDisciplinas(disciplinas.data);
      if (disciplinas.data && disciplinas.data.length == 1) {
        const disciplina = disciplinas.data[0];
        setDisciplinaSelecionada(disciplina);
        onChangeDisciplinas(disciplina.codigoComponenteCurricular);
        setDesabilitarDisciplina(true);
      }
    };
    if (turmaId) {
      obterDisciplinas(turmaId);
    } else {
      setDadosLista([]);
      setModoEdicao(false);
      setDisciplinaIdSelecionada(undefined);
    }
  }, [turmaSelecionada.turma]);

  const onChangeDisciplinas = async disciplinaId => {
    if (modoEdicao) {
      const confirmar = await perguntaAoSalvar();
      if (confirmar) {
        await salvar();
      }
    }
    setDisciplinaIdSelecionada(String(disciplinaId));
    await buscarDados(disciplinaId);
  };

  const buscarDados = async disciplinaId => {
    const resposta = await api.get(
      `v1/aula-prevista/modalidades/${modalidade}/turmas/${turmaId}/disciplinas/${disciplinaId}`
    );
    const dadosAula = resposta.data;
    if (dadosAula && dadosAula.aulasPrevistasPorBimestre) {
      const dadosBimestre = dadosAula.aulasPrevistasPorBimestre;
      let totalPrevistas = 0;
      let totalCriadasTitular = 0;
      let totalCriadasCj = 0;
      let totalDadas = 0;
      let totalRepostas = 0;
      dadosBimestre.forEach(item => {
        item.ehBimestreAtual = ehBimestreAtual(item.inicio, item.fim);
        item.dadas = item.cumpridas;
        totalPrevistas += item.previstas.quantidade;
        totalCriadasTitular += item.criadas.quantidadeTitular;
        totalCriadasCj += item.criadas.quantidadeCJ;
        totalDadas += item.dadas;
        totalRepostas += item.reposicoes;
        if (item.previstas.mensagens && item.previstas.mensagens.length > 0) {
          item.previstas.temDivergencia = true;
        }
      });
      const dados = {
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
      setAuditoria(aud);
    }
  };

  const ehBimestreAtual = (dataInicio, dataFim) => {
    const dataAtual = window.moment(new Date());
    return (
      window.moment(dataInicio) >= dataAtual &&
      window.moment(dataFim) <= dataAtual
    );
  };

  const resetarTela = () => {
    setModoEdicao(false);
    buscarDados(disciplinaIdSelecionada);
  };

  const perguntaAoSalvar = async () => {
    return await confirmar(
      'Atenção',
      '',
      'Suas alterações não foram salvas, deseja salvar agora?'
    );
  };

  const onClickVoltar = async () => {
    if (modoEdicao) {
      const confirmado = perguntaAoSalvar();
      if (confirmado) {
        await salvar();
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

  const salvar = async () => {
    const dados = {
      bimestresQuantidade: dadoslista.bimestres,
      disciplinaId: disciplinaIdSelecionada,
      modalidade,
      turmaId,
    };
    if (dadoslista.id) {
      await api
        .put(`v1/aula-prevista/${dadoslista.id}`, dados)
        .catch(e => erros(e));
    } else {
      await api.post(`v1/aula-prevista`, dados).catch(e => erros(e));
    }
  };

  const onClickSalvar = async () => {
    await salvar();
  };

  return (
    <>
      {!turmaSelecionada.turma ? (
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
      <Grid cols={12} className="p-0">
        <Titulo>
          Aula prevista X Aula dada
          <TituloAno>
            {' '}
            {` / ${anoLetivo ? anoLetivo : new Date().getFullYear()}`}{' '}
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
                placeholder="Disciplina"
                disabled={desabilitarDisciplina}
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
              {dadoslista && dadoslista.bimestres ? (
                <ListaAulasPorBimestre
                  dados={dadoslista}
                  setModoEdicao={e => setModoEdicao(e)}
                  permissoesTela={permissoesTela}
                  somenteConsulta={somenteConsulta}
                />
              ) : null}
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
