

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();





connection.on("ReceiveMessage", function (user, message) {
    document.getElementById(user).textContent = message.slice(0 , 40);
    var Location = (window.location.href).split("#")
    if (Location[Location.length - 1] == user) {
        

        $("#Messages").append("<li class='receiver'> <div class= 'receiverBox'>" + message +  "</div > </li>");
        

        document.getElementsByClassName("Chat")[0].scrollTo(0, document.getElementsByClassName("Chat")[0].scrollHeight)

        
          


       
    }
   
});


connection.start().then().catch(function (err) {
    return console.error(err.toString());
});




document.getElementById("sendButton").addEventListener("click", function (event) {

    var sender = document.getElementById("senderInput").value;
    var message = document.getElementById("MessageTextArea").value;
    var receiver = document.getElementById("receiverInput").value;
    
    document.getElementById("MessageTextArea").value = "";
    
    document.getElementById(receiver).textContent = message.slice(0 , 40)

    
 
    

    $("#Messages").append("<li class='sender'> <div class= 'senderBox'>" + message + "</div > </li>");
    document.getElementsByClassName("Chat")[0].scrollTo(0, document.getElementsByClassName("Chat")[0].scrollHeight);
    


  


       
        

        

    
    
    

    

    connection.invoke("SendMessageToGroup", sender, receiver, message).catch(function (err) {
        console.log(sender)
        console.log(receiver)

            return console.error(err.toString());
        });
    
   
    event.preventDefault();
});
    
