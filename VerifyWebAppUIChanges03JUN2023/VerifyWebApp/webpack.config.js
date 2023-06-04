const path = require('path');
module.exports = {
    mode: 'development',
    entry: ['./Scripts/app/app.js'],
    output: {
        path: path.resolve(__dirname, './Scripts/build'),
        filename: 'bundle.js'
    },
    // IMPORTANT NOTE: If you are using Webpack 2 or above, replace "loaders" with "rules"
}