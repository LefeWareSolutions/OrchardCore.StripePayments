const path = require('path');

module.exports = {
    entry: './assets/stripe-payment.js',
    output: {
        filename: 'stripe-payment.js',
        path: path.resolve(__dirname, '../wwwroot/Scripts'),
    },
    module: {
        rules: [
            {
                test: /\.css$/i,
                use: ['style-loader', 'css-loader'],
            },
        ]
    },
    mode: 'development',
    watch: true
}
