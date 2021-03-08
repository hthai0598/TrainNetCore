const express = require('express');
// const favicon = require('express-favicon');
// const compression = require('compression');
const path = require('path');
const port = process.env.PORT || 8080;
const app = express();

const outputPath = path.resolve(process.cwd(), 'build');
let root = path.join(__dirname, 'build');
app.use(function(req, res, next) {
    res.header("Access-Control-Allow-Origin", "*"); // update to match the domain you will make the request from
    res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    next();
  });
// app.use(favicon(__dirname + '/build/favicon.ico'));
// app.use(compression());
// the __dirname is the current directory from where the script is running
// app.use(express.static(__dirname));
app.use(express.static(root));
// app.use('/', express.static(outputPath));
// app.get('/ping', function (req, res) {
//  return res.send('pong');
// });
app.get('/*', function (req, res) {
  // if (req.method === 'GET' && req.accepts('html') && !req.is('json') && !req.path.includes('.')) {
  //   res.sendFile('index.html', { root })
  // } else next()
  res.sendFile(path.join(__dirname, 'build', 'index.html'));
  // res.sendFile('index.html', out)
});
app.listen(port);