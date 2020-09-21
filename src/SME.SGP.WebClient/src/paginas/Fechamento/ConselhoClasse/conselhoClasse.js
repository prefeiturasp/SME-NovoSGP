import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Loader } from '~/componentes';
import AlertaModalidadeInfantil from '~/componentes-sgp/AlertaModalidadeInfantil/alertaModalidadeInfantil';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Alert from '~/componentes/alert';
import Card from '~/componentes/card';
import RotasDto from '~/dtos/rotasDto';
import {
  limparDadosConselhoClasse,
  setAlunosConselhoClasse,
  setDadosAlunoObjectCard,
} from '~/redux/modulos/conselhoClasse/actions';
import { erros } from '~/servicos/alertas';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';
import { verificaSomenteConsulta } from '~/servicos/servico-navegacao';
import { ehTurmaInfantil } from '~/servicos/Validacoes/validacoesInfatil';
import { Container } from './conselhoClasse.css';
import BotaoGerarRelatorioConselhoClasseTurma from './DadosConselhoClasse/BotaoGerarRelatorioConselhoClasseTurma/botaoGerarRelatorioConselhoClasseTurma';
import BotaoOrdenarListaAlunos from './DadosConselhoClasse/BotaoOrdenarListaAlunos/botaoOrdenarListaAlunos';
import BotoesAcoesConselhoClasse from './DadosConselhoClasse/BotoesAcoes/botoesAcoesConselhoClasse';
import DadosConselhoClasse from './DadosConselhoClasse/dadosConselhoClasse';
import ObjectCardConselhoClasse from './DadosConselhoClasse/ObjectCardConselhoClasse/objectCardConselhoClasse';
import TabelaRetratilConselhoClasse from './DadosConselhoClasse/TabelaRetratilConselhoClasse/tabelaRetratilConselhoClasse';
import servicoSalvarConselhoClasse from './servicoSalvarConselhoClasse';

const ConselhoClasse = () => {
  const dispatch = useDispatch();

  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;
  const { turma, anoLetivo, periodo } = turmaSelecionada;
  const permissoesTela = usuario.permissoes[RotasDto.CONSELHO_CLASSE];

  const [carregandoGeral, setCarregandoGeral] = useState(false);
  const [exibirListas, setExibirListas] = useState(false);

  const modalidadesFiltroPrincipal = useSelector(
    store => store.filtro.modalidades
  );

  const obterListaAlunos = useCallback(async () => {
    setCarregandoGeral(true);
    const retorno = await ServicoConselhoClasse.obterListaAlunos(
      turma,
      anoLetivo,
      periodo
    ).catch(e => erros(e));
    if (retorno && retorno.data) {
      dispatch(setAlunosConselhoClasse(retorno.data));
      setExibirListas(true);
    }
    setCarregandoGeral(false);
  }, [anoLetivo, dispatch, turma, periodo]);

  const resetarInfomacoes = useCallback(() => {
    dispatch(limparDadosConselhoClasse());
  }, [dispatch]);

  useEffect(() => {
    const naoSetarSomenteConsultaNoStore = ehTurmaInfantil(
      modalidadesFiltroPrincipal,
      turmaSelecionada
    );
    verificaSomenteConsulta(permissoesTela, naoSetarSomenteConsultaNoStore);
  }, [turmaSelecionada, permissoesTela, modalidadesFiltroPrincipal]);

  useEffect(() => {
    resetarInfomacoes();
    if (
      turmaSelecionada &&
      turmaSelecionada.turma &&
      !ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada)
    ) {
      obterListaAlunos();
    }
  }, [
    obterListaAlunos,
    turma,
    turmaSelecionada,
    resetarInfomacoes,
    modalidadesFiltroPrincipal,
  ]);

  const obterFrequenciaAluno = async codigoAluno => {
    const retorno = await ServicoConselhoClasse.obterFrequenciaAluno(
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
  };

  const permiteOnChangeAluno = async () => {
    const validouNotaConceitoPosConselho = await servicoSalvarConselhoClasse.validarNotaPosConselho();
    if (validouNotaConceitoPosConselho) {
      const validouAnotacaoRecomendacao = await servicoSalvarConselhoClasse.validarSalvarRecomendacoesAlunoFamilia();
      if (validouNotaConceitoPosConselho && validouAnotacaoRecomendacao) {
        return true;
      }
    }
    return false;
  };

  return (
    <Container>
      {!turmaSelecionada.turma &&
      !ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada) ? (
        <div className="col-md-12">
          <Alert
            alerta={{
              tipo: 'warning',
              id: 'alerta-sem-turma-conselho-classe',
              mensagem: 'Você precisa escolher uma turma.',
              estiloTitulo: { fontSize: '18px' },
            }}
            className="mb-2"
          />
        </div>
      ) : (
        ''
      )}
      <AlertaModalidadeInfantil />
      <Cabecalho pagina="Conselho de classe" />
      <Loader loading={carregandoGeral}>
        <Card>
          <>
            <div className="col-md-12">
              <div className="row">
                <div className="col-md-12 d-flex justify-content-end pb-4">
                  <BotoesAcoesConselhoClasse />
                </div>
              </div>
            </div>
          </>
          {turmaSelecionada.turma &&
          !ehTurmaInfantil(modalidadesFiltroPrincipal, turmaSelecionada) ? (
            <>
              {exibirListas ? (
                <>
                  <div className="col-md-12 mb-2 d-flex">
                    <BotaoOrdenarListaAlunos />
                    <BotaoGerarRelatorioConselhoClasseTurma />
                  </div>
                  <div className="col-md-12 mb-2">
                    <TabelaRetratilConselhoClasse
                      onChangeAlunoSelecionado={onChangeAlunoSelecionado}
                      permiteOnChangeAluno={permiteOnChangeAluno}
                    >
                      <>
                        <ObjectCardConselhoClasse />
                        <DadosConselhoClasse
                          turmaCodigo={turmaSelecionada.turma}
                          modalidade={turmaSelecionada.modalidade}
                        />
                      </>
                    </TabelaRetratilConselhoClasse>
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

export default ConselhoClasse;
