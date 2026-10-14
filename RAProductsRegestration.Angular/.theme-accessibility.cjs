const fs = require('fs');
const postcss = require('./RAProductsRegestration/node_modules/postcss');
const files = [
 'ProductTechnicalDocs/Component/product-technical-docs',
 'ProductLicenses/component/product-license',
 'variations/component/product-variations',
 'appeals/product-appeals/product-appeals',
 'promotionalmaterial/component/product-promotional-materials',
 'Miscellaneousapproval/component/product-miscellaneous-approvals'
];
const pageLabels = ['Technical documents', 'Product license', 'Product variations', 'Product appeals', 'Promotional material', 'Miscellaneous approvals'];
files.forEach((file,index)=>{
 const path = 'RAProductsRegestration/src/app/features/' + file + '.component.html';
 let html = fs.readFileSync(path,'utf8');
 html = html.replace(/<section class="([^"]+-page)">/, '<section class="$1" [attr.aria-busy]="loading" aria-label="'+pageLabels[index]+'">');
 html = html.replace(/class="message error-message"/g, 'class="message error-message" role="alert"');
 html = html.replace(/class="message success-message"/g, 'class="message success-message" role="status"');
 html = html.replace(/class="card loading-card"/g, 'class="card loading-card" role="status"');
 html = html.replace(/class="loading-card card"/g, 'class="loading-card card" role="status"');
 html = html.replace(/class="modal-close"/g, 'class="modal-close" aria-label="Close dialog"');
 html = html.replace(/class="(pdf-close-button|pdf-close)"/g, 'class="$1" aria-label="Close document preview"');
 // Toggle semantics announce selection without implying an unimplemented arrow-key tab interaction.
 html = html.replace(/(<button\b(?:(?!<\/button>)[\s\S])*?)\[class\.active\]="([^"]+)"/g, (all,before,expression) => before+'[attr.aria-pressed]="'+expression+'"\n          [class.active]="'+expression+'"');
 fs.writeFileSync(path,html);
});
// Shared ingredient editor primitives are also used by Basic Data.
const basic = 'RAProductsRegestration/src/app/features/product-basic-data/pages/product-basic-data/product-basic-data.component.css';
const root = postcss.parse(fs.readFileSync(basic,'utf8'));
const output = postcss.root();
const classes = /\.(ingredient-(?:list|summary-card|summary-header|name|avatar|details|modal-tabs)|detail-item|modal-(?:pack-row|supplier-row|section)|(?:ai|supplier)-search-(?:results|result)|selected-master-record)/;
root.walkRules(rule=>{
 if (!classes.test(rule.selector)) return;
 const clone = rule.clone();
 clone.selector = clone.selectors.map(selector=>'.product-content '+selector).join(', ');
 clone.walkDecls(decl=>{
   decl.value=decl.value.replace(/\s+/g,' ').trim()
    .replace(/var\(--pbd-text-primary\)/g,'var(--text-primary)')
    .replace(/var\(--pbd-text-secondary\)/g,'var(--text-secondary)')
    .replace(/var\(--pbd-[^)]+\)/g,'var(--accent)')
    .replace(/#(?:2563eb|1d4ed8|3b82f6)/gi,'var(--accent)')
    .replace(/#(?:eff6ff|dbeafe)/gi,'#eaf6f6')
    .replace(/#(?:0f172a|334155)/gi,'var(--text-primary)')
    .replace(/#(?:64748b|94a3b8)/gi,'var(--text-secondary)')
    .replace(/#(?:e2e8f0|e8edf3)/gi,'var(--border)');
   if(decl.prop==='font-size' && /^(9|10|11|12)px$/.test(decl.value)) decl.value='13px';
 });
 let target=output;
 const parents=[];
 for(let parent=rule.parent;parent.type!=='root';parent=parent.parent) parents.unshift(parent);
 for(const parent of parents) {const at=postcss.atRule({name:parent.name,params:parent.params});target.append(at);target=at;}
 target.append(clone);
});
output.walk(node=>node.raws={});
output.walkDecls(node=>{node.raws.before=' ';node.raws.between=': ';});
output.walkRules(node=>{node.raws.before='\n';node.raws.after=' ';node.raws.semicolon=true;});
output.walkAtRules(node=>{node.raws.before='\n';node.raws.after='\n';});
fs.appendFileSync('RAProductsRegestration/src/app/shared-workspace.css','\n/* Ingredient editor and master-record lookup layout. */\n'+output.toString()+'\n');
console.log('Updated six templates; extracted ingredient layout rules:',output.nodes.length);
