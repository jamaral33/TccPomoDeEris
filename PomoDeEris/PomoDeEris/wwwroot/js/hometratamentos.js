// Dados dos tratamentos exibidos na home.
// Os caminhos apontam para arquivos publicados em wwwroot/images.
const TREATMENTS = [
  {
    id: 'massagem-relaxante',
    name: 'Pacote Massagem Relaxante',
    description: 'Higienização completa com extração e máscara calmante para uma pele renovada.',
    duration: 60,
    price: 280,
    oldPrice: 350,
    tags: ['relaxamento'],
    badge: 'Inverno −20%',
    image: '/images/massagem-relaxante.png'
  },
  {
    id: 'pacote-manicure',
    name: 'Procedimento, Pacote manicure',
    description: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam suscipit sollicitudin cursus.',
    duration: 30,
    price: 890,
    oldPrice: null,
    tags: ['corporal'],
    badge: 'Mais procurado',
    image: '/images/manicure.png'
  },
  {
    id: 'pelos-faciais',
    name: 'Procedimentos de pelos faciais',
    description: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam suscipit sollicitudin cursus.',
    duration: 40,
    price: 1200,
    oldPrice: null,
    tags: ['facial'],
    badge: 'Facial',
    image: '/images/pelosFaciais.png'
  },
  {
    id: 'bronzeamento',
    name: 'Pacote Bronzeamento',
    description: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam suscipit sollicitudin cursus.',
    duration: 50,
    price: 320,
    oldPrice: 400,
    tags: ['corporal'],
    badge: 'Inverno −20%',
    image: '/images/bronzeamento.png'
  },
  {
    id: 'micropigmentacao',
    name: 'Pacote Micropigmentação',
    description: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam suscipit sollicitudin cursus.',
    duration: 50,
    price: 180,
    oldPrice: null,
    tags: ['facial'],
    badge: null,
    image: '/images/pigmentacao.png'
  },
  {
    id: 'relaxamento-hidro',
    name: 'Pacote Relaxamento Hidro',
    description: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam suscipit sollicitudin cursus.',
    duration: 20,
    price: 180,
    oldPrice: null,
    tags: ['relaxamento'],
    badge: 'Relaxamento',
    image: '/images/hidroRelaxamento.png'
  }
];
