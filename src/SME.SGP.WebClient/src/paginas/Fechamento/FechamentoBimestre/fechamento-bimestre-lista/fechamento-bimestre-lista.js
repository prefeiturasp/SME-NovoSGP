import { Tooltip } from 'antd';
import React, { useState } from 'react';
import Ordenacao from '~/componentes-sgp/Ordenacao/ordenacao';
import FechamentoRegencia from '../fechamanto-regencia/fechamento-regencia';
import BotaoExpandir from './botao-expandir';
import {
  Info,
  MarcadorAulas,
  Marcadores,
  TabelaFechamento,
  SituacaoProcessadoComPendencias,
  DataFechamentoProcessado,
} from './fechamento-bimestre-lista.css';
import { Colors } from '~/componentes';
import Button from '~/componentes/button';
import situacaoFechamentoDto from '~/dtos/situacaoFechamentoDto';
import ServicoFechamentoBimestre from '~/servicos/Paginas/Fechamento/ServicoFechamentoBimestre';
import { erros, sucesso } from '~/servicos/alertas';
import history from '~/servicos/history';
import RotasDto from '~/dtos/rotasDto';
import * as moment from 'moment';
import { BtbAnotacao } from '../fechamento-bimestre.css';
import ModalAnotacaoAluno from '../../FechamentoModalAnotacaoAluno/modal-anotacao-aluno';

const FechamentoBimestreLista = props => {

  const { dados, ehRegencia, ehSintese, codigoComponenteCurricular, turmaId, anoLetivo } = props;  
  const [dadosLista, setDadosLista] = useState(
    dados ? dados.alunos : undefined
  );
  const [situacaoFechamento, setSituacaoFechamento] = useState(dados.situacao);
  const [podeProcessarReprocessar] = useState(dados.podeProcessarReprocessar);
  const [situacaosituacaoNomeFechamento, setSituacaosituacaoNomeFechamento] = useState(dados.situacaoNome);
  const [dataFechamento] = useState(dados.dataFechamento);

  const [exibirModalAnotacao, setExibirModalAnotacao] = useState(false);
  const [alunoModalAnotacao, setAlunoModalAnotacao] = useState({});
  const [fechamentoId, setFechamentoId] = useState(0);

  const mensagempRrocessamento = 'Solicitação de fechamento realizada com sucesso. Em breve você receberá uma notificação com o resultado do processo.';

  const onClickReprocessarNotasConceitos = async () => {
    const processando = await ServicoFechamentoBimestre.reprocessarNotasConceitos(
      dados.fechamentoId
    ).catch(e => erros(e));
    if (processando && processando.status == 200) {
      setSituacaoFechamento(situacaoFechamentoDto.EmProcessamento);
      setSituacaosituacaoNomeFechamento('Em Processamento');
      sucesso(mensagempRrocessamento);
    }
  };

  const onClickProcessarReprocessarSintese = async () => {
    const { alunos, fechamentoId, bimestre } = dados;

    const alunosParaProcessar = alunos.map(aluno => {
      return {
        codigoAluno: aluno.codigoAluno,
        disciplinaId: codigoComponenteCurricular,
        sinteseId: aluno.sinteseId,
      };
    });
    const params = {
      id: fechamentoId,
      turmaId,
      bimestre,
      disciplinaId: codigoComponenteCurricular,
      notaConceitoAlunos: alunosParaProcessar,
    };
    const processando = await ServicoFechamentoBimestre.processarReprocessarSintese([
      params,
    ]).catch(e => erros(e));
    if (processando && processando.status == 200) {
      setSituacaoFechamento(situacaoFechamentoDto.EmProcessamento);
      setSituacaosituacaoNomeFechamento('Em Processamento');
      sucesso(mensagempRrocessamento);
    }
  };

  const onClickVerPendecias = async () => {
    const { bimestre } = dados;
    history.push(`${RotasDto.PENDENCIAS_FECHAMENTO}/${bimestre}/${codigoComponenteCurricular}`);
  };

  const onClickAnotacao = aluno => {
    setFechamentoId(dados.fechamentoId);
    setAlunoModalAnotacao(aluno);
    setExibirModalAnotacao(true);
  };
  
  const onCloseModalAnotacao = (salvou, excluiu) => {
    if (salvou) {
      alunoModalAnotacao.temAnotacao = true;
    } else if (excluiu) {
      alunoModalAnotacao.temAnotacao = false;
    }
    setExibirModalAnotacao(false);
    setAlunoModalAnotacao({});
  };

  return (
    <TabelaFechamento>
      {exibirModalAnotacao ? (
        <ModalAnotacaoAluno
          exibirModal={exibirModalAnotacao}
          onCloseModal={onCloseModalAnotacao}
          fechamentoId={fechamentoId}
          codigoTurma={turmaId}
          anoLetivo={anoLetivo}
          dadosAlunoSelecionado={alunoModalAnotacao}
        ></ModalAnotacaoAluno>
      ) : (
        ''
      )}
      <div className="row pb-4">
       {dados.fechamentoId && dataFechamento ? (
          <div className="col-md-12 d-flex justify-content-end">
            <DataFechamentoProcessado>
              <span>{`${situacaosituacaoNomeFechamento} em ${moment(
                dataFechamento
              ).format('L')} às ${moment(dataFechamento).format('LT')}`}</span>
            </DataFechamentoProcessado>
          </div>
        ) : (
          ''
        )}
        <div className="col-md-6 col-sm-12 d-flex justify-content-start">
          <Ordenacao
            className="botao-ordenacao-avaliacao"
            conteudoParaOrdenar={dadosLista}
            ordenarColunaNumero="numeroChamada"
            ordenarColunaTexto="nome"
            retornoOrdenado={retorno => {
              setDadosLista(retorno);
            }}
            desabilitado={dadosLista ? dadosLista.length <= 0 : true}
          />
          {!ehSintese && podeProcessarReprocessar && situacaoFechamento == situacaoFechamentoDto.ProcessadoComPendencias ? (
            <>
              <Button
                label="Reprocessar"
                color={Colors.Azul}
                border
                className="mr-2"
                onClick={onClickReprocessarNotasConceitos}
              />
              <Button
                label="Ver pendências"
                color={Colors.Azul}
                border
                className="mr-2"
                onClick={onClickVerPendecias}
              />
            </>
          ) : (
            ''
          )}
          {ehSintese && podeProcessarReprocessar && situacaoFechamento != situacaoFechamentoDto.EmProcessamento ? (
            <Button
              label={dados.fechamentoId ? 'Reprocessar' : 'Processar'}
              color={Colors.Azul}
              border
              className="mr-2"
              onClick={onClickProcessarReprocessarSintese}
            />
          ) : (
            ''
          )}
        </div>
        <Marcadores className="col-md-6 col-sm-12 d-flex justify-content-end">          
          <SituacaoProcessadoComPendencias>
            <span>{ situacaoFechamento ? situacaosituacaoNomeFechamento : 'Não executado' }</span>
          </SituacaoProcessadoComPendencias> 
          <MarcadorAulas className="ml-2">
            <span>Aulas previstas </span>
            <span className="numero">
              {dados && dados.totalAulasPrevistas
                ? dados.totalAulasPrevistas
                : 0}
            </span>
          </MarcadorAulas>
          <MarcadorAulas className="ml-2">
            <span>Aulas dadas </span>
            <span className="numero">
              {dados && dados.totalAulasDadas ? dados.totalAulasDadas : 0}
            </span>
          </MarcadorAulas>
        </Marcadores>
      </div>
      <div className="col-md-12 p-0 container-table">
        <table className="table mb-0" id="table-fechamento-bimestre">
          <thead className="tabela-fechamento-thead" key="thead-fechamento">
            <tr>
              <th
                className="text-center fundo-cinza"
                style={{ minWidth: '250px' }}
                colSpan={2}
              >
                Nome
              </th>
              <th className="text-center fundo-cinza">
                {ehSintese ? 'Síntese' : 'Nota/Conceito'}
              </th>
              <th className="text-center fundo-cinza">Faltas no Bimestre</th>
              <th className="text-center fundo-cinza">Ausências Compensadas</th>
              <th className="text-center fundo-cinza">Frequência</th>
            </tr>
          </thead>
          <tbody>
            {dadosLista && dadosLista.length > 0 ? (
              dadosLista.map((item, index) => {
                const idLinhaRegencia = `fechamento-regencia-${index}`;
                return (
                  <>
                    <tr>
                      <td
                        className={`text-center ${
                          !item.ativo ? 'fundo-cinza' : ''
                        }`}
                      >
                        {item.numeroChamada}
                        {item.informacao ? (
                          <Tooltip title={item.informacao} placement="top">
                            <Info className="fas fa-circle" />
                          </Tooltip>
                        ) : (
                          ''
                        )}
                      </td>
                      <td className={`${!item.ativo ? 'fundo-cinza' : ''}`}>
                      <div
                          className="d-flex"
                          style={{ justifyContent: 'space-between' }}
                        >
                          <div className=" d-flex justify-content-start">
                            {item.nome}
                          </div>
                          { item.ativo ?
                            <Tooltip title={item.temAnotacao ? 'Estudante com anotação' : ''} placement="top">
                              <div className=" d-flex justify-content-end">
                                <BtbAnotacao className={item.temAnotacao ? 'btn-com-anotacao' : ''} onClick={() => onClickAnotacao(item)}>
                                  <i class="fas fa-pen" />
                                </BtbAnotacao> 
                              </div>
                            </Tooltip> : ''
                          }
                        </div>
                      </td>
                      <td
                        className={`text-center ${
                          !item.ativo ? 'fundo-cinza' : ''
                        }`}
                      >
                        {ehSintese ? (
                          item.sintese
                        ) : ehRegencia && item.notas ? (
                          <BotaoExpandir
                            index={index}
                            idLinhaRegencia={idLinhaRegencia}
                          />
                        ) : item.notas && item.notas.length > 0 ? (
                          item.notas[0].ehConceito ? (
                            item.notas[0].conceitoDescricao
                          ) : (
                            ServicoFechamentoBimestre.formatarNotaConceito(
                              item.notas[0].notaConceito
                            )
                          )
                        ) : null}
                      </td>
                      <td
                        className={`text-center ${
                          !item.ativo ? 'fundo-cinza' : ''
                        }`}
                      >
                        {item.quantidadeFaltas}
                      </td>
                      <td
                        className={`text-center ${
                          !item.ativo ? 'fundo-cinza' : ''
                        }`}
                      >
                        {item.quantidadeCompensacoes}
                      </td>
                      <td
                        className={`text-center ${
                          !item.ativo ? 'fundo-cinza' : ''
                        }`}
                      >
                        {item.percentualFrequencia
                          ? `${item.percentualFrequencia} %`
                          : '0%'}
                      </td>
                    </tr>
                    {!ehSintese && ehRegencia ? (
                      <FechamentoRegencia
                        dados={item.notas}
                        idRegencia={`fechamento-regencia-${index}`}
                      />
                    ) : null}
                  </>
                );
              })
            ) : (
              <tr>
                <td colSpan="6" className="text-center">
                  Sem dados
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </TabelaFechamento>
  );
};

export default FechamentoBimestreLista;
