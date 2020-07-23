import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Loader } from '~/componentes';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Alert from '~/componentes/alert';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import SelectComponent from '~/componentes/select';
import {
  limparDadosRelatorioSemestral,
  setAlunosRelatorioSemestral,
  setDadosAlunoObjectCard,
  setDentroPeriodo,
  setCodigoAlunoSelecionado,
} from '~/redux/modulos/relatorioSemestralPAP/actions';
import { erros } from '~/servicos/alertas';
import ServicoRelatorioSemestral from '~/servicos/Paginas/Relatorios/PAP/RelatorioSemestral/ServicoRelatorioSemestral';
import AlertaDentroPeriodoPAP from './DadosRelatorioSemestral/AlertaDentroPeriodoPAP/alertaDentroPeriodoPAP';
import BotaoOrdenarListaAlunos from './DadosRelatorioSemestral/BotaoOrdenarListaAlunos/botaoOrdenarListaAlunos';
import BotoesAcoesRelatorioSemestral from './DadosRelatorioSemestral/BotoesAcoes/botoesAcoesRelatorioSemestral';
import DadosRelatorioSemestral from './DadosRelatorioSemestral/dadosRelatorioSemestral';
import ObjectCardRelatorioSemestral from './DadosRelatorioSemestral/ObjectCardRelatorioSemestral/objectCardRelatorioSemestral';
import TabelaRetratilRelatorioSemestral from './DadosRelatorioSemestral/TabelaRetratilRelatorioSemestral/tabelaRetratilRelatorioSemestral';
import { Container } from './relatorioSemestral.css';
import servicoSalvarRelatorioSemestral from './servicoSalvarRelatorioSemestral';
import ModalErrosRalSemestralPAP from './DadosRelatorioSemestral/ModalErros/ModalErrosRalSemestralPAP';
import AlertaModalidadeInfantil from '~/componentes-sgp/AlertaModalidadeInfantil/alertaModalidadeInfantil';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';

const RelatorioSemestral = () => {
  const dispatch = useDispatch();

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;
  const { turma, anoLetivo } = turmaSelecionada;

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );
  const [carregandoGeral, setCarregandoGeral] = useState(false);

  const [listaSemestres, setListaSemestres] = useState([]);
  const [semestreSelecionado, setSemestreSelecionado] = useState(undefined);

  const resetarInfomacoes = useCallback(() => {
    dispatch(limparDadosRelatorioSemestral());
  }, [dispatch]);

  const obterListaAlunos = useCallback(
    async semestreConsulta => {
      if (turma) {
        setCarregandoGeral(true);
        const retorno = await ServicoRelatorioSemestral.obterListaAlunos(
          turma,
          anoLetivo,
          semestreConsulta
        ).catch(e => erros(e));
        if (retorno && retorno.data) {
          dispatch(setAlunosRelatorioSemestral(retorno.data));
        } else {
          resetarInfomacoes();
          dispatch(setAlunosRelatorioSemestral([]));
        }
        setCarregandoGeral(false);
      }
    },
    [anoLetivo, dispatch, turma, resetarInfomacoes]
  );

  const obterListaSemestres = useCallback(async () => {
    const retorno = await ServicoRelatorioSemestral.obterListaSemestres(
      turma
    ).catch(e => erros(e));
    if (retorno && retorno.data) {
      setListaSemestres(retorno.data);
    } else {
      setListaSemestres([]);
    }
  }, [turma]);

  useEffect(() => {
    resetarInfomacoes();
    dispatch(setAlunosRelatorioSemestral([]));
    if (
      turma &&
      !ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada)
    ) {
      obterListaSemestres();
    } else {
      setSemestreSelecionado(undefined);
      setListaSemestres([]);
    }
  }, [
    obterListaAlunos,
    turma,
    resetarInfomacoes,
    dispatch,
    obterListaSemestres,
    turmaSelecionada,
    modalidadesFiltroPrincipal,
  ]);

  useEffect(() => {
    if (semestreSelecionado) {
      obterListaAlunos(semestreSelecionado);
    } else {
      dispatch(setAlunosRelatorioSemestral([]));
    }
  }, [semestreSelecionado, obterListaAlunos, dispatch]);

  const obterFrequenciaAluno = async codigoAluno => {
    const retorno = await ServicoRelatorioSemestral.obterFrequenciaAluno(
      codigoAluno,
      turma
    ).catch(e => erros(e));
    if (retorno && retorno.data) {
      return retorno.data;
    }
    return 0;
  };

  const onChangeAlunoSelecionado = async aluno => {
    resetarInfomacoes();
    const frequenciaGeralAluno = await obterFrequenciaAluno(aluno.codigoEOL);
    const novoAluno = aluno;
    novoAluno.frequencia = frequenciaGeralAluno;
    dispatch(setDadosAlunoObjectCard(aluno));

    dispatch(setCodigoAlunoSelecionado(aluno.codigoEOL));
  };

  const permiteOnChangeAluno = async () => {
    const continuar = await servicoSalvarRelatorioSemestral.validarSalvarRelatorioSemestral();
    if (continuar) {
      return true;
    }
    return false;
  };

  const onChangeSemestre = valor => {
    resetarInfomacoes();
    dispatch(setAlunosRelatorioSemestral([]));
    dispatch(setCodigoAlunoSelecionado());
    setSemestreSelecionado(valor);

    let dentroPeriodo = true;
    if (valor) {
      const semestreNovo = listaSemestres.find(item => item.semestre == valor);
      dentroPeriodo = semestreNovo.podeEditar;
    }
    dispatch(setDentroPeriodo(dentroPeriodo));
  };

  return (
    <Container>
      <ModalErrosRalSemestralPAP />
      {!turmaSelecionada.turma &&
      !ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada) ? (
        <div className="col-md-12">
          <Alert
            alerta={{
              tipo: 'warning',
              id: 'alerta-sem-turma-relatorio-semestral',
              mensagem: 'Você precisa escolher uma turma.',
              estiloTitulo: { fontSize: '18px' },
            }}
            className="mb-2"
          />
        </div>
      ) : (
        ''
      )}
      <AlertaDentroPeriodoPAP />
      <AlertaModalidadeInfantil />
      <Cabecalho pagina="Relatório semestral" />
      <Loader loading={carregandoGeral}>
        <Card>
          <>
            <div className="col-md-12">
              <div className="row">
                <div className="col-md-12 d-flex justify-content-end pb-4">
                  <BotoesAcoesRelatorioSemestral />
                </div>
              </div>
            </div>
          </>
          {turmaSelecionada.turma ? (
            <>
              <div className="col-md-12">
                <div className="row">
                  <div className="col-sm-12 col-md-6 col-lg-6 col-xl-4 mb-2">
                    <SelectComponent
                      id="disciplina"
                      name="disciplinaId"
                      lista={listaSemestres}
                      valueOption="semestre"
                      valueText="descricao"
                      valueSelect={semestreSelecionado}
                      onChange={onChangeSemestre}
                      placeholder="Selecione o período"
                    />
                  </div>
                </div>
              </div>
              {semestreSelecionado ? (
                <>
                  <div className="col-md-12 mb-2 d-flex">
                    <BotaoOrdenarListaAlunos />

                    <Button
                      className="btn-imprimir"
                      icon="print"
                      color={Colors.Azul}
                      border
                      onClick={() => {}}
                      disabled
                      id="btn-imprimir-relatorio-semestral"
                    />
                  </div>
                  <div className="col-md-12 mb-2">
                    <TabelaRetratilRelatorioSemestral
                      onChangeAlunoSelecionado={onChangeAlunoSelecionado}
                      permiteOnChangeAluno={permiteOnChangeAluno}
                    >
                      <>
                        <ObjectCardRelatorioSemestral />
                        <DadosRelatorioSemestral
                          codigoTurma={turmaSelecionada.turma}
                          modalidade={turmaSelecionada.modalidade}
                          semestreConsulta={semestreSelecionado}
                        />
                      </>
                    </TabelaRetratilRelatorioSemestral>
                  </div>
                </>
              ) : (
                ''
              )}
            </>
          ) : (
            ''
          )}
        </Card>
      </Loader>
    </Container>
  );
};

export default RelatorioSemestral;
