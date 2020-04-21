import { Tabs } from 'antd';
import React, { useEffect, useState, useRef } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { Colors, Loader, Base } from '~/componentes';
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
import RotasDto from '~/dtos/rotasDto';
import { Fechamento } from './fechamento-bimestre.css';
import FechamentoFinal from '../FechamentoFinal/fechamentoFinal';
import ServicoFechamentoFinal from '~/servicos/Paginas/DiarioClasse/ServicoFechamentoFinal';
import { erros, sucesso, confirmar } from '~/servicos/alertas';
import ServicoFechamentoBimestre from '~/servicos/Paginas/Fechamento/ServicoFechamentoBimestre';
import periodo from '~/dtos/periodo';
import { setExpandirLinha } from '~/redux/modulos/notasConceitos/actions';

const FechamentoBismestre = () => {
  const dispatch = useDispatch();

  const { TabPane } = Tabs;
  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada, permissoes } = usuario;
  const permissoesTela = permissoes[RotasDto.FECHAMENTO_BIMESTRE];
  const { podeIncluir, podeAlterar } = permissoesTela;
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
  const [dadosBimestre1, setDadosBimestre1] = useState(undefined);
  const [dadosBimestre2, setDadosBimestre2] = useState(undefined);
  const [dadosBimestre3, setDadosBimestre3] = useState(undefined);
  const [dadosBimestre4, setDadosBimestre4] = useState(undefined);
  const [ehRegencia, setEhRegencia] = useState(false);
  const [ehSintese, setEhSintese] = useState(false);
  const [periodoFechamento, setPeriodoFechamento] = useState(periodo.Anual);
  const [desabilitaAbaFinal, setDesabilitaAbaFinal] = useState(false);
  const [situacaoFechamento, setSituacaoFechamento] = useState(0);

  const onChangeDisciplinas = id => {
    const disciplina = listaDisciplinas.find(
      c => String(c.codigoComponenteCurricular) === id
    );
    setEhRegencia(disciplina.regencia);
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
      dispatch(setExpandirLinha([]));
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

  useEffect(() => {
    const obterDisciplinas = async () => {
      if (turmaSelecionada && turmaSelecionada.turma) {
        const lista = await ServicoDisciplina.obterDisciplinasPorTurma(
          turmaSelecionada.turma
        );
        setListaDisciplinas([...lista.data]);
        if (lista.data.length === 1) {
          setDisciplinaIdSelecionada(undefined);
          setDisciplinaIdSelecionada(
            String(lista.data[0].codigoComponenteCurricular)
          );
          setEhRegencia(lista.data[0].regencia);
          setDesabilitarDisciplina(true);
        }
      }
    };
    obterDisciplinas();
  }, [turmaSelecionada]);

  const obterDados = async (bimestre = 0) => {
    if (disciplinaIdSelecionada) {
      setCarregandoBimestres(true);
      const fechamento = await ServicoFechamentoBimestre.buscarDados(
        turmaSelecionada.turma,
        disciplinaIdSelecionada,
        bimestre,
        turmaSelecionada.periodo
      ).finally(() => {
        setCarregandoBimestres(false);
      });
      if (fechamento && fechamento.data) {
        const dadosFechamento = fechamento.data;
        setEhSintese(dadosFechamento.ehSintese);
        setSituacaoFechamento(dadosFechamento.situacao);
        setPeriodoFechamento(dadosFechamento.periodo);
        setBimestreCorrente(`${dadosFechamento.bimestre}`);
        setDadosBimestre(dadosFechamento.bimestre, dadosFechamento);
      }
    }
  };

  const setDadosBimestre = (bimestre, dados) => {
    switch (bimestre) {
      case 1:
        setDadosBimestre1(undefined);
        setDadosBimestre1(dados);
        break;
      case 2:
        setDadosBimestre2(undefined);
        setDadosBimestre2(dados);
        break;
      case 3:
        setDadosBimestre3(undefined);
        setDadosBimestre3(dados);
        break;
      case 4:
        setDadosBimestre4(undefined);
        setDadosBimestre4(dados);
        break;
      default:
        break;
    }
  };

  useEffect(() => {
    if (disciplinaIdSelecionada) obterDados();
  }, [disciplinaIdSelecionada]);

  const onConfirmouTrocarTab = numeroBimestre => {
    setBimestreCorrente(numeroBimestre);
    if (numeroBimestre !== 'final') {
      obterDados(numeroBimestre);
    }
  };

  const onChangeTab = async numeroBimestre => {
    if (modoEdicao) {
      const confirmado = await confirmar(
        'Atenção',
        'Suas alterações não foram salvas, deseja salvar agora?'
      );
      if (confirmado) {
        const salvou = await salvarFechamentoFinal();
        if (salvou) {
          onConfirmouTrocarTab(numeroBimestre);
          setModoEdicao(false);
          dispatch(setExpandirLinha([]));
        }
      } else {
        onConfirmouTrocarTab(numeroBimestre);
        setModoEdicao(false);
        dispatch(setExpandirLinha([]));
      }
    } else {
      onConfirmouTrocarTab(numeroBimestre);
    }
  };

  //FechamentoFinal
  const refFechamentoFinal = useRef();
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
    fechamentoFinal.turmaCodigo = turmaSelecionada.turma;
    fechamentoFinal.ehRegencia = ehRegencia;
    fechamentoFinal.disciplinaId = disciplinaIdSelecionada;
    return ServicoFechamentoFinal.salvar(fechamentoFinal)
      .then(() => {
        sucesso('Fechamento final salvo com sucesso.');
        setModoEdicao(false);
        dispatch(setExpandirLinha([]));
        refFechamentoFinal.current.salvarFechamentoFinal();
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
                  hidden={ehSintese}
                />
                <Button
                  label="Salvar"
                  color={Colors.Roxo}
                  border
                  bold
                  className="mr-2"
                  onClick={salvarFechamentoFinal}
                  disabled={!modoEdicao || somenteConsulta}
                  hidden={ehSintese}
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
                  <TabPane tab="1º Bimestre" key="1">
                    {dadosBimestre1 ? (
                      <FechamentoBimestreLista
                        dados={dadosBimestre1}
                        ehRegencia={ehRegencia}
                        ehSintese={ehSintese}
                        situacaoFechamento={situacaoFechamento}
                        codigoComponenteCurricular={disciplinaIdSelecionada}
                        turmaId={turmaSelecionada.turma}
                        anoLetivo={turmaSelecionada.anoLetivo}
                      />
                    ) : null}
                  </TabPane>

                  <TabPane tab="2º Bimestre" key="2">
                    {dadosBimestre2 ? (
                      <FechamentoBimestreLista
                        dados={dadosBimestre2}
                        ehRegencia={ehRegencia}
                        ehSintese={ehSintese}
                        situacaoFechamento={situacaoFechamento}
                        codigoComponenteCurricular={disciplinaIdSelecionada}
                        turmaId={turmaSelecionada.turma}
                        anoLetivo={turmaSelecionada.anoLetivo}
                      />
                    ) : null}
                  </TabPane>
                  {periodoFechamento === periodo.Anual ? (
                    <TabPane tab="3º Bimestre" key="3">
                      {dadosBimestre3 ? (
                        <FechamentoBimestreLista
                          dados={dadosBimestre3}
                          ehRegencia={ehRegencia}
                          ehSintese={ehSintese}
                          situacaoFechamento={situacaoFechamento}
                          codigoComponenteCurricular={disciplinaIdSelecionada}
                          turmaId={turmaSelecionada.turma}
                          anoLetivo={turmaSelecionada.anoLetivo}
                        />
                      ) : null}
                    </TabPane>
                  ) : null}
                  {periodoFechamento === periodo.Anual ? (
                    <TabPane tab="4º Bimestre" key="4">
                      {dadosBimestre4 ? (
                        <FechamentoBimestreLista
                          dados={dadosBimestre4}
                          ehRegencia={ehRegencia}
                          ehSintese={ehSintese}
                          situacaoFechamento={situacaoFechamento}
                          codigoComponenteCurricular={disciplinaIdSelecionada}
                          turmaId={turmaSelecionada.turma}
                          anoLetivo={turmaSelecionada.anoLetivo}
                        />
                      ) : null}
                    </TabPane>
                  ) : null}
                  <TabPane
                    tab="Final"
                    key="final"
                    disabled={desabilitaAbaFinal}
                  >
                    <FechamentoFinal
                      turmaCodigo={turmaSelecionada.turma}
                      disciplinaCodigo={disciplinaIdSelecionada}
                      ehRegencia={ehRegencia}
                      turmaPrograma={turmaPrograma}
                      onChange={onChangeFechamentoFinal}
                      ref={refFechamentoFinal}
                      desabilitarCampo={
                        !podeIncluir || !podeAlterar || somenteConsulta
                      }
                      somenteConsulta={somenteConsulta}
                      carregandoFechamentoFinal={carregando =>
                        setCarregandoBimestres(carregando)
                      }
                      bimestreCorrente={bimestreCorrente}
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
