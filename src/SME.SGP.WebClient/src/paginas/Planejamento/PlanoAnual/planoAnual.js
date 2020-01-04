import React, { useState, useEffect, useRef } from 'react';
import { useSelector } from 'react-redux';
import { Collapse } from 'antd';
import Row from '~/componentes/row';
import { Grid, Card, SelectComponent, Button, Colors } from '~/componentes';
import Alert from '~/componentes/alert';
import modalidade from '~/dtos/modalidade';
import {
  Titulo,
  TituloAno,
  Planejamento,
  ContainerBimestres,
} from './planoAnual.css';
import { RegistroMigrado } from '~/componentes-sgp/registro-migrado';
import servicoDisciplinas from '~/servicos/Paginas/ServicoDisciplina';
import { erros, sucesso } from '~/servicos/alertas';
import servicoPlanoAnual from '~/servicos/Paginas/ServicoPlanoAnual';
import Bimestre from './bimestre';

const { Panel } = Collapse;

const PlanoAnual = () => {
  const turmaSelecionada = useSelector(c => c.usuario.turmaSelecionada);
  const [possuiTurmaSelecionada, setPossuiTurmaSelecionada] = useState(false);
  const [ehEja, setEhEja] = useState(false);
  const [planoAnual, setPlanoAnual] = useState([]);
  const [registroMigrado, setRegistroMigrado] = useState(false);
  const [listaDisciplinas, setListaDisciplinas] = useState([]);
  const [bimestreExpandido, setBimestreExpandido] = useState('');
  const [refsPainel, setRefsPainel] = useState([
    useRef(),
    useRef(),
    useRef(),
    useRef(),
  ]);

  const [
    listaDisciplinasPlanejamento,
    setListaDisciplinasPlanejamento,
  ] = useState([]);
  const [
    codigoDisciplinaSelecionada,
    setCodigoDisciplinaSelecionada,
  ] = useState('');

  const [disciplinaSelecionada, setDisciplinaSelecionada] = useState('');

  const onChangeDisciplinas = codigoDisciplina => {
    const disciplina = listaDisciplinas.find(
      c => c.codigoComponenteCurricular == codigoDisciplina
    );
    setDisciplinaSelecionada(disciplina);
    setCodigoDisciplinaSelecionada(codigoDisciplina);
  };

  const onChangeBimestre = bimestre => {
    const indiceBimestreAlterado = planoAnual.findIndex(
      c => c.bimestre == bimestre.bimestre
    );
    planoAnual[indiceBimestreAlterado].descricao = bimestre.descricao;
    planoAnual[indiceBimestreAlterado].objetivosAprendizagem =
      bimestre.objetivosAprendizagem;
    planoAnual[indiceBimestreAlterado].alterado = true;
  };

  const salvar = () => {
    const plano = {
      anoLetivo: turmaSelecionada.anoLetivo,
      bimestres: planoAnual.filter(c => c.alterado),
      componenteCurricularEolId:
        disciplinaSelecionada.codigoComponenteCurricular,
      turmaId: turmaSelecionada.turma,
      escolaId: turmaSelecionada.unidadeEscolar,
    };

    servicoPlanoAnual
      .salvar(plano)
      .then(() => sucesso('Registro salvo com sucesso.'))
      .catch(e => erros(e));
  };
  useEffect(() => {
    if (planoAnual && planoAnual.length > 0) {
      const expandido = planoAnual.find(c => c.obrigatorio);
      if (expandido) setBimestreExpandido([expandido.bimestre]);
    }
  }, [planoAnual]);

  useEffect(() => {
    if (bimestreExpandido) {
      const refBimestre = refsPainel[bimestreExpandido - 1];
      if (refBimestre && refBimestre.current) {
        setTimeout(() => {
          window.scrollTo(
            0,
            refsPainel[bimestreExpandido - 1].current.offsetTop
          );
        }, 500);
      }
    }
  }, [bimestreExpandido, refsPainel]);

  useEffect(() => {
    servicoDisciplinas
      .obterDisciplinasPorTurma(turmaSelecionada.turma)
      .then(resposta => {
        setListaDisciplinas(resposta.data);
        if (resposta.data.length === 1) {
          const disciplina = resposta.data[0];
          setDisciplinaSelecionada(disciplina);
          setCodigoDisciplinaSelecionada(
            String(disciplina.codigoComponenteCurricular)
          );
        }
      })
      .catch(e => erros(e));
  }, [turmaSelecionada.ano, turmaSelecionada.turma]);

  useEffect(() => {
    if (codigoDisciplinaSelecionada) {
      servicoPlanoAnual
        .obter(
          turmaSelecionada.anoLetivo,
          codigoDisciplinaSelecionada,
          turmaSelecionada.unidadeEscolar,
          turmaSelecionada.turma
        )
        .then(resposta => {
          setPlanoAnual(resposta.data);
        })
        .catch(e => erros(e));

      const turmaPrograma = !!(turmaSelecionada.ano === '0');
      servicoDisciplinas
        .obterDisciplinasPlanejamento(
          codigoDisciplinaSelecionada,
          turmaSelecionada.turma,
          turmaPrograma,
          disciplinaSelecionada.regencia
        )
        .then(resposta => {
          setListaDisciplinasPlanejamento(
            resposta.data.map(disciplina => {
              return {
                ...disciplina,
                selecionada: false,
              };
            })
          );
        })
        .catch(e => erros(e));
    }
  }, [
    codigoDisciplinaSelecionada,
    disciplinaSelecionada.regencia,
    turmaSelecionada,
  ]);

  useEffect(() => {
    setPossuiTurmaSelecionada(turmaSelecionada && turmaSelecionada.turma);
    if (turmaSelecionada && turmaSelecionada.turma) {
      setEhEja(turmaSelecionada.modalidade == modalidade.EJA);
    }
  }, [turmaSelecionada]);

  return (
    <>
      <div className="col-md-12">
        {!possuiTurmaSelecionada ? (
          <Row className="mb-0 pb-0">
            <Grid cols={12} className="mb-0 pb-0">
              <Alert
                alerta={{
                  tipo: 'warning',
                  id: 'AlertaPrincipal',
                  mensagem: 'Você precisa escolher uma turma.',
                }}
                className="mb-0"
              />
            </Grid>
          </Row>
        ) : null}
      </div>
      <Grid cols={12} className="p-0">
        <Planejamento> PLANEJAMENTO </Planejamento>
        <Titulo>
          {ehEja ? 'Plano Semestral' : 'Plano Anual'}
          <TituloAno>{` / ${turmaSelecionada.anoLetivo}`}</TituloAno>
          {registroMigrado && (
            <RegistroMigrado className="float-right">
              Registro Migrado
            </RegistroMigrado>
          )}
        </Titulo>
      </Grid>
      <Card className="col-md-12 p-0" mx="mx-0">
        <div className="col-md-4">
          <SelectComponent
            name="disciplinas"
            id="disciplinas"
            lista={listaDisciplinas}
            valueOption="codigoComponenteCurricular"
            valueText="nome"
            onChange={onChangeDisciplinas}
            valueSelect={codigoDisciplinaSelecionada}
            placeholder="Selecione uma disciplina"
            disabled={listaDisciplinas && listaDisciplinas.length === 1}
          />
        </div>
        <Button
          label="Copiar Conteúdo"
          icon="share-square"
          className="ml-3"
          color={Colors.Azul}
          border
        />
        <Button
          label="Voltar"
          icon="arrow-left"
          color={Colors.Azul}
          border
          className="mr-3"
        />
        <Button
          label="Cancelar"
          color={Colors.Roxo}
          border
          bold
          className="mr-3"
        />
        <Button label="Salvar" color={Colors.Roxo} bold onClick={salvar} />
        <Grid cols={12} className="p-2">
          <ContainerBimestres>
            <Collapse
              bordered={false}
              expandIconPosition="right"
              defaultActiveKey={bimestreExpandido}
              activeKey={bimestreExpandido}
              onChange={c => {
                setBimestreExpandido(c);
              }}
            >
              {planoAnual &&
                planoAnual.length > 0 &&
                planoAnual.map(plano => (
                  <Panel
                    header={`${plano.bimestre}º ${
                      ehEja ? 'Semestre' : 'Bimestre'
                    }`}
                    key={plano.bimestre}
                  >
                    <div ref={refsPainel[plano.bimestre - 1]}>
                      <Bimestre
                        className="fade"
                        disciplinas={listaDisciplinasPlanejamento}
                        bimestre={plano}
                        ano={turmaSelecionada.ano}
                        ehEja={ehEja}
                        regencia={disciplinaSelecionada.regencia}
                        onChange={onChangeBimestre}
                        key={plano.bimestre}
                      />
                    </div>
                  </Panel>
                ))}
            </Collapse>
          </ContainerBimestres>
        </Grid>
      </Card>
    </>
  );
};

export default PlanoAnual;
