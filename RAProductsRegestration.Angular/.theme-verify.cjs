const fs = require('fs');
const postcss = require('./RAProductsRegestration/node_modules/postcss');
const esbuild = require('./RAProductsRegestration/node_modules/esbuild');
const basicFile='RAProductsRegestration/src/app/features/product-basic-data/pages/product-basic-data/product-basic-data.component.css';
const basic=fs.readFileSync(basicFile,'utf8');
const marker='/* Regulatory deadlines and version history. */';
const start=basic.indexOf(marker);
const end=basic.indexOf('@media',start);
if(start<0||end<0) throw new Error('Expected basic deadline CSS block missing');
const block=postcss.parse(basic.slice(start,end));
block.walkRules(rule=>rule.selector=rule.selectors.map(selector=>'.product-content :where(app-product-basic-data) '+selector).join(', '));
fs.appendFileSync('RAProductsRegestration/src/app/shared-workspace.css','\n'+block.toString());
const files=[
 'ProductTechnicalDocs/Component/product-technical-docs',
 'ProductLicenses/component/product-license',
 'variations/component/product-variations',
 'appeals/product-appeals/product-appeals',
 'promotionalmaterial/component/product-promotional-materials',
 'Miscellaneousapproval/component/product-miscellaneous-approvals'
];
import('./RAProductsRegestration/node_modules/@angular/compiler/fesm2022/compiler.mjs').then(({parseTemplate})=>{
 for(const file of files){
   const base='RAProductsRegestration/src/app/features/'+file+'.component';
   const result=parseTemplate(fs.readFileSync(base+'.html','utf8'),base+'.html');
   if(result.errors?.length) { console.error(result.errors);process.exitCode=1; }
   else console.log(file+': template OK');
   const css=fs.readFileSync(base+'.css','utf8');
   postcss.parse(css);
   console.log('CSS bytes:',esbuild.transformSync(css,{loader:'css',minify:true}).code.length);
 }
 const shared=fs.readFileSync('RAProductsRegestration/src/app/shared-workspace.css','utf8');
 postcss.parse(shared);
 console.log('Shared CSS parse OK; minified bytes:',esbuild.transformSync(shared,{loader:'css',minify:true}).code.length);
});
