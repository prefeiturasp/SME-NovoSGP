import React, { useState, useEffect } from 'react';
import { useSelector } from 'react-redux';
import { Loader } from '~/componentes';
import { Ordenacao } from '~/componentes-sgp';
import Cabecalho from '~/componentes-sgp/cabecalho';
import Alert from '~/componentes/alert';
import DetalhesAluno from '~/componentes/Alunos/Detalhes';
import Button from '~/componentes/button';
import Card from '~/componentes/card';
import { Colors } from '~/componentes/colors';
import TabelaRetratil from '~/componentes/TabelaRetratil';
import { Container } from './conselhoClasse.css';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';
import { erros } from '~/servicos/alertas';
import DadosConselhoClasse from './DadosConselhoClasse/dadosConselhoClasse';

const ConselhoClasse = () => {
  const usuario = useSelector(store => store.usuario);
  const { turmaSelecionada } = usuario;

  const [carregandoGeral, setCarregandoGeral] = useState(false);
  const [alunos, setAlunos] = useState([]);
  const [dadosAluno, setDadosAluno] = useState({});

  const resetarTela = () => {
    setDadosAluno({});
    setAlunos([]);
  };

  const obterListaAlunos = async () => {
    const turmaCodigo = turmaSelecionada.turma;
    const anoLetivo = turmaSelecionada.anoLetivo;
    const retorno = await ServicoConselhoClasse.obterListaAlunos(
      turmaCodigo,
      anoLetivo
    ).catch(e => erros(e));
    if (retorno && retorno.data) {
      setAlunos(retorno.data);
    }
  };

  useEffect(() => {
    if (turmaSelecionada.turma) {
      obterListaAlunos();
    } else {
      // TODO - Reseta tela
    }
  }, []);

  const obterFrequenciaAluno = async codigoAluno => {
    const retorno = await ServicoConselhoClasse.obterFrequenciaAluno(
      codigoAluno
    ).catch(e => erros(e));
    if (retorno && retorno.data) {
      return retorno.data;
    }
    return 0;
  };

  const onChangeAlunoSelecionado = async aluno => {
    const frequenciaGeralAluno = await obterFrequenciaAluno(aluno.codigoEOL);
    aluno.frequencia = frequenciaGeralAluno;
    setDadosAluno(aluno);
  };

  const onClickVoltar = () => {
    console.log('onClickVoltar');
  };

  const onClickCancelar = () => {
    console.log('onClickCancelar');
  };

  const onClickSalvar = () => {
    console.log('onClickSalvar');
  };

  return (
    <Container>
      {!turmaSelecionada.turma ? (
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
      <Cabecalho pagina="Conselho de classe" />
      <Loader loading={carregandoGeral}>
        <Card>
          {turmaSelecionada.turma ? (
            <>
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
                      disabled={!alunos || alunos.length < 1}
                    />
                    <Button
                      label="Salvar"
                      color={Colors.Roxo}
                      border
                      bold
                      className="mr-2"
                      onClick={onClickSalvar}
                      disabled={!alunos || alunos.length < 1}
                    />
                  </div>
                </div>
              </div>
              {alunos && alunos.length ? (
                <>
                  <div className="col-md-12 mb-2 d-flex">
                    <Ordenacao
                      conteudoParaOrdenar={alunos}
                      ordenarColunaNumero="id"
                      ordenarColunaTexto="nome"
                      retornoOrdenado={retorno => setAlunos(retorno)}
                    />
                    <Button
                      className="btn-imprimir"
                      icon="print"
                      color={Colors.Azul}
                      border
                      onClick={() => {}}
                      disabled
                    />
                  </div>
                  <div className="col-md-12 mb-2">
                    <TabelaRetratil
                      onChangeAlunoSelecionado={onChangeAlunoSelecionado}
                      alunos={alunos}
                    >
                      <DetalhesAluno dados={dadosAluno} />
                      <DadosConselhoClasse></DadosConselhoClasse>
                    </TabelaRetratil>
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
