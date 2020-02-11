import { Tabs } from 'antd';
import React, { useEffect, useState, useRef } from 'react';
import { useSelector } from 'react-redux';
import { Colors, Loader } from '~/componentes';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Alert from '~/componentes/alert';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import Grid from '~/componentes/grid';
import SelectComponent from '~/componentes/select';
import { ContainerTabsCard } from '~/componentes/tabs/tabs.css';
import { URL_HOME } from '~/constantes/url';
import history from '~/servicos/history';
import ServicoDisciplina from '~/servicos/Paginas/ServicoDisciplina';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import FechamentoBimestreLista from './fechamento-bimestre-lista/fechamento-bimestre-lista';
import { FechamentoMock } from './fechamento.mock';
import RotasDto from '~/dtos/rotasDto';
import { Fechamento } from './fechamento-bimestre.css';
import FechamentoFinal from '../FechamentoFinal/fechamentoFinal';
import ServicoFechamentoFinal from '~/servicos/Paginas/DiarioClasse/ServicoFechamentoFinal';
import { erros, sucesso, confirmar } from '~/servicos/alertas';

const FechamentoBismestre = () => {
  const { TabPane } = Tabs;
  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada, permissoes } = usuario;
  const permissoesTela = permissoes[RotasDto.FechamentoBismestre];
  const [somenteConsulta, setSomenteConsulta] = useState(false);

  useEffect(() => {
    setSomenteConsulta(verificaSomenteConsulta(permissoesTela));
  }, [permissoesTela]);

  const [carregandoBimestres, setCarregandoBimestres] = useState(false);
  const [listaDisciplinas, setListaDisciplinas] = useState([]);
  const [disciplinaIdSelecionada, setDisciplinaIdSelecionada] = useState(null);
  const [desabilitarDisciplina, setDesabilitarDisciplina] = useState(
    listaDisciplinas && listaDisciplinas.length === 1
  );
  const [modoEdicao, setModoEdicao] = useState(false);
  const [bimestreCorrente, setBimestreCorrente] = useState('1Bimestre');
  const [dados, setDados] = useState(FechamentoMock);

  const onChangeDisciplinas = id => {
    setDisciplinaIdSelecionada(id);
  };

  const onClickVoltar = async () => {
    let confirmou = true;
    if (modoEdicao) {
      confirmou = await confirmar(
        'Atenção',
        'Existem alterações pendetes, deseja realmente sair da tela de fechamento?'
      );
    }
    if (confirmou) {
      history.push(URL_HOME);
    }
  };

  const onClickCancelar = async () => {
    const confirmou = await confirmar(
      'Atenção',
      'Existem alterações pendetes, deseja realmente cancelar?'
    );
    if (confirmou) {
      refFechamentoFinal.current.cancelar();
      setModoEdicao(false);
    }
  };

  const onClickSalvar = () => {};

  const onChangeTab = async numeroBimestre => {
    setBimestreCorrente(numeroBimestre);
  };

  useEffect(() => {
    const obterDisciplinas = async () => {
      if (turmaSelecionada && turmaSelecionada.turma) {
        const lista = await ServicoDisciplina.obterDisciplinasPorTurma(
          turmaSelecionada.turma
        );
        setListaDisciplinas(lista.data);
        if (lista.data.length === 1) {
          setDisciplinaIdSelecionada(
            String(lista.data[0].codigoComponenteCurricular)
          );
          setDesabilitarDisciplina(true);
        }
      }
    };
    obterDisciplinas();
  }, [turmaSelecionada]);

  useEffect(() => {
    //implementar o consumo de endpoint de listagem
  }, [disciplinaIdSelecionada]);

  //FechamentoFinal
  const refFechamentoFinal = useRef();
  const [ehRegencia, setEhRegencia] = useState(false);
  const [turmaPrograma, setTurmaPrograma] = useState(false);

  useEffect(() => {
    const programa = !!(turmaSelecionada.ano === '0');
    setTurmaPrograma(programa);
  }, [turmaSelecionada.ano]);

  useEffect(() => {
    if (listaDisciplinas && listaDisciplinas.length > 0) {
      const disciplina = listaDisciplinas.find(
        c => c.disciplinaId == disciplinaIdSelecionada
      );
      if (disciplina) setEhRegencia(disciplina.regencia);
    }
  }, [disciplinaIdSelecionada, listaDisciplinas]);

  const [fechamentoFinal, setFechamentoFinal] = useState({
    ehRegencia,
    turmaCodigo: turmaSelecionada.turma,
    itens: [],
  });

  const onChangeFechamentoFinal = alunosAlterados => {
    const fechamentoFinalDto = fechamentoFinal;
    fechamentoFinalDto.itens = alunosAlterados;
    setFechamentoFinal(fechamentoFinalDto);
    setModoEdicao(true);
  };
  const salvarFechamentoFinal = () => {
    ServicoFechamentoFinal.salvar(fechamentoFinal)
      .then(resposta => {
        sucesso('Fechamento final salvo com sucesso.');
        setModoEdicao(false);
      })
      .catch(e => erros(e));
  };

  //FechamentoFinal
  return (
    <>
      {!turmaSelecionada.turma ? (
        <Grid cols={12} className="p-0">
          <Alert
            alerta={{
              tipo: 'warning',
              id: 'AlertaTurmaFechamentoBimestre',
              mensagem: 'Você precisa escolher uma turma.',
              estiloTitulo: { fontSize: '18px' },
            }}
            className="mb-2"
          />
        </Grid>
      ) : null}{' '}
      <Cabecalho pagina="Fechamento" />
      <Loader loading={carregandoBimestres}>
        <Card>
          <div className="col-md-12">
            <div className="row">
              <div className="col-md-12 d-flex justify-content-end pb-4">
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
                  onClick={salvarFechamentoFinal}
                  disabled={!modoEdicao || somenteConsulta}
                />
              </div>
            </div>
          </div>
          <div className="col-md-12">
            <div className="row">
              <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 mb-4">
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
            </div>
          </div>
          <div className="col-md-12">
            <div className="row">
              <Fechamento className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
                <ContainerTabsCard
                  type="card"
                  onChange={onChangeTab}
                  activeKey={bimestreCorrente}
                >
                  <TabPane tab="1º Bimestre" key="1Bimestre">
                    <FechamentoBimestreLista dados={dados} />
                  </TabPane>

                  <TabPane tab="2º Bimestre" key="2Bimestre">
                    <FechamentoBimestreLista dados={dados} />
                  </TabPane>

                  <TabPane tab="3º Bimestre" key="3Bimestre">
                    <FechamentoBimestreLista dados={dados} />
                  </TabPane>

                  <TabPane tab="4º Bimestre" key="4Bimestre">
                    <FechamentoBimestreLista dados={dados} />
                  </TabPane>

                  <TabPane tab="Final" key="final">
                    <FechamentoFinal
                      turmaCodigo={turmaSelecionada.turma}
                      disciplinaCodigo={disciplinaIdSelecionada}
                      ehRegencia={ehRegencia}
                      turmaPrograma={turmaPrograma}
                      onChange={onChangeFechamentoFinal}
                      ref={refFechamentoFinal}
                    />
                  </TabPane>
                </ContainerTabsCard>
              </Fechamento>
            </div>
          </div>
        </Card>
      </Loader>
    </>
  );
};

export default FechamentoBismestre;
