# ChatPrototype

This project contains the source code of the "Project Chat" of the blog.
The first article can be found with the following link:

[https://welovernd.com/2017/03/17/wir-schreiben-einen-chat-teil-i/](https://welovernd.com/2017/03/17/wir-schreiben-einen-chat-teil-i/)

This project was created with Visual Studio 2013.

## Project Structure

Inside of this solution are two different projects. The one is the server, the other one is the client.

## How to run the application

To run the application you have to do the following steps:

- Open this project with Visual Studio
- Compile the project
- Start the Server (~\Server\bin\Debug\Server.exe)
    - You cannot start multiple Servers, because the Port is fixed to 4000
- Start the client (~\Client\bin\Debug\Client.exe)
    - You can start multiple clients to test it locally
- Connect the Client with the server
	- In the source, the Port is fixed to 4000 (both, in Client and Server)
	- If you are running the Appliations on your local computer, use "localhost" or "127.0.0.1" to connect to the server
- Login with "/login NAME"
- Use /help if you don't know all the commands
- Have fun chatting :)

## Final words

This project is just a prototype to show how a chat-application COULD be made.
There are many different ways for improvements or new functions. If you like, you can fork the project and add some new features.
