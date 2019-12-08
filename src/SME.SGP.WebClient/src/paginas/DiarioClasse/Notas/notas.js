import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import SelectComponent from '~/componentes/select';
import api from '~/servicos/api';
import TabsComponent from '~/componentes/tabs/tabs';
import Avaliacao from '~/componentes-sgp/avaliacao/avaliacao';
import { Container, ContainerAuditoria } from './notas.css';

const Notas = () => {

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;
  const turmaId = turmaSelecionada ? turmaSelecionada.turma : 0;

  const [listaDisciplinas, setListaDisciplinas] = useState([]);
  const [disciplinaSelecionada, setDisciplinaSelecionada] = useState(undefined);
  const [desabilitarDisciplina, setDesabilitarDisciplina] = useState(false);
  const [listaTabs, setListaTabs] = useState([]);
  const [auditoriaInfo, setAuditoriaInfo] = useState({
    auditoriaAlterado: '',
    auditoriaInserido: ''
  });

  useEffect(() => {
    const obterDisciplinas = async () => {
      const url = `v1/professores/123/turmas/${turmaId}/disciplinas`;
      const disciplinas = await api.get(url);

      setListaDisciplinas(disciplinas.data);
      if (disciplinas.data && disciplinas.data.length == 1) {
        const disciplina = disciplinas.data[0];
        setDisciplinaSelecionada(String(disciplina.codigoComponenteCurricular));
        setDesabilitarDisciplina(true);
        obterDadosBimestres(disciplina.codigoComponenteCurricular);
      }
    };

    if (turmaId) {
      setDisciplinaSelecionada(undefined);
      obterDisciplinas();
    } else {
      // TODO - Resetar tela
      setListaDisciplinas([]);
      setDesabilitarDisciplina(false);
      setDisciplinaSelecionada(undefined);
    }
  }, [turmaSelecionada.turma]);

  const onClickVoltar = ()=> {
    console.log('onClickVoltar');
  }

  const onClickCancelar = ()=> {
    console.log('onClickCancelar');
  }

  const onClickSalvar = ()=> {
    console.log('onClickSalvar');
  }

  const onChangeDisciplinas = disciplinaId => {
    setDisciplinaSelecionada(disciplinaId);
    obterDadosBimestres(disciplinaId, 0)
  };

  const onChangeTab = (item) => {
    console.log(item);
  }

  const obterDadosBimestres = async (disciplinaId, numeroBimestre) => {
    const params = {
      bimestre: numeroBimestre,
      disciplinaCodigo: disciplinaId,
      professorRf: usuario.rf
    }
    const dados = await api.get('v1/avaliacoes/notas', params);
    debugger
    if (dados && dados.data) {
      montaListaTabs(dados.data)
      setAuditoriaInfo({
        auditoriaAlterado: dados.data.auditoriaAlterado,
        auditoriaInserido: dados.data.auditoriaInserido
      });
    }
  }

  const montaListaTabs = dados => {
    const bimestres = dados.bimestres.map((item, i) => {
      return {
        nome: item.descricao,
        conteudo: (
          <Avaliacao key={i} dados={item} notaTipo={dados.notaTipo} ></Avaliacao>
        )
      }
    });
    setListaTabs(bimestres);
  }

  return (
    <Container>
      <Cabecalho pagina="Lançamento de notas" />
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
              />
              <Button
                label="Salvar"
                color={Colors.Roxo}
                border
                bold
                className="mr-2"
                onClick={onClickSalvar}
              />
            </div>
          </div>
          <div className="row">
            <div className="col-sm-12 col-md-4 col-lg-4 col-xl-4 mb-2">
              <SelectComponent
                id="disciplina"
                name="disciplinaId"
                lista={listaDisciplinas}
                valueOption="codigoComponenteCurricular"
                valueText="nome"
                valueSelect={disciplinaSelecionada}
                onChange={onChangeDisciplinas}
                placeholder="Disciplina"
                disabled={desabilitarDisciplina}
              />
            </div>
          </div>
          {
            listaTabs && listaTabs.length > 0 ?
            <>
              <div className="row">
                <div className="col-sm-12 col-md-12 col-lg-12 col-xl-12 mb-2">
                  <TabsComponent onChangeTab={onChangeTab} listaTabs={listaTabs}/>
                </div>
              </div>
              <div className="row mt-2 mb-2 mt-2">
               <div className="col-md-12">
                <ContainerAuditoria style={{float: "left"}}>
                  <span>
                    <p>{auditoriaInfo.auditoriaInserido || ''}</p>
                    <p>{auditoriaInfo.auditoriaAlterado || ''}</p>
                  </span>
                </ContainerAuditoria>
                <span style={{float: "right"}} className="mt-1 ml-1">
                  Aluno ausente na data da avaliação
                </span>
                <span className="icon-legenda-aluno-ausente">
                  <i className="fas fa-user-times"/>
                </span>
                </div>
              </div>
            </>
          :''
          }
        </div>
      </Card>
    </Container>
  );
};

export default Notas;
