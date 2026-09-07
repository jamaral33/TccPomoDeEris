(function(){

/* ============ DATA ============ */
var MONTHS=['janeiro','fevereiro','março','abril','maio','junho','julho','agosto','setembro','outubro','novembro','dezembro'];
var MONTHS_SHORT=['Jan','Fev','Mar','Abr','Mai','Jun','Jul','Ago','Set','Out','Nov','Dez'];
var DAYS_SHORT=['Dom','Seg','Ter','Qua','Qui','Sex','Sáb'];

var apptData=[
  {id:1,time:'09:00',name:'Camila Rocha',phone:'(11)99100-1234',email:'camila@email.com',treatment:'Toxina Botulínica',pro:'Dra. Beatriz Lima',status:'confirmed',price:890,note:'Preferência por resultado natural'},
  {id:2,time:'09:30',name:'Juliana Pires',phone:'(11)98200-5678',email:'juliana@email.com',treatment:'Limpeza de Pele',pro:'Dra. Fernanda Costa',status:'confirmed',price:280,note:''},
  {id:3,time:'10:30',name:'Mariana Alves',phone:'(11)97300-9012',email:'mariana@email.com',treatment:'Peeling de Diamante',pro:'Dra. Beatriz Lima',status:'pending',price:320,note:''},
  {id:4,time:'11:00',name:'Sofia Tavares',phone:'(11)96400-3456',email:'sofia@email.com',treatment:'Drenagem Linfática',pro:'Dr. Rafael Souza',status:'confirmed',price:250,note:'Pós-operatório'},
  {id:5,time:'14:00',name:'Renata Melo',phone:'(11)95500-7890',email:'renata@email.com',treatment:'Preenchimento Labial',pro:'Dra. Beatriz Lima',status:'pending',price:1200,note:''},
  {id:6,time:'15:30',name:'Ana Carvalho',phone:'(11)94600-2345',email:'ana@email.com',treatment:'Skinbooster',pro:'Dra. Fernanda Costa',status:'pending',price:1500,note:''},
  {id:7,time:'16:00',name:'Luciana Ferreira',phone:'(11)93700-6789',email:'luciana@email.com',treatment:'Massagem Modeladora',pro:'Dr. Rafael Souza',status:'cancelled',price:220,note:'Reagendar'}
];

var clientData=[
  {id:1,name:'Ana Carvalho',email:'ana@email.com',phone:'(11)94600-2345',since:'março de 2023',total:9200,sessions:7,status:'confirmed',history:[
    {date:'07/09/2026',treatment:'Skinbooster Bioestimulador',pro:'Dra. Fernanda Costa',price:1500,note:'Resultado excelente. Reaplicar em 6 meses.'},
    {date:'12/06/2026',treatment:'Toxina Botulínica',pro:'Dra. Beatriz Lima',price:890,note:''},
    {date:'02/03/2026',treatment:'Peeling de Diamante',pro:'Dra. Beatriz Lima',price:320,note:'Pele sensível, usar protocolo suave.'}
  ]},
  {id:2,name:'Camila Rocha',email:'camila@email.com',phone:'(11)99100-1234',since:'janeiro de 2022',total:14800,sessions:18,status:'confirmed',history:[
    {date:'07/09/2026',treatment:'Toxina Botulínica',pro:'Dra. Beatriz Lima',price:890,note:'Preferência por resultado natural.'},
    {date:'10/06/2026',treatment:'Preenchimento Labial',pro:'Dra. Beatriz Lima',price:1200,note:''},
    {date:'15/03/2026',treatment:'Toxina Botulínica',pro:'Dra. Beatriz Lima',price:890,note:''}
  ]},
  {id:3,name:'Juliana Pires',email:'juliana@email.com',phone:'(11)98200-5678',since:'agosto de 2024',total:3480,sessions:5,status:'confirmed',history:[
    {date:'07/09/2026',treatment:'Limpeza de Pele Profunda',pro:'Dra. Fernanda Costa',price:280,note:''},
    {date:'15/07/2026',treatment:'Drenagem Linfática',pro:'Dr. Rafael Souza',price:250,note:''},
    {date:'20/05/2026',treatment:'Massagem Modeladora',pro:'Dr. Rafael Souza',price:220,note:''}
  ]},
  {id:4,name:'Luciana Ferreira',email:'luciana@email.com',phone:'(11)93700-6789',since:'novembro de 2023',total:5600,sessions:8,status:'cancelled',history:[
    {date:'07/09/2026',treatment:'Massagem Modeladora',pro:'Dr. Rafael Souza',price:220,note:'Cancelado — reagendar.'},
    {date:'01/07/2026',treatment:'Drenagem Linfática',pro:'Dr. Rafael Souza',price:250,note:''}
  ]},
  {id:5,name:'Mariana Alves',email:'mariana@email.com',phone:'(11)97300-9012',since:'fevereiro de 2025',total:2890,sessions:4,status:'pending',history:[
    {date:'07/09/2026',treatment:'Peeling de Diamante',pro:'Dra. Beatriz Lima',price:320,note:''},
    {date:'10/05/2026',treatment:'Limpeza de Pele Profunda',pro:'Dra. Fernanda Costa',price:280,note:''}
  ]},
  {id:6,name:'Renata Melo',email:'renata@email.com',phone:'(11)95500-7890',since:'setembro de 2026',total:1200,sessions:1,status:'pending',history:[
    {date:'07/09/2026',treatment:'Preenchimento Labial',pro:'Dra. Beatriz Lima',price:1200,note:'Primeira visita.'}
  ]},
  {id:7,name:'Sofia Tavares',email:'sofia@email.com',phone:'(11)96400-3456',since:'junho de 2023',total:7850,sessions:10,status:'confirmed',history:[
    {date:'07/09/2026',treatment:'Drenagem Linfática',pro:'Dr. Rafael Souza',price:250,note:'Pós-operatório.'},
    {date:'03/08/2026',treatment:'Skinbooster Bioestimulador',pro:'Dra. Fernanda Costa',price:1500,note:''},
    {date:'15/06/2026',treatment:'Toxina Botulínica',pro:'Dra. Beatriz Lima',price:890,note:''}
  ]}
];

/* ============ TOAST ============ */
function showToast(msg){
  var t=document.getElementById('toast');
  document.getElementById('toastMsg').textContent=msg;
  t.classList.add('show');
  clearTimeout(t._tm);
  t._tm=setTimeout(function(){t.classList.remove('show');},3000);
}

/* ============ DATE ============ */
var today=new Date(); today.setHours(0,0,0,0);
var dow=DAYS_SHORT[today.getDay()];
document.getElementById('tbDate').textContent=dow+', '+today.getDate()+' de '+MONTHS[today.getMonth()]+' de '+today.getFullYear();

/* ============ NAV ============ */
var titleMap={dashboard:'Dashboard',packages:'Adicionar pacote',schedule:'Cronogramas',clients:'Histórico de clientes',settings:'Configurações'};
function nav(page){
  document.querySelectorAll('.sb-item').forEach(function(i){i.classList.remove('active');});
  var item=document.querySelector('.sb-item[data-page="'+page+'"]');
  if(item) item.classList.add('active');
  document.querySelectorAll('.page').forEach(function(p){p.classList.remove('active');});
  var pg=document.getElementById('page-'+page);
  if(pg) pg.classList.add('active');
  document.getElementById('topbarTitle').textContent=titleMap[page]||page;
  window.scrollTo({top:0,behavior:'smooth'});
}
document.querySelectorAll('[data-page]').forEach(function(el){
  el.addEventListener('click',function(){nav(el.getAttribute('data-page'));});
});

/* ============ MINI CALENDAR (dashboard) ============ */
var dcalYear=today.getFullYear(), dcalMonth=today.getMonth();
var apptDays=[3,7,10,12,14,17,19,21,24]; // mock days with appts
function renderDcal(){
  document.getElementById('dcalMonth').textContent=MONTHS[dcalMonth]+' '+dcalYear;
  var grid=document.getElementById('dcalDays');
  grid.innerHTML='';
  var first=new Date(dcalYear,dcalMonth,1).getDay();
  var dcount=new Date(dcalYear,dcalMonth+1,0).getDate();
  for(var p=0;p<first;p++){var d=document.createElement('button');d.className='cal-d pad';grid.appendChild(d);}
  for(var dd=1;dd<=dcount;dd++){
    (function(day){
      var btn=document.createElement('button');btn.className='cal-d';btn.textContent=day;
      var dt=new Date(dcalYear,dcalMonth,day);
      if(dt.getTime()===today.getTime()) btn.classList.add('today');
      if(apptDays.indexOf(day)!==-1) btn.classList.add('has-appt');
      grid.appendChild(btn);
    })(dd);
  }
}
document.getElementById('dcalPrev').addEventListener('click',function(){dcalMonth--;if(dcalMonth<0){dcalMonth=11;dcalYear--;}renderDcal();});
document.getElementById('dcalNext').addEventListener('click',function(){dcalMonth++;if(dcalMonth>11){dcalMonth=0;dcalYear++;}renderDcal();});
renderDcal();

/* ============ FULL CALENDAR (schedule) ============ */
var fcalYear=today.getFullYear(), fcalMonth=today.getMonth(), fcalSelected=today.getDate();
function renderFcal(){
  document.getElementById('fcalMonth').textContent=MONTHS[fcalMonth]+' '+fcalYear;
  var grid=document.getElementById('fcalDays');
  grid.innerHTML='';
  var first=new Date(fcalYear,fcalMonth,1).getDay();
  var dcount=new Date(fcalYear,fcalMonth+1,0).getDate();
  for(var p=0;p<first;p++){var d=document.createElement('button');d.className='cal-d pad';grid.appendChild(d);}
  for(var dd=1;dd<=dcount;dd++){
    (function(day){
      var btn=document.createElement('button');btn.className='cal-d';btn.textContent=day;
      var dt=new Date(fcalYear,fcalMonth,day);
      if(dt.getTime()===today.getTime()) btn.classList.add('today');
      if(apptDays.indexOf(day)!==-1) btn.classList.add('has-appt');
      if(fcalYear===today.getFullYear()&&fcalMonth===today.getMonth()&&day===fcalSelected) btn.classList.add('selected');
      btn.addEventListener('click',function(){
        fcalSelected=day;
        renderFcal();
        renderDayStrip();
        renderAppts();
      });
      grid.appendChild(btn);
    })(dd);
  }
}
document.getElementById('fcalPrev').addEventListener('click',function(){fcalMonth--;if(fcalMonth<0){fcalMonth=11;fcalYear--;}renderFcal();});
document.getElementById('fcalNext').addEventListener('click',function(){fcalMonth++;if(fcalMonth>11){fcalMonth=0;fcalYear++;}renderFcal();});

/* ============ DAY STRIP ============ */
function renderDayStrip(){
  var strip=document.getElementById('dayStrip');strip.innerHTML='';
  var base=new Date(today);
  base.setDate(today.getDate()-1);
  for(var i=0;i<8;i++){
    (function(offset){
      var d=new Date(today);d.setDate(today.getDate()+offset-1);
      var btn=document.createElement('button');btn.className='day-tab';
      var dots='';
      if(apptDays.indexOf(d.getDate())!==-1&&d.getMonth()===today.getMonth()){
        dots='<span style="display:flex;gap:2px;margin-top:3px;"><span class="dot-appt"></span></span>';
      }
      btn.innerHTML='<span class="dn">'+DAYS_SHORT[d.getDay()]+'</span><span class="dd">'+d.getDate()+'</span>'+dots;
      if(d.getDate()===fcalSelected&&d.getMonth()===fcalMonth) btn.classList.add('active');
      btn.addEventListener('click',function(){
        fcalSelected=d.getDate(); fcalMonth=d.getMonth(); fcalYear=d.getFullYear();
        renderFcal(); renderDayStrip(); renderAppts();
      });
      strip.appendChild(btn);
    })(i);
  }
}

/* ============ APPOINTMENTS ============ */
function statusLabel(s){return{confirmed:'Confirmado',pending:'Pendente',cancelled:'Cancelado',done:'Concluído'}[s]||s;}
function renderAppts(){
  var search=document.getElementById('schedSearch').value.trim().toLowerCase();
  var proF=document.getElementById('schedPro').value;
  var stF=document.getElementById('schedStatus').value;
  var list=document.getElementById('apptList');list.innerHTML='';
  var data=apptData.filter(function(a){
    if(search&&a.name.toLowerCase().indexOf(search)===-1&&a.treatment.toLowerCase().indexOf(search)===-1) return false;
    if(proF&&a.pro!==proF) return false;
    if(stF&&statusLabel(a.status)!==stF) return false;
    return true;
  });
  if(!data.length){
    list.innerHTML='<div class="empty"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5"><rect x="3" y="5" width="18" height="16" rx="2"/><path d="M16 3v4M8 3v4M3 9h18"/></svg><p>Nenhum agendamento encontrado.</p></div>';
    return;
  }
  data.forEach(function(a){
    var sc=a.status==='confirmed'?'confirmed':a.status==='pending'?'pending':a.status==='cancelled'?'cancelled':'done';
    var initials=a.pro.replace(/[^A-Z]/g,'').substring(0,2);
    var div=document.createElement('div');div.className='appt-card';
    div.innerHTML=
      '<div class="appt-time">'+a.time+'</div>'+
      '<div class="appt-sep"></div>'+
      '<div class="appt-info">'+
        '<h4>'+a.name+'</h4>'+
        '<span class="sub">'+a.treatment+' · R$ '+a.price.toLocaleString('pt-BR')+'</span>'+
        '<div class="appt-pro">'+
          '<div class="avatar-sm">'+initials+'</div>'+
          '<span class="pro-name">'+a.pro+'</span>'+
        '</div>'+
      '</div>'+
      '<span class="status-pill '+sc+'">'+statusLabel(a.status)+'</span>'+
      '<div class="appt-actions">'+
        (a.status==='pending'?'<button class="btn btn-ghost btn-sm confirm-btn" data-id="'+a.id+'">Confirmar</button>':'')+ 
        '<button class="btn-icon-only view-appt-btn" data-id="'+a.id+'" title="Ver detalhes">'+
          '<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.7"><path d="M2 12s3.6-7 10-7 10 7 10 7-3.6 7-10 7-10-7-10-7Z"/><circle cx="12" cy="12" r="3"/></svg>'+
        '</button>'+
      '</div>';
    list.appendChild(div);
  });
  list.querySelectorAll('.confirm-btn').forEach(function(btn){
    btn.addEventListener('click',function(){
      var id=parseInt(btn.getAttribute('data-id'),10);
      var a=apptData.find(function(x){return x.id===id;});
      if(a){a.status='confirmed'; document.getElementById('pendingBadge').textContent=apptData.filter(function(x){return x.status==='pending';}).length; renderAppts(); showToast('Agendamento de '+a.name+' confirmado.');}
    });
  });
}
['schedSearch','schedPro','schedStatus'].forEach(function(id){
  document.getElementById(id).addEventListener('input',renderAppts);
});
renderFcal();
renderDayStrip();
renderAppts();

/* ============ CLIENTS TABLE ============ */
function renderClients(filter){
  var search=(filter||'').toLowerCase();
  var tbody=document.getElementById('clientTbody');tbody.innerHTML='';
  clientData.filter(function(c){
    if(!search) return true;
    return c.name.toLowerCase().indexOf(search)!==-1||c.email.toLowerCase().indexOf(search)!==-1||c.phone.indexOf(search)!==-1;
  }).forEach(function(c){
    var sc=c.status==='confirmed'?'confirmed':c.status==='pending'?'pending':'cancelled';
    var initials=c.name.split(' ').map(function(w){return w[0];}).join('').substring(0,2);
    var lastAppt=c.history[0]?c.history[0].date:'—';
    var tr=document.createElement('tr');
    tr.innerHTML=
      '<td><div class="client-name-cell">'+
        '<div class="cli-avatar">'+initials+'</div>'+
        '<div><div class="cli-name">'+c.name+'</div><div class="cli-email">'+c.email+'</div></div>'+
      '</div></td>'+
      '<td>'+c.phone+'</td>'+
      '<td>'+lastAppt+'</td>'+
      '<td class="fw600 color-wine">R$ '+c.total.toLocaleString('pt-BR')+'</td>'+
      '<td>'+c.sessions+'</td>'+
      '<td><span class="status-pill '+sc+'">'+statusLabel(c.status)+'</span></td>'+
      '<td><button class="btn-icon-only open-modal-btn" data-id="'+c.id+'" title="Ver histórico">'+
        '<svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.7"><path d="M2 12s3.6-7 10-7 10 7 10 7-3.6 7-10 7-10-7-10-7Z"/><circle cx="12" cy="12" r="3"/></svg>'+
      '</button></td>';
    tbody.appendChild(tr);
    tr.addEventListener('click',function(e){if(!e.target.closest('.open-modal-btn')) openClientModal(c.id);});
    tr.querySelector('.open-modal-btn').addEventListener('click',function(e){e.stopPropagation(); openClientModal(c.id);});
  });
}
document.getElementById('clientSearch').addEventListener('input',function(){renderClients(this.value);});
renderClients();

/* ============ CLIENT MODAL ============ */
function openClientModal(id){
  var c=clientData.find(function(x){return x.id===id;});
  if(!c) return;
  document.getElementById('modalClientName').textContent=c.name;
  document.getElementById('mEmail').textContent=c.email;
  document.getElementById('mPhone').textContent=c.phone;
  document.getElementById('mSince').textContent='Cliente desde '+c.since;
  document.getElementById('mTotal').textContent='R$ '+c.total.toLocaleString('pt-BR')+' · '+c.sessions+' sessões';
  var tl=document.getElementById('mTimeline');tl.innerHTML='';
  c.history.forEach(function(h){
    var div=document.createElement('div');div.className='hist-entry';
    div.innerHTML='<h5>'+h.treatment+'</h5>'+
      '<div class="meta"><span>'+h.date+'</span><span>'+h.pro+'</span><span class="fw600 color-wine">R$ '+h.price.toLocaleString('pt-BR')+'</span></div>'+
      (h.note?'<p class="note">'+h.note+'</p>':'');
    tl.appendChild(div);
  });
  document.getElementById('clientModal').classList.add('open');
  document.body.style.overflow='hidden';
}
function closeModal(){
  document.getElementById('clientModal').classList.remove('open');
  document.body.style.overflow='';
}
document.getElementById('modalClose').addEventListener('click',closeModal);
document.getElementById('clientModal').addEventListener('click',function(e){if(e.target===this) closeModal();});

/* ============ PACKAGE FORM LIVE PREVIEW ============ */
function updatePreview(){
  var name=document.getElementById('pkgName').value.trim()||'Nome do pacote';
  var desc=document.getElementById('pkgDesc').value.trim()||'A descrição aparecerá aqui conforme você digita.';
  var cat=document.getElementById('pkgCat').value||'Categoria';
  var dur=document.getElementById('pkgDur').value||'— minutos';
  var price=document.getElementById('pkgPrice').value;
  var old=document.getElementById('pkgPriceOld').value;
  var promo=document.getElementById('pkgPromo').checked;

  document.getElementById('prevName').textContent=name;
  document.getElementById('prevDesc').textContent=desc.substring(0,120)+(desc.length>120?'…':'');
  document.getElementById('prevCat').textContent=cat;
  document.getElementById('prevDur').querySelector('svg').outerHTML;
  document.getElementById('prevDur').lastChild.textContent=' '+dur;

  var priceEl=document.getElementById('prevPrice');
  var strikeEl=document.getElementById('prevStrike');
  if(price){
    priceEl.textContent='R$ '+parseFloat(price).toLocaleString('pt-BR',{minimumFractionDigits:2,maximumFractionDigits:2});
    priceEl.className='preview-price'+(old?' promo':'');
    if(old){strikeEl.textContent='R$ '+parseFloat(old).toLocaleString('pt-BR',{minimumFractionDigits:2,maximumFractionDigits:2}); strikeEl.style.display='inline';}
    else{strikeEl.style.display='none';}
  }else{
    priceEl.textContent='R$ —';
    strikeEl.style.display='none';
  }

  var media=document.getElementById('previewMedia');
  var existingPromo=media.querySelector('.preview-promo');
  if(promo&&!existingPromo){
    var span=document.createElement('span');span.className='preview-promo';span.textContent='Inverno −20%';media.appendChild(span);
  } else if(!promo&&existingPromo){
    existingPromo.remove();
  }
}
['pkgName','pkgDesc','pkgCat','pkgDur','pkgPrice','pkgPriceOld','pkgPromo'].forEach(function(id){
  var el=document.getElementById(id);
  if(el) el.addEventListener('input',updatePreview);
  if(el&&el.type==='checkbox') el.addEventListener('change',updatePreview);
});
document.getElementById('pkgCat').addEventListener('change',updatePreview);
document.getElementById('pkgDur').addEventListener('change',updatePreview);
document.getElementById('pkgType').addEventListener('change',updatePreview);

document.getElementById('savePkgBtn').addEventListener('click',function(){
  var name=document.getElementById('pkgName').value.trim();
  var cat=document.getElementById('pkgCat').value;
  var price=document.getElementById('pkgPrice').value;
  if(!name||!cat||!price){showToast('Preencha nome, categoria e preço antes de publicar.'); return;}
  showToast('"'+name+'" publicado com sucesso no site!');
  document.getElementById('page-packages').querySelector('form') && document.getElementById('page-packages').querySelector('form').reset();
});
document.getElementById('saveDraftBtn').addEventListener('click',function(){
  showToast('Rascunho salvo. Você pode publicá-lo a qualquer momento.');
});
document.getElementById('resetFormBtn').addEventListener('click',function(){
  ['pkgName','pkgDesc','pkgPrice','pkgPriceOld'].forEach(function(id){document.getElementById(id).value='';});
  ['pkgCat','pkgDur','pkgType'].forEach(function(id){document.getElementById(id).selectedIndex=0;});
  ['pkgPublished','pkgPopular','pkgPromo'].forEach(function(id){
    var el=document.getElementById(id); el.checked=(id==='pkgPublished');
  });
  updatePreview();
});

/* ============ IMAGE UPLOAD ZONE ============ */
document.getElementById('pkgImage').addEventListener('change',function(e){
  var file=e.target.files[0]; if(!file) return;
  var reader=new FileReader();
  reader.onload=function(ev){
    var media=document.getElementById('previewMedia');
    media.style.backgroundImage='url('+ev.target.result+')';
    media.style.backgroundSize='cover';
    media.style.backgroundPosition='center';
    media.querySelector('svg.big-icon') && (media.querySelector('svg')?media.querySelector('svg').style.display='none':0);
    showToast('Imagem carregada na pré-visualização.');
  };
  reader.readAsDataURL(file);
});
document.getElementById('uploadZone').addEventListener('dragover',function(e){e.preventDefault(); this.style.borderColor='var(--gold)';});
document.getElementById('uploadZone').addEventListener('dragleave',function(){this.style.borderColor='';});

/* ============ EXPORT BUTTONS ============ */
document.getElementById('exportSchedBtn').addEventListener('click',function(){showToast('Exportação CSV iniciada.');});
document.getElementById('exportClientsBtn').addEventListener('click',function(){showToast('Exportação de clientes iniciada.');});

/* ============ LOGOUT ============ */
document.querySelector('.sb-logout').addEventListener('click',function(){showToast('Demonstração: logout não conectado a um sistema real.');});

updatePreview();

})();