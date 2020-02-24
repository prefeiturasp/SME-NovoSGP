class ServicoNota {
    temQuantidadeMinimaAprovada = (dados, percentualMinimoAprovados) => {
        let quantidadeAlunos = 0;
        let valorNotaTotal = 0.0;
        if (dados.alunos.length > 0) {
            dados.alunos.forEach(aluno => {
                if (aluno.podeEditar) {
                    let totalNotas = 0.0;
                    quantidadeAlunos++;
                    aluno.notasBimestre.forEach(nota => {
                        totalNotas += nota.notaConceito ? nota.notaConceito : 0;
                    });
                    valorNotaTotal += totalNotas;
                }
            });
            const mediaNotasTotal = valorNotaTotal / quantidadeAlunos;
            const ehPorcentagemAceitavel = (mediaNotasTotal * 10) > percentualMinimoAprovados;
            return ehPorcentagemAceitavel;
        }
        return true;
    };
}

export default new ServicoNota();
