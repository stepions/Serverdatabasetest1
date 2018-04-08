
var express = require('express');
var app = express();
var mysql = require('mysql');
var util = require('util');

var connection = mysql.createConnection({
    host: 'dbgametest.co4g6kmh1qpb.us-east-2.rds.amazonaws.com',
    user: 'root',
    password: '032341282Ta',
    database: 'DBGame1'
    
});

connection.connect(function (err) {
    if (err) {
        console.log('Error Connecting', err.stack);
        return;
    }
    console.log('Connected as id', connection.threadId);

});

app.get('/user/add/user', function (req, res) {
    
    var name = req.query.name;
    var password = req.query.pass;

    var user = [[name,password,0]];

    

    /*
   var user = [
        ['Aa','456789'],
        ['Bb','456789'],
        ['Cc','456789']
   ];
   */

    InsertUser(user,function(err,result){
        res.end(result)
    }); 
});



app.get('/users', function (req, res) {
    //res.end('hello');

    queryAllUser(function(err,result){
        res.end(result);
        
    }); 
});

app.get('/user/:name', function (req, res) {

    var name = req.params.name;
    console.log(name);



    
    queryUser(function(err,result){
        res.end(result);
    }, name);
    
});


var server = app.listen(8081, function () {
    console.log('Server: Running');
});

function queryAllUser(callback) {

    var json = '';
    connection.query('SELECT * FROM user',
        function (err, rows, fields) {
            if (err) throw err;

            json = JSON.stringify(rows);

            callback(null, json);
        });
}

function queryUser(callback, name) {

    var json = '';
    connection.query("SELECT * FROM user WHERE name =?",name,
        function (err, rows, fields) {
            if (err) throw err;

            json = JSON.stringify(rows);

            callback(null, json);
        });
}

function InsertUser(user,callback) {

    var sql = 'INSERT INTO user(name,password,score) values ?';

    connection.query(sql,[user],
        function (err) {

            var result = '[{"success":"true"}]'

            if (err){
                result = '[{"success":"false"}]'
                throw err;

            }

            callback(null, result);
        });
}

function UpdateScore(score, name, callback){
    var sql = util.format('UPDATE user SET score = %s WHERE name = "%s" AND score < %s', score, name, score);

    connection.query(sql,
        function (err) {

            var result = '[{"success":"true"}]'

            if (err){
                result = '[{"success":"false"}]'
                throw err;

            }

            callback(null, result);
        });
}

function queryTopTen(callback){
    var json = '';
    connection.query("SELECT name, score FROM user ORDER BY score DESC LIMIT 10;",
        function (err, rows, fields) {
            if (err) throw err;

            json = JSON.stringify(rows);

            callback(null, json);
        });
}