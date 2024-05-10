-- 1. Seleciona os alunos pelos emails levantados
IF OBJECT_ID('tempdb..#tempAlunosQueForamSuspensosIndevidamente') IS NOT NULL
	DROP TABLE #tempAlunosQueForamSuspensosIndevidamente;
SELECT
	DISTINCT
	*
INTO #tempAlunosQueForamSuspensosIndevidamente
FROM
	aluno_classroom (NOLOCK)
WHERE nm_email IN
(
	'irisbsamaral.20032017@edu.sme.prefeitura.sp.gov.br',
	'isaacbcosta.09012017@edu.sme.prefeitura.sp.gov.br',
	'isaachsantos.09072016@edu.sme.prefeitura.sp.gov.br',
	'manuelapbranco.15112016@edu.sme.prefeitura.sp.gov.br',
	'manuellammatos.05122014@edu.sme.prefeitura.sp.gov.br',
	'manuellammatos.05122014@edu.sme.prefeitura.sp.gov.br',
	'manuellammatos.05122014@edu.sme.prefeitura.sp.gov.br',
	'manuellammatos.05122014@edu.sme.prefeitura.sp.gov.br',
	'manuellammatos.05122014@edu.sme.prefeitura.sp.gov.br',
	'manuellammatos.05122014@edu.sme.prefeitura.sp.gov.br',
	'ryanvavenancio.12052016@edu.sme.prefeitura.sp.gov.br',
	'carlosessantos.30052006@edu.sme.prefeitura.sp.gov.br',
	'carlosessantos.30052006@edu.sme.prefeitura.sp.gov.br',
	'carlosessantos.30052006@edu.sme.prefeitura.sp.gov.br',
	'carlosessantos.30052006@edu.sme.prefeitura.sp.gov.br',
	'carlosessantos.30052006@edu.sme.prefeitura.sp.gov.br',
	'carlosessantos.30052006@edu.sme.prefeitura.sp.gov.br',
	'carlosessantos.30052006@edu.sme.prefeitura.sp.gov.br',
	'carlosessantos.30052006@edu.sme.prefeitura.sp.gov.br',
	'carlosessantos.30052006@edu.sme.prefeitura.sp.gov.br',
	'carlosessantos.30052006@edu.sme.prefeitura.sp.gov.br',
	'isabellassantos.29012017@edu.sme.prefeitura.sp.gov.br',
	'samuelslustosa.20122016@edu.sme.prefeitura.sp.gov.br',
	'sarahbferreira.30042016@edu.sme.prefeitura.sp.gov.br',
	'sarahrmsantos.01042016@edu.sme.prefeitura.sp.gov.br',
	'mariacpsantos.24052016@edu.sme.prefeitura.sp.gov.br',
	'mariacssousa.14111970@edu.sme.prefeitura.sp.gov.br',
	'mariacssousa.14111970@edu.sme.prefeitura.sp.gov.br',
	'mariacssousa.14111970@edu.sme.prefeitura.sp.gov.br',
	'isaquejbmendes.28012017@edu.sme.prefeitura.sp.gov.br',
	'israelsaraujo.04072016@edu.sme.prefeitura.sp.gov.br',
	'solangesoliveira.07031973@edu.sme.prefeitura.sp.gov.br',
	'solangesoliveira.07031973@edu.sme.prefeitura.sp.gov.br',
	'solangesoliveira.07031973@edu.sme.prefeitura.sp.gov.br',
	'sophiaacampezzi.12022017@edu.sme.prefeitura.sp.gov.br',
	'cosminarcassimiro.27091960@edu.sme.prefeitura.sp.gov.br',
	'cosminarcassimiro.27091960@edu.sme.prefeitura.sp.gov.br',
	'cosminarcassimiro.27091960@edu.sme.prefeitura.sp.gov.br',
	'cristianocrmoscoso.25102016@edu.sme.prefeitura.sp.gov.br',
	'mariaecsantos.05062009@edu.sme.prefeitura.sp.gov.br',
	'mariaecsantos.05062009@edu.sme.prefeitura.sp.gov.br',
	'mariaecsantos.05062009@edu.sme.prefeitura.sp.gov.br',
	'mariaecsantos.05062009@edu.sme.prefeitura.sp.gov.br',
	'mariaecsantos.05062009@edu.sme.prefeitura.sp.gov.br',
	'mariaecsantos.05062009@edu.sme.prefeitura.sp.gov.br',
	'mariaecsantos.05062009@edu.sme.prefeitura.sp.gov.br',
	'mariaecsantos.05062009@edu.sme.prefeitura.sp.gov.br',
	'mariaecsantos.05062009@edu.sme.prefeitura.sp.gov.br',
	'mariaecsantos.05062009@edu.sme.prefeitura.sp.gov.br',
	'sophiadpasilva.31072016@edu.sme.prefeitura.sp.gov.br',
	'danielcoliveira.31072016@edu.sme.prefeitura.sp.gov.br',
	'sophiaofsantos.15042016@edu.sme.prefeitura.sp.gov.br',
	'daniellmartins.22042007@edu.sme.prefeitura.sp.gov.br',
	'daniellmartins.22042007@edu.sme.prefeitura.sp.gov.br',
	'daniellmartins.22042007@edu.sme.prefeitura.sp.gov.br',
	'daniellmartins.22042007@edu.sme.prefeitura.sp.gov.br',
	'daniellmartins.22042007@edu.sme.prefeitura.sp.gov.br',
	'daniellmartins.22042007@edu.sme.prefeitura.sp.gov.br',
	'daniellmartins.22042007@edu.sme.prefeitura.sp.gov.br',
	'daniellmartins.22042007@edu.sme.prefeitura.sp.gov.br',
	'daniellmartins.22042007@edu.sme.prefeitura.sp.gov.br',
	'daniellmartins.22042007@edu.sme.prefeitura.sp.gov.br',
	'darioroliveira.21102016@edu.sme.prefeitura.sp.gov.br',
	'suelenvgsantos.25082004@edu.sme.prefeitura.sp.gov.br',
	'suelenvgsantos.25082004@edu.sme.prefeitura.sp.gov.br',
	'suelenvgsantos.25082004@edu.sme.prefeitura.sp.gov.br',
	'suelenvgsantos.25082004@edu.sme.prefeitura.sp.gov.br',
	'suelenvgsantos.25082004@edu.sme.prefeitura.sp.gov.br',
	'suelenvgsantos.25082004@edu.sme.prefeitura.sp.gov.br',
	'suelenvgsantos.25082004@edu.sme.prefeitura.sp.gov.br',
	'suelenvgsantos.25082004@edu.sme.prefeitura.sp.gov.br',
	'suelenvgsantos.25082004@edu.sme.prefeitura.sp.gov.br',
	'mariahcsouza.21022015@edu.sme.prefeitura.sp.gov.br',
	'mariahcsouza.21022015@edu.sme.prefeitura.sp.gov.br',
	'mariahcsouza.21022015@edu.sme.prefeitura.sp.gov.br',
	'mariahcsouza.21022015@edu.sme.prefeitura.sp.gov.br',
	'mariahcsouza.21022015@edu.sme.prefeitura.sp.gov.br',
	'mariahcsouza.21022015@edu.sme.prefeitura.sp.gov.br',
	'joaogfevangelista.05072016@edu.sme.prefeitura.sp.gov.br',
	'davilamenezes.31082016@edu.sme.prefeitura.sp.gov.br',
	'davilasilva.03032018@edu.sme.prefeitura.sp.gov.br',
	'davilgsilva.26092016@edu.sme.prefeitura.sp.gov.br',
	'davilhmarinho.07082016@edu.sme.prefeitura.sp.gov.br',
	'marialabueno.10052016@edu.sme.prefeitura.sp.gov.br',
	'marialmpereira.20062016@edu.sme.prefeitura.sp.gov.br',
	'joaommsantos.06082016@edu.sme.prefeitura.sp.gov.br',
	'joaomsoliveira.16012014@edu.sme.prefeitura.sp.gov.br',
	'joaomsoliveira.16012014@edu.sme.prefeitura.sp.gov.br',
	'joaomsoliveira.16012014@edu.sme.prefeitura.sp.gov.br',
	'joaomsoliveira.16012014@edu.sme.prefeitura.sp.gov.br',
	'joaomsoliveira.16012014@edu.sme.prefeitura.sp.gov.br',
	'joaomsoliveira.16012014@edu.sme.prefeitura.sp.gov.br',
	'davimsilva.11042016@edu.sme.prefeitura.sp.gov.br',
	'marianaassilva.19122016@edu.sme.prefeitura.sp.gov.br',
	'joaopamariz.13112014@edu.sme.prefeitura.sp.gov.br',
	'joaopamariz.13112014@edu.sme.prefeitura.sp.gov.br',
	'joaopamariz.13112014@edu.sme.prefeitura.sp.gov.br',
	'joaopamariz.13112014@edu.sme.prefeitura.sp.gov.br',
	'joaopamariz.13112014@edu.sme.prefeitura.sp.gov.br',
	'joaopamariz.13112014@edu.sme.prefeitura.sp.gov.br',
	'joaopfevangelista.05072016@edu.sme.prefeitura.sp.gov.br',
	'joaopmfernandes.26032015@edu.sme.prefeitura.sp.gov.br',
	'joaopmfernandes.26032015@edu.sme.prefeitura.sp.gov.br',
	'joaopmfernandes.26032015@edu.sme.prefeitura.sp.gov.br',
	'joaopmfernandes.26032015@edu.sme.prefeitura.sp.gov.br',
	'joaopmfernandes.26032015@edu.sme.prefeitura.sp.gov.br',
	'joaopmfernandes.26032015@edu.sme.prefeitura.sp.gov.br',
	'davitcardoso.01092014@edu.sme.prefeitura.sp.gov.br',
	'davitcardoso.01092014@edu.sme.prefeitura.sp.gov.br',
	'davitcardoso.01092014@edu.sme.prefeitura.sp.gov.br',
	'davitcardoso.01092014@edu.sme.prefeitura.sp.gov.br',
	'davitcardoso.01092014@edu.sme.prefeitura.sp.gov.br',
	'davitcardoso.01092014@edu.sme.prefeitura.sp.gov.br',
	'antoniemamatos.18082016@edu.sme.prefeitura.sp.gov.br',
	'girlandessantos.23061979@edu.sme.prefeitura.sp.gov.br',
	'girlandessantos.23061979@edu.sme.prefeitura.sp.gov.br',
	'girlandessantos.23061979@edu.sme.prefeitura.sp.gov.br',
	'girlandessantos.23061979@edu.sme.prefeitura.sp.gov.br',
	'girlandessantos.23061979@edu.sme.prefeitura.sp.gov.br',
	'girlandessantos.23061979@edu.sme.prefeitura.sp.gov.br',
	'girlandessantos.23061979@edu.sme.prefeitura.sp.gov.br',
	'girlandessantos.23061979@edu.sme.prefeitura.sp.gov.br',
	'girlandessantos.23061979@edu.sme.prefeitura.sp.gov.br',
	'pietraopinheiro.28062014@edu.sme.prefeitura.sp.gov.br',
	'pietraopinheiro.28062014@edu.sme.prefeitura.sp.gov.br',
	'pietraopinheiro.28062014@edu.sme.prefeitura.sp.gov.br',
	'pietraopinheiro.28062014@edu.sme.prefeitura.sp.gov.br',
	'pietraopinheiro.28062014@edu.sme.prefeitura.sp.gov.br',
	'pietraopinheiro.28062014@edu.sme.prefeitura.sp.gov.br',
	'antoniojlsantos.25072016@edu.sme.prefeitura.sp.gov.br',
	'pyetrossantos.19032015@edu.sme.prefeitura.sp.gov.br',
	'pyetrossantos.19032015@edu.sme.prefeitura.sp.gov.br',
	'pyetrossantos.19032015@edu.sme.prefeitura.sp.gov.br',
	'pyetrossantos.19032015@edu.sme.prefeitura.sp.gov.br',
	'pyetrossantos.19032015@edu.sme.prefeitura.sp.gov.br',
	'pyetrossantos.19032015@edu.sme.prefeitura.sp.gov.br',
	'arthurcsilva.29102014@edu.sme.prefeitura.sp.gov.br',
	'arthurcsilva.29102014@edu.sme.prefeitura.sp.gov.br',
	'arthurcsilva.29102014@edu.sme.prefeitura.sp.gov.br',
	'arthurcsilva.29102014@edu.sme.prefeitura.sp.gov.br',
	'arthurcsilva.29102014@edu.sme.prefeitura.sp.gov.br',
	'arthurcsilva.29102014@edu.sme.prefeitura.sp.gov.br',
	'guilhermelsilva.31102013@edu.sme.prefeitura.sp.gov.br',
	'guilhermelsilva.31102013@edu.sme.prefeitura.sp.gov.br',
	'guilhermelsilva.31102013@edu.sme.prefeitura.sp.gov.br',
	'guilhermelsilva.31102013@edu.sme.prefeitura.sp.gov.br',
	'guilhermelsilva.31102013@edu.sme.prefeitura.sp.gov.br',
	'guilhermelsilva.31102013@edu.sme.prefeitura.sp.gov.br',
	'arthurqpimenta.12062016@edu.sme.prefeitura.sp.gov.br',
	'guilhermeoaraujo.01082016@edu.sme.prefeitura.sp.gov.br',
	'guilhermesoliveira.12062007@edu.sme.prefeitura.sp.gov.br',
	'guilhermesoliveira.12062007@edu.sme.prefeitura.sp.gov.br',
	'guilhermesoliveira.12062007@edu.sme.prefeitura.sp.gov.br',
	'guilhermesoliveira.12062007@edu.sme.prefeitura.sp.gov.br',
	'guilhermesoliveira.12062007@edu.sme.prefeitura.sp.gov.br',
	'guilhermesoliveira.12062007@edu.sme.prefeitura.sp.gov.br',
	'guilhermesoliveira.12062007@edu.sme.prefeitura.sp.gov.br',
	'guilhermesoliveira.12062007@edu.sme.prefeitura.sp.gov.br',
	'guilhermesoliveira.12062007@edu.sme.prefeitura.sp.gov.br',
	'guilhermesoliveira.12062007@edu.sme.prefeitura.sp.gov.br',
	'rafaellagrosa.23092014@edu.sme.prefeitura.sp.gov.br',
	'rafaellagrosa.23092014@edu.sme.prefeitura.sp.gov.br',
	'rafaellagrosa.23092014@edu.sme.prefeitura.sp.gov.br',
	'rafaellagrosa.23092014@edu.sme.prefeitura.sp.gov.br',
	'rafaellagrosa.23092014@edu.sme.prefeitura.sp.gov.br',
	'rafaellagrosa.23092014@edu.sme.prefeitura.sp.gov.br',
	'lucassrocha.03012006@edu.sme.prefeitura.sp.gov.br',
	'lucassrocha.03012006@edu.sme.prefeitura.sp.gov.br',
	'lucassrocha.03012006@edu.sme.prefeitura.sp.gov.br',
	'lucassrocha.03012006@edu.sme.prefeitura.sp.gov.br',
	'lucassrocha.03012006@edu.sme.prefeitura.sp.gov.br',
	'lucassrocha.03012006@edu.sme.prefeitura.sp.gov.br',
	'lucassrocha.03012006@edu.sme.prefeitura.sp.gov.br',
	'lucassrocha.03012006@edu.sme.prefeitura.sp.gov.br',
	'lucassrocha.03012006@edu.sme.prefeitura.sp.gov.br',
	'lucassrocha.03012006@edu.sme.prefeitura.sp.gov.br',
	'beatrizjnascimento.11072016@edu.sme.prefeitura.sp.gov.br',
	'raulmgazarias.14072014@edu.sme.prefeitura.sp.gov.br',
	'raulmgazarias.14072014@edu.sme.prefeitura.sp.gov.br',
	'raulmgazarias.14072014@edu.sme.prefeitura.sp.gov.br',
	'raulmgazarias.14072014@edu.sme.prefeitura.sp.gov.br',
	'raulmgazarias.14072014@edu.sme.prefeitura.sp.gov.br',
	'raulmgazarias.14072014@edu.sme.prefeitura.sp.gov.br',
	'beatrizsmartins.27062016@edu.sme.prefeitura.sp.gov.br',
	'luisgmgsilva.12052016@edu.sme.prefeitura.sp.gov.br',
	'bernardoghessel.05102016@edu.sme.prefeitura.sp.gov.br',
	'bernardojmmedeiros.20072016@edu.sme.prefeitura.sp.gov.br',
	'hayllasferreira.11072016@edu.sme.prefeitura.sp.gov.br',
	'heitorlsantos.29102018@edu.sme.prefeitura.sp.gov.br',
	'helenaaarunisi.15082016@edu.sme.prefeitura.sp.gov.br',
	'renanacarvalho.27052016@edu.sme.prefeitura.sp.gov.br',
	'brennomdribeiro.29122016@edu.sme.prefeitura.sp.gov.br',
	'luizmpsilveira.28012015@edu.sme.prefeitura.sp.gov.br',
	'luizmpsilveira.28012015@edu.sme.prefeitura.sp.gov.br',
	'luizmpsilveira.28012015@edu.sme.prefeitura.sp.gov.br',
	'luizmpsilveira.28012015@edu.sme.prefeitura.sp.gov.br',
	'luizmpsilveira.28012015@edu.sme.prefeitura.sp.gov.br',
	'luizmpsilveira.28012015@edu.sme.prefeitura.sp.gov.br',
	'henzogrssilva.01102016@edu.sme.prefeitura.sp.gov.br',
	'lyviafsouza.23052016@edu.sme.prefeitura.sp.gov.br',
	'bryanfcarvalho.16022017@edu.sme.prefeitura.sp.gov.br',
	'hugogmsantos.03022016@edu.sme.prefeitura.sp.gov.br',
	'manuelagsilva.28012017@edu.sme.prefeitura.sp.gov.br',
	'yasminpsilva.30122016@edu.sme.prefeitura.sp.gov.br',
	'yasminssantana.12042016@edu.sme.prefeitura.sp.gov.br',
	'yurimraizava.31032017@edu.sme.prefeitura.sp.gov.br',
	'zaionwcsilva.19102007@edu.sme.prefeitura.sp.gov.br',
	'zaionwcsilva.19102007@edu.sme.prefeitura.sp.gov.br',
	'zaionwcsilva.19102007@edu.sme.prefeitura.sp.gov.br',
	'zaionwcsilva.19102007@edu.sme.prefeitura.sp.gov.br',
	'zaionwcsilva.19102007@edu.sme.prefeitura.sp.gov.br',
	'zaionwcsilva.19102007@edu.sme.prefeitura.sp.gov.br',
	'zaionwcsilva.19102007@edu.sme.prefeitura.sp.gov.br',
	'zaionwcsilva.19102007@edu.sme.prefeitura.sp.gov.br',
	'zaionwcsilva.19102007@edu.sme.prefeitura.sp.gov.br',
	'zaionwcsilva.19102007@edu.sme.prefeitura.sp.gov.br',
	'nicollemcunha.11042014@edu.sme.prefeitura.sp.gov.br',
	'nicollemcunha.11042014@edu.sme.prefeitura.sp.gov.br',
	'nicollemcunha.11042014@edu.sme.prefeitura.sp.gov.br',
	'nicollemcunha.11042014@edu.sme.prefeitura.sp.gov.br',
	'nicollemcunha.11042014@edu.sme.prefeitura.sp.gov.br',
	'nicollemcunha.11042014@edu.sme.prefeitura.sp.gov.br',
	'nicollyosilva.28032015@edu.sme.prefeitura.sp.gov.br',
	'anacahenrique.23022017@edu.sme.prefeitura.sp.gov.br',
	'nicollyosilva.28032015@edu.sme.prefeitura.sp.gov.br',
	'nicollyosilva.28032015@edu.sme.prefeitura.sp.gov.br',
	'nicollyosilva.28032015@edu.sme.prefeitura.sp.gov.br',
	'nicollyosilva.28032015@edu.sme.prefeitura.sp.gov.br',
	'nicollyosilva.28032015@edu.sme.prefeitura.sp.gov.br',
	'oseiasmsmarques.05012017@edu.sme.prefeitura.sp.gov.br',
	'larissaimrodrigues.07122016@edu.sme.prefeitura.sp.gov.br',
	'gabrielfbatrina.19062016@edu.sme.prefeitura.sp.gov.br',
	'pablohlopes.25042016@edu.sme.prefeitura.sp.gov.br',
	'paollacpspirlandeli.22092007@edu.sme.prefeitura.sp.gov.br',
	'paollacpspirlandeli.22092007@edu.sme.prefeitura.sp.gov.br',
	'paollacpspirlandeli.22092007@edu.sme.prefeitura.sp.gov.br',
	'paollacpspirlandeli.22092007@edu.sme.prefeitura.sp.gov.br',
	'paollacpspirlandeli.22092007@edu.sme.prefeitura.sp.gov.br',
	'paollacpspirlandeli.22092007@edu.sme.prefeitura.sp.gov.br',
	'paollacpspirlandeli.22092007@edu.sme.prefeitura.sp.gov.br',
	'paollacpspirlandeli.22092007@edu.sme.prefeitura.sp.gov.br',
	'paollacpspirlandeli.22092007@edu.sme.prefeitura.sp.gov.br',
	'paollacpspirlandeli.22092007@edu.sme.prefeitura.sp.gov.br',
	'paulocpinheiro.12082016@edu.sme.prefeitura.sp.gov.br',
	'lauramrodrigues.02032015@edu.sme.prefeitura.sp.gov.br',
	'lauramrodrigues.02032015@edu.sme.prefeitura.sp.gov.br',
	'lauramrodrigues.02032015@edu.sme.prefeitura.sp.gov.br',
	'lauramrodrigues.02032015@edu.sme.prefeitura.sp.gov.br',
	'lauramrodrigues.02032015@edu.sme.prefeitura.sp.gov.br',
	'lauramrodrigues.02032015@edu.sme.prefeitura.sp.gov.br',
	'paulohssilva.21092007@edu.sme.prefeitura.sp.gov.br',
	'paulohssilva.21092007@edu.sme.prefeitura.sp.gov.br',
	'paulohssilva.21092007@edu.sme.prefeitura.sp.gov.br',
	'paulohssilva.21092007@edu.sme.prefeitura.sp.gov.br',
	'paulohssilva.21092007@edu.sme.prefeitura.sp.gov.br',
	'paulohssilva.21092007@edu.sme.prefeitura.sp.gov.br',
	'paulohssilva.21092007@edu.sme.prefeitura.sp.gov.br',
	'paulohssilva.21092007@edu.sme.prefeitura.sp.gov.br',
	'paulohssilva.21092007@edu.sme.prefeitura.sp.gov.br',
	'paulohssilva.21092007@edu.sme.prefeitura.sp.gov.br',
	'pedroebsousa.31102016@edu.sme.prefeitura.sp.gov.br',
	'pedrohbsilva.29062014@edu.sme.prefeitura.sp.gov.br',
	'pedrohbsilva.29062014@edu.sme.prefeitura.sp.gov.br',
	'pedrohbsilva.29062014@edu.sme.prefeitura.sp.gov.br',
	'pedrohbsilva.29062014@edu.sme.prefeitura.sp.gov.br',
	'pedrohbsilva.29062014@edu.sme.prefeitura.sp.gov.br',
	'pedrohbsilva.29062014@edu.sme.prefeitura.sp.gov.br',
	'leonardocciccarelli.20032015@edu.sme.prefeitura.sp.gov.br',
	'leonardocciccarelli.20032015@edu.sme.prefeitura.sp.gov.br',
	'leonardocciccarelli.20032015@edu.sme.prefeitura.sp.gov.br',
	'leonardocciccarelli.20032015@edu.sme.prefeitura.sp.gov.br',
	'leonardocciccarelli.20032015@edu.sme.prefeitura.sp.gov.br',
	'leonardocciccarelli.20032015@edu.sme.prefeitura.sp.gov.br',
	'pedrohfsilva.16042014@edu.sme.prefeitura.sp.gov.br',
	'pedrohfsilva.16042014@edu.sme.prefeitura.sp.gov.br',
	'pedrohfsilva.16042014@edu.sme.prefeitura.sp.gov.br',
	'pedrohfsilva.16042014@edu.sme.prefeitura.sp.gov.br',
	'pedrohfsilva.16042014@edu.sme.prefeitura.sp.gov.br',
	'pedrohfsilva.16042014@edu.sme.prefeitura.sp.gov.br',
	'andersonhgsilva.10032017@edu.sme.prefeitura.sp.gov.br',
	'andreagfrederico.08032020@edu.sme.prefeitura.sp.gov.br',
	'geovannisferreira.23092016@edu.sme.prefeitura.sp.gov.br',
	'ginaldoasilva.05011974@edu.sme.prefeitura.sp.gov.br',
	'ginaldoasilva.05011974@edu.sme.prefeitura.sp.gov.br',
	'ginaldoasilva.05011974@edu.sme.prefeitura.sp.gov.br',
	'ginaldoasilva.05011974@edu.sme.prefeitura.sp.gov.br',
	'ginaldoasilva.05011974@edu.sme.prefeitura.sp.gov.br',
	'ginaldoasilva.05011974@edu.sme.prefeitura.sp.gov.br',
	'ginaldoasilva.05011974@edu.sme.prefeitura.sp.gov.br',
	'ginaldoasilva.05011974@edu.sme.prefeitura.sp.gov.br',
	'ginaldoasilva.05011974@edu.sme.prefeitura.sp.gov.br',
	'pedrohssilva.18062007@edu.sme.prefeitura.sp.gov.br',
	'pedrohssilva.18062007@edu.sme.prefeitura.sp.gov.br',
	'pedrohssilva.18062007@edu.sme.prefeitura.sp.gov.br',
	'pedrohssilva.18062007@edu.sme.prefeitura.sp.gov.br',
	'pedrohssilva.18062007@edu.sme.prefeitura.sp.gov.br',
	'pedrohssilva.18062007@edu.sme.prefeitura.sp.gov.br',
	'pedrohssilva.18062007@edu.sme.prefeitura.sp.gov.br',
	'pedrohssilva.18062007@edu.sme.prefeitura.sp.gov.br',
	'pedrohssilva.18062007@edu.sme.prefeitura.sp.gov.br',
	'pedrohssilva.18062007@edu.sme.prefeitura.sp.gov.br',
	'liviafleme.17032017@edu.sme.prefeitura.sp.gov.br',
	'annactcarvalho.27022016@edu.sme.prefeitura.sp.gov.br',
	'pedrovcardoso.12022017@edu.sme.prefeitura.sp.gov.br',
	'lorenamsantos.28042016@edu.sme.prefeitura.sp.gov.br',
	'adamttalmadge.07042016@edu.sme.prefeitura.sp.gov.br',
	'dimasotsantana.22122016@edu.sme.prefeitura.sp.gov.br',
	'eduardoadamasceno.03042014@edu.sme.prefeitura.sp.gov.br',
	'eduardoadamasceno.03042014@edu.sme.prefeitura.sp.gov.br',
	'eduardoadamasceno.03042014@edu.sme.prefeitura.sp.gov.br',
	'eduardoadamasceno.03042014@edu.sme.prefeitura.sp.gov.br',
	'eduardoadamasceno.03042014@edu.sme.prefeitura.sp.gov.br',
	'eduardoadamasceno.03042014@edu.sme.prefeitura.sp.gov.br',
	'eloahcmsouza.23022019@edu.sme.prefeitura.sp.gov.br',
	'eloahmsantos.24042016@edu.sme.prefeitura.sp.gov.br',
	'emanuellyaferreira.24072016@edu.sme.prefeitura.sp.gov.br',
	'emilylgomes.17032015@edu.sme.prefeitura.sp.gov.br',
	'emilylgomes.17032015@edu.sme.prefeitura.sp.gov.br',
	'emilylgomes.17032015@edu.sme.prefeitura.sp.gov.br',
	'emilylgomes.17032015@edu.sme.prefeitura.sp.gov.br',
	'emilylgomes.17032015@edu.sme.prefeitura.sp.gov.br',
	'emilylgomes.17032015@edu.sme.prefeitura.sp.gov.br',
	'enzogrsantos.17032017@edu.sme.prefeitura.sp.gov.br',
	'enzorroman.10102014@edu.sme.prefeitura.sp.gov.br',
	'enzorroman.10102014@edu.sme.prefeitura.sp.gov.br',
	'enzorroman.10102014@edu.sme.prefeitura.sp.gov.br',
	'enzorroman.10102014@edu.sme.prefeitura.sp.gov.br',
	'enzorroman.10102014@edu.sme.prefeitura.sp.gov.br',
	'enzorroman.10102014@edu.sme.prefeitura.sp.gov.br',
	'enzowangelo.01042016@edu.sme.prefeitura.sp.gov.br',
	'johnksnascimento.02032017@edu.sme.prefeitura.sp.gov.br',
	'joseadomingues.19091954@edu.sme.prefeitura.sp.gov.br',
	'joseadomingues.19091954@edu.sme.prefeitura.sp.gov.br',
	'joseadomingues.19091954@edu.sme.prefeitura.sp.gov.br',
	'josersousa.03101978@edu.sme.prefeitura.sp.gov.br',
	'josersousa.03101978@edu.sme.prefeitura.sp.gov.br',
	'josersousa.03101978@edu.sme.prefeitura.sp.gov.br',
	'katarinavbsilva.06012017@edu.sme.prefeitura.sp.gov.br',
	'miguelalaoliveira.06012017@edu.sme.prefeitura.sp.gov.br',
	'miguelfsoares.15062014@edu.sme.prefeitura.sp.gov.br',
	'miguelfsoares.15062014@edu.sme.prefeitura.sp.gov.br',
	'miguelfsoares.15062014@edu.sme.prefeitura.sp.gov.br',
	'miguelfsoares.15062014@edu.sme.prefeitura.sp.gov.br',
	'miguelfsoares.15062014@edu.sme.prefeitura.sp.gov.br',
	'miguelfsoares.15062014@edu.sme.prefeitura.sp.gov.br',
	'miguelscarvalho.22022017@edu.sme.prefeitura.sp.gov.br',
	'miguelssilva.14032017@edu.sme.prefeitura.sp.gov.br',
	'miriampalbuquerque.01102016@edu.sme.prefeitura.sp.gov.br',
	'murillopsilva.26022015@edu.sme.prefeitura.sp.gov.br',
	'murillopsilva.26022015@edu.sme.prefeitura.sp.gov.br',
	'murillopsilva.26022015@edu.sme.prefeitura.sp.gov.br',
	'murillopsilva.26022015@edu.sme.prefeitura.sp.gov.br',
	'murillopsilva.26022015@edu.sme.prefeitura.sp.gov.br',
	'murillopsilva.26022015@edu.sme.prefeitura.sp.gov.br',
	'murilomcsousa.16062016@edu.sme.prefeitura.sp.gov.br',
	'naiellycsfernandes.31122006@edu.sme.prefeitura.sp.gov.br',
	'naiellycsfernandes.31122006@edu.sme.prefeitura.sp.gov.br',
	'naiellycsfernandes.31122006@edu.sme.prefeitura.sp.gov.br',
	'naiellycsfernandes.31122006@edu.sme.prefeitura.sp.gov.br',
	'naiellycsfernandes.31122006@edu.sme.prefeitura.sp.gov.br',
	'naiellycsfernandes.31122006@edu.sme.prefeitura.sp.gov.br',
	'naiellycsfernandes.31122006@edu.sme.prefeitura.sp.gov.br',
	'naiellycsfernandes.31122006@edu.sme.prefeitura.sp.gov.br',
	'naiellycsfernandes.31122006@edu.sme.prefeitura.sp.gov.br',
	'naiellycsfernandes.31122006@edu.sme.prefeitura.sp.gov.br',
	'valentinalbrito.07122016@edu.sme.prefeitura.sp.gov.br',
	'vallentinallsantos.01072016@edu.sme.prefeitura.sp.gov.br',
	'vitoriabdamasceno.13012015@edu.sme.prefeitura.sp.gov.br',
	'vitoriabdamasceno.13012015@edu.sme.prefeitura.sp.gov.br',
	'vitoriabdamasceno.13012015@edu.sme.prefeitura.sp.gov.br',
	'vitoriabdamasceno.13012015@edu.sme.prefeitura.sp.gov.br',
	'vitoriabdamasceno.13012015@edu.sme.prefeitura.sp.gov.br',
	'vitoriabdamasceno.13012015@edu.sme.prefeitura.sp.gov.br',
	'vitoriatcardoso.01092014@edu.sme.prefeitura.sp.gov.br',
	'vitoriatcardoso.01092014@edu.sme.prefeitura.sp.gov.br',
	'vitoriatcardoso.01092014@edu.sme.prefeitura.sp.gov.br',
	'vitoriatcardoso.01092014@edu.sme.prefeitura.sp.gov.br',
	'vitoriatcardoso.01092014@edu.sme.prefeitura.sp.gov.br',
	'vitoriatcardoso.01092014@edu.sme.prefeitura.sp.gov.br',
	'vivianbarbosa.19082016@edu.sme.prefeitura.sp.gov.br',
	'williamlpsilveira.28012015@edu.sme.prefeitura.sp.gov.br',
	'williamlpsilveira.28012015@edu.sme.prefeitura.sp.gov.br',
	'williamlpsilveira.28012015@edu.sme.prefeitura.sp.gov.br',
	'williamlpsilveira.28012015@edu.sme.prefeitura.sp.gov.br',
	'williamlpsilveira.28012015@edu.sme.prefeitura.sp.gov.br',
	'williamlpsilveira.28012015@edu.sme.prefeitura.sp.gov.br',
	'wyllhyannwssouza.17062016@edu.sme.prefeitura.sp.gov.br'
);

-- Totalização
SELECT
	[dbo].[proc_retorna_primeiro_nome](aluno.nm_aluno) AS 'First Name [Required]',
	[dbo].[proc_retorna_ultimo_nome](aluno.nm_aluno) AS 'Last Name [Required]',
	temp.nm_email AS 'Email Address [Required]',
	'****' AS 'Password [Required]',
	'' AS 'Password Hash Function [UPLOAD ONLY]',
	temp.nm_organizacao AS 'Org Unit Path [Required]',
	'' AS 'New Primary Email [UPLOAD ONLY]',
	'' AS 'Recovery Email',
	'' AS 'Home Secondary Email',
	'' AS 'Work Secondary Email',
	'' AS 'Recovery Phone [MUST BE IN THE E.164 FORMAT]',
	'' AS 'Work Phone',
	'' AS 'Home Phone',
	'' AS 'Mobile Phone',
	'' AS 'Work Address',
	'' AS 'Home Address',
	'' AS 'Employee ID',
	'' AS 'Employee Type',
	'' AS 'Employee Title',
	'' AS 'Manager Email',
	'' AS 'Department',
	'' AS 'Cost Center',
	'' AS 'Building ID',
	'' AS 'Floor Name',
	'' AS 'Floor Section',
	'False' AS 'Change Password at Next Sign-In',
	CASE WHEN temp.in_ativo = 'True' THEN 'ACTIVE' ELSE 'SUSPENDED' END AS 'New Status [UPLOAD ONLY]'
FROM
	#tempAlunosQueForamSuspensosIndevidamente temp
INNER JOIN
	v_aluno_cotic aluno (NOLOCK)
	ON temp.cd_aluno_eol = aluno.cd_aluno
ORDER BY
	cd_aluno_eol;
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- Alunos x Curso
DECLARE @anoLetivo AS INT = 2021;
DECLARE @situacaoAtivo AS CHAR = 1;
DECLARE @situacaoPendenteRematricula AS CHAR = 6;
DECLARE @situacaoRematriculado AS CHAR = 10;
DECLARE @situacaoSemContinuidade AS CHAR = 13;

DECLARE @situacaoAtivoInt AS INT = 1;
DECLARE @situacaoPendenteRematriculaInt AS INT = 6;
DECLARE @situacaoRematriculadoInt AS INT = 10;
DECLARE @situacaoSemContinuidadeInt AS INT = 13;

IF OBJECT_ID('tempdb..#tempAlunosMatriculaExportacaoAtivos') IS NOT NULL
	DROP TABLE #tempAlunosMatriculaExportacaoAtivos;
SELECT
	DISTINCT
	aluno.cd_aluno_eol,
	te.cd_turma_escola
INTO #tempAlunosMatriculaExportacaoAtivos
FROM
	#tempAlunosQueForamSuspensosIndevidamente aluno (NOLOCK)
INNER JOIN 
	v_matricula_cotic matr (NOLOCK) 
	ON aluno.cd_aluno_eol = matr.cd_aluno
INNER JOIN 
	matricula_turma_escola mte (NOLOCK) 
	ON matr.cd_matricula = mte.cd_matricula
INNER JOIN
	turma_escola te (NOLOCK)
	ON te.cd_turma_escola = mte.cd_turma_escola
INNER JOIN 
	escola esc 
	ON te.cd_escola = esc.cd_escola
INNER JOIN
	v_cadastro_unidade_educacao ue 
	on ue.cd_unidade_educacao = esc.cd_escola
INNER JOIN 
	unidade_administrativa dre 
	on ue.cd_unidade_administrativa_referencia = dre.cd_unidade_administrativa
WHERE
	matr.st_matricula IN (@situacaoAtivo, @situacaoPendenteRematricula, @situacaoRematriculado, @situacaoSemContinuidade)
	AND mte.cd_situacao_aluno IN (@situacaoAtivoInt, @situacaoPendenteRematriculaInt, @situacaoRematriculadoInt, @situacaoSemContinuidadeInt)
	AND matr.an_letivo = @anoLetivo;

-- 3. Criar tabela de relacionamento
IF OBJECT_ID('tempdb..#tempRelacaoTurmaComponenteCurricular') IS NOT NULL
	DROP TABLE #tempRelacaoTurmaComponenteCurricular;
CREATE TABLE #tempRelacaoTurmaComponenteCurricular
(
	cd_turma_escola INT NOT NULL,
	cd_componente_curricular INT NOT NULL
);

--- 3.1. Pegar todas as turmas distintas
IF OBJECT_ID('tempdb..#tempTurmasAlunosMatriculaExportacaoAtivos') IS NOT NULL
	DROP TABLE #tempTurmasAlunosMatriculaExportacaoAtivos;
SELECT
	DISTINCT
	temp.cd_turma_escola
INTO #tempTurmasAlunosMatriculaExportacaoAtivos
FROM
	#tempAlunosMatriculaExportacaoAtivos temp;

--- 3.2. Busca relação entre turmas e componentes curriculares
IF OBJECT_ID('tempdb..#tempComponentesCurricularesPorTurmas') IS NOT NULL
	DROP TABLE #tempComponentesCurricularesPorTurmas;
select distinct 
	CASE
			WHEN etapa_ensino.cd_etapa_ensino IN( 1 )
		THEN 'REGÊNCIA DE CLASSE INFANTIL'
	ELSE
		iif(tgt.cd_serie_grade is not null, concat(LTRIM(RTRIM(ter.dc_territorio_saber)), ' - ',  LTRIM(RTRIM(tep.dc_experiencia_pedagogica))), 
		coalesce(LTRIM(RTRIM(pcc.dc_componente_curricular)),   LTRIM(RTRIM(cc.dc_componente_curricular))))
	END Nome,
	CONCAT(CASE
			WHEN etapa_ensino.cd_etapa_ensino IN( 2, 3, 7, 11 )
		THEN 
		--eja  
		'EJA'
			WHEN etapa_ensino.cd_etapa_ensino IN ( 4, 5, 12, 13 )
		THEN 
		--fundamental     
		'EF'
			WHEN etapa_ensino.cd_etapa_ensino IN
				( 6, 7, 8, 9, 17, 14 ) THEN 
		--médio  
		'EM' 
		WHEN etapa_ensino.cd_etapa_ensino IN ( 1 )
			THEN 
		--infantil
		'EI'
		ELSE 'P'
		END, ' - ',  te.dc_turma_escola, ' - ', 
		te.cd_turma_escola, ue.cd_unidade_educacao, ' - ', LTRIM(RTRIM(tpe.sg_tp_escola)), ' ', ue.nm_unidade_educacao) Secao,
	CASE
			WHEN etapa_ensino.cd_etapa_ensino IN( 1 )
		THEN 512
	ELSE
		MIN(iif(pcc.cd_componente_curricular is not null, pcc.cd_componente_curricular,  cc.cd_componente_curricular))
	END ComponenteCurricularId,
	te.cd_turma_escola TurmaId
INTO #tempComponentesCurricularesPorTurmas
from turma_escola te
inner join escola esc ON te.cd_escola = esc.cd_escola
inner join v_cadastro_unidade_educacao ue on ue.cd_unidade_educacao = esc.cd_escola
inner join tipo_escola tpe on esc.tp_escola = tpe.tp_escola
inner join unidade_administrativa dre on dre.tp_unidade_administrativa = 24
and ue.cd_unidade_administrativa_referencia = dre.cd_unidade_administrativa
--Serie Ensino
left join serie_turma_escola ON serie_turma_escola.cd_turma_escola = te.cd_turma_escola
left join serie_turma_grade ON serie_turma_grade.cd_turma_escola = serie_turma_escola.cd_turma_escola and serie_turma_grade.dt_fim is null
left join escola_grade ON serie_turma_grade.cd_escola_grade = escola_grade.cd_escola_grade
left join grade ON escola_grade.cd_grade = grade.cd_grade
left join grade_componente_curricular gcc on gcc.cd_grade = grade.cd_grade
left join componente_curricular cc on cc.cd_componente_curricular = gcc.cd_componente_curricular
and cc.dt_cancelamento is null
left join turma_grade_territorio_experiencia tgt on tgt.cd_serie_grade = serie_turma_grade.cd_serie_grade and tgt.cd_componente_curricular = cc.cd_componente_curricular
left join territorio_saber ter on ter.cd_territorio_saber = tgt.cd_territorio_saber 
left join tipo_experiencia_pedagogica tep on tep.cd_experiencia_pedagogica = tgt.cd_experiencia_pedagogica
left join serie_ensino
    ON grade.cd_serie_ensino = serie_ensino.cd_serie_ensino
LEFT JOIN etapa_ensino
ON serie_ensino.cd_etapa_ensino = etapa_ensino.cd_etapa_ensino
-- Programa
left join tipo_programa tp on te.cd_tipo_programa = tp.cd_tipo_programa
left join turma_escola_grade_programa tegp on tegp.cd_turma_escola = te.cd_turma_escola
left join escola_grade teg on teg.cd_escola_grade = tegp.cd_escola_grade
left join grade pg on pg.cd_grade = teg.cd_grade
left join grade_componente_curricular pgcc on pgcc.cd_grade = teg.cd_grade
left join componente_curricular pcc on pgcc.cd_componente_curricular = pcc.cd_componente_curricular
and pcc.dt_cancelamento is null
-- Atribuicao turma regular
left join atribuicao_aula atb_ser
	on gcc.cd_grade = atb_ser.cd_grade and
		gcc.cd_componente_curricular = atb_ser.cd_componente_curricular
		and atb_ser.cd_serie_grade = serie_turma_grade.cd_serie_grade and atb_ser.dt_cancelamento is null
		and (atb_ser.dt_disponibilizacao_aulas is null)
		and atb_ser.an_atribuicao = year(getdate())
-- Atribuicao turma programa
left join atribuicao_aula atb_pro
	on pgcc.cd_grade = atb_pro.cd_grade and
		pgcc.cd_componente_curricular = atb_pro.cd_componente_curricular and
		atb_pro.cd_turma_escola_grade_programa = tegp.cd_turma_escola_grade_programa and
		atb_pro.dt_cancelamento is null
		and atb_pro.dt_disponibilizacao_aulas is null
		and atb_pro.an_atribuicao = year(getdate())
-- Servidor
left join v_cargo_base_cotic vcbc on (atb_ser.cd_cargo_base_servidor = vcbc.cd_cargo_base_servidor or
								atb_pro.cd_cargo_base_servidor = vcbc.cd_cargo_base_servidor)
								and vcbc.dt_cancelamento is null and vcbc.dt_fim_nomeacao is null
left join v_servidor_cotic serv on serv.cd_servidor = vcbc.cd_servidor
-- Turno     
inner join duracao_tipo_turno dtt on te.cd_tipo_turno = dtt.cd_tipo_turno and te.cd_duracao = dtt.cd_duracao
where  
	te.st_turma_escola in ('O', 'A', 'C')
	and   te.cd_tipo_turma in (1,2,3,5,6)
	and   esc.tp_escola in (1,2,3,4,10,13,16,17,18,19,23,25,28,31)
	and    NOT esc.cd_escola IN ('200242', '019673')
	and   te.an_letivo = 2021
group by 
	ue.cd_unidade_educacao,
	ue.nm_unidade_educacao,
	tgt.cd_serie_grade, 
CASE
	WHEN etapa_ensino.cd_etapa_ensino IN( 1 ) THEN 'REGÊNCIA DE CLASSE INFANTIL'
ELSE
	iif(tgt.cd_serie_grade is not null, concat(LTRIM(RTRIM(ter.dc_territorio_saber)), ' - ',  LTRIM(RTRIM(tep.dc_experiencia_pedagogica))), 
	coalesce( LTRIM(RTRIM(pcc.dc_componente_curricular)),   LTRIM(RTRIM(cc.dc_componente_curricular))))
END,
te.cd_turma_escola, 
te.dc_turma_escola,
tpe.sg_tp_escola,
etapa_ensino.cd_etapa_ensino;

--- 3.3. Insere os componentes curriculares das turmas de alunos ativos
INSERT INTO #tempRelacaoTurmaComponenteCurricular (cd_turma_escola, cd_componente_curricular)
SELECT
	t1.cd_turma_escola,
	t2.ComponenteCurricularId AS cd_componente_curricular
FROM
	#tempTurmasAlunosMatriculaExportacaoAtivos t1
INNER JOIN
	#tempComponentesCurricularesPorTurmas t2
	ON t1.cd_turma_escola = t2.TurmaId;

-- 4. Final
IF OBJECT_ID('tempdb..#tempAlunosCurso') IS NOT NULL
	DROP TABLE #tempAlunosCurso;
SELECT
	DISTINCT
	t1.cd_aluno_eol,
	t1.nm_email,
	t2.cd_turma_escola,
	t3.cd_componente_curricular
INTO #tempAlunosCurso
FROM
	#tempAlunosQueForamSuspensosIndevidamente t1
INNER JOIN
	#tempAlunosMatriculaExportacaoAtivos t2
	ON t1.cd_aluno_eol = t2.cd_aluno_eol
INNER JOIN
	#tempRelacaoTurmaComponenteCurricular t3
	ON t2.cd_turma_escola = t3.cd_turma_escola
ORDER BY
	nm_email;

-- 5. CSV
SELECT
	temp.cd_aluno_eol AS AlunoEol,
	temp.nm_email AS Email,
	temp.cd_turma_escola AS TurmaId,
	temp.cd_componente_curricular AS ComponenteCurricularId,
	curso.cd_curso_classroom AS CursoId
FROM
	#tempAlunosCurso temp
INNER JOIN
	turma_componente_curricular_classroom curso
	ON temp.cd_turma_escola = curso.cd_turma_escola AND temp.cd_componente_curricular = curso.cd_componente_curricular
ORDER BY 
	temp.cd_aluno_eol;
----------------
SELECT
	DISTINCT
	temp.cd_turma_escola--, temp.cd_componente_curricular
FROM
	#tempAlunosCurso temp
LEFT JOIN
	turma_componente_curricular_classroom curso
	ON temp.cd_turma_escola = curso.cd_turma_escola AND temp.cd_componente_curricular = curso.cd_componente_curricular
WHERE
	curso.cd_componente_curricular IS NULL