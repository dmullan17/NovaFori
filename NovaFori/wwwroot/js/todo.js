const uri = 'api/todoitems';

var TodoViewModel = function () {

    var self = this;

    //Declare observable which will be bind with UI  
    self.Description = ko.observable("");
    self.IsComplete = ko.observable(false);

    //The Object which stored data entered in the observables  
    var TodoData = {
        Description: self.Description,
        IsComplete: self.IsComplete
    };

    //Declare 2 ObservableArrays for storing both types of todos 
    self.PendingTodos = ko.observableArray([]);
    self.CompletedTodos = ko.observableArray([]);

    //Call the Function which gets all todos using ajax call  
    GetTodoList();

    self.save = function () {
        //Ajax call to add a to-do
        $.ajax({
            type: "POST",
            url: uri,
            data: ko.toJSON(TodoData),
            contentType: "application/json",
            success: function (data) {
                console.log("Added");
                GetTodoList();
            },
            error: function (error) {
                console.log(error.status + "<!----!>" + error.statusText);
            }
        });
    };

    self.update = function (item) {
        var url = `${uri}/${item.id}`;
        $.ajax({
            type: "PUT",
            url: url,
            data: ko.toJSON(item),
            contentType: "application/json",
            success: function (data) {
                console.log("Updated");
                GetTodoList();
            },
            error: function (error) {
                console.log(error.status + "<!----!>" + error.statusText);
            }
        });
    };

    function GetTodoList() {
        $.ajax({
            type: "GET",
            url: uri,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                self.PendingTodos(data.filter(x => !x.isComplete));
                self.CompletedTodos(data.filter(x => x.isComplete));
            },
            error: function (error) {
                console.log(error.status + "<--and--> " + error.statusText);
            }
        });
    }
};
ko.applyBindings(new TodoViewModel());
