Ext.Loader.setConfig({
    disableCaching: false
});
Ext.application({
    name: 'BarsProject',
    appFolder: 'BarsProject/app',
    controllers: ['Humans'],
    //requires: ['BarsProject.view.HumanTable'],
    launch: function() {
        Ext.Date.defaultFormat='d.m.Y';
        var alertButton = Ext.create('Ext.Button', {
            margin:'10 0 0 30',
            renderTo: Ext.getBody(),
            text: 'Начать работу',
            handler: function(){
                Ext.create('BarsProject.view.SearchWindow');
            },
            scope:this
        });
        // Ext.Ajax.request({
        //     url: '/home/SearchHuman',
        //     params: {surname: 'Nasyrov'},
        //     success: function(response){
        //         alert(response.responseText);
        //     }
        // });

    }
});