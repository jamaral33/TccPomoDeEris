const botaoPix = document.getElementById("botaoPix");
const botaoCredito = document.getElementById("botaoCredito");
const botaoDebito = document.getElementById("botaoDebito");

const painelPix = document.getElementById("painelPix");
const painelCartao = document.getElementById("painelCartao");
const painelDebito = document.getElementById("painelDebito");

botaoPix.addEventListener("click", () => {
    botaoPix.classList.add("ativo");
    botaoCredito.classList.remove("ativo");
    botaoDebito.classList.remove("ativo");

    painelPix.classList.remove("oculto");
    painelCartao.classList.add("oculto");
    painelDebito.classList.add("oculto");
});

botaoCredito.addEventListener("click", () => {
    botaoPix.classList.remove("ativo");
    botaoCredito.classList.add("ativo");
    botaoDebito.classList.remove("ativo");

    painelPix.classList.add("oculto");
    painelCartao.classList.remove("oculto");
    painelDebito.classList.add("oculto");
});

botaoDebito.addEventListener("click", () => {
    botaoPix.classList.remove("ativo");
    botaoCredito.classList.remove("ativo");
    botaoDebito.classList.add("ativo");

    painelPix.classList.add("oculto");
    painelCartao.classList.add("oculto");
    painelDebito.classList.remove("oculto");
});