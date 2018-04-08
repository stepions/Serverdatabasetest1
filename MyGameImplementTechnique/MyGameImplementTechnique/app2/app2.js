var mysql = require('mysql');
var connection = mysql.createConnection({
    host: 'localhost',
    user: 'root',
    password: 'Cgm123456*',
    database: 'game1'
});

connection.connect(function (err) {
    if (err) {
        console.log('Error Connecting', err.stack);
        return;
    }
    console.log('Connected as id', connection.threadId);

});

/*
connection.query('SELECT * FROM user',
    function (err, rows, fields) {
        if(err) throw err;

        for(var i in rows){
            console.log('user : ',rows[i].name);
        }
    });
    */

var values = { name: 'Jj', password: '789456', score: 789 };
connection.query('INSERT INTO user SET ?',
    values, function (err, result) {
        if(err) throw err;

        console.log(result);
    });


connection.end(function (err) {
    console.log('Terminated Connection');
})

console.log('App2 test mysql : running');



