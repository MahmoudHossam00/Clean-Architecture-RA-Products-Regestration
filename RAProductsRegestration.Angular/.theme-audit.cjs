const fs = require('fs');
const path = require('path');
const postcss = require('./RAProductsRegestration/node_modules/postcss');
const esbuild = require('./RAProductsRegestration/node_modules/esbuild');
const targets = [
 ['ProductTechnicalDocs/Component/product-technical-docs', 'app-product-technical-docs'],
 ['ProductLicenses/component/product-license', 'app-product-license'],
 ['variations/component/product-variations', 'app-product-variations'],
 ['appeals/product-appeals/product-appeals', 'app-product-appeals'],
 ['promotionalmaterial/component/product-promotional-materials', 'app-product-promotional-materials'],
 ['Miscellaneousapproval/component/product-miscellaneous-approvals', 'app-product-miscellaneous-approvals']
];
const entries = targets.map(([name, host]) => ({name, host, file: path.join('RAProductsRegestration/src/app/features', name + '.component.css')}));
const normalize = str => str.replace(/\s+/g, ' ').trim();
const keys = new Map();
const colors = {
 '#0f172a':'var(--text-primary)', '#172033':'var(--text-primary)', '#334155':'var(--text-primary)',
 '#475569':'var(--text-secondary)', '#64748b':'var(--text-secondary)', '#94a3b8':'#687b8f',
 '#2563eb':'var(--accent)', '#1d4ed8':'var(--accent-hover)', '#3b82f6':'var(--accent)', '#1e40af':'var(--accent-hover)',
 '#eff6ff':'#edf8f8', '#dbeafe':'#dbf0f0', '#bfdbfe':'#bddfe2', '#f8fbff':'#f4fafb', '#60a5fa':'var(--accent)',
 '#e2e8f0':'var(--border)', '#e6eaf0':'var(--border)', '#cbd5e1':'var(--border-dark)',
 '#f8fafc':'var(--surface-secondary)', '#f1f5f9':'#edf2f6', '#ffffff':'var(--surface)'
};
const recolor = value => value.replace(/#[a-f0-9]{6}\b/gi, color => colors[color.toLowerCase()] || color);
for (const entry of entries) {
 entry.root = postcss.parse(fs.readFileSync(entry.file, 'utf8'));
 entry.root.walkComments(node => node.remove());
 entry.root.walkDecls(node => {
   node.value = recolor(normalize(node.value));
   if (node.prop === 'font-size' && /^(9|10|11|12)px$/.test(node.value)) node.value = '13px';
 });
 const groupedRules = [];
 entry.root.walkRules(node => { if (node.selectors.length > 1) groupedRules.push(node); });
 for (const node of groupedRules) { for (const selector of node.selectors) node.parent.insertBefore(node, node.clone({selector})); node.remove(); }
 entry.root.walkRules(node => {
   node.selector = normalize(node.selector);
   const ancestors = [];
   let parent = node.parent;
   while (parent.type !== 'root') { ancestors.unshift('@' + parent.name + ' ' + normalize(parent.params)); parent = parent.parent; }
   const key = JSON.stringify([ancestors, node.selector]);
   node.sharedKey = key;
   if (!keys.has(key)) keys.set(key, []);
   keys.get(key).push({entry,node});
 });
}
const shared = postcss.root();
const common = new Set();
const append = (destination, rule, ancestors) => {
 let target = destination;
 for (const ancestor of ancestors) {
   const parsed = ancestor.match(/^@(\S+)\s?(.*)$/);
   const wrapper = postcss.atRule({name:parsed[1], params:parsed[2]});
   target.append(wrapper);
   target = wrapper;
 }
 target.append(rule);
};
for (const [key, matches] of keys) {
 const [ancestors,selector] = JSON.parse(key);
 if (ancestors.some(a => a.startsWith('@keyframes'))) continue;
 if (new Set(matches.map(x => x.entry.host)).size < 2) continue;
 common.add(key);
 const frequencies = new Map();
 for (const {node} of matches) {
   const signature = node.nodes.map(n => n.toString()).join(';');
   frequencies.set(signature, (frequencies.get(signature) || 0) + 1);
 }
 const selected = matches.reduce((best,item) => frequencies.get(item.node.nodes.map(n=>n.toString()).join(';')) > frequencies.get(best.node.nodes.map(n=>n.toString()).join(';')) ? item : best);
 const rule = selected.node.clone();
 rule.selector = selector === ':host' ? '.product-content > *' : '.product-content ' + selector;
 append(shared,rule,ancestors);
}
const prefixes = [ /\.(technical-|current-document|document-section|history-)/, /\.(license-|renewal-)/, /\.(variations-|variation-)/, /\.(appeals-|appeal-)/, /\.(promotional-|materials-|material-)/, /\.(misc-|approvals-)/ ];
entries.forEach((entry,index) => {
 const unique = postcss.root();
 entry.root.walkRules(node => {
   const [ancestors,selector] = JSON.parse(node.sharedKey);
   if (common.has(node.sharedKey) || ancestors.some(a=>a.startsWith('@keyframes'))) return;
   const rule = node.clone();
   if (prefixes[index].test(selector)) append(unique,rule,ancestors);
   else {
     rule.selector = '.product-content :where(' + entry.host + ') ' + selector;
     append(shared,rule,ancestors);
   }
 });
 const clean = format(unique);
 console.log(entry.name, 'component minified', esbuild.transformSync(clean, {loader:'css',minify:true}).code.length);
 fs.writeFileSync(entry.file, '/* Layout specific to this registration section. Shared patterns live in app/shared-workspace.css. */\n' + clean);
});
const keyframes = new Set();
for (const entry of entries) entry.root.walkAtRules('keyframes',node=>{
 if (!keyframes.has(node.params)) { shared.append(node.clone()); keyframes.add(node.params); }
});
function format(root) {
 root.walkComments(node=>node.remove());
 root.walk(node=>{node.raws={};});
 root.walkDecls(node=>{ node.raws.before=' ';node.raws.between=': '; });
 root.walkRules(node=>{node.raws.before='\n';node.raws.after=' ';node.raws.semicolon=true;});
 root.walkAtRules(node=>{node.raws.before='\n';node.raws.after='\n';});
 return root.toString().trim()+'\n';
}
fs.writeFileSync('RAProductsRegestration/src/app/shared-workspace.css', '/* Shared regulatory workspace components. Feature-specific exceptions remain scoped to their host. */\n' + format(shared));
console.log('Shared minified',esbuild.transformSync(shared.toString(),{loader:'css',minify:true}).code.length);
