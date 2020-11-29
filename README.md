# Bravely_Default_Index_Injector

## Conceito do projeto
### A aplicação permite criar um arquivo binário Mestre chamado Crowd.fs e um arquivo Index.fs para o jogo Bravely Default de Nintendo 3DS.
O arquivo Crowd.fs possui estrutura lógica similar a um arquivo em .zip, ou seja, este formato é uma compactação proprietária de arquivos menores feita pela empresa Square Enix para este jogo em específico, os sub-arquivos devem ser indicados pelo usuário em um diretório escolhido para que sejam compactados.<br/>
Além disso,o projeto também permite editar o arquivo Index.fs feito pelos desenvolvedores, este arquivo contêm informações sensíveis ao Crowd.fs,tais como o tamanho dos arquivos compactados e a posição de onde se iniciam dentro do Crowd.fs, dessa forma, esta função pode modificar estas duas sequências de bytes para que seja possível carregar os arquivos traduzidos de forma correta dentro do jogo.

## Pré-requisitos e recursos utilizados
O Programa feito em C# usando as Funções nativas da linguagem conhecidas como BinaryReader/BinaryWriter para o jogo Bravely Default de Nintendo 3DS. Além disso, foi utilizado a Biblioteca System.Threading para inserir comandos de Delay no console de Debug, ficando mais simples de verificar as operações feitas e as bibliotecas padrões do Windows Forms, permitindo fazer uma interface para o projeto.<br>
Para Debug do programa, foi criado a exibição de um Console para que o usuário possa verificar o que está ocorrendo com o programa em tempo real, sendo mais simples de localizar erros em arquivos, caso haja.
  
## Passo a passo

1. Extraí a RomFS do jogo Brvaely Default e descompactei os arquivos Crowd.fs e Index.fs originais para que fosse gerado os sub-arquivos do projeto
2. Estuei a estrutura de ponteiros e textos dos arquivos do jogo para realizar a engenharia reversa nos arquivos Mestre
3. Implementei uma função com o objetivo de compactar os sub-arquivos e gerar um arquivo chamado Crowd.fs, utilizando da estrutura estudada
4. Implementei uma função com o objeto de gerar o arquivo Index.fs com as sequências de bytes modificadas referentes ao tamanho dos arquivos traduzidos e da posição de onde se iniciam na compactação do novo Crowd.fs 
4. Implementei uma interface gráfica para utilizar o programa de comunicação de forma mais intuitiva, adicionando um menu de Ajuda com tutoriais para utilizar o programa e abas de referência ao repositório do código e meu perfil no Github.

## Imagens/screenshots

![Imagem](https://github.com/MrVtR/Bravely_Default_Index_Injector/blob/master/Imagens/Interface.PNG)

---
![Imagem](https://github.com/MrVtR/Bravely_Default_Index_Injector/blob/master/Imagens/CrowdFunction.PNG)

---
![Imagem](https://github.com/MrVtR/Bravely_Default_Index_Injector/blob/master/Imagens/IndexFunction.PNG)
