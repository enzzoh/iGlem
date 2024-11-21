// SUPORTE // 
const faqs = [
    {
        title: 'Como reiniciar o modem?',
        content: 'Para reiniciar o modem, desligue-o da tomada por 30 segundos e, em seguida, reconecte-o. Aguarde até que todas as luzes estabilizem.'
    },
    {
        title: 'O que fazer se a conexão estiver lenta?',
        content: 'Caso sua conexão esteja lenta, tente reiniciar o roteador e verifique se não há dispositivos sobrecarregando a rede. Caso o problema persista, entre em contato com o suporte técnico.'
    },
    {
        title: 'Como alterar a senha do Wi-Fi?',
        content: 'Para alterar a senha do Wi-Fi, acesse as configurações do seu roteador através do IP indicado no manual do aparelho. Lá, procure a seção de segurança e altere a senha.'
    },
    {
        title: 'Como reiniciar o modem?',
        content: 'Para reiniciar o modem, desligue-o da tomada por 30 segundos e, em seguida, reconecte-o. Aguarde até que todas as luzes estabilizem.'
    },
    {
        title: 'O que fazer se a conexão estiver lenta?',
        content: 'Caso sua conexão esteja lenta, tente reiniciar o roteador e verifique se não há dispositivos sobrecarregando a rede. Caso o problema persista, entre em contato com o suporte técnico.'
    },
    {
        title: 'Como reiniciar o modem?',
        content: 'Para reiniciar o modem, desligue-o da tomada por 30 segundos e, em seguida, reconecte-o. Aguarde até que todas as luzes estabilizem.'
    },
    {
        title: 'O que fazer se a conexão estiver lenta?',
        content: 'Caso sua conexão esteja lenta, tente reiniciar o roteador e verifique se não há dispositivos sobrecarregando a rede. Caso o problema persista, entre em contato com o suporte técnico.'
    },
    {
        title: 'Como reiniciar o modem?',
        content: 'Para reiniciar o modem, desligue-o da tomada por 30 segundos e, em seguida, reconecte-o. Aguarde até que todas as luzes estabilizem.'
    },

];

const faqContainer = document.getElementById('faq-container');
const noResults = document.getElementById('no-results');

function renderFAQs(faqsToRender) {
    faqContainer.innerHTML = '';
    noResults.style.display = faqsToRender.length === 0 ? 'block' : 'none';

    faqsToRender.forEach(faq => {
        const faqItem = document.createElement('div');
        faqItem.classList.add('faq-item');

        const faqTitle = document.createElement('h2');
        faqTitle.textContent = faq.title;
        faqItem.appendChild(faqTitle);

        const faqContent = document.createElement('p');
        faqContent.textContent = faq.content;
        faqItem.appendChild(faqContent);

        faqTitle.addEventListener('click', () => {
            faqItem.classList.toggle('active');
        });

        faqContainer.appendChild(faqItem);
    });
}

function searchFAQs() {
    const searchTerm = document.getElementById('search').value.toLowerCase();
    const filteredFAQs = faqs.filter(faq =>
        faq.title.toLowerCase().includes(searchTerm) ||
        faq.content.toLowerCase().includes(searchTerm)
    );
    renderFAQs(filteredFAQs);
}

document.getElementById('search').addEventListener('input', searchFAQs);

renderFAQs(faqs);

